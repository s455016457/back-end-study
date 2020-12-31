using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationIdentity.Enitities
{
    /// <summary>
    /// 表示用户授权的令牌
    /// </summary>
    public class ApplicationIdentityUserToken: IdentityUserToken<Guid>
    {
    }
}
