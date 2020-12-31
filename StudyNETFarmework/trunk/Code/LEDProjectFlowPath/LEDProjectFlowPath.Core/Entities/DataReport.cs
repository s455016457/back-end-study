using LEDProjectFlowPath.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDProjectFlowPath.Core.Entities
{
    /// <summary>
    /// 数据报告
    /// </summary>
    public class DataReport : BaseWorkTask, IDataReport
    {
        #region 私有属性
        private DataReportStatus _dataReportStatus = DataReportStatus.UnCommit;
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
        public override bool TaskCompletion
        {
            get; protected set;
        }
        /// <summary>
        /// 数据报告状态
        /// </summary>
        public DataReportStatus DataReportStatus
        {
            get
            {
                return _dataReportStatus;
            }
            private set
            {
                _dataReportStatus = value;
                TaskCompletion = value == DataReportStatus.Commit;
            }
        }
        #endregion

        #region 构造函数
        private DataReport() { }
        public DataReport(Guid ID, string Name, string GroupName, string PreviewViewPath, string IndexViewPath, DataReportStatus DataReportStatus)
            : base(ID, Name)
        {
            this.DataReportStatus = DataReportStatus;
            this.GroupName = GroupName;
            this.PreviewViewPath = PreviewViewPath;
            this.IndexViewPath = IndexViewPath;
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 状态修改
        /// </summary>
        /// <param name="dataReportStatus"></param>
        public void UpdateDataReportStatus(DataReportStatus dataReportStatus)
        {
            if (WorkTaskStatus == WorkTaskStatus.Waiting)
                throw new Exception("工作任务处于等待状态，不能修改数据报告状态！");

            DataReportStatus = dataReportStatus;
            if (dataReportStatus == DataReportStatus.Commit && WorkTaskStatus == WorkTaskStatus.Executing)
            {
                UpdateWorkTaskStatus(WorkTaskStatus.Executed);
            }
            else if (dataReportStatus == DataReportStatus.UnCommit && WorkTaskStatus == WorkTaskStatus.Executed)
            {
                UpdateWorkTaskStatus(WorkTaskStatus.Executing);
            }
        }

        public override string GetFlowTypeName()
        {
            return "数据报告";
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
