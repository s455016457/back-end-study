using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Core.Entities
{
    /// <summary>
    /// 聊天群信息
    /// </summary>
    public class ChatGroupMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public Guid ChatGroupMessageId { get; set; }
        /// <summary>
        /// 发送者ID
        /// </summary>
        public Guid SenderId { get; private set; }
        /// <summary>
        /// 聊天群ID
        /// </summary>
        public Guid ChatGroupId { get; private set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendStamp { get; private set; }
        /// <summary>
        /// 聊天群消息接收用户
        /// </summary>
        public virtual List<ChatGroupMessageReceiverUser> Users { get; private set; }
        /// <summary>
        /// 消息发送者
        /// </summary>
        public virtual User SenderUser { get; private set; }
        /// <summary>
        /// 聊天群
        /// </summary>
        public virtual ChatGroup ChatGroup { get; private set; }

        private ChatGroupMessage() { }

        public ChatGroupMessage(User SendUser, ChatGroup ChatGroup, SendMessage sendMessage, DateTime SendStamp)
        {
            ChatGroupMessageId = sendMessage.MessageId;
            this.SenderUser = SendUser;
            this.SenderId = SendUser.UserId;
            this.ChatGroupId = ChatGroup.ChatGroupId;
            this.ChatGroup = ChatGroup;
            this.Message = sendMessage.Message;
            this.SendStamp = SendStamp;
            Users = new List<ChatGroupMessageReceiverUser>();
            foreach (var user in ChatGroup.Users)
            {
                Users.Add(new ChatGroupMessageReceiverUser(this, user));
            }
        }
    }

    /// <summary>
    /// 聊天群消息接收用户
    /// </summary>
    public class ChatGroupMessageReceiverUser
    {
        /// <summary>
        /// 聊天群消息接收用户ID
        /// </summary>
        public Guid ID { get; private set; }
        /// <summary>
        /// 群消息ID
        /// </summary>
        public Guid ChatGroupMessageId { get; private set; }
        /// <summary>
        /// 群用户ID
        /// </summary>
        public Guid ChatGroupUserID { get; private set; }
        /// <summary>
        /// 已阅读
        /// </summary>
        public bool IsRead { get; private set; }
        /// <summary>
        /// 聊天群信息
        /// </summary>
        public virtual ChatGroupMessage ChatGroupMessage { get; private set; }
        /// <summary>
        /// 聊天群用户
        /// </summary>
        public virtual ChatGroupUser ChatGroupUser { get; private set; } 

        private ChatGroupMessageReceiverUser() { }
        /// <summary>
        /// 初始化聊天群消息接收用户
        /// </summary>
        /// <param name="ChatGroupMessageId">聊天群消息ID</param>
        /// <param name="ChatGroupUserID">聊天群用户ID</param>
        public ChatGroupMessageReceiverUser(Guid ChatGroupMessageId, Guid ChatGroupUserID)
        {
            this.ID = Guid.NewGuid();
            this.ChatGroupMessageId = ChatGroupMessageId;
            this.ChatGroupUserID = ChatGroupUserID;
        }
        public ChatGroupMessageReceiverUser(ChatGroupMessage ChatGroupMessage, ChatGroupUser ChatGroupUser)
        {
            this.ID = Guid.NewGuid();
            this.ChatGroupMessageId = ChatGroupMessage.ChatGroupMessageId;
            this.ChatGroupUserID = ChatGroupUser.ChatGroupUserID;
            this.ChatGroupUser = ChatGroupUser;
            this.ChatGroupMessage = ChatGroupMessage;
        }

        /// <summary>
        /// 阅读
        /// </summary>
        public void Read()
        {
            IsRead = true;
        }
    }

    /// <summary>
    /// 聊天群
    /// </summary>
    public class ChatGroup
    {
        /// <summary>
        /// 聊天群ID
        /// </summary>
        public Guid ChatGroupId { get; private set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 聊天群用户
        /// </summary>
        public virtual List<ChatGroupUser> Users { get; private set; }

        private ChatGroup() { }
        public ChatGroup(Guid ChatGroupId, string Name, List<ChatGroupUser> Users)
        {
            this.ChatGroupId = ChatGroupId;
            this.Name = Name;
            this.Users = Users;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendMessage"></param>
        /// <returns></returns>
        public ChatGroupMessage SendMessage(User SendUser,SendMessage sendMessage)
        {
            return new ChatGroupMessage(SendUser, this, sendMessage, DateTime.Now);
        }
    }

    /// <summary>
    /// 聊天群用户
    /// </summary>
    public class ChatGroupUser
    {
        /// <summary>
        /// 聊天群用户ID
        /// </summary>
        public Guid ChatGroupUserID { get; set; }
        /// <summary>
        /// 聊天群ID
        /// </summary>
        public Guid ChatGroupId { get; private set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; private set; }
        /// <summary>
        /// 聊天群
        /// </summary>
        public virtual ChatGroup ChatGroup { get; private set; }
        /// <summary>
        /// 聊天群用户
        /// </summary>
        public virtual User User { get; private set; }
        private ChatGroupUser() { }

        public ChatGroupUser(Guid ChatGroupUserID, Guid ChatGroupId, Guid UserId)
        {
            this.ChatGroupId = ChatGroupId;
            this.ChatGroupUserID = ChatGroupUserID;
            this.UserId = UserId;
        }
        public ChatGroupUser(Guid ChatGroupUserID,  ChatGroup ChatGroup, User User)
        {
            this.ChatGroupId = ChatGroup.ChatGroupId;
            this.ChatGroupUserID = ChatGroupUserID;
            this.UserId = User.UserId;
            this.ChatGroup = ChatGroup;
            this.User = User;
        }
    }
}
