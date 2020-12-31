using LEDProjectFlowPath.Plugin.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Web.Compilation;
using Common.Logging;

namespace LEDProjectFlowPath.Plugin.Manage
{
    /// <summary>
    /// 插件管理者
    /// </summary>
    public class PluginManager
    {
        private static List<Plugin> _plugins = new List<Plugin>();

        static ILog _log = LogManager.GetLogger<PluginManager>();
        /// <summary>
        /// 获取所有插件
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyList<Plugin> GetPlugins()
        {
            return _plugins.AsReadOnly();
        }

        /// <summary>
        /// 添加插件
        /// </summary>
        /// <param name="plugin"></param>
        internal static void AddPlugin(Plugin plugin)
        {
            _plugins.Add(plugin);
        }
        /// <summary>
        /// 添加插件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="assembly"></param>
        public static void AddPlugins(FileInfo fileInfo, Assembly assembly)
        {
            BuildManager.AddReferencedAssembly(assembly);
            if (assembly.GetCustomAttribute<PluginAssmeblyAttribute>() == null) return;
            foreach (var type in assembly.GetExportedTypes())
            {
                var pluginAttribute = type.GetCustomAttribute<PluginAttribute>();
                if (pluginAttribute == null) continue;
                try
                {
                    AddPlugin(new Plugin
                    {
                        ID = pluginAttribute.ID,
                        PluginFriendlyName = pluginAttribute.PluginFriendlyName,
                        Description = pluginAttribute.Description,
                        DisplayOrder = pluginAttribute.DisplayOrder,
                        Group = pluginAttribute.Group,
                        IndexViewPath = pluginAttribute.IndexViewPath,
                        PluginFileInfo = fileInfo,
                        PluginName = pluginAttribute.Name,
                        PreviewImagePath = pluginAttribute.PreviewImagePath,
                        ReferencedAssembly = assembly,
                        InitMethod = pluginAttribute.InitMethod,
                        InitType = pluginAttribute.InitType
                    });
                    _log.Info($"成功加载插件【{pluginAttribute.Name} {pluginAttribute.ID}】");
                }
                catch (Exception ex)
                {
                    _log.Error($"加载插件【{type}】失败，失败原因：", ex);
                }
            }
        }
        /// <summary>
        /// 安装所有插件
        /// </summary>
        public static void InstallAllPlugins()
        {
            foreach(var plugin in _plugins)
            {
                try
                {
                    InstallPlugin(plugin);
                }catch(Exception ex)
                {
                    _log.Error(ex);
                }
            }
        }

        public static void InstallPlugin(Plugin plugin)
        {
            if (!plugin.Installed)
            {
                plugin.Install();
                _log.Info($"成功安装插件【{plugin.PluginName} {plugin.ID}】");
            }
            else
            {
                _log.Info($"插件【{plugin.PluginName} {plugin.ID}】已经安装");
            }
        }

        public static void RemovePlugins(Plugin plugin)
        {
            _plugins.Remove(plugin);
        }
    }
}
