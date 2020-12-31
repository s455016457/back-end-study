using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOperatorInterpreter
{
    /// <summary>
    /// 实体表达式
    /// </summary>
    public class EntityExepression : IEntityExepression
    {
        private List<FormatKey> keys = null;
        private Dictionary<string, string> keyValues = null;
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="context"></param>
        public void Interpreter(EntityInterpreterContext context)
        {
            keys = new List<FormatKey>();
            keyValues = new Dictionary<string, string>();
            SetFormateKey(context.Format);
            SetFormateValue(context.Data);
            SetResult(context);
        }
        /// <summary>
        /// 设置格式Key
        /// </summary>
        /// <param name="strFormate"></param>
        private void SetFormateKey(string strFormate)
        {
            int startIndex = 0;
            int beginIndex = strFormate.IndexOf('{', startIndex);
            while (beginIndex >= 0)
            {
                int endIndex = strFormate.IndexOf('}', startIndex);
                var key = strFormate.Substring(beginIndex, (endIndex - beginIndex) + 1);
                FormatKey formatKey = new FormatKey
                {
                    key = key,
                    beginIndex = beginIndex,
                    endIndex = endIndex
                };
                keys.Add(formatKey);
                startIndex = endIndex + 1;
                beginIndex = strFormate.IndexOf('{', startIndex);
            }
        }
        /// <summary>
        /// 设置格式值
        /// </summary>
        /// <param name="data"></param>
        private void SetFormateValue(object data)
        {
            foreach(var formateKey in keys)
            {
                if (keyValues.Keys.Contains(formateKey.key)) continue;
                var name = formateKey.key.Substring(1, formateKey.key.Length - 2);
                keyValues.Add(formateKey.key, GetObjectValue(data, name).ToString());
            }
        }
        /// <summary>
        /// 获取对象值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private object GetObjectValue(object data,string name)
        {
            if (data == null) return string.Empty;
            if (name.Equals("this")) return data;
            string propertyName = name;
            var index = name.IndexOf('.');
            if (index > 0)
            {
                propertyName = name.Substring(0, index);
                name = name.Substring(index + 1);
            }
            /**
            if (data is Newtonsoft.Json.Linq.JObject)
            {
                var jObject = data as Newtonsoft.Json.Linq.JObject;
                if (jObject == null) return string.Empty;
                var value = jObject.GetValue(propertyName);
                if (index > 0)
                {
                    return GetObjectValue(value, name);
                }
                return value == null ? string.Empty : value.ToString();
            }
            else **/if (data is Newtonsoft.Json.Linq.JToken)
            {
                var jToken = data as Newtonsoft.Json.Linq.JToken;
                if (jToken == null) return string.Empty;
                var value = jToken.Value<object>(propertyName);
                if (index > 0)
                {
                    return GetObjectValue(value, name);
                }
                return value == null ? string.Empty : value.ToString();
            }
            else if (data is Dictionary<string, object>)
            {
                var dic = data as Dictionary<string, object>;
                if (dic == null) return string.Empty;
                var value = dic[propertyName];
                if (index > 0)
                {
                    return GetObjectValue(value, name);
                }
                return value == null ? string.Empty : value;
            }
            else if (data is object && !data.GetType().IsValueType && !(data is string))
            {
                var property = data.GetType().GetProperty(propertyName);
                return property.GetValue(data).ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="context"></param>
        private void SetResult(EntityInterpreterContext context)
        {
            context.Result = context.Format;
            int offset = 0;
            foreach (var item in keys)
            {
                var value = keyValues[item.key];
                StringBuilder Result = new StringBuilder(context.Result);
                Result.Replace(item.key, value, item.beginIndex + offset, item.key.Length);
                offset += value.Length - item.key.Length;
                context.Result = Result.ToString();
            }
        }
        class FormatKey
        {
            public string key { get; set; }

            public int beginIndex { get; set; }
            public int endIndex { get; set; }
        }
    }
}
