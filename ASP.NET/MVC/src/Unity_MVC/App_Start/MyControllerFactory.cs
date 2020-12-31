using Common.Logging;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Unity_MVC.App_Start
{
    /// <summary>
    /// 控制器工厂类
    /// <see cref="http://www.prideparrot.com/blog/archive/2012/8/controller_lookup_and_default_controller_factory"/>
    /// 源码地址：https://github.com/aspnet/AspNetWebStack/blob/749384689e027a2fcd29eb79a9137b94cea611a8/src/System.Web.Mvc/DefaultControllerFactory.cs
    /// </summary>
    public class MyControllerFactory : DefaultControllerFactory 
    {
        ILog log = LogManager.GetLogger<MyControllerFactory>();
        /// <summary>
        /// 创建控制器
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            LogDebug($"CreateController：创建控制器【{controllerName}】");
            return base.CreateController(requestContext, controllerName);
        }
        /// <summary>
        /// 获取控制器实例
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            LogDebug($"GetControllerInstance：获取控制器实例【{controllerType.FullName}】");
            return base.GetControllerInstance(requestContext, controllerType);
        }
        /// <summary>
        /// 返回控制器的会话行为
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        protected override SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, Type controllerType)
        {
            LogDebug($"SessionStateBehavior：返回控制器的会话行为【{controllerType.FullName}】");
            return base.GetControllerSessionBehavior(requestContext, controllerType);
        }
        /// <summary>
        /// 获取控制器类型
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            LogDebug($"GetControllerType：获取控制器类型【{controllerName}】");
            return base.GetControllerType(requestContext, controllerName);
        }
        /// <summary>
        /// 释放控制器
        /// </summary>
        /// <param name="controller"></param>
        public override void ReleaseController(IController controller)
        {
            LogDebug($"GetControllerType：释放控制器【{controller.GetType().FullName}】");
            base.ReleaseController(controller);
        }

        private void LogDebug(object message, string typeName = "")
        {
            if (!string.IsNullOrWhiteSpace(typeName))
                log.Debug($"=====================BEGIN {typeName}=============================");

            log.Debug(message);

            if (!string.IsNullOrWhiteSpace(typeName))
                log.Debug($"=====================END {typeName}=============================");
            
        }
    }
}