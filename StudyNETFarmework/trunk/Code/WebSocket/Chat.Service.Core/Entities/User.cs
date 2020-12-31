using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketService.Core;

namespace Chat.Service.Core.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        public Guid UserId { get; private set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 聊天在线
        /// </summary>
        public bool IsChatOnLine { get; private set; }
        /// <summary>
        /// 好友
        /// </summary>
        public virtual List<User> Friends { get; private set; }
        /// <summary>
        /// 用户群
        /// </summary>
        public virtual List<ChatGroup> ChatGroups { get; private set; }

        private WebSocketContext WebSocketContext;

        private User() { }

        public User(Guid guid,string Name)
        {
            UserId = guid;
            this.Name = Name;
        }
        
        /// <summary>
        /// 用户聊天在线
        /// </summary>
        /// <param name="WebSocketContext"></param>
        public void ChatOnLine(WebSocketContext WebSocketContext)
        {
            IsChatOnLine = true;
            this.WebSocketContext = WebSocketContext;
        }
        /// <summary>
        /// 用户聊天离线
        /// </summary>
        public void ChatOffLine()
        {
            IsChatOnLine = false;
            WebSocketContext = null;
        }

        public async Task<UserChatMessage> SendMessageAsync(SendMessage sendMessage)
        {
            var model = new UserChatMessage(sendMessage.MessageId, sendMessage.SenderId, UserId, sendMessage.Message, DateTime.Now);
            if (IsChatOnLine)
            {
                await WebSocketContext.SendMessageAsync(Newtonsoft.Json.JsonConvert.SerializeObject(sendMessage));
            }
            return model;
        }
    }
}
