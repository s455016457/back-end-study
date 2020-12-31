using LEDProjectFlowPath.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LEDProjectFlowPath.Core
{
    public class FlowHelper
    {
        /// <summary>
        /// 获取项目流程中指定工作任务的下个工作任务
        /// </summary>
        /// <param name="projectFlow">项目流程</param>
        /// <param name="workTask">指定工作任务</param>
        /// <returns></returns>
        public static IWorkTask GetNextWorkTask(IProjectFlow projectFlow, IWorkTask workTask)
        {
            if (projectFlow.IFlows == null) return null;
            var children = projectFlow.IFlows.ToList();

            var index = children.ToList().FindIndex(p => p.ID.Equals(workTask.ID));
            if (0 <= index && index < children.Count - 1)
            {
                return children[index + 1];
            }

            foreach (var flow in children)
            {
                var wt = GetNextWorkTask(flow, workTask);
                if (wt != null) return wt;

                /**
                 * 工作流程子工作任务中包含指定工作任务，且未找到下个工作任务
                 * 则返回下个工作流程
                 * */
                if (HasWorkTask(flow.GetChildrenWorkTasks(), workTask))
                {
                    index = children.IndexOf(flow);
                    if (0 <= index && index < children.Count - 1)
                    {
                        return children[index + 1];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取工作流程中指定工作任务的下个工作任务
        /// </summary>
        /// <param name="reviewReport">工作流程</param>
        /// <param name="workTask">指定工作任务</param>
        /// <returns></returns>
        public static IWorkTask GetNextWorkTask(IWorkFlow workFlow, IWorkTask workTask)
        {
            var children = workFlow.GetChildrenWorkTasks();
            var index = children.ToList().FindIndex(p => p.ID.Equals(workTask.ID));
            if (0 <= index && index < children.Count-1)
            {
                return children[index + 1];
            }

            var reviewReports = children.Where(p => p is IReviewReport).Select(p => p as IReviewReport).ToList();

            foreach (var item in reviewReports)
            {
                var wt = GetNextWorkTask(item, workTask);
                if (wt != null) return wt;
            }
            return null;
        }
        /// <summary>
        /// 获取评审报告中指定工作任务的下个工作任务
        /// </summary>
        /// <param name="reviewReport">评审报告</param>
        /// <param name="workTask">指定工作任务</param>
        /// <returns></returns>
        public static IWorkTask GetNextWorkTask(IReviewReport reviewReport, IWorkTask workTask)
        {
            if (reviewReport.DataReports == null) return null;

            var dataReports = reviewReport.DataReports.ToList();
            if (dataReports.Any(p => p.ID == workTask.ID))
            {
                if (reviewReport.DataReportType == DataReportType.Parallel)
                {
                    return reviewReport;  // 在审批报告中，审批报告是子工作任务的下个流程
                }
                else
                {
                    var index = dataReports.FindIndex(p => p.ID.Equals(workTask.ID));
                    if (0 <= index && index < dataReports.Count - 1)
                    {
                        return dataReports[index + 1];
                    }
                    else if (index == dataReports.Count - 1)
                    {
                        return reviewReport; // 在审批报告中，审批报告是子工作任务的下个流程
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 工作任务集合中包含工作任务
        /// </summary>
        /// <param name="workTasks">工作任务集合</param>
        /// <param name="workTask">指定的动作任务</param>
        /// <returns></returns>
        public static bool HasWorkTask(IEnumerable<IWorkTask> workTasks, IWorkTask workTask)
        {
            return workTasks.Any(p => p.ID == workTask.ID);
        }

    }
}
