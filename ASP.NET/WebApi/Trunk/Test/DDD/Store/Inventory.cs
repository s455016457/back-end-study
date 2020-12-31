namespace Test.DDD.Store
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Inventory")]
    public partial class Inventory
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string RefNum { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ItemNum { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string LocId { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string Lot { get; set; }

        public decimal Qty { get; set; }

        [StringLength(10)]
        public string Uom { get; set; }
    }
}
