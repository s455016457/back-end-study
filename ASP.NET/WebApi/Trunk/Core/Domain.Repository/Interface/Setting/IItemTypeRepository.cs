using Domain.Aggregate.Setting;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Repository.Interface.Setting
{
    public interface IItemTypeRepository:IBaseRepository
    {
        ItemTypeAggregate FindItemType(String Code);

        IQueryable<ItemTypeAggregate> GetItemTypeList(Expression<Func<ItemTypeAggregate, bool>> predicate);

        IQueryable<ItemTypeAggregate> GetItemTypeChildes(String Code);

        void AddItemType(ItemTypeAggregate itemType);

        void UpdateItemType(ItemTypeAggregate itemType);

        void RemoveItemType(ItemTypeAggregate itemType);

        void SaveChange(ItemTypeAggregate itemType);

        ItemTypeAggregate GetParent(String Code);
    }
}
