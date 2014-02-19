using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL
{
   public class Report_IMEIInfo
    {
       public string ReportViewName = "Report_IMEIInfo";
       public int MethodID;
       
       public Report_IMEIInfo()
       { 
       
       }
       public Report_IMEIInfo(int MethodID)
       {
           this.MethodID = MethodID;
       }
       public IQueryable<ReportModel.Report_IMEIInfo> GetList(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
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
                   { return new List<ReportModel.Report_IMEIInfo>().AsQueryable(); }

                   #endregion

                   var obj = ObjectContext.Report_IMEIInfo.AsQueryable();

                   obj = from b in obj
                         join c in ValidHallIDS
                         on b.门店编码 equals c.HallID
                         select b;
                   obj = from b in obj
                         join c in ValidProIDS
                         on b.类别编码 equals c.ClassID
                         select b;
                   return obj;
 
               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_IMEIInfo>().AsQueryable();

               }
               //}
           }
       }
       public IQueryable<ReportModel.Report_IMEIInfo> GetList(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext,string IMEIS)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               //using (TransactionScope ts = new TransactionScope())
               //{
               try
               {
                   string[] strs = IMEIS.Split(new Char[] { '\n' });
                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_IMEIInfo>().AsQueryable(); }

                   #endregion

                   var obj = ObjectContext.Report_IMEIInfo.AsQueryable();

                   obj = from b in obj
                         join c in ValidHallIDS
                         on b.门店编码 equals c.HallID
                         where strs.Contains(b.串码)
                         select b;
                   obj = from b in obj
                         join c in ValidProIDS
                         on b.类别编码 equals c.ClassID
                         select b;
                   return obj;

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_IMEIInfo>().AsQueryable();

               }
               //}
           }
       }

       public Model.WebReturn GetList(Model.Sys_UserInfo user,  List<string> list)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               //using (TransactionScope ts = new TransactionScope())
               //{
               try
               { 
                   #region 权限
                   IQueryable<Model.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<Model.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user,this.MethodID, out ValidHallIDS, out ValidProIDS,lqh);

                   if (ret.ReturnValue != true)
                   { return ret; }

                   #endregion

                   var obj = lqh.Umsdb.Report_IMEIInfo.AsQueryable();

                   obj = from b in obj
                         join c in ValidHallIDS
                         on b.门店编码 equals c.HallID
                         where list.Contains(b.串码)
                         select b;
                   obj = from b in obj
                         join c in ValidProIDS
                         on b.类别编码 equals c.ClassID
                         select b;
                   return new Model.WebReturn() { ReturnValue=true, Message="", Obj=obj.ToList() };

               }

               catch (Exception ex)
               {
                   return new Model.WebReturn() {  ReturnValue=false, Message="查询失败："+ex.Message };

               }
               //}
           }
       }
   
       
   }
}
