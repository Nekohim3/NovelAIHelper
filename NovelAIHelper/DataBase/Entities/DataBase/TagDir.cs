using NovelAIHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelAIHelper.DataBase.Entities.DataBase
{
    internal class TagDir : ViewModelBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;    set; }
        public int DirId { get; set; }
        public int TagId { get; set; }

        [ForeignKey("DirId")]
        public virtual Dir Dir { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }

        public TagDir()
        {
            
        }

        public TagDir(Dir dir, Tag tag)
        {
            Dir = dir;
            Tag = tag;
        }

        public TagDir(int dirId, int tagId)
        {
            DirId = dirId;
            TagId = tagId;
        }
    }
}
