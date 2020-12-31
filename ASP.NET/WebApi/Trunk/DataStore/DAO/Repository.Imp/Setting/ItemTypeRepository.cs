using AutoMapper;
using Domain.Aggregate.Setting;
using Domain.Entity.Setting;
using Domain.Repository.Interface.Setting;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.EF.Repository.Imp.Setting
{
    public class ItemTypeRepository : BaseRepository, IItemTypeRepository
    {
        public ItemTypeRepository(DbContext dbContext) : base(dbContext)
        {
            Mapper.Initialize(p =>
            {
                p.CreateMap<ItemTypeEntity, ItemType>()
                    .ForMember(t => t.ITEMGRP, opp => opp.ResolveUsing(t => t.ItemGroup))
                    .ForMember(t => t.MAXLEN, op => op.ResolveUsing(t => t.ItemNumMaxLength))
                    .ForMember(t => t.MINLEN, op => op.ResolveUsing(t => t.ItemNumMinLength))
                    .ForMember(t => t.LOTMANAGED, op => op.ResolveUsing(t => t.IsLotManaged));
                p.CreateMap<ItemType, ItemTypeEntity>()
                    .ForMember(t => t.ItemGroup, opp => opp.ResolveUsing(t => t.ITEMGRP))
                    .ForMember(t => t.ItemNumMaxLength, op => op.ResolveUsing(t => t.MAXLEN))
                    .ForMember(t => t.ItemNumMinLength, op => op.ResolveUsing(t => t.MINLEN))
                    .ForMember(t => t.IsLotManaged, op => op.ResolveUsing(t => t.LOTMANAGED));
            });
        }

        public void AddItemTypeAggregate(ItemTypeAggregate itemType)
        {
            if (_dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals(itemType.Code)) != null) throw new Exception("物料次类【" + itemType.Code + "】已存在于系统中，不能插入重复数据！");
            //Mapper.Map<ItemTypeEntity, ItemType>(itemType);
            _dbContext.Set<ItemType>().Add(Mapper.Map<ItemTypeEntity, ItemType>(itemType.ItemType));
        }

        public ItemTypeAggregate FindItemTypeAggregate(string Code)
        {
            var entity = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals(Code));
            if (entity == null) return null;
            return new ItemTypeAggregate(Code, Mapper.Map<ItemType, ItemTypeEntity>(entity));
        }

        public IQueryable<ItemTypeAggregate> GetChildes(String Code)
        {
            var entity = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals(Code));
            return entity.Childes.Select(
              p => new ItemTypeAggregate(p.CODE, Mapper.Map<ItemType, ItemTypeEntity>(p))
          ).AsQueryable();
        }

        public ItemTypeAggregate GetParent(String Code)
        {
            var entity = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals(Code));
            if (entity == null)
            {
                throw new Exception("次类【" + Code + "】不存在！");
            }
            return new ItemTypeAggregate(entity.CODE, Mapper.Map<ItemType, ItemTypeEntity>(_dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE == entity.PARENTID)));
        }

        public void RemoveItemTypeAggregate(ItemTypeAggregate itemType)
        {
            var entity = _dbContext.Set<ItemType>().FirstOrDefault<ItemType>(p => p.CODE.Equals(itemType.Code));
            Mapper.Map<ItemTypeEntity, ItemType>(itemType.ItemType, entity);
            _dbContext.Set<ItemType>().Remove(entity);
        }

        public void SaveChange(ItemTypeAggregate itemType)
        {
            var entity = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals(itemType.Code));
            var oldItemType = new ItemTypeAggregate(itemType.Code, Mapper.Map<ItemType, ItemTypeEntity>(entity));

            if (oldItemType == null)
            {
                throw new Exception("物料次类【" + itemType.Code + "】不存在，更新失败！");
            }
            
            if (!oldItemType.ItemType.Equals(itemType.ItemType))
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public void UpdateItemTypeAggregate(ItemTypeAggregate itemType)
        {
            var entity = _dbContext.Set<ItemType>().FirstOrDefault(p => p.CODE.Equals(itemType.Code));
            Mapper.Map(itemType.ItemType, entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        private void SaveChange(ItemTypeEntity itemType)
        {
            ItemType entity = null;
            switch (itemType.State)
            {
                case Domain.Entity.EntityState.Detached:
                    _dbContext.Set<ItemType>().Add(Mapper.Map<ItemTypeEntity, ItemType>(itemType));
                    break;
                case Domain.Entity.EntityState.Added:
                    _dbContext.Set<ItemType>().Add(Mapper.Map<ItemTypeEntity, ItemType>(itemType));
                    break;
                case Domain.Entity.EntityState.Deleted:
                    entity = _dbContext.Set<ItemType>().FirstOrDefault(p=>p.CODE.Equals(itemType.Code));
                    if (entity == null)
                    {
                        new Exception("次类【" + itemType.Code + "】不存在，删除失败！");
                    }
                    Mapper.Map<ItemTypeEntity, ItemType>(itemType, entity);
                    _dbContext.Set<ItemType>().Add(Mapper.Map<ItemTypeEntity, ItemType>(itemType));
                    break;

            }
        }
    }
}
