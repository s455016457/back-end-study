using System;

namespace LEDProjectFlowPath.Plugin.SDK
{
    /// <summary>
    /// 插件属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false, Inherited = true)]
    public class PluginAttribute : Attribute
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string PluginFriendlyName { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 预览图片地址
        /// </summary>
        public string PreviewImagePath { get; set; }
        /// <summary>
        /// 索引视图地址
        /// </summary>
        public string IndexViewPath { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 初始化类型
        /// </summary>
        public Type InitType { get; set; }
        /// <summary>
        /// 初始化方法
        /// </summary>
        public string InitMethod { get; set; }

        public PluginAttribute(string ID,string Name,string PreviewImagePath, string IndexViewPath,Type InitType, string InitMethod)
        {
            this.ID = Guid.Parse(ID);
            this.Name = Name;
            this.PreviewImagePath = PreviewImagePath;
            this.IndexViewPath = IndexViewPath;
            this.InitType = InitType;
            this.InitMethod = InitMethod;
        }
    }

    /// <summary>
    /// 插件程序集属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public class PluginAssmeblyAttribute : Attribute
    {

    }
}
