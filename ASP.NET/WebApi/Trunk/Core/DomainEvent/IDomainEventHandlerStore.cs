using Domain.Event.ValueObject;
using System;
using System.Collections.Generic;

namespace Domain.Event
{
    /// <summary>
    /// 领域事件处理仓储
    /// </summary>
    public interface IDomainEventHandlerStore
    {
        /// <summary>
        /// 添加领域事件处理
        /// </summary>
        /// <param name="entity"></param>
        void AddDomainEventHandler(DomainEventHandlerValueObject entity);
        /// <summary>
        /// 添加领域事件处理
        /// </summary>
        /// <param name="entities"></param>
        void AddDomainEventHandler(DomainEventHandlerValueObject[] entities);
        /// <summary>
        /// 添加领域事件处理
        /// </summary>
        /// <param name="entities"></param>
        void AddDomainEventHandler(ICollection<DomainEventHandlerValueObject> entities);
        /// <summary>
        /// 更新领域事件处理状态
        /// </summary>
        /// <param name="entity"></param>
        void ChangDomainEventHandlerStuts(DomainEventHandlerValueObject entity);
        /// <summary>
        /// 获取领域事件处理
        /// </summary>
        /// <returns></returns>
        ICollection<DomainEventHandlerValueObject> GetEntities();
        /// <summary>
        /// 获取领域事件处理
        /// </summary>
        /// <param name="predicate">筛选表达式</param>
        /// <returns></returns>
        ICollection<DomainEventHandlerValueObject> GetEntities(Func<DomainEventHandlerValueObject, bool> predicate);
        /// <summary>
        /// 获取领域事件处理
        /// </summary>
        /// <param name="EventId">事件ID</param>
        /// <returns></returns>
        DomainEventHandlerValueObject GetEntity(String EventId);
    }
}
