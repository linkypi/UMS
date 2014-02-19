using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 会员卡类别初始服务
    /// 
    /// 
    /// 无判断值,只能返回全值
    /// 
    /// </summary>
    public class VIP_TypeService : Sys_InitParentInfo
    {
        private int MenthodID;

	    public VIP_TypeService()
	    {
		    this.MenthodID = 0;
	    }

        public VIP_TypeService(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }

        public  List<Model.VIP_VIPTypeService> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
             using(LinQSqlHelper lqh=new LinQSqlHelper())
            {
               try
                    {                    
                        var query =from b in lqh.Umsdb.GetTable<Model.VIP_VIPTypeService>()
                                         select b;
                        if (query == null || query.Count() == 0)
                        {
                            return null;
                        }
                        //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                        //arr.AddRange(query);
                    
                        return query.ToList();
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }

        public Model.WebReturn GetModel(Model.Sys_UserInfo user,int ID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query =( from b in lqh.Umsdb.View_VIPTypeService                                 
                                 where b.ID==ID
                                select b).ToList();
                         if (query == null || query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue=false, Message = "获取失败" };
                    }
                    return new Model.WebReturn() { Obj = query, ReturnValue=true, Message = "获取成功" };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取失败" };
                }
            
            }
        }
        
    }
}
