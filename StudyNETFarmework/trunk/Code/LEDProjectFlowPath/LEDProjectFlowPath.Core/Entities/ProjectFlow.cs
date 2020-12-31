using LEDProjectFlowPath.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LEDProjectFlowPath.Core.Entities
{
    /// <summary>
    /// 项目流程
    /// </summary>
    public class ProjectFlow : BaseWorkTask, IProjectFlow
    {
        private List<IWorkFlow> _workTasks = new List<IWorkFlow>();
        private ProjectFlowStatus projectFlowStatus = ProjectFlowStatus.Executing;

        #region 成员属性
        public IReadOnlyList<IWorkFlow> IFlows { get { return _workTasks.AsReadOnly(); } }

        public override bool TaskCompletion
        {
            get; protected set;
        }

        public ProjectFlowStatus ProjectFlowStatus
        {
            get { return projectFlowStatus; }
            private set
            {
                projectFlowStatus = value;
                TaskCompletion = value == ProjectFlowStatus.NoPass || value == ProjectFlowStatus.Pass;
            }
        }
        #endregion

        #region 构造函数
        private ProjectFlow() { }
        public ProjectFlow(Guid ID, string Name, Guid LeaderId, List<IWorkFlow> workTasks) : base(ID, Name)
        {
            _workTasks = workTasks;
            foreach (var item in _workTasks)
            {
                item.OnWorkTaskStatusChange += Item_OnWorkTaskStatusChange;
            }
        }
        #endregion

        #region 私有方法
        private void Item_OnWorkTaskStatusChange(IWorkTask workTask)
        {
            if ((workTask as IWorkFlow).WorkFlowStatus == WorkFlowStatus.NoPass)
            {
                ProjectFlowStatus = ProjectFlowStatus.NoPass;
                UpdateWorkTaskStatus(WorkTaskStatus.Executed);
            }
            else if ((workTask as IWorkFlow).WorkFlowStatus == WorkFlowStatus.Pass)
            {
                ExecutNextWorkTask();
            }

            if (_workTasks.All(p => (p as IWorkFlow).WorkFlowStatus == WorkFlowStatus.Pass))
            {
                ProjectFlowStatus = ProjectFlowStatus.Pass;
                UpdateWorkTaskStatus(WorkTaskStatus.Executed);
            }
        }

        /// <summary>
        /// 获取工作流程后正在执行或已执行完成的工作任务
        /// </summary>
        /// <param name="workTask"></param>
        /// <returns></returns>
        public List<IWorkTask> GetNextWorkTasksExcecutingOrExecuted(IWorkTask workTask)
        {
            List<IWorkTask> workTasks = new List<IWorkTask>();
            if (workTask is IDataReport)
            {
                var workFlow = _workTasks.FirstOrDefault(p => p.GetChildrenWorkTasks().Any(t => t.ID.Equals(workTask.ID)));
                if (workFlow != null)
                {
                    var list = workFlow.GetChildrenWorkTasks().SkipWhile(p => p.ID == workTask.ID)
                    .Where(p => p.WorkTaskStatus == WorkTaskStatus.Executed || p.WorkTaskStatus == WorkTaskStatus.Executing)
                        .ToList();

                    workTasks.AddRange(list);
                    if (workFlow.WorkTaskStatus == WorkTaskStatus.Executed)
                    {
                        workTasks.Add(workFlow);
                        workTasks.AddRange(GetNextWorkTasksExcecutingOrExecuted(workFlow));
                    }
                    return workTasks;
                }

                IReviewReport reviewReport = null;
                foreach (var item in _workTasks)
                {
                    reviewReport = item.GetChildrenWorkTasks()
                        .FirstOrDefault(p=>p is IReviewReport&&(p as IReviewReport).DataReports.Any(t=>t.ID==workTask.ID)) as IReviewReport;

                    if (reviewReport != null)
                        break;
                }

                if (reviewReport == null) throw new Exception($"工作流程【{workTask.Name}】不属于项目流程【{Name}】！");

                if (reviewReport.DataReportType == DataReportType.Serial)
                {
                    var list = reviewReport.DataReports.SkipWhile(p => p.ID == workTask.ID)
                        .Where(p => p.WorkTaskStatus == WorkTaskStatus.Executed || p.WorkTaskStatus == WorkTaskStatus.Executing)
                            .ToList<IWorkTask>();
                    workTasks.AddRange(list);
                }
                if (reviewReport.WorkTaskStatus == WorkTaskStatus.Executed)
                {
                    workTasks.Add(reviewReport);
                    workTasks.AddRange(GetNextWorkTasksExcecutingOrExecuted(reviewReport));
                }
                return workTasks;
            }

            if (workTask is IReviewReport)
            {
                var workFlow = _workTasks.FirstOrDefault(p => p.GetChildrenWorkTasks().Any(t => t.ID.Equals(workTask.ID)));
                var list = workFlow.GetChildrenWorkTasks().SkipWhile(p => p.ID == workTask.ID)
                    .Where(p => p.WorkTaskStatus == WorkTaskStatus.Executed|| p.WorkTaskStatus == WorkTaskStatus.Executing)
                    .ToList();

                if (workFlow == null) throw new Exception($"工作流程【{workTask.Name}】不属于项目流程【{Name}】！");

                workTasks.AddRange(list);
                if (workFlow.WorkTaskStatus == WorkTaskStatus.Executed)
                {
                    workTasks.Add(workFlow);
                    workTasks.AddRange(GetNextWorkTasksExcecutingOrExecuted(workFlow));
                }
                return workTasks;
            }

            if (workTask is IWorkFlow)
            {
                return _workTasks.SkipWhile(p => p.ID == workTask.ID)
                    .Where(p => p.WorkTaskStatus == WorkTaskStatus.Executed || p.WorkTaskStatus == WorkTaskStatus.Executing)
                    .ToList<IWorkTask>();
            }

            return workTasks;
        }

        /// <summary>
        /// 执行下个工作任务
        /// </summary>
        private void ExecutNextWorkTask()
        {
            if (_workTasks.Any(p => p.WorkTaskStatus == WorkTaskStatus.Executing))
            {
                throw new Exception("存在正在执行的工作任务，无法执行下个工作任务！");
            }
            var nexWorkTask = _workTasks.FirstOrDefault(p => p.WorkTaskStatus == WorkTaskStatus.Waiting);
            if (nexWorkTask != null)
                nexWorkTask.Start();
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 更新工作任务状态
        /// </summary>
        /// <param name="workTaskStatus"></param>
        protected override void UpdateWorkTaskStatus(WorkTaskStatus workTaskStatus)
        {
            if (workTaskStatus == WorkTaskStatus.Executing && (_workTasks == null || _workTasks.Count == 0))
                throw new Exception("项目流程不包含工作流程，不能开启");

            var oldWorkTaskStatus = WorkTaskStatus;
            base.UpdateWorkTaskStatus(workTaskStatus);

            if (oldWorkTaskStatus == WorkTaskStatus.Waiting && workTaskStatus == WorkTaskStatus.Executing)
            {
                var firstWorkTask = _workTasks.FirstOrDefault();
                firstWorkTask.Start();
            }
        }

        public override string GetFlowTypeName()
        {
            return "项目流程";
        }

        public void AllowCorrectionWorkTask(IWorkTask workTask)
        {
            if (WorkTaskStatus == WorkTaskStatus.Executed)
            {
                AllowCorrectionWorkTask();
            }
        }

        public override void AllowCorrectionWorkTask()
        {
            base.AllowCorrectionWorkTask();
            projectFlowStatus = ProjectFlowStatus.Executing;
        }
        #endregion
    }
}
