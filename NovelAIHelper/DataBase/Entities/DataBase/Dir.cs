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
    public class Dir : ViewModelBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int?    ParentId { get; set; }

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

        [ForeignKey("ParentId")] public virtual Dir? ParentDir { get; set; }
        
        public virtual ICollection<Dir> ChildDirs  { get; set; } = new List<Dir>();

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();


        public Dir()
        {
            
        }

        public Dir(string name, int? parentId = null, string? link = null)
        {
            Name     = name;
            ParentId = parentId;
            Link     = link;
        }


        public static bool operator !=(Dir? a, Dir? b) => !(a == b);

        public static bool operator ==(Dir? a, Dir? b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Dir dir) return false;
            if (dir.Id          == 0 && Id == 0)
                return dir.Name == Name;
            return dir.Id == Id;

        }

        protected bool Equals(Dir dir)
        {
            if (dir.Id          == 0 && Id == 0)
                return dir.Name == Name;
            return dir.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
