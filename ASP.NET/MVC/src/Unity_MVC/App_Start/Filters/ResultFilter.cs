using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Unity_MVC.App_Start.Filters
{
    public class ResultFilter : IResultFilter
    {
        ILog log = LogManager.GetLogger<ResultFilter>();
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            log.Debug("OnResultExecuted：在操作结果执行后调用");
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            log.Debug("OnResultExecuting：在操作结果执行之前调用");
        }
    }
}