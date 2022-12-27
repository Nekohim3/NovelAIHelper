using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Avalonia.Animation;
using NovelAIHelper.ViewModels;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.DataBase;

public class GroupTag : IdEntity
{
    private int _order;
    public int Order
    {
        get => _order;
        set => this.RaiseAndSetIfChanged(ref _order, value);
    }

    private int _strength;
    public int Strength
    {
        get => _strength;
        set => this.RaiseAndSetIfChanged(ref _strength, value);
    }

    public                                int   IdGroup { get; set; }
    [ForeignKey("IdGroup")] public virtual Group Group   { get; set; }

    public                               int IdTag { get; set; }
    [ForeignKey("IdTag")] public virtual Tag Tag   { get; set; }

    public GroupTag()
    {
        _order    = 0;
        _strength = 0;
    }

    public GroupTag(int order = 0, int strength = 0)
    {
        _order    = order;
        _strength = strength;
    }

    public GroupTag(Tag tag, int order = 0, int strength = 0)
    {
        _order = order;
        _strength = strength;
        Tag = tag;
        IdTag = tag.Id;
    }

    public GroupTag(int idTag, int order = 0, int strength = 0)
    {
        _order    = order;
        _strength = strength;
        IdTag     = idTag;
    }

    protected bool Equals(GroupTag groupTag)
    {
        var eq = base.Equals(groupTag);
        if (!eq)
            return groupTag.Order == Order && groupTag.Strength == Order;
        return false;
    }

    public override int GetHashCode() { return HashCode.Combine(Id, Order, Strength); }
}