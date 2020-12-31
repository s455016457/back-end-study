using ASP.NET.MVC.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace ASP.NET.MVC.Areas.Restaurant.Controllers
{
    /// <summary>
    /// 餐厅评论控制器
    /// </summary>
    public class RestaurantReviewController : Controller
    {
        MVCDbContext dbContext = new MVCDbContext();
        // GET: Restaurant/RestaurantReview
        public ActionResult Index([Bind(Prefix = "id")] int restaurantId)
        {
            var model = dbContext.Restaurants.Find(restaurantId);
            if (model != null)
                return View(model);
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Create([Bind(Prefix = "id")] int restaurantId)
        {
            return View(new Models.RestaurantReview { RestaurantId=restaurantId});
        }

        [HttpPost]
        public ActionResult Create([Form] Models.RestaurantReview review)
        {
            if (ModelState.IsValid)
            {
                dbContext.RestaurantReviews.Add(review);
                dbContext.SaveChanges();
                return RedirectToAction("Index", new { id = review.RestaurantId });
            }
            return View(review);
        }

        public ActionResult Edit(int id)
        {
            return View(dbContext.RestaurantReviews.Find(id));
        }

        [HttpPost]
        public ActionResult Edit([Form] Models.RestaurantReview review)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(review).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index", new { id = review.RestaurantId });
            }
            return View(review);
        }

        protected override void Dispose(bool disposing)
        {
            if (dbContext != null)
                dbContext.Dispose();
            dbContext = null;
            base.Dispose(disposing);
        }
    }
}