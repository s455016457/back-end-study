using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Core.Entities
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
        /// 接收者类别
        /// </summary>
        public ReceiverType ReceiverType { get; private set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        public MessageType MessageType { get; private set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; private set; }

        private SendMessage() { }

        public SendMessage(Guid MessageId, Guid SenderId, ReceiverType receiverType, MessageType messageType, string Message)
        {
            this.MessageType = messageType;
            this.Message = Message;
            this.MessageId = MessageId;
            this.SenderId = SenderId;
            this.ReceiverType = receiverType;
        }
    }

    /// <summary>
    /// 发送异常消息
    /// </summary>
    public class SendErrorMessage : SendMessage
    {
        public SendErrorMessage(Guid MessageId, Guid SenderId, ReceiverType receiverType, Guid errorMessageId, string message)
            : base(MessageId,SenderId, receiverType, MessageType.erroe
                  , JsonConvert.SerializeObject(new Dictionary<string, object> { { "ErrorMessageId", errorMessageId }, { "Message", message } }))
        {
        }
    }

    /// <summary>
    /// 发送已读状态消息
    /// </summary>
    public class SendReadStatusMessage : SendMessage
    {
        public SendReadStatusMessage(Guid MessageId,Guid SenderId, ReceiverType receiverType, Guid receiveMessageId, bool status,string message)
            : base(MessageId,SenderId, receiverType, MessageType.readStatusChange
                  , JsonConvert.SerializeObject(new Dictionary<string, object> { { "ReceiveMessageId", receiveMessageId },{ "Status",status}, { "Message", message } }))
        {
        }
    }
}
