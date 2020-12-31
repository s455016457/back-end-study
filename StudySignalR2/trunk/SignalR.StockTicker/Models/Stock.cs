using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.StockTicker.Models
{
    public class Stock
    {
        decimal _price;
        /// <summary>
        /// 标示
        /// </summary>
        public string Symbol { get; private set; }
        /// <summary>
        /// 开始价格
        /// </summary>
        public decimal OpenDayPrice { get; private set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (_price == value)
                    return;
                _price = value;
                if (OpenDayPrice == 0)
                    OpenDayPrice = value;
            }
        }
        /// <summary>
        /// 变动
        /// </summary>
        public decimal Change => Price - OpenDayPrice;
        /// <summary>
        /// 变浮
        /// </summary>
        public double PercentChange => (double)Math.Round(Change / Price, 4);

        public Stock(string symbol)
        {
            Symbol = symbol;
        }
    }
}