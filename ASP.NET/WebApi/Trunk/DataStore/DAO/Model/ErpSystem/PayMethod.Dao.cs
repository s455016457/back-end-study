
    
//实体类代码自动生成2017-05-05 17:40:29:511

#region Model
namespace DAO.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// PayMethod
    /// </summary>
    [Serializable]
    [Table("PAYMETHOD")]
    public partial class PayMethod
    {
        /// <summary>
        /// CODE
        /// </summary>
        public String CODE { get; set; }

        /// <summary>
        /// DESCR
        /// </summary>
        public String DESCR { get; set; }

    }
}
#endregion


#region Mapping
namespace DAO.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    /// <summary>
    /// 实体映射PayMethod
    /// </summary>
    internal class PayMethodMapping : EntityTypeConfiguration<PayMethod>{
        public PayMethodMapping(){
            this.ToTable("PAYMETHOD");
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
        }
    }
}
#endregion
