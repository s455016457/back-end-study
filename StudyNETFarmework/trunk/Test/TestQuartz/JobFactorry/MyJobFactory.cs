using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuartz.JobFactory
{
    /// <summary>
    /// Job 工厂
    /// </summary>
    public class MyJobFactory : IJobFactory
    {
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            // 使用默认构造函数创建实体
            var job = Activator.CreateInstance(bundle.JobDetail.JobType);

            //Type[] types = new Type[0];
            //var job = bundle.JobDetail.JobType.GetConstructor(types: types).Invoke(types);

            JobDataMap jobDataMap = bundle.JobDetail.JobDataMap;

            foreach (var property in bundle.JobDetail.JobType.GetProperties())
            {
                property.SetValue(job,jobDataMap[property.Name]);
            }

            return job as Quartz.IJob;
        }

        public void ReturnJob(Quartz.IJob job)
        {
        }        
    }
}
