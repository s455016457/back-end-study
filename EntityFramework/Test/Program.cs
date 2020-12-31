using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyEntityFramework;
using StudyEntityFramework2.DBContext;
using StudyEntityFramework2.Model;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("====================使用现有数据库=========================");
            //Console.Write("输入新博客：");
            //var name = Console.ReadLine();

            //var blogs = new BlogService();
            //var blog = new Blogs { Name = name };
            //blogs.Add(blog);

            //var query = blogs.Query();
            //Console.WriteLine("所有数据：");
            //foreach (var item in query)
            //{
            //    Console.WriteLine(item.Name);
            //}

            //Console.WriteLine("====================创建新数据库=========================");
            //Console.Write("输入新博客：");
            //var name2 = Console.ReadLine();

            //var blogs2 = new StudyEntityFramework2.Services();
            //var blog2 = new StudyEntityFramework2.Blog { Name = name2 };
            //blogs2.AddBlog(blog2);

            //var query2 = blogs2.QueryBlog();
            //Console.WriteLine("所有数据：");
            //foreach (var item in query2)
            //{
            //    Console.WriteLine(item.Name);
            //}

            Console.WriteLine(Test.test != null);

            Test test1 = new Test("这里是Test1初始化的值！");
            Test test2 = new Test("这里是Test2初始化的值！");

            Console.WriteLine("test1输出：{0}", test1.GetTest2().Name);
            Console.WriteLine("test2输出：{0}", test1.GetTest2().Name);
            Console.WriteLine("test1.GetTest2().Equals(test2.GetTest2()):{0}", test1.GetTest2().Equals(test2.GetTest2()));
            
            test1 = new Test_1("这里是Test1初始化的值！");
            test2 = new Test_2("这里是Test2初始化的值！");

            Console.WriteLine("test_1输出：{0}", test1.GetTest2().Name);
            Console.WriteLine("test_2输出：{0}", test1.GetTest2().Name);
            Console.WriteLine("test1.GetTest2().Equals(test2.GetTest2()):{0}", test1.GetTest2().Equals(test2.GetTest2()));

            //{
            //    Random random = new Random();
            //    /// <summary>
            //    /// 用于产生随机密码
            //    /// </summary>
            //    string temp = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //    for (int i = 0; i < 10; i++)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        int length = 9;
            //        while (length > 0)
            //        {
            //            sb.Append(temp[random.Next(0, temp.Length)]);
            //            length--;
            //        }

            //        Console.WriteLine(sb.ToString());
            //    }
            //}

            {
                List<Rectangle> list = new List<Rectangle>()
                {
                    new Rectangle{ Width=3,Height=4},
                    new Rectangle{ Width=3,Height=5},
                    new Rectangle{ Width=4,Height=5},
                };

                var rectagle1 = list.FirstOrDefault(p => p.Width == 2);
                var rectagle2 = list.FirstOrDefault(p => p.Width == 3);

                Console.WriteLine("rectagle1：{0},{1}", rectagle1.Width, rectagle1.Height);
                Console.WriteLine("rectagle2：{0},{1}", rectagle2.Width, rectagle2.Height);

                rectagle1.Width = 2;
                rectagle1.Height = 10;
                rectagle1 = list.FirstOrDefault(p => p.Width == 2);
                Console.WriteLine("rectagle1：{0},{1}", rectagle1.Width, rectagle1.Height);

                rectagle2.Width = 3;
                rectagle2.Height = 10;
                rectagle2 = list.FirstOrDefault(p => p.Width == 3);
                Console.WriteLine("rectagle2：{0},{1}", rectagle2.Width, rectagle2.Height);
            }

            {
                List<SysFunctionAggregate> list = null;
                using (SysMenuDBContext sysMenuDBContext = new SysMenuDBContext("data source =.; initial catalog = DBStudy; user id = swerp; password = swerp2008; "))
                {
                    //MenuGroups必须是Iconllection接口的实现类
                    //Include 同时查询外表数据
                    list = sysMenuDBContext.SysFunctions./*Include("MenuGroups").*/ToList();
                    foreach (var sysFunction in list)
                    {
                        if (sysFunction.MenuGroups != null && sysFunction.MenuGroups.Count > 0)
                        {
                            foreach (var menuGrou in sysFunction.MenuGroups)
                            {
                                Console.WriteLine("功能ID{0}，功能名称{1}，菜单类别{4}，菜单组ID{2}，菜单组名称{3}"
                                    , sysFunction.SysFunction_Id, sysFunction.SysFunction.Name, menuGrou.MenuGroup_Id, menuGrou.Name, sysFunction.SysFunction.MenuType);
                            }
                        }
                        else
                        {
                            Console.WriteLine("功能ID{0}，功能名称{1}，菜单类别{2}"
                                   , sysFunction.SysFunction_Id, sysFunction.SysFunction.Name, sysFunction.SysFunction.MenuType);
                        }
                    }
                }

                using (SysMenuDBContext sysMenuDBContext = new SysMenuDBContext("data source =.; initial catalog = DBStudy; user id = swerp; password = swerp2008; "))
                {
                    foreach (var sysFunction in list)//Include
                    {
                        sysFunction.SysFunction.Name += "1";// 修改失败
                    }
                    sysMenuDBContext.SaveChanges();
                }
                using (SysMenuDBContext sysMenuDBContext = new SysMenuDBContext("data source =.; initial catalog = DBStudy; user id = swerp; password = swerp2008; "))
                {
                    list = sysMenuDBContext.SysFunctions.Include("MenuGroups").ToList(); //重新获取list，修改成功
                    foreach (var sysFunction in list)//Include 同时查询外表数据
                    {
                        sysFunction.SysFunction.Name += "2";
                    }
                    sysMenuDBContext.SaveChanges();
                }
            }

            {
                List<String> list = new List<string> {
                    "AA",
                    "BB",
                    "CC",
                };
                List<String> list2 = new List<string> {
                    "BB",
                    "CC",
                    "DD",
                };

                var newlist = list.Intersect(list2);

                foreach (var v in newlist)
                {
                    Console.WriteLine(v);
                }
            }

            Console.WriteLine("输入任意键....");
            Console.ReadKey();
        }
    }

    public struct Rectangle
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
    }

    public class Test {
        public static Test test;
        private static Test2 test2 { get; set; }

        public Test2 GetTest2()
        {
            return test2;
        }

        public Test(String Name)
        {
            if (test2 == null)
            {
                test2 = new Test2(Name);
            }
        }
    }

    public class Test2
    {
        public String Name { get;private set; }

        public Test2(String Name)
        {
            this.Name = Name;
        }
    }

    public class Test_1 : Test
    {
        public Test_1(string Name) : base(Name)
        {
        }
    }

    public class Test_2 : Test
    {
        public Test_2(string Name) : base(Name)
        {
        }
    }
}
