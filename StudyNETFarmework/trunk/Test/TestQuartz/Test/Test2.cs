using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQuartz.Jobs;
using TestQuartz.Listener;

namespace TestQuartz.Test
{
    [ClassDiscrible("简单监听器测试")]
    public class Test2 : ITest
    {
        public async Task RunAsync()
        {
            // 通过工厂获取默认调度器
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());
            scheduler.ListenerManager.AddTriggerListener(new TriggerListener());
            scheduler.ListenerManager.AddJobListener(new JobListener());
            // 开启一个调度
            await scheduler.Start();

            {
                IJobDetail parameterJob = JobBuilder.Create<ParameterJob>()
                    .WithIdentity("带参数作业", "带参数作业组")
                    .WithDescription("测试带参数作业")
                    //.UsingJobData("dataBaseCon", "这里是数据库连接配置")
                    //.UsingJobData("jsonParameter", "这里是JSon参数配置")
                    .SetJobData(new JobDataMap() {
                        {"dataBaseCon","数据库配置字符串" },
                        {"jsonParameter","json参数字符串" }
                    })
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("带参数作业触发器", "带参数作业触发器组")
                    .WithDescription("触发器描述")
                    .StartNow()
                    .UsingJobData("jsonParameter", "触发器中的Json参数字符串")
                    .WithSimpleSchedule(p => p
                        .WithIntervalInSeconds(5)
                        .RepeatForever())
                    .Build();



                var dateTimeOffset = scheduler.ScheduleJob(parameterJob, trigger).Result;
                Console.WriteLine(dateTimeOffset.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            await Task.Delay(TimeSpan.FromSeconds(30)).ConfigureAwait(false);
            // 关闭调度
            await scheduler.Shutdown(false);
        }
    }
}
