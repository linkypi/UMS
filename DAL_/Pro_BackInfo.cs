using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 退库
    /// </summary>
    public class Pro_BackInfo
    {
        /// <summary>
        /// 新增退库
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user,Model.Pro_BackInfo model)
        {
            //验证是否可退库
            //生成单号 存储过程OrderMacker
            //插入表头
            //插入明细
            //插入串号明细
            //减去库存
            //删除串号
            //
            //返回
            throw new System.NotImplementedException();
        }
    }
}
