using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NovelAIHelper.DataBase.Entities.DataBase;
using ReactiveUI;
using NovelAIHelper.Utils.Collections;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.Utils;

namespace NovelAIHelper.DataBase.Entities.ViewModels
{
    public class UI_Tag : Tag, ISelected, IDraggable
    {
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

        private int _strength;
        public int Strength
        {
            get => _strength;
            set
            {
                this.RaiseAndSetIfChanged(ref _strength, value);
                this.RaisePropertyChanged("DisplayInTagGridName");
            }
        }

        private UI_Dir _uI_Dir;
        public UI_Dir UI_Dir
        {
            get => _uI_Dir;
            set => this.RaiseAndSetIfChanged(ref _uI_Dir, value);
        }

        public string SearchedDisplay => GetSearchedDisplay();
        public string DisplayInTagGridName => $"{(Strength > 0 ? new string('(', Strength) : new string('{', Math.Abs(Strength)))}{Name}{(Strength > 0 ? new string(')', Strength) : new string('}', Math.Abs(Strength)))}";
        
        public ReactiveCommand<Unit, Unit> AddStrCmd { get; }
        public ReactiveCommand<Unit, Unit> SubStrCmd      { get; }
        
        public UI_Tag()
        {
            AddStrCmd = ReactiveCommand.Create(OnAddStr);
            SubStrCmd = ReactiveCommand.Create(OnSubStr);
        }
        
        public UI_Tag(string name, string? link = null, string? comment = null) : base(name, link, comment)
        {
            AddStrCmd = ReactiveCommand.Create(OnAddStr);
            SubStrCmd = ReactiveCommand.Create(OnSubStr);
        }

        public UI_Tag(string name, int dirId, string? link = null, string? comment = null) : base(name, dirId, link, comment)
        {
            AddStrCmd = ReactiveCommand.Create(OnAddStr);
            SubStrCmd = ReactiveCommand.Create(OnSubStr);
        }

        private string GetSearchedDisplay()
        {
            var str = $"{Name}\n  {UI_Dir.SearchedDisplay}";
            return str.TrimEnd('\n');
        }

        private void OnAddStr()
        {
            if(Strength < 5)
                Strength++;
        }

        private void OnSubStr()
        {
            if(Strength > -5)
                Strength--;
        }
    }
}
