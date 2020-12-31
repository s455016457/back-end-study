using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IInfrastructure.Attribute
{
    public class EntityMappingAttribute:System.Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; }
        /// <summary>
        /// 映射的实体
        /// </summary>
        public String Entity { get; }
        /// <summary>
        /// 关联的键
        /// </summary>
        public String Key { get; }
        /// <summary>
        /// 对应属性名称
        /// </summary>
        /// <param name="Name"></param>
        public EntityMappingAttribute(String Name)
        {
            this.Name = Name;
        }
        /// <summary>
        /// 关联其他实体属性名称
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="Key"></param>
        /// <param name="Name"></param>
        public EntityMappingAttribute(String Entity,String Key,String Name)
        {
            this.Entity = Entity;
            this.Key = Key;
            this.Name = Name;
        }
    }
}
