using System.Web.Http;
using WebApi.App_Start;

namespace WebApi
{
    /// <summary>
    /// WebApi配置
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 注册配置
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            ////启动数据库上下文服务
            //SqlServerContextService.Start();
            // Web API 路由
            config.MapHttpAttributeRoutes();//开启特性路由

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ExceptionFilterAttibute());//注册全局异常筛选器
        }
    }
}
