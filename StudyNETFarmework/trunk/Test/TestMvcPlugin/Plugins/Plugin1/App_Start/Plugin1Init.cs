using Common.Logging;
using LEDProjectFlowPath.Plugin.SDK;
using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

[assembly:PluginAssmebly]
namespace Plugin1.App_Start
{
    [Plugin("67BC831F-2230-4204-94E0-F23629FA738D", "测试插件1", "/Plugins/Plugin1/Content/Images/preview.png", "Plugins/Plugin_1/Plugin1/View1", typeof(Plugin1Init), "Initialize")]
    public class Plugin1Init
    {
        ILog log = LogManager.GetLogger<Plugin1Init>();
        public Plugin1Init()
        {
        }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name
        {
            get
            {
                return "Plugin_1";
            }
        }

        public void Initialize()
        {
            log.Debug($"插件{Name}开始启动");
            RegRoute();
            RegViewEngine();
            log.Debug($"插件{Name}已启动");
        }


        /// <summary>
        /// 注册视图引擎
        /// </summary>
        private void RegViewEngine()
        {
            ViewEngines.Engines.Add(new RazorViewEngine { AreaViewLocationFormats = new[] { "~/Plugins/" + Name + "/Views/{1}/{0}.cshtml" } });
        }

        /// <summary>
        /// 注册插件路由
        /// </summary>
        public void RegRoute()
        {
            RouteTable.Routes.MapRoute(
                "Plugin1",
                "Plugins/" + Name + "/{controller}/{action}/{id}",
                new { controller = "home", action = "Index", id = UrlParameter.Optional }).DataTokens["area"] =
                Name;
        }
    }
}