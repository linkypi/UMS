using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 借贷
    /// </summary>
    public class Pro_BorowInfo
    {
        /// <summary>
        /// 借贷
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.Pro_BorowInfo model)
        {
            //验证审批单是否有效，更新审批单状态Pro_BorowAduit
            //生成单号 存储过程OrderMacker
            //插入表头 
            //插入明细
            //插入串号明细
            //减少库存
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
        public Model.WebReturn Delete(Model.UserInfo user, Model.Pro_BorowInfo model)
        {
            //验证是否超时，user中有功能的操作时限，Admin除外，也即是 roleid=1
            //更新借贷单取消信息
            //加库存
            //更新串号表
            //借贷审批单返回状态
            //返回
            throw new System.NotImplementedException();
        }
    }
}
