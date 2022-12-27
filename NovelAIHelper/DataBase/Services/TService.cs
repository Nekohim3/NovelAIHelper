using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NovelAIHelper.DataBase.Entities.DataBase;

namespace NovelAIHelper.DataBase.Services;

public class TService<T, TT> where T : IdEntity where TT : T
{
    static TService() { g.AddMapper<T, TT>(); }

    public virtual List<TT> GetAll()
    {
        using var ctx = new TagContext();
        return ctx.Set<T>().ProjectTo<TT>(g.GetMap<TT>()).AsEnumerable().ToList();
    }
    
    public virtual bool AddRange(IList<TT> ttList) => SaveRange(ttList.OfType<T>().ToList());
    public virtual bool SaveRange(IList<TT> ttList) => SaveRange(ttList.OfType<T>().ToList());
    public virtual bool DeleteRange(IList<TT> ttList) => DeleteRange(ttList.OfType<T>().ToList());

    public virtual bool Add(T t) => Save(t);
    public virtual bool Save(T t)
    {
        using var ctx = new TagContext();
        if (t.Id == 0)
            ctx.Add(t);
        else
        {
            //try
            //{
            //    ctx.Attach(t);
            //}
            //catch (Exception e)
            //{
                
            //}
            ctx.Update(t);
        }

        return ctx.SaveChanges() > 0;
    }

    public virtual bool AddRange(IList<T> tList) => SaveRange(tList);
    public virtual bool SaveRange(IList<T> tList)
    {
        using var ctx  = new TagContext();
        var       news = tList.Where(x => x.Id == 0);
        var       olds = tList.Where(x => x.Id != 0);
        ctx.AddRange(news);
        //ctx.AttachRange(olds);
        ctx.UpdateRange(olds);
        return ctx.SaveChanges() > 0;
    }

    public virtual bool Delete(T t)
    {
        using var ctx = new TagContext();
        ctx.Entry(t).State = EntityState.Deleted;
        return ctx.SaveChanges() > 0;
    }

    public virtual bool DeleteRange(IList<T> list)
    {
        using var ctx                                = new TagContext();
        foreach (var x in list) ctx.Entry(x).State = EntityState.Deleted;
        return ctx.SaveChanges() > 0;
    }
}