using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityNetCore.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class ApplicationUser:IdentityUser<Guid>
    {
        /// <summary>
        /// 用户声明
        /// </summary>
        /// <value></value>
        public virtual ICollection<ApplicationUserClaim> Claims { get; private set; }
        /// <summary>
        /// 用户登录名
        /// </summary>
        /// <value></value>
        public virtual ICollection<ApplicationUserLogin> Logins { get; private set; }
        /// <summary>
        /// 用户身份令牌
        /// </summary>
        /// <value></value>
        public virtual ICollection<ApplicationUserToken> Tokens { get; private set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        /// <value></value>
        public virtual ICollection<ApplicationUserRole> UserRolses { get; private set; }
    }
}