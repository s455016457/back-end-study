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
    /// 身份验证过滤器
    /// </summary>
    public class AuthenticationFilter : IAuthenticationFilter
    {
        ILog log = LogManager.GetLogger<AuthenticationFilter>();
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            log.Debug("OnAuthentication：开始身份验证");
            if (filterContext.RequestContext.HttpContext.Request.QueryString["未知身份"] != null&& filterContext.RequestContext.HttpContext.Request.QueryString["未知身份"] .Length>0)
            {
                // 设置身份验证失败
                // 程序调用 OnAuthenticationChallenge 方法
                filterContext.Result = new HttpUnauthorizedResult("未知身份");
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            log.Debug("OnAuthentication：身份验证质询");
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                var result = filterContext.Result as HttpUnauthorizedResult;
                log.Debug($"OnAuthentication：{result.StatusDescription}");
            }
        }
    }
}