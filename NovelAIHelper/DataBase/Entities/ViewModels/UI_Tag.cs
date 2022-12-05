using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<UI_Dir> _ui_Dirs;

        public ObservableCollection<UI_Dir> UI_Dirs
        {
            get => _ui_Dirs ??= new ObservableCollection<UI_Dir>(UI_Dir.Mapper.Map<ICollection<Dir>, ICollection<UI_Dir>>(Dirs));
            set => this.RaiseAndSetIfChanged(ref _ui_Dirs, value);
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
