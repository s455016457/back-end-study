using Domain.Entity.Setting;
using Domain.Repository.Interface.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Aggregate.Setting
{
    public class ItemTypeAggregate
    {
        public String Code { get;  private set; }
        public ItemTypeEntity ItemType { get; private set; }

        public ItemTypeAggregate() { }

        public ItemTypeAggregate(String Code, ItemTypeEntity ItemType)
        {
            this.Code = Code;
            this.ItemType = ItemType;
        }

        public void UpdateItemTypeEntity(ItemTypeEntity itemType)
        {
            ItemType = itemType; ;
        }

        public ItemTypeAggregate AddChild(String Code,String Descr,int MinLen,int MaxLen,String InvAcc
            ,Decimal OverQty,Boolean IsFgRef,Boolean IsVirtual,Boolean IsLotManaged
            ,Decimal OverRate,String PreFix,Decimal MinScpRate,Decimal MaxScpRate)
        {
            ItemTypeEntity ItemType = new ItemTypeEntity()
            {
                Code = Code,
                Descr = Descr,
                ItemNumMinLength = MinLen,
                ItemNumMaxLength = MaxLen,
                AttribType = this.ItemType.AttribType,
                InvAcc = InvAcc,
                IsFgRef = IsFgRef,
                IsVirtual = IsVirtual,
                ItemGroup = this.ItemType.ItemGroup,
                IsLotManaged = IsLotManaged,
                MaxScpRate = MaxScpRate,
                MinScpRate=MinScpRate,
                OverQty=OverQty,
                OverRate=OverRate,
                ParentID=this.Code,
                PreFix=PreFix,
                State=Entity.EntityState.Added
            };
            
            return new ItemTypeAggregate(ItemType.Code, ItemType);
        }
        
    }
}
