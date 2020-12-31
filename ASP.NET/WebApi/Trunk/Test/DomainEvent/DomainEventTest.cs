using Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test.TestDomainEvent;

namespace Test.DomainEvent
{
    public class DomainEventTest : BaseTest
    {
        /// 事件
        /// </summary>
        public static event DomainEventHandler HelloDomainEvent;
        public static event DomainEventHandler BayBayDomainEvent;
        public override void Test()
        {
            var chinaSayHelloHandler = new ChinaSayHelloHandler();
            var englishSayHelloHandler = new EnglishSayHelloHandler();
            {
                DomainEventPublisherService domainEventPublisher = DomainEventPublisherService.Initialization(new DomainEventHandlerStore());
                domainEventPublisher.Start(5);
            }
            {

                var domainEventPublisher = DomainEventPublisherService.Context;

                HelloDomainEvent += chinaSayHelloHandler.Handler;
                HelloDomainEvent += englishSayHelloHandler.Handler;
                domainEventPublisher.Publisher(HelloDomainEvent, new SayEventArgs() { Message = "在见面时打招呼" });
            }
            WriteLine("*********睡6秒*********");
            Thread.Sleep(6 * 1000);
            WriteLine("*********睡完了*********");
            {

                var domainEventPublisher = DomainEventPublisherService.Context;
                HelloDomainEvent -= englishSayHelloHandler.Handler;
                domainEventPublisher.Publisher(HelloDomainEvent, new SayEventArgs() { Message = "在见面时打招呼" });
            }
            WriteLine("*********睡6秒*********");
            Thread.Sleep(6 * 1000);
            WriteLine("*********睡完了*********");
            {
                var domainEventPublisher = DomainEventPublisherService.Context;
                BayBayDomainEvent += new ChinaSayByeByeHandler().Handler;
                BayBayDomainEvent += new EnglishSayHelloHandler().Handler;
                domainEventPublisher.Publisher(BayBayDomainEvent, new SayEventArgs() { Message = "在离开时打招呼" });
            }
            {
                foreach (var item in HelloDomainEvent.GetInvocationList())//按照调用顺序返回此多路广播委托的调用列表。
                {
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
                }
            }
        }
    }
}
