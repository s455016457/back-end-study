using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using React;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestReactJS.Net.MVC.App_Start
{
    public static class ReactConfig
    {
        public static void Configure()
        {
            ReactSiteConfiguration.Configuration
                .SetReuseJavaScriptEngines(true)
                //.SetAllowJavaScriptPrecompilation(true)
                .AddScript("~/Content/Sample.jsx");

            JsEngineSwitcher.Current.DefaultEngineName = V8JsEngine.EngineName;
            JsEngineSwitcher.Current.EngineFactories.AddV8();
        }
    }
}