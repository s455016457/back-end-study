using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityNetCore.Entities
{
    /// <summary>
    /// 关联用户和角色的联接实体
    /// </summary>
    public class ApplicationUserRole:IdentityUserRole<Guid>
    {
        /// <summary>
        /// 用户
        /// </summary>
        /// <value></value>
        public virtual ApplicationUser User{get;private set;}
        /// <summary>
        /// 角色
        /// </summary>
        /// <value></value>
        public virtual ApplicationRole Role{get;private set;}
    }
}