using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NovelAIHelper.DataBase.Entities.DataBase;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels
{
    internal class UI_Dir : Dir, ISelected
    {
        public static MapperConfiguration Map    = new(x => x.CreateMap<Dir, UI_Dir>());
        public static Mapper              Mapper = new(Map);

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }


        private ObservableCollection<UI_Dir> _ui_Childs;

        public ObservableCollection<UI_Dir> UI_Childs
        {
            get => _ui_Childs ??= new ObservableCollection<UI_Dir>(Mapper.Map<ICollection<Dir>, ICollection<UI_Dir>>(ChildDirs));
            set => this.RaiseAndSetIfChanged(ref _ui_Childs, value);
        }

        private ObservableCollection<UI_Tag> _ui_Tags;

        public ObservableCollection<UI_Tag> UI_Tags
        {
            get => _ui_Tags ??= new ObservableCollection<UI_Tag>(UI_Tag.Mapper.Map<ICollection<Tag>, ICollection<UI_Tag>>(Tags));
            set => this.RaiseAndSetIfChanged(ref _ui_Tags, value);
        }

        public UI_Dir()
        {

        }

        public UI_Dir(string name, int? parentId = null, string? link = null)
        {
            Name     = name;
            ParentId = parentId;
            Link     = link;
        }
    }
}
