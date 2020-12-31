using Domain.Aggregate.Setting;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Repository.Interface.Setting
{
    public interface IItemTypeRepository:IBaseRepository
    {
        ItemTypeAggregate FindItemTypeAggregate(String Code);

        IQueryable<ItemTypeAggregate> GetChildes(String Code);
        ItemTypeAggregate GetParent(String Code);

        void AddItemTypeAggregate(ItemTypeAggregate itemType);

        void UpdateItemTypeAggregate(ItemTypeAggregate itemType);

        void RemoveItemTypeAggregate(ItemTypeAggregate itemType);

        void SaveChange(ItemTypeAggregate itemType);
    }
}
