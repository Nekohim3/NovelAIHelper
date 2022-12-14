using NovelAIHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.DataBase
{
    public class Tag : ViewModelBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;       set; }

        //public string  Name { get; set; } = string.Empty;
        //public string? Link { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string? _link;

        public string? Link
        {
            get => _link;
            set => this.RaiseAndSetIfChanged(ref _link, value);
        }

        private int _dirId;

        public int DirId
        {
            get => _dirId;
            set => this.RaiseAndSetIfChanged(ref _dirId, value);
        }

        [ForeignKey("DirId")]
        public virtual Dir Dir { get; set; }

        //public virtual ICollection<Dir> Dirs { get; set; } = new List<Dir>();

        public Tag()
        {

        }

        public Tag(string name, int dirId, string? link = null)
        {
            Name  = name;
            DirId = dirId;
            Link  = link;
        }

        public Tag(string name, string? link = null)
        {
            Name  = name;
            Link  = link;
        }
    }
}
