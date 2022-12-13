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

        private TagTree _tagTree = new();

        public TagTree TagTree
        {
            get => _tagTree;
            set => this.RaiseAndSetIfChanged(ref _tagTree, value);
        }

        public MainWindowViewModel()
        {

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
            f.Show(_wnd);
        }
    }
}
