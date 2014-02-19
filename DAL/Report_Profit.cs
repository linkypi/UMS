﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL
{
   public class Report_Profit
    {
       public string ReportViewName = "Report_Profit";
       public Expression<Func<ReportModel.Report_Profit, bool>> GetList1(Model.Sys_UserInfo user,
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




                   var objSet = ObjectContext.Report_Profit.AsQueryable();

                   return
                       o =>
                       ((ValidHallIDS.Any(p => p.HallID == o.门店编码)) &&
                        ValidProIDS.Any(p => p.ClassID == o.类别编码 || o.类别编码 <= 0 || o.类别编码 == null));


                   //           return objSet.OrderByDescending(p => p.销售日期);
               }

       public Expression<Func<ReportModel.Report_Profit2, bool>> GetList2(Model.Sys_UserInfo user,
                                                                               ReportModel.Entities ObjectContext)
       {


           #region 权限
           IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
           //有权限的商品
           IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

           Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, "Report_Profit2", out  ValidHallIDS, out ValidProIDS, ObjectContext);

           if (ret.ReturnValue != true)
           {
               return o => false;
           }

           #endregion




           var objSet = ObjectContext.Report_Profit2.AsQueryable();

           return
               o =>
               ((ValidHallIDS.Any(p => p.HallID == o.门店编码)) &&
                ValidProIDS.Any(p => p.ClassID == o.类别编码 || o.类别编码 <= 0 || o.类别编码 == null));


           //           return objSet.OrderByDescending(p => p.销售日期);
       }


   }
}