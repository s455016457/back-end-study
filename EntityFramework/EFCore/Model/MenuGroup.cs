using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Model
{
    public class MenuGroup
    {
        ILazyLoader _lazyLoader;
        public MenuGroup(String menuGroup_Id,String sysFunction_Id, ILazyLoader lazyLoader)
        {
            this.MenuGroup_Id = menuGroup_Id;
            this.SysFunction_Id = sysFunction_Id;
            _lazyLoader = lazyLoader;
        }

        private String _name;
        private SysFunctionAggregate _sysFunction;

        public String MenuGroup_Id { get; }
        public String SysFunction_Id { get; }

        public String GetName()
        {
            return _name;
        }

        public void SetName(String name)
        {
            _name = name;
        }

        public SysFunctionAggregate SysFunction {
            get => _lazyLoader.Load(this, ref _sysFunction);    //只有在使用到时候才加载
            private set => _sysFunction = value;
        }
    }
}
