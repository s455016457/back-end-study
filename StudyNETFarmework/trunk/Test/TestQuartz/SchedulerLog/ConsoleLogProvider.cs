using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuartz.SchedulerLog
{
    public class ConsoleLogProvider : ILogProvider
    {
        public Logger GetLogger(string name)
        {
            return (logLevel, messageFunc, exception, formatParameters) =>
            {
                if (logLevel < LogLevel.Info) return true;
                if (exception != null)
                {
                    Console.WriteLine("【4】 【5】 【{0}】:【{1}】 参数【{2}】 异常信息【{3}】"
                        , logLevel
                        , messageFunc()
                        , Newtonsoft.Json.JsonConvert.SerializeObject(formatParameters)
                        , exception.Message
                        ,DateTime.Now.ToLongTimeString()
                        ,name);
                }
                else
                    Console.WriteLine("【{3}】 【{4}】 【{0}】:【{1}】 参数【{2}】"
                        , logLevel
                        , messageFunc!=null? messageFunc.Invoke():String.Empty
                        , Newtonsoft.Json.JsonConvert.SerializeObject(formatParameters)
                        , DateTime.Now.ToLongTimeString()
                        ,name);
                return true;
            };
        }

        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }
    }
}
