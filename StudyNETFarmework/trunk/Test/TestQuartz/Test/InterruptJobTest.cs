using System;
using System.Threading.Tasks;
using Common.Extend;
using Quartz;
using Quartz.Impl;
using TestQuartz.Jobs;

namespace TestQuartz.Test
{
    [ClassDiscrible("中断作业测试")]
    public class InterruptJobTest : ITest
    {
        public async Task RunAsync()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 10);

            IJobDetail job = JobBuilder.Create<InterruptJob>()
                .WithIdentity("job1","jobGroup1")
                .Build();

            ISimpleTrigger trigger =(ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger1","triggerGroup1")
                .StartAt(startTime)
                .WithSimpleSchedule(x=>x.WithIntervalInSeconds(3).RepeatForever())
                .Build();

            DateTimeOffset df = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"作业【{job.Key}】 触发器【{trigger.Key}】 执行时间【{df.LocalDateTime}】 " +
                $"执行时间间隔【{trigger.RepeatInterval.Seconds}】s 重复执行次数【{trigger.RepeatCount}】");
            await scheduler.Start();

            Console.WriteLine("每7秒中断一次调度");

            for (int i = 0; i < 50; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(7));
                try
                {
                    await scheduler.Interrupt(job.Key);
                }
                catch (Exception ex)
                {
                    ConsoleExtend.WriteLineError(ex.Message);
                }
            }

            await scheduler.Shutdown(true);

            SchedulerMetaData metaData = await scheduler.GetMetaData();
            ConsoleExtend.WriteLineDebug2($"执行了 {metaData.NumberOfJobsExecuted} 次作业.");
            ConsoleExtend.WriteLineDebug2($"作业【{job.Key}】 中循环共执行了【{InterruptJob.ExecutedCount}】次" +
                $"，其中成功【{InterruptJob.SuccessfulCount}】次，中断【{InterruptJob.InterruptlCount}】次");
        }
    }
}
