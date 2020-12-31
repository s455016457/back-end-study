using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityNetCore.Entities
{
    /// <summary>
    /// 用户拥有的声明
    /// </summary>
    public class ApplicationUserClaim:IdentityUserClaim<Guid>
    {
        
    }
}