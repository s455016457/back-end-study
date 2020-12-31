using System;
using System.Web.Http;

namespace WebApi.Controllers.Purchase
{
    /// <summary>
    /// 获取订单列表
    /// </summary>
    [RoutePrefix("api/Purchase/Pos")]//定义特性路由前缀
    public class PoController : ApiController
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public PoController() : base()
        {
            LoggerService.LoggerService.ApplicationLogger.Debug("PoController无参数构造函数初始化！");
        }

        
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="PoNum">订单编号</param>
        /// <returns></returns>
        [Route("GetModel")]
        [HttpGet]
        public String GetModel(String PoNum)
        {
            return String.Format("GetModel:{0}",PoNum);
        }

        /// <summary>
        /// 获取订单,Restful风格的WebApi服务
        /// </summary>
        /// <param name="PoNum">订单编号</param>
        /// <returns></returns>
        [Route("{PoNum}/Model")]//Restful风格的WebApi服务
        [HttpGet]
        public String GetModelByPoNum(String PoNum)
        {
            return "GetModelByPoNum";
        }

        /// <summary>
        /// 获取订单明细
        /// </summary>
        /// <param name="PoNum">订单编号</param>
        /// <param name="ItemNum">物料编码</param>
        /// <param name="Lot">批次</param>
        /// <returns></returns>
        [Route("GetDetailModel")]
        [HttpGet]
        public String GetDetailModel(String PoNum, String ItemNum, String Lot)
        {
            return String.Format("GetDetailModel:{0}**{1}**{2}", PoNum, ItemNum, Lot);
        }

        /// <summary>
        /// 获取订单明细列表,Restful风格的WebApi服务
        /// </summary>
        /// <param name="PoNum">订单编号</param>
        /// <returns></returns>
        [Route("{PoNum}/ModelDetails")]//Restful风格的WebApi服务
        [HttpGet]
        public String GetModelDetails(String PoNum)
        {
            return "GetModelDetails";
        }

        /// <summary>
        /// 获取订单明细,Restful风格的WebApi服务
        /// </summary>
        /// <param name="PoNum">订单编号</param>
        /// <param name="ItemNum">物料编码</param>
        /// <param name="Lot">批次</param>
        /// <returns></returns>
        [Route("{PoNum}/{ItemNum}/{Lot}/DetailModel")]//,Restful风格的WebApi服务
        [HttpGet]
        public String GetDetailModel2(String PoNum, String ItemNum, String Lot)
        {
            return String.Format("GetDetailModel2:{0}**{1}**{2}", PoNum, ItemNum, Lot);
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="PoNum">订单编号</param>
        /// <returns></returns>
        [Route("GetModel")]
        [HttpPost]
        public String PostGetModel([FromBody] String PoNum)
        {
            return String.Format("PostGetModel:{0}", PoNum);
        }

        /// <summary>
        /// 查询参数
        /// </summary>
        public class Paramater
        {
            /// <summary>
            /// 开始日期
            /// </summary>
            public String beginDate { get; set; }
            /// <summary>
            /// 结束日期
            /// </summary>
            public String endDate { get; set; }
            /// <summary>
            /// 订单号
            /// </summary>
            public String poNum { get; set; }
            /// <summary>
            /// 采购员
            /// </summary>
            public String buyer { get; set; }
            /// <summary>
            /// 分页
            /// </summary>
            public Models.GridPager pager { get; set; }
        }
    }
}