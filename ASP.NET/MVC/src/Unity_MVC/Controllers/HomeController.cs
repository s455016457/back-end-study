using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using Unity_MVC.DDD.Services.Interfaces;
using Unity;
using System.Web.Services.Description;

namespace Unity_MVC.Controllers
{
    public class HomeController : Controller
    {
        ILog log = LogManager.GetLogger<HomeController>();
        IRequestLiftTimeSerivce requestLiftTimeSerivce;

        public HomeController()
        {
            log.Debug("HomeController 构造函数：创建HomeController控制器");
        }

        public HomeController(IRequestLiftTimeSerivce requestLiftTimeSerivce)
        {
            this.requestLiftTimeSerivce = requestLiftTimeSerivce;
            log.Debug("HomeController 构造函数：创建HomeController控制器，含一个参数");
        }


        // GET: Home
        public ActionResult Index()
        {
            log.Debug("Index");

            requestLiftTimeSerivce.ToDoSomeThing();
            App_Start.UnityConfig.container.Resolve<ISingletonService>().ToDoSomeThing();

            using (var Service = App_Start.UnityConfig.container.Resolve<ITransientLifetimeService>())
            {
                Service.ToDoSomeThing();
            }

            using (var Service = App_Start.UnityConfig.container.Resolve<ITransientLifetimeService>())
            {
                Service.ToDoSomeThing();
            }

            return View();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            log.Debug("Initialize：对调用构造函数时可能不可用的数据进行初始化。");
            base.Initialize(requestContext);
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            log.Debug("BeginExecute：开始执行指定的请求上下文");
            return base.BeginExecute(requestContext, callback, state);
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            log.Debug("BeginExecuteCore：开始调用当前控制器上下文中的操作");
            return base.BeginExecuteCore(callback, state);
        }

        protected override IActionInvoker CreateActionInvoker()
        {
            log.Debug("CreateActionInvoker：创建操作调用程序");
            return base.CreateActionInvoker();
        }

        protected override ITempDataProvider CreateTempDataProvider()
        {
            log.Debug("CreateTempDataProvider：创建临时数据提供程序");
            return base.CreateTempDataProvider();
        }

        protected override void Dispose(bool disposing)
        {
            log.Debug("Dispose：释放控制器");
            base.Dispose(disposing);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            log.Debug("EndExecute：结束当前控制器上下文中的操作的调用");
            base.EndExecute(asyncResult);
        }

        protected override void EndExecuteCore(IAsyncResult asyncResult)
        {
            log.Debug("EndExecuteCore：结束执行核心");
            base.EndExecuteCore(asyncResult);
        }
        protected override void Execute(RequestContext requestContext)
        {
            log.Debug("Execute：执行指定请求上下文");
            base.Execute(requestContext);
        }

        protected override void ExecuteCore()
        {
            log.Debug("ExecuteCore：调用当前控制器上下文中的操作");
            base.ExecuteCore();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            log.Debug("OnActionExecuted：在调用操作方法后调用");
            base.OnActionExecuted(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log.Debug("OnActionExecuting：在调用操作方法前调用");
            base.OnActionExecuting(filterContext);
        }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            log.Debug("OnAuthentication：在进行身份验证时调用");
            base.OnAuthentication(filterContext);
        }

        protected override void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            log.Debug("OnAuthenticationChallenge：在进行身份验证质询时调用");
            base.OnAuthenticationChallenge(filterContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            log.Debug("OnAuthorization：在进行授权时调用");
            base.OnAuthorization(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            log.Debug("OnAuthorization：操作发生异常时调用");
            base.OnException(filterContext);
        }
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            log.Debug("OnResultExecuted：在执行由操作方法返回的操作结果后调用");
            base.OnResultExecuted(filterContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            log.Debug("OnResultExecuting：在执行由操作方法返回的操作结果前调用");
            base.OnResultExecuting(filterContext);
        }
    }
}