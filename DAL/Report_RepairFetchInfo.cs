using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ReportModel;

namespace DAL
{
    public class Report_RepairFetchInfo
    {
        private string ReportViewName = "Report_RepairFetchInfo";  // IQueryable<ReportModel.Report_Borow> 
        public Expression<Func<ReportModel.Report_RepairFetchInfo, bool>> GetList(Model.Sys_UserInfo user, Entities currentDataSource)
        {
            #region 权限
            IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null ;
            //有权限的商品
            IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

            Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out  ValidHallIDS, out ValidProIDS, currentDataSource);

            if (ret.ReturnValue != true)
            {
                return o => false;
            }

            #endregion

            #region 
            var objSet = currentDataSource.Report_RepairFetchInfo.AsQueryable();


            objSet = from b in objSet
                        join c in ValidHallIDS
                        on b.领出仓库 equals c.HallName
                     where b.领出仓库.Contains("_")
                        select b;

            //if(ValidProIDS.Count()>0)
                  
            #endregion

            return o => ((ValidHallIDS.Any(p =>  o.网点名称.Contains("_") && p.HallName==o.网点名称)));
        }
    }
}
