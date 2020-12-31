using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extend;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using TestQuartz.Jobs;
using TestQuartz.Listener;

namespace TestQuartz.Test
{
    /// <summary>
    /// 监听测试
    /// </summary>
    [ClassDiscrible("作业监听测试")]
    public class ListenerTest : ITest
    {
        public async Task RunAsync()
        {
            IScheduler scheduler =await StdSchedulerFactory.GetDefaultScheduler();

            IJobDetail job = JobBuilder.Create<SimpleJob1>()
                .WithIdentity("job1","jobGroup1")
                .Build();

            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger1","triggerGroup1")
                .StartNow()
                .WithSimpleSchedule(x=>x.WithIntervalInSeconds(20).RepeatForever())
                .Build();

            scheduler.ListenerManager.AddJobListener(new JobListener(), KeyMatcher<JobKey>.KeyEquals(job.Key));

            DateTimeOffset df = await scheduler.ScheduleJob(job, trigger);

            ConsoleExtend.WriteLineRed($"作业【{job.Key}】 触发器【{trigger.Key}】 下次执行时间【{df.LocalDateTime}】 " +
                $"执行时间间隔【{trigger.RepeatInterval.Seconds}】s 重复次数【{trigger.RepeatCount}】");


            //job = JobBuilder.Create<SimpleJob2>()
            //    .WithIdentity("job2","jobGroup")
            //    .Build();

            //trigger = (ISimpleTrigger)TriggerBuilder.Create()
            //    .WithIdentity("trigger2","triggerGroup1")
            //    .StartNow()
            //    .WithSimpleSchedule(x=>x.WithIntervalInSeconds(15).RepeatForever())
            //    .Build();

            //df = await scheduler.ScheduleJob(job, trigger);

            //ConsoleExtend.WriteLineRed($"作业【{job.Key}】 触发器【{trigger.Key}】 下次执行时间【{df.LocalDateTime}】 " +
            //    $"执行时间间隔【{trigger.RepeatInterval.Seconds}】s 重复次数【{trigger.RepeatCount}】");

            await scheduler.Start();

            Console.WriteLine("等待1分钟。。。");

            await Task.Delay(TimeSpan.FromMinutes(1));

            await scheduler.Shutdown();
        }
    }
}
