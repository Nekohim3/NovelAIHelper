using System;
using System.Collections.Generic;
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
    public class UI_SessionPart : SessionPart, ISelected
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        private ObservableCollectionWithSelectedItem<UI_PartTag> _uI_PartTags = new();
        public ObservableCollectionWithSelectedItem<UI_PartTag> UI_PartTags
        {
            get => _uI_PartTags;
            set => this.RaiseAndSetIfChanged(ref _uI_PartTags, value);
        }

        private UI_Session _uI_Session;
        public UI_Session UI_Session
        {
            get => _uI_Session;
            set => this.RaiseAndSetIfChanged(ref _uI_Session, value);
        }

        public UI_SessionPart()
        {
            
        }
    }
}
