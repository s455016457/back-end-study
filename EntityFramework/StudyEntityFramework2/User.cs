using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyEntityFramework2
{
    public class User
    {
        [Key]
        public String UserName { get; set; }
        public String DisplayName { get; set; }
    }
}
