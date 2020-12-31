namespace Test.DDD.Store
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.ModelConfiguration;

    public partial class DDDTestContext : DbContext
    {
        public DDDTestContext()
            : base("name=DDDTest")
        {
        }

        public virtual DbSet<GrnDetail> GrnDetail { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Po> Po { get; set; }
        public virtual DbSet<PoDetail> PoDetail { get; set; }

        //public virtual DbSet<PoDetail_TotalGrnQty> PoDetail_TotalGrnQty { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PoDetailConfiguration());
            modelBuilder.Configurations.Add(new PoDetail_TotalGrnQtyConfiguration());

            modelBuilder.Entity<GrnDetail>()
                .Property(e => e.GrnNum)
                .IsUnicode(false);

            modelBuilder.Entity<GrnDetail>()
                .Property(e => e.PoNum)
                .IsUnicode(false);

            modelBuilder.Entity<GrnDetail>()
                .Property(e => e.ItemNum)
                .IsUnicode(false);

            modelBuilder.Entity<GrnDetail>()
                .Property(e => e.PoLot)
                .IsUnicode(false);

            modelBuilder.Entity<GrnDetail>()
                .Property(e => e.GrnLot)
                .IsUnicode(false);

            modelBuilder.Entity<GrnDetail>()
                .Property(e => e.Qty)
                .HasPrecision(18, 5);

            modelBuilder.Entity<GrnDetail>()
                .Property(e => e.LocId)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.RefNum)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.ItemNum)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.LocId)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.Lot)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.Qty)
                .HasPrecision(18, 5);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.Uom)
                .IsUnicode(false);

            modelBuilder.Entity<Po>()
                .Property(e => e.PoNum)
                .IsUnicode(false);

            modelBuilder.Entity<Po>()
                .Property(e => e.VenId)
                .IsUnicode(false);

            modelBuilder.Entity<Po>()
                .Property(e => e.TotalAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Po>()
                .Property(e => e.RowVersion)
                .HasMaxLength(50)
                .IsRowVersion();

            /**
            modelBuilder.Entity<PoDetail>()
                .ToTable("PoDetail")
                .HasKey(e => new {  e.Ponum, e.ItemNum,e.Lot });

            modelBuilder.Entity<PoDetail>()
                .Property(e => e.Ponum)
                .IsUnicode(false);

            modelBuilder.Entity<PoDetail>()
                .Property(e => e.ItemNum)
                .IsUnicode(false);

            modelBuilder.Entity<PoDetail>()
                .Property(e => e.Lot)
                .IsUnicode(false);

            modelBuilder.Entity<PoDetail>()
                .Property(e => e.Qty)
                .HasPrecision(18, 5);

            modelBuilder.Entity<PoDetail>()
                .Property(e => e.Uprice)
                .HasPrecision(18, 5);

            modelBuilder.Entity<PoDetail>()
                .Property(e => e.Amount)
                .HasPrecision(18, 5);

            modelBuilder.Entity<PoDetail>()
                .HasRequired(e => e.PoDetail_TotalGrnQty)
                .WithRequiredPrincipal();

            **/

            /**
            modelBuilder.Entity<PoDetail_TotalGrnQty>()
                .ToTable("PoDetail")
                .HasKey(e => new { e.Ponum, e.ItemNum, e.Lot });

            modelBuilder.Entity<PoDetail_TotalGrnQty>()
                .Property(e => e.Ponum)
                .IsUnicode(false);

            modelBuilder.Entity<PoDetail_TotalGrnQty>()
                .Property(e => e.ItemNum)
                .IsUnicode(false);

            modelBuilder.Entity<PoDetail_TotalGrnQty>()
                .Property(e => e.Lot)
                .IsUnicode(false);

            modelBuilder.Entity<PoDetail_TotalGrnQty>()
                .Property(e => e.TotalGrnQty)
                //.HasColumnName("TotalGrnQty")
                .HasPrecision(18, 5);
            **/

            //配置乐观锁
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Configurations.AddFromAssembly(typeof(DDDTestContext).Assembly);
            modelBuilder.Properties().Where(o => typeof(IRowVersion).IsAssignableFrom(o.DeclaringType) && o.PropertyType == typeof(byte[]) && o.Name == "RowVersion")
               .Configure(o => o.IsConcurrencyToken().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None));
            //Database.SetInitializer(new DDDTestContext());
        }
        ///// <summary>
        ///// 更新乐观锁
        ///// </summary>
        ///// <returns></returns>
        //public override int SaveChanges()
        //{
        //    this.ChangeTracker.DetectChanges();
        //    var objectContext = ((IObjectContextAdapter)this).ObjectContext;
        //    foreach (ObjectStateEntry entry in objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Added | EntityState.Deleted))
        //    {
        //        var v = entry.Entity as IRowVersion;
        //        if (v != null)
        //        {
        //            //v.RowVersion = System.Text.Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
        //        }
        //    }
        //    return base.SaveChanges();
        //}
    }
    /// <summary>
    /// 定义乐观锁
    /// </summary>
    public interface IRowVersion
    {
        byte[] RowVersion { get; set; }
    }

    public class PoDetailConfiguration : EntityTypeConfiguration<PoDetail>
    {
        public PoDetailConfiguration()
        {
            ToTable("PoDetail");
            HasKey(e => new { e.Ponum, e.ItemNum, e.Lot });

            Property(t => t.Ponum);
            
            Property(t => t.ItemNum);
            
            Property(t => t.Lot);

            Property(e => e.Qty)
                .HasPrecision(18, 5);

            Property(e => e.Uprice)
                .HasPrecision(18, 5);

            Property(e => e.Amount)
                .HasPrecision(18, 5);

            HasRequired(e => e.PoDetail_TotalGrnQty)
                .WithRequiredPrincipal();
        }
    }

    public class PoDetail_TotalGrnQtyConfiguration : EntityTypeConfiguration<PoDetail_TotalGrnQty>
    {
        public PoDetail_TotalGrnQtyConfiguration()
        {
            ToTable("PoDetail");
            HasKey(e => new { e.Ponum, e.ItemNum, e.Lot });

            Property(t => t.Ponum);

            Property(t => t.ItemNum);

            Property(t => t.Lot);

            Property(e => e.TotalGrnQty)
                .HasPrecision(18, 5);            
        }
    }
}
