using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event.ValueObject
{
    /// <summary>
    /// 领域事件处理值对象
    /// </summary>
    public class DomainEventHandlerValueObject
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public String EventId { get; set; }
        /// <summary>
        /// 事件已执行
        /// </summary>
        public Boolean IsRuned { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public String EventType { get; set; }
        /// <summary>
        /// 事件方法名称
        /// </summary>
        public String EventMethodName { get; set; }
        /// <summary>
        /// 事件方法参数类型
        /// </summary>
        public String[] EvenMethodParameterTypes { get; set; }
        /// <summary>
        /// 事件参数
        /// </summary>
        public String EventJsonParameter { get; set; }
        /// <summary>
        /// 事件参数
        /// </summary>
        public String EventParameterType { get; set; }
    }
}
