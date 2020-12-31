using ASP.NET.MVC.Areas.Restaurant.Models;
using ASP.NET.MVC.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ASP.NET.MVC.Areas.Restaurant.Controllers
{
    public class RestaurantController : Controller
    {
        MVCDbContext dbContext = new MVCDbContext();

        public ActionResult Autocomplete(string term=null)
        {
            var model = dbContext.Restaurants.Where(t => t.Name.Contains(term))
                .Take(10)
                .Select(p => new { label = p.Name });

            return Json(model,JsonRequestBehavior.AllowGet);
        }

        // GET: Restaurant/RestaurantReview
        public ActionResult Index(string searchTerm="",int page=1)
        {
            var model = dbContext.Restaurants
                .Where(t => t.City.Contains(searchTerm)
                    || t.Country.Contains(searchTerm)
                    || t.Id.ToString().Contains(searchTerm)
                    || t.Name.Contains(searchTerm)
                    )
                .Select(p => new ListModel
                {
                    Id=p.Id,
                    Name=p.Name,
                    City=p.City,
                    Country=p.Country,
                    CountOfReviews = p.Reviews.Count()
                })
                .OrderBy(p=>p.Id)
                .ToPagedList(page,20);
            if (Request.IsAjaxRequest())
            {
                return View("_list", model);
            }
            return View(model);
        }

        public ViewResult Edit(int id)
        {
            return View(dbContext.Restaurants.FirstOrDefault(p => p.Id.Equals(id)));
        }
        public ViewResult Detail(int id)
        {
            return View(dbContext.Restaurants.FirstOrDefault(p => p.Id.Equals(id)));
        }
        public ViewResult Delete(int id)
        {
            var model = dbContext.Restaurants.FirstOrDefault(p => p.Id.Equals(id));
            return View(dbContext.Restaurants.Remove(model));
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