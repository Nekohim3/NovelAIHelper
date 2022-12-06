using NovelAIHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelAIHelper.DataBase.Entities.DataBase
{
    internal class Tag : ViewModelBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;       set; }

        public string  Name { get; set; } = string.Empty;
        public string? Link { get; set; }

        public virtual ICollection<Dir> Dirs { get; set; } = new List<Dir>();

        public Tag()
        {
            
        }

        public Tag(string name, string? link = null)
        {
            Name = name;
            Link = link;
        }
    }
}
