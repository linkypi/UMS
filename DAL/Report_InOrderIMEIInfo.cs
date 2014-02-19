using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL
{
    public class Report_InOrderIMEIInfo
    {
        public string ReportViewName = "Report_InOrderIMEIInfo";
        public Expression<Func<ReportModel.Report_InOrderIMEIInfo, bool>> GetList1(Model.Sys_UserInfo user,
                                                                              ReportModel.Entities ObjectContext)
       {


           #region 权限
           IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
           //有权限的商品
           IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

           Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out  ValidHallIDS, out ValidProIDS, ObjectContext);

           if (ret.ReturnValue != true)
           {
               return o => false;
           }

           #endregion




           var objSet = ObjectContext.Report_InOrderIMEIInfo.AsQueryable();
           
           return
               o =>
               ((ValidHallIDS.Any(p => p.HallID==o.入库仓库编码)) &&
                ValidProIDS.Any(p => p.ClassID==o.类别编码));
           
 
//           return objSet.OrderByDescending(p => p.销售日期);
       }

        
    }
}
