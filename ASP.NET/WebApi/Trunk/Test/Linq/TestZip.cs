using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Linq
{
    public class TestZip : BaseTest
    {
        IList<a> list1;
        IList<a> list2;

        public TestZip()
        {
            initData();
        }
    
        private void initData()
        {
            #region 准备数据
            list1 = new List<a>();
            list2 = new List<a>();

            var a1 = new a()
            {
                Id = "id1",
                Name = "Name1",
                Pwd = "Pwd1"
            };

            var a2 = new a()
            {
                Id = "id2",
                Name = "Name2",
                Pwd = "Pwd2"
            };

            var a3 = new a()
            {
                Id = "id3",
                Name = "Name3",
                Pwd = "Pwd3"
            };

            var a4 = new a()
            {
                Id = "id4",
                Name = "Name4",
                Pwd = "Pwd4"
            };

            var a5 = new a()
            {
                Id = "id5",
                Name = "Name5",
                Pwd = "Pwd5"
            };

            var a6 = new a()
            {
                Id = "id6",
                Name = "Name6",
                Pwd = "Pwd6"
            };

            var a7 = new a()
            {
                Id = "id7",
                Name = "Name7",
                Pwd = "Pwd7"
            };
            
            
            list1.Add(a1);
            list1.Add(a2);
            list1.Add(a3);
            list1.Add(a4);
            list1.Add(a5);

            list2.Add(a4);
            list2.Add(a5);
            list2.Add(a6);
            list2.Add(a7);

            #endregion
        }

        public override void Test()
        {
            var list1EquestList2 = list1.Zip(list2, (a, b) => { if (a.Id.Equals(b.Id)) { return a; } else { return null; } }).Where(p => p != null);
            var list1NotEquestList2 = list1.Zip(list2, (a, b) => { if (!a.Id.Equals(b.Id)) { return b; } else { return null; } }).Where(p => p != null);

            var list1InList2 = list1.Where(p => list2.FirstOrDefault(p2 => p2.Id.Equals(p.Id)) != null);
            var list1NotInList2 = list1.Where(p => list2.FirstOrDefault(p2 => p2.Id.Equals(p.Id)) == null);
            var list2NotInList1 = list2.Where(p => list1.FirstOrDefault(p2 => p2.Id.Equals(p.Id)) == null);

            var union = list1.Union(list2);

            WriteLine("**************Begin List1中元素ID与List2中元素ID相等**************");
            foreach (var item in list1EquestList2)
            {
                WriteLine(item.ToString());
            }
            WriteLine("**************End List1中元素ID与List2中元素ID相等**************");

            WriteLine("**************Begin List1中元素ID与List2中元素ID不相等**************");
            foreach (var item in list1NotEquestList2)
            {
                WriteLine(item.ToString());
            }
            WriteLine("**************End List1中元素ID与List2中元素ID不相等**************");

            WriteLine("**************Begin List1 Union List2**************");
            foreach (var item in union)
            {
                WriteLine(item.ToString());
            }
            WriteLine("**************End List1 Union List2**************");

            WriteLine("**************Begin List1 In List2**************");
            foreach (var item in list1InList2)
            {
                WriteLine(item.ToString());
            }
            WriteLine("**************End List1 In List2**************");

            WriteLine("**************Begin List1 NotIn List2**************");
            foreach (var item in list1NotInList2)
            {
                WriteLine(item.ToString());
            }
            WriteLine("**************End List1 NotIn List2**************");

            WriteLine("**************Begin List2 NotIn List1**************");
            foreach (var item in list2NotInList1)
            {
                WriteLine(item.ToString());
            }
            WriteLine("**************End List2 NotIn List1**************");
        }
    }

    public class a
    {
        public String Id { get; set; }

        public String Name { get; set; }

        public String Pwd { get; set; }

        public override string ToString()
        {
            return String.Format("class：【{0}】：Id:【{1}】Name:【{2}】Pwd:【{3}】", this.GetType().ToString(), Id, Name, Pwd);
        }
    }
}
