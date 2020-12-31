using System;

namespace Example1.Models.Po
{
    public class PoModel
    {
        public string PoNum { get;  set; }

        public string VenId { get;  set; }

        public decimal TotalAmount { get;  set; }

        public string RowVersion { get;  set; }
    }
}