using EFCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DBContext
{
    public class SysMenuDBContext : DbContext
    {
        public SysMenuDBContext()
        {
            //Database.Log = new Action<string>(p => LogHelper.Create().WriteLog(p));
        }
        public DbSet<MenuGroup> MenuGroups { get; set; }
        public DbSet<SysFunctionAggregate> SysFunctionAggregates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source =.; initial catalog = DBStudy; user id = swerp; password = swerp2008;");
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuGroup>().ToTable("MenuGroup");
            modelBuilder.Entity<MenuGroup>().HasKey("MenuGroup_Id", "SysFunction_Id");
            modelBuilder.Entity<MenuGroup>().Property("MenuGroup_Id").HasColumnName("MenuGroup_Id");
            modelBuilder.Entity<MenuGroup>().Property("SysFunction_Id").HasColumnName("SysFunction_Id");
            modelBuilder.Entity<MenuGroup>().Property("_name").HasColumnName("Name");
            modelBuilder.Entity<MenuGroup>().HasOne(typeof(SysFunctionAggregate), "SysFunction").WithMany("MenuGroups").HasForeignKey("SysFunction_Id");

            //modelBuilder.Entity<MenuGroup>().HasOne(s => s.SysFunction).WithMany(s => s.MenuGroups).HasForeignKey(s => s.SysFunction_Id);

            modelBuilder.Entity<SysFunctionAggregate>().ToTable("SysFunction");
            modelBuilder.Entity<SysFunctionAggregate>().HasKey("SysFunction_Id");
            modelBuilder.Entity<SysFunctionAggregate>().Property("SysFunction_Id").HasColumnName("SysFunction_Id");
            modelBuilder.Entity<SysFunctionAggregate>().Property("_name").HasColumnName("Name");
            modelBuilder.Entity<SysFunctionAggregate>().HasMany(typeof(MenuGroup), "MenuGroups").WithOne("SysFunction").HasForeignKey("SysFunction_Id");

            //modelBuilder.Entity<SysFunctionAggregate>().HasMany(s => s.MenuGroups).WithOne(s => s.SysFunction).HasForeignKey(s=>s.SysFunction_Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
