using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AvaloniaEdit.Utils;
using DynamicData;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils;
using NovelAIHelper.Utils.Collections;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels;

public class UI_Dir : Dir, ISelected, IExpanded
{
    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }

    private bool _isExpanded;

    public bool IsExpanded
    {
        get => _isExpanded;
        set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
    }

    private ObservableCollectionWithSelectedItem<UI_Dir> _ui_Childs = new();

    public ObservableCollectionWithSelectedItem<UI_Dir> UI_Childs
    {
        get => _ui_Childs;
        set => this.RaiseAndSetIfChanged(ref _ui_Childs, value);
    }

    private ObservableCollectionWithSelectedItem<UI_Tag> _ui_Tags = new();

    public ObservableCollectionWithSelectedItem<UI_Tag> UI_Tags
    {
        get => _ui_Tags;
        set => this.RaiseAndSetIfChanged(ref _ui_Tags, value);
    }

    private UI_Dir? _ui_Parent;

    public UI_Dir? UI_Parent
    {
        get => _ui_Parent;
        set => this.RaiseAndSetIfChanged(ref _ui_Parent, value);
    }

    public string SearchedDisplay => $"{(UI_Parent != null ? $"{UI_Parent.SearchedDisplay}\n  " : "")}{Name}";

    public UI_Dir() : base()
    {
        _ui_Childs.CollectionChanged += Ui_ChildsOnCollectionChanged;
        UI_Tags.CollectionChanged    += UI_TagsOnCollectionChanged;
    }

    public UI_Dir(string name, int? parentId = null, string? link = null, string? comment = null) : base(name, parentId, link, comment)
    {
        _ui_Childs.CollectionChanged += Ui_ChildsOnCollectionChanged;
        UI_Tags.CollectionChanged    += UI_TagsOnCollectionChanged;
    }



    private void Ui_ChildsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ChildDirs = UI_Childs.OfType<Dir>().ToList();
        //switch (e.Action)
        //{
        //    case NotifyCollectionChangedAction.Add:
        //        ChildDirs.Add(e.NewItems[0] as UI_Dir);
        //        break;
        //    case NotifyCollectionChangedAction.Remove:
        //        ChildDirs.Remove(e.OldItems[0] as UI_Dir);
        //        break;
        //    case NotifyCollectionChangedAction.Replace:
        //        (ChildDirs as List<Dir>)[e.NewStartingIndex] = e.NewItems[0] as UI_Dir;
        //        break;
        //    case NotifyCollectionChangedAction.Reset:
        //        ChildDirs.Clear();
        //        break;
        //    case NotifyCollectionChangedAction.Move:
        //        ((ChildDirs as List<Dir>)[e.OldStartingIndex], (ChildDirs as List<Dir>)[e.NewStartingIndex]) =
        //            ((ChildDirs as List<Dir>)[e.NewStartingIndex], (ChildDirs as List<Dir>)[e.OldStartingIndex]);
        //        break;
        //}
    }

    private void UI_TagsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Tags = UI_Tags.OfType<Tag>().ToList();
        //switch (e.Action)
        //{
        //    case NotifyCollectionChangedAction.Add:
        //        Tags.Add(e.NewItems[0] as UI_Tag);
        //        break;
        //    case NotifyCollectionChangedAction.Remove:
        //        Tags.Remove(e.OldItems[0] as UI_Tag);
        //        break;
        //    case NotifyCollectionChangedAction.Replace:
        //        (Tags as List<Tag>)[e.NewStartingIndex] = e.NewItems[0] as UI_Tag;
        //        break;
        //    case NotifyCollectionChangedAction.Reset:
        //        Tags.Clear();
        //        break;
        //    case NotifyCollectionChangedAction.Move:
        //        ((Tags as List<Tag>)[e.OldStartingIndex], (Tags as List<Tag>)[e.NewStartingIndex]) = ((Tags as List<Tag>)[e.NewStartingIndex], (Tags as List<Tag>)[e.OldStartingIndex]);
        //        break;
        //}
    }
}
