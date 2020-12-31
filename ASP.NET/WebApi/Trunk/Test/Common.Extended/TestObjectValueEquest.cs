using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extended;

namespace Test.Common.Extended
{
    public class TestObjectValueEquest : BaseTest
    {
        public override void Test()
        {
            {
                ICollection<b> list = new HashSet<b>();
                var a1 = new a()
                {
                    id = "idstr",
                    name = "namestr",
                    age = 1,
                    list = list
                };

                list.Add(new b()
                {
                    id = "bid1",
                    name = "bName1",
                    message = "bMessage1"
                });

                list.Add(new b()
                {
                    id = "bid2",
                    name = "bName2",
                    message = "bMessage1"
                });

                list = new List<b>();
                var a2 = new a()
                {
                    id = "idstr",
                    name = "namestr",
                    age = 1,
                    list = list
                };
                list.Add(new b()
                {
                    id = "bid1",
                    name = "bName1",
                    message = "bMessage1"
                });

                list.Add(new b()
                {
                    id = "bid2",
                    name = "bName2",
                    message = "bMessage1"
                });

                //WriteLine(a1.ValueEquest(a2));
            }

            {
                WriteLine("============开始泛型测试============");
                ICollection<object> ic1 = new List<object>();
                ICollection<a> ic2 = new List<a>();

                ic1.Add(new a()
                {
                    id = "id1",
                    age = 1,
                    name = "name1"
                });
                ic1.Add(new a()
                {
                    id = "id2",
                    age = 2,
                    name = "name2"
                });
                ic2.Add(new a()
                {
                    id = "id1",
                    age = 1,
                    name = "name1"
                });
                ic2.Add(new a()
                {
                    id = "id2",
                    age = 2,
                    name = "name2"
                });

                ICollection<object> ic2Toic1 = ((IEnumerable<object>)ic2).ToList();
                WriteLine("ICollection<a> 转换为 ICollection<object>:{0}", ic2Toic1);
                ICollection<a> ic1Toic2 = ((IEnumerable<a>)ic1).ToList();
                WriteLine("ICollection<object> 转换为 ICollection<a>:{0}", ic1Toic2);

                WriteLine("============泛型测试结束============");
            }
        }
    }

    public class a
    {
        public String id { get; set; }

        public String name { get; set; }

        public int age { get; set; }

        public ICollection<b> list { get; set; }

    }

    public class b
    {
        public String id { get; set; }
        public String name { get; set; }

        public string message { get; set; }
    }
}
