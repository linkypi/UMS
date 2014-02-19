using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 属性值
    /// 
    /// 无判断值
    /// 
    /// 
    /// </summary>
    public class Pro_PropertyValue : Sys_InitParentInfo
    {
        
        private int MenthodID;

	    public Pro_PropertyValue()
	    {
		    this.MenthodID = 0;
	    }

        public Pro_PropertyValue(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }


        public  List<Model.Pro_PropertyValue> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
             
                    try
                    {
                        var query = from b in lqh.Umsdb.GetTable<Model.Pro_PropertyValue>()

                                          select b;
                        if (query == null || query.Count() == 0)
                        {
                            return null;
                        }
                        //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                        //arr.AddRange(query_user);
                     
                        return query.ToList();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

        

    }
}

