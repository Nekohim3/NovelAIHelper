using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelAIHelper.DataBase.Entities.DataBase;

namespace NovelAIHelper.DataBase
{
    internal class TagContext : DbContext
    {
        public DbSet<Dir>    Dirs    { get; set; }
        public DbSet<Tag>    Tags    { get; set; }
        public DbSet<TagDir> TagDirs { get; set; }

        public TagContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=NovelAIHelper;Username=postgres;Password=KuroNeko2112");
        }
    }
}
