using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{ /// <summary>
  /// 分页类
  /// </summary>
    public class GridPager
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int rows { get; set; }//每页行数
        /// <summary>
        /// 当前页是第几页
        /// </summary>
        public int page { get; set; }//当前页是第几页
        /// <summary>
        /// 排序方式
        /// </summary>
        public string order { get; set; }//排序方式
        /// <summary>
        /// 排序列
        /// </summary>
        public string sort { get; set; }//排序列
        /// <summary>
        /// 总行数
        /// </summary>
        public int totalRows { get; set; }//总行数
        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPages //总页数
        {
            get
            {
                return (int)Math.Ceiling((float)totalRows / (float)rows);
            }
        }

    }

    public class Sort
    {
        /// <summary>
        /// 排序列
        /// </summary>
        public String sort { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public String order { get; set; }
    }
}