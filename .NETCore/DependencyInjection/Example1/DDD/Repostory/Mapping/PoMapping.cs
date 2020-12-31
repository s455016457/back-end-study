using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example1.DDD.Repostory.Mapping
{
    public class PoMapping:IEntityTypeConfiguration<DDD.Entities.Po>{
        public void Configure(EntityTypeBuilder<DDD.Entities.Po> builder){
            builder.Property(p=>p.RowVersion)
                .IsRowVersion();
        
            builder.Property(p=>p.TotalAmount)
                .HasColumnType("decimal(19,5)");
        }
    }
}

