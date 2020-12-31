//using Domain.Entity.Setting;
//using System.Data.Entity.ModelConfiguration;

//namespace DAO.EF.Model.Setting
//{
//    public class ItemTypeMapping : EntityTypeConfiguration<ItemTypeEntity>
//    {
//        public ItemTypeMapping()
//        {
//            ToTable("ITEMTYPE");
//            HasKey(t => t.Code);

//            /// <summary>
//            /// CODE
//            /// </summary>
//            Property(t => t.Code)
//                .HasMaxLength(10)
//                .HasColumnName("CODE");

//            Property(t => t.ParentID)
//                .HasMaxLength(10)
//                .HasColumnName("PARENTID");

//            #region 其它属性配置
//            /// <summary>
//            /// DESCR
//            /// </summary>
//            Property(t => t.Descr)
//                .HasMaxLength(50)
//                .HasColumnName("DESCR");

//            Property(t => t.AttribType)
//                .HasMaxLength(2)
//                .HasColumnName("ATTRIBTYPE");

//            Property(t => t.InvAcc)
//                .HasMaxLength(30)
//                .HasColumnName("INVACC");

//            Property(t => t.IsFgRefValue)
//                .HasColumnName("ISFGREF");

//            Property(t => t.IsVirtualValue)
//                .HasColumnName("ISVIRTUAL");

//            Property(t => t.ItemGroup)
//                .HasMaxLength(10)
//                .HasColumnName("ITEMGRP");

//            Property(t => t.ItemNumMaxLength)
//                .HasColumnName("MAXLEN");

//            Property(t => t.ItemNumMinLength)
//                .HasColumnName("MINLEN");

//            Property(t => t.LotManaged)
//                .HasColumnName("LOTMANAGED")
//                ;
//            Property(t => t.MaxScpRate)
//                .HasColumnName("MAXSCPRATE")
//                .HasPrecision(19, 5);

//            Property(t => t.MinScpRate)
//                .HasColumnName("MINSCPRATE")
//                .HasPrecision(19, 5);

//            Property(t => t.OverQty)
//                .HasColumnName("OVERQTY")
//                .HasPrecision(19, 5);

//            Property(t => t.OverRate)
//                .HasColumnName("OVERRATE")
//                .HasPrecision(19, 5);

//            Property(t => t.PreFix)
//                .HasColumnName("PREFIX");
//            #endregion
            
//            HasMany(t => t.Childes)
//                .WithOptional(t => t.Parent)
//                .HasForeignKey(t => t.ParentID)
//                .WillCascadeOnDelete(false);
//        }
//    }
//}
