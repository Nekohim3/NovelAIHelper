using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelAIHelper.ViewModels;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.DataBase
{
    public class PartTag : ViewModelBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private int _idPart;

        public int IdPart
        {
            get => _idPart;
            set => this.RaiseAndSetIfChanged(ref _idPart, value);
        }
        
        private int _idTag;

        public int IdTag
        {
            get => _idTag;
            set => this.RaiseAndSetIfChanged(ref _idTag, value);
        }

        private int _order;

        public int Order
        {
            get => _order;
            set => this.RaiseAndSetIfChanged(ref _order, value);
        }

        private int _strength;

        public int Strength
        {
            get => _strength;
            set => this.RaiseAndSetIfChanged(ref _strength, value);
        }

        [ForeignKey("IdPart")]
        public virtual Part Part { get; set; }

        [ForeignKey("IdTag")]
        public virtual Tag Tag { get; set; }
    }
}
