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
    [SuppressMessage("ReSharper", "ReplaceWithSingleCallToCount")]
    internal class DirService
    {
        //public List<UI_Dir> Tree { get; set; } = new List<UI_Dir>();

        public DirService()
        {
            if(g.Ctx != null)
                g.Ctx.Dispose();
            g.Ctx = new TagContext();
        }

        public DirService(TagContext ctx)
        {
            if (g.Ctx != null)
                g.Ctx.Dispose();
            g.Ctx = ctx;
        }

        #region Get
        
        public IEnumerable<UI_Dir> GetAllDirs()
        {
            return g.Ctx.Dirs.Include(x => x.Tags).ThenInclude(x => x.Dirs).Include(x => x.ChildDirs).ProjectTo<UI_Dir>(UI_Dir.Map).AsEnumerable();
        }

        public IEnumerable<UI_Dir> GetTopDirs()
        {
            return g.Ctx.Dirs.Include(x => x.Tags).ThenInclude(x => x.Dirs).Include(x => x.ChildDirs).ProjectTo<UI_Dir>(UI_Dir.Map).AsEnumerable().Where(x => !x.ParentId.HasValue);
        }

        

        //public IEnumerable<UI_Dir> GetSubDirs(UI_Dir dir, bool includeSelf = true)
        //{
        //    var dirs    = GetAllDirs().ToList();
        //    var newList = new List<UI_Dir>();
        //    if (includeSelf)
        //        newList.Add(dir);
        //    GetSubDirs(dir, dirs, newList);
        //    return newList;
        //}

        //private void GetSubDirs(UI_Dir dir, List<UI_Dir> dbList, List<UI_Dir> newList)
        //{
        //    var subs = dbList.Where(x => x.ParentId == dir.Id).ToList();
        //    newList.AddRange(subs);
        //    foreach (var x in subs)
        //        GetSubDirs(x, dbList, newList);
        //}

        #endregion

        #region Add/Save

        public bool Save(UI_Dir dir)
        {
            if (dir.Id == 0)
                return Add(dir);
            g.Ctx.Dirs.Attach(dir);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool SaveRange(IList<UI_Dir> dirs)
        {
            var newDirs = dirs.Where(x => x.Id == 0);
            var oldDirs = dirs.Where(x => x.Id != 0);
            g.Ctx.Dirs.AddRange(newDirs);
            g.Ctx.Dirs.AttachRange(oldDirs);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool Add(UI_Dir dir)
        {
            g.Ctx.Dirs.Add(dir);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool AddRange(IList<UI_Dir> dirs)
        {
            g.Ctx.Dirs.AttachRange(dirs);
            return g.Ctx.SaveChanges() > 0;
        }

        public bool Remove(UI_Dir dir)
        {
            var d = g.Ctx.Dirs.Entry(dir);
            d.State = EntityState.Deleted;
            return g.Ctx.SaveChanges() > 0;
        }

        public bool RemoveRange(IList<UI_Dir> dirs)
        {
            g.Ctx.Dirs.RemoveRange(dirs);
            return g.Ctx.SaveChanges() > 0;
        }

        #endregion
    }
}
