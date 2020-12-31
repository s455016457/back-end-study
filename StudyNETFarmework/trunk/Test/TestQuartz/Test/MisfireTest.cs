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
    /***
     * 共有两个作业，分别为作业1和作业2 执行需要10秒时间
     * 有三个作业触发器，分别为触发器1、触发器2、触发器3
     * 触发器1 触发作业1，每3秒触发一次，无限触发
     * 触发器2 触发作业2，每3触发一次，无限触发 设置WithMisfireHandlingInstructionNowWithExistingCount
     * 触发器3 触发作业1，每3秒触发一次，无限触发 设置WithMisfireHandlingInstructionNowWithExistingCount
     * 设置WithMisfireHandlingInstructionNowWithExistingCount 设置触发器为MisfireInstruction.SimpleTrigger.RescheduleNowWithExistingRepeatCount
     *  
     * 当这个测试正在运行时，你可以注意到3个触发器在同一个调度器中，触发同一个作业
     * 触发器想每3秒触发一次，但是作业需要10秒才能执行完。
     * 因此，在作业完成的时间中，触发器已经“misfired”(除非调度的"misfire阈值"超过7秒)
     * 你可以看到其中一个工作有它的错误指令 <see cref="MisfireInstruction.SimpleTrigger.RescheduleNowWithExistingRepeatCount" />
     * 当检测到错误触发时，它会立即触发
     * 另一个触发器使用默认的“智能策略”错误指令，它会导致触发器提前到下一个火灾时间（跳过它错过的时间）
     * 这样它就不会立即重新触发，而是在下一次预定的时间。
     * 
     * 智能策略：它会导致触发器提前到下一个触发点，跳过那些它错过的时间）——这样它就不会立即重新触发，而是在下一次预定的时间内。
     * */
    [ClassDiscrible("失败触发测试")]
    public class MisfireTest : ITest
    {
        public async Task RunAsync()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            DateTimeOffset startTime = DateTimeOffset.Now.AddSeconds(3);

            #region 作业1 每3秒执行一次
            IJobDetail job = JobBuilder.Create<LogTimeJob>()
                .WithIdentity("job1","jobGroup1")
                .SetJobData(new JobDataMap {
                    { LogTimeJob.NumExecutions,0},
                    { LogTimeJob.ExecutionDelay,10}
                })
                .Build();

            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger1","triggerGroup1")
                .StartAt(startTime)
                .WithSimpleSchedule(x=>x.WithInterval(TimeSpan.FromSeconds(3)).RepeatForever())
                .Build();

            DateTimeOffset df = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"触发器【{trigger.Key}】 作业【{job.Key}】 下次执行时间【{df.LocalDateTime.ToString("r")}】" +
                $" 执行时间间隔【{trigger.RepeatInterval.Seconds}】 重复次数【{trigger.RepeatCount}】");
            #endregion

            #region 作业2 每3秒执行一次
            job = JobBuilder.Create<LogTimeJob>()
                .WithIdentity("job2", "jobGroup1")
                .SetJobData(new JobDataMap {
                    { LogTimeJob.NumExecutions,0},
                    { LogTimeJob.ExecutionDelay,10}
                })
                .Build();
            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger2", "triggerGroup1")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(3)).RepeatForever().WithMisfireHandlingInstructionNowWithExistingCount())
                .Build();
            df = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"触发器【{trigger.Key}】 作业【{job.Key}】 下次执行时间【{df.LocalDateTime.ToString("r")}】" +
                $" 执行时间间隔【{trigger.RepeatInterval.Seconds}】 重复次数【{trigger.RepeatCount}】");
            #endregion

            #region 给作业1 添加一个新的触发器
            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger3", "triggerGroup1")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(3)).RepeatForever().WithMisfireHandlingInstructionNowWithExistingCount())
                .ForJob("job1", "jobGroup1")
                .Build();
            df = await scheduler.ScheduleJob(trigger);
            ConsoleExtend.WriteLineRed($"触发器【{trigger.Key}】 作业【{trigger.JobKey}】 下次执行时间【{df.LocalDateTime.ToString("r")}】" +
                $" 执行时间间隔【{trigger.RepeatInterval.Seconds}】 重复次数【{trigger.RepeatCount}】");
            #endregion

            await scheduler.Start();
            await Task.Delay(TimeSpan.FromMinutes(2));
            await scheduler.Shutdown();

        }
    }
}
