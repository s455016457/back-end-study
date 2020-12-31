using Domain.Event;
using System;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using Test.Common.Extended;
using Test.Dao.Ef;
using Test.DDD.Test;
using Test.Domain.Repository;
using Test.DomainEvent;
using Test.lambda;
using Test.Linq;
using Test.TestDomainEvent;

namespace Test
{
    class Program
    {

        static void Main(string[] args)
        {
            {
                BaseTest test = new DomainEventTest();
                //test.Test();
            }
            {
                BaseTest test = new DomainEventTest2();
                //test.Test();
            }
            {
                BaseTest test = new ItemTypeRepositoryTest();
                //test.Test();
            }
            {
                BaseTest test = new TestObjectValueEquest();
                //test.Test();
            }
            {
                BaseTest test = new TestZip();
                //test.Test();
            }
            {
                BaseTest test = new LambdaTest1();
                //test.Test();
            }
            {
                BaseTest test = new LambdaTest2();
                //test.Test();
            }
            {
                BaseTest test = new TestQueryExtensions();
                //test.Test();
            }
            {
                BaseTest test = new ParsingExpressionTreesTest();
                test.Test();
            }
            //{
            //    BaseTest test = new BuildDynamicQueriesTest1();
            //    test.Test();
            //}
            //{
            //    BaseTest test = new BuildDynamicQueriesTest2();
            //    test.Test();
            //}
            //{
            //    BaseTest test = new TestTransaction();
            //test.Test();
            //}
            {
                BaseTest test = new TestUpdateTransaction();
                //test.Test();
            }
            {
                BaseTest test = new TestOptimisticLock();
                //test.Test();
            }
            {
                BaseTest test = new TestEF();
                test.Test();
            }
            {
                Console.WriteLine(typeof(String).IsValueType);
                Console.WriteLine(typeof(int).IsValueType);
                Console.WriteLine(typeof(DateTime).IsValueType);
                Console.WriteLine(typeof(decimal).IsValueType);
                Console.WriteLine(typeof(float).IsValueType);
                Console.WriteLine(typeof(object).IsValueType);
            }
            {
                a a = new a();
                foreach (var item in a.GetType().GetProperties()) {
                    if (item.Name.Equals("Id"))
                        item.SetValue(a, "Id");
                    else if (item.Name.Equals("age"))
                        item.SetValue(a, Convert.ChangeType("13", item.PropertyType));
                    else if (item.Name.Equals("CreatedStapm"))                        
                        item.SetValue(a, Convert.ChangeType("2017-06-22 13:21:45",item.PropertyType));
                }
            }
            Console.ReadKey();
        }
        class a
        {
            public String Id { get; set; }
            public int age { get; set; }
            public DateTime CreatedStapm { get; set; }
        }
    }

}
