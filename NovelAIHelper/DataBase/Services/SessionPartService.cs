using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using NovelAIHelper.DataBase.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using NovelAIHelper.DataBase.Entities.DataBase;
using HarfBuzzSharp;

namespace NovelAIHelper.DataBase.Services
{
    public class SessionPartService : TService<SessionPart, UI_SessionPart>
    {
        //public SessionTagService()
        //{
        //    g.SessionCtx ??= new TagContext();
        //}

        //public IEnumerable<UI_PartTag> GetAll()
        //{
        //    return g.SessionCtx.SessionTags.ProjectTo<UI_PartTag>(UI_PartTag.Map).AsEnumerable().OrderBy(x => x.Order);
        //}

        //public IEnumerable<UI_PartTag> GetAllBySessionId(int id)
        //{
        //    return g.SessionCtx.SessionTags.Where(x => x.IdSession == id).ProjectTo<UI_PartTag>(UI_PartTag.Map).AsEnumerable();
        //}

        //#region Add/Save/Remove

        //public bool Save(UI_PartTag sessionTag)
        //{
        //    if (sessionTag.Id == 0)
        //        return Add(sessionTag);
        //    g.SessionCtx.Attach(sessionTag);
        //    return g.SessionCtx.SaveChanges() > 0;
        //    //var d = g.Ctx.SessionTags.Where(x => x.Id == sessionTag.Id).FirstOrDefault();
        //    //d.Order    = sessionTag.Order;
        //    //d.Strength = sessionTag.Strength;
        //    //return g.Ctx.SaveChanges() > 0;
        //}

        //public bool SaveRange(IList<UI_PartTag> sessionTag)
        //{
        //    var newDirs = sessionTag.Where(x => x.Id == 0);
        //    var oldDirs = sessionTag.Where(x => x.Id != 0);
        //    g.SessionCtx.SessionTags.AddRange(newDirs);
        //    g.SessionCtx.SessionTags.AttachRange(oldDirs);
        //    return g.Ctx.SaveChanges() > 0;
        //}

        //public bool Add(UI_PartTag sessionTag)
        //{
        //    g.SessionCtx.SessionTags.Add(sessionTag);
        //    return g.SessionCtx.SaveChanges() > 0;
        //}

        //public bool AddRange(IEnumerable<UI_PartTag> sessionTag)
        //{
        //    g.SessionCtx.SessionTags.AttachRange(sessionTag);
        //    return g.SessionCtx.SaveChanges() > 0;
        //}

        //public bool Remove(UI_PartTag sessionTag)
        //{
        //    var d = g.SessionCtx.SessionTags.Entry(sessionTag);
        //    d.State = EntityState.Deleted;
        //    return g.SessionCtx.SaveChanges() > 0;
        //}

        //public bool RemoveRange(IEnumerable<UI_PartTag> sessionTag)
        //{
        //    foreach (var x in sessionTag)
        //        g.SessionCtx.SessionTags.Entry(x).State = EntityState.Deleted;
        //    return g.SessionCtx.SaveChanges() > 0;
        //}

        //#endregion
    }
}
