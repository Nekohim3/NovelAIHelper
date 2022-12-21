using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelAIHelper.ViewModels;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.DataBase;

public class SessionPart : IdEntity
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

    public                                   int     IdSession { get; set; }
    [ForeignKey("IdSession")] public virtual Session Session   { get; set; }

    public virtual ICollection<PartTag> PartTags { get; set; } = new List<PartTag>();

    public SessionPart()
    {
        _name    = "";
        _comment = null;
    }

    public SessionPart(string name, string? comment = null)
    {
        _name     = name;
        _comment  = comment;
    }

    public SessionPart(string name, int idSession, string? comment = null)
    {
        _name     = name;
        IdSession = idSession;
        _comment  = comment;
    }

    protected bool Equals(SessionPart sessionPart)
    {
        var eq = base.Equals(sessionPart);
        if (!eq)
            return sessionPart.Name == Name;
        return false;
    }

    public override int GetHashCode() { return HashCode.Combine(Id, Name); }
}