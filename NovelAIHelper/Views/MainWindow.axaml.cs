using System;
using System.Data.Entity.Core.Mapping;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.Utils;
using NovelAIHelper.Utils.Collections;
using NovelAIHelper.Utils.DragHelpers;
using NovelAIHelper.ViewModels;

namespace NovelAIHelper.Views;

public partial class MainWindow : Window
{
    private Point     _startPos = new(0, 0);
    private bool      _captured = false;
    private bool      _isDrag   = false;
    private UI_Tag?   _tag;
    private UI_Group? _group;

    private DragObject _dragObject;

    public MainWindow()
    {
        InitializeComponent();
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }

    private async void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Grid tagGrid && tagGrid.Classes.Contains("TagGrid") && tagGrid.Parent is Border {Parent: ContentPresenter {Parent: ItemsControl tagCtrl}})
        {
            if (e.KeyModifiers == KeyModifiers.None)
            {
                _startPos   = new Point(e.GetPosition(this));
                _captured   = true;
                _tag        = tagGrid.DataContext as UI_Tag;
                _group   = tagCtrl.DataContext as UI_Group;
                _dragObject = DragObject.Tag;
            }
            else
            {
                var res = await MessageBoxManager
                                .GetMessageBoxStandardWindow("", $"Remove tag: \"{_group.Name}\"?", ButtonEnum.YesNo, MessageBox.Avalonia.Enums.Icon.Question)
                                .ShowDialog(this);
                if (res == ButtonResult.Yes)
                {
                    //var vm = (DataContext as MainWindowViewModel).TagTree.Sessions.SelectedItem;
                    //vm.UI_SessionParts.Remove(_sessionPart);
                    //var vm = (DataContext as MainWindowViewModel).TagGroupVM;
                    //vm.TagGrid.Remove(_tagGroup);
                }
            }
        }
        else if (sender is Grid groupTagGrid && groupTagGrid.Classes.Contains("GroupTagGrid"))
        {
            if (e.KeyModifiers == KeyModifiers.None)
            {
                _startPos    = new Point(e.GetPosition(this));
                _captured    = true;
                _group = groupTagGrid.DataContext as UI_Group;
                _dragObject  = DragObject.Group;
            }
            else if (e.KeyModifiers == KeyModifiers.Control)
            {
                var res = await MessageBoxManager
                                .GetMessageBoxStandardWindow("", $"Remove tag group: \"{_group.Name}\"?", ButtonEnum.YesNo, MessageBox.Avalonia.Enums.Icon.Question)
                                .ShowDialog(this);
                if (res == ButtonResult.Yes)
                {
                    //var vm = (DataContext as MainWindowViewModel).TagTree.Sessions.SelectedItem;
                    //var vm = (DataContext as MainWindowViewModel).TagGroupVM;
                    //vm.TagGrid.Remove((groupTagGrid.DataContext as TagGroup));
                }
            }
        }
        else if (sender is Grid grid && grid.Classes.Contains("ListBoxTagGrid"))
        {
            _startPos   = new Point(e.GetPosition(this));
            _captured   = true;
            _tag        = grid.DataContext as UI_Tag;
            _group   = null;
            _dragObject = DragObject.SearchedTag;
        }
    }

    private async void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isDrag && _captured && _startPos.Far(e.GetPosition(this)))
        {
            _isDrag = true;
            var dragData = new DataObject();
            if (_dragObject == DragObject.Tag)
            {
                if (_tag == null) return;
                dragData.Set("NovelAIHelper.UI_Tag", "");
                g.TagTree.DragStart(_group, _tag, _dragObject);
            }
            else if (_dragObject == DragObject.Group)
            {
                if (_group == null) return;
                dragData.Set("NovelAIHelper.TagGroup", "");
                g.TagTree.DragStart(_group, _dragObject);
            }
            else if (_dragObject == DragObject.SearchedTag)
            {
                if (_tag == null) return;
                dragData.Set("NovelAIHelper.UI_Tag", "");
                g.TagTree.DragStart(_group, _tag, _dragObject);
            }

            await DragDrop.DoDragDrop(e, dragData, DragDropEffects.Move);
            g.TagTree.DragEnd();
            _isDrag   = false;
            _captured = false;
            _tag      = null;
            _group    = null;
        }
    }


    private void DragOver(object? sender, DragEventArgs e)
    {
        if (e.Source is Grid tagGrid && tagGrid.Classes.Contains("TagGrid") && tagGrid.Parent is Border {Parent: ContentPresenter {Parent: ItemsControl tagCtrl}})
        {
            if (_dragObject is DragObject.Tag or DragObject.SearchedTag)
            {
                e.DragEffects = DragDropEffects.Move;
                g.TagTree.DragOver(tagCtrl.DataContext as UI_Group, tagGrid.DataContext as UI_Tag);
            }
            else
            {
                e.DragEffects = DragDropEffects.None;
            }
        }
        else if (e.Source is WrapPanel panel && panel.Classes.Contains("GroupTagWrapPanel"))
        {
            if (_dragObject is DragObject.Tag or DragObject.SearchedTag)
            {
                e.DragEffects = DragDropEffects.Move;
                g.TagTree.DragOver(panel.DataContext as UI_Group);
            }
            else
            {
                e.DragEffects = DragDropEffects.None;
            }
        }
        else if (e.Source is Grid groupTagGrid && groupTagGrid.Classes.Contains("GroupTagGrid"))
        {
            if (_dragObject == DragObject.Group)
            {
                e.DragEffects = DragDropEffects.Move;
                g.TagTree.DragOver(groupTagGrid.DataContext as UI_Group);
            }
            else
            {
                e.DragEffects = DragDropEffects.None;
            }
        }
    }

    private void InputElement_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (sender is not Grid {DataContext: UI_Tag tag}) return;
        if (e.KeyModifiers != KeyModifiers.Control) return;
        if (e.Delta.Y > 0)
        {
            if (tag.Strength < 5)
                tag.Strength++;
        }
        else if (e.Delta.Y < 0)
        {
            if (tag.Strength > -5)
                tag.Strength--;
        }
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

    public bool Far(Avalonia.Point point)
    {
        return Math.Abs(X - (int) point.X) > 5 || Math.Abs(Y - (int) point.Y) > 5;
    }
}
