using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityNetCore.Repositories.Mapping
{
    public class ApplicationUserMapping:IEntityTypeConfiguration<Entities.ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<Entities.ApplicationUser> builder){
            builder.HasMany(p=>p.Claims)
                .WithOne()
                .HasForeignKey(p=>p.UserId)
                .IsRequired();
        }
    }
}