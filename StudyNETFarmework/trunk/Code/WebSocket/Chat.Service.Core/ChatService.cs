using Chat.Service.Core.Entities;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Core
{
    public class ChatService:WebSocketService.Core.WebSocketService
    {
        #region  私有属性
        private List<User> Users;
        private ILog log = LogManager.GetLogger<ChatService>();
        #endregion

        #region 成员属性
        /// <summary>
        /// 创建资源库方法
        /// </summary>
        public Func<Repositories.IChatServiceRepository> CreateRepositoryFunc { get; private set; }
        /// <summary>
        /// 获取用户ID方法
        /// </summary>
        public Func<HttpListenerWebSocketContext,Guid> GetUidFunc { get; private set; }
        #endregion

        #region 构造函数
        public ChatService(string ip, int port) : base(ip, port, "chat")
        {
            OnWebSocketServiceBeginStart += ChatService_OnWebSocketServiceBeginStart;
            OnAddWebSocket += ChatService_OnAddWebSocket;
            OnRemoveWebSocket += ChatService_OnRemoveWebSocket;
            OnReceiveMessage += ChatService_OnReceiveMessage;
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 设置创建资源库方法
        /// </summary>
        /// <param name="func"></param>
        public void SetCreateRepositoryFunc(Func<Repositories.IChatServiceRepository> func)
        {
            CreateRepositoryFunc = func;
        }
        /// <summary>
        /// 设置获取用户ID方法
        /// </summary>
        /// <param name="func"></param>
        public void SetGetUidFunc(Func<HttpListenerWebSocketContext,Guid> func)
        {
            GetUidFunc = func;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 服务开启前触发
        /// </summary>
        /// <param name="service"></param>
        /// <param name="httpListener"></param>
        private void ChatService_OnWebSocketServiceBeginStart(WebSocketService.Core.WebSocketService service, System.Net.HttpListener httpListener)
        {
            if (CreateRepositoryFunc == null)
            {
                httpListener.Close();
                throw new Exception("未设置资创建源库方法！");
            }
            using (var repository = CreateRepositoryFunc())
            {
                Users = repository.GetUsers().ToList();
            }
        }

        /// <summary>
        /// 当接收到消息时触发
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        private void ChatService_OnReceiveMessage(WebSocketService.Core.WebSocketContext context, string message)
        {
            ReceiveMessage receiveMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceiveMessage>(message );
            switch (receiveMessage.MessageType)
            {
                case MessageType.message:
                    switch (receiveMessage.ReceiverType)
                    {
                        case ReceiverType.individual:
                            IndividualSendMessage(receiveMessage);
                            break;
                        case ReceiverType.group:
                            GroupSendMessage(receiveMessage);
                            break;
                    }
                    break;
                case MessageType.readStatusChange:
                    MessageRead(receiveMessage);
                    break;
            }
        }

        /// <summary>
        /// 发送消息给个体
        /// </summary>
        /// <param name="receiveMessage"></param>
        private void IndividualSendMessage(ReceiveMessage receiveMessage)
        {
            var user = Users.FirstOrDefault(p => p.UserId.Equals(receiveMessage.ReceiverId));
            var senduser = Users.FirstOrDefault(p => p.UserId.Equals(receiveMessage.SenderId));
            if (senduser == null)
            {
                log.Error(new Exception("未获取到发送用户！"));
                return;
            }
            var sendMessage = new SendMessage(receiveMessage.MessageId,receiveMessage.SenderId, receiveMessage.ReceiverType, receiveMessage.MessageType, receiveMessage.Message);
            try
            {
                var message = user.SendMessageAsync(sendMessage).Result;
                using (var repository = CreateRepositoryFunc())
                {
                    repository.AddUserChatMessage(message);
                    repository.SaveChange();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                var errorMessage = new SendErrorMessage(Guid.NewGuid(), receiveMessage.SenderId, receiveMessage.ReceiverType, receiveMessage.MessageId
                    , $"消息发送失败！{GetExceptionMessage(ex)}");
                senduser.SendMessageAsync(errorMessage).Wait();
            }
        }

        /// <summary>
        /// 发送消息给用户组
        /// </summary>
        /// <param name="receiveMessage"></param>
        private void GroupSendMessage(ReceiveMessage receiveMessage)
        {
            /**
             * 获取组用户列表
             * 排除发消息用户
             * 循环发送消息
             * */
            using (var repository = CreateRepositoryFunc())
            {
                User senduser = Users.FirstOrDefault(p => p.UserId.Equals(receiveMessage.SenderId));
                try
                {
                    var chatGroup = repository.GetChatGroups().FirstOrDefault(p => p.ChatGroupId.Equals(receiveMessage.ReceiverId));
                    if (chatGroup == null)
                    {
                        var errorMessage = new SendErrorMessage(Guid.NewGuid(), receiveMessage.SenderId, receiveMessage.ReceiverType, receiveMessage.MessageId
                            , $"用户组【{receiveMessage.ReceiverId}】不能存在，消息发送失败！");
                        senduser.SendMessageAsync(errorMessage).Wait();
                        return;
                    }

                    var sendMessage = new SendMessage(receiveMessage.MessageId, receiveMessage.SenderId, receiveMessage.ReceiverType, receiveMessage.MessageType, receiveMessage.Message);
                    var chatGroupMessage = chatGroup.SendMessage(senduser,sendMessage);
                    repository.AddChatGroupMessage(chatGroupMessage);
                    foreach (var recevieUser in chatGroupMessage.Users)
                    {
                        if (recevieUser.ChatGroupUser.UserId == senduser.UserId)
                            continue;
                        
                        repository.AddChatGroupMessageReceiverUser(recevieUser);
                        var chatGrouUser= Users.FirstOrDefault(p => p.UserId.Equals(recevieUser.ChatGroupUser.UserId));
                        chatGrouUser.SendMessageAsync(sendMessage).Wait();
                    }
                    repository.SaveChange();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    var errorMessage = new SendErrorMessage(Guid.NewGuid(),receiveMessage.SenderId, receiveMessage.ReceiverType, receiveMessage.MessageId
                        , $"消息发送失败！{GetExceptionMessage(ex)}");
                    senduser.SendMessageAsync(errorMessage).Wait();
                }
            }
        }

        /// <summary>
        /// 当服务删除WebSocket时触发
        /// </summary>
        /// <param name="webSocketContext"></param>
        private void ChatService_OnRemoveWebSocket(WebSocketService.Core.WebSocketContext webSocketContext)
        {
            Guid userId = GetUidFunc(webSocketContext.HttpListenerWebSocketContext);
            var user = Users.FirstOrDefault(p => p.UserId.Equals(userId));
            if (user != null)
                user.ChatOffLine();
        }
        /// <summary>
        /// 当服务添加WebSocket时触发
        /// </summary>
        /// <param name="webSocketContext"></param>
        private void ChatService_OnAddWebSocket(WebSocketService.Core.WebSocketContext webSocketContext)
        {
            Guid uid = default(Guid);
            if (GetUidFunc == null && webSocketContext.HttpListenerWebSocketContext.User != null)
            {
                string name = webSocketContext.HttpListenerWebSocketContext.User.Identity.Name;
                uid = Guid.Parse(name);
            }
            else
            {
                uid = GetUidFunc(webSocketContext.HttpListenerWebSocketContext);
            }
            var user = Users.FirstOrDefault(p => p.UserId.Equals(uid));
            List<ReceiveMessage> receiveMessages = null;
            using (var repository = CreateRepositoryFunc())
            {
                if (user == null)
                {
                    user = repository.GetUsers().FirstOrDefault(p => p.UserId.Equals(uid));
                    if (user == null) throw new Exception($"用户【{uid}】不存在！");
                    Users.Add(user);
                }
                receiveMessages = repository.GetUnReadChatMessage(uid).ToList();
            }
            user.ChatOnLine(webSocketContext);
            foreach (var message in receiveMessages)
            {
                var sendMessage = new SendMessage(message.MessageId, message.SenderId, message.ReceiverType, message.MessageType, message.Message);
                user.SendMessageAsync(sendMessage).Wait();
            }
        }
        /// <summary>
        /// 消息已阅读
        /// </summary>
        /// <param name="message"></param>
        private void MessageRead(ReceiveMessage message)
        {
            var sendMessage = new SendReadStatusMessage(message.MessageId,message.SenderId, message.ReceiverType,message.MessageId, true, "消息已阅读");
            using (var repository = CreateRepositoryFunc())
            {
                try
                {
                    var sendUser = Users.FirstOrDefault(p => p.UserId.Equals(message.ReceiverId));
                    switch (message.ReceiverType)
                    {
                        case ReceiverType.group:
                            var chatGroupMessage = repository.GetChatGroupMessage(message.MessageId);
                            var chatGroupMessReceiveUser = chatGroupMessage.Users.FirstOrDefault(p => p.ChatGroupUser.UserId.Equals(message.ReceiverId));
                            chatGroupMessReceiveUser.Read();
                            repository.UpdateChatGroupMessageReceiverUser(chatGroupMessReceiveUser);
                            break;
                        case ReceiverType.individual:
                            var userChatMessage = repository.GetUserChatMessage(message.MessageId);
                            userChatMessage.Read();
                            repository.UpdateUserChatMessage(userChatMessage);
                            break;
                    }
                    repository.SaveChange();
                    sendUser.SendMessageAsync(sendMessage).Wait();
                }catch(Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        /// <summary>
        /// 递归获取异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string GetExceptionMessage(Exception ex)
        {
            if (ex == null) return string.Empty;

            string message = ex.Message;
            if (ex.InnerException != null)
            {
                message += $" => {GetExceptionMessage(ex.InnerException)}";
            }
            return message;
        }
        #endregion
    }
}
