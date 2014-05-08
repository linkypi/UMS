using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 优惠活动
    ///
    /// </summary>
    public class VIP_OffList
    {
        /// <summary>
        /// 新增活动
        /// </summary>
        /// <remarks>用户登录</remarks>
        /// <param name="user">只需要用户名和密码</param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user,Model.VIP_OffList model)
        {
            //Type 0 单品，Type=1 捆绑活动，Type=2 整单满就送活动 ，Type=3 优惠券
            //满多少元 ArriveMoney
            //满多少件 ArriveCount
            //打折 OffRate
            //减金额 OffMoney
            //积分兑换比列 OffPoint  OffPointMoney
            //送积分 SendPoint
            //封顶 HaveTop，1 则1层级，0则无限级
            //插入VIP_ProOffList   model.VIP_ProOffList   验证 ProID  SellTypeID有效性
            //插入会员等级专享列表 VIP_VIPTypeOffLIst 验证
            //插入会员特享列表 VIP_VIPOffLIst 验证
            //其他活动相关信息
            throw new Exception();
        }
        /// <summary>
        /// 删除活动
        /// </summary>
        /// <remarks>用户登录</remarks>
        /// <param name="user">只需要用户名和密码</param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.VIP_OffList model)
        {
            //Flag
            throw new Exception();
        }
       
    }
}
