using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.DDD.Domain.AggregateRoot;
using Test.DDD.Domain.Entity;

namespace Test.DDD.Domain.Aggregate
{
    public class PoDetailAggregate
    {
        private PoRoot Root;

        private PoTotalAmountEntitiy PoTotalAmount;

        private IList<PoDetailEntity> PoDetails;

        public PoDetailAggregate(PoRoot Root, PoTotalAmountEntitiy PoTotalAmount, IList<PoDetailEntity> PoDetails)
        {
            this.Root = Root;
            this.PoTotalAmount = PoTotalAmount;
            this.PoDetails = PoDetails;
        }

        public void AddPoDetail(string ItemNum, string Lot, decimal Qty,string Uom, decimal Uprice)
        {
            var item = new PoDetailEntity()
            {
                Ponum = Root.PoNum,
                ItemNum = ItemNum,
                Lot = Lot,
                Qty = Qty,
                Uom = Uom,
                Uprice = Uprice
            };

            PoDetails.Add(item);
            PoTotalAmount.TotalAmount += item.Amount;
        }

        public void UpdatePoDetail(string ItemNum, string Lot, decimal Qty, decimal Uprice)
        {
           var item =  PoDetails.FirstOrDefault(p => p.ItemNum==ItemNum && p.Lot == Lot);
            var outAmount = item.Amount;
            item.Qty = Qty;
            item.Uprice = Uprice;
            PoTotalAmount.TotalAmount += item.Amount - outAmount;
        }

        public void DeletePoDetail(string ItemNum, string Lot)
        {
            var item = PoDetails.FirstOrDefault(p => p.ItemNum == ItemNum && p.Lot == Lot);
            PoTotalAmount.TotalAmount -= item.Amount;
            PoDetails.Remove(item);
        }
    }
}
