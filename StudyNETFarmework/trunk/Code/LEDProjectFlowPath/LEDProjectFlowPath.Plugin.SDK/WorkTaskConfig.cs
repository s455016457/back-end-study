using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDProjectFlowPath.Plugin.SDK
{
    /// <summary>
    /// 工作任务配置管理
    /// </summary>
    public static class WorkTaskConfigManage
    {
        private static List<WorkTaskConfig> _workTaskConfigs = new List<WorkTaskConfig>();
        private static object lock_obj = new object();
        public static void AddWorkTaskConfig(WorkTaskConfig workTaskEntity)
        {
            lock (lock_obj)
            {
                _workTaskConfigs.Add(workTaskEntity);
            }
        }

        /// <summary>
        /// 工作任务配置
        /// </summary>
        public static IReadOnlyList<WorkTaskConfig> WorkTaskConfigs
        {
            get
            {
                return _workTaskConfigs.AsReadOnly();
            }
        }
    }
    /// <summary>
    /// 工作任务配置
    /// </summary>
    public class WorkTaskConfig
    {
        /// <summary>
        /// 任务ID
        /// </summary>
       public Guid ID { get;private set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get;private set; }
        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName { get;private set; }
        /// <summary>
        /// 预览视图路径
        /// </summary>
        public string PreviewPath { get;private set; }
        /// <summary>
        /// 主页视图路径
        /// </summary>
        public string IndexViewPath { get;private set; }
        /// <summary>
        /// 工作任务类别
        /// </summary>
        public WorkTaskType WorkTaskType { get;private set; }

        /// <summary>
        /// 初始化工作任务配置
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="GroupName"></param>
        /// <param name="PreviewPath"></param>
        /// <param name="IndexViewPath"></param>
        /// <param name="WorkTaskType"></param>
        public WorkTaskConfig(Guid ID,string Name,string GroupName,string PreviewPath,string IndexViewPath,WorkTaskType WorkTaskType)
        {
            this.ID = ID;
            this.Name = Name;
            this.GroupName = GroupName;
            this.PreviewPath = PreviewPath;
            this.IndexViewPath = IndexViewPath;
            this.WorkTaskType = WorkTaskType;
        }
    }
    /// <summary>
    /// 工作任务类别
    /// </summary>
    public enum WorkTaskType
    {
        /// <summary>
        /// 数据报告
        /// </summary>
        DataReport=1,
        /// <summary>
        /// 评审报告
        /// </summary>
        ReviewReport=2,
    }
}
