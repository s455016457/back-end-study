using Domain.Aggregate.Setting;
using Domain.Entity.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Factory
{
    public static class SettingEntityFactory
    {
        public static ItemTypeEntity CreateNew(
            String Code, String Descr, String ParentId, String ItemGroup, String AttribType, String PreFix, String InvAcc
            , int ItemNumMaxLength, int ItemNumMinLength, Decimal MaxScpRate, Decimal MinScpRate, Decimal OverQty, Decimal OverRate
            , Boolean IsLotManaged, Boolean IsFgRef, Boolean IsVirtual
            )
        {
            return new ItemTypeEntity()
            {
                Code = Code,
                Descr = "测试",
                ParentID = "1101",
                AttribType = AttribType,
                InvAcc = InvAcc,
                IsFgRef = IsFgRef,
                IsVirtual = IsVirtual,
                ItemGroup = ItemGroup,
                ItemNumMaxLength = ItemNumMaxLength,
                ItemNumMinLength = ItemNumMinLength,
                IsLotManaged = IsLotManaged,
                MaxScpRate = MaxScpRate,
                MinScpRate = MinScpRate,
                OverQty = OverQty,
                OverRate = OverRate,
                PreFix = PreFix,
                State = Entity.EntityState.Added
            };
        }
    }
}
