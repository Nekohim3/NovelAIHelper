using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NovelAIHelper.DataBase;
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
