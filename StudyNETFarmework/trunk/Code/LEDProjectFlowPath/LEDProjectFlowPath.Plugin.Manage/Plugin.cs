using System;
using System.IO;
using System.Reflection;

namespace LEDProjectFlowPath.Plugin.Manage
{
    /// <summary>
    /// 插件
    /// </summary>
    public class Plugin
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; internal set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string PluginName { get; internal set; }

        /// <summary>
        /// 插件显示名称
        /// </summary>
        public virtual string PluginFriendlyName { get; internal set; }
        /// <summary>
        /// 分组
        /// </summary>
        public string Group { get; internal set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Description { get; internal set; }
        /// <summary>
        /// 预览照片地址
        /// </summary>
        public string PreviewImagePath { get; internal set; }
        /// <summary>
        /// 索引视图地址
        /// </summary>
        public string IndexViewPath { get; internal set; }
        /// <summary>
        /// 插件主文件（DLL）名称
        /// </summary>
        public string PluginFileName { get { return ReferencedAssembly == null ? string.Empty : ReferencedAssembly.GetName().Name; } }
        /// <summary>
        /// 插件控制器命名空间
        /// </summary>
        //public string ControllerNamespace { get; internal set; }

        /// <summary>
        /// 插件主文件文件信息
        /// </summary>
        public virtual FileInfo PluginFileInfo { get; internal set; }
        /// <summary>
        /// 插件程序集
        /// </summary>
        public virtual Assembly ReferencedAssembly { get; internal set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public virtual int DisplayOrder { get; internal set; }
        /// <summary>
        /// 是否已安装
        /// </summary>
        public virtual bool Installed { get; private set; }
        /// <summary>
        /// 初始化类型
        /// </summary>
        public Type InitType { get; internal set; }
        /// <summary>
        /// 初始化方法
        /// </summary>
        public string InitMethod { get; internal set; }
        /// <summary>
        /// 安装
        /// </summary>
        /// <returns></returns>
        public Plugin Install()
        {
            if (InitType == null)
                throw new Exception($"插件【{PluginName} {ID}】未设置初始化类型");
            if(InitMethod==null)
                throw new Exception($"插件【{PluginName} {ID}】未设置初始化方法");

            var method = InitType.GetMethod(InitMethod);
            if(method == null)
                throw new Exception($"插件【{PluginName} {ID}】 初始化类型【{InitType}】不包含初始化方法【{InitMethod}】");
            try
            {
                var obj = ReferencedAssembly.CreateInstance(InitType.FullName);
                method.Invoke(obj, null);
            }catch(Exception ex)
            {
                throw new Exception($"安装插件【{PluginName} {ID}】失败，失败原因：", ex);
            }
            Installed = true;
            return this;
        }
    }
}