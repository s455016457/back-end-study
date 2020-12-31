
    
//实体类代码自动生成2017-05-05 17:41:09:557

#region Model
namespace DAO.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Buyer
    /// </summary>
    [Serializable]
    [Table("BUYER")]
    public partial class Buyer
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
        /// ISDENY
        /// </summary>
        public Int32? ISDENY { get; set; }

        /// <summary>
        /// BUYERTEL
        /// </summary>
        public String BUYERTEL { get; set; }

        /// <summary>
        /// BUYERFAX
        /// </summary>
        public String BUYERFAX { get; set; }

        /// <summary>
        /// SYSUID
        /// </summary>
        public String SYSUID { get; set; }

    }
}
#endregion


#region Mapping
namespace DAO.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    /// <summary>
    /// 实体映射Buyer
    /// </summary>
    internal class BuyerMapping : EntityTypeConfiguration<Buyer>{
        public BuyerMapping(){
            this.ToTable("BUYER");
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
            /// ISDENY
            /// </summary>
            this.Property(t => t.ISDENY)
                .HasColumnName("ISDENY");

            /// <summary>
            /// BUYERTEL
            /// </summary>
            this.Property(t => t.BUYERTEL)
                .HasMaxLength(50)
                .HasColumnName("BUYERTEL");

            /// <summary>
            /// BUYERFAX
            /// </summary>
            this.Property(t => t.BUYERFAX)
                .HasMaxLength(50)
                .HasColumnName("BUYERFAX");

            /// <summary>
            /// SYSUID
            /// </summary>
            this.Property(t => t.SYSUID)
                .HasMaxLength(50)
                .HasColumnName("SYSUID");
        }
    }
}
#endregion
