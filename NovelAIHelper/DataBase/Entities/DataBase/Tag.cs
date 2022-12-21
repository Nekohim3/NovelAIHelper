using NovelAIHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.DataBase;

public class Tag : IdEntity
{
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

    private string? _comment;
    public string? Comment
    {
        get => _comment;
        set => this.RaiseAndSetIfChanged(ref _comment, value);
    }
    
    public                               int IdDir { get; set; }
    [ForeignKey("IdDir")] public virtual Dir Dir   { get; set; }

    public virtual ICollection<PartTag> SessionTags { get; set; } = new List<PartTag>();

    public Tag()
    {
        _name    = "";
        _link    = null;
        _comment = null;
    }

    public Tag(string name, string? link = null, string? comment = null)
    {
        _name    = name;
        _link    = link;
        _comment = comment;
    }

    public Tag(string name, int dirId, string? link = null, string? comment = null)
    {
        _name    = name;
        IdDir   = dirId;
        _link    = link;
        _comment = comment;
    }
    
    protected bool Equals(Tag tag)
    {
        var eq = base.Equals(tag);
        if (!eq)
            return tag.Name == Name;
        return false;
    }

    public override int GetHashCode() { return HashCode.Combine(Id, Name); }
}