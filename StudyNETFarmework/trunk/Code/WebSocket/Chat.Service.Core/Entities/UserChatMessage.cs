using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Core.Entities
{
    /// <summary>
    /// 用户聊天信息
    /// </summary>
    public class UserChatMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public Guid MessageId { get; set; }
        /// <summary>
        /// 发送者ID
        /// </summary>
        public Guid SenderId { get; private set; }
        /// <summary>
        /// 接受者ID
        /// </summary>
        public Guid ReceiverId { get; private set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendStamp { get; private set; }
        /// <summary>
        /// 已阅读
        /// </summary>
        public bool IsRead { get; private set; }
        /// <summary>
        /// 发送用户
        /// </summary>
        public virtual User SenderUser { get; private set; }
        /// <summary>
        /// 接收用户
        /// </summary>
        public virtual User ReceiverUser { get; private set; }

        private UserChatMessage() { }

        public UserChatMessage(Guid MessageId, Guid SenderId,Guid ReceiverId,string Message,DateTime SendStamp)
        {
            this.MessageId = MessageId;
            this.SenderId = SenderId;
            this.ReceiverId = ReceiverId;
            this.Message = Message;
            this.SendStamp = SendStamp;
            IsRead = false;
        }

        /// <summary>
        /// 阅读
        /// </summary>
        public void Read()
        {
            IsRead = true;
        }
    }
}
