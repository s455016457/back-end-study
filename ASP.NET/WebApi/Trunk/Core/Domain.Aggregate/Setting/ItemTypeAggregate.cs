using Domain.Entity.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregate.Setting
{
    public class ItemTypeAggregate
    {
        public String Code { get; private set; }
        public ItemTypeEntity ItemType { get; private set; }

        public ICollection<ItemTypeEntity> Childs { get;private set; }

        public ItemTypeAggregate(String Code, ItemTypeEntity ItemType, ICollection<ItemTypeEntity> Childs)
        {
            this.Code = Code;
            this.ItemType = ItemType;
            this.Childs = Childs;
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

            Childs.Add(ItemType);
            return new ItemTypeAggregate(ItemType.Code, ItemType, null);
        }

        public ICollection<ItemTypeAggregate> GetChilds()
        {
            return null;
        }

        public ItemTypeEntity GetParent()
        {
            return null;
        }
    }
}
