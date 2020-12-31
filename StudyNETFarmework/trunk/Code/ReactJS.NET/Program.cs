using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;
using React;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactJS.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialize();

            ReactSiteConfiguration.Configuration
                .SetReuseJavaScriptEngines(false)
                .AddScript("Sample.jsx");

            JsEngineSwitcher.Current.DefaultEngineName = ChakraCoreJsEngine.EngineName;
            JsEngineSwitcher.Current.EngineFactories.AddChakraCore();

            var environment = ReactEnvironment.Current;


            Console.WriteLine($"ReactJS.NET 版本：{environment.EngineVersion}"); 
            var component = environment.CreateComponent("HelloWorld", new { name = "Daniel" });
            // renderServerOnly omits the data-reactid attributes
            var html = component.RenderHtml(renderServerOnly: true);
            var babel = ReactEnvironment.Current.Babel;
            var result = babel.Transform("class Foo { }");
            Console.WriteLine(value: $"class Foo {{ }} 转成ES5 {result}");
            Console.WriteLine(html);
            Console.ReadKey();
        }

        private static void Initialize()
        {
            Initializer.Initialize(registration => registration.AsSingleton());
            var container = React.AssemblyRegistration.Container;
            // Register some components that are normally provided by the integration library
            // (eg. React.AspNet or React.Web.Mvc4)
            container.Register<ICache, NullCache>();
            container.Register<IFileSystem, SimpleFileSystem>();
        }
    }
}
