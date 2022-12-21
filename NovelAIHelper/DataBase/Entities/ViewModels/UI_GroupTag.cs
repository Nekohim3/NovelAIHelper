using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.ViewModels
{
    public class UI_GroupTag : GroupTag, ISelected
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        private UI_Tag _uI_Tag;
        public UI_Tag UI_Tag
        {
            get => _uI_Tag;
            set
            {
                this.RaiseAndSetIfChanged(ref _uI_Tag, value);
                IdTag = _uI_Tag.Id;
            }
        }

        public UI_GroupTag()
        {

        }

        public UI_GroupTag(int order = 0, int strength = 0) : base(order, strength)
        {

        }

        public UI_GroupTag(int idTag, int order = 0, int strength = 0) : base(idTag, order, strength)
        {

        }

        public UI_GroupTag(UI_Tag tag, int order = 0, int strength = 0) : base(tag, order, strength)
        {
            UI_Tag = tag;
        }
    }
}
