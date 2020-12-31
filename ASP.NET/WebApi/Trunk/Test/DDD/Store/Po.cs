namespace Test.DDD.Store
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Po")]
    public partial class Po: IRowVersion
    {
        [Key]
        [StringLength(20)]
        public string PoNum { get; set; }

        [Required]
        [StringLength(20)]
        public string VenId { get; set; }

        public decimal? TotalAmount { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
