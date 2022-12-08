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
using NovelAIHelper.Utils.Collections;

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

        private ObservableCollectionWithSelectedItem<UI_Dir> _ui_Dirs;

        public ObservableCollectionWithSelectedItem<UI_Dir> UI_Dirs
        {
            get => _ui_Dirs ??= new ObservableCollectionWithSelectedItem<UI_Dir>(UI_Dir.Mapper.Map<ICollection<Dir>, ICollection<UI_Dir>>(Dirs));
            set => this.RaiseAndSetIfChanged(ref _ui_Dirs, value);
        }

        public UI_Tag()
        {

        }

        public UI_Tag(string name, string? link = null) : base(name, link)
        {

        }

        public void UiDirsLoad()
        {
            _ui_Dirs = new ObservableCollectionWithSelectedItem<UI_Dir>(UI_Dir.Mapper.Map<ICollection<Dir>, ICollection<UI_Dir>>(Dirs));
        }
    }
}
