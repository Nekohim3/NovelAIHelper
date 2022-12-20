using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Services;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels
{
    public class UI_PartTag : PartTag
    {
        public static MapperConfiguration Map    = new(x => x.CreateMap<PartTag, UI_PartTag>());
        public static Mapper              Mapper = new(Map);

        private UI_Tag _uI_Tag;

        public UI_Tag UI_Tag
        {
            get => _uI_Tag;
            set => this.RaiseAndSetIfChanged(ref _uI_Tag, value);
        }

        public UI_PartTag()
        {
            
        }

        public bool Save()
        {
            if (Id == 0) return Add();
            var service = new SessionTagService();
            return service.Save(this);
        }

        public bool Add()
        {
            if (Id != 0) return Save();
            var service = new SessionTagService();
            service.Add(this);
            return true;
        }

        public bool Remove()
        {
            if (Id == 0) return false;
            var service = new SessionTagService();
            return service.Remove(this);
        }
    }
}
