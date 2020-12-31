using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Pipeline.App_Start.Filters
{
    /// <summary>
    /// 结果过滤器
    /// </summary>
    public class MyResultFilter : IResultFilter
    {
        ILog log = LogManager.GetLogger("MyResultFilter(结果过滤器)");
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            log.Info("OnResultExecuted 在执行由操作方法返回的操作结果后调用 ,在写入http Response后执行");
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            log.Info("OnResultExecuting 在执行由操作方法返回的操作结果前调用 ，在找到view或写入http response前执行");
        }
    }
}