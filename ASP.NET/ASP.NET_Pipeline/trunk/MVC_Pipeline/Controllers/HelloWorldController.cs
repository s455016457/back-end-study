using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace MVC_Pipeline.Controllers
{
    /// <summary>
    /// 控制器
    /// 
    /// 控制器也是一个过滤器，在所有过滤器对应方法前执行
    /// OnActionExecuted OnResultExecuted OnException 在所有过滤器的OnActionExecuted OnResultExecuted OnException方法之后执行
    /// </summary>
    public class HelloWorldController : Controller
    {
        ILog log = LogManager.GetLogger("HelloWorldController");
        // GET: Main
        public ActionResult Index()
        {
            log.Info("执行Index Action");
            //throw new Exception("Test Error");
            return View();
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            log.Info("BeginExecute 开始执行指定的请求上下文 ");
            return base.BeginExecute(requestContext, callback, state);
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            log.Info("BeginExecuteCore 开始调用当前控制器上下文中的操作 ");
            return base.BeginExecuteCore(callback, state);
        }

        protected override ContentResult Content(string content, string contentType, Encoding contentEncoding)
        {
            log.Info("Content 使用字符串、内容类型和内容编码创建一个内容结果对象 ");
            return base.Content(content, contentType, contentEncoding);
        }

        protected override IActionInvoker CreateActionInvoker()
        {
            log.Info("CreateActionInvoker 创建操作调用程序 ");
            return base.CreateActionInvoker();
        }

        protected override ITempDataProvider CreateTempDataProvider()
        {
            log.Info("CreateTempDataProvider 创建临时数据提供程序 ");
            return base.CreateTempDataProvider();
        }

        protected override void Dispose(bool disposing)
        {
            log.Info("Dispose 释放非托管资源和托管资源（后者为可选项） ");
            base.Dispose(disposing);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            log.Info("EndExecute 结束当前控制器上下文中的操作的调用 ");
            base.EndExecute(asyncResult);
        }

        protected override void EndExecuteCore(IAsyncResult asyncResult)
        {
            log.Info("EndExecuteCore 结束执行核心 ");
            base.EndExecuteCore(asyncResult);
        }

        protected override void Execute(RequestContext requestContext)
        {
            log.Info("Execute 执行指定的请求上下文 ");
            base.Execute(requestContext);
        }

        protected override void ExecuteCore()
        {
            log.Info("ExecuteCore 执行指定的请求上下文核心 ");
            base.ExecuteCore();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log.Info("OnActionExecuting 在Action之前执行 ");
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            log.Info("OnActionExecuted 在Action之后执行 ");
            base.OnActionExecuted(filterContext);
        }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            log.Info("OnAuthentication 身份认证  在进行授权时调用");
            base.OnAuthentication(filterContext);
        }

        protected override void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            log.Info("OnAuthenticationChallenge 在身份认证质询时调用");
            base.OnAuthenticationChallenge(filterContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            log.Info("OnAuthorization 授权认证  在进行授权时调用");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            log.Info("OnException 出现异常时调用");
            base.OnException(filterContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            log.Info("OnResultExecuting 在执行由操作方法返回的操作结果前调用 ，在找到view或写入http response前执行");
            base.OnResultExecuting(filterContext);
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            log.Info("OnResultExecuted 在执行由操作方法返回的操作结果后调用 ,在写入http Response后执行");
            base.OnResultExecuted(filterContext);
        }
    }
}