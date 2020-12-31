using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDProjectFlowPath.Core.Interface
{
    /// <summary>
    /// 项目流程
    /// </summary>
    public interface IProjectFlow : IWorkTask
    {
        /// <summary>
        /// 工作流程集合
        /// </summary>
        IReadOnlyList<IWorkFlow> IFlows { get; }
        /// <summary>
        /// 项目流程状态
        /// </summary>
        ProjectFlowStatus ProjectFlowStatus { get; }
        /// <summary>
        /// 允许修正工作任务
        /// </summary>
        /// <param name="workTask"></param>
        void AllowCorrectionWorkTask(IWorkTask workTask);

        /// <summary>
        /// 获取工作流程后正在执行或已执行完成的工作任务
        /// </summary>
        /// <param name="workTask"></param>
        /// <returns></returns>
        List<IWorkTask> GetNextWorkTasksExcecutingOrExecuted(IWorkTask workTask)
    }

    /// <summary>
    /// 项目流程状态
    /// </summary>
    public enum ProjectFlowStatus
    {
        /// <summary>
        /// 通过
        /// </summary>
        Pass = 1,
        /// <summary>
        /// 正在执行
        /// </summary>
        Executing = 0,
        /// <summary>
        /// 不通过
        /// </summary>
        NoPass = -1,
    }
}
