using LEDProjectFlowPath.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDProjectFlowPath.Core.Entities
{
    /// <summary>
    /// 工作流程
    /// </summary>
    public class WorkFlow :BaseWorkTask, IWorkFlow
    {
        #region 私有属性
        private List<IWorkTask> _workTasks = new List<IWorkTask>();
        private WorkFlowStatus workFlowStatus = WorkFlowStatus.Pending;
        #endregion
        #region 成员属性
        /// <summary>
        /// 子任务集合
        /// </summary>
        public IReadOnlyList<IWorkTask> GetChildrenWorkTasks()
        {
            return _workTasks.AsReadOnly();
        }
        /// <summary>
        /// 任务已完成
        /// </summary>
        public override bool TaskCompletion { get; protected set; }

        /// <summary>
        /// 可以开启下个流程
        /// </summary>
        public bool CanStartNextFlow
        {
            get
            {
                if (WorkFlowStatus == WorkFlowStatus.Pass) return true;
                else
                {
                    foreach (var item in GetChildrenWorkTasks())
                    {
                        if (item is IReviewReport)
                        {
                            var _reviewReport = item as IReviewReport;
                            if (_reviewReport.AllowParentFlowProceedNextProcess
                                && _reviewReport.ReviewReportStatus == ReviewReportStatus.Pass)
                                return true;
                        }
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// 工作流程状态
        /// </summary>
        public WorkFlowStatus WorkFlowStatus
        {
            get { return workFlowStatus; }
            private set
            {
                workFlowStatus = value;
                TaskCompletion = value == WorkFlowStatus.Pass || value == WorkFlowStatus.NoPass;
            }
        }
        #endregion
        #region  构造函数
        private WorkFlow() { }
        public WorkFlow(Guid ID, string Name, Guid LeaderId, List<IWorkTask> workTasks)
            :base(ID,Name)
        {
            _workTasks = workTasks;
            foreach (var item in _workTasks)
            {
                item.OnWorkTaskStatusChange += Item_OnWorkTaskStatusChange;
            }
        }
        #endregion
        #region  私有方法
        private void Item_OnWorkTaskStatusChange(IWorkTask workTask)
        {
            if (WorkFlowStatus != WorkFlowStatus.Pending)
                return;
            if (workTask is IReviewReport)
            {
                var reviewReport = workTask as IReviewReport;
                if (reviewReport.ReviewReportStatus == ReviewReportStatus.NoPass)
                {
                    if (WorkTaskStatus != WorkTaskStatus.Executing)
                        throw new Exception("工作任务不是执行状态，不将工作流程状态改为不通过状态！");

                    WorkFlowStatus = WorkFlowStatus.NoPass;
                    UpdateWorkTaskStatus(WorkTaskStatus.Executed);
                }
                else if (reviewReport.ReviewReportStatus == ReviewReportStatus.Pending)
                {
                    WorkFlowStatus = WorkFlowStatus.Pending;
                    if (WorkTaskStatus != WorkTaskStatus.Executed)
                        UpdateWorkTaskStatus(WorkTaskStatus.Executing);
                }
                else if (reviewReport.ReviewReportStatus == ReviewReportStatus.Pass)
                {
                    ExecutNextWorkTask();
                }
            }
            else if (workTask is IDataReport)
            {
                var dataReport = workTask as IDataReport;
                if (dataReport.DataReportStatus == DataReportStatus.UnCommit)
                {
                    WorkFlowStatus = WorkFlowStatus.Pending;
                    if (WorkTaskStatus == WorkTaskStatus.Executed)
                        UpdateWorkTaskStatus(WorkTaskStatus.Executing);
                }else if (dataReport.DataReportStatus == DataReportStatus.Commit)
                {
                    ExecutNextWorkTask();
                }
            }

            // 所有子任务状态为完成状态，工作流程状态改为完成
            if (_workTasks.All(p => p.TaskCompletion))
            {
                if (WorkTaskStatus != WorkTaskStatus.Executing)
                    throw new Exception("工作任务不是执行状态，不将工作流程状态改为通过状态！");
                WorkFlowStatus = WorkFlowStatus.Pass;
                UpdateWorkTaskStatus(WorkTaskStatus.Executed);
            }

            // 所有子任务转台为等待状态，工作流程状态改为等待
            if (_workTasks.All(p => p.WorkTaskStatus == WorkTaskStatus.Waiting))
            {
                if (WorkTaskStatus != WorkTaskStatus.Executing)
                    throw new Exception("工作任务不是执行状态，不将工作流程状态改为通过状态！");

                WorkFlowStatus = WorkFlowStatus.Pending;
                UpdateWorkTaskStatus(WorkTaskStatus.Waiting);
            }
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
        #region  成员方法
        ///// <summary>
        ///// 允许修正工作任务
        ///// </summary>
        ///// <param name="workTask"></param>
        //public void AllowCorrectionWorkTask(IWorkTask workTask)
        //{
        //    if (WorkTaskStatus == WorkTaskStatus.Executed)
        //    {
        //        throw new Exception("工作流程已完成，不允许修正工作任务！");
        //    }
        //    if (WorkTaskStatus == WorkTaskStatus.Waiting)
        //    {
        //        throw new Exception("工作流程为等待状态，不允许修正工作任务！");
        //    }

        //    var _workReport = _workTasks.FirstOrDefault(p => p.ID.Equals(workTask.ID));
        //    if (_workReport == null)
        //        throw new Exception($"评审报告【{Name}】不包含数据报告【{workTask.Name}】");
        //    if (_workReport is IDataReport)
        //    {
        //        var dataReport = _workReport as IDataReport;
        //        if (dataReport.DataReportStatus == DataReportStatus.UnCommit)
        //            throw new Exception($"数据报告【{workTask.Name}】已是未提交状态，可以直接修改！");

        //        dataReport.UpdateDataReportStatus(DataReportStatus.UnCommit);
        //    }
        //    else if (_workReport is IReviewReport)
        //    {
        //        var reviewReport = _workReport as IReviewReport;
        //        if (reviewReport.ReviewReportStatus == ReviewReportStatus.Pending)
        //            throw new Exception($"数据报告【{workTask.Name}】已是未决定状态，可以直接修改！");

        //        reviewReport.UpdateReviewReportStatus(ReviewReportStatus.Pending);
        //    }

        //    var _nextWorkReport = GetNextWorkTask(workTask);
        //    if (_nextWorkReport != null)
        //        _nextWorkReport.Wait();
        //}

        protected override void UpdateWorkTaskStatus(WorkTaskStatus workTaskStatus)
        {
            if (workTaskStatus == WorkTaskStatus.Executing && (_workTasks == null || _workTasks.Count == 0))
                throw new Exception("项目流程不包含工作流程，不能开启");

            var oldWorkTaskStatus = WorkTaskStatus;
            base.UpdateWorkTaskStatus(workTaskStatus);

            // 从等待状态变为执行状态
            if (oldWorkTaskStatus == WorkTaskStatus.Waiting && workTaskStatus == WorkTaskStatus.Executing)
            {
                var firstWorkTask = _workTasks.FirstOrDefault();
                firstWorkTask.Start();
            }

            // 从完成状态变为执行状态
            if (oldWorkTaskStatus == WorkTaskStatus.Executed && workTaskStatus == WorkTaskStatus.Executing)
            {
                WorkFlowStatus = WorkFlowStatus.Pending;
            }
        }

        public override string GetFlowTypeName()
        {
            return "工作流程";
        }
        /// <summary>
        /// 获取下个工作任务
        /// </summary>
        /// <param name="workTask"></param>
        /// <returns></returns>
        public IWorkTask GetNextWorkTask(IWorkTask workTask)
        {
            var index = _workTasks.FindIndex(p => p.ID.Equals(workTask.ID));
            if (index == _workTasks.Count - 1)
                return null;
            return _workTasks[index + 1];
        }

        public override void AllowCorrectionWorkTask()
        {
            base.AllowCorrectionWorkTask();
        }
        #endregion


    }
}
