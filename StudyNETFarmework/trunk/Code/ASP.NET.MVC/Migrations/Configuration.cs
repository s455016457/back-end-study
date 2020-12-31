namespace ASP.NET.MVC.Migrations
{
    using ASP.NET.MVC.Areas.Restaurant.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASP.NET.MVC.Database.MVCDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ASP.NET.MVC.Database.MVCDbContext";
        }

        protected override void Seed(ASP.NET.MVC.Database.MVCDbContext context)
        {
            var list = context.Restaurants.ToList();
            context.Restaurants.RemoveRange(list);
            context.SaveChanges();
            context.Restaurants.AddOrUpdate(r => r.Name,
                new Restaurant { City = "台湾", Country = "中国", Name = "兰州拉面" },
                new Restaurant { City = "香港", Country = "中国", Name = "北京烤鸭" },
                new Restaurant { City = "澳门", Country = "中国", Name = "苏格兰风情" },
                new Restaurant
                {
                    City = "武汉",
                    Country = "中国",
                    Name = "武汉热干面",
                    Reviews = new List<RestaurantReview>
                    {
                        new RestaurantReview{ Rating=9,Body="超级好吃",RestaurantName="张三"},
                        new RestaurantReview{ Rating=8,Body="很好吃",RestaurantName="小王"}
                    }
                },
                new Restaurant
                {
                    City = "新疆",
                    Country = "中国",
                    Name = "沙县小吃",
                    Reviews = new List<RestaurantReview>
                    {
                        new RestaurantReview{ Rating=9,Body="超级好吃",RestaurantName="小王"},
                        new RestaurantReview{ Rating=9,Body="超级好吃",RestaurantName="张三"},
                    }
                }
                );

            for (int i = 0; i < 1000; i++)
            {
                context.Restaurants.AddOrUpdate(r => r.Name, new Areas.Restaurant.Models.Restaurant
                {
                    City = "City" + i,
                    Country = "Contry" + i,
                    Name = "Name" + i,
                });
            }
        }
    }
}
