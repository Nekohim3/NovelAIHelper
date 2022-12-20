using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelAIHelper.DataBase.Services;
using NovelAIHelper.ViewModels;

namespace NovelAIHelper.DataBase.Entities.DataBase
{
    public class IdEntity : ViewModelBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public static bool operator !=(IdEntity? a, IdEntity? b) { return !(a == b); }

        public static bool operator ==(IdEntity? a, IdEntity? b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public virtual bool Equals(object? obj)
        {
            if (obj is not IdEntity entity) return false;
            if (entity.Id          == 0 && Id == 0)
                return false;
            return entity.Id == Id;
        }

        public virtual bool Equals(IdEntity entity)
        {
            if (entity.Id          == 0 && Id == 0)
                return false;
            return entity.Id == Id;
        }

        public virtual int GetHashCode() { return HashCode.Combine(Id); }

        public bool Add() => new TService<IdEntity, IdEntity>().Add(this);
        public bool Save() => new TService<IdEntity, IdEntity>().Save(this);
        public bool Delete() => new TService<IdEntity, IdEntity>().Delete(this);
    }
}
