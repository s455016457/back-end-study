
    
//实体类代码自动生成2017-05-22 15:15:09:994

#region Model
namespace DAO.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// GradeData
    /// </summary>
        public partial class GradeData
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// IMPDATE
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime IMPDATE { get; set; }

        /// <summary>
        /// REMARK
        /// </summary>
        public String REMARK { get; set; }

        /// <summary>
        /// LABELID
        /// </summary>
        public String LABELID { get; set; }

        /// <summary>
        /// VFAID
        /// </summary>
        public String VFAID { get; set; }

        /// <summary>
        /// IVAID
        /// </summary>
        public String IVAID { get; set; }

        /// <summary>
        /// WDAID
        /// </summary>
        public String WDAID { get; set; }

        /// <summary>
        /// VFAMIN
        /// </summary>
        public Decimal? VFAMIN { get; set; }

        /// <summary>
        /// VFAMAX
        /// </summary>
        public Decimal? VFAMAX { get; set; }

        /// <summary>
        /// IVAMIN
        /// </summary>
        public Decimal? IVAMIN { get; set; }

        /// <summary>
        /// IVAMAX
        /// </summary>
        public Decimal? IVAMAX { get; set; }

        /// <summary>
        /// WDAMIN
        /// </summary>
        public Decimal? WDAMIN { get; set; }

        /// <summary>
        /// WDAMAX
        /// </summary>
        public Decimal? WDAMAX { get; set; }

        /// <summary>
        /// VFBID
        /// </summary>
        public String VFBID { get; set; }

        /// <summary>
        /// IVBID
        /// </summary>
        public String IVBID { get; set; }

        /// <summary>
        /// WDBID
        /// </summary>
        public String WDBID { get; set; }

        /// <summary>
        /// VFBMIN
        /// </summary>
        public Decimal? VFBMIN { get; set; }

        /// <summary>
        /// VFBMAX
        /// </summary>
        public Decimal? VFBMAX { get; set; }

        /// <summary>
        /// IVBMIN
        /// </summary>
        public Decimal? IVBMIN { get; set; }

        /// <summary>
        /// IVBMAX
        /// </summary>
        public Decimal? IVBMAX { get; set; }

        /// <summary>
        /// WDBMIN
        /// </summary>
        public Decimal? WDBMIN { get; set; }

        /// <summary>
        /// WDBMAX
        /// </summary>
        public Decimal? WDBMAX { get; set; }

        /// <summary>
        /// VFCID
        /// </summary>
        public String VFCID { get; set; }

        /// <summary>
        /// IVCID
        /// </summary>
        public String IVCID { get; set; }

        /// <summary>
        /// WDCID
        /// </summary>
        public String WDCID { get; set; }

        /// <summary>
        /// VFCMIN
        /// </summary>
        public Decimal? VFCMIN { get; set; }

        /// <summary>
        /// VFCMAX
        /// </summary>
        public Decimal? VFCMAX { get; set; }

        /// <summary>
        /// IVCMIN
        /// </summary>
        public Decimal? IVCMIN { get; set; }

        /// <summary>
        /// IVCMAX
        /// </summary>
        public Decimal? IVCMAX { get; set; }

        /// <summary>
        /// WDCMIN
        /// </summary>
        public Decimal? WDCMIN { get; set; }

        /// <summary>
        /// WDCMAX
        /// </summary>
        public Decimal? WDCMAX { get; set; }

        /// <summary>
        /// POS
        /// </summary>
        public String POS { get; set; }

        /// <summary>
        /// POS1X
        /// </summary>
        public Decimal? POS1X { get; set; }

        /// <summary>
        /// POS1Y
        /// </summary>
        public Decimal? POS1Y { get; set; }

        /// <summary>
        /// POS2X
        /// </summary>
        public Decimal? POS2X { get; set; }

        /// <summary>
        /// POS2Y
        /// </summary>
        public Decimal? POS2Y { get; set; }

        /// <summary>
        /// POS3X
        /// </summary>
        public Decimal? POS3X { get; set; }

        /// <summary>
        /// POS3Y
        /// </summary>
        public Decimal? POS3Y { get; set; }

        /// <summary>
        /// POS4X
        /// </summary>
        public Decimal? POS4X { get; set; }

        /// <summary>
        /// POS4Y
        /// </summary>
        public Decimal? POS4Y { get; set; }

        /// <summary>
        /// CTID
        /// </summary>
        public String CTID { get; set; }

        /// <summary>
        /// CTMIN
        /// </summary>
        public Decimal? CTMIN { get; set; }

        /// <summary>
        /// CTMAX
        /// </summary>
        public Decimal? CTMAX { get; set; }

        /// <summary>
        /// FLUXID
        /// </summary>
        public String FLUXID { get; set; }

        /// <summary>
        /// FLUXMIN
        /// </summary>
        public Decimal? FLUXMIN { get; set; }

        /// <summary>
        /// FLUXMAX
        /// </summary>
        public Decimal? FLUXMAX { get; set; }

        /// <summary>
        /// POWERSUPPLY
        /// </summary>
        public String POWERSUPPLY { get; set; }

        /// <summary>
        /// CHIPSIZE
        /// </summary>
        public String CHIPSIZE { get; set; }

        /// <summary>
        /// CHIPSUPPLIER
        /// </summary>
        public String CHIPSUPPLIER { get; set; }

        /// <summary>
        /// SHELLCOLOR
        /// </summary>
        public String SHELLCOLOR { get; set; }

        /// <summary>
        /// CABLETYPE
        /// </summary>
        public String CABLETYPE { get; set; }

        /// <summary>
        /// LABELDESCR
        /// </summary>
        public String LABELDESCR { get; set; }

    }
}
#endregion


#region Mapping
namespace DAO.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    /// <summary>
    /// 实体映射GradeData
    /// </summary>
    internal class GradeDataMapping : EntityTypeConfiguration<GradeData>{
        public GradeDataMapping(){
            this.ToTable("GRADEDATA");
            this.HasKey(t => t.LABELID);

            /// <summary>
            /// ID
            /// </summary>
            this.Property(t => t.ID)
                .IsRequired()
                .HasColumnName("ID");

            /// <summary>
            /// IMPDATE
            /// </summary>
            this.Property(t => t.IMPDATE)
                .HasColumnName("IMPDATE");

            /// <summary>
            /// REMARK
            /// </summary>
            this.Property(t => t.REMARK)
                .HasMaxLength(100)
                .HasColumnName("REMARK");

            /// <summary>
            /// LABELID
            /// </summary>
            this.Property(t => t.LABELID)
                .HasMaxLength(500)
                .HasColumnName("LABELID");

            /// <summary>
            /// VFAID
            /// </summary>
            this.Property(t => t.VFAID)
                .HasMaxLength(10)
                .HasColumnName("VFAID");

            /// <summary>
            /// IVAID
            /// </summary>
            this.Property(t => t.IVAID)
                .HasMaxLength(10)
                .HasColumnName("IVAID");

            /// <summary>
            /// WDAID
            /// </summary>
            this.Property(t => t.WDAID)
                .HasMaxLength(10)
                .HasColumnName("WDAID");

            /// <summary>
            /// VFAMIN
            /// </summary>
            this.Property(t => t.VFAMIN)
                .HasColumnName("VFAMIN");

            /// <summary>
            /// VFAMAX
            /// </summary>
            this.Property(t => t.VFAMAX)
                .HasColumnName("VFAMAX");

            /// <summary>
            /// IVAMIN
            /// </summary>
            this.Property(t => t.IVAMIN)
                .HasColumnName("IVAMIN");

            /// <summary>
            /// IVAMAX
            /// </summary>
            this.Property(t => t.IVAMAX)
                .HasColumnName("IVAMAX");

            /// <summary>
            /// WDAMIN
            /// </summary>
            this.Property(t => t.WDAMIN)
                .HasColumnName("WDAMIN");

            /// <summary>
            /// WDAMAX
            /// </summary>
            this.Property(t => t.WDAMAX)
                .HasColumnName("WDAMAX");

            /// <summary>
            /// VFBID
            /// </summary>
            this.Property(t => t.VFBID)
                .HasMaxLength(10)
                .HasColumnName("VFBID");

            /// <summary>
            /// IVBID
            /// </summary>
            this.Property(t => t.IVBID)
                .HasMaxLength(10)
                .HasColumnName("IVBID");

            /// <summary>
            /// WDBID
            /// </summary>
            this.Property(t => t.WDBID)
                .HasMaxLength(10)
                .HasColumnName("WDBID");

            /// <summary>
            /// VFBMIN
            /// </summary>
            this.Property(t => t.VFBMIN)
                .HasColumnName("VFBMIN");

            /// <summary>
            /// VFBMAX
            /// </summary>
            this.Property(t => t.VFBMAX)
                .HasColumnName("VFBMAX");

            /// <summary>
            /// IVBMIN
            /// </summary>
            this.Property(t => t.IVBMIN)
                .HasColumnName("IVBMIN");

            /// <summary>
            /// IVBMAX
            /// </summary>
            this.Property(t => t.IVBMAX)
                .HasColumnName("IVBMAX");

            /// <summary>
            /// WDBMIN
            /// </summary>
            this.Property(t => t.WDBMIN)
                .HasColumnName("WDBMIN");

            /// <summary>
            /// WDBMAX
            /// </summary>
            this.Property(t => t.WDBMAX)
                .HasColumnName("WDBMAX");

            /// <summary>
            /// VFCID
            /// </summary>
            this.Property(t => t.VFCID)
                .HasMaxLength(10)
                .HasColumnName("VFCID");

            /// <summary>
            /// IVCID
            /// </summary>
            this.Property(t => t.IVCID)
                .HasMaxLength(10)
                .HasColumnName("IVCID");

            /// <summary>
            /// WDCID
            /// </summary>
            this.Property(t => t.WDCID)
                .HasMaxLength(10)
                .HasColumnName("WDCID");

            /// <summary>
            /// VFCMIN
            /// </summary>
            this.Property(t => t.VFCMIN)
                .HasColumnName("VFCMIN");

            /// <summary>
            /// VFCMAX
            /// </summary>
            this.Property(t => t.VFCMAX)
                .HasColumnName("VFCMAX");

            /// <summary>
            /// IVCMIN
            /// </summary>
            this.Property(t => t.IVCMIN)
                .HasColumnName("IVCMIN");

            /// <summary>
            /// IVCMAX
            /// </summary>
            this.Property(t => t.IVCMAX)
                .HasColumnName("IVCMAX");

            /// <summary>
            /// WDCMIN
            /// </summary>
            this.Property(t => t.WDCMIN)
                .HasColumnName("WDCMIN");

            /// <summary>
            /// WDCMAX
            /// </summary>
            this.Property(t => t.WDCMAX)
                .HasColumnName("WDCMAX");

            /// <summary>
            /// POS
            /// </summary>
            this.Property(t => t.POS)
                .HasMaxLength(10)
                .HasColumnName("POS");

            /// <summary>
            /// POS1X
            /// </summary>
            this.Property(t => t.POS1X)
                .HasColumnName("POS1X");

            /// <summary>
            /// POS1Y
            /// </summary>
            this.Property(t => t.POS1Y)
                .HasColumnName("POS1Y");

            /// <summary>
            /// POS2X
            /// </summary>
            this.Property(t => t.POS2X)
                .HasColumnName("POS2X");

            /// <summary>
            /// POS2Y
            /// </summary>
            this.Property(t => t.POS2Y)
                .HasColumnName("POS2Y");

            /// <summary>
            /// POS3X
            /// </summary>
            this.Property(t => t.POS3X)
                .HasColumnName("POS3X");

            /// <summary>
            /// POS3Y
            /// </summary>
            this.Property(t => t.POS3Y)
                .HasColumnName("POS3Y");

            /// <summary>
            /// POS4X
            /// </summary>
            this.Property(t => t.POS4X)
                .HasColumnName("POS4X");

            /// <summary>
            /// POS4Y
            /// </summary>
            this.Property(t => t.POS4Y)
                .HasColumnName("POS4Y");

            /// <summary>
            /// CTID
            /// </summary>
            this.Property(t => t.CTID)
                .HasMaxLength(30)
                .HasColumnName("CTID");

            /// <summary>
            /// CTMIN
            /// </summary>
            this.Property(t => t.CTMIN)
                .HasColumnName("CTMIN");

            /// <summary>
            /// CTMAX
            /// </summary>
            this.Property(t => t.CTMAX)
                .HasColumnName("CTMAX");

            /// <summary>
            /// FLUXID
            /// </summary>
            this.Property(t => t.FLUXID)
                .HasMaxLength(30)
                .HasColumnName("FLUXID");

            /// <summary>
            /// FLUXMIN
            /// </summary>
            this.Property(t => t.FLUXMIN)
                .HasColumnName("FLUXMIN");

            /// <summary>
            /// FLUXMAX
            /// </summary>
            this.Property(t => t.FLUXMAX)
                .HasColumnName("FLUXMAX");

            /// <summary>
            /// POWERSUPPLY
            /// </summary>
            this.Property(t => t.POWERSUPPLY)
                .HasMaxLength(100)
                .HasColumnName("POWERSUPPLY");

            /// <summary>
            /// CHIPSIZE
            /// </summary>
            this.Property(t => t.CHIPSIZE)
                .HasMaxLength(100)
                .HasColumnName("CHIPSIZE");

            /// <summary>
            /// CHIPSUPPLIER
            /// </summary>
            this.Property(t => t.CHIPSUPPLIER)
                .HasMaxLength(100)
                .HasColumnName("CHIPSUPPLIER");

            /// <summary>
            /// SHELLCOLOR
            /// </summary>
            this.Property(t => t.SHELLCOLOR)
                .HasMaxLength(100)
                .HasColumnName("SHELLCOLOR");

            /// <summary>
            /// CABLETYPE
            /// </summary>
            this.Property(t => t.CABLETYPE)
                .HasMaxLength(100)
                .HasColumnName("CABLETYPE");

            /// <summary>
            /// LABELDESCR
            /// </summary>
            this.Property(t => t.LABELDESCR)
                .HasMaxLength(100)
                .HasColumnName("LABELDESCR");
        }
    }
}
#endregion
