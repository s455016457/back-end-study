using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestNewtonsoft.Json.Test
{
    [ClassDiscrible("Json字符串转成键值对集合")]
    public class TestJsonToDic:ITest
    {
        readonly string strJson1 = "{\"a\":123,\"b\":456,\"c\":[{\"a\":12},{\"a\":23}]}";
        readonly string strJson2 = "{\"a\":123,\"b\":456,\"c\":[\"abc\",\"def\"]}";

        public Task RunAsync()
        {
            return Task.Factory.StartNew(Test);
        }

        private void Test()
        {
            var strJsonDic1= JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson1);

            Console.WriteLine($"Json字符串：{strJson1} 转换成Dictionary<string, object> 的结果为：");
            foreach (var keyValue in strJsonDic1)
            {
                Console.WriteLine($"key:{keyValue.Key};value:{keyValue.Value}");
                Console.WriteLine($"value.type:{keyValue.Value.GetType()}");
                if (keyValue.Value is Newtonsoft.Json.Linq.JArray)
                {
                    Newtonsoft.Json.Linq.JArray value = keyValue.Value as Newtonsoft.Json.Linq.JArray;
                    foreach (var item in value)
                    {
                        Console.WriteLine(item.GetType());
                    }
                }
            }

            var strJsonDic2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson2);
            Console.WriteLine($"Json字符串：{strJson2} 转换成Dictionary<string, object> 的结果为：");
            foreach (var keyValue in strJsonDic2)
            {
                Console.WriteLine($"key:{keyValue.Key};value:{keyValue.Value}");
                Console.WriteLine($"value.type:{keyValue.Value.GetType()}");
                if (keyValue.Value is Newtonsoft.Json.Linq.JArray)
                {
                    Newtonsoft.Json.Linq.JArray value = keyValue.Value as Newtonsoft.Json.Linq.JArray;
                    foreach (var item in value)
                    {
                        Console.WriteLine(item.GetType());
                    }
                }
            }

        }
    }
}
