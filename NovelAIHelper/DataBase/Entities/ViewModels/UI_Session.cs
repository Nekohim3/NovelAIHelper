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
    private ObservableCollectionWithSelectedItem<UI_SessionPart> _uI_SessionParts = new();
    public ObservableCollectionWithSelectedItem<UI_SessionPart> UI_SessionParts
    {
        get => _uI_SessionParts;
        set => this.RaiseAndSetIfChanged(ref _uI_SessionParts, value);
    }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }

    public UI_Session()
    {
        UI_SessionParts.CollectionChanged += UI_SessionPartsOnCollectionChanged;
    }

    public UI_Session(string name, string? comment = null) : base(name, comment)
    {
        UI_SessionParts.CollectionChanged += UI_SessionPartsOnCollectionChanged;
    }

    private void UI_SessionPartsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Parts = UI_SessionParts.OfType<SessionPart>().ToList();
    }
}