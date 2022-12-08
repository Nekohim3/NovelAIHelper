using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AvaloniaEdit.Utils;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils.Collections;
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


        private ObservableCollectionWithSelectedItem<UI_Dir> _ui_Childs;

        public ObservableCollectionWithSelectedItem<UI_Dir> UI_Childs
        {
            get => _ui_Childs ??= new ObservableCollectionWithSelectedItem<UI_Dir>(Mapper.Map<ICollection<Dir>, ICollection<UI_Dir>>(ChildDirs));
            set => this.RaiseAndSetIfChanged(ref _ui_Childs, value);
        }

        private ObservableCollectionWithSelectedItem<UI_Tag> _ui_Tags;

        public ObservableCollectionWithSelectedItem<UI_Tag> UI_Tags
        {
            get => _ui_Tags ??= new ObservableCollectionWithSelectedItem<UI_Tag>(UI_Tag.Mapper.Map<ICollection<Tag>, ICollection<UI_Tag>>(Tags));
            set => this.RaiseAndSetIfChanged(ref _ui_Tags, value);
        }

        

        private UI_Dir? _ui_Parent;

        public UI_Dir? UI_Parent
        {
            get => _ui_Parent ??= Mapper.Map<UI_Dir>(ParentDir);
            set => this.RaiseAndSetIfChanged(ref _ui_Parent, value);
        }

        public UI_Dir()
        {

        }

        public UI_Dir(string name, int? parentId = null, string? link = null) : base(name, parentId, link)
        {

        }

        public void UiChildsLoad()
        {
            UI_Childs.Clear();
            UI_Childs.AddRange(Mapper.Map<ICollection<Dir>, ICollection<UI_Dir>>(ChildDirs));
        }

        public void UiTagsLoad(bool showInner = false)
        {
            UI_Tags.Clear();
            if (!showInner)
                UI_Tags.AddRange(UI_Tag.Mapper.Map<ICollection<Tag>, ICollection<UI_Tag>>(Tags));
            else
            {
                var list = new List<Tag>();
                list.AddRange(Tags);
                foreach (var x in ChildDirs)
                    TagsLoadRec(x, list);
                UI_Tags.AddRange(UI_Tag.Mapper.Map<ICollection<Tag>, ICollection<UI_Tag>>(list));
            }
        }

        private void TagsLoadRec(Dir dir, List<Tag> tags)
        {
            tags.AddRange(dir.Tags);
            foreach (var x in dir.ChildDirs)
                TagsLoadRec(x, tags);
        }

        public bool Save()
        {
            if (Id == 0) return Add();
            var service = new DirService();
            return service.Save(this);
        }

        public bool Add()
        {
            if (Id != 0) return Save();
            var service = new DirService();
            return service.Add(this);
        }

        public bool Remove()
        {
            if (Id == 0) return false;
            var service = new DirService();
            return service.Remove(this);
        }
    }

    
}
