using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 返库
    /// </summary>
    public class Pro_RepairReturnInfo
    {
        /// <summary>
        /// 返库
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.Pro_RepairReturnInfo model)
        {
            //生成单号 存储过程OrderMacker
            //插入表头 
            //插入明细 若串号变更，更新原串号NEW_IMEI_ID
            //加库存
            //更新串号表
            //
            //返回
            throw new System.NotImplementedException();
        }
    }
}
