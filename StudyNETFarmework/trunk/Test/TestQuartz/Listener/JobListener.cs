using Common.Extend;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestQuartz.Jobs;

namespace TestQuartz.Listener
{
    public class JobListener : IJobListener
    {
        public string Name => "作业监听";

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLineRed(@"监听器名称【{0}】作业【{1}=》{2}】作业取消 作业描述【{3}】 
作业参数【{4}】 作业类型【{5}】 作业实体【{6}】 触发器【{7}=》{8}】 恢复触发器【{9}=》{10}】 
触发器参数【{11}】 合并参数【{12}】 调度触发时间【{18}】 作业触发时间【{13}】  触发实体ID【{14}】
作业运行时间【{15}】 下次执行时间【{16}】 之前触发时间【{17}】"
                    , Name
                    , context.JobDetail.Key.Group
                    , context.JobDetail.Key.Name
                    , context.JobDetail.Description
                    , Newtonsoft.Json.JsonConvert.SerializeObject(context.JobDetail.JobDataMap)
                    , context.JobDetail.JobType.FullName
                    , context.JobInstance.GetType().FullName
                    , context.Trigger.Key.Group
                    , context.Trigger.Key.Name
                    , context.Recovering ? context.RecoveringTriggerKey.Group : String.Empty
                    , context.Recovering ? context.RecoveringTriggerKey.Name : String.Empty
                    , Newtonsoft.Json.JsonConvert.SerializeObject(context.Trigger.JobDataMap)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(context.MergedJobDataMap)
                    , context.FireTimeUtc.LocalDateTime
                    , context.FireInstanceId
                    , /*new DateTime(context.JobRunTime.Ticks)*/ TimeSpan.FromTicks(context.JobRunTime.Ticks).Seconds+"s"
                    , context.NextFireTimeUtc.HasValue?context.NextFireTimeUtc.Value.LocalDateTime.ToString():String.Empty
                    , context.PreviousFireTimeUtc.HasValue? context.PreviousFireTimeUtc.Value.LocalDateTime.ToString():String.Empty
                    , context.ScheduledFireTimeUtc.HasValue? context.ScheduledFireTimeUtc.Value.LocalDateTime.ToString():String.Empty);
            });
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLineYellow(@"监听器名称【{0}】作业【{1}=》{2}】作业将被执行 作业描述【{3}】 
作业参数【{4}】 作业类型【{5}】 作业实体【{6}】 触发器【{7}=》{8}】 恢复触发器【{9}=》{10}】 
触发器参数【{11}】 合并参数【{12}】 调度触发时间【{18}】 作业触发时间【{13}】  触发实体ID【{14}】
作业运行时间【{15}】 下次执行时间【{16}】 之前触发时间【{17}】"
                    , Name
                    , context.JobDetail.Key.Group
                    , context.JobDetail.Key.Name
                    , context.JobDetail.Description
                    , Newtonsoft.Json.JsonConvert.SerializeObject(context.JobDetail.JobDataMap)
                    , context.JobDetail.JobType.FullName
                    , context.JobInstance.GetType().FullName
                    , context.Trigger.Key.Group
                    , context.Trigger.Key.Name
                    , context.Recovering ? context.RecoveringTriggerKey.Group : String.Empty
                    , context.Recovering ? context.RecoveringTriggerKey.Name : String.Empty
                    , Newtonsoft.Json.JsonConvert.SerializeObject(context.Trigger.JobDataMap)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(context.MergedJobDataMap)
                    , context.FireTimeUtc.LocalDateTime
                    , context.FireInstanceId
                    , /*new DateTime(context.JobRunTime.Ticks)*/ TimeSpan.FromTicks(context.JobRunTime.Ticks).Seconds + "s"
                    , context.NextFireTimeUtc.HasValue ? context.NextFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty
                    , context.PreviousFireTimeUtc.HasValue ? context.PreviousFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty
                    , context.ScheduledFireTimeUtc.HasValue ? context.ScheduledFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty);
            });
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            ConsoleExtend.WriteLineDarkMagenta(@"监听器名称【{0}】作业【{1}=》{2}】作业执行完成 作业描述【{3}】 
作业参数【{4}】 作业类型【{5}】 作业实体【{6}】 触发器【{7}=》{8}】 恢复触发器【{9}=》{10}】 
触发器参数【{11}】 合并参数【{12}】 调度触发时间【{18}】 作业触发时间【{13}】  触发实体ID【{14}】
作业运行时间【{15}】 下次执行时间【{16}】 之前触发时间【{17}】"
                , Name
                , context.JobDetail.Key.Group
                , context.JobDetail.Key.Name
                , context.JobDetail.Description
                , Newtonsoft.Json.JsonConvert.SerializeObject(context.JobDetail.JobDataMap)
                , context.JobDetail.JobType.FullName
                , context.JobInstance.GetType().FullName
                , context.Trigger.Key.Group
                , context.Trigger.Key.Name
                , context.Recovering ? context.RecoveringTriggerKey.Group : String.Empty
                , context.Recovering ? context.RecoveringTriggerKey.Name : String.Empty
                , Newtonsoft.Json.JsonConvert.SerializeObject(context.Trigger.JobDataMap)
                , Newtonsoft.Json.JsonConvert.SerializeObject(context.MergedJobDataMap)
                , context.FireTimeUtc.LocalDateTime
                , context.FireInstanceId
                , /*new DateTime(context.JobRunTime.Ticks)*/ TimeSpan.FromTicks(context.JobRunTime.Ticks).Seconds + "s"
                , context.NextFireTimeUtc.HasValue ? context.NextFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty
                , context.PreviousFireTimeUtc.HasValue ? context.PreviousFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty
                , context.ScheduledFireTimeUtc.HasValue ? context.ScheduledFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty);

            IScheduler scheduler = context.Scheduler;

            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals("jobGroup2"));
            IJobDetail job = null;
            ISimpleTrigger trigger = null;

            if (jobKeys.Any(p => p.Group.Equals("jobGroup2") && p.Name.Equals("job2")))
            {
                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                   .WithIdentity("trigger2", "triggerGroup2")
                   .StartNow()
                   .ForJob("job2", "jobGroup2")
                   .Build();
                DateTimeOffset df = await scheduler.ScheduleJob(trigger);
                ConsoleExtend.WriteLineDarkMagenta($"监听器【{Name}】调用作业【{job.Key}】");

                ConsoleExtend.WriteLineRed($"作业【{trigger.JobKey}】 触发器【{trigger.Key}】 执行时间【{df.LocalDateTime}】");
                //await scheduler.Shutdown(true);
            }
            else
            {
                job = JobBuilder.Create<SimpleJob2>()
                        .WithIdentity("job2", "jobGroup2")
                        .Build();

                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                    .WithIdentity("trigger2", "triggerGroup2")
                    .StartNow()
                    .Build();
                DateTimeOffset df = await scheduler.ScheduleJob(job, trigger);
                ConsoleExtend.WriteLineDarkMagenta($"监听器【{Name}】调用作业【{job.Key}】");

                ConsoleExtend.WriteLineRed($"作业【{job.Key}】 触发器【{trigger.Key}】 执行时间【{df.LocalDateTime}】");
                //await scheduler.Shutdown(true);
            }
        }
    }
}
