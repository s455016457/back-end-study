
    
//实体类代码自动生成2017-05-05 17:40:13:934

#region Model
namespace DAO.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// InvType
    /// </summary>
    [Serializable]
    [Table("INVTYPE")]
    public partial class InvType
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
        /// TAXRATE
        /// </summary>
        public Decimal TAXRATE { get; set; }

        /// <summary>
        /// INPUTVAT
        /// </summary>
        public String INPUTVAT { get; set; }

        /// <summary>
        /// OUTPUTVAT
        /// </summary>
        public String OUTPUTVAT { get; set; }

        /// <summary>
        /// ISCALTAX
        /// </summary>
        public Int32? ISCALTAX { get; set; }

    }
}
#endregion


#region Mapping
namespace DAO.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    /// <summary>
    /// 实体映射InvType
    /// </summary>
    internal class InvTypeMapping : EntityTypeConfiguration<InvType>{
        public InvTypeMapping(){
            this.ToTable("INVTYPE");
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
            /// TAXRATE
            /// </summary>
            this.Property(t => t.TAXRATE)
                .HasColumnName("TAXRATE");

            /// <summary>
            /// INPUTVAT
            /// </summary>
            this.Property(t => t.INPUTVAT)
                .HasMaxLength(30)
                .HasColumnName("INPUTVAT");

            /// <summary>
            /// OUTPUTVAT
            /// </summary>
            this.Property(t => t.OUTPUTVAT)
                .HasMaxLength(30)
                .HasColumnName("OUTPUTVAT");

            /// <summary>
            /// ISCALTAX
            /// </summary>
            this.Property(t => t.ISCALTAX)
                .HasColumnName("ISCALTAX");
        }
    }
}
#endregion
