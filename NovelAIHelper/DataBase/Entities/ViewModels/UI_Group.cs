using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.Utils;
using NovelAIHelper.Utils.Collections;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels
{
    public class UI_Group : Group, ISelected
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        private ObservableCollectionWithSelectedItem<UI_GroupTag> _uI_GroupTags = new();
        public ObservableCollectionWithSelectedItem<UI_GroupTag> UI_GroupTags
        {
            get => _uI_GroupTags;
            set => this.RaiseAndSetIfChanged(ref _uI_GroupTags, value);
        }

        private UI_Session _uI_Session;
        public UI_Session UI_Session
        {
            get => _uI_Session;
            set => this.RaiseAndSetIfChanged(ref _uI_Session, value);
        }

        public UI_Group()
        {
            UI_GroupTags.CollectionChanged += UI_PartTagsOnCollectionChanged;
        }

        public UI_Group(string name, int order = 0, string? comment = null) : base(name, order, comment)
        {
            UI_GroupTags.CollectionChanged += UI_PartTagsOnCollectionChanged;
        }

        public UI_Group(string name, int idSession, int order = 0, string? comment = null) : base(name, idSession, order, comment)
        {
            UI_GroupTags.CollectionChanged += UI_PartTagsOnCollectionChanged;
        }

        private void UI_PartTagsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            GroupTags = UI_GroupTags.OfType<GroupTag>().ToList();
        }
    }
}
