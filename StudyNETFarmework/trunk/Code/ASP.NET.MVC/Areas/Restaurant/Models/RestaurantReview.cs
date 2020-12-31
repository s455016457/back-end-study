using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET.MVC.Areas.Restaurant.Models
{
    /// <summary>
    /// 餐厅评价
    /// </summary>
    public class RestaurantReview : IValidatableObject
    {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "Id")]
        public int Id { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        [Range(0, 9)]
        [Display(Name = "等级")]
        public int Rating { get; set; }
        /// <summary>
        /// 评论正文
        /// </summary>
        [Display(Name = "评论正文")]
        [Required]
        [StringLength(1024)]
        public string Body { get; set; }
        /// <summary>
        /// 评论人名称
        /// </summary>
        [Display(Name = "评论人名称")]
        [Required]
        [StringLength(20)]
        public string RestaurantName { get; set; }
        /// <summary>
        /// 餐厅ID
        /// </summary>
        [Display(Name = "餐厅ID")]
        public int RestaurantId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RestaurantName.ToLower().Equals("小王") && Rating < 3)
                yield return new ValidationResult("您的信用等级太低，不能这样做！");
        }

        /// <summary>
        /// 餐厅
        /// </summary>
        //public Restaurant Restaurant { get; set; }
    }
}