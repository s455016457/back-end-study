
    
//实体类代码自动生成2017-05-05 17:41:19:940

#region Model
namespace DAO.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Po
    /// </summary>
    [Serializable]
    [Table("PO")]
    public partial class Po
    {
        /// <summary>
        /// PONUM
        /// </summary>
        public String PONUM { get; set; }

        /// <summary>
        /// IDATE
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime IDATE { get; set; }

        /// <summary>
        /// POTYPE
        /// </summary>
        public String POTYPE { get; set; }

        /// <summary>
        /// REV
        /// </summary>
        public String REV { get; set; }

        /// <summary>
        /// WONUM
        /// </summary>
        public String WONUM { get; set; }

        /// <summary>
        /// PROJECT
        /// </summary>
        public String PROJECT { get; set; }

        /// <summary>
        /// VENID
        /// </summary>
        public String VENID { get; set; }

        /// <summary>
        /// CURR
        /// </summary>
        public String CURR { get; set; }

        /// <summary>
        /// RATE
        /// </summary>
        public Decimal RATE { get; set; }

        /// <summary>
        /// INVTYPE
        /// </summary>
        public String INVTYPE { get; set; }

        /// <summary>
        /// TAXRATE
        /// </summary>
        public Decimal TAXRATE { get; set; }

        /// <summary>
        /// BUYER
        /// </summary>
        public String BUYER { get; set; }

        /// <summary>
        /// PAYTERM
        /// </summary>
        public String PAYTERM { get; set; }

        /// <summary>
        /// PAYMETHOD
        /// </summary>
        public String PAYMETHOD { get; set; }

        /// <summary>
        /// TRADETERM
        /// </summary>
        public String TRADETERM { get; set; }

        /// <summary>
        /// SHIPTO
        /// </summary>
        public String SHIPTO { get; set; }

        /// <summary>
        /// SHIPMARK
        /// </summary>
        public String SHIPMARK { get; set; }

        /// <summary>
        /// VIA
        /// </summary>
        public String VIA { get; set; }

        /// <summary>
        /// REMARK
        /// </summary>
        public String REMARK { get; set; }

        /// <summary>
        /// AMOUNT
        /// </summary>
        public Decimal? AMOUNT { get; set; }

        /// <summary>
        /// CLOSED
        /// </summary>
        public Int32? CLOSED { get; set; }

        /// <summary>
        /// CREATED
        /// </summary>
        public DateTime? CREATED { get; set; }

        /// <summary>
        /// CREATEDBY
        /// </summary>
        public String CREATEDBY { get; set; }

        /// <summary>
        /// WFSTATUS
        /// </summary>
        public String WFSTATUS { get; set; }

        /// <summary>
        /// ISPRINTED
        /// </summary>
        public String ISPRINTED { get; set; }

        /// <summary>
        /// OBJTYPE
        /// </summary>
        public String OBJTYPE { get; set; }

        /// <summary>
        /// CLOSEDDATE
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? CLOSEDDATE { get; set; }

        /// <summary>
        /// DEPTID
        /// </summary>
        public String DEPTID { get; set; }

        /// <summary>
        /// COMMENTS
        /// </summary>
        public String COMMENTS { get; set; }

        /// <summary>
        /// INSURE
        /// </summary>
        public Decimal? INSURE { get; set; }

        /// <summary>
        /// HANDLE
        /// </summary>
        public Decimal? HANDLE { get; set; }

        /// <summary>
        /// DELIVERYCHRG
        /// </summary>
        public Decimal? DELIVERYCHRG { get; set; }

        /// <summary>
        /// MISCHRG
        /// </summary>
        public Decimal? MISCHRG { get; set; }

        /// <summary>
        /// MISCHRGTIT
        /// </summary>
        public String MISCHRGTIT { get; set; }

        /// <summary>
        /// MISCACC
        /// </summary>
        public String MISCACC { get; set; }

        /// <summary>
        /// TOTALQTY
        /// </summary>
        public Decimal? TOTALQTY { get; set; }

        /// <summary>
        /// EXECSTATE
        /// </summary>
        public Int32 EXECSTATE { get; set; }

        /// <summary>
        /// EXECSTATEUPDATEDSTAMP
        /// </summary>
        public DateTime? EXECSTATEUPDATEDSTAMP { get; set; }

    }


    /// <summary>
    /// PoDetail
    /// </summary>
    [Serializable]
    [Table("PODTL")]
    public class PoDetail
    {
        /// <summary>
        /// PONUM
        /// </summary>
        public String PONUM { get; set; }

        /// <summary>
        /// ITEMNUM
        /// </summary>
        public String ITEMNUM { get; set; }

        /// <summary>
        /// LOT
        /// </summary>
        public String LOT { get; set; }

        /// <summary>
        /// VENITEMNUM
        /// </summary>
        public String VENITEMNUM { get; set; }

        /// <summary>
        /// RECVDATE
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime RECVDATE { get; set; }

        /// <summary>
        /// UPRICE
        /// </summary>
        public Decimal UPRICE { get; set; }

        /// <summary>
        /// EXQTY
        /// </summary>
        public Decimal? EXQTY { get; set; }

        /// <summary>
        /// EXUOM
        /// </summary>
        public String EXUOM { get; set; }

        /// <summary>
        /// QTY
        /// </summary>
        public Decimal QTY { get; set; }

        /// <summary>
        /// UOM
        /// </summary>
        public String UOM { get; set; }

        /// <summary>
        /// AMOUNT
        /// </summary>
        public Decimal? AMOUNT { get; set; }

        /// <summary>
        /// REMARK
        /// </summary>
        public String REMARK { get; set; }

        /// <summary>
        /// PRNUM
        /// </summary>
        public String PRNUM { get; set; }

        /// <summary>
        /// GRNQTY
        /// </summary>
        public Decimal GRNQTY { get; set; }

        /// <summary>
        /// REJECTQTY
        /// </summary>
        public Decimal REJECTQTY { get; set; }

        /// <summary>
        /// STATUS
        /// </summary>
        public Int32 STATUS { get; set; }

        /// <summary>
        /// CREATED
        /// </summary>
        public DateTime? CREATED { get; set; }

        /// <summary>
        /// CREATEDBY
        /// </summary>
        public String CREATEDBY { get; set; }

        /// <summary>
        /// CLOSEDDATE
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? CLOSEDDATE { get; set; }

        /// <summary>
        /// REPLYRECVDATE
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        public DateTime? REPLYRECVDATE { get; set; }

        /// <summary>
        /// EXTUPRICE
        /// </summary>
        public Decimal? EXTUPRICE { get; set; }

        /// <summary>
        /// EXTEXPENSE
        /// </summary>
        public Decimal? EXTEXPENSE { get; set; }

        /// <summary>
        /// SUITENUM
        /// </summary>
        public String SUITENUM { get; set; }

        /// <summary>
        /// SUITENAME
        /// </summary>
        public String SUITENAME { get; set; }

        /// <summary>
        /// SUITEQTY
        /// </summary>
        public Decimal? SUITEQTY { get; set; }

    }
}
#endregion


#region Mapping
namespace DAO.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    /// <summary>
    /// 实体映射Po
    /// </summary>
    internal class PoMapping : EntityTypeConfiguration<Po>{
        public PoMapping(){
            this.ToTable("PO");
            this.HasKey(t => t.PONUM);

            /// <summary>
            /// PONUM
            /// </summary>
            this.Property(t => t.PONUM)
                .HasMaxLength(20)
                .HasColumnName("PONUM");

            /// <summary>
            /// IDATE
            /// </summary>
            this.Property(t => t.IDATE)
                .HasColumnName("IDATE");

            /// <summary>
            /// POTYPE
            /// </summary>
            this.Property(t => t.POTYPE)
                .HasMaxLength(10)
                .HasColumnName("POTYPE");

            /// <summary>
            /// REV
            /// </summary>
            this.Property(t => t.REV)
                .HasMaxLength(10)
                .HasColumnName("REV");

            /// <summary>
            /// WONUM
            /// </summary>
            this.Property(t => t.WONUM)
                .HasMaxLength(20)
                .HasColumnName("WONUM");

            /// <summary>
            /// PROJECT
            /// </summary>
            this.Property(t => t.PROJECT)
                .HasMaxLength(20)
                .HasColumnName("PROJECT");

            /// <summary>
            /// VENID
            /// </summary>
            this.Property(t => t.VENID)
                .HasMaxLength(20)
                .HasColumnName("VENID");

            /// <summary>
            /// CURR
            /// </summary>
            this.Property(t => t.CURR)
                .HasMaxLength(10)
                .HasColumnName("CURR");

            /// <summary>
            /// RATE
            /// </summary>
            this.Property(t => t.RATE)
                .HasColumnName("RATE");

            /// <summary>
            /// INVTYPE
            /// </summary>
            this.Property(t => t.INVTYPE)
                .HasMaxLength(20)
                .HasColumnName("INVTYPE");

            /// <summary>
            /// TAXRATE
            /// </summary>
            this.Property(t => t.TAXRATE)
                .HasColumnName("TAXRATE");

            /// <summary>
            /// BUYER
            /// </summary>
            this.Property(t => t.BUYER)
                .HasMaxLength(20)
                .HasColumnName("BUYER");

            /// <summary>
            /// PAYTERM
            /// </summary>
            this.Property(t => t.PAYTERM)
                .HasMaxLength(20)
                .HasColumnName("PAYTERM");

            /// <summary>
            /// PAYMETHOD
            /// </summary>
            this.Property(t => t.PAYMETHOD)
                .HasMaxLength(20)
                .HasColumnName("PAYMETHOD");

            /// <summary>
            /// TRADETERM
            /// </summary>
            this.Property(t => t.TRADETERM)
                .HasMaxLength(20)
                .HasColumnName("TRADETERM");

            /// <summary>
            /// SHIPTO
            /// </summary>
            this.Property(t => t.SHIPTO)
                .HasMaxLength(200)
                .HasColumnName("SHIPTO");

            /// <summary>
            /// SHIPMARK
            /// </summary>
            this.Property(t => t.SHIPMARK)
                .HasMaxLength(200)
                .HasColumnName("SHIPMARK");

            /// <summary>
            /// VIA
            /// </summary>
            this.Property(t => t.VIA)
                .HasMaxLength(20)
                .HasColumnName("VIA");

            /// <summary>
            /// REMARK
            /// </summary>
            this.Property(t => t.REMARK)
                .HasMaxLength(500)
                .HasColumnName("REMARK");

            /// <summary>
            /// AMOUNT
            /// </summary>
            this.Property(t => t.AMOUNT)
                .HasColumnName("AMOUNT");

            /// <summary>
            /// CLOSED
            /// </summary>
            this.Property(t => t.CLOSED)
                .HasColumnName("CLOSED");

            /// <summary>
            /// CREATED
            /// </summary>
            this.Property(t => t.CREATED)
                .HasColumnName("CREATED");

            /// <summary>
            /// CREATEDBY
            /// </summary>
            this.Property(t => t.CREATEDBY)
                .HasMaxLength(20)
                .HasColumnName("CREATEDBY");

            /// <summary>
            /// WFSTATUS
            /// </summary>
            this.Property(t => t.WFSTATUS)
                .HasColumnName("WFSTATUS");

            /// <summary>
            /// ISPRINTED
            /// </summary>
            this.Property(t => t.ISPRINTED)
                .HasColumnName("ISPRINTED");

            /// <summary>
            /// OBJTYPE
            /// </summary>
            this.Property(t => t.OBJTYPE)
                .HasMaxLength(50)
                .HasColumnName("OBJTYPE");

            /// <summary>
            /// CLOSEDDATE
            /// </summary>
            this.Property(t => t.CLOSEDDATE)
                .HasColumnName("CLOSEDDATE");

            /// <summary>
            /// DEPTID
            /// </summary>
            this.Property(t => t.DEPTID)
                .HasMaxLength(30)
                .HasColumnName("DEPTID");

            /// <summary>
            /// COMMENTS
            /// </summary>
            this.Property(t => t.COMMENTS)
                .HasMaxLength(1000)
                .HasColumnName("COMMENTS");

            /// <summary>
            /// INSURE
            /// </summary>
            this.Property(t => t.INSURE)
                .HasColumnName("INSURE");

            /// <summary>
            /// HANDLE
            /// </summary>
            this.Property(t => t.HANDLE)
                .HasColumnName("HANDLE");

            /// <summary>
            /// DELIVERYCHRG
            /// </summary>
            this.Property(t => t.DELIVERYCHRG)
                .HasColumnName("DELIVERYCHRG");

            /// <summary>
            /// MISCHRG
            /// </summary>
            this.Property(t => t.MISCHRG)
                .HasColumnName("MISCHRG");

            /// <summary>
            /// MISCHRGTIT
            /// </summary>
            this.Property(t => t.MISCHRGTIT)
                .HasMaxLength(200)
                .HasColumnName("MISCHRGTIT");

            /// <summary>
            /// MISCACC
            /// </summary>
            this.Property(t => t.MISCACC)
                .HasMaxLength(20)
                .HasColumnName("MISCACC");

            /// <summary>
            /// TOTALQTY
            /// </summary>
            this.Property(t => t.TOTALQTY)
                .HasColumnName("TOTALQTY");

            /// <summary>
            /// EXECSTATE
            /// </summary>
            this.Property(t => t.EXECSTATE)
                .IsRequired()
                .HasColumnName("EXECSTATE");

            /// <summary>
            /// EXECSTATEUPDATEDSTAMP
            /// </summary>
            this.Property(t => t.EXECSTATEUPDATEDSTAMP)
                .HasColumnName("EXECSTATEUPDATEDSTAMP");
        }
    }


    /// <summary>
    /// 实体映射PoDetail
    /// </summary>
    internal class PoDetailMapping : EntityTypeConfiguration<PoDetail>{
        public PoDetailMapping(){
            this.ToTable("PODTL");
            this.HasKey(t => t.PONUM);
            this.HasKey(t => t.ITEMNUM);
            this.HasKey(t => t.LOT);

            /// <summary>
            /// PONUM
            /// </summary>
            this.Property(t => t.PONUM)
                .HasMaxLength(20)
                .HasColumnName("PONUM");

            /// <summary>
            /// ITEMNUM
            /// </summary>
            this.Property(t => t.ITEMNUM)
                .HasMaxLength(50)
                .HasColumnName("ITEMNUM");

            /// <summary>
            /// LOT
            /// </summary>
            this.Property(t => t.LOT)
                .HasMaxLength(50)
                .HasColumnName("LOT");

            /// <summary>
            /// VENITEMNUM
            /// </summary>
            this.Property(t => t.VENITEMNUM)
                .HasMaxLength(50)
                .HasColumnName("VENITEMNUM");

            /// <summary>
            /// RECVDATE
            /// </summary>
            this.Property(t => t.RECVDATE)
                .HasColumnName("RECVDATE");

            /// <summary>
            /// UPRICE
            /// </summary>
            this.Property(t => t.UPRICE)
                .HasColumnName("UPRICE");

            /// <summary>
            /// EXQTY
            /// </summary>
            this.Property(t => t.EXQTY)
                .HasColumnName("EXQTY");

            /// <summary>
            /// EXUOM
            /// </summary>
            this.Property(t => t.EXUOM)
                .HasMaxLength(10)
                .HasColumnName("EXUOM");

            /// <summary>
            /// QTY
            /// </summary>
            this.Property(t => t.QTY)
                .HasColumnName("QTY");

            /// <summary>
            /// UOM
            /// </summary>
            this.Property(t => t.UOM)
                .HasMaxLength(10)
                .HasColumnName("UOM");

            /// <summary>
            /// AMOUNT
            /// </summary>
            this.Property(t => t.AMOUNT)
                .HasColumnName("AMOUNT");

            /// <summary>
            /// REMARK
            /// </summary>
            this.Property(t => t.REMARK)
                .HasMaxLength(500)
                .HasColumnName("REMARK");

            /// <summary>
            /// PRNUM
            /// </summary>
            this.Property(t => t.PRNUM)
                .HasMaxLength(20)
                .HasColumnName("PRNUM");

            /// <summary>
            /// GRNQTY
            /// </summary>
            this.Property(t => t.GRNQTY)
                .HasColumnName("GRNQTY");

            /// <summary>
            /// REJECTQTY
            /// </summary>
            this.Property(t => t.REJECTQTY)
                .HasColumnName("REJECTQTY");

            /// <summary>
            /// STATUS
            /// </summary>
            this.Property(t => t.STATUS)
                .IsRequired()
                .HasColumnName("STATUS");

            /// <summary>
            /// CREATED
            /// </summary>
            this.Property(t => t.CREATED)
                .HasColumnName("CREATED");

            /// <summary>
            /// CREATEDBY
            /// </summary>
            this.Property(t => t.CREATEDBY)
                .HasMaxLength(20)
                .HasColumnName("CREATEDBY");

            /// <summary>
            /// CLOSEDDATE
            /// </summary>
            this.Property(t => t.CLOSEDDATE)
                .HasColumnName("CLOSEDDATE");

            /// <summary>
            /// REPLYRECVDATE
            /// </summary>
            this.Property(t => t.REPLYRECVDATE)
                .HasColumnName("REPLYRECVDATE");

            /// <summary>
            /// EXTUPRICE
            /// </summary>
            this.Property(t => t.EXTUPRICE)
                .HasColumnName("EXTUPRICE");

            /// <summary>
            /// EXTEXPENSE
            /// </summary>
            this.Property(t => t.EXTEXPENSE)
                .HasColumnName("EXTEXPENSE");

            /// <summary>
            /// SUITENUM
            /// </summary>
            this.Property(t => t.SUITENUM)
                .HasMaxLength(50)
                .HasColumnName("SUITENUM");

            /// <summary>
            /// SUITENAME
            /// </summary>
            this.Property(t => t.SUITENAME)
                .HasMaxLength(200)
                .HasColumnName("SUITENAME");

            /// <summary>
            /// SUITEQTY
            /// </summary>
            this.Property(t => t.SUITEQTY)
                .HasColumnName("SUITEQTY");
        }
    }
}
#endregion
