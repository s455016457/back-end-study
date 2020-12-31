using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MVCChat
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="message">消息</param>
        public void Send(Message message)
        {
            var user = Context.User;
            var id = Context.ConnectionId;
            var index = Context.User.Identity;

            foreach (var userName in message.ReceiveUser)
            {
                var users = OnLineUserService.GetOnLineUsers(userName);
                foreach (var u in users)
                {
                    Clients.Client(u.ConnectionId).addNewMessageToPage(message.SendBy, message.Msg);
                }
            }
        }

        public List<string> GetAllOnLineUser()
        {
           return OnLineUserService.GetAllOnLineUses();
        }

        /// <summary>
        /// 建立连接时触发
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            var userName = Context.QueryString["userName"];
            var connectionId = Context.ConnectionId;
            OnLineUserService.AddOnLineUser(connectionId, userName);
            return base.OnConnected();
        }

        /// <summary>
        /// 断开连接时触发
        /// </summary>
        /// <param name="stopCalled">
        /// true 客户端关闭连接
        /// false 连接超时
        /// </param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            /**
             * stopCalled
             * true 客户端关闭连接
             * false 连接超时
             * */
            var index = 0;
            while (!OnLineUserService.RemoveOnLineUser(Context.ConnectionId))
            {
                if(index++>99)
                {
                    /**
                     * 做日志记录
                     * 在线用户删除失败
                     * */
                    break;
                }
            }
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 连接重连时触发
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            var userName = Context.QueryString["userName"];
            var connectionId = Context.ConnectionId;
            OnLineUserService.AddOnLineUser(connectionId, userName);
            return base.OnReconnected();
        }
    }

    public class Message
    {
        public string[] ReceiveUser { get; set; }
        public string Msg { get; set; }

        public string SendBy { get; set; }
    }

    public class OnLineUserBroadcast
    {
        private readonly static Lazy<OnLineUserBroadcast> _instance = new Lazy<OnLineUserBroadcast>(() => new OnLineUserBroadcast(GlobalHost.ConnectionManager.GetHubContext<ChatHub>()));

        private static bool _onLineUserChanged = false;
        private static object lock_obj = new object();
        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);

        private IHubContext _context;
        Timer timer;

        private OnLineUserBroadcast(IHubContext context)
        {
            _context = context;
            timer = new Timer(Broadcast, null, _updateInterval, _updateInterval);
        }

        public static OnLineUserBroadcast Instance => _instance.Value;

        /// <summary>
        /// 在线用户已更改
        /// </summary>
        public void OnLineUserChange()
        {
            lock (lock_obj)
            {
                _onLineUserChanged = true;
            }
        }
        private void Broadcast(object state)
        {
            if (!_onLineUserChanged) return;
            lock (lock_obj)
            {
                if (!_onLineUserChanged) return;
                Broadcast();
                _onLineUserChanged = false;
            }
        }


        /// <summary>
        /// 广播更改后的在线用户
        /// 有定时器调用
        /// </summary>
        private void Broadcast()
        {
            _context.Clients.All.onLineUsersChange(OnLineUserService.GetAllOnLineUses());
        }
    }

    /// <summary>
    /// 在线用户服务
    /// </summary>
    public static class OnLineUserService
    {
        private static ConcurrentBag<OnLineUser> users = new ConcurrentBag<OnLineUser>();
        static object lock_obj = new object();

        /// <summary>
        /// 添加在线用户
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="userName"></param>
        public static void AddOnLineUser(string connectionId, string userName)
        {
            if (users.Any(p => p.ConnectionId.Equals(connectionId)))
                return;
            lock (lock_obj)
            {
                if (users.Any(p => p.ConnectionId.Equals(connectionId)))
                    return;
                users.Add(OnLineUser.CreateNew(connectionId, userName));
                OnLineUserBroadcast.Instance.OnLineUserChange();
            }
        }

        /// <summary>
        /// 删除在线用户
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public static bool RemoveOnLineUser(string connectionId)
        {
            if (!users.Any(p => p.ConnectionId.Equals(connectionId)))
                return true;
            lock (lock_obj)
            {
                var user = users.FirstOrDefault(p => p.ConnectionId.Equals(connectionId));
                if (user != null)
                    return true;

                if( users.TryTake(out user))
                {
                    OnLineUserBroadcast.Instance.OnLineUserChange();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public static OnLineUser GetOnLineUser(string connectionId)
        {
            return users.FirstOrDefault(p => p.ConnectionId.Equals(connectionId)); 
        }

        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static List<OnLineUser> GetOnLineUsers(string userName)
        {
            return users.Where(p => p.UserName.Equals(userName)).ToList();
        }

        public static List<string> GetAllOnLineUses()
        {
            return users.Select(p => p.UserName).Distinct().ToList();
        }
    }

    public class OnLineUser
    {
        public string ConnectionId { get; private set; }
        public string UserName { get; private set; }

        public static OnLineUser CreateNew(string connectionId,string userName)
        {
            return new OnLineUser
            {
                ConnectionId = connectionId,
                UserName = userName
            };
        }
    }
}