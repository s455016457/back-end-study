using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOperatorInterpreter
{
    public class UserOperatorExepression : IUserOperatorExepression
    {
        List<Format> formates;
        public void Interpreter(UserOperatorInterpreterContext context)
        {
            formates = new List<Format>();
            SetFormats(context);
            SetFormatValue();
            SetResult(context);
        }

        private void SetFormats(UserOperatorInterpreterContext context)
        {
            string format = context.Format;
            int collectionBeginIndex = format.IndexOf('[');
            int collectionEndIndex = format.IndexOf(']');
            if (collectionBeginIndex > 0 && collectionBeginIndex < collectionEndIndex)
            {
                formates.Add(new Format { Key = format.Substring(0,collectionBeginIndex), Data = context.Data, IsCollection = false });

                string temp = format.Substring(collectionBeginIndex + 1, collectionEndIndex- collectionBeginIndex-1);
                var collectionFormat= temp.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                if (collectionFormat.Length != 2) throw new Exception("明细格式不正确！");
                formates.Add(new Format
                {
                    Key = collectionFormat[1].Trim(),
                    Data = GetObjectValue(context.Data, collectionFormat[0].Trim()),
                    IsCollection = true
                });

                if (collectionEndIndex < format.Length - 1)
                {
                    formates.Add(new Format
                    {
                        Key = format.Substring(collectionEndIndex + 1, format.Length - (collectionEndIndex + 1)),
                        Data = context.Data,
                        IsCollection = false
                    });
                }
            }
            else
            {
                formates.Add(new Format { Key = format, Data = context.Data, IsCollection = false });
            }
        }

        /// <summary>
        /// 获取对象值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private object GetObjectValue(object data, string name)
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
            if (data is Newtonsoft.Json.Linq.JToken)
            {
                var jToken = data as Newtonsoft.Json.Linq.JToken;
                if (jToken == null) return string.Empty;
                var value = jToken.Value<object>(propertyName);
                if (index > 0)
                {
                    return GetObjectValue(value, name);
                }
                return value == null ? string.Empty : value;
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
                return property.GetValue(data);
            }
            return string.Empty;
        }

        private void SetFormatValue()
        {
            var collectionExepression = new CollectionExepression();
            var entityExepression = new EntityExepression();
            foreach (var item in formates)
            {
                if (item.IsCollection)
                {
                    var context= new CollectionInterpreterContext(item.Key, item.Data);
                    collectionExepression.Interpreter(context);
                    item.FormatValue.AddRange(context.Result);
                }
                else
                {
                    var context = new EntityInterpreterContext(item.Key, item.Data);
                    entityExepression.Interpreter(context);
                    item.FormatValue.Add(context.Result);
                }
            }
        }

        private void SetResult(UserOperatorInterpreterContext context)
        {
            var collectionFormat = formates.FirstOrDefault(p => p.IsCollection);
            StringBuilder sbFormat = new StringBuilder();

            foreach (var item in formates)
            {
                if (item.IsCollection)
                    sbFormat.Append("{0}");
                else
                    sbFormat.Append(item.FormatValue.FirstOrDefault().Replace("{","{{").Replace("}","}}"));
            }

            if (collectionFormat == null)
            {
                context.Result.Add(string.Format(sbFormat.ToString()));
            }
            else
            {
                foreach (var item in collectionFormat.FormatValue)
                {
                    string result = string.Format(sbFormat.ToString(), item);
                    context.Result.Add(result);
                }
            }
        }


        class Format
        {
            public string Key { get; set; }

            public object Data { get; set; }

            public List<string> FormatValue { get; set; } = new List<string>();

            public bool IsCollection { get; set; }
        }
    }
}
