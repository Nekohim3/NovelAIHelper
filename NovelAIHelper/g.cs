using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NovelAIHelper.DataBase;
using NovelAIHelper.DataBase.Entities.DataBase;
using NovelAIHelper.DataBase.Entities.ViewModels;

namespace NovelAIHelper;

public static class g
{
    private static List<(Type type, MapperConfiguration mapperConfiguration, Mapper mapper)> _mapList = new();

    public static TagContext Ctx { get; set; }

    public static void ResetCtx(bool reset = false)
    {
        if (Ctx != null) Ctx.Dispose();
        Ctx = new TagContext(reset);
    }

    static g() { Ctx = new TagContext(); }

    public static MapperConfiguration? GetMap<T>() where T : IdEntity
    {
        var mc = _mapList.FirstOrDefault(x => x.type == typeof(T));
        return mc == default ? null : mc.mapperConfiguration;
    }

    public static MapperConfiguration? GetMapper<T>() where T : IdEntity
    {
        var mc = _mapList.FirstOrDefault(x => x.type == typeof(T));
        return mc == default ? null : mc.mapperConfiguration;
    }

    public static void AddMapper<T, TT>() where T : IdEntity where TT : T
    {
        if (_mapList.Count(x => x.type == typeof(TT)) != 0) return;
        var mc = new MapperConfiguration(x => x.CreateMap<T, TT>());
        _mapList.Add((typeof(TT), mc, new Mapper(mc)));
    }
}