using ASP.NET.MVC.App_Start.Filter;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MyActionFilterAttribute());
        }
    }
}
