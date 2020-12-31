using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Extend;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;

namespace TestQuartz.Plugin
{
    public class LoggingTriggerHistoryPlugin : ISchedulerPlugin, ITriggerListener
    {
        public string Name { get; private set; }

        public Task Initialize(string pluginName, IScheduler scheduler, CancellationToken cancellationToken = default(CancellationToken))
        {
            Name = pluginName;
            scheduler.ListenerManager.AddTriggerListener(this, EverythingMatcher<TriggerKey>.AllTriggers());
            return Task.FromResult(true);
        }

        public Task Shutdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }

        public Task Start(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            ConsoleExtend.WriteLineRed($"插件【{Name}】 触发器【{trigger.Key}】 触发作业【{context.JobDetail.Key}】完成 触发时间【{SystemTime.UtcNow()}】 下次触发时间【{trigger.GetNextFireTimeUtc()}】");
            return Task.FromResult(true);
        }

        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            ConsoleExtend.WriteLineRed($"插件【{Name}】 触发器【{trigger.Key}】 触发作业【{context.JobDetail.Key}】 触发时间【{SystemTime.UtcNow()}】");
            return Task.FromResult(true);
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            ConsoleExtend.WriteLineRed($"插件【{Name}】 触发器【{trigger.Key}】 触发失败 触发时间【{SystemTime.UtcNow()}】 下次触发时间【{trigger.GetNextFireTimeUtc()}】");
            return Task.FromResult(true);
        }

        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            ConsoleExtend.WriteLineRed($"插件【{Name}】 触发器【{trigger.Key}】 取消作业【{context.JobDetail.Key}】执行:false 触发时间【{SystemTime.UtcNow()}】 下次触发时间【{trigger.GetNextFireTimeUtc()}】");
            return Task.FromResult(false);
        }
    }
}
