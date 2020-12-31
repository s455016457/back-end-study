using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationIdentity.Enitities
{
    /// <summary>
    /// 表示授予角色中的所有用户的声明。
    /// </summary>
    public class ApplicationIdentityRoleClaim: IdentityRoleClaim<Guid>
    {
    }
}
