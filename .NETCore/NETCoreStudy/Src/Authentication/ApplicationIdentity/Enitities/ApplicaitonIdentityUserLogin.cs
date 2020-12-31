using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationIdentity.Enitities
{
    /// <summary>
    /// 用户登录及关联的登陆提供程序
    /// </summary>
    public class ApplicaitonIdentityUserLogin: IdentityUserLogin<Guid>
    {
    }
}
