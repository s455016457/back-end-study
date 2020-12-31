using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Example1.DDD.Repostory;
using Example1.Models.Po;

namespace Namespace
{
    public class PoController : Controller
    {

        MyDbContext DbContext;
        public PoController(MyDbContext dbContext){
            this.DbContext=dbContext;
        }

        public IActionResult Index()
        {
            return View(new IndexViewModel().Init(DbContext).Search());
        }
    }
}