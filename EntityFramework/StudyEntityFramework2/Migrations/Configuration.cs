namespace StudyEntityFramework2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StudyEntityFramework2.BlogggingContext>
    {
        public Configuration()
        {
            //获取或设置指示迁移数据库时是否可使用自动迁移，默认为False运行时会出错，设置为ture才可以运行
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(StudyEntityFramework2.BlogggingContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
