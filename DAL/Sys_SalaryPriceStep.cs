using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Sys_SalaryPriceStep : Sys_InitParentInfo
    {
          private int _MethodID;
       
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Sys_SalaryPriceStep()
        {
            this.MethodID = 0;
        }

        public Sys_SalaryPriceStep(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        public List<Model.Sys_SalaryPriceStep> GetList(Model.Sys_UserInfo sysUser, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query_area = from b in lqh.Umsdb.GetTable<Model.Sys_SalaryPriceStep>()
                                    
                                     select b;
        
                    return query_area.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Sys_SalaryPriceStep>();
                }
            }
        }
    }
}
