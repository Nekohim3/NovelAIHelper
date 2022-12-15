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
using NovelAIHelper.Utils.Collections;
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

        private ObservableCollectionWithSelectedItem<UI_Tag> _firstList;

        public ObservableCollectionWithSelectedItem<UI_Tag> FirstList
        {
            get => _firstList;
            set => this.RaiseAndSetIfChanged(ref _firstList, value);
        }

        private ObservableCollectionWithSelectedItem<UI_Tag> _secondList;

        public ObservableCollectionWithSelectedItem<UI_Tag> SecondList
        {
            get => _secondList;
            set => this.RaiseAndSetIfChanged(ref _secondList, value);
        }

        private ObservableCollectionWithSelectedItem<UI_Tag> _thirdList;

        public ObservableCollectionWithSelectedItem<UI_Tag> ThirdList
        {
            get => _thirdList;
            set => this.RaiseAndSetIfChanged(ref _thirdList, value);
        }

        private bool _isDragging;

        public bool IsDragging
        {
            get => _isDragging;
            set => this.RaiseAndSetIfChanged(ref _isDragging, value);
        }

        private UI_Tag _draggedTag;

        public UI_Tag DraggedTag
        {
            get => _draggedTag;
            set => this.RaiseAndSetIfChanged(ref _draggedTag, value);
        }

        private ObservableCollectionWithSelectedItem<UI_Tag> _sourceDragList;

        public ObservableCollectionWithSelectedItem<UI_Tag> SourceDragList
        {
            get => _sourceDragList;
            set => this.RaiseAndSetIfChanged(ref _sourceDragList, value);
        }

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(Window wnd)
        {
            _wnd             = wnd;
            TagEditorShowCmd = ReactiveCommand.Create(OnTagEditorShow);
            _firstList       = new ObservableCollectionWithSelectedItem<UI_Tag>(TagTree.GetRange(10000, 10));
            _secondList       = new ObservableCollectionWithSelectedItem<UI_Tag>(TagTree.GetRange(20000, 10));
            _thirdList       = new ObservableCollectionWithSelectedItem<UI_Tag>(TagTree.GetRange(30000, 10));
        }

        private void OnTagEditorShow()
        {
            var f = new TagEditorView();
            f.DataContext = new TagEditorViewModel(f);
            f.Show(_wnd);
            f.Closed += (_, _) =>
                        {
                            f.DataContext = null;
                            TagTree       = new TagTree();
                            GC.Collect();
                        };
        }

        public void DragStart(ObservableCollectionWithSelectedItem<UI_Tag>? list)
        {
            if (list?.SelectedItem == null) return;
            SourceDragList    = list;
            DraggedTag        = list.SelectedItem;
            DraggedTag.IsDrag = true;
            IsDragging        = true;
        }

        public void DragEnd()
        {
            IsDragging        = false;
            DraggedTag.IsDrag = false;
        }

        public void DragEnter(ObservableCollectionWithSelectedItem<UI_Tag>? list)
        {
            if (list == null || SourceDragList == list) return;
            if(!list.Contains(DraggedTag))
                list.Add(DraggedTag);
        }

        public void DragLeave(ObservableCollectionWithSelectedItem<UI_Tag>? list)
        {
            if (list == null || SourceDragList == list) return;
            list.Remove(DraggedTag);
        }
    }
}
