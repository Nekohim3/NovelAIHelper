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
        public string  Name { get; set; }
        public string? Link { get; set; }

        public virtual ICollection<TagDir> TagDirs { get; set; } = new List<TagDir>();
    }
}
