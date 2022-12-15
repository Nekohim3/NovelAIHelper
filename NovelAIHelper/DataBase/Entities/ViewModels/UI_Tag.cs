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
using NovelAIHelper.DataBase.Services;

namespace NovelAIHelper.DataBase.Entities.ViewModels
{
    public class UI_Tag : Tag, ISelected, IDraggable
    {
        public static MapperConfiguration Map    = new(x => x.CreateMap<Tag, UI_Tag>());
        public static Mapper              Mapper = new(Map);

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        private bool _isDrag;

        public bool IsDrag
        {
            get => _isDrag;
            set => this.RaiseAndSetIfChanged(ref _isDrag, value);
        }

        public string SearchedDisplay => GetSearchedDisplay();

        //private ObservableCollectionWithSelectedItem<UI_Dir> _ui_Dirs = new();

        //public ObservableCollectionWithSelectedItem<UI_Dir> UI_Dirs
        //{
        //    get => _ui_Dirs;// ??= new ObservableCollectionWithSelectedItem<UI_Dir>(UI_Dir.Mapper.Map<ICollection<Dir>, ICollection<UI_Dir>>(Dirs));
        //    set => this.RaiseAndSetIfChanged(ref _ui_Dirs, value);
        //}

        private UI_Dir _uI_Dir;

        public UI_Dir UI_Dir
        {
            get => _uI_Dir;
            set => this.RaiseAndSetIfChanged(ref _uI_Dir, value);
        }

        public UI_Tag()
        {

        }

        public UI_Tag(string name, int dirId, string? link = null) : base(name, dirId, link)
        {

        }

        public UI_Tag(string name, string? link = null) : base(name, link)
        {

        }

        public bool Save()
        {
            if (Id == 0) return Add();
            var service = new TagService();
            return service.Save(this);
        }

        public bool Add()
        {
            if (Id != 0) return Save();
            var service = new TagService();
            return service.Add(this);
        }

        public bool Remove()
        {
            if (Id == 0) return false;
            var service = new TagService();
            return service.Remove(this);
        }

        private string GetSearchedDisplay()
        {
            var str = $"{Name}\n  {UI_Dir.SearchedDisplay}";

            return str.TrimEnd('\n');
        }
    }
}
