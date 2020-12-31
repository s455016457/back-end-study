using Chat.Service.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Core.Repositories
{
    /// <summary>
    /// 聊天服务器资源库
    /// </summary>
    public interface IChatServiceRepository : IDisposable
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        IQueryable<User> GetUsers();
        /// <summary>
        /// 获取聊天群
        /// </summary>
        /// <returns></returns>
        IQueryable<ChatGroup> GetChatGroups();
        /// <summary>
        /// 获取用户未读消息
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        IQueryable<ReceiveMessage> GetUnReadChatMessage(Guid UserId);
        /// <summary>
        /// 获取用户聊天消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserChatMessage GetUserChatMessage(Guid id);
        /// <summary>
        /// 添加用户聊天信息
        /// </summary>
        /// <param name="userChatMessage"></param>
        void AddUserChatMessage(UserChatMessage userChatMessage);
        /// <summary>
        /// 更新用户聊天信息
        /// </summary>
        /// <param name="userChatMessage"></param>
        void UpdateUserChatMessage(UserChatMessage userChatMessage);
        /// <summary>
        /// 添加群聊天信息
        /// </summary>
        /// <param name="chatGroupMessage"></param>
        void AddChatGroupMessage(ChatGroupMessage chatGroupMessage);
        /// <summary>
        /// 获取群聊天信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ChatGroupMessage GetChatGroupMessage(Guid id);
        /// <summary>
        /// 获取聊天群信息接收用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ChatGroupMessageReceiverUser GetChatGroupMessageReceiverUser(Guid id);
        /// <summary>
        /// 获取聊天群信息接收用户
        /// </summary>
        /// <param name="chatGroupMessageid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ChatGroupMessageReceiverUser GetChatGroupMessageReceiverUser(Guid chatGroupMessageid,Guid userId);
        /// <summary>
        /// 添加聊天群信息接收用户
        /// </summary>
        /// <param name="chatGroupMessageReceiverUser"></param>
        void AddChatGroupMessageReceiverUser(ChatGroupMessageReceiverUser chatGroupMessageReceiverUser);
        /// <summary>
        /// 更新聊天群信息接收用户
        /// </summary>
        /// <param name="chatGroupMessageReceiverUser"></param>
        void UpdateChatGroupMessageReceiverUser(ChatGroupMessageReceiverUser chatGroupMessageReceiverUser);
        /// <summary>
        /// 保存更新
        /// </summary>
        void SaveChange();
    }
}
