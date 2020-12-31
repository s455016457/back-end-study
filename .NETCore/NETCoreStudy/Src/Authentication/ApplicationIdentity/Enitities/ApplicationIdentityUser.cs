using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApplicationIdentity.Enitities
{
    /// <summary>
    ///  表示身份系统中的用户
    /// </summary>
    public class ApplicationIdentityUser: IdentityUser<Guid>
    {
    }
}
