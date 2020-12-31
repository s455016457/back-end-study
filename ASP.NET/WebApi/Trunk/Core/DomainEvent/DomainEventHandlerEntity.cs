using Domain.Event.ValueObject;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Domain.Event
{
    /// <summary>
    /// 领域事件处理实体
    /// </summary>
    public class DomainEventHandlerEntity
    {
        private Type eventType;
        private Type eventParameterType;
        private BaseDomainEventArgs eventArgs;

        public Boolean IsRuned()
        {
            return objectValue.IsRuned;
        }
        /// <summary>
        /// 领域事件处理值对象
        /// </summary>
        public DomainEventHandlerValueObject objectValue { get; private set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="objectValue"></param>
        private DomainEventHandlerEntity(DomainEventHandlerValueObject objectValue)
        {
            eventType = Type.GetType(objectValue.EventType);
            eventParameterType = Type.GetType(objectValue.EventParameterType);

            eventArgs = (BaseDomainEventArgs)eventParameterType.Assembly.CreateInstance(eventParameterType.FullName);
            JsonConvert.PopulateObject(objectValue.EventJsonParameter, eventArgs);

            this.objectValue = objectValue;
        }
        /// <summary>
        /// 创建新的值对象
        /// </summary>
        /// <param name="EventType"></param>
        /// <param name="EventParameterType"></param>
        /// <param name="EventJsonParameter"></param>
        /// <returns></returns>
        private static DomainEventHandlerValueObject CreateNewObjectValue(String EventType,String EventMethodName,String[] EventMethodParameterTypes, String EventParameterType, String EventJsonParameter)
        {
            return new DomainEventHandlerValueObject()
            {
                EventId = Guid.NewGuid().ToString(),
                EventType = EventType,
                EventJsonParameter = EventJsonParameter,
                EventParameterType = EventParameterType,
                EventMethodName= EventMethodName,
                EvenMethodParameterTypes= EventMethodParameterTypes,
                IsRuned = false
            };
        }
        /// <summary>
        /// 创建新的领域事件处理实体
        /// </summary>
        /// <param name="EventType">事件类型</param>
        /// <param name="EventParameterType">事件参数类型</param>
        /// <param name="eventArgs">事件参数</param>
        /// <returns></returns>
        public static DomainEventHandlerEntity CreateNew(Type EventType,MethodInfo EventMethod, Type EventParameterType, BaseDomainEventArgs eventArgs)
        {
            var parameters = EventMethod.GetParameters();
            String[] parameterTypes = new String[parameters.Length];
            int index = 0;
            foreach (var item in parameters)
            {
                parameterTypes[index++] = item.ParameterType.AssemblyQualifiedName;
            }
            var objectValue = CreateNewObjectValue(EventType.AssemblyQualifiedName, EventMethod.Name, parameterTypes, EventParameterType.AssemblyQualifiedName, JsonConvert.SerializeObject(eventArgs));
            return new DomainEventHandlerEntity(objectValue);
        }
        /// <summary>
        /// 根据值对象创建实体
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public static DomainEventHandlerEntity Create(DomainEventHandlerValueObject valueObject)
        {
            return new DomainEventHandlerEntity(valueObject);
        }

        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {
            var target = eventType.Assembly.CreateInstance(eventType.FullName);
            //var deletgate = Utilities.DelegateFactory.CreateDelegate<DomainEventHandler>(eventType.GetMethod("Handler"), new Type[] { typeof(object), typeof(BaseDomainEventArgs) });
            var deletgate = CreateDelegator<DomainEventHandler>(objectValue.EventType, objectValue.EventMethodName, objectValue.EvenMethodParameterTypes);
            deletgate(target, eventArgs);

            objectValue.IsRuned = true;
        }

        private TDelegator CreateDelegator<TDelegator>(String strEventType,String eventMethodName,String[] strEventMethodParameterTypes)
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
