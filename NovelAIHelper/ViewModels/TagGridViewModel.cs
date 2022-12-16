using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelAIHelper.DataBase.Entities.ViewModels;
using NovelAIHelper.Utils;
using NovelAIHelper.Utils.Collections;
using ReactiveUI;

namespace NovelAIHelper.ViewModels
{
    public class TagGridViewModel : ViewModelBase
    {
        private ObservableCollectionWithSelectedItem<TagGroup> _tagGrid;

        public ObservableCollectionWithSelectedItem<TagGroup> TagGrid
        {
            get => _tagGrid;
            set => this.RaiseAndSetIfChanged(ref _tagGrid, value);
        }

        public TagGridViewModel()
        {
            
        }

        public TagGridViewModel(ObservableCollectionWithSelectedItem<TagGroup> tagGrid)
        {
            _tagGrid = tagGrid;
        }
    }
}
