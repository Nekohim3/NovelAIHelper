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
using NovelAIHelper.Utils;

namespace NovelAIHelper.DataBase.Services
{
    public class DirService : TService<Dir, UI_Dir>//, IMapped
    {
        //#region Get

        //public IEnumerable<UI_Dir> GetAllDirs()
        //{
        //    return g.Ctx.Dirs.ProjectTo<UI_Dir>(UI_Dir.Map).AsEnumerable().OrderBy(x => x.Name);
        //}

        //#endregion

        //#region Add/Save/Remove

        //public bool Save(UI_Dir dir)
        //{
        //    if (dir.Id == 0)
        //        g.Ctx.Dirs.Add(dir);
        //    else
        //        g.Ctx.Attach(dir);
        //    return g.Ctx.SaveChanges() > 0;
        //}

        //public bool SaveRange(IList<UI_Dir> dirs)
        //{
        //    var newDirs = dirs.Where(x => x.Id == 0);
        //    var oldDirs = dirs.Where(x => x.Id != 0);
        //    g.Ctx.Dirs.AddRange(newDirs);
        //    g.Ctx.Dirs.AttachRange(oldDirs);
        //    return g.Ctx.SaveChanges() > 0;
        //}

        //public bool Remove(UI_Dir dir)
        //{
        //    var d = g.Ctx.Dirs.Entry(dir);
        //    d.State = EntityState.Deleted;
        //    return g.Ctx.SaveChanges() > 0;
        //}

        //public bool RemoveRange(IEnumerable<UI_Dir> dirs)
        //{
        //    foreach (var x in dirs)
        //        g.Ctx.Dirs.Entry(x).State = EntityState.Deleted;
        //    return g.Ctx.SaveChanges() > 0;
        //}

        //#endregion


        //public static void SetMapper() { g.AddMapper<Dir, UI_Dir>(); }
    }
}
