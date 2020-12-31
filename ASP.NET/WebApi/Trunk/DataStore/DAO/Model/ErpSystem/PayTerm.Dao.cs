
    
//实体类代码自动生成2017-05-05 17:40:42:064

#region Model
namespace DAO.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// PayTerm
    /// </summary>
    [Serializable]
    [Table("PAYTERM")]
    public partial class PayTerm
    {
        /// <summary>
        /// CODE
        /// </summary>
        public String CODE { get; set; }

        /// <summary>
        /// DESCR
        /// </summary>
        public String DESCR { get; set; }

        /// <summary>
        /// NETDAY
        /// </summary>
        public Int32 NETDAY { get; set; }

        /// <summary>
        /// DISCOUNTDAY
        /// </summary>
        public Int32 DISCOUNTDAY { get; set; }

        /// <summary>
        /// DISCOUNT
        /// </summary>
        public Decimal DISCOUNT { get; set; }

        /// <summary>
        /// AMS
        /// </summary>
        public Int32 AMS { get; set; }

    }
}
#endregion


#region Mapping
namespace DAO.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    /// <summary>
    /// 实体映射PayTerm
    /// </summary>
    internal class PayTermMapping : EntityTypeConfiguration<PayTerm>{
        public PayTermMapping(){
            this.ToTable("PAYTERM");
            this.HasKey(t => t.CODE);

            /// <summary>
            /// CODE
            /// </summary>
            this.Property(t => t.CODE)
                .HasMaxLength(20)
                .HasColumnName("CODE");

            /// <summary>
            /// DESCR
            /// </summary>
            this.Property(t => t.DESCR)
                .HasMaxLength(50)
                .HasColumnName("DESCR");

            /// <summary>
            /// NETDAY
            /// </summary>
            this.Property(t => t.NETDAY)
                .IsRequired()
                .HasColumnName("NETDAY");

            /// <summary>
            /// DISCOUNTDAY
            /// </summary>
            this.Property(t => t.DISCOUNTDAY)
                .IsRequired()
                .HasColumnName("DISCOUNTDAY");

            /// <summary>
            /// DISCOUNT
            /// </summary>
            this.Property(t => t.DISCOUNT)
                .HasColumnName("DISCOUNT");

            /// <summary>
            /// AMS
            /// </summary>
            this.Property(t => t.AMS)
                .IsRequired()
                .HasColumnName("AMS");
        }
    }
}
#endregion
