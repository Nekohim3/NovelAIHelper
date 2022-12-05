using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using NovelAIHelper.DataBase.Entities.ViewModels;

namespace NovelAIHelper.DataBase.Services
{
    [SuppressMessage("ReSharper", "ReplaceWithSingleCallToCount")]
    internal class DirService
    {
        private TagContext _ctx;

        public DirService()
        {
            _ctx = new TagContext();
        }

        public DirService(TagContext ctx)
        {
            _ctx = ctx;
        }

        #region Get

        public IEnumerable<UI_Dir> GetAllDirs()
        {
            return _ctx.Dirs.ProjectTo<UI_Dir>(UI_Dir.Map).AsEnumerable();
        }

        public IEnumerable<UI_Dir> GetTopDirs()
        {
            var q = _ctx.Dirs.Include(x => x.TagDirs);
            return _ctx.Dirs.Include("TagDirs").Where(x => x.ParentId == null).ProjectTo<UI_Dir>(UI_Dir.Map).AsEnumerable();
        }

        public IEnumerable<UI_Dir> GetSubDirs(UI_Dir dir, bool includeSelf = true)
        {
            var dirs    = GetAllDirs().ToList();
            var newList = new List<UI_Dir>();
            if (includeSelf)
                newList.Add(dir);
            GetSubDirs(dir, dirs, newList);
            return newList;
        }

        private void GetSubDirs(UI_Dir dir, List<UI_Dir> dbList, List<UI_Dir> newList)
        {
            var subs = dbList.Where(x => x.ParentId == dir.Id).ToList();
            newList.AddRange(subs);
            foreach (var x in subs)
                GetSubDirs(x, dbList, newList);
        }

        #endregion

        #region Add/Save

        public bool Save(UI_Dir dir)
        {
            if (dir.Id == 0)
                return Add(dir);
            _ctx.Dirs.Attach(dir);
            return _ctx.SaveChanges() > 0;
        }

        public bool SaveRange(IList<UI_Dir> dirs)
        {
            var newDirs = dirs.Where(x => x.Id == 0);
            var oldDirs = dirs.Where(x => x.Id != 0);
            _ctx.Dirs.AddRange(newDirs);
            _ctx.Dirs.AttachRange(oldDirs);
            return _ctx.SaveChanges() > 0;
        }

        public bool Add(UI_Dir dir)
        {
            _ctx.Dirs.Add(dir);
            return _ctx.SaveChanges() > 0;
        }

        public bool AddRange(IList<UI_Dir> dirs)
        {
            _ctx.Dirs.AttachRange(dirs);
            return _ctx.SaveChanges() > 0;
        }

        #endregion
    }
}
