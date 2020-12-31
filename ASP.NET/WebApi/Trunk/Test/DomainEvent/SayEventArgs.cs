using Domain.Event;
using System;

namespace Test.TestDomainEvent
{
    public class SayEventArgs: BaseDomainEventArgs
    {
        private String message;
        public String Message { get { return message; } set { message = value.ToUpper(); } }
    }
}
