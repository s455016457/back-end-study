using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.EF
{
    public partial class PayMethod
    {
        public virtual ICollection<Po> Po { get; set; }

    }
}
