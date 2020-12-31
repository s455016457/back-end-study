
    
//实体类代码自动生成2017-05-18 16:54:09:677

#region Model
namespace DAO.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// ItemType
    /// </summary>
        public partial class ItemType
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
        /// PARENTID
        /// </summary>
        public String PARENTID { get; set; }

        /// <summary>
        /// MINLEN
        /// </summary>
        public Int32? MINLEN { get; set; }

        /// <summary>
        /// MAXLEN
        /// </summary>
        public Int32? MAXLEN { get; set; }

        /// <summary>
        /// ATTRIBTYPE
        /// </summary>
        public String ATTRIBTYPE { get; set; }

        /// <summary>
        /// INVACC
        /// </summary>
        public String INVACC { get; set; }

        /// <summary>
        /// OVERQTY
        /// </summary>
        public Decimal? OVERQTY { get; set; }

        /// <summary>
        /// ISFGREF
        /// </summary>
        public Int32? ISFGREF { get; set; }

        /// <summary>
        /// ITEMGRP
        /// </summary>
        public String ITEMGRP { get; set; }

        /// <summary>
        /// ISVIRTUAL
        /// </summary>
        public Int32? ISVIRTUAL { get; set; }

        /// <summary>
        /// LOTMANAGED
        /// </summary>
        public Int32? LOTMANAGED { get; set; }

        /// <summary>
        /// OVERRATE
        /// </summary>
        public Decimal? OVERRATE { get; set; }

        /// <summary>
        /// PREFIX
        /// </summary>
        public String PREFIX { get; set; }

        /// <summary>
        /// MINSCPRATE
        /// </summary>
        public Decimal MINSCPRATE { get; set; }

        /// <summary>
        /// MAXSCPRATE
        /// </summary>
        public Decimal MAXSCPRATE { get; set; }

    }
}
#endregion


#region Mapping
namespace DAO.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    /// <summary>
    /// 实体映射ItemType
    /// </summary>
    internal class ItemTypeMapping : EntityTypeConfiguration<ItemType>{
        public ItemTypeMapping(){
            this.ToTable("ITEMTYPE");
            this.HasKey(t => t.CODE);

            /// <summary>
            /// CODE
            /// </summary>
            this.Property(t => t.CODE)
                .HasMaxLength(10)
                .HasColumnName("CODE");

            /// <summary>
            /// DESCR
            /// </summary>
            this.Property(t => t.DESCR)
                .HasMaxLength(50)
                .HasColumnName("DESCR");

            /// <summary>
            /// PARENTID
            /// </summary>
            this.Property(t => t.PARENTID)
                .HasMaxLength(10)
                .HasColumnName("PARENTID");

            /// <summary>
            /// MINLEN
            /// </summary>
            this.Property(t => t.MINLEN)
                .HasColumnName("MINLEN");

            /// <summary>
            /// MAXLEN
            /// </summary>
            this.Property(t => t.MAXLEN)
                .HasColumnName("MAXLEN");

            /// <summary>
            /// ATTRIBTYPE
            /// </summary>
            this.Property(t => t.ATTRIBTYPE)
                .HasMaxLength(2)
                .HasColumnName("ATTRIBTYPE");

            /// <summary>
            /// INVACC
            /// </summary>
            this.Property(t => t.INVACC)
                .HasMaxLength(30)
                .HasColumnName("INVACC");

            /// <summary>
            /// OVERQTY
            /// </summary>
            this.Property(t => t.OVERQTY)
                .HasColumnName("OVERQTY");

            /// <summary>
            /// ISFGREF
            /// </summary>
            this.Property(t => t.ISFGREF)
                .HasColumnName("ISFGREF");

            /// <summary>
            /// ITEMGRP
            /// </summary>
            this.Property(t => t.ITEMGRP)
                .HasMaxLength(10)
                .HasColumnName("ITEMGRP");

            /// <summary>
            /// ISVIRTUAL
            /// </summary>
            this.Property(t => t.ISVIRTUAL)
                .HasColumnName("ISVIRTUAL");

            /// <summary>
            /// LOTMANAGED
            /// </summary>
            this.Property(t => t.LOTMANAGED)
                .HasColumnName("LOTMANAGED");

            /// <summary>
            /// OVERRATE
            /// </summary>
            this.Property(t => t.OVERRATE)
                .HasColumnName("OVERRATE");

            /// <summary>
            /// PREFIX
            /// </summary>
            this.Property(t => t.PREFIX)
                .HasMaxLength(20)
                .HasColumnName("PREFIX");

            /// <summary>
            /// MINSCPRATE
            /// </summary>
            this.Property(t => t.MINSCPRATE)
                .HasColumnName("MINSCPRATE");

            /// <summary>
            /// MAXSCPRATE
            /// </summary>
            this.Property(t => t.MAXSCPRATE)
                .HasColumnName("MAXSCPRATE");
        }
    }
}
#endregion
