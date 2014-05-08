//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using DAL;
using Model;
using ReportModel;
using Sys_UserInfo = Model.Sys_UserInfo;
using System.Data.Services.Providers;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;


namespace UserMSService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [JSONPSupportBehavior]
    // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
//    [ServiceContract(SessionMode = SessionMode.Required)]
    public class WcfReportService : DataService<Entities>
    {
       // public Entities ObjectContext;

        // This method is called only once to initialize service-wide policies.
        protected override Entities CreateDataSource()
        {
            var ctx = new Entities();
            ctx.CommandTimeout=300;
            
            ctx.Report_OutInfo.Include("Report_OutOrderListInfo");
            return ctx;
        }

       
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:

//            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            //config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
            config.UseVerboseErrors = true;
            
            
        }
       // [EnableSessions=true]

        public Model.Sys_UserInfo Login()
        {
            string username = HttpContext.Current.Request.Headers["X-UserName"];
            string password = HttpContext.Current.Request.Headers["X-Password"];
            string sign = null;
            return Login(username, password, sign);
        }

        public Model.Sys_UserInfo Login(string username, string password, string sign)
        {

//            username = "1";
//            password = "1";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {

                throw new Exception("用户名密码不能为空");
            }
            username = HttpUtility.UrlDecode(username);
            password =  HttpUtility.UrlDecode(password);

            #region 解密时间戳
            if (!Common.MainCheckHelp.CheckTimeTrick(sign))
            {

                throw new Exception("非法请求");
            }
            #endregion


            //验证用户信息
            Model.Sys_UserInfo User = new Model.Sys_UserInfo();
            User.UserName = username;
            User.UserPwd = password;

            DAL.Sys_LoginInfo login = new DAL.Sys_LoginInfo();
           var  webR = login.CheckUserByReport(User);
            
                
            return (Sys_UserInfo) webR;
           
        }


        public IDictionary<string, object> MySession
        {
            get
            {

                return Common.MyWcfContext.Current.Items;
            }

        }
        public bool IsLogin()
        {
            try
            {
                return MySession["User"] != null;
            }
            catch (Exception)
            {
                return false;
                
                
            }
            
        }
//
//        [WebGet]
//        public IQueryable<ReportModel.Demo_MapReport> GetMapReport()
//        {
//            //this.ObjectContext = this.CurrentDataSource;
//            IsLogin();
//            return CurrentDataSource.Demo_MapReport.OrderBy(p => p.Date);
//        }
//         [WebGet]
//        public IQueryable<ReportModel.Demo_借贷查询> GetJieDai()
//        {
//            return this.CurrentDataSource.Demo_借贷查询.OrderBy(p => p.ID);
//        }
//        [WebGet]
//         public IQueryable<ReportModel.Demo_各厅库存> GetKuCun()
//        {
//            return this.CurrentDataSource.Demo_各厅库存.OrderBy(p => p.ID);
//
//        }
//         public IQueryable<ReportModel.Demo_周转率报表> ZhouZhuan()
//        {
//            return this.CurrentDataSource.Demo_周转率报表.OrderBy(p => p.ID);
//
//        }
//         public IQueryable<ReportModel.Demo_归还记录> GuiHuan()
//        {
//            return this.CurrentDataSource.Demo_归还记录.OrderBy(p => p.ID);
//
//        }
//         public IQueryable<ReportModel.Demo_进销存报表> JinXiaoCun()
//        {
//            return this.CurrentDataSource.Demo_进销存报表.OrderBy(p => p.ID);
//        }
//         public IQueryable<ReportModel.Demo_退货查询> TuiHuo()
//        {
//            return this.CurrentDataSource.Demo_退货查询.OrderBy(p => p.ID);
//        }
//         public IQueryable<ReportModel.Demo_销售明细> XiaoShouMingXi()
//        {
//            try
//            {
//                return this.CurrentDataSource.Demo_销售明细.OrderBy(p => p.ID);
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }


        [QueryInterceptor("Report_RepairFetchInfo")]
        public Expression<Func<ReportModel.Report_RepairFetchInfo, bool>> Report_RepairFetchInfo()
        {
            try
            {
                var user = Login();

                DAL.Report_RepairFetchInfo rep = new DAL.Report_RepairFetchInfo();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }


        [QueryInterceptor("Report_BaoWaiCash")]
        public Expression<Func<ReportModel.Report_BaoWaiCash, bool>> Report_BaoWaiCash()
        {
            try
            {
                var user = Login();

                DAL.Report_BaoWaiCash rep = new DAL.Report_BaoWaiCash();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }

        [QueryInterceptor("Report_CallBackInfo")]
        public Expression<Func<ReportModel.Report_CallBackInfo, bool>> Report_CallBackInfo()
        {
            try
            {
                var user = Login();

                DAL.Report_CallBackInfo rep = new DAL.Report_CallBackInfo();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }


        [QueryInterceptor("Report_RepairTimeOutInfo")]
        public Expression<Func<ReportModel.Report_RepairTimeOutInfo, bool>> Report_RepairTimeOutInfo()
        {
            try
            {
                var user = Login();

                DAL.Report_RepairTimeOutInfo rep = new DAL.Report_RepairTimeOutInfo();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }


        [QueryInterceptor("Report_AfterSale")]
        public Expression<Func<ReportModel.Report_AfterSale, bool>> Report_AfterSale()
        {
            try
            {
                var user = Login();

                DAL.Report_AfterSale rep = new DAL.Report_AfterSale();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }

        [QueryInterceptor("Report_邮储三方报表")]
        public Expression<Func<ReportModel.Report_邮储三方报表, bool>> Report_邮储三方报表()
        {
            try
            {
                var user = Login();

                DAL.Report_邮储三方报表 rep = new DAL.Report_邮储三方报表();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }

        [QueryInterceptor("Report_SMSSign")]
        public Expression<Func<ReportModel.Report_SMSSign, bool>> Report_SMSSign()
        {
            try
            {
                var user = Login();
                return o => true;

            }
            catch (Exception)
            {
                return o => false;
            }
        }

        [QueryInterceptor("Report_SellListInfo2")]
        public Expression<Func<ReportModel.Report_SellListInfo2, bool>> Report_SellListInfo2()
        {
            try
            {
                var user = Login();
                return o => true;

            }
            catch (Exception)
            {
                return o => false;
            }
        }
        [QueryInterceptor("Report_VipBuyTimeInfo2")]
        public Expression<Func<ReportModel.Report_VipBuyTimeInfo2, bool>> Report_VipBuyTimeInfo2()
        {
            try
            {
                var user = Login();
                return o => true;

            }
            catch (Exception)
            {
                return o => false;
            }
        }
        [QueryInterceptor("Report_YANBAO2")]
        public Expression<Func<ReportModel.Report_YANBAO2, bool>> Report_YANBAO2()
        {
            try
            {
                var user = Login();
                return o => true;

            }
            catch (Exception)
            {
                return o => false;
            }
        }

        [QueryInterceptor("Chart_MapReport")]
        public Expression<Func<ReportModel.Chart_MapReport, bool>> Chart_MapReport()
        {
            try
            {
                var user = Login();
                return o => true;

            }
            catch (Exception)
            {
                return o => false;
            }
        }
        [QueryInterceptor("Report_Profit")]
        public Expression<Func<ReportModel.Report_Profit, bool>> Report_Profit()
        {
            try
            {
                //return o => true;
                var user = Login();
                DAL.Report_Profit rep = new DAL.Report_Profit();
                return rep.GetList1(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }
        [QueryInterceptor("Report_Profit2")]
        public Expression<Func<ReportModel.Report_Profit2, bool>> Report_Profit2()
        {
            try
            {
                //return o => true;
                var user = Login();
                DAL.Report_Profit rep = new DAL.Report_Profit();
                return rep.GetList2(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }
        [QueryInterceptor("Report_SellReport")]
        public Expression<Func<ReportModel.Report_SellReport, bool>> Report_SellReport()
        {
            try
            {
                //return o => true;
                var user = Login();
                DAL.Report_SellReport rep = new DAL.Report_SellReport();
                return rep.GetList1(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }
        [QueryInterceptor("Report_SellReport2")]
        public Expression<Func<ReportModel.Report_SellReport2, bool>> Report_SellReport2()
        {
            try
            {
                //return o => true;
                var user = Login();
                DAL.Report_SellReport rep = new DAL.Report_SellReport();
                rep.ReportViewName = "Report_SellReport2";
                return rep.GetList2(user, this.CurrentDataSource);

            }
            catch (Exception)
            {
                return o => false;
            }
        }
        [QueryInterceptor("Report_SellListInfo")]
        public Expression<Func<ReportModel.Report_SellListInfo, bool>> Report_SellListInfo()
        {
            
            try
            {
                var user = Login();

                DAL.Report_SellListInfo rep = new DAL.Report_SellListInfo();
                return rep.GetList1(user, this.CurrentDataSource);
                //return o => true;
            }
            catch (Exception)
            {
                return o => false;
                
            }
           
        }
        [QueryInterceptor("Report_InOrderListInfo")]
        public Expression<Func<ReportModel.Report_InOrderListInfo, bool>> Report_InOrderListInfo()
        {

            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_InOrderListInfo rep = new DAL.Report_InOrderListInfo();
                return rep.GetList1(user, this.CurrentDataSource);
                //return o => true;
            }
            catch (Exception)
            {
                return o => false;

            }

        }
        [QueryInterceptor("Report_InOrderIMEIInfo")]
        public Expression<Func<ReportModel.Report_InOrderIMEIInfo, bool>> Report_InOrderIMEIInfo()
        {

            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_InOrderIMEIInfo rep = new DAL.Report_InOrderIMEIInfo();
                return rep.GetList1(user, this.CurrentDataSource);
                //return o => true;
            }
            catch (Exception)
            {
                return o => false;

            }

        }
        [QueryInterceptor("Report_BackListInfo")]
        public Expression<Func<ReportModel.Report_BackListInfo, bool>> Report_BackListInfo()
        {

            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_BackListInfo rep = new DAL.Report_BackListInfo();
                return rep.GetList1(user, this.CurrentDataSource);
                //return o => true;
            }
            catch (Exception)
            {
                return o => false;

            }

        }
        [QueryInterceptor("Report_BackIMEIInfo")]
        public Expression<Func<ReportModel.Report_BackIMEIInfo, bool>> Report_BackIMEIInfo()
        {

            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_BackIMEIInfo rep = new DAL.Report_BackIMEIInfo();
                return rep.GetList1(user, this.CurrentDataSource);
                //return o => true;
            }
            catch (Exception)
            {
                return o => false;

            }

        }
        [QueryInterceptor("Report_ChangeListInfo")]
        public Expression<Func<ReportModel.Report_ChangeListInfo, bool>> Report_ChangeListInfo()
        {

            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_ChangeListInfo rep = new DAL.Report_ChangeListInfo();
                return rep.GetList1(user, this.CurrentDataSource);
                //return o => true;
            }
            catch (Exception)
            {
                return o => false;

            }

        }
        [QueryInterceptor("Report_ChangeIMEIInfo")]
        public Expression<Func<ReportModel.Report_ChangeIMEIInfo, bool>> Report_ChangeIMEIInfo()
        {

            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_ChangeIMEIInfo rep = new DAL.Report_ChangeIMEIInfo();
                return rep.GetList1(user, this.CurrentDataSource);
                //return o => true;
            }
            catch (Exception)
            {
                return o => false;

            }

        }
        [WebGet]
        public IQueryable<ReportModel.Report_OutOrderListInfo> Report_OutOrderListInfo()
        {

            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_OutOrderListInfo rep = new DAL.Report_OutOrderListInfo();
                return rep.GetList1(user, this.CurrentDataSource);

                //var objectIdExpr = this.Report_OutInfo();

                //Expression<Func<ReportModel.Report_OutOrderListInfo,
                //ReportModel.Report_OutInfo>> parentEX
                //    = p => p.Report_OutInfo;
                //var swap = new SwapVisitor(objectIdExpr.Parameters[0], parentEX.Body);
                //var newExpr = Expression.Lambda<Func<ReportModel.Report_OutOrderListInfo, bool>>(
                //       swap.Visit(objectIdExpr.Body), parentEX.Parameters);
                //return newExpr;


                //return newExpr;
            }
            catch (Exception)
            {
                return new List<ReportModel.Report_OutOrderListInfo>().AsQueryable();

            }

        }
        //[QueryInterceptor("Report_OutInfo")]
        [WebGet]
        public IQueryable<ReportModel.Report_OutInfo> Report_OutInfo()
        {

            //this.CurrentDataSource.LoadProperty(this.CurrentDataSource.Report_OutOrderListInfo, "");
            try
            {
                //this.CurrentDataSource.Report_OutInfo.Include("Report_OutOrderListInfo");
                //var objectIdExpr=this.Report_OutOrderListInfo();
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.AllReportInfo rep = new DAL.AllReportInfo("Report_OutInfo");
                return rep.GetList_AssetsOut(user, this.CurrentDataSource);


                //return o => true;
            }
            catch (Exception)
            {
                return new List<ReportModel.Report_OutInfo>().AsQueryable();

            }
        }
        [WebGet]
        public IQueryable<ReportModel.Report_OutOrderIMEIInfo> Report_OutOrderIMEIInfo()
        {

            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_OutOrderIMEIInfo rep = new DAL.Report_OutOrderIMEIInfo();
                return rep.GetList1(user, this.CurrentDataSource);
                //return o => true;
            }
            catch (Exception)
            {
                return new List<ReportModel.Report_OutOrderIMEIInfo>().AsQueryable();

            }

        }
        /// <summary>
        /// 串码跟踪
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_IMEITracksInfo> Report_IMEITracksInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"],
                    HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.Report_IMEITracksInfo rep = new DAL.Report_IMEITracksInfo();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_IMEITracksInfo>().AsQueryable();
            }
        }
        [WebGet]
        public IQueryable<ReportModel.Report_StoreInfo> Report_StoreInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                
                DAL.Report_StoreInfo rep = new DAL.Report_StoreInfo();

                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_StoreInfo>().AsQueryable();
            }
        }
        #region 串码查询
        /// <summary>
        /// 串码查询
        /// </summary>
        /// <returns></returns>

        [WebGet]
        public IQueryable<ReportModel.Report_IMEIInfo> Report_IMEIInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);
                
                DAL.Report_IMEIInfo rep = new DAL.Report_IMEIInfo();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_IMEIInfo>().AsQueryable();
            }
        }

        /// <summary>
        /// 串码查询
        /// </summary>
        /// <returns></returns> 
        [WebGet]
        public IQueryable<ReportModel.Report_IMEIInfo> Report_IMEIInfoMulti(String IMEIS)
        {
            try
            {
                 

                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_IMEIInfo rep = new DAL.Report_IMEIInfo();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_IMEIInfo>().AsQueryable();
            }
        }
        #endregion
        
        /// <summary>
        /// 各厅库存
        /// </summary>
        /// <returns></returns>
        //[QueryInterceptor("Report_EveryHallStoreInfo")]
        [WebGet]
        public IQueryable<ReportModel.Report_EveryHallStoreInfo> Report_EveryHallStoreInfo()
        {
            try
            {
                
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

                DAL.Report_EveryHallStoreInfo rep = new DAL.Report_EveryHallStoreInfo();
                return rep.GetList(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_EveryHallStoreInfo>().AsQueryable();
            }
        }
        
         ////<summary>
         ////进销存
         ////</summary>
         ////<returns></returns>
        //[QueryInterceptor("GetInOutSellInfo")]
        //public Expression<Func<ReportModel.GetInOutSellInfo_Result, bool>> GetInOutSellInfo(DateTime dt, DateTime dt2)
        //{
        //    try
        //    {
        //        return p=>true;
        //        //this.CurrentDataSource.GetInOutSellInfo()
        //        //var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);

        //        //DAL.Report_EveryHallStoreInfo rep = new DAL.Report_EveryHallStoreInfo();
        //        //return rep.GetList(user, this.CurrentDataSource);
        //        //this.CurrentDataSource.GetInOutSellInfo
        //    }
        //    catch (Exception ex)
        //    {
        //        return o => false;
        //    }
        //}
  
         public IQueryable<ReportModel.Report_SellListInfo> Report_SellListInfo_()
        {
            try
            {
               
                if (!IsLogin()) return new List<ReportModel.Report_SellListInfo>().AsQueryable(); ;
                DAL.Report_SellListInfo rep = new DAL.Report_SellListInfo();
                return rep.GetList((Model.Sys_UserInfo)MySession["User"], this.CurrentDataSource);
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
         
        public static bool a = true;

        //[WebGet]
        //public IQueryable<ReportModel.Report_InOutSellInfo> Report_InOutSellInfo()
        //{
        //    var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);
        //    DAL.Report_InOutSellInfo rep = new DAL.Report_InOutSellInfo();
        //    return rep.GetListNew(user, this.CurrentDataSource);
           
        //}
        [WebGet]
        public IQueryable<ReportModel.Report_InOutSellInfo> Report_InOutSellInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DateTime BeginTime = Convert.ToDateTime(HttpContext.Current.Request.Params["BeginTime"]).Date;

                DateTime EndTime = Convert.ToDateTime(HttpContext.Current.Request.Params["EndTime"]).Date.AddDays(1).AddSeconds(-1);

                

                if (BeginTime >= EndTime)
                {
                    BeginTime = DateTime.Now.AddMonths(-1);
                    EndTime = DateTime.Now.AddDays(1).AddSeconds(-1);// HttpContext.Current.Request.Params["EndTime"];
                }

                DAL.Report_InOutSellInfo rep = new DAL.Report_InOutSellInfo();


                var query = this.CurrentDataSource.GetInOutSellInfo(BeginTime, EndTime, "Report_InOutSellInfo",user.RoleID).AsQueryable();
                //query = from b in query
                //        where this.CurrentDataSource.Pro_HallInfo.Any(p => p.HallID == b.门店编码) &&
                //        this.CurrentDataSource.Pro_ClassInfo.Any(p => p.ClassID == b.类别编码)
                //        select b;
                return query.ToList().AsQueryable();
                //return this.CurrentDataSource.Report_InOutSellInfo;
            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_InOutSellInfo>().AsQueryable();
            }
        }
        /// <summary>
        /// 成本价格表
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_CostPriceListInfo> Report_CostPriceListInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_CostPriceListInfo");

                return report.GetList_CostPrice(user, this.CurrentDataSource);
                
            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_CostPriceListInfo>().AsQueryable();
            }
        }
        /// <summary>
        /// 销售价格表
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_PriceInfo> Report_PriceInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_PriceInfo");

                var x = report.GetList_Price(user, this.CurrentDataSource);

                return x;

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_PriceInfo>().AsQueryable();
            }
        }
        #region 借贷

        #region 借贷明细
        /// <summary>
        /// 借贷明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_BorrowInfo> Report_BorrowInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_BorrowInfo");

                return report.GetList_BorrowList(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_BorrowInfo>().AsQueryable();
            }
        }
        #endregion
        #region 借贷串码明细
        /// <summary>
        /// 借贷串码明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_BorrowIMEIInfo> Report_BorrowIMEIInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_BorrowIMEIInfo");

                return report.GetList_BorrowIMEI(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_BorrowIMEIInfo>().AsQueryable();
            }
        }
        #endregion
        #endregion

        #region 归还

        #region 归还明细
        /// <summary>
        /// 归还明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_ReturnListInfo> Report_ReturnListInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_ReturnListInfo");

                return report.GetList_ReturnList(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_ReturnListInfo>().AsQueryable();
            }
        }
        #endregion
        #region 借贷串码明细
        /// <summary>
        /// 归还串码明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_ReturnIMEIInfo> Report_ReturnIMEIInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_ReturnIMEIInfo");

                return report.GetList_ReturnIMEI(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_ReturnIMEIInfo>().AsQueryable();
            }
        }
        #endregion


        #endregion

        #region 续期
        /// <summary>
        /// 续期明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_ReNewInfo> Report_ReNewInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_ReNewInfo");

                return report.GetList_ReturnReNew(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_ReNewInfo>().AsQueryable();
            }
        }
        #endregion


        #region 送修
        /// <summary>
        /// 送修明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_RepairInfo> Report_RepairInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_RepairInfo");

                return report.GetList_ReturnRepair(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_RepairInfo>().AsQueryable();
            }
        }
        #endregion

        #region 返库
        /// <summary>
        /// 返库明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_RepairReturnInfo> Report_RepairReturnInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_RepairReturnInfo");

                return report.GetList_ReturnRepairReturn(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_RepairReturnInfo>().AsQueryable();
            }
        }
        #endregion


        #region 打印
        /// <summary>
        /// 打印销售
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Print_SellInfo> Print_SellInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Print_SellInfo");

                return report.GetList_PrintSell(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Print_SellInfo>().AsQueryable();
            }
        }
        /// <summary>
        /// 打印退货
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Print_SellBackInfo> Print_SellBackInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Print_SellBackInfo");

                return report.GetList_PrintBackSell(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Print_SellBackInfo>().AsQueryable();
            }
        }
        #endregion

        #region 商品信息

        /// <summary>
        /// 商品信息
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_ProductInfo> Report_ProductInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_ProductInfo");

                return report.GetList_Product(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_ProductInfo>().AsQueryable();
            }
        }

        #endregion

        #region 空充调拨
        /// <summary>
        /// 空充调拨
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_AirOutListInfo> Report_AirOutListInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_AirOutListInfo");

                return report.GetList_AirOut(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_AirOutListInfo>().AsQueryable();
            }
        }
        #endregion
        #region 广信延保销售明细
        /// <summary>
        /// 空充调拨
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_YANBAO> Report_YANBAO()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_YANBAO");

                return report.GetList_Yanbao(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_YANBAO>().AsQueryable();
            }
        }
        #endregion

        #region 会员消费明细
        /// <summary>
        /// 空充调拨
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_VipBuyTimeInfo> Report_VipBuyTimeInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_VipBuyTimeInfo");

                return report.GetList_VipBuy(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_VipBuyTimeInfo>().AsQueryable();
            }
        }
        #endregion

        #region 下载导出
        /// <summary>
        /// 下载导出
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<string> ExportFiles()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_YANBAO");

                return new string[] { "0","1" }.AsQueryable<string>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 借贷审批明细
        /// <summary>
        /// 借贷审批明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_BorrowAduitInfo> Report_BorrowAduitInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_BorrowAduitInfo");
                //this.CurrentDataSource.Report_BorrowAduitInfo.Include("Report_BorrowAduitListInfo");
                return report.GetList_BorrowAduit(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_BorrowAduitInfo>().AsQueryable();
            }
        }
        #endregion

        #region 批发审批明细
        /// <summary>
        /// 批发审批明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.Report_SellAduitInfo> Report_SellAduitInfo()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("Report_SellAduitInfo");

                return report.GetList_SellAduit(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.Report_SellAduitInfo>().AsQueryable();
            }
        }
        #endregion


        #region 借贷申请
        #region 一级待审批
        /// <summary>
        /// 借贷审批明细
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public IQueryable<ReportModel.View_BorowAduit> View_BorowAduit()
        {
            try
            {
                var user = Login(HttpContext.Current.Request.Headers["X-UserName"], HttpContext.Current.Request.Headers["X-Password"], null);


                DAL.AllReportInfo report = new AllReportInfo("View_BorowAduit"); 
                return report.GetList_AduitFirst(user, this.CurrentDataSource);

            }
            catch (Exception ex)
            {
                return new List<ReportModel.View_BorowAduit>().AsQueryable();
            }
        }
        #endregion
        #endregion


        
    }
}



public class JSONPSupportInspector : IDispatchMessageInspector
{
    // Assume utf-8, note that Data Services supports
    // charset negotation, so this needs to be more
    // sophisticated (and per-request) if clients will 
    // use multiple charsets
    private static Encoding encoding = Encoding.UTF8;

    #region IDispatchMessageInspector Members

    public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel, InstanceContext instanceContext)
    {
        if (request.Properties.ContainsKey("UriTemplateMatchResults"))
        {
            HttpRequestMessageProperty httpmsg = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
            UriTemplateMatch match = (UriTemplateMatch)request.Properties["UriTemplateMatchResults"];

            string format = match.QueryParameters["$format"];
            if ("json".Equals(format, StringComparison.InvariantCultureIgnoreCase))
            {
                // strip out $format from the query options to avoid an error
                // due to use of a reserved option (starts with "$")
                match.QueryParameters.Remove("$format");

                // replace the Accept header so that the Data Services runtime 
                // assumes the client asked for a JSON representation
                httpmsg.Headers["Accept"] = "application/json";

                string callback = match.QueryParameters["$callback"];
                if (!string.IsNullOrEmpty(callback))
                {
                    match.QueryParameters.Remove("$callback");
                    return callback;
                }
            }
        }
        return null;
    }

    public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
    {
        if (correlationState != null && correlationState is string)
        {
            // if we have a JSONP callback then buffer the response, wrap it with the
            // callback call and then re-create the response message

            string callback = (string)correlationState;

            XmlDictionaryReader reader = reply.GetReaderAtBodyContents();
            reader.ReadStartElement();
            string content = JSONPSupportInspector.encoding.GetString(reader.ReadContentAsBase64());

            content = callback + "(" + content + ")";

            Message newreply = Message.CreateMessage(MessageVersion.None, "", new Writer(content));
            newreply.Properties.CopyProperties(reply.Properties);

            reply = newreply;
        }
    }

    #endregion

    class Writer : BodyWriter
    {
        private string content;

        public Writer(string content)
            : base(false)
        {
            this.content = content;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("Binary");
            byte[] buffer = JSONPSupportInspector.encoding.GetBytes(this.content);
            writer.WriteBase64(buffer, 0, buffer.Length);
            writer.WriteEndElement();
        }
    }


}
// Simply apply this attribute to a DataService-derived class to get
// JSONP support in that service
[AttributeUsage(AttributeTargets.Class)]
public class JSONPSupportBehaviorAttribute : Attribute, IServiceBehavior
{
    #region IServiceBehavior Members

    void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
    {
    }

    void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
        foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
        {
            foreach (EndpointDispatcher ed in cd.Endpoints)
            {
                ed.DispatchRuntime.MessageInspectors.Add(new JSONPSupportInspector());
            }
        }
    }

    void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
    }

    #endregion
}
