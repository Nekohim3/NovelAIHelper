using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.Utils;
using NovelAIHelper.ViewModels;
using System.Diagnostics;

namespace NovelAIHelper.Views;

public enum DragObject
{
    Tag,
    Group,
    SearchedTag
}

public partial class TagGridView : UserControl
{
    private Point     _startPos = new(0, 0);
    private bool      _captured = false;
    private bool      _isDrag   = false;
    private UI_Tag?   _tag;
    private TagGroup? _tagGroup;

    private DragObject _dragObject;

    public TagGridView()
    {
        InitializeComponent();
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Grid tagGrid && tagGrid.Classes.Contains("TagGrid") && tagGrid.Parent is Border {Parent: ContentPresenter {Parent: ItemsControl tagCtrl}})
        {
            _startPos   = new Point(e.GetPosition(this));
            _captured   = true;
            _tag        = tagGrid.DataContext as UI_Tag;
            _tagGroup   = tagCtrl.DataContext as TagGroup;
            _dragObject = DragObject.Tag;
        }
        else if (sender is Grid groupTagGrid && groupTagGrid.Classes.Contains("GroupTagGrid"))
        {
            _startPos   = new Point(e.GetPosition(this));
            _captured   = true;
            _tagGroup   = groupTagGrid.DataContext as TagGroup;
            _dragObject = DragObject.Group;
        }
    }

    private async void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isDrag && _captured && _startPos.Far(e.GetPosition(this)))
        {
            _isDrag = true;
            var vm       = DataContext as TagGridViewModel;
            var dragData = new DataObject();
            if (_dragObject == DragObject.Tag)
            {
                if (_tag == null || _tagGroup == null) return;
                dragData.Set("NovelAIHelper.UI_Tag", "");
                vm.DragStart(_tagGroup, _tag);
            }
            else if (_dragObject == DragObject.Group)
            {
                if (_tagGroup == null) return;
                dragData.Set("NovelAIHelper.TagGroup", "");
                vm.DragStart(_tagGroup);
            }

            var result = await DragDrop.DoDragDrop(e, dragData, DragDropEffects.Move);
            vm.DragEnd();
            _isDrag   = false;
            _captured = false;
            _tag      = null;
            _tagGroup = null;
        }
    }

    private void DragOver(object? sender, DragEventArgs e)
    {
        if (e.Source is Grid tagGrid && tagGrid.Classes.Contains("TagGrid") && tagGrid.Parent is Border {Parent: ContentPresenter {Parent: ItemsControl tagCtrl}})
        {
            if (_dragObject == DragObject.Tag)
            {
                e.DragEffects = DragDropEffects.Move;
                var vm = DataContext as TagGridViewModel;
                vm.DragOver(tagCtrl.DataContext as TagGroup, tagGrid.DataContext as UI_Tag);
            }
            else
            {
                e.DragEffects = DragDropEffects.None;
            }
        }
        else if (e.Source is WrapPanel panel && panel.Classes.Contains("GroupTagWrapPanel"))
        {
            if (_dragObject == DragObject.Tag)
            {
                e.DragEffects = DragDropEffects.Move;
                var vm = DataContext as TagGridViewModel;
                vm.DragOver(panel.DataContext as TagGroup);
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
                var vm = DataContext as TagGridViewModel;
                vm.DragOver(groupTagGrid.DataContext as TagGroup);
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
