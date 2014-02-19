using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ASP_CheckInfo : Sys_InitParentInfo
    {
        public List<Model.ASP_CheckInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.ASP_CheckInfo>()
                                select b;
      
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.ASP_CheckInfo>();
                }
            }
        }
    }
}
