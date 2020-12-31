namespace Test.DDD.Store
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrnDetail")]
    public partial class GrnDetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string GrnNum { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string PoNum { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string ItemNum { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string PoLot { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string GrnLot { get; set; }

        public decimal Qty { get; set; }

        [Required]
        [StringLength(20)]
        public string LocId { get; set; }
    }
}
