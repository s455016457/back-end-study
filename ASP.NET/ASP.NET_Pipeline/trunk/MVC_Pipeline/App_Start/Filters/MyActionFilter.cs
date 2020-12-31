using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Pipeline.App_Start.Filters
{
    /// <summary>
    /// 操作过滤器
    /// </summary>
    public class MyActionFilter : IActionFilter
    {
        ILog log = LogManager.GetLogger("MyActionFilter(操作过滤器)");
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            log.Info("OnActionExecuted 在Action之后执行");
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log.Info("OnActionExecuting 在Action之前执行");
        }
    }
}