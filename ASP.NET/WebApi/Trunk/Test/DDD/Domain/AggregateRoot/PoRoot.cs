using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DDD.Domain.AggregateRoot
{
    public class PoRoot
    {
        public String PoNum { get; set; }

        public DateTime LastUpdatedStamp { get; set; }
    }
}
