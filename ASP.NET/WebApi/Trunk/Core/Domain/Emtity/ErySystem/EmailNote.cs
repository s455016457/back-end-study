using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Emtity.ErySystem
{
    public class EmailNote
    {
        private IList<String> _recipientUids;
        private List<String> _copyRecipientUids;
        private String _recipient;
        private String _copyRecipient;

        /// <summary>
        /// 邮件Id
        /// </summary>
        public int Nid { get; set; }
        /// <summary>
        /// 邮件类型
        /// </summary>
        public String Type { get; set; }
        /// <summary>
        /// 优先级别
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public String SendBy { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public String Recipient
        {
            get
            {
                return _recipient;
            }
            set
            {
                _recipient = value;
                if (_recipient != null)
                {
                    _recipientUids = new List<String>();
                    string strselectedAutoComplete = _recipient.TrimEnd(';');
                    string[] arrayselectedAutoComplete = strselectedAutoComplete.Split(';');
                    for (int i = 0; i < arrayselectedAutoComplete.Length; i++)
                    {
                        if (arrayselectedAutoComplete[i].IndexOf('<', 0) > 0)
                        {
                            _recipientUids.Add(arrayselectedAutoComplete[i].Substring(arrayselectedAutoComplete[i].IndexOf('<', 0) + 1, arrayselectedAutoComplete[i].IndexOf('>', 0) - arrayselectedAutoComplete[i].IndexOf('<', 0) - 1).Trim());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 收件人ID
        /// </summary>
        public IList<String> RecipientUids { get { return _recipientUids; } }

        /// <summary>
        /// 抄送人
        /// </summary>
        public String CopyRecipients
        {
            get
            {
                return _copyRecipient;
            }
            set
            {
                _copyRecipient = value;
                if (_copyRecipient != null)
                {
                    _copyRecipientUids = new List<String>();
                    string strselectedAutoComplete = _copyRecipient.TrimEnd(';');
                    string[] arrayselectedAutoComplete = strselectedAutoComplete.Split(';');
                    for (int i = 0; i < arrayselectedAutoComplete.Length; i++)
                    {
                        if (arrayselectedAutoComplete[i].IndexOf('<', 0) > 0)
                        {
                            _copyRecipientUids.Add(arrayselectedAutoComplete[i].Substring(arrayselectedAutoComplete[i].IndexOf('<', 0) + 1, arrayselectedAutoComplete[i].IndexOf('>', 0) - arrayselectedAutoComplete[i].IndexOf('<', 0) - 1).Trim());
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 主题
        /// </summary>
        public String Subject { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public String Message { get; set; }
        /// <summary>
        /// 数据参数
        /// </summary>
        public String DataParams { get; set; }
        /// <summary>
        /// 消息数据
        /// </summary>
        public String MsgData { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public String Attachment { get; set; }
        /// <summary>
        /// 已发送
        /// </summary>
        public Boolean WasSent { get; set; }
        /// <summary>
        /// 发送日期
        /// </summary>
        public DateTime SendStamp { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedStamp { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedStamp { get; set; }
    }
}
