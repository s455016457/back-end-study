using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyEntityFramework2.Model
{
    public class MenuGroup
    {
        public MenuGroup() { }
        public MenuGroup(String MenuGroup_Id)
        {
            this.MenuGroup_Id = MenuGroup_Id;
        }
        public String MenuGroup_Id { get; private set; }

        public String SysFunction_Id { get; private set; }

        public String Name { get; set; }

        public virtual SysFunctionAggregate SysFunction { get; set; }
    }
}
