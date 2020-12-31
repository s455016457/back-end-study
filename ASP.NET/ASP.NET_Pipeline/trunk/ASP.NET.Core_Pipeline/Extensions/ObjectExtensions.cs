using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET.Core_Pipeline.Extensions
{
    public static class ObjectExtensions
    {
        const string br = "\n";
        const string tab = "    ";
        /// <summary>
        /// 对象转为json字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="level">层级</param>
        /// <returns></returns>
        public static string ToJson(this object obj, string propertyName = "", int level = 0)
        {
            if (obj == null) return $"{GetJsonPropertyName(propertyName, level)}: undefined";
            var type = obj.GetType();

            if (obj is string)
            {
                return (obj as string).ToJson(propertyName, level);
            }
            if (obj is StringBuilder)
            {
                return (obj as StringBuilder).ToJson(propertyName, level);
            }
            if (type.IsValueType)
            {
                return obj.ValueToJson(propertyName, level);
            }

            if (obj is IEnumerable)
            {
                var ie = obj as IEnumerable;
                return ie.ToJson(propertyName, level);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{GetJsonPropertyName(propertyName, level)}{{");
            foreach (var item in type.GetProperties())
            {
                sb.AppendLine(item.GetValue(obj).ToJson(item.Name, level + 1) + ",");
            }
            sb.Append($"{GetTabStr(level)}}}");

            return sb.ToString();
        }

        public static string ValueToJson(this object obj, string propertyName = "", int level = 0)
        {
            if (obj.GetType().IsValueType)
            {
                if(obj is DateTime)
                    return $"{GetJsonPropertyName(propertyName, level)} \"{obj.ToString()}\"";

                return $"{GetJsonPropertyName(propertyName, level)} {obj.ToString()}";
            }
            throw new Exception("传入参数不是值类型");
        }

        public static string ToJson(this string str, string propertyName = "", int level = 0)
        {
            return $"{GetJsonPropertyName(propertyName, level)} \"{str}\"";
        }

        public static string ToJson(this StringBuilder sb, string propertyName = "", int level = 0)
        {
            return $"{GetJsonPropertyName(propertyName, level)} \"{sb.ToString()}\"";
        }

        public static string ToJson(this IEnumerable enumerable, string propertyName = "", int level = 0)
        {
            if (enumerable == null) return $"{GetJsonPropertyName(propertyName, level)}: undefined";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{GetJsonPropertyName(propertyName, level)}[");
            foreach (var item in enumerable)
            {
                sb.AppendLine(item.ToJson("", level + 1) + ",");
            }
            sb.Append($"{GetTabStr(level)}]");
            return sb.ToString();
        }

        private static string GetJsonPropertyName(string propertyName,int level)
        {
            return $"{(propertyName.Length > 0 ? GetTabStr(level) + propertyName + ":" : GetTabStr(level))}";
        }

        private static string GetTabStr(int level)
        {
            return "".PadLeft(tab.Length * level, ' ');
        }
    }
}
