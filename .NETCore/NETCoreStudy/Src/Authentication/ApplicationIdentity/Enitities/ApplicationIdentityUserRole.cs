using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationIdentity.Enitities
{
    /// <summary>
    /// 表示用户和角色之间的链接。
    /// </summary>
    public class ApplicationIdentityUserRole: IdentityUserRole<Guid>
    {
    }
}
