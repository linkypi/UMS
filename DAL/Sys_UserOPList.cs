using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class Sys_UserOPList : Sys_InitParentInfo
    {
              private int MenthodID;

	    public Sys_UserOPList()
	    {
		    this.MenthodID = 0;
	    }

        public Sys_UserOPList(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }

        public List<Model.Sys_UserOPList> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_UserOPList>()

                                where b.Flag==true //&& (b.LeaveDate==null ||b.LeaveDate>DateTime.Now) && b.CreateDate<DateTime.Now
                                     select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}
                    //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    //arr.AddRange(query_area);

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Sys_UserOPList>();
                }
            }
        }
    }
}