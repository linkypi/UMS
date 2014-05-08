using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 成本价调价单
    /// </summary>
    public class Pro_PriceCostChange
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.Pro_PriceCostChange model)
        {

            //插入条件单信息Pro_PriceCostChange
            //插入调价单明细Pro_PriceCostChangeList  保存新旧价格
            //更新新价格Pro_InOrderList    调价单入库时间范围，或者 批次号
            //
            //返回
            throw new System.NotImplementedException();
        }
    }
}
