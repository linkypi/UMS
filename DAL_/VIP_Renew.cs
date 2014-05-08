using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 续期
    /// </summary>
    public class VIP_Renew
    { 
        /// <summary>
        /// 续期
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.VIP_Renew VIP_VIPInfo)
        {
            //有效期都是天
            //续期的方式 配置表 Sys_Option 金额续期比列 积分续期比列
            //更新有效期 VIP_VIPInfo 
            //返回
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.UserInfo user, Model.VIP_Renew VIP_VIPInfo)
        {
            //验证退款金额是否 有效，一定要小于等于客户支付金额
            //取消续期天数 Validity
            //取消信息 VIP_RenewBack
            //更新审批单状态
            //更新会员信息VIP_VIPInfo Flag
            //返回
            throw new System.NotImplementedException();
        }
       
    }
}
