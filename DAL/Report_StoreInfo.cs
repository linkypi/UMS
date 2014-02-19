using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL
{
   public class Report_StoreInfo
    {
       public string ReportViewName = "Report_StoreInfo";
       public IQueryable<ReportModel.Report_StoreInfo> GetList(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               //using (TransactionScope ts = new TransactionScope())
               //{
               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_StoreInfo>().AsQueryable(); }

                   #endregion

                   var objSet = ObjectContext.Report_StoreInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.门店编码 equals c.HallID
                            into temp2
                            from c1 in temp2
                            //orderby b.SysDate descending
                            select b;

                   //if(ValidProIDS.Count()>0)
                   objSet = from b in objSet
                            //where ValidProIDS.Contains(b.商品编码) || string.IsNullOrEmpty(b.商品编码)
                            join c in ValidProIDS
                            on b.类别编码 equals c.ClassID
                            into temp2
                            from c1 in temp2
                            select b;

                   return objSet.OrderBy(p => p.序号);
               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_StoreInfo>().AsQueryable();

               }
               //}
           }
       }
    }
}
