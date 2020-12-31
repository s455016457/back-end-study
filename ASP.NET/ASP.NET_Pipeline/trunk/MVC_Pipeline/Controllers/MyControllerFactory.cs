using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace MVC_Pipeline.Controllers
{
    /// <summary>
    /// 自定义控制器创建方法
    /// 参考文献
    /// https://www.codeproject.com/articles/599189/defaultcontrollerfactory-in-asp-net-mvc
    /// </summary>
    public class MyControllerFactory : DefaultControllerFactory
    {
        ILog log = LogManager.GetLogger("MyControllerFactory");
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            log.Info("CreateController  使用指定的请求上下文来创建指定的控制器。");
            return base.CreateController(requestContext, controllerName);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            log.Info("GetControllerInstance  检索指定请求上下文和控制器类型的控制器实例。");
            return base.GetControllerInstance(requestContext, controllerType);
        }

        protected override SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, Type controllerType)
        {
            log.Info("SessionStateBehavior  返回控制器的会话行为。");
            return base.GetControllerSessionBehavior(requestContext, controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            log.Info("ReleaseController  释放指定的控制器。");
            base.ReleaseController(controller);
        }

        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            log.Info("GetControllerType  检索指定名称和请求上下文的控制器类型。");
            return base.GetControllerType(requestContext, controllerName);
        }
    }
}