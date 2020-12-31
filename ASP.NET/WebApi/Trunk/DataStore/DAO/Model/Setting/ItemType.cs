using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.EF
{
    public partial class ItemType
    {
        public virtual ItemType Parent { get; set; }

        public virtual ICollection<ItemType> Childes { get; set; }

    }
}
