
using System.ServiceModel.Activation;
using System.Web;
using System.Web.SessionState;

namespace UserMSService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using ReportModel;
    using System.ServiceModel;


    // 使用 Entities 上下文实现应用程序逻辑。
    // TODO: 将应用程序逻辑添加到这些方法中或其他方法中。
    // TODO: 连接身份验证(Windows/ASP.NET Forms)并取消注释以下内容，以禁用匿名访问
    //还可考虑添加角色，以根据需要限制访问。
    // [RequiresAuthentication]
    //[ServiceKnownType(typeof(ReportModel.Report_InOutSellInfo_Result))]
    //[ServiceKnownType(typeof(List<ReportModel.Report_InOutSellInfo_Result>))]  
    //[ServiceKnownType(typeof(List<ReportModel.Report_OutOrderListInfo>))]
    //[ServiceKnownType(typeof(System.Data.Objects.DataClasses.EntityCollection<ReportModel.Report_OutOrderListInfo>))]
    [EnableClientAccess()]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ReportService : LinqToEntitiesDomainService<Entities>
    {
        
        public HttpSessionState MySession
        {
            get { return HttpContext.Current.Session; }

        }
        public bool IsLogin()
        {
            return MySession["User"] != null;
        }

        // TODO:
        // 考虑约束查询方法的结果。如果需要其他输入，
        //可向此方法添加参数或创建具有不同名称的其他查询方法。
        // 为支持分页，需要向“Pro_ProInfo”查询添加顺序。
//        public IQueryable<Pro_ProInfo> GetPro_ProInfo()
//        {
//            IsLogin();
//            return this.ObjectContext.Pro_ProInfo.OrderBy(p=>p.ProID);
//        }
       
        //public IQueryable<Demo_MapReport> GetMapReport()
        //{
        //    IsLogin();
        //    return this.ObjectContext.Demo_MapReport.OrderBy(p => p.Date);
        //} 

        //public IQueryable<Demo_借贷查询> GetJieDai()
        //{
        //    return this.ObjectContext.Demo_借贷查询.OrderBy(p => p.ID);
        //}
        //public IQueryable<Demo_各厅库存> GetKuCun()
        //{
        //    return this.ObjectContext.Demo_各厅库存.OrderBy(p => p.ID);

        //} 
        //public IQueryable<Demo_周转率报表> ZhouZhuan()
        //{
        //    return this.ObjectContext.Demo_周转率报表.OrderBy(p => p.ID);

        //} 
        //public IQueryable<Demo_归还记录> GuiHuan()
        //{
        //    return this.ObjectContext.Demo_归还记录.OrderBy(p => p.ID);

        //} 
        //public IQueryable<Demo_进销存报表> JinXiaoCun()
        //{
        //    return this.ObjectContext.Demo_进销存报表.OrderBy(p => p.ID);
        //} 
        //public IQueryable<Demo_退货查询>TuiHuo()
        //{
        //    return this.ObjectContext.Demo_退货查询.OrderBy(p => p.ID);
        //}
        //public IQueryable<Demo_销售明细> XiaoShouMingXi()
        //{
        //    try
        //    {
        //        return this.ObjectContext.Demo_销售明细.OrderBy(p => p.ID);
        //    }
        //    catch (Exception ex) {
        //        return null;
        //    }
        //}
     
        public IQueryable<Report_SellListInfo> Report_SellListInfo()
        {
            try
            {
                if (!IsLogin()) return new List<ReportModel.Report_SellListInfo>().AsQueryable(); ;
                DAL.Report_SellListInfo rep = new DAL.Report_SellListInfo();
                return rep.GetList((Model.Sys_UserInfo)MySession["User"], this.ObjectContext);
                //var objSet = this.ObjectContext.Report_SellListInfo;
                //var ValidHallIDS = this.ObjectContext.Pro_HallInfo;
                //var d = from b in objSet
                //         join c in ValidHallIDS
                //         on b.门店编码 equals c.HallID
                //         into temp2
                //         from c1 in temp2
                //         //orderby b.SysDate descending
                //         select b;

                //return d.OrderBy(p=>p.销售日期);
            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_SellListInfo>().AsQueryable(); ;
            }
        }
        //public IQueryable<Report_StoreInfo> Report_StoreInfo()
        //{
        //    try
        //    {
        //        if (!IsLogin()) return new List<ReportModel.Report_StoreInfo>().AsQueryable(); 
        //        //return this.ObjectContext.Report_SellListInfo.OrderByDescending(p => p.销售日期);
        //        DAL.Report_StoreInfo rep = new DAL.Report_StoreInfo();
        //        return rep.GetList((Model.Sys_UserInfo)MySession["User"], this.ObjectContext);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<ReportModel.Report_StoreInfo>().AsQueryable(); 
        //    }
        //}
        public static bool a = true;
        
        public IQueryable<ReportModel.Report_InOutSellInfo> Report_InOutSellInfo(DateTime BeginTime,DateTime EndTime, int PageIndex, int PageSize)
        {
            try
            {
                if (!IsLogin()) return new List<ReportModel.Report_InOutSellInfo>().AsQueryable();

                

                //DAL.Report_InOutSellInfo rep = new DAL.Report_InOutSellInfo();
                //return rep.GetList((Model.Sys_UserInfo)MySession["User"], this.ObjectContext);
                //DAL.LinQSqlHelper lqh = new DAL.LinQSqlHelper();
                //var a= DAL.Report_InOutSellInfo.GetQuery().Invoke(lqh.Umsdb).AsQueryable();
                //return  this.ObjectContext.GetInOutSellInfo(BeginTime, EndTime, PageIndex, PageSize).AsQueryable();
                return null;
                //return a;
            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_InOutSellInfo>().AsQueryable(); 
            }
        }

        /// <summary>
        /// 串码查询
        /// </summary>
        /// <returns></returns>
        //public IQueryable<Report_IMEIInfo> Report_IMEIInfo()
        //{
        //    try
        //    {
        //        if (!IsLogin()) return new List<ReportModel.Report_IMEIInfo>().AsQueryable();
        //        //return this.ObjectContext.Report_SellListInfo.OrderByDescending(p => p.销售日期);
        //        DAL.Report_IMEIInfo rep = new DAL.Report_IMEIInfo();
        //        return rep.GetList((Model.Sys_UserInfo)MySession["User"], this.ObjectContext);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<ReportModel.Report_IMEIInfo>().AsQueryable();
        //    }
        //}
        /// <summary>
        /// 串码跟踪
        /// </summary>
        /// <returns></returns>
        public IQueryable<Report_IMEITracksInfo> Report_IMEITracksInfo()
        {
            try
            {
                if (!IsLogin()) return new List<ReportModel.Report_IMEITracksInfo>().AsQueryable();
                //return this.ObjectContext.Report_SellListInfo.OrderByDescending(p => p.销售日期);
                DAL.Report_IMEITracksInfo rep = new DAL.Report_IMEITracksInfo();
                return rep.GetList((Model.Sys_UserInfo)MySession["User"], this.ObjectContext);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_IMEITracksInfo>().AsQueryable();
            }
        }
        //public IQueryable<Class2> GetClass2()
        //{
        //    return null;
        //}
        /// <summary>
        /// 串码跟踪
        /// </summary>
        /// <returns></returns>
        //public IQueryable<Class1> GetClass1()
        //{
        //    try
        //    {
        //        if (!IsLogin()) return new List<ReportModel.Class1>().AsQueryable();

        //        //var  x= this.ObjectContext.out.Include("Sys_Role_MenuInfo");
        //        //using (DAL.LinQSqlHelper lqh = new DAL.LinQSqlHelper())
        //        //{
        //        //    System.Data.Linq.DataLoadOptions dataload = new System.Data.Linq.DataLoadOptions();
        //        //    dataload.LoadWith<Model.Sys_MenuInfo>(c => c.Sys_Role_MenuInfo); 
        //        //    lqh.Umsdb.LoadOptions = dataload;
        //            //var x = from b in lqh.Umsdb.Pro_OutInfo select b;
        //            //var xx = x.ToList();
        //            //var a = from b in lqh.Umsdb.Pro_OutInfo
        //            //        select new Class1
        //            //        {
        //            //            ID = b.ID,
        //            //            ChildClass2 =b.Pro_OutOrderList.AsQueryable()
        //            //        };
        //            //return null;
        //        //} 
        //        //var xx=    x.ToList();
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<ReportModel.Class1>().AsQueryable();
        //    }
        //}
        public IQueryable<Report_OutOrderListInfo> Report_OutOrderListInfo()
        {
            try
            {
                if (!IsLogin()) return new List<ReportModel.Report_OutOrderListInfo>().AsQueryable();

                var x = this.ObjectContext.Report_OutOrderListInfo;
               
                return x.OrderBy(p => p.批次号);
            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_OutOrderListInfo>().AsQueryable();
            }
        }
      
        
        public IQueryable<Report_OutInfo> Report_OutInfo()
        {
            try
            {
                if (!IsLogin()) return new List<ReportModel.Report_OutInfo>().AsQueryable();
                var t = this.ObjectContext.Report_OutInfo.Include("Report_OutOrderListInfo").OrderBy(p => p.序号);
       
                return t;
            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_OutInfo>().AsQueryable();
            }
        }
        //public IQueryable<Demo_销售汇总> XiaoShouHuiZong()
        //{
        //    return this.ObjectContext.Demo_销售汇总.OrderBy(p => p.ID);
        //} 
        
       public IQueryable<Demo_提成报表> TiCheng()
       {
           return this.ObjectContext.Demo_提成报表.OrderBy(p => p.日期);
       } 
    }
}


