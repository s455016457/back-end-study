using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;
using React;
using System;

namespace TestReactJS.NET
{
    class Program
    {
        private const string stringScript = @"
class SkinnedMesh extends THREE.Mesh {
  constructor(geometry, materials) {
    super(geometry, materials);

    this.idMatrix = SkinnedMesh.defaultMatrix();
    this.bones = [];
    this.boneMatrices = [];
    //...
  }
  update(camera) {
    //...
    super.update();
  }
  static defaultMatrix() {
    return new THREE.Matrix4();
  }
}
";

        public static string StringScript => stringScript;

        static void Main(string[] args)
        {
            Initialize();

            ReactSiteConfiguration.Configuration
                .SetReuseJavaScriptEngines(false);

            JsEngineSwitcher.Current.DefaultEngineName = ChakraCoreJsEngine.EngineName;
            JsEngineSwitcher.Current.EngineFactories.AddChakraCore();

            var environment = ReactEnvironment.Current;


            Console.WriteLine($"ReactJS.NET 版本：{environment.EngineVersion}");
            var babel = ReactEnvironment.Current.Babel;

            string script = StringScript;

            var result = babel.Transform(script);
            Console.WriteLine(value: $"{script} 转成ES5 ");
            Console.WriteLine(result);
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
