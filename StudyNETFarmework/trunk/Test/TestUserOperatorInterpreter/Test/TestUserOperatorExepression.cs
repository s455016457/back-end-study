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
    [ClassDiscrible("用户操作日志解析")]
    public class TestUserOperatorExepression : ITest
    {
        readonly string strJson = "{\"ponum\":\"123{venid}\",\"venid\":\"456{idate}{ponum}\",\"ven\":{\"venname\":\"venname123456\"},\"idate\":\"2018-10-11\",\"detail\":[" +
            "{\"ponum\":12,\"itemnum\":\"itemnum1\",\"qty\":20,\"uom\":{\"uom\":\"g\",\"exuom\":\"kg\",\"rate\":1000}}" +
            ",{\"ponum\":12,\"itemnum\":\"itemnum2\",\"qty\":30,\"uom\":{\"uom\":\"g\",\"exuom\":\"包\",\"rate\":2000}}" +
            ",{\"ponum\":12,\"itemnum\":\"itemnum3\",\"qty\":40,\"uom\":{\"uom\":\"g\",\"exuom\":\"瓶\",\"rate\":5000}}]}";

        readonly string strJson2 = "{\"ponum\":\"123{venid}\",\"venid\":\"456{idate}{ponum}\",\"ven\":{\"venname\":\"venname123456\"},\"idate\":\"2018-10-11\"" +
            ",\"detail\":[\"abc\",\"def\"]}";

        readonly string userOperatorActionFormat = "新增采购订单明细采购单号【{ponum}】单据日期【{idate}】供应商【{venid}】供应商名称【{ven.venname}】" +
            "[detail=>物料编码【{itemnum}】数量【{qty}】单位【{uom.exuom}】]！";

        readonly string userOperatorActionFormat2 = "新增采购订单明细采购单号【{ponum}】单据日期【{idate}】供应商【{venid}】供应商名称【{ven.venname}】" +
            "[detail=>物料编码【{this}】]！";
        public Task RunAsync()
        {
            return Task.Factory.StartNew(()=>{ Test();Test2(); });
        }

        private void Test()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var exepression = new UserOperatorExepression();
            UserOperatorInterpreterContext context = null; ;
            int count = 100000;
            for (int i = 0; i < count; i++)
            {
                context = new UserOperatorInterpreterContext(userOperatorActionFormat, JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(strJson));
                exepression.Interpreter(context);
            }
            stopwatch.Stop();
            Console.WriteLine($"解析{count}次用时【{stopwatch.Elapsed.TotalMilliseconds}】毫秒");
            Console.WriteLine("解析结果为：");
            foreach (var item in context.Result)
            {
                Console.WriteLine(item);
            }
        }
        private void Test2()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var exepression = new UserOperatorExepression();
            UserOperatorInterpreterContext context = null; ;
            int count = 100000;
            for (int i = 0; i < count; i++)
            {
                context = new UserOperatorInterpreterContext(userOperatorActionFormat2, JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(strJson2));
                exepression.Interpreter(context);
            }
            stopwatch.Stop();
            Console.WriteLine($"解析{count}次用时【{stopwatch.Elapsed.TotalMilliseconds}】毫秒");
            Console.WriteLine("解析结果为：");
            foreach (var item in context.Result)
            {
                Console.WriteLine(item);
            }
        }
    }
}
