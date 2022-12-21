using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaEdit.Utils;
using NovelAIHelper.DataBase;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.ViewModels;
using NovelAIHelper.Views;

namespace NovelAIHelper
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            //g.ResetCtx(true);
            //var dir  = new UI_Dir("testdir");
            //var tag1 = new UI_Tag("1");
            //var tag2 = new UI_Tag("2");
            //var tag3 = new UI_Tag("3");
            //dir.UI_Tags.AddRange(new List<UI_Tag> {tag1, tag2, tag3});
            //dir.Save();
            //dir.UI_Tags.RemoveAt(0);
            //dir.Save();
            ////g.ResetCtx();
            //dir.UI_Tags[0].Name = tag1.Name;
            //dir.Save();


            //new DirService().Save(dir);
            //var tag = new UI_Tag("testtag", dir.Id);
            //new TagService().Save(tag);
            //var q = new DirService().GetAll().ToList();
            //var w = new TagService().GetAll().ToList();
            //new DirService().Delete(dir);
            //q = new DirService().GetAll().ToList();
            //w = new TagService().GetAll().ToList();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                var vm = new MainWindowViewModel(desktop.MainWindow);
                desktop.MainWindow.DataContext = vm;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
