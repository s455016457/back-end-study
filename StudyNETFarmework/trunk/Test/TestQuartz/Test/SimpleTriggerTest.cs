using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extend;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using TestQuartz.Jobs;
using TestQuartz.SchedulerLog;

namespace TestQuartz.Test
{
    [ClassDiscrible("简单触发器测试")]
    public class SimpleTriggerTest : ITest
    {
        ILogProvider logProvider = new ConsoleLogProvider();
        public async Task RunAsync()
        {
            // 获取默认调度器
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            // 获取一个15秒后执行的时间
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 15);

            #region 作业1 15秒后执行，只执行一次
            // 创建带参数作业
            IJobDetail job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job1", "jobGroup1")
                .WithDescription("带参数作业")
                .SetJobData(new JobDataMap() {
                        {"dataBaseCon","数据库配置字符串" },
                        {"jsonParameter","json参数字符串" }
                })
                .Build();

            // 创建一个15秒后执行的触发器,只执行一次
            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger1", "triggerGroup1")
                .WithDescription("15秒后执行，只执行一次")
                .StartAt(startTime)
                .UsingJobData("jsonParameter", "trigger json参数字符串")
                .Build();

            // 调度作业执行
            DateTimeOffset? ft = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineRed($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 重复【{trigger.RepeatCount}】次，每【{trigger.RepeatInterval.TotalSeconds}】秒重复一次。");
            #endregion

            #region 作业2 15秒后执行，然后每过10秒执行一次，共执行5次
            // 创建带参数作业
            job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job2", "jobGroup1")
                .WithDescription("带参数作业")
                .SetJobData(new JobDataMap() {
                        {"dataBaseCon","数据库配置字符串" },
                        {"jsonParameter","json参数字符串" }
                })
                .Build();

            // 创建一个15秒后执行的触发器，每过10秒触发一次，共触发5次
            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger2", "triggerGroup1")
                .WithDescription("15秒后执行，然后每过10秒触发一次，共触发5次")
                .StartAt(startTime)
                .UsingJobData("jsonParameter", "trigger json参数字符串")
                .WithSimpleSchedule(x=>x.WithIntervalInSeconds(10).WithRepeatCount(5))
                .Build();

            // 调度作业执行
            ft = await scheduler.ScheduleJob(job, trigger);

            ConsoleExtend.WriteLineYellow($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 重复【{trigger.RepeatCount}】次，每【{trigger.RepeatInterval.TotalSeconds}】秒重复一次。");

            //logProvider.GetLogger(typeof(SimpleTriggerTest).FullName).Invoke(LogLevel.Info
            //    , () => $"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 重复【{trigger.RepeatCount}】次，每【{trigger.RepeatInterval.TotalSeconds}】秒重复一次。");
            #endregion

            #region 相同的作业2可以被其他触发器执行，每5秒执行一次，共执行10次

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger3_2", "triggerGroup1")
                .WithDescription("相同的作业2可以被其他触发器执行，每5秒执行一次，共执行10次")
                .ForJob("job2", "jobGroup1")               // 根据job名称和组绑定
                //.ForJob(new JobKey("job2", "jobGroup1")) // 根据jobKey绑定
                //.ForJob(job)                             // 根据job绑定
                .StartAt(startTime)
                .WithSimpleSchedule(x=>x.WithIntervalInSeconds(5).WithRepeatCount(10))
                .Build();

            ft = await scheduler.ScheduleJob(trigger);
            ConsoleExtend.WriteLineRed($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 重复【{trigger.RepeatCount}】次，每【{trigger.RepeatInterval.TotalSeconds}】秒重复一次。");
            #endregion

            #region 作业3 将无限执行，每20秒执行一次
            job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job3", "jobGroup1")
                .WithDescription("带参数作业")
                .SetJobData(new JobDataMap() {
                        {"dataBaseCon","数据库配置字符串" },
                        {"jsonParameter","json参数字符串" }
                })
                .Build();

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger4","triggerGroup1")
                .WithDescription("将无限执行，每20秒执行一次")
                .WithSimpleSchedule(x=>x.WithIntervalInSeconds(20).RepeatForever())
                .Build();

            ft=await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineDarkMagenta($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 重复【{trigger.RepeatCount}】次，每【{trigger.RepeatInterval.TotalSeconds}】秒重复一次。");
            #endregion

            await scheduler.Start();

            #region 作业4 作业也可以在调度开始后添加进调度
            job =JobBuilder.Create<ParameterJob>()
                .WithIdentity("job4", "jobGroup1")
                .WithDescription("带参数作业")
                .SetJobData(new JobDataMap() {
                        {"dataBaseCon","数据库配置字符串" },
                        {"jsonParameter","json参数字符串" }
                })
                .Build();

            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger5", "triggerGroup1")
                .WithDescription("作业也可以在调度开始后添加进调度 将无限执行，每20秒执行一次")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(20).RepeatForever())
                .Build();

            ft = await scheduler.ScheduleJob(job, trigger);
            ConsoleExtend.WriteLineYellow($"{job.Key} 将在以下时间运行【{ft.Value.LocalDateTime}】 重复【{trigger.RepeatCount}】次，每【{trigger.RepeatInterval.TotalSeconds}】秒重复一次。");
            #endregion

            #region 作业也可以直接出发(而不是通过触发器)
            job = JobBuilder.Create<ParameterJob>()
                .WithIdentity("job4", "jobGroup1")
                .WithDescription("带参数作业")
                .SetJobData(new JobDataMap() {
                        {"dataBaseCon","数据库配置字符串" },
                        {"jsonParameter","json参数字符串" }
                })
                .StoreDurably() //设置没有触发器纸箱改作业，调度不存储改作业
                .Build();

            await scheduler.AddJob(job, true);
            await scheduler.TriggerJob(new JobKey("job4", "jobGroup1")
                ,new JobDataMap { { "jsonParameter", "手动出发作业传入 json参数字符串" } });// 手动出发作业

            ConsoleExtend.WriteLineRed($"{job.Key} 手动触发作业。");
            #endregion

            #region 手动触发之前作业
            await scheduler.TriggerJob(new JobKey("job3", "jobGroup1")
                , new JobDataMap { { "jsonParameter", "手动出发作业传入 json参数字符串" } });// 手动出发作业

            ConsoleExtend.WriteLineRed("手动出发之前作业job3");
            #endregion

            #region 重新安排之前作业
            trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger3_2", "triggerGroup1")
                .WithDescription("重新安排触发器3_2的作业，每20秒执行一次，共执行5次")
                .StartAt(startTime)
                .WithSimpleSchedule(x=>x.WithIntervalInSeconds(20).WithRepeatCount(5))
                .Build();

            ft = await scheduler.RescheduleJob(trigger.Key,trigger);
            ConsoleExtend.WriteLineRed($" 重新安排触发器3_2 job2 将在以下时间运行【{ft.Value.LocalDateTime}】 重复【{trigger.RepeatCount}】次，每【{trigger.RepeatInterval.TotalSeconds}】秒重复一次。");
            #endregion

            Console.WriteLine("等待3分钟");
            await Task.Delay(TimeSpan.FromMinutes(3)).ConfigureAwait(false);
            // 关闭调度
            await scheduler.Shutdown(false);
        }
    }
}
