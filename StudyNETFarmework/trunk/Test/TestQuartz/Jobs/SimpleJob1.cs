using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuartz.Jobs
{
    public class SimpleJob1 : Quartz.IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            Console.WriteLine("SimpleJob1！ time:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
