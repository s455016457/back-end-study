using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET.MVC.App_Start.Filter
{
    public class MyActionFilterAttribute: ActionFilterAttribute
    {
        ILog _log = LogManager.GetLogger<MyActionFilterAttribute>();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _log.Debug("MyActionFilterAttribute 执行");

            IDictionary<string, object> keyValuePairs = filterContext.ActionParameters;

            Dictionary<string, object> newKeyValuePairs = new Dictionary<string, object>();
            foreach (var keyValue in keyValuePairs)
            {
                _log.Debug($"参数：{keyValue.Key}，值：{keyValue.Value}");
                newKeyValuePairs.Add(keyValue.Key, keyValue.Value!=null&&keyValue.Value.GetType().Equals(typeof(string))? keyValue.Value.ToString()+"ActionFilter": keyValue.Value);
            }

            filterContext.ActionParameters = newKeyValuePairs;
            base.OnActionExecuting(filterContext);
        }
    }
}