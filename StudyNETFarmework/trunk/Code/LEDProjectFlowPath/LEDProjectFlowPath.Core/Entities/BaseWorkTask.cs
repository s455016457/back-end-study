using LEDProjectFlowPath.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEDProjectFlowPath.Core.Entities
{
    /// <summary>
    /// 工作任务基类
    /// </summary>
    public abstract class BaseWorkTask : IWorkTask
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual Guid ID { get; private set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; private set; }
        /// <summary>
        /// 任务完成
        /// </summary>
        public abstract bool TaskCompletion { get;protected set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public virtual Guid LeaderId { get; private set; }
        /// <summary>
        /// 工作任务执行状态
        /// </summary>
        public WorkTaskStatus WorkTaskStatus { get; protected set; }

        /// <summary>
        /// 工作任务状态更改更改时触发
        /// </summary>
        public event WorkTaskStatusChangeHandler OnWorkTaskStatusChange;

        protected BaseWorkTask() { }

        public BaseWorkTask(Guid ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }

        public virtual void SetLeaderId (Guid leaderId)
        {
            this.LeaderId = leaderId;
        }
        /// <summary>
        /// 更新工作任务执行状态
        /// </summary>
        /// <param name="workTaskStatus"></param>
        protected virtual void UpdateWorkTaskStatus(WorkTaskStatus workTaskStatus)
        {
            var oldWorkTaskStatus = WorkTaskStatus;
            WorkTaskStatus = workTaskStatus;

            if (OnWorkTaskStatusChange != null && oldWorkTaskStatus != WorkTaskStatus)
                OnWorkTaskStatusChange.Invoke(this);
        }

        public virtual void Start()
        {
            if (WorkTaskStatus != WorkTaskStatus.Waiting)
            {
                throw new Exception($"{GetFlowTypeName()}不是等待状态，不能开启！");
            }

            UpdateWorkTaskStatus(WorkTaskStatus.Executing);
        }

        public abstract string GetFlowTypeName();

        public virtual void Wait()
        {
            if(WorkTaskStatus!=WorkTaskStatus.Executing)
                throw new Exception($"{GetFlowTypeName()}不是执行状态，工作任务不能进入等待状态！");
            UpdateWorkTaskStatus(WorkTaskStatus.Waiting);
        }

        public virtual void AllowCorrectionWorkTask()
        {
            if(WorkTaskStatus==WorkTaskStatus.Executing)
                throw new Exception($"{GetFlowTypeName()}以是执行状态，可以直接修改数据！");
            if(WorkTaskStatus == WorkTaskStatus.Waiting)
                throw new Exception($"{GetFlowTypeName()}以是等待状态，不能修改数据！");

            UpdateWorkTaskStatus(WorkTaskStatus.Executing);
        }
    }
}
