using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyEntityFramework2.Model
{
    public class SysFunctionAggregate
    {

        /**
         * 
         * 私有构造函数，惰性加载失效
         * 惰性加载必须包含无参数共有构造函数
         * private SysFunctionAggregate() { }
         * 
         * */

        // 使用EntityFarmeWork必须有无参数构造函数
        public SysFunctionAggregate() { }
        public SysFunctionAggregate(String sysFunction_Id)
        {
            this.SysFunction_Id = sysFunction_Id;
        }

        private String _sysFunction_Id;

        public String SysFunction_Id { get; private set; }
        public SysFunction SysFunction { get; private set; }

        public virtual List<MenuGroup> MenuGroups { get; set; }

        //private IList<MenuGroup> MenuGroups2 { get; set; }
    }

    public class SysFunction
    {
        private SysFunction()
        {

        }
        public SysFunction(String sysFunction_Id,String name)
        {
            //this.SysFunction_Id = sysFunction_Id;
            this.Name = name;
        }
        //public String SysFunction_Id { get; set; }
        public String Name { get; set; }

        public MenuType MenuType { get; set; }
    }

    /// <summary>
    /// 菜单状态
    /// </summary>
    public enum MenuType
    {
        JICHUCAIDAN = 1,
        GONGYLIANCAIDAN = 2,
        SHENGCCAIDAN = 3,
        CAIWUCAIDAN = 4,
        BAOBIAOCAIDAN = 5,
    }
}
