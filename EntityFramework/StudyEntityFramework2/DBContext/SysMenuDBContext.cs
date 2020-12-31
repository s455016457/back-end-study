using StudyEntityFramework2.Model;
using StudyEntityFramework2.Model.Mapping;
using System;
using System.Data.Entity;

namespace StudyEntityFramework2.DBContext
{
    public class SysMenuDBContext:DbContext
    {
        public SysMenuDBContext(string nameOrConnectionString) :base(nameOrConnectionString)
        {
            Console.WriteLine(Configuration.ProxyCreationEnabled);
            Console.WriteLine(Configuration.LazyLoadingEnabled);
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }
        public IDbSet<SysFunctionAggregate> SysFunctions { get; set; }
        public IDbSet<MenuGroup> MenuGroups { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new SysFunctionMapping());
            modelBuilder.Configurations.Add(new MenuGroupMapping());
        }
    }    
}
