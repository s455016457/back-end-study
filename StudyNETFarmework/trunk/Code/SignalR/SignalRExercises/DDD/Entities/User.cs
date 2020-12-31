using SignalRExercises.DDD.Comm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRExercises.DDD.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserGuid")]
        public Guid UserGuid { get; private set; }

        [Column("UserId")]
        public string UserId { get; private set; }

        [Column("UserName")]
        public string UserName { get; private set; }

        [Column("Password")]
        public string Password { get; private set; }

        private User() { }

        public static User CreateNew(string userId, string userName, string password)
        {
            return new User
            {
                UserGuid = Guid.NewGuid(),
                UserId = userId,
                UserName = userName,
                Password = password
            };
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        public void UpdatePassword(string oldPassword,string newPassword)
        {
            if (Password != oldPassword)
                throw new Exception("旧密码不正确，修改密码失败！");
            if (Password == newPassword)
                throw new Exception("新密码不能与旧密码相同！");
            Password = newPassword;
        }
    }
}