using Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

[assembly: PreApplicationStartMethod(typeof(LEDProjectFlowPath.Plugin.Manage.PluginManageInit), "Initialize")]
namespace LEDProjectFlowPath.Plugin.Manage
{
    /// <summary>
    /// 插件管理初始化类
    /// </summary>
    public class PluginManageInit
    {
        static ILog log = LogManager.GetLogger<PluginManageInit>();
        static PluginManageInit()
        {
            PluginFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/plugins"));
            ShadowCopyFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/plugins/temp"));
            log.Info("构建插件管理初始化类");
        }

        /// <summary>
        ///  插件目录
        ///  从源插件目录用于镜像
        /// </summary>
        /// <remarks>
        /// 此文件夹可以包含用于组织插件类型的子文件夹
        /// </remarks>
        private static readonly DirectoryInfo PluginFolder;

        /// <summary>
        /// 这个目录镜像插件目录的DLLs，用于运行应用程序
        /// </summary>
        private static readonly DirectoryInfo ShadowCopyFolder;

        public static void Initialize()
        {
            log.Info("初始化插件");
            Directory.CreateDirectory(ShadowCopyFolder.FullName);

            // 清理插件
            foreach (var f in ShadowCopyFolder.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                f.Delete();
            }

            // 镜像文件
            foreach (var plug in PluginFolder.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                var di = Directory.CreateDirectory(Path.Combine(ShadowCopyFolder.FullName, plug.Directory.Name.Equals("bin", StringComparison.CurrentCultureIgnoreCase) ? string.Empty : plug.Directory.Name));
                // 注意：你不能将插件DLL重命名为其他名字，因为如果程序是显示(manifest)的，程序名称将是部分（part）的，将会失败
                // NOTE: You cannot rename the plugin DLL to a different name, it will fail because the assembly name is part if it's manifest
                // 引用如何加载程序集: http://msdn.microsoft.com/en-us/library/yx7xezcf
                // (a reference to how assemblies are loaded: http://msdn.microsoft.com/en-us/library/yx7xezcf )
                File.Copy(plug.FullName, Path.Combine(di.FullName, plug.Name), true);
            }

            // 现在，我们需要告诉BulidManager我们的插件dll存在并引用它们。
            // 我们需要考虑不同程序加载上下文，这些上下文在本文中定义如下:
            // http://blogs.msdn.com/b/suzcook/archive/2003/05/29/57143.aspx
            // Now, we need to tell the BuildManager that our plugin DLLs exists and to reference them.
            // There are different Assembly Load Contexts that we need to take into account which 
            // are defined in this article here:
            // http://blogs.msdn.com/b/suzcook/archive/2003/05/29/57143.aspx

            // 这将把插件程序集放在“Load”上下文中
            // 这可以工作，但需要在web.config中定义一个“probing”文件夹
            // * This will put the plugin assemblies in the 'Load' context
            // This works but requires a 'probing' folder be defined in the web.config
            foreach (var fileInfo in
                ShadowCopyFolder
                .GetFiles("*.dll", SearchOption.AllDirectories))
            {
                if (Environment.Is64BitProcess && fileInfo.FullName.EndsWith("x86.dll", true, null)
                    || (!Environment.Is64BitProcess) && fileInfo.FullName.EndsWith("amd64.dll", true, null))
                    continue;
                try
                {
                    PluginManager.AddPlugins(fileInfo, Assembly.Load(AssemblyName.GetAssemblyName(fileInfo.FullName)));
                }
                catch (BadImageFormatException ex)
                {
                    log.Error(ex);
                }
            }
            PluginManager.InstallAllPlugins();
        }
    }
}
