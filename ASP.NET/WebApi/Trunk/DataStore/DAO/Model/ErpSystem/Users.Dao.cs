
    
//实体类代码自动生成2017-05-05 17:40:54:247

#region Model
namespace DAO.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// User
    /// </summary>
    [Serializable]
    [Table("USERS")]
    public partial class User
    {
        /// <summary>
        /// UID
        /// </summary>
        public String UID { get; set; }

        /// <summary>
        /// NAME
        /// </summary>
        public String NAME { get; set; }

        /// <summary>
        /// PWD
        /// </summary>
        public String PWD { get; set; }

        /// <summary>
        /// DEPT
        /// </summary>
        public String DEPT { get; set; }

        /// <summary>
        /// DESCR
        /// </summary>
        public String DESCR { get; set; }

        /// <summary>
        /// EMAIL
        /// </summary>
        public String EMAIL { get; set; }

        /// <summary>
        /// ISROLE
        /// </summary>
        public Int16 ISROLE { get; set; }

        /// <summary>
        /// CREATED
        /// </summary>
        public DateTime? CREATED { get; set; }

        /// <summary>
        /// CREATEDBY
        /// </summary>
        public String CREATEDBY { get; set; }

        /// <summary>
        /// UPDATESTAMP
        /// </summary>
        public DateTime? UPDATESTAMP { get; set; }

        /// <summary>
        /// UPDATEUSER
        /// </summary>
        public String UPDATEUSER { get; set; }

        /// <summary>
        /// STATUS
        /// </summary>
        public Int16 STATUS { get; set; }

        /// <summary>
        /// CHANGEDPWD
        /// </summary>
        public Int32? CHANGEDPWD { get; set; }

        /// <summary>
        /// INTERNETACCESS
        /// </summary>
        public Int32? INTERNETACCESS { get; set; }

    }
}
#endregion


#region Mapping
namespace DAO.EF.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    /// <summary>
    /// 实体映射User
    /// </summary>
    internal class UserMapping : EntityTypeConfiguration<User>{
        public UserMapping(){
            this.ToTable("USERS");
            this.HasKey(t => t.UID);

            /// <summary>
            /// UID
            /// </summary>
            this.Property(t => t.UID)
                .HasMaxLength(20)
                .HasColumnName("UID");

            /// <summary>
            /// NAME
            /// </summary>
            this.Property(t => t.NAME)
                .HasMaxLength(100)
                .HasColumnName("NAME");

            /// <summary>
            /// PWD
            /// </summary>
            this.Property(t => t.PWD)
                .HasMaxLength(20)
                .HasColumnName("PWD");

            /// <summary>
            /// DEPT
            /// </summary>
            this.Property(t => t.DEPT)
                .HasMaxLength(10)
                .HasColumnName("DEPT");

            /// <summary>
            /// DESCR
            /// </summary>
            this.Property(t => t.DESCR)
                .HasMaxLength(100)
                .HasColumnName("DESCR");

            /// <summary>
            /// EMAIL
            /// </summary>
            this.Property(t => t.EMAIL)
                .HasMaxLength(100)
                .HasColumnName("EMAIL");

            /// <summary>
            /// ISROLE
            /// </summary>
            this.Property(t => t.ISROLE)
                .HasColumnName("ISROLE");

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
            /// UPDATESTAMP
            /// </summary>
            this.Property(t => t.UPDATESTAMP)
                .HasColumnName("UPDATESTAMP");

            /// <summary>
            /// UPDATEUSER
            /// </summary>
            this.Property(t => t.UPDATEUSER)
                .HasMaxLength(20)
                .HasColumnName("UPDATEUSER");

            /// <summary>
            /// STATUS
            /// </summary>
            this.Property(t => t.STATUS)
                .HasColumnName("STATUS");

            /// <summary>
            /// CHANGEDPWD
            /// </summary>
            this.Property(t => t.CHANGEDPWD)
                .HasColumnName("CHANGEDPWD");

            /// <summary>
            /// INTERNETACCESS
            /// </summary>
            this.Property(t => t.INTERNETACCESS)
                .HasColumnName("INTERNETACCESS");
        }
    }
}
#endregion
