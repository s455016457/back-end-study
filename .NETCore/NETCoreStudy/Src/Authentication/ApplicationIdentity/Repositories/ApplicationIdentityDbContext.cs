using ApplicationIdentity.Enitities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApplicationIdentity.Repositories
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationIdentityUser, ApplicationIdentityRole, Guid,ApplicationIdentityUserClaim,ApplicationIdentityUserRole,ApplicaitonIdentityUserLogin,ApplicationIdentityRoleClaim,ApplicationIdentityUserToken>
    {
        public ApplicationIdentityDbContext(DbContextOptions options) : base(options) { }
    }
}
