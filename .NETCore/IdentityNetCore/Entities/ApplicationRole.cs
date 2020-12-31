using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityNetCore.Entities
{
    /// <summary>
    /// 角色
    /// </summary>
    public class ApplicationRole:IdentityRole<Guid>
    {
        /// <summary>
        /// 角色声明
        /// </summary>
        /// <value></value>
        public virtual ICollection<ApplicationRoleClaim> Claims{get;private set;}
        /// <summary>
        /// 用户角色
        /// </summary>
        /// <value></value>
        public virtual ICollection<ApplicationUserRole> UserRoles{get;private set;}
    }
}