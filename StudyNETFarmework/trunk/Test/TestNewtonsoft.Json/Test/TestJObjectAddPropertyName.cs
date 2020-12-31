using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNewtonsoft.Json.Test
{
    [ClassDiscrible("JObject对象添加属性")]
    public class TestJObjectAddPropertyName : ITest
    {
        readonly string strJson1 = "{\"ponum\":123,\"venid\":456,\"detail\":[{\"ponum\":12,\"itemnum\":\"itemnum1\",\"qty\":20}" +
            ",{\"ponum\":12,\"itemnum\":\"itemnum2\",\"qty\":30},{\"ponum\":12,\"itemnum\":\"itemnum3\",\"qty\":40}]}";
        readonly string strJson2 = "{\"a\":123,\"b\":456,\"c\":[{\"a\":12},{\"a\":23}]}";
        readonly string strJson3 = "{\"a\":123,\"b\":456,\"c\":[\"abc\",\"def\"],\"d\":[123,123456]}";

        public Task RunAsync()
        {
            return Task.Factory.StartNew(Test);
        }

        private void Test()
        {
            var strJsonObject1 = JsonConvert.DeserializeObject<JObject>(strJson1);
            var strJsonObject2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson2);
            var strJsonObject3 = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson3);

            foreach (var item in strJsonObject2)
            {
                strJsonObject1.Add(new JProperty(item.Key, item.Value));
            }
            foreach (var item in strJsonObject3)
            {
                if (!strJsonObject1.ContainsKey(item.Key))
                    strJsonObject1.Add(new JProperty(item.Key, item.Value));
            }
            Console.WriteLine(strJsonObject1);
        }
    }
}
