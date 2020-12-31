using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityNetCore.Entities
{
    /// <summary>
    /// 用户的身份验证令牌
    /// </summary>
    public class ApplicationUserToken:IdentityUserToken<Guid>
    {
        /// <summary>
        /// 用户
        /// </summary>
        /// <value></value>
        public virtual ApplicationUser User{get;private set;}
   }
}