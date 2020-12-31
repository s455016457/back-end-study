using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace EFCore.Model
{
    public class SysFunctionAggregate
    {
        private ILazyLoader _lazyLoader;
        public SysFunctionAggregate() { }
        public SysFunctionAggregate(String sysFunction_Id, ILazyLoader lazyLoader)
        {
            this.SysFunction_Id = sysFunction_Id;
            _lazyLoader = lazyLoader;
        }

        private String _name;
        private MenuType _menuType;
        private List<MenuGroup> _menuGroups;

        public String SysFunction_Id { get; }

        public String GetName()
        {
            return _name;
        }

        public void SetName(String name)
        {
            _name = name;
        }

        public MenuType GetMenuType()
        {
            return _menuType;
        }

        public void SetMenuType(MenuType menuType)
        {
            _menuType = menuType;
        }

        public IReadOnlyList<MenuGroup> MenuGroups {
            get => _lazyLoader.Load(this, ref _menuGroups);     //只有在使用到时候才加载
            private set => _menuGroups = new List<MenuGroup>(value);
        }
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
