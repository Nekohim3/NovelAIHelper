using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
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
            OnLoadTree();
        }

        public ReactiveCommand<Unit, Unit> LoadTreeCmd { get; }


        private void OnLoadTree()
        {
            var service = new DirService();

            DirsTree = new ObservableCollectionWithSelectedItem<UI_Dir>(service.GetTopDirs());
            DirsTree.SelectionChanged += DirsTreeOnSelectionChanged;
        }

        private void DirsTreeOnSelectionChanged(ObservableCollectionWithSelectedItem<UI_Dir> sender, UI_Dir newselection, UI_Dir oldselection)
        {
            
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
            var loader   = new DanbooruLoader();
            loader.DownloadAll();
        }
    }
}
