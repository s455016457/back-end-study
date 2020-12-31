using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyEntityFramework2.Model.Mapping
{
    public class SysFunctionMapping:System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SysFunctionAggregate>
    {
        public SysFunctionMapping()
        {
            ToTable("SysFunction");

            HasKey(t => t.SysFunction_Id);

            Property(t => t.SysFunction_Id)
                .HasColumnName("SysFunction_Id");

            //Ignore(t => t.SysFunction.SysFunction_Id);

            Property(t => t.SysFunction.Name)
                .HasColumnName("Name");
            Property(t => t.SysFunction.MenuType)
                .HasColumnName("MenuType");

            //这样声明是错误的MenuGroups必须是Iconllection接口的实现类
            HasMany(t => (List<MenuGroup>)t.MenuGroups).WithRequired(t => t.SysFunction).HasForeignKey(t => t.SysFunction_Id);
                           
        }
    }
}
