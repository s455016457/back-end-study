using Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.TestDomainEvent;

namespace Test.DomainEvent
{
    public class DomainEventTest2 : BaseTest
    {
        /// 事件
        /// </summary>
        public static event DomainEventHandler HelloDomainEvent;

        private List<Dictionary<String, Object>> eventStort = new List<Dictionary<string, Object>>();//事件及调用参数持久化

        public override void Test()
        {
            var chinaSayHelloHandler = new ChinaSayHelloHandler();
            var englishSayHelloHandler = new EnglishSayHelloHandler();
            {
                HelloDomainEvent += chinaSayHelloHandler.Handler;
                HelloDomainEvent += englishSayHelloHandler.Handler;
                WriteLine("持久化事件中。。。");
                Persistence(HelloDomainEvent, new SayEventArgs() { Message = "在见面时打招呼" });
                WriteLine("持久化事件完成");
            }
            {
                WriteLine("加载并执行事件中。。。");
                foreach (var item in eventStort)
                {
                    object objEventType = null;
                    object objEventMethodName = null;
                    object objEventMethodParamaterTypes = null;
                    object objEventArgs = null;
                    item.TryGetValue("eventType", out objEventType);
                    item.TryGetValue("eventMethodName", out objEventMethodName);

                    item.TryGetValue("eventMethodParamaterTypes", out objEventMethodParamaterTypes);//如为JSon格式还需反序列化成原来对象
                    item.TryGetValue("eventArgs", out objEventArgs);//如为JSon格式还需反序列化成原来对象

                    var delegator = CreateDelegator<DomainEventHandler>(objEventType.ToString(),objEventMethodName.ToString(),objEventMethodParamaterTypes as String[]);
                    var eventType = Type.GetType(objEventType.ToString());              //获得委托调用者类型
                    var target = eventType.Assembly.CreateInstance(eventType.FullName); //实例化委托调用者
                    delegator(eventType, objEventArgs as BaseDomainEventArgs);
                }
                WriteLine("加载并执行事件完成");
            }
        }

        private void Persistence(DomainEventHandler DomainEvent, BaseDomainEventArgs eventArgs)
        {
            foreach (var item in HelloDomainEvent.GetInvocationList())//按照调用顺序返回此多路广播委托的调用列表。
            {
                var dic = new Dictionary<String, Object>();
                var eventType = item.Target.GetType().AssemblyQualifiedName;//获取当前委托调用者的 System.Type 的程序集限定名，其中包括从中加载 System.Type 的程序集的名称
                var eventMethodName = item.Method.Name;//获取委托表示方法的名称
                var eventMethodParamaters = item.Method.GetParameters();//获取委托表示方法的参数
                String[] eventMethodParamaterTypes = new String[eventMethodParamaters.Length];//委托表示方法的参数类型
                var index = 0;
                foreach (var paramater in eventMethodParamaters)
                {
                    eventMethodParamaterTypes[index] = paramater.ParameterType.AssemblyQualifiedName;//获取参数的System.Type 的程序集限定名，其中包括从中加载 System.Type 的程序集的名称
                    index++;
                }
                dic.Add("eventType", eventType);
                dic.Add("eventMethodName", eventMethodName);
                dic.Add("eventMethodParamaterTypes", eventMethodParamaterTypes);//可转为JSon格式保存
                dic.Add("eventArgs", eventArgs);//可转为JSon格式保存
                eventStort.Add(dic);
            }
        }

        private TDelegator CreateDelegator<TDelegator>(String strEventType, String eventMethodName, String[] strEventMethodParameterTypes)
        {
            var eventType = Type.GetType(strEventType);                         //获得委托调用者类型
            var target = eventType.Assembly.CreateInstance(eventType.FullName); //实例化委托调用者
            var targetConstant = Expression.Constant(target, eventType);        //创建委托调用常量表达式

            ParameterExpression[] arguments = new ParameterExpression[strEventMethodParameterTypes.Length];//创建参数表达式数组

            var index = 0;
            foreach (var item in strEventMethodParameterTypes)
            {
                arguments[index] = Expression.Parameter(Type.GetType(strEventMethodParameterTypes[index]));//创建参数表达式，并将其赋值给数表达式数组
                index++;
            }

            var methodCallExp = Expression.Call(targetConstant, eventType.GetMethod(eventMethodName), arguments);//创建一个表示调用带参数的方法的MethodCallExpression。
            //Expression.Lambda<TDelegator>(methodCallExp, arguments) 返回以表达式目录树的形式将强类型 lambda 表达式表示为数据结构的 Expression<TDelegate>
            //Expression<TDelegate>.Compile()  将表达式树描述的 lambda 表达式编译为可执行代码，并生成表示该 lambda 表达式的委托
            return Expression.Lambda<TDelegator>(methodCallExp, arguments).Compile();
        }
    }
}
