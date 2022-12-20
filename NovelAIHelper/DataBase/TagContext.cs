﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelAIHelper.DataBase.Entities.DataBase;

namespace NovelAIHelper.DataBase
{
    public class TagContext : DbContext
    {
        public DbSet<Dir>         Dirs         { get; set; }
        public DbSet<Tag>         Tags         { get; set; }
        public DbSet<Session>     Sessions     { get; set; }
        public DbSet<SessionPart> SessionParts { get; set; }
        public DbSet<PartTag>     PartTags     { get; set; }

        public TagContext(bool resetDatabase = false)
        {
            if(resetDatabase)
                Database.EnsureDeleted();
            Database.EnsureCreated();
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=NovelAIHelper;Username=postgres;Password=KuroNeko2112");
        }
    }
}
