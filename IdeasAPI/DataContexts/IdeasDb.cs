using IdeasAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdeasAPI.DataContexts
{
    public class IdeasDb : DbContext
    {
        public IdeasDb()
            :base("DefaultConnection")
        {

        }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}