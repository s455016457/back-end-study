using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Emtity.ErySystem
{
    /// <summary>
    /// 收件箱
    /// </summary>
    public class EmailInBox
    {
        /// <summary>
        /// 邮件Id
        /// </summary>
        public int Nid { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public String Recipient { get; set; }
        /// <summary>
        /// 收件夹
        /// </summary>
        public String Folder { get; set; }
        /// <summary>
        /// 收件日期
        /// </summary>
        public DateTime RecDate { get; set; }
        /// <summary>
        /// 已打开
        /// </summary>
        public Boolean Opened { get; set; }
        /// <summary>
        /// 已阅读
        /// </summary>
        public Boolean WasRead { get; set; }
        /// <summary>
        /// 已删除
        /// </summary>
        public Boolean Delete { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 新邮件
        /// </summary>
        public Boolean IsNewEmail { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedStamp { get; set; }
    }
}
