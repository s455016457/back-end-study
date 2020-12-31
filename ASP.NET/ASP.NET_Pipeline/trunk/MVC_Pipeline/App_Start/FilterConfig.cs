using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Pipeline.App_Start
{
    public class FilterConfig
    {
        public static void RegisterRoutes(GlobalFilterCollection Filters)
        {
            Filters.Add(new Filters.MyActionFilter());
            Filters.Add(new Filters.MyAuthenticationFilter());
            Filters.Add(new Filters.MyAuthorizationFilter());
            Filters.Add(new Filters.MyResultFilter());
            Filters.Add(new Filters.MyExceptionFilter());
        }
    }
}