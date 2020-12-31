using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LEDProjectFlowPath.Core.Interface
{
    public delegate void FlowItemPassHandler(IFlowItem flowItem);
    public delegate void FlowItemNoPassHandler(IFlowItem flowItem);
    /// <summary>
    /// 流程项接口
    /// </summary>
    public interface IFlowItem: IFlow
    {
        #region 成员属性
        /// <summary>
        /// 流程项视图
        /// </summary>
        string ViewResultPath { get; }
        /// <summary>
        /// 父流程
        /// </summary>
        IFlow ParentFlow { get; }
        /// <summary>
        /// 子流程
        /// </summary>
        IReadOnlyList<IFlowItem> ChilenChildrenFlow { get; }
        /// <summary>
        /// 子流程项类别
        /// </summary>
        ChildrenFlowType ChildrenFlowType { get; }
        /// <summary>
        /// 当通过时允许父流程开始下个流程
        /// </summary>
        Boolean AllowParentFlowProceedNextProcess { get; }
        #endregion

        #region 事件
        /// <summary>
        /// 当流程项通过时触发
        /// </summary>
        event FlowItemPassHandler OnFlowItemPass;
        /// <summary>
        /// 当流程项不同过时触发
        /// </summary>
        event FlowItemNoPassHandler OnFlowItemNoPass;
        #endregion

        #region 成员方法
        /// <summary>
        /// 流程项通过
        /// </summary>
        void FlowItemPass();
        /// <summary>
        /// 流程项不通过
        /// </summary>
        void FlowItemNoPass();
        /// <summary>
        /// 提交审核
        /// </summary>
        void Commit();
        /// <summary>
        /// 在提交后请求更正数据
        /// </summary>
        void AskingForCorrection();
        /// <summary>
        /// 允许更正数据
        /// </summary>
        void AllowCorrenction();
        /// <summary>
        /// 允许开始下个流程
        /// </summary>
        /// <returns></returns>
        bool AllowProceedNextProcess();


        #endregion
    }

    /// <summary>
    /// 流程项状态
    /// </summary>
    public enum FlowStatus
    {
        /// <summary>
        /// 未提交
        /// </summary>
        NoCommit=0,
        /// <summary>
        /// 提交
        /// </summary>
        Commit=1,
        /// <summary>
        /// 通过
        /// </summary>
        Pass=2,
        /// <summary>
        /// 不通过
        /// </summary>
        NoPass=3,
    }

    /// <summary>
    /// 流程子项类别
    /// </summary>
    public enum ChildrenFlowType
    {
        /// <summary>
        /// 串行流程
        /// </summary>
        Serial=0,
        /// <summary>
        /// 并流程
        /// </summary>
        Parallel=1
    }
}
