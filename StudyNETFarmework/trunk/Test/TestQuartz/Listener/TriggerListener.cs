using Common.Extend;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestQuartz.Listener
{
    public class TriggerListener : ITriggerListener
    {
        public string Name => "触发器监听";

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLineDebug(@"监听器名称【{0}】 触发器【{1}=>{2}】触发完成 触发器描述【{3}】 作业【{4}=》{5}】 触发器参数【{6}】 
触发器日历名称【{7}】 最后执行时间【{8}】 下次执行时间【{9}】 作业执行时间【{10}】 触发的实际时间【{11}】 context中下次执行时间【{12}】"
                , Name
                , trigger.Key.Group
                , trigger.Key.Name
                , trigger.Description
                , trigger.JobKey.Group
                , trigger.JobKey.Name
                , Newtonsoft.Json.JsonConvert.SerializeObject(trigger.JobDataMap)
                , trigger.CalendarName
                , trigger.FinalFireTimeUtc.HasValue ? trigger.FinalFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty //最终执行时间
                , trigger.GetFireTimeAfter(DateTimeOffset.Now).HasValue
                    ? trigger.GetFireTimeAfter(DateTimeOffset.Now).Value.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss") : String.Empty //下次执行时间
                , new DateTime(context.JobRunTime.Ticks)    //作业执行时间
                , context.FireTimeUtc.LocalDateTime
                , context.NextFireTimeUtc.HasValue ? context.NextFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty
                );
            });
        }

        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLineDebug1(@"监听器名称【{0}】 触发器【{1}=>{2}】触发开始 触发器描述【{3}】 作业【{4}=》{5}】 触发器参数【{6}】 
触发器日历名称【{7}】 最后执行时间【{8}】 下次执行时间【{9}】 作业执行时间【{10}】 触发的实际时间【{11}】 context中下次执行时间【{12}】"
                    , Name
                    , trigger.Key.Group
                    , trigger.Key.Name
                    , trigger.Description
                    , trigger.JobKey.Group
                    , trigger.JobKey.Name
                    , Newtonsoft.Json.JsonConvert.SerializeObject(trigger.JobDataMap)
                    , trigger.CalendarName
                    , trigger.FinalFireTimeUtc.HasValue ? trigger.FinalFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty //最终执行时间
                    , trigger.GetFireTimeAfter(DateTimeOffset.Now).HasValue
                        ? trigger.GetFireTimeAfter(DateTimeOffset.Now).Value.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss") : String.Empty //下次执行时间
                    , new DateTime(context.JobRunTime.Ticks)    //作业执行时间
                    , context.FireTimeUtc.LocalDateTime
                    , context.NextFireTimeUtc.HasValue ? context.NextFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty
                    );
            });
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLineDebug2(@"监听器名称【{0}】 触发器【{1}=>{2}】触发丢失 触发器描述【{3}】 作业【{4}=》{5}】 触发器参数【{6}】 
触发器日历名称【{7}】 最后执行时间【{8}】 下次执行时间【{9}】"
                    , Name
                    , trigger.Key.Group
                    , trigger.Key.Name
                    , trigger.Description
                    , trigger.JobKey.Group
                    , trigger.JobKey.Name
                    , Newtonsoft.Json.JsonConvert.SerializeObject(trigger.JobDataMap)
                    , trigger.CalendarName
                    , trigger.FinalFireTimeUtc.HasValue ? trigger.FinalFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty //最终执行时间
                    , trigger.GetFireTimeAfter(DateTimeOffset.Now).HasValue
                        ? trigger.GetFireTimeAfter(DateTimeOffset.Now).Value.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss") : String.Empty //下次执行时间
                        );
            });
        }

        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new TaskFactory().StartNew(() =>
            {
                ConsoleExtend.WriteLineDebug(@"监听器名称【{0}】 触发器【{1}=>{2}】作业取消执行 触发器描述【{3}】 作业【{4}=》{5}】 触发器参数【{6}】 
触发器日历名称【{7}】 最后执行时间【{8}】 下次执行时间【{9}】 作业执行时间【{10}】 触发的实际时间【{11}】 context中下次执行时间【{12}】"
                    , Name
                    , trigger.Key.Group
                    , trigger.Key.Name
                    , trigger.Description
                    , trigger.JobKey.Group
                    , trigger.JobKey.Name
                    , Newtonsoft.Json.JsonConvert.SerializeObject(trigger.JobDataMap)
                    , trigger.CalendarName
                    , trigger.FinalFireTimeUtc.HasValue ? trigger.FinalFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty //最终执行时间
                    , trigger.GetFireTimeAfter(DateTimeOffset.Now).HasValue
                        ? trigger.GetFireTimeAfter(DateTimeOffset.Now).Value.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss") : String.Empty //下次执行时间
                    , new DateTime(context.JobRunTime.Ticks)    //作业执行时间
                    , context.FireTimeUtc.LocalDateTime
                    , context.NextFireTimeUtc.HasValue ? context.NextFireTimeUtc.Value.LocalDateTime.ToString() : String.Empty
                        );
                return false;
            });
        }
    }
}
