using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Setting
{
    /// <summary>
    /// 物料组
    /// </summary>
    public class ItemGroupEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public String Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String Descr { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public String Attribute { get; set; }
    }
}
