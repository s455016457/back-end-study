using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET.Core_Pipeline.Extensions
{
    public static class ConsoleExtensions
    {
        public static void WriteObject<T>(T t, string prefix = "") where T : class
        {
            if (t == null)
            {
                Console.WriteLine($"{prefix}{(prefix.Length > 0 ? ":" : string.Empty)}{DBNull.Value.ToString()}");
                return;
            }
            var type = t.GetType();
            if (type.IsValueType || t is string || t is System.Text.StringBuilder)
            {
                Console.WriteLine($"{prefix}{(prefix.Length > 0 ? ":" : string.Empty)}{t.ToString()}");
            }
            else if (t is IEnumerable)
            {
                var ie = t as IEnumerable;
                var index = 0;
                foreach (var item in ie)
                {
                    WriteObject(item, $"{prefix}_{index++}");
                }
            }
            else
            {
                foreach (var item in type.GetProperties())
                {
                    WriteObject(item.GetValue(t), prefix.Length > 0 ? $"{prefix}.{ item.Name}" : item.Name);
                }
            }
        }
    }
}
