using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Unity_MVC.App_Start.Filters
{
    public class MyActionFilterAttribute: ActionFilterAttribute
    {
        ILog log = LogManager.GetLogger<MyActionFilterAttribute>();
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            log.Debug("OnActionExecuted：在执行操作方法后由 ASP.NET MVC 框架调用。");
            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log.Debug("OnActionExecuting：在执行操作方法之前由 ASP.NET MVC 框架调用。");
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            log.Debug("OnResultExecuted： 在执行操作结果后由 ASP.NET MVC 框架调用。");
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            log.Debug("OnResultExecuted： 在执行操作结果之前由 ASP.NET MVC 框架调用。");
            base.OnResultExecuting(filterContext);
        }
    }
}