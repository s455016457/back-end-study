using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestQuartz.Jobs;
using TestQuartz.Listener;

namespace TestQuartz.Test
{
    [ClassDiscrible("简单作业测试")]
    public class Test1 : ITest
    {
        public async Task RunAsync()
        {

            // 通过工厂获取默认调度器
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            scheduler.ListenerManager.AddTriggerListener(new TriggerListener());
            // 开启一个调度
            await scheduler.Start();
            {
                // 定义作业并将其与我们的HelloJob类联系起来
                IJobDetail job = JobBuilder.Create<HelloJob>()
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
            }

            {
                IJobDetail job2 = JobBuilder.Create<HelloJob2>()
                    .WithIdentity("job2", "group1")
                    .Build();
                var job1 = JobBuilder.Create<HelloJob>()
                    .WithIdentity("NewJob1", "group1")
                    .Build();

                ITrigger trigger2 = TriggerBuilder.Create()
                    .WithIdentity("trigger2", "group1")
                    .WithSimpleSchedule(p => p
                        .WithIntervalInSeconds(5)
                        .RepeatForever())
                    .Build();
                var dateTimeOffset = scheduler.ScheduleJob(job2, trigger2).Result;
                Console.WriteLine(dateTimeOffset.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

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

            {
                //scheduler.JobFactory = new MyJobFactory();


                //TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                //    .WithIdentity("工厂作业触发器", "工厂作业触发器组")
                //    .WithDescription("触发器描述")
                //    .ForJob("","")
                //    .StartNow()
                //    .UsingJobData("jsonParameter", "触发器中的Json参数字符串")
                //    .WithSimpleSchedule(p => p
                //        .WithIntervalInSeconds(5)
                //        .RepeatForever());


                //scheduler.ScheduleJob(triggerBuilder.Build());

            }

            // 获取当前正在执行的作业
            var list = scheduler.GetCurrentlyExecutingJobs().Result;
            Console.WriteLine("当前正在执行作业：");
            foreach (IJobExecutionContext item in list)
            {
                var itemJob = item.JobDetail;
                var itemTrigger = item.Trigger;
                Console.WriteLine("当前触发器：组：【{0}】 名称【{1}】 描述【{2}】", itemTrigger.Key.Group, itemTrigger.Key.Name, itemTrigger.Description);
                Console.WriteLine("当前触作业：组：【{0}】 名称【{1}】 描述【{2}】", itemJob.Key.Group, itemJob.Key.Name, itemJob.Description);
            }

            Console.WriteLine("等待60秒");
            await Task.Delay(TimeSpan.FromSeconds(60)).ConfigureAwait(false);
            // 关闭调度
            await scheduler.Shutdown(false);
        }
    }
}
