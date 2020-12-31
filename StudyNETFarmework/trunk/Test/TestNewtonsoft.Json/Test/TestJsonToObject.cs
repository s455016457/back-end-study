using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNewtonsoft.Json.Test
{

    [ClassDiscrible("Json字符串转成Object")]
    public class TestJsonToObject : ITest
    {
        readonly string strJson1 = "{\"a\":123,\"b\":456,\"c\":[{\"a\":12},{\"a\":23}]}";
        readonly string strJson2 = "{\"a\":123,\"b\":456,\"c\":[\"abc\",\"def\"]}";
        readonly string strJson3 = "\"a\"";
        readonly string strJson4 = "123";
        readonly string strJson5 = "true";
        readonly string strJson6 = "false";
        readonly string strJson7 = "{\"ponum\":123,\"venid\":456,\"detail\":[{\"ponum\":12,\"itemnum\":\"itemnum1\",\"qty\":20}" +
            ",{\"ponum\":12,\"itemnum\":\"itemnum2\",\"qty\":30},{\"ponum\":12,\"itemnum\":\"itemnum3\",\"qty\":40}]}";

        public Task RunAsync()
        {
            return Task.Factory.StartNew(Test);
        }

        private void Test()
        {
            var strJsonObject1 = JsonConvert.DeserializeObject<Object>(strJson1);
            var strJsonObject2 = JsonConvert.DeserializeObject<Object>(strJson2);
            var strJsonObject3 = JsonConvert.DeserializeObject<Object>(strJson3);
            var strJsonObject4 = JsonConvert.DeserializeObject<Object>(strJson4);
            var strJsonObject5 = JsonConvert.DeserializeObject<Object>(strJson5);
            var strJsonObject6 = JsonConvert.DeserializeObject<Object>(strJson6);
            var strJsonObject7 = JsonConvert.DeserializeObject<Object>(strJson7);

            Console.WriteLine($"Json字符串：{strJson1} 转换成object 的结果为：");
            WriteObject(strJsonObject1);
            Console.WriteLine("");
            Console.WriteLine($"Json字符串：{strJson2} 转换成object 的结果为：");
            WriteObject(strJsonObject2);
            Console.WriteLine("");
            Console.WriteLine($"Json字符串：{strJson3} 转换成object 的结果为：");
            WriteObject(strJsonObject3);
            Console.WriteLine("");
            Console.WriteLine($"Json字符串：{strJson4} 转换成object 的结果为：");
            WriteObject(strJsonObject4);
            Console.WriteLine("");
            Console.WriteLine($"Json字符串：{strJson5} 转换成object 的结果为：");
            WriteObject(strJsonObject5);
            Console.WriteLine("");
            Console.WriteLine($"Json字符串：{strJson6} 转换成object 的结果为：");
            WriteObject(strJsonObject6);
            Console.WriteLine("");
            Console.WriteLine($"Json字符串：{strJson7} 转换成object 的结果为：");
            WriteObject(strJsonObject7);
            Console.WriteLine("");
        }

        private void WriteObject(object value)
        {
            if (value.GetType().IsValueType || value is string || value is Newtonsoft.Json.Linq.JValue)
            {
                Console.Write(value);
            }
            else if (value is Newtonsoft.Json.Linq.JObject)
            {
                var temp = value as Newtonsoft.Json.Linq.JObject;
                Console.WriteLine("{");
                foreach (Newtonsoft.Json.Linq.JProperty item in temp.Properties())
                {
                    Console.Write(item.Name + ":");
                    WriteObject(item.Value);
                    Console.WriteLine(",");
                }
                Console.WriteLine("}");
            }
            else if (value is Newtonsoft.Json.Linq.JArray)
            {
                var temp = value as Newtonsoft.Json.Linq.JArray;
                Console.WriteLine("[");
                foreach (var item in temp)
                {
                    WriteObject(item);
                    Console.Write(",");
                }
                Console.WriteLine("]");
            }
            //Console.WriteLine($"value 的类型为：{value.GetType().FullName}");
        }
    }
}
