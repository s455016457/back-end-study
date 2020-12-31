using Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.TestDomainEvent
{
    public class ChinaSayHelloHandler : BaseDomainEventHandler
    {
        public override void Handler(object sender, BaseDomainEventArgs eventArgs)
        {
            SayEventArgs sayEventArgs = (SayEventArgs)eventArgs;
            Console.WriteLine("{0}：中国说你好", sayEventArgs.Message);   
        }
    }

    public class EnglishSayHelloHandler : BaseDomainEventHandler
    {
        public override void Handler(object sender, BaseDomainEventArgs eventArgs)
        {
            SayEventArgs sayEventArgs = (SayEventArgs)eventArgs;
            Console.WriteLine("{0}：English Say Hello", sayEventArgs.Message);
        }
    }

    public class ChinaSayByeByeHandler : BaseDomainEventHandler
    {
        public override void Handler(object sender, BaseDomainEventArgs eventArgs)
        {
            SayEventArgs sayEventArgs = (SayEventArgs)eventArgs;
            Console.WriteLine("{0}：中国说再见", sayEventArgs.Message);
        }
    }

    public class EnglishSyeByeBayHandler : BaseDomainEventHandler
    {
        public override void Handler(object sender, BaseDomainEventArgs eventArgs)
        {
            SayEventArgs sayEventArgs = (SayEventArgs)eventArgs;
            Console.WriteLine("{0}：English Say Bye Bye", sayEventArgs.Message);
        }
    }
}
