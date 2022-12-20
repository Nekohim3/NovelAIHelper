using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils.Collections;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels;

public class UI_Session : Session
{
    public static MapperConfiguration Map    = new(x => x.CreateMap<Session, UI_Session>());
    public static Mapper              Mapper = new(Map);

    private ObservableCollectionWithSelectedItem<UI_PartTag> _uI_SessionTags = new();

    public ObservableCollectionWithSelectedItem<UI_PartTag> UI_SessionTags
    {
        get => _uI_SessionTags;
        set => this.RaiseAndSetIfChanged(ref _uI_SessionTags, value);
    }

    public UI_Session()
    {
    }

    public bool Save()
    {
        if (Id == 0) return Add();
        var service = new SessionService();
        service.Save(this);
        var tagService = new SessionTagService();
        foreach (var tag in UI_SessionTags) tagService.Save(tag);
        return true;
    }

    public bool Add()
    {
        if (Id != 0) return Save();
        var service = new SessionService();
        service.Add(this);
        var tagService = new SessionTagService();
        foreach (var tag in UI_SessionTags) tagService.Add(tag);
        return true;
    }

    public bool Remove()
    {
        if (Id == 0) return false;
        var lst     = new SessionTagService().GetAllBySessionId(Id).ToList();
        var service = new SessionTagService();
        return service.RemoveRange(lst.Where(x => x.Id > 0).Reverse());
    }
}
