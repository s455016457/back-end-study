using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extend;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using TestQuartz.Jobs;

namespace TestQuartz.Test
{
    [ClassDiscrible("日历调度器，在对应日期不调度作业")]
    public class CalendarTest : ITest
    {
        public async Task RunAsync()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            #region 添加日历
            AnnualCalendar holidays = new AnnualCalendar(); // 创建一个年度假日日历

            // 元旦 1月1日
            DateTime NewYearDay = new DateTime(DateTime.UtcNow.Year, 1, 1);
            holidays.SetDayExcluded(NewYearDay, true); // true 排除元旦执行  false 在元旦执行

            // 劳动节 5月1日
            DateTime LaborDay = new DateTime(DateTime.UtcNow.Year, 5, 1);
            holidays.SetDayExcluded(LaborDay, true); // true 排除元旦执行  false 在元旦执行

            // 国庆节 10月1日
            DateTime NationalDay = new DateTime(DateTime.UtcNow.Year, 10, 1);
            holidays.SetDayExcluded(NationalDay, true); // true 排除元旦执行  false 在元旦执行

            await scheduler.AddCalendar("holidays", holidays, true, true);
            #endregion

            DateTimeOffset runDate = DateBuilder.DateOf(10,0,0,1,10);

            IJobDetail job = JobBuilder.Create<HelloJob2>()
                .WithIdentity("job1","jobGroup1")
                .Build();

            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger1","triggerGroup1")
                .StartAt(runDate)
                .WithSimpleSchedule(x=>x.WithIntervalInHours(2).RepeatForever())
                .ModifiedByCalendar("holidays")
                .Build();

            DateTimeOffset df = await scheduler.ScheduleJob(job, trigger);

            ConsoleExtend.WriteLineDebug1($"作业【{job.Key}】 触发器【{trigger.Key}】 将在【{df.LocalDateTime}】执行" +
                $"，重复执行时间间隔【{trigger.RepeatInterval.Hours}】小时 重复【{trigger.RepeatCount}】次");

            await scheduler.Start();
            Console.WriteLine("等待30秒。。。");
            await Task.Delay(TimeSpan.FromSeconds(30));
            await scheduler.Shutdown(true);

            ConsoleExtend.WriteLineDebug2($"共执行了{(await scheduler.GetMetaData()).NumberOfJobsExecuted}个作业");
        }
    }
}
