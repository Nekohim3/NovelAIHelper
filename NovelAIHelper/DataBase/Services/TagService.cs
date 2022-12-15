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
namespace NovelAIHelper.DataBase.Services
{
    internal class TagService
    {
        public TagService()
        {
            if (g.Ctx != null)
                g.Ctx.Dispose();
            g.Ctx = new TagContext();
        }

        public TagService(TagContext ctx)
        {
            if (g.Ctx != null)
                g.Ctx.Dispose();
            g.Ctx = ctx;
        }

        #region Get

        public IEnumerable<UI_Tag> GetAllTags()
        {
            return g.Ctx.Tags.Include(x => x.Dir).ProjectTo<UI_Tag>(UI_Tag.Map).AsEnumerable().OrderBy(x => x.Name);
        }

        #endregion

        #region Add/Save/Remove

        public bool Save(UI_Tag tag)
        {
            if (tag.Id == 0)
                return Add(tag);
            var t = g.Ctx.Tags.Where(x => x.Id == tag.Id).FirstOrDefault();
            t.Name = tag.Name;
            return g.Ctx.SaveChanges() > 0;
        }

        public bool SaveRange(IList<UI_Tag> tags)
        {
            var newTags = tags.Where(x => x.Id == 0);
            var oldTags = tags.Where(x => x.Id != 0);
            g.Ctx.Tags.AddRange(newTags);
            g.Ctx.Tags.AttachRange(oldTags);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool Add(UI_Tag tag)
        {
            g.Ctx.Tags.Add(tag);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool AddRange(IList<UI_Tag> tags)
        {
            g.Ctx.Tags.AttachRange(tags);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool Remove(UI_Tag tag)
        {
            var d = g.Ctx.Tags.Entry(tag);
            d.State = EntityState.Deleted;
            return g.Ctx.SaveChanges() > 0;
        }

        public bool RemoveRange(IList<UI_Tag> tags)
        {
            foreach (var x in tags)
                g.Ctx.Tags.Entry(x).State = EntityState.Deleted;
            return g.Ctx.SaveChanges() > 0;
        }

        #endregion
    }
}
