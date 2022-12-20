using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NovelAIHelper.ViewModels;
using ReactiveUI;

namespace NovelAIHelper.DataBase.Entities.DataBase;

public class PartTag : IdEntity
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

    public                                int         IdPart { get; set; }
    [ForeignKey("IdPart")] public virtual SessionPart Part   { get; set; }

    public                               int IdTag { get; set; }
    [ForeignKey("IdTag")] public virtual Tag Tag   { get; set; }

    public PartTag()
    {
        _order    = 0;
        _strength = 0;
    }

    public PartTag(int order, int strength)
    {
        _order    = order;
        _strength = strength;
    }

    protected bool Equals(PartTag partTag)
    {
        var eq = base.Equals(partTag);
        if (!eq)
            return partTag.Order == Order && partTag.Strength == Order;
        return false;
    }

    public override int GetHashCode() { return HashCode.Combine(Id, Order, Strength); }
}