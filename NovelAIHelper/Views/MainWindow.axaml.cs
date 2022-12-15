using System;
using System.Data.Entity.Core.Mapping;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.Utils.Collections;
using NovelAIHelper.ViewModels;

namespace NovelAIHelper.Views
{
    public partial class MainWindow : Window
    {
        private Point _startPos = new(0, 0); 
        private bool  _captured = false;
        private bool  _isDrag   = false;
        public MainWindow()
        {
            InitializeComponent();
            AddHandler(DragDrop.DragEnterEvent, DragEnter);
            AddHandler(DragDrop.DragLeaveEvent, DragLeave);
        }

        private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            _startPos = new Point(e.GetPosition(this));
            _captured = true;
        }

        private async void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (!_isDrag && _captured && _startPos.Far(e.GetPosition(this)))
                _isDrag = true;
            if (!_isDrag || !_captured) return;
            if (sender is not Grid grid) return;
            if ((grid.Parent as ListBoxItem)?.Parent is not ListBox {Items: ObservableCollectionWithSelectedItem<UI_Tag> obsCol}) return;
            if (obsCol.SelectedItem == null) return;
            var dragData = new DataObject();
            dragData.Set(DataFormats.Text, "");
            var vm = DataContext as MainWindowViewModel;
            vm?.DragStart(obsCol);
            var result = await DragDrop.DoDragDrop(e, dragData, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link);
            Debug.WriteLine($"[DragDropEffects]: {sender.GetType()}");
            vm?.DragEnd();
            _isDrag   = false;
            _captured = false;
        }

        private void DragLeave(object? sender, DragEventArgs e)
        {
            if (((e.Source as Grid)?.Parent as Grid)?.Children.FirstOrDefault(x => x is ListBox) is not ListBox lb) return;
            var vm = DataContext as MainWindowViewModel;
            vm?.DragLeave(lb.Items as ObservableCollectionWithSelectedItem<UI_Tag>);
        }

        private void DragEnter(object? sender, DragEventArgs e)
        {
            if (((e.Source as Grid)?.Parent as Grid)?.Children.FirstOrDefault(x => x is ListBox) is not ListBox lb) return;
            var vm = DataContext as MainWindowViewModel;
            vm?.DragEnter(lb.Items as ObservableCollectionWithSelectedItem<UI_Tag>);
        }
    }

    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Avalonia.Point point)
        {
            X = (int) point.X;
            Y = (int) point.Y;
        }

        public bool Far(Avalonia.Point point) => Math.Abs(X - (int) point.X) > 5 || Math.Abs(Y - (int) point.Y) > 5;
    }
}
