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

    public virtual IEnumerable<TT> GetAll() => g.Ctx.Set<T>().ProjectTo<TT>(g.GetMap<TT>()).AsEnumerable();

    //public virtual bool Add(TT tt) => Add((T)tt);
    public virtual bool AddRange(IList<TT> ttList) => SaveRange(ttList.OfType<T>().ToList());
    //public virtual bool Save(TT             tt)     => Save((T)tt);
    public virtual bool SaveRange(IList<TT> ttList) => SaveRange(ttList.OfType<T>().ToList());
    //public virtual bool Delete(TT             tt)     => Delete((T)tt);
    public virtual bool DeleteRange(IList<TT> ttList) => DeleteRange(ttList.OfType<T>().ToList());

    public virtual bool Add(T t) => Save(t);
    public virtual bool Save(T t)
    {
        if (t.Id == 0)
            g.Ctx.Add(t);
        else
        {
            try
            {
                g.Ctx.Attach(t);
            }
            catch (Exception e)
            {
                
            }
            g.Ctx.Update(t);
        }

        return g.Ctx.SaveChanges() > 0;
    }

    public virtual bool AddRange(IList<T> tList) => SaveRange(tList);
    public virtual bool SaveRange(IList<T> tList)
    {
        var news = tList.Where(x => x.Id == 0);
        var olds = tList.Where(x => x.Id != 0);
        g.Ctx.AddRange(news);
        g.Ctx.AttachRange(olds);
        g.Ctx.UpdateRange(olds);
        return g.Ctx.SaveChanges() > 0;
    }

    public virtual bool Delete(T t)
    {
        g.Ctx.Entry(t).State = EntityState.Deleted;
        return g.Ctx.SaveChanges() > 0;
    }

    public virtual bool DeleteRange(IList<T> list)
    {
        foreach (var x in list) g.Ctx.Entry(x).State = EntityState.Deleted;
        return g.Ctx.SaveChanges() > 0;
    }
}