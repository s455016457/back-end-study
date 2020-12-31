using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET.MVC.Areas.Restaurant.Models
{
    /// <summary>
    /// 餐厅
    /// </summary>
    public class Restaurant
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 评价
        /// </summary>
        public virtual ICollection<RestaurantReview> Reviews { get; set; }
    }
}