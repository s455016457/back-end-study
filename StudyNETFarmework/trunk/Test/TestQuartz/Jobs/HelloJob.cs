using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuartz.Jobs
{
    [DisallowConcurrentExecution]
    public class HelloJob : Quartz.IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return new TaskFactory().StartNew(() => {
                try
                {
                    var b = 0;
                    var a = 100 / b;
                    Console.WriteLine("Hello world！ time:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                catch (Exception ex)
                {
                    throw new JobExecutionException(ex);
                }
            });
        }
    }
}
