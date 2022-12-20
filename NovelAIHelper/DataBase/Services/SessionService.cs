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
    internal class SessionService
    {
        public SessionService()
        {
            g.Ctx ??= new TagContext();
        }

        public IEnumerable<UI_Session> GetAll()
        {
            return g.Ctx.Sessions.ProjectTo<UI_Session>(UI_Session.Map).AsEnumerable().OrderBy(x => x.Name);
        }

        #region Add/Save/Remove

        public bool Save(UI_Session session)
        {
            if (session.Id == 0)
                return Add(session);
            var d = g.Ctx.Sessions.Where(x => x.Id == session.Id).FirstOrDefault();
            d.Name    = session.Name;
            d.Comment = session.Comment;
            return g.Ctx.SaveChanges() > 0;
        }

        public bool SaveRange(IList<UI_Session> session)
        {
            var newDirs = session.Where(x => x.Id == 0);
            var oldDirs = session.Where(x => x.Id != 0);
            g.Ctx.Sessions.AddRange(newDirs);
            g.Ctx.Sessions.AttachRange(oldDirs);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool Add(UI_Session session)
        {
            g.Ctx.Sessions.Add(session);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool AddRange(IEnumerable<UI_Session> session)
        {
            g.Ctx.Sessions.AttachRange(session);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool Remove(UI_Session session)
        {
            var d = g.Ctx.Sessions.Entry(session);
            d.State = EntityState.Deleted;
            return g.Ctx.SaveChanges() > 0;
        }

        public bool RemoveRange(IEnumerable<UI_Session> session)
        {
            foreach (var x in session)
                g.Ctx.Sessions.Entry(x).State = EntityState.Deleted;
            return g.Ctx.SaveChanges() > 0;
        }

        #endregion
    }
}
