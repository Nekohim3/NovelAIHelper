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




        private ObservableCollectionWithSelectedItem<UI_Dir> _dirsTree;

        public ObservableCollectionWithSelectedItem<UI_Dir> DirsTree
        {
            get => _dirsTree;
            set => this.RaiseAndSetIfChanged(ref _dirsTree, value);
        }

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
                                                                 (dir, tag) => dir                                          != null && tag != null));
            EditTagCmd = ReactiveCommand.Create(OnEditTag,
                                                this.WhenAnyValue(x => x.DirsTree.SelectedItem, x => x.DirsTree.SelectedItem.UI_Tags.SelectedItem,
                                                                  (dir, tag) => dir                                          != null && tag != null));
            RemoveTagCmd = ReactiveCommand.Create(OnRemoveTag,
                                                  this.WhenAnyValue(x => x.DirsTree.SelectedItem, x => x.DirsTree.SelectedItem.UI_Tags.SelectedItem,
                                                                    (dir, tag) => dir                                          != null && tag != null));
            MoveTagCmd = ReactiveCommand.Create(OnMoveTag,
                                                this.WhenAnyValue(x => x.DirsTree.SelectedItem, x => x.DirsTree.SelectedItem.UI_Tags.SelectedItem,
                                                                  (dir, tag) => dir                                          != null && tag != null));
            var service = new DirService();
            DirsTree                  =  new ObservableCollectionWithSelectedItem<UI_Dir>(service.GetTopDirs());
            DirsTree.SelectionChanged += DirsTreeOnSelectionChanged;
        }



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
                new TagContext(true);
        }

        private async void OnDownloadFromDanbooru()
        {
            var res = await MessageBoxManager.GetMessageBoxStandardWindow("", "Reset database?", ButtonEnum.YesNoCancel, Icon.Question).ShowDialog(_wnd);
            if (res == ButtonResult.Yes)
                new TagContext(true);
            else if (res == ButtonResult.Cancel) return;
            var loader = new DanbooruLoader();
            loader.DownloadAll();
            var service = new DirService();
            DirsTree                  =  new ObservableCollectionWithSelectedItem<UI_Dir>(service.GetTopDirs());
            DirsTree.SelectionChanged += DirsTreeOnSelectionChanged;
        }
        
        private async void OnAddRootDir()
        {

        }

        private async void OnAddChildDir()
        {

        }

        private async void OnEditDir()
        {

        }

        private async void OnRemoveDir()
        {

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




        #endregion

        private void DirsTreeOnSelectionChanged(ObservableCollectionWithSelectedItem<UI_Dir> sender, UI_Dir newselection, UI_Dir oldselection)
        {

        }
    }
}
