using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelAIHelper.ViewModels;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.DataBase;

public class Group : IdEntity
{
    private string _name;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private int _order;

    public int Order
    {
        get => _order;
        set => this.RaiseAndSetIfChanged(ref _order, value);
    }

    private string? _comment;
    public string? Comment
    {
        get => _comment;
        set => this.RaiseAndSetIfChanged(ref _comment, value);
    }

    public                                   int     IdSession { get; set; }
    [ForeignKey("IdSession")] public virtual Session Session   { get; set; }

    public virtual ICollection<GroupTag> GroupTags { get; set; } = new List<GroupTag>();

    public Group()
    {
        _name    = "";
        _comment = null;
    }

    public Group(string name, int order = 0, string? comment = null)
    {
        _name    = name;
        _comment = comment;
        _order   = order;
    }

    public Group(string name, int idSession, int order = 0, string? comment = null)
    {
        _name     = name;
        IdSession = idSession;
        _comment  = comment;
        _order = order;
    }

    protected bool Equals(Group sessionPart)
    {
        var eq = base.Equals(sessionPart);
        if (!eq)
            return sessionPart.Name == Name;
        return false;
    }

    public override int GetHashCode() { return HashCode.Combine(Id, Name); }
}