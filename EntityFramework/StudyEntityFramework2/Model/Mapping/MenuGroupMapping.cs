using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyEntityFramework2.Model.Mapping
{
    public class MenuGroupMapping:System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MenuGroup>
    {
        public MenuGroupMapping()
        {
            ToTable("MenuGroup");

            HasKey(t => new { t.SysFunction_Id, t.MenuGroup_Id });

            Property(t => t.SysFunction_Id)
                .HasColumnName("SysFunction_Id");

            Property(t => t.MenuGroup_Id)
                .HasColumnName("MenuGroup_Id");

            Property(t => t.Name)
                .HasColumnName("Name");

            HasRequired(t => t.SysFunction)
                .WithMany(t => t.MenuGroups)
                .HasForeignKey(t => t.SysFunction_Id);
            
        }
    }
}
