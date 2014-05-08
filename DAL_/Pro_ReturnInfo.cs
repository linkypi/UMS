using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 归还
    /// </summary>
    public class Pro_ReturnInfo
    {
        /// <summary>
        /// 归还
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.Pro_ReturnInfo model)
        {
            //生成单号 存储过程OrderMacker
            //插入表头 
            //插入明细
            //插入串号明细
            //加库存
            //更新串号表
            //
            //返回
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.UserInfo user, Model.Pro_ReturnInfo model)
        {
            //验证是否超时，user中有功能的操作时限，Admin除外，也即是 roleid=1
            //更新归还单取消信息
            //减库存
            //更新串号表
            //返回
            throw new System.NotImplementedException();
        }
    }
}
