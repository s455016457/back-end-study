using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDProjectFlowPath.Core.Interface
{
    /// <summary>
    /// 工作流程接口
    /// </summary>
    public interface IWorkFlow : IWorkTask
    {
        #region 成员属性
        /// <summary>
        /// 可以开启下个流程
        /// </summary>
        bool CanStartNextFlow { get; }
        /// <summary>
        /// 工作流程状态
        /// </summary>
        WorkFlowStatus WorkFlowStatus { get; }

        #endregion
        /// <summary>
        /// 子任务集合
        /// </summary>
        IReadOnlyList<IWorkTask> GetChildrenWorkTasks();
        /// <summary>
        /// 获取下个工作任务
        /// </summary>
        /// <param name="workTask"></param>
        /// <returns></returns>
        IWorkTask GetNextWorkTask(IWorkTask workTask);
    }

    /// <summary>
    /// 工作流程状态
    /// </summary>
    public enum WorkFlowStatus
    {
        /// <summary>
        /// 通过
        /// </summary>
        Pass = 1,
        /// <summary>
        /// 未决定
        /// </summary>
        Pending = 0,
        /// <summary>
        /// 不通过
        /// </summary>
        NoPass = -1,
    }
}
