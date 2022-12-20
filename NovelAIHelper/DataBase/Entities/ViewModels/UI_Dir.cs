using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AvaloniaEdit.Utils;
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

    public UI_Dir() { }

    public UI_Dir(string name, int? parentId = null, string? link = null) : base(name, parentId, link) { }
    
    
}