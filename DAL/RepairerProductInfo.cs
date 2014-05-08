using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class RepairerProductInfo:Sys_InitParentInfo
    {
          private int MethodID;

        public RepairerProductInfo()
        {
            this.MethodID = 0;
        }

        public RepairerProductInfo(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }


        public List<Model.ASP_RepairerProductInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.ASP_RepairerProductInfo>()
                                //where b.IsDelete == false
                                select b;

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.ASP_RepairerProductInfo>();
                }
            }
        }


    }
}
