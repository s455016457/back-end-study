using SignalRExercises.Areas.Sys.Models.User;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace SignalRExercises.Areas.Sys.Controllers
{
    public class UserController : Controller
    {
        // GET: Sys/User
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Register()
        {
            return View(new Register());
        }
        [HttpPost]
        public JsonResult Register([Form] Register register)
        {
            return Json(register.Save());
        }
    }
}