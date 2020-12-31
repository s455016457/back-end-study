using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Chat.Service.Core.Entities
{
    /// <summary>
    /// 接收消息
    /// </summary>
    public class ReceiveMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        [JsonProperty]
        public Guid MessageId { get; private set; }
        /// <summary>
        /// 发送者ID
        /// </summary>
        [JsonProperty]
        public Guid SenderId { get; private set; }
        /// <summary>
        /// 接受者ID
        /// </summary>
        [JsonProperty]
        public Guid ReceiverId { get; private set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        [JsonProperty]
        public MessageType MessageType { get; set; }
        /// <summary>
        /// 接收者类别
        /// </summary>
        [JsonProperty]
        public ReceiverType ReceiverType { get; private set; }
        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty]
        public string Message { get; private set; }

        public ReceiveMessage() { }
        public ReceiveMessage(Guid MessageId, Guid SenderId, Guid ReceiverId, MessageType MessageType, ReceiverType ReceiverType, string Message)
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
        individual=0,
        /// <summary>
        /// 群体
        /// </summary>
        group=1,
    }
    /// <summary>
    /// 消息类别
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 消息
        /// </summary>
        message=0,
        /// <summary>
        /// 已阅读状态修改
        /// </summary>
        readStatusChange=1,
        /// <summary>
        /// 异常
        /// </summary>
        erroe=-1,
    }
}
