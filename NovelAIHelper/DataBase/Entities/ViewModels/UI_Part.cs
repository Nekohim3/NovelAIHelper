using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.Utils.Collections;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels
{
    public class UI_Part : Part
    {
        public static MapperConfiguration Map    = new(x => x.CreateMap<Part, UI_Part>());
        public static Mapper              Mapper = new(Map);

        private UI_Session _uI_Session;

        public UI_Session UI_Session
        {
            get => _uI_Session;
            set => this.RaiseAndSetIfChanged(ref _uI_Session, value);
        }

        private ObservableCollectionWithSelectedItem<UI_PartTag> _partTags = new();

        public ObservableCollectionWithSelectedItem<UI_PartTag> PartTags
        {
            get => _partTags;
            set => this.RaiseAndSetIfChanged(ref _partTags, value);
        }

        public UI_Part()
        {
            
        }
    }
}
