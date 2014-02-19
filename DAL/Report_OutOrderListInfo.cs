using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL
{
    public class Report_OutOrderListInfo
    {
        public string ReportViewName = "Report_OutOrderListInfo";
        public IQueryable<ReportModel.Report_OutOrderListInfo> GetList1(Model.Sys_UserInfo user,
                                                                              ReportModel.Entities ObjectContext)
       {


           #region 权限
           IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
           //有权限的商品
           IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

           Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out  ValidHallIDS, out ValidProIDS, ObjectContext);

           if (ret.ReturnValue != true)
           {
               return new List<ReportModel.Report_OutOrderListInfo>().AsQueryable();
           }

           #endregion

           var objSet = ObjectContext.Report_OutOrderListInfo.AsQueryable();

           objSet = from b in objSet
                    where ValidHallIDS.Any(p => p.HallID == b.调出仓库编码) || ValidHallIDS.Any(p => p.HallID == b.调入仓库编码)
                    //join c in ValidHallIDS
                    //on b.调出仓库编码 equals c.HallID 
                    //into temp2
                    //from c1 in temp2
                    //orderby b.SysDate descending
                    select b;

           


           return objSet.OrderByDescending(p => p.序号);
       }

        
    }
}
