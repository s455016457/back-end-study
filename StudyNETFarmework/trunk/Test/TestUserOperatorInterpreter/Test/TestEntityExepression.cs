using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOperatorInterpreter;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TestUserOperatorInterpreter.Test
{
    [ClassDiscrible("实体解析")]
    public class TestEntityExepression : ITest
    {
        readonly string strJson = "{\"ponum\":\"123{venid}\",\"venid\":\"456{idate}{ponum}\",\"ven\":{\"venname\":\"venname123456\"},\"idate\":\"2018-10-11\",\"detail\":[" +
            "{\"ponum\":12,\"itemnum\":\"itemnum1\",\"qty\":20,\"uom\":{\"uom\":\"g\",\"exuom\":\"kg\",\"rate\":1000}}" +
            ",{\"ponum\":12,\"itemnum\":\"itemnum2\",\"qty\":30,\"uom\":{\"uom\":\"g\",\"exuom\":\"包\",\"rate\":2000}}" +
            ",{\"ponum\":12,\"itemnum\":\"itemnum3\",\"qty\":40,\"uom\":{\"uom\":\"g\",\"exuom\":\"瓶\",\"rate\":5000}}]}";

        readonly string userOperatorActionFormat = "新增采购订单明细采购单号【{ponum}】供应商【{venid}】供应商名称【{ven.venname}】单据日期【{idate}】单据【{this}】";
        public Task RunAsync()
        {
            return Task.Factory.StartNew(Test);
        }

        private void Test()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var exepression = new EntityExepression();
            EntityInterpreterContext context = null; ;
            int count = 1;
            for (int i = 0; i < count; i++)
            {
                context = new EntityInterpreterContext(userOperatorActionFormat, JsonConvert.DeserializeObject(strJson));
                exepression.Interpreter(context);
            }
            stopwatch.Stop();
            Console.WriteLine($"解析{count}次用时【{stopwatch.Elapsed.TotalMilliseconds}】毫秒");
            Console.WriteLine("解析结果为：");
            Console.WriteLine(context.Result);
        }
    }
}
