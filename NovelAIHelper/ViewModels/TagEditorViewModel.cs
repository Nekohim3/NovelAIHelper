using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Avalonia.Controls;
using Avalonia.Threading;
using AvaloniaEdit.Utils;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using NovelAIHelper.DataBase;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils.Collections;
using NovelAIHelper.Utils.TagDownloader;
using ReactiveUI;

namespace NovelAIHelper.ViewModels;

internal class TagEditorViewModel : ViewModelBase
{
    private Window _wnd;

    #region Commands

    public ReactiveCommand<Unit, Unit> DownloadFromDanbooruCmd { get; }
    public ReactiveCommand<Unit, Unit> ResetDatabaseCmd        { get; }
    public ReactiveCommand<Unit, Unit> LoadTreeCmd             { get; }

    public ReactiveCommand<Unit, Unit> AddRootDirCmd   { get; }
    public ReactiveCommand<Unit, Unit> AddChildDirCmd  { get; }
    public ReactiveCommand<Unit, Unit> EditDirCmd      { get; }
    public ReactiveCommand<Unit, Unit> RemoveDirCmd    { get; }
    public ReactiveCommand<Unit, Unit> MoveDirCmd      { get; }
    public ReactiveCommand<Unit, Unit> AddTagCmd       { get; }
    public ReactiveCommand<Unit, Unit> EditTagCmd      { get; }
    public ReactiveCommand<Unit, Unit> AssignNewDirCmd { get; }

    public ReactiveCommand<Unit, Unit> RemoveTagCmd { get; }
    public ReactiveCommand<Unit, Unit> MoveTagCmd   { get; }

    public ReactiveCommand<Unit, Unit> SaveDirCmd   { get; }
    public ReactiveCommand<Unit, Unit> CancelDirCmd { get; }
    public ReactiveCommand<Unit, Unit> SaveTagCmd   { get; }
    public ReactiveCommand<Unit, Unit> CancelTagCmd { get; }

    #endregion

    #region Properties

    private TagTree _tagTree = new();

    public TagTree TagTree
    {
        get => _tagTree;
        set => this.RaiseAndSetIfChanged(ref _tagTree, value);
    }

    private bool _showInnerTags;

    public bool ShowInnerTags
    {
        get => _showInnerTags;
        set
        {
            this.RaiseAndSetIfChanged(ref _showInnerTags, value);
            LoadTags();
        }
    }

    private bool _dirEditVisible;

    public bool DirEditVisible
    {
        get => _dirEditVisible;
        set => this.RaiseAndSetIfChanged(ref _dirEditVisible, value);
    }

    private bool _tagEditVisible;

    public bool TagEditVisible
    {
        get => _tagEditVisible;
        set => this.RaiseAndSetIfChanged(ref _tagEditVisible, value);
    }

    private UI_Dir? _editedDir;

    public UI_Dir? EditedDir
    {
        get => _editedDir;
        set => this.RaiseAndSetIfChanged(ref _editedDir, value);
    }

    private UI_Tag _editedTag;

    public UI_Tag EditedTag
    {
        get => _editedTag;
        set => this.RaiseAndSetIfChanged(ref _editedTag, value);
    }

    #endregion

    #region Ctor

    public TagEditorViewModel()
    {
    }

    public TagEditorViewModel(Window wnd)
    {
        _wnd                              =  wnd;
        _editedTag                        =  new UI_Tag();
        TagTree.RootDirs.SelectionChanged += DirsTreeOnSelectionChanged;
        DownloadFromDanbooruCmd           =  ReactiveCommand.Create(OnDownloadFromDanbooru);
        ResetDatabaseCmd                  =  ReactiveCommand.Create(OnResetDatabase);
        LoadTreeCmd                       =  ReactiveCommand.Create(OnLoadTree);

        AddRootDirCmd  = ReactiveCommand.Create(OnAddRootDir);
        AddChildDirCmd = ReactiveCommand.Create(OnAddChildDir, this.WhenAnyValue(x => x.TagTree.RootDirs.SelectedItem).Select(x => x != null));
        EditDirCmd     = ReactiveCommand.Create(OnEditDir,     this.WhenAnyValue(x => x.TagTree.RootDirs.SelectedItem).Select(x => x != null));
        RemoveDirCmd   = ReactiveCommand.Create(OnRemoveDir,   this.WhenAnyValue(x => x.TagTree.RootDirs.SelectedItem).Select(x => x != null));
        MoveDirCmd     = ReactiveCommand.Create(OnMoveDir,     this.WhenAnyValue(x => x.TagTree.RootDirs.SelectedItem).Select(x => x != null));
        AddTagCmd      = ReactiveCommand.Create(OnAddTag,      this.WhenAnyValue(x => x.TagTree.RootDirs.SelectedItem).Select(x => x != null));
        EditTagCmd = ReactiveCommand.Create(OnEditTag,
                                            this.WhenAnyValue(x => x.TagTree.RootDirs.SelectedItem, x => x.TagTree.Tags.SelectedItem,
                                                              (dir, tag) => dir != null && tag != null));
        RemoveTagCmd = ReactiveCommand.Create(OnRemoveTag,
                                              this.WhenAnyValue(x => x.TagTree.RootDirs.SelectedItem, x => x.TagTree.Tags.SelectedItem,
                                                                (dir, tag) => dir != null && tag != null));
        AssignNewDirCmd = ReactiveCommand.Create(OnAssignNewDir,
                                                 this.WhenAnyValue(x => x.TagTree.RootDirs.SelectedItem, x => x.TagTree.Tags.SelectedItem,
                                                                   (dir, tag) => dir != null && tag != null));
        MoveTagCmd = ReactiveCommand.Create(OnMoveTag,
                                            this.WhenAnyValue(x => x.TagTree.RootDirs.SelectedItem, x => x.TagTree.Tags.SelectedItem,
                                                              (dir, tag) => dir != null && tag != null));
        CancelTagCmd = ReactiveCommand.Create(OnCancelTag);
        SaveDirCmd   = ReactiveCommand.Create(OnSaveDir);
        CancelDirCmd = ReactiveCommand.Create(OnCancelDir);
        SaveTagCmd   = ReactiveCommand.Create(OnSaveTag);
    }

    #endregion

    #region CmdExec

    private async void OnLoadTree()
    {
        var res = await MessageBoxManager.GetMessageBoxStandardWindow("", "Reload tree from database?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd);
        if (res == ButtonResult.Yes) LoadTree(true);
    }

    private async void OnResetDatabase()
    {
        if (await MessageBoxManager.GetMessageBoxStandardWindow("", "Are you sure about that?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd) == ButtonResult.Yes)
        {
            g.Ctx = new TagContext(true);
            LoadTree();
        }
    }

    private async void OnDownloadFromDanbooru()
    {
        var res = await MessageBoxManager.GetMessageBoxStandardWindow("", "Reset database?", ButtonEnum.YesNoCancel, Icon.Question).ShowDialog(_wnd);
        if (res == ButtonResult.Yes)
            g.Ctx = new TagContext(true);
        else if (res == ButtonResult.Cancel) return;
        var loader = new DanbooruLoader();
        loader.DownloadAll();
        LoadTree();
    }

    private async void OnAddRootDir()
    {
        EditedDir      = new UI_Dir("");
        DirEditVisible = true;
    }

    private async void OnAddChildDir()
    {
        EditedDir      = new UI_Dir("", TagTree.RootDirs.SelectedItem.Id);
        DirEditVisible = true;
    }

    private async void OnEditDir()
    {
        EditedDir      = new UI_Dir();
        EditedDir.Id   = TagTree.RootDirs.SelectedItem.Id;
        EditedDir.Name = TagTree.RootDirs.SelectedItem.Name;
        DirEditVisible = true;
    }

    private async void OnSaveDir()
    {
        //if (EditedDir.Id == 0)
        //{
        //    EditedDir.Add();
        //    if (EditedDir.ParentId.HasValue)
        //    {
        //        TagTree.RootDirs.SelectedItem.ChildDirs.Add(EditedDir);
        //        LoadTree(true);
        //    }
        //    else
        //    {
        //        TagTree.RootDirs.Add(EditedDir);
        //    }
        //}
        //else
        //{
        //    TagTree.RootDirs.SelectedItem.Name = EditedDir.Name;
        //    TagTree.RootDirs.SelectedItem.Save();
        //}

        DirEditVisible = false;
        EditedDir      = null;
    }

    private async void OnCancelDir()
    {
        DirEditVisible = false;
        EditedDir      = null;
    }

    private async void OnRemoveDir()
    {
        EditedDir      = null;
        DirEditVisible = false;
        var res = await MessageBoxManager.GetMessageBoxStandardWindow("", $"Remove dir: \"{TagTree.RootDirs.SelectedItem.Name}\"?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd);
        if (res == ButtonResult.Yes)
        {
            //TagTree.RootDirs.SelectedItem.Remove();
            if (TagTree.RootDirs.SelectedItem.UI_Parent == null)
            {
                TagTree.RootDirs.Remove(TagTree.RootDirs.SelectedItem);
            }
            else
            {
                TagTree.RootDirs.SelectedItem.UI_Parent.ChildDirs.Remove(TagTree.RootDirs.SelectedItem);
                TagTree.RootDirs.SelectedItem.ParentDir.ChildDirs.Remove(TagTree.RootDirs.SelectedItem);
                LoadTree(true);
            }
        }
    }

    private async void OnMoveDir()
    {
    }

    private async void OnAddTag()
    {
        EditedTag      = new UI_Tag("");
        TagEditVisible = true;
    }

    private async void OnEditTag()
    {
        EditedTag      = new UI_Tag();
        EditedTag.Id   = TagTree.Tags.SelectedItem.Id;
        EditedTag.Name = TagTree.Tags.SelectedItem.Name;
        TagEditVisible = true;
    }

    private async void OnRemoveTag()
    {
        EditedTag      = null;
        TagEditVisible = false;
        var res = await MessageBoxManager.GetMessageBoxStandardWindow("", $"Remove tag: \"{TagTree.Tags.SelectedItem.Name}\"?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd);
        if (res == ButtonResult.Yes)
        {
            //TagTree.Tags.SelectedItem.Remove();
            LoadTree(true);
        }
    }

    private void OnAssignNewDir()
    {
        
    }

    private async void OnMoveTag()
    {
    }

    private async void OnSaveTag()
    {
        if (EditedTag.Id == 0)
        {
            TagTree.RootDirs.SelectedItem.Tags.Add(EditedTag);
            //TagTree.RootDirs.SelectedItem.Save();
            //EditedTag.Dirs.Add(TagTree.RootDirs.SelectedItem);
            //EditedTag.Add();
        }
        else
        {
            TagTree.Tags.SelectedItem.Name = EditedTag.Name;
            //TagTree.Tags.SelectedItem.Save();
        }

        TagEditVisible = false;
        EditedTag      = null;
        LoadTree(true);
    }

    private async void OnCancelTag()
    {
        TagEditVisible = false;
        EditedTag      = null;
    }

    #endregion

    #region Funcs

    public void LoadTree(bool remember = false)
    {
        TagTree.LoadTree(remember);
    }

    public void LoadTags()
    {
        if (TagTree.RootDirs.SelectedItem == null) return;
        TagTree.Tags.Clear();
        if (ShowInnerTags)
            LoadTags(TagTree.RootDirs.SelectedItem);
        else
            TagTree.Tags.AddRange(TagTree.RootDirs.SelectedItem.UI_Tags);
    }

    private void LoadTags(UI_Dir dir)
    {
        TagTree.Tags.AddRange(dir.UI_Tags);
        foreach (var x in dir.UI_Childs) LoadTags(x);
    }

    #endregion


    #region Callbacks

    private void DirsTreeOnSelectionChanged(ObservableCollectionWithSelectedItem<UI_Dir> sender, UI_Dir newselection, UI_Dir oldselection)
    {
        if (TagTree.RootDirs.SelectedItem != null)
            LoadTags();
        OnCancelDir();
        OnCancelTag();
    }

    public void TagsOnSelectionChanged(UI_Tag newselection, UI_Tag oldselection)
    {
        OnCancelTag();
    }

    #endregion
}
