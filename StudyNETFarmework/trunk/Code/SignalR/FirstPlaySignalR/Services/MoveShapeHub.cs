using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace FirstPlaySignalR.Services
{
    /// <summary>
    /// 移动图形中心
    /// </summary>
    public class MoveShapeHub:Hub
    {
        private Broadcaster broadcaster;

        public MoveShapeHub() : this(Broadcaster.Instance) { }

        public MoveShapeHub(Broadcaster broadcaster)
        {
            this.broadcaster = broadcaster;
        }

        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;
            // 排除自己，在我们的广播器中更新形状模型
            //Clients.AllExcept(clientModel.LastUpdatedBy).updateShape(clientModel);

            // 更新定时广播器形状
            broadcaster.UpdateShape(clientModel);
        }
    }
    /// <summary>
    /// 形状模型
    /// </summary>
    public class ShapeModel
    {
        [JsonProperty("left")]
        public double Left { get; set; }
        [JsonProperty("top")]
        public double Top { get; set; }
        [JsonProperty]
        public string LastUpdatedBy { get; set; }
    }

    /// <summary>
    /// 定时广播类
    /// </summary>
    public class Broadcaster
    {
        /// <summary>
        /// 提供对延迟初始化的支持  延时初始化定时广播
        /// </summary>
        private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());
        /// <summary>
        /// 广播时间间隔
        /// </summary>
        private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(40);
        private readonly IHubContext _hubContext;
        /// <summary>
        /// 定时器
        /// </summary>
        private Timer _broadcastLoop;
        /// <summary>
        /// 形状模型
        /// </summary>
        private ShapeModel _model;
        /// <summary>
        /// 模型已更改
        /// </summary>
        private bool _modelUpdated;

        public Broadcaster()
        {
            // 保存我们的hub上下文，以便我们可以轻松地使用它发送到它的连接客户端
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<MoveShapeHub>();
            _model = new ShapeModel();
            _modelUpdated = false;
            // Start the broadcast loop
            _broadcastLoop = new Timer(
                BroadcastShape,
                null,
                BroadcastInterval,
                BroadcastInterval);
        }
        /// <summary>
        /// 广播形状
        /// </summary>
        /// <param name="state"></param>
        public void BroadcastShape(object state)
        {
            // No need to send anything if our model hasn't changed
            if (_modelUpdated)
            {
                // 排除自己，在我们的广播器中更新形状模型
                _hubContext.Clients.AllExcept(_model.LastUpdatedBy).updateShape(_model);
                _modelUpdated = false;
            }
        }
        /// <summary>
        /// 更改形状
        /// </summary>
        /// <param name="clientModel"></param>
        public void UpdateShape(ShapeModel clientModel)
        {
            _model = clientModel;
            _modelUpdated = true;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static Broadcaster Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }
}