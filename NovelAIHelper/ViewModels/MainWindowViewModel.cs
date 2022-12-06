using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using Avalonia.Controls;
using NovelAIHelper.DataBase;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.Views;

namespace NovelAIHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Window                      _wnd;
        public  ReactiveCommand<Unit, Unit> TagEditorShowCmd { get; }

        public MainWindowViewModel()
        {
            //var ctx = new TagContext();
            //var dir = new UI_Dir("testDir", null, null);
            //dir.ChildDirs.Add(new UI_Dir("testDir1", null, null));
            //var tag = new UI_Tag("testTag", null);
            //tag.TagDirs.Add(new TagDir(){Dir = dir, Tag = tag});
            //var tag1 = new UI_Tag("testTag1", null);
            //tag1.TagDirs.Add(new TagDir() { Dir = dir.ChildDirs.First(), Tag = tag1 });
            //ctx.Tags.Add(tag1);
            //ctx.Dirs.Add(dir);
            //ctx.Tags.Add(tag);
            //ctx.SaveChanges();
        }

        public MainWindowViewModel(Window wnd)
        {
            _wnd             = wnd;
            TagEditorShowCmd = ReactiveCommand.Create(OnTagEditorShow);
        }

        private void OnTagEditorShow()
        {
            var f = new TagEditorView();
            f.DataContext = new TagEditorViewModel(f);
            f.ShowDialog(_wnd);
        }
    }
}
