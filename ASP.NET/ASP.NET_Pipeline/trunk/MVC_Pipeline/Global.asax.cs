using Common.Logging;
using MVC_Pipeline.App_Start;
using MVC_Pipeline.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_Pipeline
{
    public class MvcApplication : System.Web.HttpApplication
    {
        ILog log = LogManager.GetLogger<MvcApplication>();
        protected void Application_Start()
        {
            log.Info("begin application start");
            ControllerBuilder.Current.SetControllerFactory(typeof(MyControllerFactory)); // 自定义控制器工程
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterRoutes(GlobalFilters.Filters);
            log.Info("end application start");
        }
        public override void Init()
        {
            log.Info("begin application Init");
            base.Init();
            HttpRequestLogger.Repositories.DefaultRequestLoggerRepository.SetConnectionString("Data Source=.;Initial Catalog=SWERP_ORDER;User ID=sa;Password=sa@2008;");
            HttpRequestLogger.Config.HttpRequestLoggerConfig.CreateRequestLoggerRepositoryFunc = () => new Repositories.FileRequestLoggerRepository();
            log.Info("end application Init");
        }

        public override void Dispose()
        {
            log.Info(" dispose application ");
            base.Dispose();
        }
    }
}
