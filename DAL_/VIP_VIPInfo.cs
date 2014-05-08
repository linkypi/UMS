using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 会员注册
    /// </summary>
    public class VIP_VIPInfo
    { 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.VIP_VIPType VIP_VIPInfo)
        {

            //插入会员信息
            //插入类别指定的服务信息 VIP_VIPService
            //调用销售 待定 留接口
            //返回
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.UserInfo user, Model.VIP_VIPType VIP_VIPInfo)
        {
            //备份VIP_VIPInfo_BAK
            //修改会员信息 只修改无关联字段，客户个人信息 

            //返回
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 退卡
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.UserInfo user, Model.VIP_Renew VIP_Renew)
        {
            //验证退卡审批单状态 审批单金额有效性，不能超过卡年费
            //更改审批单状态
            //插入退卡信息，VIP_VIPBack
            //卡作废，将卡放入Pro_IMEI_Deleted，Pro_IMEI不能删
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 换卡、升级
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn LevelUp(Model.UserInfo user, Model.VIP_CardChange VIP_CardChange)
        {
            //插入会员信息VIP_VIPInfo
            //插入类别指定的服务信息 VIP_VIPService
            //调用销售 待定 留接口
            //插入换卡信息VIP_CardChange 
            //更新旧卡信息VIP_VIPInfo Flag
            //返回
            throw new System.NotImplementedException();
        }
    }
}
