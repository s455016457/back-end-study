using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StudyEntityFramework2
{
    public class BlogggingContext:DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //将user.DisplayName的值存入Display_Name列中
            modelBuilder.Entity<User>().Property(u => u.DisplayName).HasColumnName("Display_Name");
        }
    }
}
