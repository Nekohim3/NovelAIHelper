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
    public class Session : IdEntity
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string? _comment;

        public string? Comment
        {
            get => _comment;
            set => this.RaiseAndSetIfChanged(ref _comment, value);
        }
        
        public virtual ICollection<SessionPart> Parts { get; set; } = new List<SessionPart>();

        public Session()
        {
            _name    = "";
            _comment = null;
        }

        public Session(string name, string? comment = null)
        {
            _name    = name;
            _comment = comment;
        }

        protected bool Equals(Session session)
        {
            var eq = base.Equals(session);
            if (!eq)
                return session.Name == Name;
            return false;
        }

        public override int GetHashCode() { return HashCode.Combine(Id, Name); }
    }
}
