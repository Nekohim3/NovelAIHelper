using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils;
using NovelAIHelper.Utils.Collections;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels;

public class UI_Session : Session, ISelected
{
    private ObservableCollectionWithSelectedItem<UI_Group> _uI_SessionGroups = new();
    public ObservableCollectionWithSelectedItem<UI_Group> UI_SessionGroups
    {
        get => _uI_SessionGroups;
        set => this.RaiseAndSetIfChanged(ref _uI_SessionGroups, value);
    }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }

    public UI_Session()
    {
        UI_SessionGroups.CollectionChanged += UI_SessionPartsOnCollectionChanged;
    }

    public UI_Session(string name, string? comment = null) : base(name, comment)
    {
        UI_SessionGroups.CollectionChanged += UI_SessionPartsOnCollectionChanged;
    }

    private void UI_SessionPartsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Groups = UI_SessionGroups.OfType<Group>().ToList();
    }
}