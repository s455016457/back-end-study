using Domain.Event;
using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Event.ValueObject;

namespace Test.DomainEvent
{
    public class DomainEventHandlerStore : IDomainEventHandlerStore
    {
        private static IList<DomainEventHandlerValueObject> _store = new List<DomainEventHandlerValueObject>();
        public void AddDomainEventHandler(DomainEventHandlerValueObject entity)
        {
            _store.Add(entity);
        }

        public void AddDomainEventHandler(DomainEventHandlerValueObject[] entities)
        {
            foreach (var item in entities)
                _store.Add(item);
        }

        public void AddDomainEventHandler(ICollection<DomainEventHandlerValueObject> entities)
        {
            foreach (var item in entities)
                _store.Add(item);
        }

        public void ChangDomainEventHandlerStuts(DomainEventHandlerValueObject entity)
        {
            _store.Remove(GetEntity(entity.EventId));
            _store.Add(entity);
        }

        public ICollection<DomainEventHandlerValueObject> GetEntities()
        {
            return _store;
        }

        public ICollection<DomainEventHandlerValueObject> GetEntities(Func<DomainEventHandlerValueObject, bool> predicate)
        {
            return _store.Where(predicate).ToList();
        }

        public DomainEventHandlerValueObject GetEntity(string EventId)
        {
            return _store.FirstOrDefault(p => p.EventId == EventId);
        }
    }
}
