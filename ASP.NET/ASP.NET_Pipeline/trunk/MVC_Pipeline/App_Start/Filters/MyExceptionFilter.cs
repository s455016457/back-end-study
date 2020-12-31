using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Pipeline.App_Start.Filters
{
    public class MyExceptionFilter : IExceptionFilter
    {
        readonly ILog log = LogManager.GetLogger("MyExceptionFilter(异常过滤器)");
        public void OnException(ExceptionContext filterContext)
        {
            log.Info("OnException 出现异常时调用");
        }
    }
}