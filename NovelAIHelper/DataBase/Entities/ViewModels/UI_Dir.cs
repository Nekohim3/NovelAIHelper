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


        private ObservableCollection<UI_Dir> _uI_Childs;

        public ObservableCollection<UI_Dir> UI_Childs
        {
            get => _uI_Childs ??= new ObservableCollection<UI_Dir>(Mapper.Map<ICollection<Dir>, ICollection<UI_Dir>>(ChildDirs));
            set => this.RaiseAndSetIfChanged(ref _uI_Childs, value);
        }

        private ObservableCollection<UI_Tag> _uI_Tags;

        public ObservableCollection<UI_Tag> UI_Tags
        {
            get => _uI_Tags ??= new ObservableCollection<UI_Tag>(UI_Tag.Mapper.Map<IEnumerable<Tag>, IEnumerable<UI_Tag>>(TagDirs.Select(x => x.Tag)).ToList());
            set => this.RaiseAndSetIfChanged(ref _uI_Tags, value);
        }

        public UI_Dir()
        {

        }

        public UI_Dir(string name, int? parentId = null, string? link = null)
        {
            Name     = name; //.Replace("Tag group:", "");
            ParentId = parentId;
            Link     = link;
        }
    }
}
