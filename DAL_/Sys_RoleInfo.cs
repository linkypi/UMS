using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Sys_RoleInfo : Sys_InitParentInfo
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.Sys_RoleInfo model,Model.Sys_RoleMethod role_menu_method)
        {
            
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List
            //获取方法 生成 Method_ID_List 
            //插入Sys_RoleInfo Sys_RoleMethod 
            //
            //返回
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.UserInfo user, Model.Sys_RoleInfo model, Model.Sys_RoleMethod role_menu_method)
        {
            //备份角色信息Sys_RoleInfo_back
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List
            //获取方法 生成 Method_ID_List 
            //插入Sys_RoleInfo Sys_RoleMethod 
            //更新原Sys_RoleInfo
            //返回
            throw new System.NotImplementedException();
        }

        public System.Collections.ArrayList GetList(Model.UserInfo user, DateTime dt)
        {
            throw new NotImplementedException();
        }
    }
}
