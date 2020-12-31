using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public abstract class BaseTest
    {
        protected void WriteLine(Object value)
        {
            Console.WriteLine(value);
        }

        protected void WriteLine(String format,params object[] param)
        {
            Console.WriteLine(format, param);
        }
        public abstract void Test();
    }
}
