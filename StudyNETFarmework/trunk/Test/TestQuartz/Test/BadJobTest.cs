using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extend;
using Quartz;
using Quartz.Impl;
using TestQuartz.Jobs;

namespace TestQuartz.Test
{
    /**
     * 作业1 遇到错误会立即重新执行
     * 作业2 遇到错误会终止执行，并停止与之相关的所有触发器的执行 
     * */
    [ClassDiscrible("异常作业测试")]
    public class BadJobTest : ITest
    {
        public async Task RunAsync()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            DateTimeOffset startTime = DateTimeOffset.Now.AddSeconds(10);

            #region 错误作业1 带参数 设置作业失败后马上重新开始
            IJobDetail job = JobBuilder.Create<BadJob1>()
                .WithIdentity("job1","jobGroup1")
                .UsingJobData(BadJob1.denominatorKey,0)
                .Build();

            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger1","trrigerGroup1")
                .StartAt(startTime)
                .WithSimpleSchedule(x=>x.WithIntervalInSeconds(10).RepeatForever())
                .Build();

            DateTimeOffset df = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"作业【{job.Key}】 触发器【{trigger.Key}】 执行时间【{df.LocalDateTime}】 " +
                $"执行时间间隔【{trigger.RepeatInterval.Seconds}】 重复执行次数【{trigger.RepeatCount}】");
            #endregion

            #region 错误作业2 不带参数 设置作业失败后停止相关触发器执行
            job = JobBuilder.Create<BadJob2>()
                .WithIdentity("job2","jobGroup2")
                .Build();

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger2","triggerGroup1")
                .StartAt(startTime)
                .WithSimpleSchedule(x=>x.WithIntervalInSeconds(10).RepeatForever())
                .Build();

            df = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"作业【{job.Key}】 触发器【{trigger.Key}】 执行时间【{df.LocalDateTime}】 " +
                $"执行时间间隔【{trigger.RepeatInterval.Seconds}】 重复执行次数【{trigger.RepeatCount}】");
            #endregion

            #region 给错误作业2添加新的触发器
            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger3", "triggerGroup1")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                .ForJob("job2", "jobGroup2")
                .Build();

            df = await scheduler.ScheduleJob(trigger);
            ConsoleExtend.WriteLineRed($"作业【{trigger.JobKey}】 触发器【{trigger.Key}】 执行时间【{df.LocalDateTime}】 " +
                $"执行时间间隔【{trigger.RepeatInterval.Seconds}】 重复执行次数【{trigger.RepeatCount}】");
            #endregion

            await scheduler.Start();

            await Task.Delay(TimeSpan.FromMinutes(1));
            await scheduler.Shutdown();
        }
    }
}
