using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test.DDD.Store;

namespace Test.DDD.Test
{
    public class TestEF : BaseTest
    {
        public override void Test()
        {
            using (DDDTestContext context = new DDDTestContext())
            {
                //var PoDetail = context.PoDetail.FirstOrDefault(p => p.Ponum == "TestPonum" && p.ItemNum == "TestItemnum" && p.Lot == "TestLot");
                //if (PoDetail != null)
                //{
                //    context.PoDetail.Remove(PoDetail);
                //    context.SaveChanges();
                //}
            }

            using (DDDTestContext context = new DDDTestContext())
            {
                context.PoDetail.Add(
                    new PoDetail()
                    {
                        Ponum = "TestPonum",
                        ItemNum = "TestItemnum",
                        Lot = "TestLot",
                        Qty = 1000,
                        Uprice = (decimal)0.5,
                        Amount = (decimal)(1000 * 0.5),
                        PoDetail_TotalGrnQty = new PoDetail_TotalGrnQty()
                        {
                            Ponum = "TestPonum",
                            ItemNum = "TestItemnum",
                            Lot = "TestLot",
                            TotalGrnQty = 0
                        }
                    });
                context.SaveChanges();
            }
            using (DDDTestContext context = new DDDTestContext())
            {
                var PoDetail = context.PoDetail.FirstOrDefault(p => p.Ponum == "TestPonum" && p.ItemNum == "TestItemnum" && p.Lot == "TestLot");
                if (PoDetail != null)
                {
                    PoDetail.PoDetail_TotalGrnQty.TotalGrnQty = 123;
                    context.SaveChanges();
                }
            }

            using (DDDTestContext context = new DDDTestContext())
            {
                var PoDetail = context.PoDetail.FirstOrDefault(p => p.Ponum == "TestPonum" && p.ItemNum == "TestItemnum" && p.Lot == "TestLot");
                if (PoDetail != null)
                {
                    WriteLine("采购单号【{0}】 物料编码【{1}】 批次【{2}】 采购数量【{3}】 单价【{4}】 金额【{5}】 收货数量【{6}】", PoDetail.Ponum, PoDetail.ItemNum, PoDetail.Lot, PoDetail.Qty, PoDetail.Uprice, PoDetail.Amount, PoDetail.PoDetail_TotalGrnQty.TotalGrnQty);
                }
            }
        }
    }
}
