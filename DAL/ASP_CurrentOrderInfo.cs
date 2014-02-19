using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class ASP_CurrentOrderInfo
    {
          private int MethodID;

        public ASP_CurrentOrderInfo()
        {
            this.MethodID = 0;
        }

        public ASP_CurrentOrderInfo(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        /// 321
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn Search(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region 权限

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return new WebReturn() { ReturnValue = false, Obj = pageParam }; }

                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var aduit_query = from b in lqh.Umsdb.View_ASPCurrentOrderInfo
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "Repairer":
                                Model.ReportSqlParams_String Repairer = (Model.ReportSqlParams_String)item;
                                 aduit_query = from b in aduit_query
                                               where b.Repairer==Repairer.ParamValues
                                                  select b;
                              
                                break;

                            case "HasRepaired":
                                Model.ReportSqlParams_Bool HasRepaired = (Model.ReportSqlParams_Bool)item;
                                if (HasRepaired.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasRepaired == HasRepaired.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasRepaired == null || b.HasRepaired == false
                                                  select b;
                                }
                                break;

                            //case "UnToFactOrToFacUnBack":

                            //    aduit_query = from b in aduit_query
                            //                  where ((b.IsToFact == null || b.IsToFact == false) && b.HasAudited != true)
                            //                  || (b.IsToFact == true && b.IsBack == true)
                            //                  select b;

                            //    break;
                            case "SysDate":
                                Model.ReportSqlParams_DataTime mm = (Model.ReportSqlParams_DataTime)item;

                                aduit_query = from b in aduit_query
                                                where b.SysDate >= mm.ParamValues
                                                select b;
                               
                                break;
                            case "OldID":
                                Model.ReportSqlParams_String pass = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                                where b.OldID.Contains(pass.ParamValues)
                                                select b;
                                break;
                            case "HallID":
                                Model.ReportSqlParams_String pass1 = (Model.ReportSqlParams_String)item;

                               
                                aduit_query = from b in aduit_query
                                                where b.HallID == pass1.ParamValues
                                                select b;
                              
                                break;
                           
                            case "Cus_Name":
                                Model.ReportSqlParams_String mm2 = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                                where b.Cus_Name.Contains(mm2.ParamValues)
                                                select b;
                                break;

                            case "Cus_Phone":
                                Model.ReportSqlParams_String pass2 = (Model.ReportSqlParams_String)item;

                               
                                aduit_query = from b in aduit_query
                                              where b.Cus_Phone.Contains( pass2.ParamValues)
                                                select b;
                              
                                break;

                            case "Pro_IMEI":
                                Model.ReportSqlParams_String imei = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.Pro_IMEI == imei.ParamValues
                                              select b;
                                break;
                           
                            case "VIP_IMEI": //会员卡号
                                Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.IMEI == mm3.ParamValues
                                              select b;
                                break;

                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                if (mm5.ParamValues != null)
                                {
                                    Model.ReportSqlParams_DataTime mm6 = new ReportSqlParams_DataTime();
                                    mm6.ParamValues = DateTime.Now;
                                    foreach (var xxd in pageParam.ParamList)
                                    {
                                        if (xxd.ParamName == "EndTime")
                                        {
                                            mm6 = (Model.ReportSqlParams_DataTime)xxd;
                                            break;
                                        }
                                    }
                                    if (mm5.ParamValues == mm6.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.SysDate >= mm5.ParamValues &&
                                                      b.SysDate < DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.SysDate >= mm5.ParamValues
                                                      && b.SysDate <= DateTime.Parse(mm6.ParamValues.ToString()).AddDays(1)
                                                      select b;
                                    }
                                }
                                break;

                            case "EndTime":
                                Model.ReportSqlParams_DataTime ed = (Model.ReportSqlParams_DataTime)item;
                                if (ed.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.SysDate < DateTime.Parse(ed.ParamValues.ToString()).AddDays(1)
                                                  select b;
                                }
                                break;
                        }
                    }

                    #endregion

                    #region 过滤仓库

                    if (ValidHallIDS.Count() > 0)
                    {
                        aduit_query = from b in aduit_query
                                      where ValidHallIDS.Contains(b.HallID)
                                      orderby b.SysDate descending
                                      select b;
                    }
                    else
                    {
                        aduit_query = from b in aduit_query
                                      orderby b.SysDate descending
                                      select b;
                    }
                    #endregion
                    pageParam.RecordCount = aduit_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in aduit_query.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPCurrentOrderInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPCurrentOrderInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion

                    return new Model.WebReturn() {ReturnValue  =true };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() {Message=ex.Message,ReturnValue  =false };   
                }
            }

        }

        /// <summary>
        /// 322
        /// </summary>
        /// <param name="user"></param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public Model.WebReturn GetErrorInfo(Model.Sys_UserInfo user,int orderid,bool hasBJ )
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var order = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                where a.ID == orderid
                                select a;
                    if (order.Count() == 0)
                    {
                        return new WebReturn() { ReturnValue = false, Message = "" };
                    }
                    int rid = Convert.ToInt32(order.First().CurrentRepairID);

                    List<Model.View_BJModels> bjlist = new List<View_BJModels>();
                    List<Model.ASP_ErrorInfo> errlist = new List<Model.ASP_ErrorInfo>();
                    List<View_ASPCurrentOrderPros> pros = new List<View_ASPCurrentOrderPros>();

                    if (rid > 0) //若已经存在维修单则取维修单数据
                    {
                        var list = from a in lqh.Umsdb.ASP_CurrentOrder_ErrorInfo
                                   join b in lqh.Umsdb.ASP_ErrorInfo on a.ErrorID equals b.ID
                                   where a.RepairID == rid
                                   select b;

                        var bjinfo = from a in lqh.Umsdb.View_BJModels
                                     where a.RepairID==rid
                                     select a;

                        var pjinfo = from a in lqh.Umsdb.View_ASPCurrentOrderPros
                                     where a.RepairID == rid
                                     select a;
                        errlist = list.Count() == 0 ? new List<Model.ASP_ErrorInfo>() : list.ToList();
                        bjlist = bjinfo.Count() == 0 ? new List<Model.View_BJModels>() : bjinfo.ToList();
                        pros = pjinfo.Count()==0?new  List<Model.View_ASPCurrentOrderPros>():pjinfo.ToList();
                    }
                    else  //否则去原始单数据
                    {
                        var list = from a in lqh.Umsdb.ASP_CurrentOrder_ErrorInfo
                                   join b in lqh.Umsdb.ASP_ErrorInfo on a.ErrorID equals b.ID
                                   where a.OrderID == orderid
                                   select b;

                        var bjinfo = from a in lqh.Umsdb.View_BJModels
                                     join b in lqh.Umsdb.ASP_CurrentOrderInfo
                                     on a.OrderID equals b.ID
                                     where a.OrderID == orderid && b.HasBJ == true && a.RepairID==null
                                     select a;

                        var pjinfo = from a in lqh.Umsdb.View_ASPCurrentOrderPros
                                     where a.OrderID == orderid && a.RepairID == null
                                     select a;
                        errlist = list.Count() == 0 ? new List<Model.ASP_ErrorInfo>() : list.ToList();
                        bjlist = bjinfo.Count() == 0 ? new List<Model.View_BJModels>() : bjinfo.ToList();
                        pros = pjinfo.Count() == 0 ? new List<Model.View_ASPCurrentOrderPros>() : pjinfo.ToList();
                    }

                    //foreach (var item in pros)
                    //{
                    //    var need = from a in lqh.Umsdb.Pro_ProInfo
                    //               where item.ProID == item.ProID
                    //               select a;
                    //    if (need.Count() > 0)
                    //    {
                    //        item.NeedIMEI = need.First().NeedIMEI;
                    //    }
                    //}
                    return new WebReturn() { ReturnValue = true, Obj = errlist, ArrList = new System.Collections.ArrayList() { bjlist,pros } };
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue = false,Message= ex.Message};
                }
               
            }
        }


        /// <summary>
        /// 删除受理单  340
        /// </summary>
        /// <param name="user"></param>
        /// <param name="imei"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, List<Model.View_ASPReceiveInfo> recInfo)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var ids = (from a in recInfo
                                  select a.ID).ToList();

                        var list = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                   where ids.Contains((int)a.ReceiveID) 
                                   select a;

                        if (list.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue =false,Message="未能找到指定受理单，删除失败！"};
                        }

                        foreach (var item in list)
                        {
                            if (item.CurrentRepairID >0)
                            {
                                var ac  = from a in lqh.Umsdb.ASP_ReceiveInfo
                                         where a.ID == item.ReceiveID
                                         select a ;
                               
                                return new WebReturn() { ReturnValue = false, Message = "受理单 "+ac.First().ServiceID+" 已使用，删除失败！" };
                            }
                        }

                        var backup = from a in lqh.Umsdb.ASP_CurrentOrder_BackupPhoneInfo
                                  join b in lqh.Umsdb.ASP_CurrentOrderInfo on a.OrderID equals b.ID
                                  where ids.Contains(b.ID) 
                                  select a;

                        var err = from a in lqh.Umsdb.ASP_CurrentOrder_ErrorInfo
                                  join b in lqh.Umsdb.ASP_CurrentOrderInfo on a.OrderID equals b.ID
                                  where ids.Contains(b.ID) 
                                  select a;

                        #region 标记受理单已删除

                        var rec = from a in lqh.Umsdb.ASP_ReceiveInfo
                                  join b in list
                                  on a.ID equals b.ReceiveID
                                  select a;
                        if (rec.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "未能找到指定受理单，删除失败！" };
                        }
                        else
                        {
                            foreach (var item in rec)
                            {
                                item.Flag = false;
                            }
                        }
                        #endregion

                        lqh.Umsdb.ASP_CurrentOrder_BackupPhoneInfo.DeleteAllOnSubmit(backup);
                        lqh.Umsdb.ASP_CurrentOrder_ErrorInfo.DeleteAllOnSubmit(err);
                        lqh.Umsdb.ASP_CurrentOrderInfo.DeleteAllOnSubmit(list);

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue = true, Message = "删除成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }

            }
        }

    }
}
