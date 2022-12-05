using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelAIHelper.ViewModels;

namespace NovelAIHelper.DataBase.Entities.DataBase
{
    internal class Dir : ViewModelBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int?    ParentId { get; set; }
        public string  Name     { get; set; }
        public string? Link     { get; set; }


        [ForeignKey("ParentId")] public virtual Dir ParentDir { get; set; }

        public virtual ICollection<Dir> ChildDirs { get; set; } = new List<Dir>();

        public virtual ICollection<TagDir> TagDirs { get; set; } = new List<TagDir>();
    }
}
