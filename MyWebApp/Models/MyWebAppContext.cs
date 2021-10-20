using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    public class MyWebAppContext : DbContext
    {
        public MyWebAppContext(DbContextOptions<MyWebAppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().ToTable("UserInfos");
        }
    }
}
