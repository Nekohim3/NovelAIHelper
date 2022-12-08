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
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using NovelAIHelper.DataBase;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils.Collections;
using NovelAIHelper.Utils.TagDownloader;
using ReactiveUI;

namespace NovelAIHelper.ViewModels
{
    internal class TagEditorViewModel : ViewModelBase
    {
        private Window _wnd;
        
        #region Commands

        public ReactiveCommand<Unit, Unit> DownloadFromDanbooruCmd { get; }
        public ReactiveCommand<Unit, Unit> ResetDatabaseCmd        { get; }
        public ReactiveCommand<Unit, Unit> LoadTreeCmd             { get; }

        public ReactiveCommand<Unit, Unit> AddRootDirCmd  { get; }
        public ReactiveCommand<Unit, Unit> AddChildDirCmd { get; }
        public ReactiveCommand<Unit, Unit> EditDirCmd     { get; }
        public ReactiveCommand<Unit, Unit> RemoveDirCmd   { get; }
        public ReactiveCommand<Unit, Unit> MoveDirCmd     { get; }
        public ReactiveCommand<Unit, Unit> AddTagCmd      { get; }
        public ReactiveCommand<Unit, Unit> EditTagCmd     { get; }
        public ReactiveCommand<Unit, Unit> RemoveTagCmd   { get; }
        public ReactiveCommand<Unit, Unit> MoveTagCmd     { get; }

        public ReactiveCommand<Unit, Unit> SaveDirCmd { get; }
        public ReactiveCommand<Unit, Unit> CancelDirCmd { get; }
        public ReactiveCommand<Unit, Unit> SaveTagCmd   { get; }
        public ReactiveCommand<Unit, Unit> CancelTagCmd { get; }
        
        #endregion
        
        #region Properties

        private ObservableCollectionWithSelectedItem<UI_Dir> _dirsTree;

        public ObservableCollectionWithSelectedItem<UI_Dir> DirsTree
        {
            get => _dirsTree;
            set => this.RaiseAndSetIfChanged(ref _dirsTree, value);
        }

        private bool _showInnerTags;

        public bool ShowInnerTags
        {
            get => _showInnerTags;
            set
            {
                this.RaiseAndSetIfChanged(ref _showInnerTags, value);
                if (DirsTree.SelectedItem != null)
                    DirsTree.SelectedItem.UiTagsLoad(_showInnerTags);
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

        private UI_Tag? _editedTag;

        public UI_Tag? EditedTag
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
            _wnd                    = wnd;
            DownloadFromDanbooruCmd = ReactiveCommand.Create(OnDownloadFromDanbooru);
            ResetDatabaseCmd        = ReactiveCommand.Create(OnResetDatabase);
            LoadTreeCmd             = ReactiveCommand.Create(OnLoadTree);

            AddRootDirCmd  = ReactiveCommand.Create(OnAddRootDir);
            AddChildDirCmd = ReactiveCommand.Create(OnAddChildDir, this.WhenAnyValue(x => x.DirsTree.SelectedItem).Select(x => x != null));
            EditDirCmd     = ReactiveCommand.Create(OnEditDir,     this.WhenAnyValue(x => x.DirsTree.SelectedItem).Select(x => x != null));
            RemoveDirCmd   = ReactiveCommand.Create(OnRemoveDir,   this.WhenAnyValue(x => x.DirsTree.SelectedItem).Select(x => x != null));
            MoveDirCmd     = ReactiveCommand.Create(OnMoveDir,     this.WhenAnyValue(x => x.DirsTree.SelectedItem).Select(x => x != null));
            AddTagCmd = ReactiveCommand.Create(OnAddTag,
                                               this.WhenAnyValue(x => x.DirsTree.SelectedItem, x => x.DirsTree.SelectedItem.UI_Tags.SelectedItem,
                                                                 (dir, tag) => dir != null && tag != null));
            EditTagCmd = ReactiveCommand.Create(OnEditTag,
                                                this.WhenAnyValue(x => x.DirsTree.SelectedItem, x => x.DirsTree.SelectedItem.UI_Tags.SelectedItem,
                                                                  (dir, tag) => dir != null && tag != null));
            RemoveTagCmd = ReactiveCommand.Create(OnRemoveTag,
                                                  this.WhenAnyValue(x => x.DirsTree.SelectedItem, x => x.DirsTree.SelectedItem.UI_Tags.SelectedItem,
                                                                    (dir, tag) => dir != null && tag != null));
            MoveTagCmd = ReactiveCommand.Create(OnMoveTag,
                                                this.WhenAnyValue(x => x.DirsTree.SelectedItem, x => x.DirsTree.SelectedItem.UI_Tags.SelectedItem,
                                                                  (dir, tag) => dir != null && tag != null));
            CancelTagCmd = ReactiveCommand.Create(OnCancelTag);
            SaveDirCmd   = ReactiveCommand.Create(OnSaveDir);
            CancelDirCmd = ReactiveCommand.Create(OnCancelDir);
            SaveTagCmd   = ReactiveCommand.Create(OnSaveTag);
            var service = new DirService();
            DirsTree                  =  new ObservableCollectionWithSelectedItem<UI_Dir>(service.GetTopDirs());
            DirsTree.SelectionChanged += DirsTreeOnSelectionChanged;
        }

        #endregion
        
        #region CmdExec

        private async void OnLoadTree()
        {
            var res = await MessageBoxManager.GetMessageBoxStandardWindow("", "Reload tree from database?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd);
            if (res == ButtonResult.Yes)
            {
                var service = new DirService();
                DirsTree                  =  new ObservableCollectionWithSelectedItem<UI_Dir>(service.GetTopDirs());
                DirsTree.SelectionChanged += DirsTreeOnSelectionChanged;
            }
        }

        private async void OnResetDatabase()
        {
            if (await MessageBoxManager.GetMessageBoxStandardWindow("", "Are you sure about that?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd) == ButtonResult.Yes)
                g.Ctx = new TagContext(true);
        }

        private async void OnDownloadFromDanbooru()
        {
            var res = await MessageBoxManager.GetMessageBoxStandardWindow("", "Reset database?", ButtonEnum.YesNoCancel, Icon.Question).ShowDialog(_wnd);
            if (res == ButtonResult.Yes)
                g.Ctx = new TagContext(true);
            else if (res == ButtonResult.Cancel) return;
            var loader = new DanbooruLoader();
            loader.DownloadAll();
            var service = new DirService();
            DirsTree                  =  new ObservableCollectionWithSelectedItem<UI_Dir>(service.GetTopDirs());
            DirsTree.SelectionChanged += DirsTreeOnSelectionChanged;
        }
        
        private async void OnAddRootDir()
        {
            EditedDir      = new UI_Dir("");
            DirEditVisible = true;
        }

        private async void OnAddChildDir()
        {
            EditedDir      = new UI_Dir("", DirsTree.SelectedItem.Id);
            DirEditVisible = true;
        }

        private async void OnEditDir()
        {
            EditedDir      = new UI_Dir();
            EditedDir.Id   = DirsTree.SelectedItem.Id;
            EditedDir.Name = DirsTree.SelectedItem.Name;
            DirEditVisible = true;
        }
        
        private async void OnSaveDir()
        {
            if (EditedDir.Id == 0)
            {
                EditedDir.Add();
                if (EditedDir.ParentId.HasValue)
                {
                    DirsTree.SelectedItem.ChildDirs.Add(EditedDir);
                    DirsTree.SelectedItem.UiChildsLoad();
                }
                else
                {
                    DirsTree.Add(EditedDir);
                }
            }
            else
            {
                DirsTree.SelectedItem.Name = EditedDir.Name;
                DirsTree.SelectedItem.Save();
            }

            DirEditVisible = false;
            EditedDir = null;
        }

        private async void OnCancelDir()
        {
            DirEditVisible = false;
            EditedDir  = null;
        }

        private async void OnRemoveDir()
        {
            EditedDir      = null;
            DirEditVisible = false;
            var res = await MessageBoxManager.GetMessageBoxStandardWindow("", $"Remove dir: \"{DirsTree.SelectedItem.Name}\"?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd);
            if (res == ButtonResult.Yes)
            {
                DirsTree.SelectedItem.Remove();
                if (DirsTree.SelectedItem.UI_Parent == null)
                {
                    DirsTree.Remove(DirsTree.SelectedItem);
                }
                else
                {
                    DirsTree.SelectedItem.UI_Parent.ChildDirs.Remove(DirsTree.SelectedItem as Dir);
                    DirsTree.SelectedItem.ParentDir.ChildDirs.Remove(DirsTree.SelectedItem as Dir);
                    DirsTree.SelectedItem.UI_Parent.UiChildsLoad();
                }
            }
        }

        private async void OnMoveDir()
        {

        }

        private async void OnAddTag()
        {

        }

        private async void OnEditTag()
        {

        }

        private async void OnRemoveTag()
        {

        }

        private async void OnMoveTag()
        {

        }

        private async void OnSaveTag()
        {

        }

        private async void OnCancelTag()
        {

        }


        #endregion

        private void DirsTreeOnSelectionChanged(ObservableCollectionWithSelectedItem<UI_Dir> sender, UI_Dir newselection, UI_Dir oldselection)
        {
            if(DirsTree.SelectedItem !=  null)
                DirsTree.SelectedItem.UiTagsLoad(_showInnerTags);
            OnCancelDir();
            OnCancelTag();
        }

        public void TagsOnSelectionChanged(UI_Tag newselection, UI_Tag oldselection)
        {
            OnCancelTag();
        }
    }
}
