using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;

namespace Unity_MVC.App_Start.Filters
{
    /// <summary>
    /// 授权控制器
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        ILog log = LogManager.GetLogger<AuthorizationFilter>();
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            /**
            If(AuthorizationContext.Result == null){
                do NextFilter;
            }else{
                调用 IAuthenticationFilter 中的 OnAuthenticationChallenge 方法
            }
    **/

            log.Debug("OnAuthorization：授权验证");
            if (filterContext.RequestContext.HttpContext.Request.QueryString["未知授权"] != null && filterContext.RequestContext.HttpContext.Request.QueryString["未知授权"].Length > 0)
            {
                filterContext.Result = new HttpUnauthorizedResult("未知授权");
            }
        }
    }
}