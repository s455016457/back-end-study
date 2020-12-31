using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Core;
using Quartz.Impl;
using TestQuartz.Jobs;

namespace TestQuartz.Test
{
    [ClassDiscrible("作业历史记录插件测试")]
    public class LoggingJobHistoryPluginTest : ITest
    {
        public async Task RunAsync()
        {
            var properties = new NameValueCollection
            {
                ["quartz.plugin.triggerHistory333.type"] = "TestQuartz.Plugin.LoggingTriggerHistoryPlugin, TestQuartz",
                ["quartz.plugin.jobHistory333.type"] = "TestQuartz.Plugin.LoggingJobHistoryPlugin, TestQuartz",
            };

            // First we must get a reference to a scheduler
            StdSchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler scheduler = await sf.GetScheduler();

            // 定义作业并将其与我们的HelloJob类联系起来
            IJobDetail job = JobBuilder.Create<HelloJob2>()
                .WithIdentity("job1", "group1")
                .Build();

            // 创建一个触发器，现在就运行，每10秒执行一次
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(p => p
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            // 调度作业
            DateTimeOffset dateTimeOffset = scheduler.ScheduleJob(job, trigger).Result;
            Console.WriteLine(dateTimeOffset.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            await scheduler.Start();
            await Task.Delay(TimeSpan.FromSeconds(60));
            await scheduler.Shutdown();
        }
    }
}
