using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Unity_MVC.App_Start.Filters
{
    public class ActionFilter : IActionFilter
    {
        ILog log = LogManager.GetLogger<ActionFilter>();
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            log.Debug("OnActionExecuted：在执行操作方法后调用");
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log.Debug("OnActionExecuting：在执行操作方法之前调用");
        }
    }
}