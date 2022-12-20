﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelAIHelper.ViewModels;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.DataBase;

public class Dir : IdEntity
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

    public                                  int? IdParent  { get; set; }
    [ForeignKey("IdParent")] public virtual Dir? ParentDir { get; set; }

    public virtual ICollection<Dir> ChildDirs { get; set; } = new List<Dir>();
    public virtual ICollection<Tag> Tags      { get; set; } = new List<Tag>();

    public Dir()
    {
        _name    = "";
        _link    = null;
        _comment = null;
    }

    public Dir(string name, int? idParent = null, string? link = null, string? comment = null)
    {
        _name     = name;
        IdParent = idParent;
        _link     = link;
        _comment  = comment;
    }

    protected bool Equals(Dir dir)
    {
        var eq = base.Equals(dir);
        if (!eq)
            return dir.Name == Name;
        return false;
    }

    public override int GetHashCode() { return HashCode.Combine(Id, Name); }
}