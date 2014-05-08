using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 零售价调价单
    /// </summary>
    public class Pro_PriceChange
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.Pro_PriceChange model)
        {

            //插入条件单信息Pro_PriceChange
            //插入调价单明细Pro_ChangeList  保存新旧价格
            //更新新价格Pro_SellTypeProduct   无记录 则新增
            //
            //返回
            throw new System.NotImplementedException();
        }
    }
}
