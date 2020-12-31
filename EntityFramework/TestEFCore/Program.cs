using System;
using EFCore.DBContext;
using EFCore.Model;
using EFCore.Model.Mapping;
using System.Linq;


namespace TestEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            {
                using (SysMenuDBContext context = new SysMenuDBContext())
                {
                    var lsit = context.SysFunctionAggregates.ToList();
                    foreach (var sysFunction in lsit)
                    {
                        if (sysFunction.MenuGroups != null && sysFunction.MenuGroups.Count > 0)
                        {
                            foreach (var menuGroup in sysFunction.MenuGroups)
                            {
                                Console.WriteLine("功能ID{0}，功能名称{1}，菜单类别{4}，菜单组ID{2}，菜单组名称{3}"
                                    , sysFunction.SysFunction_Id, sysFunction.GetName(), menuGroup.MenuGroup_Id, menuGroup.GetName(), menuGroup.SysFunction.GetMenuType());
                            }
                        }
                        else
                        {
                            Console.WriteLine("功能ID{0}，功能名称{1}，菜单类别{2}"
                                   , sysFunction.SysFunction_Id, sysFunction.GetName(), sysFunction.GetMenuType());
                        }
                    }
                }
            }
        }
    }
}
