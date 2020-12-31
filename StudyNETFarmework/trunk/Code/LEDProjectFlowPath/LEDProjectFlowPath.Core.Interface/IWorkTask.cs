using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDProjectFlowPath.Core.Interface
{
    /// <summary>
    /// 工作任务状态更改
    /// </summary>
    /// <param name="workTask"></param>
    public delegate void WorkTaskStatusChangeHandler(IWorkTask workTask);
    /// <summary>
    /// 工作任务
    /// </summary>
    public interface IWorkTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// 任务名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 任务完成
        /// </summary>
        bool TaskCompletion { get; }
        /// <summary>
        /// 流程项负责人
        /// </summary>
        Guid LeaderId { get; }
        /// <summary>
        /// 工作任务状态
        /// </summary>
        WorkTaskStatus WorkTaskStatus { get; }
        /// <summary>
        /// 工作任务状态更改更改时触发
        /// </summary>
        event WorkTaskStatusChangeHandler OnWorkTaskStatusChange;

        /***
         * 工作任务等待
         * 只有正在执行的工作任务才可以进入等待状态
         * */
         void Wait();
        /// <summary>
        /// 开启流程
        /// </summary>
        void Start();
    }

    /// <summary>
    /// 数据报告
    /// </summary>
    public interface IDataReport: IWorkTask
    {
        /// <summary>
        /// 数据报告状态
        /// </summary>
        DataReportStatus DataReportStatus { get; }
        /// <summary>
        /// 组名称
        /// </summary>
        string GroupName { get; }
        /// <summary>
        /// 预览视图路径
        /// </summary>
        string PreviewViewPath { get; }
        /// <summary>
        /// 主页视图路径
        /// </summary>
        string IndexViewPath { get; }
        /// <summary>
        /// 修改数据报告状态
        /// </summary>
        /// <param name="dataReportStatus"></param>
        void UpdateDataReportStatus(DataReportStatus dataReportStatus);

        /***
         * 工作任务允许修改 
         * 将下个工作任务由执行状态改为等待状态
         * 本工作任务由执行完成状态改为执行状态
         * 只有已执行完成的工作任务才可以请求【工作任务允许修改】
         * */
        void AllowCorrectionWorkTask(IWorkTask nexWorkTask);
    }

    /// <summary>
    /// 评审报告
    /// </summary>
    public interface IReviewReport: IWorkTask
    {
        /// <summary>
        /// 组名称
        /// </summary>
        string GroupName { get; }
        /// <summary>
        /// 预览视图路径
        /// </summary>
        string PreviewViewPath { get; }
        /// <summary>
        /// 主页视图路径
        /// </summary>
        string IndexViewPath { get; }
        /// <summary>
        /// 数据报告状态
        /// </summary>
        DataReportType DataReportType { get; }
        /// <summary>
        /// 评审报告状态
        /// </summary>
        ReviewReportStatus ReviewReportStatus { get; }
        /// <summary>
        /// 当通过时允许父流程开始下个流程
        /// </summary>
        bool AllowParentFlowProceedNextProcess { get; }
        /// <summary>
        /// 数据报告集合
        /// </summary>
        IReadOnlyList<IDataReport> DataReports { get; }
        /// <summary>
        /// 修改评审报告状态
        /// </summary>
        /// <param name="reviewReportStatus"></param>
        void UpdateReviewReportStatus(ReviewReportStatus reviewReportStatus);

        /***
         * 工作任务允许修改 
         * 将下个工作任务由执行状态改为等待状态
         * 本工作任务由执行完成状态改为执行状态
         * 只有已执行完成的工作任务才可以请求【工作任务允许修改】
         * */
        void AllowCorrectionWorkTask(IWorkTask nexWorkTask);
    }

    /// <summary>
    /// 数据报告类别
    /// </summary>
    public enum DataReportType
    {
        /// <summary>
        /// 串行流程
        /// </summary>
        Serial = 0,
        /// <summary>
        /// 并流程
        /// </summary>
        Parallel = 1
    }

    /// <summary>
    /// 数据报告状态
    /// </summary>
    public enum DataReportStatus
    {
        /// <summary>
        /// 提交
        /// </summary>
        Commit = 1,
        /// <summary>
        /// 未提交
        /// </summary>
        UnCommit = 0,
    }

    /// <summary>
    /// 评审报告状态
    /// </summary>
    public enum ReviewReportStatus
    {
        /// <summary>
        /// 通过
        /// </summary>
        Pass=1,
        /// <summary>
        /// 未决定
        /// </summary>
        Pending=0,
        /// <summary>
        /// 不通过
        /// </summary>
        NoPass =-1,
    }
    /// <summary>
    /// 工作任务状态
    /// </summary>
    public enum WorkTaskStatus
    {
        /// <summary>
        /// 等待执行
        /// </summary>
        Waiting=0,
        /// <summary>
        /// 正在执行
        /// </summary>
        Executing=1,
        /// <summary>
        /// 执行完成
        /// </summary>
        Executed=2,
    }
}
