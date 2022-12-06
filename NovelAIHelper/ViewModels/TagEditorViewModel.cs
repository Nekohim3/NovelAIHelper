using System;
using System.Collections.Generic;
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
using NovelAIHelper.Utils.TagDownloader;
using ReactiveUI;

namespace NovelAIHelper.ViewModels
{
    internal class TagEditorViewModel
    {
        private Window _wnd;

        public ReactiveCommand<Unit, Unit> DownloadFromDanbooruCmd { get; }
        public ReactiveCommand<Unit, Unit> ResetDatabaseCmd        { get; }

        public TagEditorViewModel()
        {
            
        }

        public TagEditorViewModel(Window wnd)
        {
            _wnd                    = wnd;
            DownloadFromDanbooruCmd = ReactiveCommand.Create(OnDownloadFromDanbooru);
            ResetDatabaseCmd        = ReactiveCommand.Create(OnResetDatabase);
            LoadTreeCmd             = ReactiveCommand.Create(OnLoadTree);
        }

        public ReactiveCommand<Unit, Unit> LoadTreeCmd { get; }


        private void OnLoadTree()
        {
            var service = new DirService();

            var lst = service.GetTopDirs().ToList();
        }

        private async void OnResetDatabase()
        {
            if (await MessageBoxManager.GetMessageBoxStandardWindow("", "Are you sure about that?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd) == ButtonResult.Yes)
                new TagContext(true);
        }

        private async void OnDownloadFromDanbooru()
        {
            if (await MessageBoxManager.GetMessageBoxStandardWindow("", "Reset database?", ButtonEnum.YesNo, Icon.Question).ShowDialog(_wnd) == ButtonResult.Yes)
                new TagContext(true);
            var loader   = new DanbooruLoader();
            loader.DownloadAll();
            //var dirsTree = loader.DownloadDirs();
            //var saved    = loader.SaveDirs(dirsTree);
            //if (saved)
            //    await MessageBoxManager.GetMessageBoxStandardWindow("", "Success", ButtonEnum.Ok, Icon.Success).ShowDialog(_wnd);
            //else
            //    await MessageBoxManager.GetMessageBoxStandardWindow("", "Error while download or save", ButtonEnum.Ok, Icon.Error).ShowDialog(_wnd);
        }

        //private void Load(UI_Dir dir)
        //{
        //    var q = dir.UI_Childs;
        //    var w = dir.UI_Tags;
        //    foreach (var x in q)
        //    {
        //        Load(x);
        //    }
        //}
    }
}
