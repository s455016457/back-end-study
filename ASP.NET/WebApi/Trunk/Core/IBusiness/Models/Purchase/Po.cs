using System;
using System.Linq;

namespace IInfrastructure.Models.Purchase
{
    public class Po
    {
        public string PoNum { get; set; }
        public System.DateTime IDATE { get; set; }
        public string POTYPE { get; set; }
        public string REV { get; set; }
        public string WONUM { get; set; }
        public string PROJECT { get; set; }
        public string VENID { get; set; }
        public string CURR { get; set; }
        public decimal RATE { get; set; }
        public decimal TAXRATE { get; set; }
        public string TRADETERM { get; set; }
        public string SHIPTO { get; set; }
        public string SHIPMARK { get; set; }
        public string VIA { get; set; }
        public string REMARK { get; set; }
        public Nullable<decimal> AMOUNT { get; set; }
        public Nullable<int> CLOSED { get; set; }
        public Nullable<System.DateTime> CREATED { get; set; }
        public string CREATEDBY { get; set; }
        public string WFSTATUS { get; set; }
        public string ISPRINTED { get; set; }
        public string OBJTYPE { get; set; }
        public Nullable<System.DateTime> CLOSEDDATE { get; set; }
        public string DEPTID { get; set; }
        public string COMMENTS { get; set; }
        public Nullable<decimal> INSURE { get; set; }
        public Nullable<decimal> HANDLE { get; set; }
        public Nullable<decimal> DELIVERYCHRG { get; set; }
        public Nullable<decimal> MISCHRG { get; set; }
        public string MISCHRGTIT { get; set; }
        public string MISCACC { get; set; }
        public Nullable<decimal> TOTALQTY { get; set; }
        public String BuyerDescr { get; set; }
        public String InvTypeDescr { get; set; }
        public String PayMethodDescr { get; set; }
        public String PayTermDescr { get; set; }
        public String CreatedUserName { get; set; }
        public String UpdatedUserName { get; set; }
    }

    public class PoDetail
    {
        public string PONUM { get; set; }
        public string ITEMNUM { get; set; }
        public string LOT { get; set; }
        public string VENITEMNUM { get; set; }
        public System.DateTime RECVDATE { get; set; }
        public decimal UPRICE { get; set; }
        public Nullable<decimal> EXQTY { get; set; }
        public string EXUOM { get; set; }
        public decimal QTY { get; set; }
        public string UOM { get; set; }
        public Nullable<decimal> AMOUNT { get; set; }
        public string REMARK { get; set; }
        public string PRNUM { get; set; }
        public decimal GRNQTY { get; set; }
        public decimal REJECTQTY { get; set; }
        public int STATUS { get; set; }
        public Nullable<System.DateTime> CREATED { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CLOSEDDATE { get; set; }
        public Nullable<System.DateTime> REPLYRECVDATE { get; set; }
        public Nullable<decimal> EXTUPRICE { get; set; }
        public Nullable<decimal> EXTEXPENSE { get; set; }
        public string SUITENUM { get; set; }
        public string SUITENAME { get; set; }
        public Nullable<decimal> SUITEQTY { get; set; }
    }
}
