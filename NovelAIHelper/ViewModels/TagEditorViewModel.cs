using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using NovelAIHelper.DataBase;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils.TagDownloader;
using ReactiveUI;

namespace NovelAIHelper.ViewModels
{
    internal class TagEditorViewModel
    {
        public ReactiveCommand<Unit, Unit> DownloadFromDanbooruCmd { get; }
        
        public TagEditorViewModel()
        {
            DownloadFromDanbooruCmd = ReactiveCommand.Create(OnDownloadFromDanbooru);
        }

        private void OnDownloadFromDanbooru()
        {
            //var loader   = new DanbooruLoader();
            //var dirsTree = loader.DownloadDirs();
            //var saved    = loader.SaveDirs(dirsTree);
            var lst      = new DirService().GetTopDirs().ToList();
            foreach (var x in lst)
            {
                Load(x);
            }
        }

        private void Load(UI_Dir dir)
        {
            var q = dir.UI_Childs;
            var w = dir.UI_Tags;
            foreach (var x in q)
            {
                Load(x);
            }
        }
    }
}
