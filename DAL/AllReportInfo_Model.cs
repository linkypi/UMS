using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace DAL
{
    public class AllReportInfo_Model
    {
        /// <summary>
        /// 批发审批明细
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ObjectContext"></param>
        /// <returns></returns>
        public IQueryable<Model.Report_SellAduitInfo> GetList_SellAduit(Model.Sys_UserInfo user, int MenuID)
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
                    { return new List<Model.Report_SellAduitInfo>().AsQueryable(); }

                    #endregion


                    var objSet = lqh.Umsdb.Report_SellAduitInfo.AsQueryable();

                    objSet = from b in objSet
                             where ValidHallIDS.Any(p => p.HallID == b.门店编码)

                             select b;



                    return objSet.OrderBy(p => p.序号);

                }

                catch (Exception ex)
                {
                    return new List<Model.Report_SellAduitInfo>().AsQueryable(); ;

                }
                //}
            }
        }
        /// <summary>
        /// 套餐审批明细
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ObjectContext"></param>
        /// <returns></returns>
        public IQueryable<ReportModel.View_VIPOffListAduitHeader> GetList_SellOffAduit(Model.Sys_UserInfo user, int MenuID)
        {
            
                try
                {

                    //DataLoadOptions d = new DataLoadOptions();
                    //d.LoadWith<ReportModel.View_VIPOffListAduitHeader>(p => p.View_VIPOffListAduit);
                    //d.LoadWith<ReportModel.View_VIPOffListAduit>(p => p.View_Package_GroupInfo);
                    //lqh.Umsdb.LoadOptions = MyOptions;

                    #region 权限
                    //IQueryable<Model.Pro_HallInfo> ValidHallIDS = null;
                    //有权限的商品


                    //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MenuID, out ValidHallIDS, lqh);

                    //if (ret.ReturnValue != true)
                    //{ return new List<ReportModel.View_VIPOffListAduitHeader>().AsQueryable(); }

                    #endregion

                    var entity = new ReportModel.Entities();

                    var objSet = entity.View_VIPOffListAduitHeader.AsQueryable();

                    return objSet.OrderBy(p => p.ID);

                }

                catch (Exception ex)
                {
                    return new List<ReportModel.View_VIPOffListAduitHeader>().AsQueryable(); ;

                }
                //}
            
        }
    }
}
