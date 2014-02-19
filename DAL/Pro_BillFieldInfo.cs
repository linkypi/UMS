using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class Pro_BillFieldInfo : Sys_InitParentInfo
    {
        private int MethodID = 0;

        public Pro_BillFieldInfo()
        {
            
        }

        public Pro_BillFieldInfo(int method)
        {
            this.MethodID = method;
        }

        public List<Model.Pro_BillFieldInfo> GetList(Model.Sys_UserInfo sysUser,DateTime dt)
    {
        using (LinQSqlHelper lqh = new LinQSqlHelper())
        {
            try
            {
                var query = from b in lqh.Umsdb.Pro_BillFieldInfo
                            select b;
                //if (query == null || query.Count() == 0)
                //{
                //    return null;
                //}

                return query.ToList();
            }
            catch (Exception ex)
            {
                return new List<Model.Pro_BillFieldInfo>();
            }
        }
    }
    }
}