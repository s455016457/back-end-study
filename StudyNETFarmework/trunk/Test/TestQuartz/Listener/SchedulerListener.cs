using Common.Extend;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestQuartz.Listener
{
    public class SchedulerListener : ISchedulerListener
    {
        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 添加作业【{0}=》{1}】 类型【{2}】 描述【{3}】 参数【{4}】",
                    jobDetail.Key.Group
                    , jobDetail.Key.Name
                    , jobDetail.JobType.FullName
                    , jobDetail.Description
                    , Newtonsoft.Json.JsonConvert.SerializeObject(jobDetail.JobDataMap == null ? new object() : jobDetail.JobDataMap)
                    );
            });
        }

        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 删除作业【{0}=》{1}】 ",
                    jobKey.Group
                    , jobKey.Name
                    );
            });
        }

        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 作业中断【{0}=》{1}】 ",
                    jobKey.Group
                    , jobKey.Name
                    );
            });
        }

        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 作业停顿【{0}=》{1}】 ",
                    jobKey.Group
                    , jobKey.Name
                    );
            });
        }

        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 作业恢复【{0}=》{1}】 ",
                    jobKey.Group
                    , jobKey.Name
                    );
            });
        }

        public Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, @"调度监听 作业调度 触发器【{0}=》{1}】 
触发器描述【{2}】 触发器参数【{3}】 作业【{4}=》{5}】 触发器开始时间【{6}】",
                    trigger.Key.Group
                    , trigger.Key.Name
                    , trigger.Description
                    , Newtonsoft.Json.JsonConvert.SerializeObject(trigger.JobDataMap)
                    , trigger.JobKey.Group
                    , trigger.JobKey.Name
                    ,trigger.StartTimeUtc.LocalDateTime
                    );
            });
        }

        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 Jobs停顿作业【{0}】 ",
                    jobGroup
                    );
            });
        }

        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 Jobs恢复作业【{0}】 ",
                    jobGroup
                    );
            });
        }

        public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 未调度的作业 触发器【{0}=》{1}】 ",
                    triggerKey.Group
                    , triggerKey.Name
                    );
            });
        }

        public Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 调度异常 消息【{0}】 调度异常消息【{1}】 异常帮助链接【{2}】 ",
                    msg
                    , cause.Message
                    ,cause.HelpLink
                    );
            });
        }

        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 调度器待机状态");
            });
        }

        public Task SchedulerShutdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 调度器关机");
            });
        }

        public Task SchedulerShuttingdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 调度器关机中");
            });
        }

        public Task SchedulerStarted(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 调度器开启");
            });
        }

        public Task SchedulerStarting(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 调度器开启中");
            });
        }

        public Task SchedulingDataCleared(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, "调度监听 调度数据清除");
            });
        }

        public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, @"调度监听 触发完成 触发器【{0}=》{1}】 
触发器描述【{2}】 触发器参数【{3}】 作业【{4}=》{5}】 触发器开始时间【{6}】",
                    trigger.Key.Group
                    , trigger.Key.Name
                    , trigger.Description
                    , Newtonsoft.Json.JsonConvert.SerializeObject(trigger.JobDataMap)
                    , trigger.JobKey.Group
                    , trigger.JobKey.Name
                    , trigger.StartTimeUtc.LocalDateTime
                    );
            });
        }

        public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, @"调度监听 触发终止 触发器【{0}=》{1}】 ",
                    triggerKey.Group
                    , triggerKey.Name
                    );
            });
        }

        public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, @"调度监听 触发恢复 触发器【{0}=》{1}】 ",
                    triggerKey.Group
                    , triggerKey.Name
                    );
            });
        }

        public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, @"调度监听 Triggers触发暂停 触发器【{0}】 ",
                    triggerGroup
                    );
            });
        }

        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLine(ConsoleColor.Cyan, @"调度监听 Triggers触发恢复 触发器【{0}】 ",
                    triggerGroup
                    );
            });
        }
    }
}
