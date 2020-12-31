using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.EF
{
    public partial class Po
    {
        public virtual Buyer BUYER1 { get; set; }

        public virtual InvType INVTYPE1 { get; set; }

        public virtual PayMethod PAYMETHOD1 { get; set; }

        public virtual PayTerm PAYTERM1 { get; set; }

        public virtual User CreatedUser { get; set; }
    }
}
