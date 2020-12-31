using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Core.Entities
{
    /// <summary>
    /// 发送消息
    /// </summary>
    public class SendMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public Guid MessageId { get; private set; }
        /// <summary>
        /// 发送者ID
        /// </summary>
        public Guid SenderId { get; private set; }
        /// <summary>
        /// 接受者ID
        /// </summary>
        public Guid ReceiverId { get; private set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        public MessageType MessageType { get; set; }
        /// <summary>
        /// 接收者类别
        /// </summary>
        public ReceiverType ReceiverType { get; private set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; private set; }

        public SendMessage(Guid MessageId, Guid SenderId, Guid ReceiverId, ReceiverType ReceiverType, MessageType MessageType, string Message)
        {
            this.MessageId = MessageId;
            this.SenderId = SenderId;
            this.ReceiverId = ReceiverId;
            this.MessageType = MessageType;
            this.ReceiverType = ReceiverType;
            this.Message = Message;
        }
    }

    /// <summary>
    /// 接收者类别
    /// </summary>
    public enum ReceiverType
    {
        /// <summary>
        /// 个体
        /// </summary>
        individual = 0,
        /// <summary>
        /// 群体
        /// </summary>
        group = 1,
    }

    /// <summary>
    /// 消息类别
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 消息
        /// </summary>
        message = 0,
        /// <summary>
        /// 已阅读状态修改
        /// </summary>
        readStatusChange = 1,
        /// <summary>
        /// 异常
        /// </summary>
        erroe = -1,
    }
}
