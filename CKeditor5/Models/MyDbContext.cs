using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CKeditor5.Models
{
    public class MyDbContext:DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}