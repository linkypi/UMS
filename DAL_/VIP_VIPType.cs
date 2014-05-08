using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 会员卡类别
    /// </summary>
    public class VIP_VIPType:Sys_InitParentInfo
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.VIP_VIPType model)
        {
            
            //插入类别信息
            //插入类别指定的服务信息 VIP_VIPTypeService
            //返回
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.UserInfo user, Model.VIP_VIPType model)
        {
            //备份 VIP_VIPType_Bak ，VIP_VIPTypeService_BAK
            //更新类别信息
            //更新类别指定的服务信息 VIP_VIPTypeService
            //返回
            throw new System.NotImplementedException();
        }

        public System.Collections.ArrayList GetList(Model.UserInfo user, DateTime dt)
        {
            throw new NotImplementedException();
        }
    }
}
