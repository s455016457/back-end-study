using SignalRExercises.DDD.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Unity;

namespace SignalRExercises.Areas.Sys.Models.User
{
    public class Register
    {
        [Display(Name ="登陆名")]
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string UserId { get; set; }
        [Display(Name = "用户名")]
        [Required]
        [StringLength(maximumLength: 8, MinimumLength = 3)]
        public string UserName { get; set; }
        [Display(Name = "密码")]
        [Required]
        [StringLength(maximumLength:20,MinimumLength =3)]
        public string Password { get; set; }
        [Display(Name = "验证密码")]
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string VerifyPassword { get; set; }

        public Guid Save()
        {
            if(Password!= VerifyPassword)
            {
                throw new Exception("确认密码与密码不一致！");
            }
            var service = UnityConfig.Container.Resolve<UserService>();
            return service.CrateUser(UserId, UserName, Password).UserGuid;
        }
    }
}