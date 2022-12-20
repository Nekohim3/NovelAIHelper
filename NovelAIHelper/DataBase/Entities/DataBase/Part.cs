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
    public class Part : ViewModelBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private int _idSession;

        public int IdSession
        {
            get => _idSession;
            set => this.RaiseAndSetIfChanged(ref _idSession, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
        
        [ForeignKey("IdSession")]
        public virtual Session Session { get; set; }
        
        public virtual ICollection<PartTag> PartTags { get; set; } = new List<PartTag>();
    }
}
