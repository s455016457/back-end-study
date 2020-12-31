using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Core.Entities
{
    /// <summary>
    /// 接受到的消息
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
        /// 接收者类别
        /// </summary>
        [JsonProperty]
        public ReceiverType ReceiverType { get; private set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        [JsonProperty]
        public MessageType MessageType { get; private set; }
        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty]
        public string Message { get; private set; }

        public ReceiveMessage() { }
    }    
}