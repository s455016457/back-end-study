using Domain.Aggregate.Setting;
using Domain.Entity.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Factory
{
    public static class SettingAggregateFactory
    {
        public static ItemTypeAggregate CreateNew(
            String Code, String Descr, String ParentId, String ItemGroup, String AttribType, String PreFix, String InvAcc
            , int ItemNumMaxLength, int ItemNumMinLength, Decimal MaxScpRate, Decimal MinScpRate, Decimal OverQty, Decimal OverRate
            , Boolean IsLotManaged, Boolean IsFgRef, Boolean IsVirtual
            )
        {
            return new ItemTypeAggregate(Code
                , SettingEntityFactory.CreateNew(
                    Code, Descr, ParentId, ItemGroup, AttribType, PreFix, InvAcc, ItemNumMaxLength, ItemNumMinLength
                    , MaxScpRate, MinScpRate, OverQty, OverRate, IsLotManaged, IsFgRef, IsVirtual
                )
            );

        }
    }
}
