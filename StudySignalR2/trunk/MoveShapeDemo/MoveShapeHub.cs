using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace MoveShapeDemo
{
    /// <summary>
    /// 定义一个广播
    /// 用于限制传出消息速率
    /// </summary>
    public class Broadcaster
    {
        // 定义一个惰加载的静态广播实例
        private readonly static Lazy<Broadcaster> broadcaster = new Lazy<Broadcaster>(() => new Broadcaster());
        /// <summary>
        ///  广播频率
        /// </summary>
        private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(40);
        /// <summary>
        /// 定义hub上下文
        /// </summary>
        private readonly IHubContext _hubContext;
        /// <summary>
        /// 定义广播定时器
        /// </summary>
        private Timer _broadcastLoop;
        /// <summary>
        /// 模组已更新
        /// </summary>
        private bool _modelUpdated;
        /// <summary>
        /// 图形模组
        /// </summary>
        private ShapeModel _model;

        private Broadcaster()
        {
            // 获取连接上下文
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<MoveShapeHub>();
            _model = new ShapeModel();
            _modelUpdated = false;
            // 初始化定时器
            _broadcastLoop = new Timer(BroadcastShape, null, BroadcastInterval, BroadcastInterval);
        }
        /// <summary>
        /// 广播图形
        /// </summary>
        /// <param name="state"></param>
        public void BroadcastShape(object state) {
            if (_modelUpdated) {
                _hubContext.Clients.AllExcept(_model.LastUpdateBy).updateShape(_model);
                _modelUpdated = false;
            }
        }
        /// <summary>
        /// 更新图形
        /// </summary>
        /// <param name="clientModel"></param>
        public void UpdateShape(ShapeModel clientModel)
        {
            _model = clientModel;
            _modelUpdated = true;
        }

        /// <summary>
        /// 广播实例
        /// </summary>
        public static Broadcaster Instance => broadcaster.Value;
    }

    public class MoveShapeHub : Hub
    {
        private Broadcaster _broadcaster;
        public MoveShapeHub():this(Broadcaster.Instance) { }

        public MoveShapeHub(Broadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdateBy = Context.ConnectionId;
            // 调用客户端定义的updateShape方法
            // Clients.AllExcept(clientModel.LastUpdateBy).updateShape(clientModel);

            // 调用广播的更新图形方法
            _broadcaster.UpdateShape(clientModel);
        }
    }

    public class ShapeModel
    {
        [JsonProperty("left")]
        public decimal X { get; set; }
        [JsonProperty("top")]
        public decimal Y { get; set; }
        [JsonIgnore]
        public string LastUpdateBy { get; set; }
    }
}