using LEDProjectFlowPath.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDProjectFlowPath.Core.Entities
{
    /// <summary>
    /// 评审报告
    /// </summary>
    public class ReviewReport : BaseWorkTask, IReviewReport
    {
        #region 私有属性
        private List<IDataReport> _dataReports;
        private ReviewReportStatus _reviewReportStatus = ReviewReportStatus.Pending;
        #endregion

        #region 成员属性
        /// <summary>
        /// 组名称
        /// </summary>
        public virtual string GroupName { get; private set; }
        /// <summary>
        /// 预览视图路径
        /// </summary>
        public virtual string PreviewViewPath { get; private set; }
        /// <summary>
        /// 主页视图路径
        /// </summary>
        public virtual string IndexViewPath { get; private set; }
        /// <summary>
        /// 任务完成
        /// </summary>
        public override bool TaskCompletion { get; protected set; }
        /// <summary>
        /// 获取报告集合
        /// </summary>
        public IReadOnlyList<IDataReport> DataReports
        {
            get { return _dataReports.AsReadOnly(); }
        }
        /// <summary>
        /// 数据报告类别
        /// </summary>
        public DataReportType DataReportType { get; private set; }
        /// <summary>
        /// 评审报告状态
        /// </summary>
        public ReviewReportStatus ReviewReportStatus
        {
            get { return _reviewReportStatus; }
            private set
            {
                _reviewReportStatus = value;
                TaskCompletion = value == ReviewReportStatus.Pass || value == ReviewReportStatus.NoPass;
            }
        }
        /// <summary>
        /// 运行父流程开始下个流程
        /// </summary>
        public bool AllowParentFlowProceedNextProcess { get; private set; }
        #endregion

        #region 构造函数
        private ReviewReport() { }
        public ReviewReport(Guid ID, string Name, string GroupName, string PreviewViewPath, string IndexViewPath
            , DataReportType DataReportType, ReviewReportStatus ReviewReportStatus
            , bool AllowParentFlowProceedNextProcess, List<IDataReport> DataReports) 
            : base(ID, Name)
        {
            _dataReports = DataReports;
            this.DataReportType = DataReportType;
            this.ReviewReportStatus = ReviewReportStatus;
            this.AllowParentFlowProceedNextProcess = AllowParentFlowProceedNextProcess;
            this.GroupName = GroupName;
            this.PreviewViewPath = PreviewViewPath;
            this.IndexViewPath = IndexViewPath;

        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 修改工作任务状态
        /// </summary>
        /// <param name="workTaskStatus"></param>
        protected override void UpdateWorkTaskStatus(WorkTaskStatus workTaskStatus)
        {
            var oldWorkTaskStatus = WorkTaskStatus;
            base.UpdateWorkTaskStatus(workTaskStatus);

            if (DataReports == null || DataReports.Count == 0)
                return;

            if (oldWorkTaskStatus == WorkTaskStatus.Waiting && workTaskStatus == WorkTaskStatus.Executing)
            {
                if (DataReportType == DataReportType.Parallel)
                {
                    foreach (var item in DataReports)
                    {
                        item.Start();
                    }
                }
                else
                {
                    var item = DataReports.FirstOrDefault();
                    item.Start();
                }
            }            
        }

        /// <summary>
        /// 修改评审报告状态
        /// </summary>
        /// <param name="reviewReportStatus"></param>
        public void UpdateReviewReportStatus(ReviewReportStatus reviewReportStatus)
        {
            if (WorkTaskStatus == WorkTaskStatus.Waiting)
                throw new Exception("工作任务处于等待状态，不能修改评审报告状态！");

            if (_dataReports.Any(p => p.DataReportStatus == DataReportStatus.UnCommit))
                throw new Exception("存在未提交的数据报告，不能修改评审报告状态！");

            ReviewReportStatus = reviewReportStatus;

            if ((reviewReportStatus == ReviewReportStatus.Pass || reviewReportStatus == ReviewReportStatus.NoPass)
                && WorkTaskStatus == WorkTaskStatus.Executing)
            {
                UpdateWorkTaskStatus(WorkTaskStatus.Executed);
            }
            else if (reviewReportStatus == ReviewReportStatus.Pending && WorkTaskStatus == WorkTaskStatus.Executed)
            {
                UpdateWorkTaskStatus(WorkTaskStatus.Executing);
            }
        }

        public override string GetFlowTypeName()
        {
            return "评审报告";
        }

        /// <summary>
        /// 工作任务允许修改
        /// 将下个工作任务由执行状态改为等待状态
        /// 本工作任务由执行完成状态改为执行状态
        /// 只有已执行完成的工作任务才可以请求【工作任务允许修改】
        /// </summary>
        /// <param name="nexWorkTask">下个工作任务</param>
        public void AllowCorrectionWorkTask(IWorkTask nexWorkTask)
        {
            if (WorkTaskStatus == WorkTaskStatus.Executing)
                throw new Exception("工作任务处于执行状态，可以直接修改数据报告！");
            if (WorkTaskStatus == WorkTaskStatus.Waiting)
                throw new Exception("工作任务处于等待状态，不能修改数据报告状态！");

            if (nexWorkTask != null)
                nexWorkTask.Wait();

            UpdateWorkTaskStatus(WorkTaskStatus.Executing);
        }
        #endregion
    }
}
