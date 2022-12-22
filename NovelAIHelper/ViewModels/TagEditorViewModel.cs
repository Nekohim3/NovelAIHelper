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
        _wnd       = wnd;
        _editedTag = new UI_Tag();
        g.TagTree.LoadTree();
        g.TagTree.RootDirs.SelectionChanged += DirsTreeOnSelectionChanged;
        DownloadFromDanbooruCmd             =  ReactiveCommand.Create(OnDownloadFromDanbooru);
        ResetDatabaseCmd                    =  ReactiveCommand.Create(OnResetDatabase);
        LoadTreeCmd                         =  ReactiveCommand.Create(OnLoadTree);

        AddRootDirCmd   = ReactiveCommand.Create(OnAddRootDir);
        AddChildDirCmd  = ReactiveCommand.Create(OnAddChildDir,  Observable.Return(g.TagTree.RootDirs.SelectedItem != null));
        EditDirCmd      = ReactiveCommand.Create(OnEditDir,      Observable.Return(g.TagTree.RootDirs.SelectedItem != null));
        RemoveDirCmd    = ReactiveCommand.Create(OnRemoveDir,    Observable.Return(g.TagTree.RootDirs.SelectedItem != null));
        MoveDirCmd      = ReactiveCommand.Create(OnMoveDir,      Observable.Return(g.TagTree.RootDirs.SelectedItem != null));
        AddTagCmd       = ReactiveCommand.Create(OnAddTag,       Observable.Return(g.TagTree.RootDirs.SelectedItem != null));
        EditTagCmd      = ReactiveCommand.Create(OnEditTag,      Observable.Return(g.TagTree.RootDirs.SelectedItem != null && g.TagTree.Tags.SelectedItem != null));
        RemoveTagCmd    = ReactiveCommand.Create(OnRemoveTag,    Observable.Return(g.TagTree.RootDirs.SelectedItem != null && g.TagTree.Tags.SelectedItem != null));
        AssignNewDirCmd = ReactiveCommand.Create(OnAssignNewDir, Observable.Return(g.TagTree.RootDirs.SelectedItem != null && g.TagTree.Tags.SelectedItem != null));
        MoveTagCmd      = ReactiveCommand.Create(OnMoveTag,      Observable.Return(g.TagTree.RootDirs.SelectedItem != null && g.TagTree.Tags.SelectedItem != null));
        CancelTagCmd    = ReactiveCommand.Create(OnCancelTag);
        SaveDirCmd      = ReactiveCommand.Create(OnSaveDir);
        CancelDirCmd    = ReactiveCommand.Create(OnCancelDir);
        SaveTagCmd      = ReactiveCommand.Create(OnSaveTag);
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
        EditedDir      = new UI_Dir("", g.TagTree.RootDirs.SelectedItem.Id);
        DirEditVisible = true;
    }

    private async void OnEditDir()
    {
        EditedDir      = new UI_Dir();
        EditedDir.Id   = g.TagTree.RootDirs.SelectedItem.Id;
        EditedDir.Name = g.TagTree.RootDirs.SelectedItem.Name;
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
        var res = await MessageBoxManager.GetMessageBoxStandardWindow("", $"Remove dir: \"{g.TagTree.RootDirs.SelectedItem.Name}\"?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd);
        if (res == ButtonResult.Yes)
        {
            //TagTree.RootDirs.SelectedItem.Remove();
            if (g.TagTree.RootDirs.SelectedItem.UI_Parent == null)
            {
                g.TagTree.RootDirs.Remove(g.TagTree.RootDirs.SelectedItem);
            }
            else
            {
                g.TagTree.RootDirs.SelectedItem.UI_Parent.ChildDirs.Remove(g.TagTree.RootDirs.SelectedItem);
                g.TagTree.RootDirs.SelectedItem.ParentDir.ChildDirs.Remove(g.TagTree.RootDirs.SelectedItem);
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
        EditedTag.Id   = g.TagTree.Tags.SelectedItem.Id;
        EditedTag.Name = g.TagTree.Tags.SelectedItem.Name;
        TagEditVisible = true;
    }

    private async void OnRemoveTag()
    {
        EditedTag      = null;
        TagEditVisible = false;
        var res = await MessageBoxManager.GetMessageBoxStandardWindow("", $"Remove tag: \"{g.TagTree.Tags.SelectedItem.Name}\"?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd);
        if (res == ButtonResult.Yes)
            //TagTree.Tags.SelectedItem.Remove();
            LoadTree(true);
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
            g.TagTree.RootDirs.SelectedItem.Tags.Add(EditedTag);
        //TagTree.RootDirs.SelectedItem.Save();
        //EditedTag.Dirs.Add(TagTree.RootDirs.SelectedItem);
        //EditedTag.Add();
        else
            g.TagTree.Tags.SelectedItem.Name = EditedTag.Name;
        //TagTree.Tags.SelectedItem.Save();

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
        g.TagTree.LoadTree(remember);
    }

    public void LoadTags()
    {
        if (g.TagTree.RootDirs.SelectedItem == null) return;
        g.TagTree.Tags.Clear();
        if (ShowInnerTags)
            LoadTags(g.TagTree.RootDirs.SelectedItem);
        else
            g.TagTree.Tags.AddRange(g.TagTree.RootDirs.SelectedItem.UI_Tags);
    }

    private void LoadTags(UI_Dir dir)
    {
        g.TagTree.Tags.AddRange(dir.UI_Tags);
        foreach (var x in dir.UI_Childs) LoadTags(x);
    }

    #endregion


    #region Callbacks

    private void DirsTreeOnSelectionChanged(ObservableCollectionWithSelectedItem<UI_Dir> sender, UI_Dir newselection, UI_Dir oldselection)
    {
        if (g.TagTree.RootDirs.SelectedItem != null)
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
