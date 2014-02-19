using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL
{
   public class Report_SellListInfo
    {
       public string ReportViewName = "Report_SellListInfo";
       public Expression<Func<ReportModel.Report_SellListInfo, bool>> GetList1(Model.Sys_UserInfo user,
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


 

           var objSet = ObjectContext.Report_SellListInfo.AsQueryable();
           
           return
               o =>
               ((ValidHallIDS.Any(p => p.HallID==o.门店编码)) &&
                ValidProIDS.Any(p => p.ClassID==o.类别编码 || o.类别编码<=0 || o.类别编码==null));
           
 
//           return objSet.OrderByDescending(p => p.销售日期);
       }

       public IQueryable<ReportModel.Report_SellListInfo> GetList(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {



           #region 权限
           IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
           //有权限的商品
           IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

           Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out  ValidHallIDS, out ValidProIDS, ObjectContext);

           if (ret.ReturnValue != true)
           { return new List<ReportModel.Report_SellListInfo>().AsQueryable(); }

           #endregion



           #region 过滤仓库

           var objSet = ObjectContext.Report_SellListInfo.AsQueryable();
           //ValidHallIDS = ObjectContext.Pro_HallInfo.AsQueryable();
           //if (ValidHallIDS.Count() > 0)
           var d = from b in objSet
                   join c in ValidHallIDS
                   on b.门店编码 equals c.HallID
                   into temp2
                   from c1 in temp2
                   select b;

           //if(ValidProIDS.Count()>0)
           d = from b in d
               
               join c in ValidProIDS
               on b.类别编码 equals c.ClassID
               into temp2
               from c1 in temp2
               select b;
           #endregion
           return objSet.OrderByDescending(p => p.销售日期);


       }
    }
}
