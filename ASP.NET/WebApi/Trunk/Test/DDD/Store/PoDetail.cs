namespace Test.DDD.Store
{

    public class PoDetailKey
    {
        public string Ponum { get; set; }

        public string ItemNum { get; set; }

        public string Lot { get; set; }
        public PoDetailKey(string Ponum, string ItemNum, string Lot)
        {
            this.Ponum = Ponum;
            this.ItemNum = ItemNum;
            this.Lot = Lot;
        }
    }
    public partial class PoDetail
    {
        public string Ponum { get; set; }
        
        public string ItemNum { get; set; }
        
        public string Lot { get; set; }

        public decimal Qty { get; set; }

        public decimal Uprice { get; set; }

        public decimal Amount { get; set; }

        public virtual PoDetail_TotalGrnQty PoDetail_TotalGrnQty { get; set; }
    }
    
    public class PoDetail_TotalGrnQty
    {
        public string Ponum { get; set; }
        
        public string ItemNum { get; set; }
        
        public string Lot { get; set; }

        public decimal? TotalGrnQty { get; set; }
        
    }
}
