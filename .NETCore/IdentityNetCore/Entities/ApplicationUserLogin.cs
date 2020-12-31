using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityNetCore.Entities
{
    /// <summary>
    /// 用户登录名
    /// </summary>
    public class ApplicationUserLogin:IdentityUserLogin<Guid>
    {
        /// <summary>
        /// 用户
        /// </summary>
        /// <value></value>
        public virtual ApplicationUser User{get;private set;}
    }
}