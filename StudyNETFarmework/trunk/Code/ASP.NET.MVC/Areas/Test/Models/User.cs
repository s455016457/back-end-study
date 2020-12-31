using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET.MVC.Areas.Test.Models
{
    public class User
    {
        [Display(Name ="用户ID")]
        [Required]
        public string Uid { get; set; }

        [Display(Name = "用户名称",AutoGenerateFilter =true,Description ="用户的名称",GroupName ="基本信息",Prompt ="用户名称的提示",ShortName ="用户名网络列标签值")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "用户密码")]
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "电子邮件")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name="登陆次数")]
        public int LoginCout { get; set; }
        [Display(Name="启用状态")]
        public Boolean Enable { get; set; }
    }
}