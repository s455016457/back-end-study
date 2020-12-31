using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Pipeline.App_Start.Filters
{
    /// <summary>
    /// 授权过滤器
    /// </summary>
    public class MyAuthorizationFilter : IAuthorizationFilter
    {
        readonly ILog log = LogManager.GetLogger("MyAuthorizationFilter(授权过滤器)");
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            log.Info("OnAuthorization 授权认证  在进行授权时调用");
        }
    }
}