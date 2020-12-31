using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;

namespace MVC_Pipeline.App_Start.Filters
{
    /// <summary>
    /// 身份验证过滤器
    /// </summary>
    public class MyAuthenticationFilter : IAuthenticationFilter
    {
        readonly ILog log = LogManager.GetLogger("MyAuthenticationFilter(身份过滤器)");
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            log.Info("OnAuthentication 身份认证  在进行授权时调用");
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            log.Info("OnAuthenticationChallenge 在身份认证质询时调用");
        }
    }
}