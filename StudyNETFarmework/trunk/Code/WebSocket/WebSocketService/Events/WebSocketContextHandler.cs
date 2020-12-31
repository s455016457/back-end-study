using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketService.Events
{
    /// <summary>
    /// 发送消息前
    /// </summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    public delegate bool BeforeSendHandler(WebSocketContext context, string message);
    /// <summary>
    /// 发送消息后
    /// </summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    public delegate void AflterSendHandler(WebSocketContext context, string message);
    /// <summary>
    /// 接收消息前
    /// </summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public delegate bool BeforeReceiveHandler(WebSocketContext context, ArraySegment<byte> buffer);
    /// <summary>
    /// 接收到消息后
    /// </summary>
    /// <param name="context">WebSocket上下文</param>
    /// <param name="message">消息</param>
    public delegate void AflterReceiveHandler(WebSocketContext context, string message);
    /// <summary>
    /// WebSocket 关闭
    /// </summary>
    /// <param name="context">WebSocket上下文</param>
    /// <param name="webSocketCloseStatus"></param>
    /// <param name="closeStatusDescription"></param>
    public delegate void CloseWebStocketHandler(WebSocketContext context, WebSocketCloseStatus webSocketCloseStatus, string closeStatusDescription);
    public static class WebSocketContextHandler
    {
        /// <summary>
        /// 发送消息前 返回false 则停止继续执行
        /// </summary>
        public static event BeforeSendHandler BeforeSendEvent;
        /// <summary>
        /// 发送消息后
        /// </summary>
        public static event AflterSendHandler AflterSendEvent;
        /// <summary>
        /// 接收消息前 返回false 则停止继续执行
        /// </summary>
        public static event BeforeReceiveHandler BeforeReceiveEvent;
        /// <summary>
        /// 接收到消息后
        /// </summary>
        public static event AflterReceiveHandler AflterReceiveEvent;
        /// <summary>
        /// WebSocket 关闭
        /// </summary>
        public static event CloseWebStocketHandler CloseWebStocketEvent;

        /// <summary>
        /// 执行 发送消息前 事件 返回false 则停止继续执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestLogger"></param>
        internal static bool BeforeSendHandler(WebSocketContext context, string message)
        {
            if (BeforeSendEvent == null) return true;
            foreach (BeforeSendHandler item in BeforeSendEvent.GetInvocationList())
            {
                if (!item.Invoke(context, message))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 执行 发送消息后 事件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestLogger"></param>
        internal static void AflterSendHandler(WebSocketContext context, string message)
        {
            if (AflterSendEvent == null) return;
            AflterSendEvent(context, message);
        }

        /// <summary>
        /// 执行 接收消息前 事件 返回false 则停止继续执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestLogger"></param>
        internal static bool BeforeReceiveHandler(WebSocketContext context, ArraySegment<byte> buffer)
        {
            if (BeforeReceiveEvent == null) return true;
            foreach (BeforeReceiveHandler endRequest in BeforeReceiveEvent.GetInvocationList())
            {
                if (!endRequest.Invoke(context, buffer))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 执行 接收到消息后 事件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestLogger"></param>
        internal static void AflterReceiveHandler(WebSocketContext context, string message)
        {
            if (AflterReceiveEvent == null) return;
            AflterReceiveEvent(context, message);
        }
        /// <summary>
        /// WebSocket 关闭
        /// </summary>
        /// <param name="webSocketCloseStatus"></param>
        /// <param name="closeStatusDescription"></param>
        internal static void CloseWebStocketHandler(WebSocketContext context, WebSocketCloseStatus webSocketCloseStatus, string closeStatusDescription)
        {
            if (CloseWebStocketEvent == null) return;
            CloseWebStocketEvent(context,webSocketCloseStatus, closeStatusDescription);
        }
    }
}
