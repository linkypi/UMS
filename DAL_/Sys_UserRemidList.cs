using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL_
{
    /// <summary>
    /// 提醒
    /// </summary>
    public class Sys_UserRemidList
    {
        /// <summary>
        /// 配置提醒
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.Sys_UserRemidList model)
        {

            
            //
            //返回
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 获取提醒
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.UserInfo user)
        {


            //调用提醒存储过程UserRemindGet ，调用子存储过程生成sql
            //执行sql，返回我的提醒列表
            throw new System.NotImplementedException();
        }
    }
}
