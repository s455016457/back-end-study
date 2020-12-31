using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQuartz.Jobs;

namespace TestQuartz.Test
{
    [ClassDiscrible("负载测试")]
    public class LoadTest : ITest
    {
        private const int NumberOfJobs = 500;
        public async Task RunAsync()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = await sf.GetScheduler();
            Random r = new Random();
            // schedule 500 jobs to run
            for (int count = 1; count <= NumberOfJobs; count++)
            {
                IJobDetail job = JobBuilder.Create<LogTimeJob>()
                    .WithIdentity($"job{count}","jobGroup")
                    .WithDescription($"负载测试作业{count}")
                    .SetJobData(new JobDataMap {
                        {LogTimeJob.NumExecutions,0},
                        { LogTimeJob.ExecutionDelay,(int)(r.NextDouble()*25)},
                    })
                    .RequestRecovery() // 当调度程序宕机时，请求调度器重新执行这个作业。
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"trigger{count}","triggerGroup")
                    //.WithSimpleSchedule(x=>x.WithIntervalInSeconds(10).RepeatForever())
                    .StartAt(DateBuilder.FutureDate(10000 + count * 100, IntervalUnit.Millisecond))
                    .Build();

                await sched.ScheduleJob(job, trigger);
                if (count % 25 == 0)
                {
                    Console.WriteLine($"...调度了【{count}】个作业");
                }
            }

            await sched.Start();

            await Task.Delay(TimeSpan.FromMinutes(2));

            await sched.Shutdown();
        }
    }
}
