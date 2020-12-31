using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace FirstPlaySignalR.Services
{
    /// <summary>
    /// 股票行情自动收录器中心
    /// </summary>
    [HubName("stockTickerMini")]
    public class StockTickerHub:Hub
    {
        /// <summary>
        /// 股票行情自动收录器
        /// </summary>
        private StockTicker _stockTicker;
        /// <summary>
        /// 初始化股票行情自动收录器中心
        /// </summary>
        public StockTickerHub() : this(StockTicker.Instance) { }
        /// <summary>
        /// 初始化股票行情自动收录器中心
        /// </summary>
        /// <param name="stockTicker">股票行情自动收录器</param>
        public StockTickerHub(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }
        /// <summary>
        /// 获取所有股票
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stock> GetAllStocks()
        {
            return _stockTicker.GetAllStocks();
        }

        public string GetMarketState()
        {
            return _stockTicker.MarketState.ToString();
        }

        /// <summary>
        /// 开市
        /// </summary>
        public void OpenMarket()
        {
            _stockTicker.OpenMarket();
        }
        /// <summary>
        /// 收市
        /// </summary>
        public void CloseMarket()
        {
            _stockTicker.CloseMarket();
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _stockTicker.Reset();
        }
    }
    /// <summary>
    /// 股票行情自动收录器
    /// </summary>
    public class StockTicker
    {
        private readonly static Lazy<StockTicker> _instance = new Lazy<StockTicker>(() => new StockTicker(GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>().Clients));
        private readonly ConcurrentDictionary<string,Stock> _stocks = new ConcurrentDictionary<string, Stock>();
        private readonly object _updateStockPricesLock = new object();
        private readonly object _marketStateLock = new object();
        /// <summary>
        /// 每一次变动，股票都会上涨或下跌这个因素的一个百分比
        /// </summary>
        private readonly double _rangePercent = .002;
        /// <summary>
        /// 变动评率
        /// </summary>
        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Random _updateOrNotRandom = new Random();
        private Timer _timer;
        /// <summary>
        /// 正在修改单价
        /// </summary>
        private volatile bool _updatingStockPrices = false;
        /// <summary>
        /// 市场状态  volatile 指示一个字段可以由多个同时执行的线程修改
        /// </summary>
        private volatile MarketState _marketState=default(MarketState);

        private StockTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            LoadDefaultStocks();
        }
        /// <summary>
        /// 股票行情自动收录器实例
        /// </summary>
        public static StockTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        /// <summary>
        /// 市场状态
        /// </summary>
        public MarketState MarketState
        {
            get
            { return _marketState; }
            set
            {
                _marketState = value;
            }
        }
        /// <summary>
        /// 获取所有股票
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stock> GetAllStocks()
        {
            return _stocks.Values;
        }
        /// <summary>
        /// 开市
        /// </summary>
        public void OpenMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Open)
                {
                    _timer = new Timer(UpdateStockPrices, null, _updateInterval, _updateInterval);
                    MarketState = MarketState.Open;
                    BroadcastMarketStateChange(MarketState.Open);
                }
            }
        }
        /// <summary>
        /// 收市
        /// </summary>
        public void CloseMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Closed)
                {
                    if (_timer != null)
                        _timer.Dispose();
                    MarketState = MarketState.Closed;
                    BroadcastMarketStateChange(MarketState.Closed);
                }
            }
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Closed)
                {
                    throw new InvalidOperationException("Market must be closed before it can be reset.");
                }
                LoadDefaultStocks();
                BroadcastMarketReset();
            }
        }
        /// <summary>
        /// 加载默认股票
        /// </summary>
        private void LoadDefaultStocks()
        {
            _stocks.Clear();
            var stocks = new List<Stock>
            {
                new Stock { Symbol = "MSFT", Price = 30.31m },
                new Stock { Symbol = "APPL", Price = 578.18m },
                new Stock { Symbol = "SWKEJI", Price = 478.18m },
                new Stock { Symbol = "XMKEJI", Price = 408.18m },
                new Stock { Symbol = "GOOG", Price = 570.30m }
            };
            stocks.ForEach(stock => _stocks.TryAdd(stock.Symbol, stock));
        }

        private void UpdateStockPrices(object state)
        {
            lock (_updateStockPricesLock)
            {
                if (!_updatingStockPrices)
                {
                    _updatingStockPrices = true;

                    foreach (var stock in _stocks.Values)
                    {
                        if (TryUpdateStockPrice(stock))
                        {
                            BroadcastStockPrice(stock);
                        }
                    }

                    _updatingStockPrices = false;
                }
            }
        }

        private bool TryUpdateStockPrice(Stock stock)
        {
            // 随机选择是否更新此股票
            var r = _updateOrNotRandom.NextDouble();
            if (r > .1)
            {
                return false;
            }

            // 以范围百分比的随机因子更新股票价格
            var random = new Random((int)Math.Floor(stock.Price));
            var percentChange = random.NextDouble() * _rangePercent;
            var pos = random.NextDouble() > .51;
            var change = Math.Round(stock.Price * (decimal)percentChange, 2);
            change = pos ? change : -change;

            stock.Price += change;
            return true;
        }
        /// <summary>
        /// 广播股票价格
        /// </summary>
        /// <param name="stock"></param>
        private void BroadcastStockPrice(Stock stock)
        {
            Clients.All.updateStockPrice(stock);
        }
        /// <summary>
        /// 广播市场状态更改
        /// </summary>
        /// <param name="marketState"></param>
        private void BroadcastMarketStateChange(MarketState marketState)
        {
            switch (marketState)
            {
                case MarketState.Closed:
                    Clients.All.marketClosed();
                    break;
                case MarketState.Open:
                    Clients.All.marketOpened();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 广播重置
        /// </summary>
        private void BroadcastMarketReset()
        {
            Clients.All.marketReset();
        }
    }
    /// <summary>
    /// 股票
    /// </summary>
    public class Stock
    {
        private decimal _price;
        /// <summary>
        /// 股票标示
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 股票价格
        /// </summary>
        public decimal Price {
            get { return _price; }
            set
            {
                if (_price == value)
                {
                    return;
                }

                _price = value;

                if (High < 0) High = value;
                else if (_price > High) High = value;
                if (Low < 0) Low = value;
                else if (_price < Low) Low = value;
                if (DayOpen < 0)
                    DayOpen = _price;
            }
        }
        /// <summary>
        /// 开盘价格
        /// </summary>
        public decimal DayOpen { get; private set; } = -1;
        /// <summary>
        /// 股票价格变动
        /// </summary>
        public decimal Change
        {
            get
            {
                return Price - DayOpen;
            }
        }
        /// <summary>
        /// 股票价格变动百分比
        /// </summary>
        public double PercentChange
        {
            get
            {
                return (double)Math.Round(Change / Price, 4);
            }
        }

        /// <summary>
        /// 最高价格
        /// </summary>
        public decimal High{get; private set;} = -1;
        /// <summary>
        /// 最低价格
        /// </summary>
        public decimal Low{get; private set;} = -1;
    }

    /// <summary>
    /// 市场状态
    /// </summary>
    public enum MarketState
    {
        Closed,
        Open
    }
}