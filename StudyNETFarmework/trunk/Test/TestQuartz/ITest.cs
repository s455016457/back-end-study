using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuartz
{
    public interface ITest
    {
        Task RunAsync();
    }
    /// <summary>
    /// 类描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassDiscribleAttribute : Attribute
    {
        public String Discrible { get; private set; }

        public ClassDiscribleAttribute(String discrible)
        {
            Discrible = discrible;
        }
    }
}
