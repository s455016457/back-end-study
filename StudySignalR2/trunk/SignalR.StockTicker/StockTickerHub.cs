using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalR.StockTicker.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SignalR.StockTicker
{
    /// <summary>
    /// 股票自动报机总线
    /// </summary>
    [HubName("stockTickerMini")]
    public class StockTickerHub : Hub
    {
        readonly StockTicker stockTicker;

        public StockTickerHub() : this(StockTicker.Instance) { }

        public StockTickerHub(StockTicker stockTicker)
        {
            this.stockTicker = stockTicker;
        }
        /// <summary>
        /// 获取所有股票
        /// 提供给客户端调用
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stock> GetAllStocks()
        {
            return stockTicker.GetAllStocks();
        }
        /// <summary>
        /// 开市
        /// </summary>
        /// <returns></returns>
        public bool OpenMarket()
        {
            return stockTicker.OpenMarket();
        }
        /// <summary>
        /// 关市
        /// </summary>
        /// <returns></returns>
        public bool CloseMarket()
        {
            return stockTicker.CloseMarket();
        }
    }
    /// <summary>
    /// 股票自动报机
    /// </summary>
    public class StockTicker
    {
        private readonly static Lazy<StockTicker> stockTicker = new Lazy<StockTicker>(() => new StockTicker(GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>().Clients));
        /// <summary>
        /// 股票集合
        /// 线程安全的键值对集合
        /// </summary>
        private readonly static ConcurrentDictionary<string, Stock> stocks = new ConcurrentDictionary<string, Stock>();
        readonly static object _updateStockPricesLock = new object();
        bool _updatingStockPrices;
        Random _updateOrNotRandom = new Random(1);
        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
        Timer timer;

        private static bool marketIsOpened = false;

        // 股票在每次变动时可以根据这个因素上升或下降一个百分比
        private readonly double _rangePercent = .002;

        private IHubConnectionContext<dynamic> Clients { get;  set; }

        public StockTicker (IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            var st = new List<Stock> {
                new Stock("Google"){Price=10.54M},
                new Stock("Apple"){Price=12.34M},
                new Stock("BaiDu"){Price=6.04M},
                new Stock("HuaWei"){Price=11.04M}
            };
            stocks.Clear();
            st.ForEach(p => stocks.TryAdd(p.Symbol, p));

        }
        
        public static StockTicker Instance => stockTicker.Value;
        /// <summary>
        /// 获取所有股票
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stock> GetAllStocks()
        {
            return stocks.Values;
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <returns></returns>
        public bool OpenMarket()
        {
            if (marketIsOpened)
                throw new Exception("已开市，不能再次开市！");
            marketIsOpened = true;
            var st = new List<Stock>();
            foreach (var item in stocks.Values)
            {
                st.Add(new Stock(item.Symbol) { Price = item.Price });
            }

            stocks.Clear();
            st.ForEach(p => stocks.TryAdd(p.Symbol, p));

            timer = new Timer(UpdateStockPrice, null, _updateInterval, _updateInterval);
            return true;
        }
        /// <summary>
        /// 关市
        /// </summary>
        /// <returns></returns>
        public bool CloseMarket()
        {
            if (!marketIsOpened)
                throw new Exception("已g关市，不能再次关市！");
            marketIsOpened = false;

            timer.Dispose();
            return true;
        }

        private void UpdateStockPrice(object state)
        {
            lock (_updateStockPricesLock)
            {
                if (!_updatingStockPrices)
                {
                    _updatingStockPrices = true;
                    foreach (var stock in stocks.Values)
                    {
                        if (TryUpdateRandomPrice(stock))
                            BroadcastStockPrice(stock);
                    }
                    _updatingStockPrices = false;
                }
            }
        }

        private Boolean TryUpdateRandomPrice(Stock stock)
        {
            var r = _updateOrNotRandom.NextDouble();
            if (r > .1)
                return false;

            // 以范围百分比的随机因子更新股票价格           
            var random = new Random((int)Math.Floor(stock.Price));
            var percentChange = random.NextDouble() * _rangePercent;
            var pos = random.NextDouble() >= 0.49;
            var change = Math.Round(stock.Price * (decimal)percentChange, 2);
            change = pos ? change : -1 * change;

            stock.Price += change;
            return true;
        }
        /// <summary>
        /// 广播股票价格
        /// </summary>
        /// <param name="stock"></param>
        private void BroadcastStockPrice(Stock stock)
        {
            // 调用所有客户端的updateStockPrice方法
            Clients.All.updateStockPrice(stock);
        }
    }
}