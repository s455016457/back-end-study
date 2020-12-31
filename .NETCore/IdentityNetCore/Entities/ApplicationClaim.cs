using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityNetCore.Entities
{
    /// <summary>
    /// 应用程序申明
    /// </summary>
    public class ApplicationClaim
    {
        /// <summary>
        /// 申明Id
        /// </summary>
        /// <value></value>
        public Guid Id { get; private set; }
        /// <summary>
        /// 模组ID
        /// </summary>
        /// <value></value>
        public string ModelId{get;private set;}
        /// <summary>
        /// 申明值
        /// </summary>
        /// <value></value>
        public int Value { get; set; }
    }
}