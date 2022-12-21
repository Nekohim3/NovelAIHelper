using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.Utils;
using NovelAIHelper.Utils.Collections;
using NovelAIHelper.Utils.DragHelpers;
using NovelAIHelper.Views;
using ReactiveUI;

namespace NovelAIHelper.ViewModels
{
    //public class TagGridViewModel : ViewModelBase
    //{
    //    private ObservableCollectionWithSelectedItem<TagGroup> _tagGrid;

    //    public ObservableCollectionWithSelectedItem<TagGroup> TagGrid
    //    {
    //        get => _tagGrid;
    //        set => this.RaiseAndSetIfChanged(ref _tagGrid, value);
    //    }

    //    private TagGroup? _sourceDragGroup;

    //    public TagGroup? SourceDragGroup
    //    {
    //        get => _sourceDragGroup;
    //        set => this.RaiseAndSetIfChanged(ref _sourceDragGroup, value);
    //    }

    //    private UI_Tag? _draggedTag;

    //    public UI_Tag? DraggedTag
    //    {
    //        get => _draggedTag;
    //        set => this.RaiseAndSetIfChanged(ref _draggedTag, value);
    //    }

    //    private TagGroup? _lastGroup;

    //    public TagGroup? LastGroup
    //    {
    //        get => _lastGroup;
    //        set => this.RaiseAndSetIfChanged(ref _lastGroup, value);
    //    }


    //    private DragObject _dragObject;

    //    public TagGridViewModel()
    //    {

    //    }

    //    public TagGridViewModel(ObservableCollectionWithSelectedItem<TagGroup> tagGrid)
    //    {
    //        _tagGrid = tagGrid;
    //    }

    //    public void DragStart(TagGroup? group, UI_Tag tag, DragObject dragObject)
    //    {
    //        SourceDragGroup   = group;
    //        DraggedTag        = tag;
    //        LastGroup         = SourceDragGroup;
    //        DraggedTag.IsDrag = true;
    //        _dragObject       = dragObject;
    //    }

    //    public void DragStart(TagGroup group, DragObject dragObject)
    //    {
    //        SourceDragGroup = group;
    //        _dragObject     = dragObject;
    //    }

    //    public void DragEnd()
    //    {
    //        SourceDragGroup = null;
    //        if (DraggedTag == null) return;
    //        DraggedTag.IsDrag = false;
    //        DraggedTag        = null;
    //    }

    //    public void DragOver(TagGroup? group, UI_Tag? tag = null)
    //    {
    //        if (_dragObject is DragObject.Tag or DragObject.SearchedTag)
    //        {
    //            if (DraggedTag == null) return;
    //            if (group      == null) return;
    //            if (tag == null)
    //            {
    //                if (LastGroup == null)
    //                {
    //                    LastGroup = group;
    //                    if (!group.TagList.Contains(DraggedTag))
    //                        group.TagList.Add(DraggedTag);
    //                }
    //                else
    //                {
    //                    if (LastGroup != group)
    //                    {
    //                        foreach (var x in TagGrid)
    //                            x.TagList.Remove(DraggedTag);
    //                        LastGroup = group;
    //                        group.TagList.Add(DraggedTag);
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                if (DraggedTag == tag) return;
    //                if (LastGroup == null)
    //                {
    //                    foreach (var x in TagGrid)
    //                        x.TagList.Remove(DraggedTag);
    //                    LastGroup = group;
    //                    if (!group.TagList.Contains(DraggedTag))
    //                        group.TagList.Add(DraggedTag);
    //                }
    //                else if (LastGroup == group)
    //                {
    //                    if (!group.TagList.Contains(DraggedTag))
    //                        group.TagList.Insert(group.TagList.IndexOf(tag), DraggedTag);
    //                    else
    //                        group.TagList.Move(group.TagList.IndexOf(DraggedTag), group.TagList.IndexOf(tag));
    //                }
    //                else
    //                {
    //                    foreach (var x in TagGrid)
    //                        x.TagList.Remove(DraggedTag);
    //                    LastGroup = group;
    //                    group.TagList.Insert(group.TagList.IndexOf(tag), DraggedTag);
    //                }
    //            }
    //        }
    //        else if (_dragObject == DragObject.Group)
    //        {
    //            if (group == null || SourceDragGroup == null) return;
    //            if (SourceDragGroup != group)
    //                TagGrid.Move(TagGrid.IndexOf(SourceDragGroup), TagGrid.IndexOf(group));
    //        }
    //    }
    //}
}
