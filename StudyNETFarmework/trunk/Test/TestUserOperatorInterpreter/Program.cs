using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestUserOperatorInterpreter
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now} will run at: {DateTime.Now:r}");//语法糖

            Console.WriteLine("测试执行开始");
            RunTestAsync().Wait();
            Console.WriteLine("测试执行结束");
        }

        private static async Task RunTestAsync()
        {
            //获取程序集的 System.Reflection.Assembly 实例
            var assembly = typeof(Program).Assembly;

            var ITestsType = new List<Type>(GetAssemblyTypes<ITest>(assembly));
            ITestsType.Sort(new TypeNameComparer());
            var count = 1;

            Console.WriteLine("选择要运行的测试(输入exit退出测试)：");

            if (ITestsType.Count <= 0) return;

            foreach (var iTestType in ITestsType)
            {
                ClassDiscribleAttribute classDiscribleAttribute = GetTypeAttribute<ClassDiscribleAttribute>(iTestType);
                Console.WriteLine("{0} {1} {2}"
                    , $"[{count++}]".PadRight(4)
                    , $"{iTestType.Namespace}  {iTestType.Name}".PadRight(50)
                    , classDiscribleAttribute == null ? String.Empty : classDiscribleAttribute.Discrible);
            }

            Console.WriteLine();
            Console.Write("> ");
            String writeText = Console.ReadLine();
            Console.WriteLine("");

            if (writeText.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                Environment.Exit(Environment.ExitCode);

            int num = 0;

            while (!int.TryParse(writeText, out num) || num > ITestsType.Count() || num < 1)
            {
                Console.WriteLine($"请输入正确的数字，范围在1到{ITestsType.Count()}之间");
                Console.Write("> ");
                writeText = Console.ReadLine();
            }

            Type type = ITestsType[num - 1];

            ITest test = (ITest)Activator.CreateInstance(type);

            await test.RunAsync().ConfigureAwait(false);

            Console.ReadKey();
            //await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取程序集中所有类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static Type[] GetAssemblyTypes(Assembly assembly)
        {
            return assembly.GetTypes();
        }

        /// <summary>
        /// 获取程序集中指的所有类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        private static IEnumerable<Type> GetAssemblyTypes<T>(Assembly assembly) where T : class
        {
            Type tType = typeof(T);
            foreach (Type type in GetAssemblyTypes(assembly))
            {
                if (type is T || type.GetInterfaces().ToList().Contains(tType))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// 获取类型指定自定义特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private static T GetTypeAttribute<T>(Type type) where T : Attribute
        {
            return (T)type.GetCustomAttribute(typeof(T));
        }
    }

    class TypeNameComparer : IComparer<Type>
    {
        public int Compare(Type t1, Type t2)
        {
            if (t1.Namespace.Length > t2.Namespace.Length)
            {
                return 1;
            }

            if (t1.Namespace.Length < t2.Namespace.Length)
            {
                return -1;
            }

            return t1.Namespace.CompareTo(t2.Namespace);
        }
    }
}