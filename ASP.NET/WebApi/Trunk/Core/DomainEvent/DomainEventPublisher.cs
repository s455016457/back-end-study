using Domain.Event.Utilities;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Domain.Event.ValueObject;
using System.Timers;
using System.Threading.Tasks;

namespace Domain.Event
{
    /// <summary>
    /// 领域事件发布服务
    /// </summary>
    public class DomainEventPublisherService
    {
        private static readonly Object lock_obj = new Object();
        private static DomainEventPublisherService _this;
        private IDomainEventHandlerStore store;
        private Boolean _isRuning = false;
        private Timer timer = null;

        private DomainEventPublisherService(IDomainEventHandlerStore store)
        {
            this.store = store;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static DomainEventPublisherService Initialization(IDomainEventHandlerStore store)
        {
            if (_this == null)
            {
                lock (lock_obj)
                {
                    _this = new DomainEventPublisherService(store);
                }
            }
            return _this;
        }
        /// <summary>
        /// 获取领域事件发布服务上下文
        /// </summary>
        public static DomainEventPublisherService Context { get { return _this; } }
        /// <summary>
        //  发行
        /// </summary>
        /// <param name="eventArgs"></param>
        public void Publisher(DomainEventHandler DomainEvent, BaseDomainEventArgs eventArgs)
        {
            foreach (Delegate item in DomainEvent.GetInvocationList())
            {
                DomainEventHandlerEntity entity = DomainEventHandlerEntity.CreateNew(item.Target.GetType(),item.Method, eventArgs.GetType(), eventArgs);
                store.AddDomainEventHandler(entity.objectValue);
            }
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="Frequency">执行频率，单位s</param>
        public void Start(int Frequency)
        {
            if (!_isRuning)
            {
                lock (lock_obj)
                {
                    if (!_isRuning)
                    {
                        Task.Factory.StartNew(Exec);
                        Frequency *= 1000;
                        timer = new Timer(Frequency);
                        timer.Elapsed += (object sender, ElapsedEventArgs e) => { Task.Factory.StartNew(Exec); };
                        timer.Start();
                        _isRuning = true;
                    }
                }
            }
        }

        public void Stop()
        {
            if (_isRuning)
            {
                lock (lock_obj)
                {
                    if (_isRuning)
                    {
                        timer.Stop();
                        timer.Dispose();
                        _isRuning = false;
                    }
                }
            }
        }

        private void Exec()
        {
            lock (lock_obj)
            {
                var ObjectValues = store.GetEntities(p => !p.IsRuned);
                DomainEventHandlerValueObject[] entityArray = new DomainEventHandlerValueObject[ObjectValues.Count];
                ObjectValues.CopyTo(entityArray, 0);

                foreach (var objectValue in entityArray)
                {
                    var entity = DomainEventHandlerEntity.Create(objectValue);
                    entity.Run();

                    store.ChangDomainEventHandlerStuts(objectValue);
                }
            }
        }
    }
}
