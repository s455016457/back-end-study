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
    [ClassDiscrible("日历触发器")]
    public class CronTriggerTest : ITest
    {
        public async Task RunAsync()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            #region 作业1 每20秒执行一次
            var job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job1","jobGroup1")
                .WithDescription("每20秒执行一次")
                .SetJobData(new JobDataMap {
                    { "dataBaseCon","数据库配置字符串"},
                    { "jsonParameter","Jons 参数"},
                })
                .Build();

            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger1","triggerGroup")
                .WithCronSchedule("0/20 * * * * ?")
                .Build();

            // 调度作业执行
            DateTimeOffset? ft = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 基于重复表达式【{trigger.CronExpressionString}】。");
            #endregion

            #region 每2分钟的15秒执行一次
            job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job2", "jobGroup1")
                .WithDescription("每2分钟的15秒执行一次")
                .SetJobData(new JobDataMap {
                    { "dataBaseCon","数据库配置字符串"},
                    { "jsonParameter","Jons 参数"},
                })
                .Build();

            trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger2", "triggerGroup")
                .WithCronSchedule("15 0/2 * * * ?")
                .Build();

            // 调度作业执行
            ft = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 基于重复表达式【{trigger.CronExpressionString}】。");
            #endregion

            #region 在每天的 12点到18点 之间，每1分钟的20秒执行一次
            job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job3", "jobGroup1")
                .WithDescription("在每天的12点到18点之间，每1分钟的20秒执行一次")
                .SetJobData(new JobDataMap {
                    { "dataBaseCon","数据库配置字符串"},
                    { "jsonParameter","Jons 参数"},
                })
                .Build();

            trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger3", "triggerGroup")
                .WithCronSchedule("20 0/1 12-18 * * ?")
                .Build();

            // 调度作业执行
            ft = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 基于重复表达式【{trigger.CronExpressionString}】。");
            #endregion

            #region 在每月的1号29号的下午4点执行
            job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job4", "jobGroup1")
                .WithDescription("在每月的29号1号的下午4点执行")
                .SetJobData(new JobDataMap {
                    { "dataBaseCon","数据库配置字符串"},
                    { "jsonParameter","Jons 参数"},
                })
                .Build();

            trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger4", "triggerGroup")
                .WithCronSchedule("0 0 16 1,29 * ?", x => x.InTimeZone(TimeZoneInfo.Local))
                .Build();

            // 调度作业执行
            ft = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 基于重复表达式【{trigger.CronExpressionString}】。");
            #endregion

            #region 在每个星期一到星期六，每30秒执行一次
            job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job5", "jobGroup1")
                .WithDescription("在每个星期一到星期六，每30秒执行一次")
                .SetJobData(new JobDataMap {
                    { "dataBaseCon","数据库配置字符串"},
                    { "jsonParameter","Jons 参数"},
                })
                .Build();

            trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger5", "triggerGroup")
                .WithCronSchedule("0/30 * * ? * MON,SAT",x=>x.InTimeZone(TimeZoneInfo.Local))
                .Build();

            // 调度作业执行
            ft = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 基于重复表达式【{trigger.CronExpressionString}】。");
            #endregion

            #region 在每个星期天，每1小时执行一次
            job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job6", "jobGroup1")
                .WithDescription("在每个星期天，每1小时执行一次")
                .SetJobData(new JobDataMap {
                    { "dataBaseCon","数据库配置字符串"},
                    { "jsonParameter","Jons 参数"},
                })
                .Build();

            trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger6", "triggerGroup")
                .WithCronSchedule("0 0 1 ? * SUN")
                .Build();

            // 调度作业执行
            ft = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 基于重复表达式【{trigger.CronExpressionString}】。");
            #endregion

            #region 在每天的10点42份执行
            job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job7", "jobGroup1")
                .WithDescription("在每个星期天，每1小时执行一次")
                .SetJobData(new JobDataMap {
                    { "dataBaseCon","数据库配置字符串"},
                    { "jsonParameter","Jons 参数"},
                })
                .Build();

            trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger7", "triggerGroup")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(10, 42).InTimeZone(TimeZoneInfo.Local))
                //.WithCronSchedule("0 42 10 * * ?", x => x.InTimeZone(TimeZoneInfo.Local))
                .Build();
            
            // 调度作业执行
            ft = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 基于重复表达式【{trigger.CronExpressionString}】。");
            #endregion

            await scheduler.Start();
            Console.WriteLine("等待3分钟");
            await Task.Delay(TimeSpan.FromMinutes(3)).ConfigureAwait(false);
            // 关闭调度
            await scheduler.Shutdown(false);
        }
    }
}
