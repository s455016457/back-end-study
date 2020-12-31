using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Setting
{
    public static class SettingEntityFactory
    {
        public static ItemTypeEntity CreateNew(String Code, String Descr, int MinLen, int MaxLen
            , String AttribType, String ItemGroup, String InvAcc, String ParentID
            , Decimal OverQty, Boolean IsFgRef, Boolean IsVirtual, Boolean IsLotManaged
            , Decimal OverRate, String PreFix, Decimal MinScpRate, Decimal MaxScpRate)
        {
            return new ItemTypeEntity()
            {
                Code = Code,
                Descr = Descr,
                ItemNumMinLength = MinLen,
                ItemNumMaxLength = MaxLen,
                AttribType = AttribType,
                InvAcc = InvAcc,
                IsFgRef = IsFgRef,
                IsVirtual = IsVirtual,
                ItemGroup = ItemGroup,
                IsLotManaged = IsLotManaged,
                MaxScpRate = MaxScpRate,
                MinScpRate = MinScpRate,
                OverQty = OverQty,
                OverRate = OverRate,
                ParentID = ParentID,
                PreFix = PreFix,
                State = Entity.EntityState.Added
            };
        }
    }
}
