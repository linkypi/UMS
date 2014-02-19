using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Pro_BigAreaInfo : Sys_InitParentInfo
    {
        public List<Model.Pro_BigAreaInfo> GetList(Model.Sys_UserInfo sysUser, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query_area = from b in lqh.Umsdb.GetTable<Model.Pro_BigAreaInfo>()
                                     where b.Flag == true
                                     select b;
                    
                    return query_area.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Pro_BigAreaInfo>();
                }
            }
        }
    }
}
