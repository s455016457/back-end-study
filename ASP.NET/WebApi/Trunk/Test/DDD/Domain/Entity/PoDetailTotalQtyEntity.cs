using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DDD.Domain.Entity
{
    public class PoDetailTotalQtyEntity
    {
        public string Ponum { get; set; }

        public string ItemNum { get; set; }

        public string Lot { get; set; }

        public decimal Qty { get; set; }

    }
}
