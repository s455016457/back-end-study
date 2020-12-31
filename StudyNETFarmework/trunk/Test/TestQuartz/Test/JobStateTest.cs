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
    [ClassDiscrible("作业数据状态测试")]
    public class JobStateTest : ITest
    {
        public async Task RunAsync()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            DateTimeOffset startTime = DateTimeOffset.Now.AddSeconds(10);

            #region 作业1 每10秒执行一次
            IJobDetail job = JobBuilder.Create<ColorJob>()
                .WithIdentity("job1", "jobGroup1")
                .WithDescription("作业1 每10秒执行一次")
                .SetJobData(new JobDataMap {
                    { ColorJob.ColorKey,"Red"},
                    { ColorJob.ColorCountKey,0}
                })
                .Build();

            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger1", "triggerGroup1")
                .UsingJobData(ColorJob.ColorKey, "Blue")
                .UsingJobData(ColorJob.ColorCountKey, 1)
                .StartAt(startTime)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                .Build();
            DateTimeOffset df = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"作业【{job.Key}】 描述【{job.Description}】 执行时间【{df.LocalDateTime}】" +
                $" 执行时间间隔【{trigger.RepeatInterval.Seconds}】s 执行次数【{trigger.RepeatCount}】");
            #endregion

            #region 作业2 每15秒执行一次
            job = JobBuilder.Create<ColorJob>()
                .WithIdentity("job2", "jobGroup1")
                .WithDescription("作业2 每15秒执行一次")
                .SetJobData(new JobDataMap {
                    { ColorJob.ColorKey,"Yellow"},
                    { ColorJob.ColorCountKey,10}
                })
                .Build();
            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger2", "triggerGroup1")
                .UsingJobData(ColorJob.ColorKey, "White")
                .UsingJobData(ColorJob.ColorCountKey, 11)
                .StartAt(startTime.AddSeconds(5))
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(15).RepeatForever())
                .Build();
            df = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"作业【{job.Key}】 描述【{job.Description}】 执行时间【{df.LocalDateTime}】" +
                $" 执行时间间隔【{trigger.RepeatInterval.Seconds}】s 执行次数【{trigger.RepeatCount}】");
            #endregion

            #region 作业1 添加触发器，每25秒触发一次
            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger3", "triggerGroup1")
                .UsingJobData(ColorJob.ColorKey, "Green")
                .UsingJobData(ColorJob.ColorCountKey, 21)
                .StartAt(startTime.AddSeconds(15))
                .ForJob("job1", "jobGroup1")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(25).RepeatForever())
                .Build();
            df = await scheduler.ScheduleJob(trigger);
            ConsoleExtend.WriteLineRed($"作业【{trigger.JobKey}】 执行时间【{df.LocalDateTime}】" +
                $" 执行时间间隔【{trigger.RepeatInterval.Seconds}】s 执行次数【{trigger.RepeatCount}】");
            #endregion
            await scheduler.Start();
            Console.WriteLine("等待3分钟...");
            await Task.Delay(TimeSpan.FromMinutes(3));
            await scheduler.Shutdown();
        }
    }
}
