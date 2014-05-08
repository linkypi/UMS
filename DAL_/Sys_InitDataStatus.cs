using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 
    /// </summary>
    public class Sys_InitDataStatus
    {
        /// <summary>
        /// 获取已更新的初始数据
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="dt">客户端最后一次更新时间</param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.UserInfo user,DateTime dt)
        {
            //获取已更新列表
            //利用反射 Sys_InitParentInfo.GetList()，存入Sys_InitDataStatus_Child.InitArray
            //无数据则返回null
            //返回值_arrList存放Sys_InitDataStatus_Child列表
            Sys_InitParentInfo p = new DAL_.Pro_HallInfo();
            p.GetList(user,dt);
         
            throw new System.NotImplementedException();
        }
    }
}
