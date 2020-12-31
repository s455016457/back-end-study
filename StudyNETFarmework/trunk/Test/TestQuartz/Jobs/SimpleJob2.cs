using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuartz.Jobs
{
    public class SimpleJob2 : Quartz.IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("SimpleJob2！ time:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return Task.FromResult(true);
        }
    }
}
