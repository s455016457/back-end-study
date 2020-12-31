using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event
{
    /// <summary>
    /// 领域事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    /// <summary>
    public delegate void DomainEventHandler(object sender, BaseDomainEventArgs eventArgs);
    /// <summary>
    /// 领域事件处理
    /// </summary>
    public abstract class BaseDomainEventHandler
    {
        public abstract void Handler(object sender, BaseDomainEventArgs eventArgs);
    }
}
