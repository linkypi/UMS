using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class AllReportInfo
    {

        private string reportViewName;

        public string ReportViewName
        {
            get { return reportViewName; }
            set { reportViewName = value; }
        }
        public AllReportInfo()
        {
            
        }
        public AllReportInfo(string reportViewName)
        {
            this.reportViewName = reportViewName;
        }
        /// <summary>
        /// 成本价格表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ObjectContext"></param>
        /// <returns></returns>
       public IQueryable<ReportModel.Report_CostPriceListInfo> GetList_CostPrice(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               
               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_CostPriceListInfo>().AsQueryable(); }

                   #endregion
                    
              
                   var objSet = ObjectContext.Report_CostPriceListInfo.AsQueryable();
                  
                  
                   return objSet.OrderBy(p=>p.序号);
 
               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_CostPriceListInfo>().AsQueryable(); ;

               }
               //}
           }
       }
       /// <summary>
       /// 销售价格表
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_PriceInfo> GetList_Price(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_PriceInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_PriceInfo.AsQueryable();


                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_PriceInfo>().AsQueryable(); ;

               }
               //}
           }
       }

       /// <summary>
       /// 借贷明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_BorrowInfo> GetList_BorrowList(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_BorrowInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_BorrowInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.借贷仓库编码 equals c.HallID
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
                   return new List<ReportModel.Report_BorrowInfo>().AsQueryable(); ;

               }
               //}
           }
       }
       /// <summary>
       /// 借贷串码明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_BorrowIMEIInfo> GetList_BorrowIMEI(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_BorrowIMEIInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_BorrowIMEIInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.借贷仓库编码 equals c.HallID
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
                   return new List<ReportModel.Report_BorrowIMEIInfo>().AsQueryable(); ;

               }
               //}
           }
       }



       /// <summary>
       /// 归还明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_ReturnListInfo> GetList_ReturnList(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_ReturnListInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_ReturnListInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.借贷仓库编码 equals c.HallID
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
                   return new List<ReportModel.Report_ReturnListInfo>().AsQueryable(); 

               }
               //}
           }
       }
       /// <summary>
       /// 归还串码明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_ReturnIMEIInfo> GetList_ReturnIMEI(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_ReturnIMEIInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_ReturnIMEIInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.借贷仓库编码 equals c.HallID
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
                   return new List<ReportModel.Report_ReturnIMEIInfo>().AsQueryable(); ;

               }
               //}
           }
       }


       /// <summary>
       /// 续期明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_ReNewInfo> GetList_ReturnReNew(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_ReNewInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_ReNewInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.门店编码 equals c.HallID
                            into temp2
                            from c1 in temp2
                            //orderby b.SysDate descending
                            select b;

                   //if(ValidProIDS.Count()>0)
                   //objSet = from b in objSet
                   //         //where ValidProIDS.Contains(b.商品编码) || string.IsNullOrEmpty(b.商品编码)
                   //         join c in ValidProIDS
                   //         on b.类别编码 equals c.ClassID
                   //         into temp2
                   //         from c1 in temp2
                   //         select b;

                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_ReNewInfo>().AsQueryable(); ;

               }
               //}
           }
       }

       /// <summary>
       /// 送修明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_RepairInfo> GetList_ReturnRepair(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_RepairInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_RepairInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.仓库编码 equals c.HallID
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
                   return new List<ReportModel.Report_RepairInfo>().AsQueryable(); ;

               }
               //}
           }
       }
       /// <summary>
       /// 返库明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_RepairReturnInfo> GetList_ReturnRepairReturn(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_RepairReturnInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_RepairReturnInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.仓库编码 equals c.HallID
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
                   return new List<ReportModel.Report_RepairReturnInfo>().AsQueryable(); ;

               }
               //}
           }
       }

       /// <summary>
       /// 打印销售单
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Print_SellInfo> GetList_PrintSell(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Print_SellInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Print_SellInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.门店编码 equals c.HallID
                            into temp2
                            from c1 in temp2
                            //orderby b.SysDate descending
                            select b;

                   //if(ValidProIDS.Count()>0)
                   //objSet = from b in objSet
                   //         //where ValidProIDS.Contains(b.商品编码) || string.IsNullOrEmpty(b.商品编码)
                   //         join c in ValidProIDS
                   //         on b. equals c.ClassID
                   //         into temp2
                   //         from c1 in temp2
                   //         select b;

                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Print_SellInfo>().AsQueryable(); ;

               }
               //}
           }
       }
       /// <summary>
       /// 打印退货单
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Print_SellBackInfo> GetList_PrintBackSell(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Print_SellBackInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Print_SellBackInfo.AsQueryable();
                   objSet = from b in objSet
                            //where ValidHallIDS.Contains(b.门店编码) || string.IsNullOrEmpty(b.门店编码)
                            join c in ValidHallIDS
                            on b.门店编码 equals c.HallID
                            into temp2
                            from c1 in temp2
                            //orderby b.SysDate descending
                            select b;

                   //if(ValidProIDS.Count()>0)
                   //objSet = from b in objSet
                   //         //where ValidProIDS.Contains(b.商品编码) || string.IsNullOrEmpty(b.商品编码)
                   //         join c in ValidProIDS
                   //         on b. equals c.ClassID
                   //         into temp2
                   //         from c1 in temp2
                   //         select b;

                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Print_SellBackInfo>().AsQueryable(); ;

               }
               //}
           }
       }


       /// <summary>
       /// 商品信息
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_ProductInfo> GetList_Product(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_ProductInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_ProductInfo.AsQueryable();
                  
 
                    

                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_ProductInfo>().AsQueryable(); ;

               }
               //}
           }
       }

       /// <summary>
       /// 空充调拨
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_AirOutListInfo> GetList_AirOut(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_AirOutListInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_AirOutListInfo.AsQueryable();

                   objSet = from b in objSet
                            where ValidHallIDS.Any(p => p.HallID == b.调出仓库编码) || ValidHallIDS.Any(p => p.HallID == b.调入仓库编码)
                            //join c in ValidHallIDS
                            //on b.调出仓库 equals c.HallID
                            //into temp2
                            //from c1 in temp2
                            //orderby b.SysDate descending
                            select b;

                   //if(ValidProIDS.Count()>0)
                   //objSet = from b in objSet
                   //         //where ValidProIDS.Contains(b.商品编码) || string.IsNullOrEmpty(b.商品编码)
                   //         join c in ValidProIDS
                   //         on b.类别编码 equals c.ClassID
                   //         into temp2
                   //         from c1 in temp2
                   //         select b;


                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_AirOutListInfo>().AsQueryable(); ;

               }
               //}
           }
       }

       /// <summary>
       /// 广信保销售明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_YANBAO> GetList_Yanbao(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_YANBAO>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_YANBAO.AsQueryable();

                   objSet = from b in objSet
                            where ValidHallIDS.Any(p => p.HallID == b.门店编码)  
                            //join c in ValidHallIDS
                            //on b.调出仓库 equals c.HallID
                            //into temp2
                            //from c1 in temp2
                            //orderby b.SysDate descending
                            select b;

                 


                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_YANBAO>().AsQueryable(); ;

               }
               //}
           }
       }
       /// <summary>
       /// 会员消费明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_VipBuyTimeInfo> GetList_VipBuy(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_VipBuyTimeInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_VipBuyTimeInfo.AsQueryable();

                   objSet = from b in objSet
                            where ValidHallIDS.Any(p => p.HallID == b.门店编码)
                            //join c in ValidHallIDS
                            //on b.调出仓库 equals c.HallID
                            //into temp2
                            //from c1 in temp2
                            //orderby b.SysDate descending
                            select b;




                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_VipBuyTimeInfo>().AsQueryable(); ;

               }
               //}
           }
       }


       /// <summary>
       /// 借贷审批明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_BorrowAduitInfo> GetList_BorrowAduit(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_BorrowAduitInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_BorrowAduitInfo.AsQueryable();

                   objSet = from b in objSet
                            where ValidHallIDS.Any(p => p.HallID == b.门店编码) 
                         
                            select b;

      

                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_BorrowAduitInfo>().AsQueryable(); ;

               }
               //}
           }
       }
       /// <summary>
       /// 一级未审批借贷明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_BorrowAduitInfo> GetList_BorrowAduit1(Model.Sys_UserInfo user, int MenuID, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<Model.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品


                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MenuID, out ValidHallIDS, lqh);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_BorrowAduitInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_BorrowAduitInfo.AsQueryable();

                   objSet = from b in objSet
                            where ValidHallIDS.Any(p => p.HallID == b.门店编码)

                            select b;



                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_BorrowAduitInfo>().AsQueryable(); ;

               }
               //}
           }
       }

       /// <summary>
       /// 批发审批明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_SellAduitInfo> GetList_SellAduit(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_SellAduitInfo>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.Report_SellAduitInfo.AsQueryable();

                   objSet = from b in objSet
                            where ValidHallIDS.Any(p => p.HallID == b.门店编码)

                            select b;



                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_SellAduitInfo>().AsQueryable(); ;

               }
               //}
           }
       }
       /// <summary>
       /// 一级待审批
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.View_BorowAduit> GetList_AduitFirst(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.View_BorowAduit>().AsQueryable(); }

                   #endregion


                   var objSet = ObjectContext.View_BorowAduit.AsQueryable();

                   objSet = from b in objSet

                            where ValidHallIDS.Any(p => p.HallID == b.HallID)

                            select b;



                   return objSet.OrderBy(p => p.ApplyDate);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.View_BorowAduit>().AsQueryable(); ;

               }
               //}
           }
       }
       /// <summary>
       /// 获取固定资产调拨单，包括固定资产串码明细
       /// </summary>
       /// <param name="user"></param>
       /// <param name="ObjectContext"></param>
       /// <returns></returns>
       public IQueryable<ReportModel.Report_OutInfo> GetList_AssetsOut(Model.Sys_UserInfo user, ReportModel.Entities ObjectContext)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {

               try
               {

                   #region 权限
                   IQueryable<ReportModel.Pro_HallInfo> ValidHallIDS = null;
                   //有权限的商品
                   IQueryable<ReportModel.Pro_ClassInfo> ValidProIDS = null;

                   Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.ReportViewName, out ValidHallIDS, out ValidProIDS, ObjectContext);

                   if (ret.ReturnValue != true)
                   { return new List<ReportModel.Report_OutInfo>().AsQueryable(); }

                   #endregion

                   ObjectContext.Report_OutInfo.Include("Report_OutOrderIMEIInfo");
                   var objSet = ObjectContext.Report_OutInfo.AsQueryable();
                   
                   objSet = from b in objSet
                            //where ValidHallIDS.Any(p => p.HallID == b.调出仓库编码) || ValidHallIDS.Any(p => p.HallID == b.调入仓库编码)
                            join c in ObjectContext.Sys_Option
                            on b.调出仓库编码 equals c.Value2
                            where c.ClassName == "AssetsHallID"
                            //into temp2
                            //from c1 in temp2
                            //orderby b.SysDate descending
                            select b;
 

                   return objSet.OrderBy(p => p.序号);

               }

               catch (Exception ex)
               {
                   return new List<ReportModel.Report_OutInfo>().AsQueryable(); ;

               }
               //}
           }
       }
    }
}
