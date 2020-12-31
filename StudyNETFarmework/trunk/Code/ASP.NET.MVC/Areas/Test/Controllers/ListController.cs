using ASP.NET.MVC.Areas.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace ASP.NET.MVC.Areas.Test.Controllers
{
    public class ListController : Controller
    {
        static IList<User> users = new List<User>();
        public ListController()
        {
            if (users.Count() == 0)
            {
                for (int i = 0; i < 1000; i++)
                {
                    users.Add(new Models.User
                    {
                        Uid = "UID" + i,
                        Email = $"Email{i}@baidu.com",
                        Enable = true,
                        LoginCout = i,
                        Name = "Name" + i,
                        Password = "Password" + i
                    });
                }
            }
        }

        //// GET: Test/List
        //public ActionResult Index()
        //{
        //    return View(users.Take(20).ToList());
        //}
        public ActionResult Index(string searchText="")
        {
            var data = from item in users
                       where item.Uid.Contains(searchText)
                          || item.Name.Contains(searchText)
                          || item.Email.Contains(searchText)
                       select item;
            data = data.Take(20).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_list", data);
            }

            return View(data);
        }

        // GET: Test/List/Details/5
        public ActionResult Details(string Uid)
        {
            return View();
        }

        // GET: Test/List/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Test/List/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/List/Edit/5
        public ActionResult Edit(string Uid)
        {
            return View();
        }

        // POST: Test/List/Edit/5
        [HttpPost]
        public ActionResult Edit(int Uid, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/List/Delete/5
        public ActionResult Delete(string Uid)
        {
            return View();
        }

        // POST: Test/List/Delete/5
        [HttpPost]
        public ActionResult Delete(string Uid, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public class SearchModel
        {
            public string searchText { get; set; }
        }
    }
}
