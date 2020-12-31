using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityNetCore.Entities
{
    /// <summary>
    /// 角色拥有的声明
    /// </summary>
    public class ApplicationRoleClaim:IdentityRoleClaim<Guid>
    {
        
    }
}