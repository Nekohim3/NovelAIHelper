using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NovelAIHelper.DataBase.Entities.DataBase;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels
{
    internal class UI_Tag : Tag, ISelected
    {
        public static MapperConfiguration Map    = new(x => x.CreateMap<Tag, UI_Tag>());
        public static Mapper              Mapper = new(Map);

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        public UI_Tag()
        {

        }

        public UI_Tag(string name, string? link = null)
        {
            Name = name;
            Link = link;
        }
    }
}
