using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{

    /// <summary>
    /// 提醒
    /// </summary>
    public class Sys_UserRemidList
    {
        private int MenthodID;

	    public Sys_UserRemidList()
	    {
		    this.MenthodID = 0;
	    }

        public Sys_UserRemidList(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }


        /// <summary>
        /// 配置提醒
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Sys_UserRemindList model)
        {
            using(LinQSqlHelper lqh=new LinQSqlHelper())
            {
                using(TransactionScope ts=new TransactionScope())
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_UserRemindList>()
                                where b.Remind == model.Remind
                                select b;
                    if (query.Count() > 0)
                    {
                        return new Model.WebReturn() { Obj = false, Message = "该提醒已经存在 " };
                    }
                    model.UserID = user.UserID;
                    lqh.Umsdb.Sys_UserRemindList.InsertOnSubmit(model);
                    lqh.Umsdb.SubmitChanges();
                    ts.Complete();
                }
            }
         
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
        public Model.WebReturn GetList(Model.Sys_UserInfo user)
        {


            //调用提醒存储过程UserRemindGet ，调用子存储过程生成sql
            //执行sql，返回我的提醒列表
            ///
            ///无存储过程
            using(LinQSqlHelper lqh=new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    StringBuilder sqlstring = new StringBuilder();
                    sqlstring.Append(lqh.Umsdb.UserRemindGet());
                    return new Model.WebReturn() { Obj = sqlstring ,Message="返回提醒列表"};                    
                }
            }
            throw new System.NotImplementedException();
        }
    }
}
