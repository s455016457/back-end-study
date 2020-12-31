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
    [Plugin("98EFAE10-3588-4812-A120-43D6DEF6FB2F", "测试插件2", "/Plugins/Plugin2/Content/Images/preview.png", "Plugins/Plugin2/Default/View1", typeof(Plugin2Init), "Initialize")]
    public class Plugin2Init
    {
        ILog log = LogManager.GetLogger<Plugin2Init>();
        public Plugin2Init()
        {
        }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name
        {
            get
            {
                return "Plugin2";
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
                "Plugin2",
                "Plugins/" + Name + "/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }).DataTokens["area"] =
                Name;
        }
    }
}