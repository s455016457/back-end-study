using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ASP.NET.MVC.Areas.Restaurant.Models;

namespace ASP.NET.MVC.Database
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext():base("name=DefaultConnection") { }
        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<RestaurantReview> RestaurantReviews { get; set; }
    }
}