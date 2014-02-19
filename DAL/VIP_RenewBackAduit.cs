using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class VIP_RenewBackAduit
    {
         private int MethodID;
         public VIP_RenewBackAduit()
	    {
		    this.MethodID = 0;
	    }

         public VIP_RenewBackAduit(int MenthodID)
	    {
		    this.MethodID = MenthodID;
	    }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         public Model.WebReturn Add(Model.Sys_UserInfo user, Model.VIP_RenewBackAduit model)
        {
           
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        {
                            return result;
                        }

                        #region "验证用户操作仓库  商品的权限 "
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                        }

                        //验证商品权限
                        //if (ValidProIDS.Count() > 0)
                        //{
                        //    List<string> proids = new List<string>();
                        //    foreach (var item in model.Pro_OutOrderList)
                        //    {
                        //        proids.Add(item.ProID.ToString());
                        //    }
                        //    foreach (var child in proids)
                        //    {
                        //        if (!ValidProIDS.Contains(child))
                        //        {
                        //            var que = from h in lqh.Umsdb.Pro_ProInfo
                        //                      where h.ProID == child
                        //                      select h;
                        //            return new Model.WebReturn() { ReturnValue = false, Message = "商品" + que.First().ProName + "无权操作" };
                        //        }
                        //    }
                        //}
                        #endregion

                        string aduitid = "";
                        lqh.Umsdb.OrderMacker(1, "RNA", "RNA", ref aduitid);
                        if (string.IsNullOrEmpty(aduitid))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单生成出错" };
                        }
                        model.AduitID = aduitid;
                        model.Aduited = false;
                        model.Passed = false;
                        model.Used = false;

                        lqh.Umsdb.VIP_RenewBackAduit.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true,Obj = model.AduitID};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                    }
                }
            }
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
         public Model.WebReturn Aduit(Model.Sys_UserInfo user, List<Model.VIP_RenewBackAduit> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper()) 
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        {
                            return result;
                        }

                        #region "验证用户操作仓库  商品的权限 "

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS,lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        if (ValidHallIDS.Count > 0)
                        {
                            List<string> hallidlist = new List<string>();
                            foreach (var item in models)
                            {
                                var query = from aduit in lqh.Umsdb.VIP_RenewBackAduit
                                            where aduit.ID == item.ID
                                            select aduit;
                                hallidlist.Add(query.First().HallID);
                            }
                            foreach (var item in hallidlist)
                            {
                                if (!ValidHallIDS.Contains(item))
                                {
                                    var que = from h in lqh.Umsdb.Pro_HallInfo
                                              where h.HallID == item
                                              select h;
                                    return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                                }
                            }
                        }
                        #endregion

                        foreach (var item in models)
                        {
                            var query = from aduit in lqh.Umsdb.VIP_RenewBackAduit
                                        where aduit.ID == item.ID
                                        select aduit;
                            if (query.Count() != 0)
                            {
                                query.First().AduitDate = item.AduitDate;
                                query.First().AduitUser = item.AduitUser;
                                query.First().Aduited = item.Aduited;
                                query.First().Note = item.Note;

                                query.First().Money = item.Money;
                                query.First().Point = item.Point;
                                query.First().Validity = item.Validity;
                                query.First().Passed = item.Passed;
                                lqh.Umsdb.SubmitChanges();
                            }
                        }
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                    }
                }
            }
          
        }

        /// <summary>
        /// 获取待审批列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="countOnePage"></param>
        /// <returns></returns>
         public Model.WebReturn GetList(Model.Sys_UserInfo user, int pageIndex, int countOnePage)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    int? total = 0;
                    if (pageIndex == 0)
                    {
                        pageIndex = 1;
                    }
                    List<Model.GetRenewBackAduitListResult> results =
                        lqh.Umsdb.GetRenewBackAduitList(pageIndex, countOnePage, ref total).ToList();

                       if (results.Count() != 0)
                     {
                        List<Model.AduitModel> models = new List<Model.AduitModel>();
                        Model.AduitModel am = null;

                        foreach (var item in results)
                        {
                            am = new Model.AduitModel();
                            am.ID = int.Parse(item.ID.ToString());
                            am.AduitID = item.AduitID;
                            am.ApplyUser = item.ApplyUser.ToString();
                            am.MemberName = item.MemberName;
                            am.Validity =int.Parse( item.Validity.ToString());
                            am.ApplyDate = DateTime.Parse(item.ApplyDate.ToString());
                            am.HallID = item.HallID;
                            am.HallName = item.HallName;
                            am.Money = decimal.Parse(item.Money.ToString());
                            models.Add(am);
                        }
                        ArrayList arrList = new ArrayList();
                        arrList.Add(total);
                        return new Model.WebReturn() { Obj = models, ReturnValue = true ,ArrList = arrList};
                    }
                       return new Model.WebReturn() { ReturnValue = false };
                }
                catch(Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false};
                }
            }
        }

         /// <summary>
         /// 查询
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

                     Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                     if (!result.ReturnValue)
                     { return new WebReturn() { ReturnValue = false, Obj = pageParam }; }

                     #region 权限

                     List<string> ValidHallIDS = new List<string>();
                     //有权限的商品
                     List<string> ValidProIDS = new List<string>();

                     Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                     if (ret.ReturnValue != true)
                     { return ret; }

                     #endregion

                     if (pageParam == null || pageParam.PageIndex < 0 )
                     {
                         return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                     }
                     if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                     #region "过滤数据"

                     var aduit_query = from b in lqh.Umsdb.View_VIP_RenewBackAduit
                                       where b.Flag==true
                                       select b;
                     foreach (var item in pageParam.ParamList)
                     {
                         switch (item.ParamName)
                         {
                             case "Aduited":
                                 Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)item;

                                 aduit_query = from b in aduit_query
                                               where b.Aduited == mm.ParamValues
                                               select b;
                                 break;

                             case "Passed":
                                 Model.ReportSqlParams_String pass = (Model.ReportSqlParams_String)item;

                                 aduit_query = from b in aduit_query
                                               where b.Passed == pass.ParamValues
                                               select b;
                                 break;

                             case "Used":
                                 Model.ReportSqlParams_String use = (Model.ReportSqlParams_String)item;
                                 aduit_query = from b in aduit_query
                                               where b.Used == use.ParamValues
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

                             case "HallID":
                                 Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                 if (para1.ParamValues != null)
                                 {
                                     aduit_query = from b in aduit_query
                                                   where para1.ParamValues.Contains(b.HallID)
                                                   select b;

                                 }
                                 break;

                             case "IMEI":
                                 Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                 if (para.ParamValues != null)
                                 {
                                     aduit_query = from b in aduit_query
                                                   where b.IMEI.Contains(para.ParamValues)
                                                   select b;
                                 }
                                 break;
                             case "MobiPhone":
                                 Model.ReportSqlParams_String mobi = (Model.ReportSqlParams_String)item;
                                 if (mobi.ParamValues != null)
                                 {
                                     aduit_query = from b in aduit_query
                                                   where mobi.ParamValues.Contains(b.MobiPhone)
                                                   select b;
                                 }
                                 break;

                             case "IDCard":
                                 Model.ReportSqlParams_String cardid = (Model.ReportSqlParams_String)item;
                                 if (cardid.ParamValues != null)
                                 {
                                     aduit_query = from b in aduit_query
                                                   where b.IDCard.Contains(cardid.ParamValues)
                                                   select b;
                                 }
                                 break;
                             case "MemberName":
                                 Model.ReportSqlParams_String name = (Model.ReportSqlParams_String)item;
                                 if (name.ParamValues != null)
                                 {
                                     aduit_query = from b in aduit_query
                                                   where b.MemberName.Contains(name.ParamValues)
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
                                       orderby b.ApplyDate descending
                                       select b;
                     }
                     else
                     {
                         aduit_query = from b in aduit_query
                                       orderby b.ApplyDate descending
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

                         List<Model.View_VIP_RenewBackAduit> aduitList = results.ToList();

                         pageParam.Obj = aduitList;
                         return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                     }

                     else
                     {
                         var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                       select a;

                         List<Model.View_VIP_RenewBackAduit> aduitList = results.ToList();

                         pageParam.Obj = aduitList;
                         return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                     }
                     #endregion
                 }
                 catch (Exception ex)
                 {
                     return new WebReturn() { ReturnValue = false, Message = ex.Message };
                 }
             }
         }

        /// <summary>
        /// 删除取消续期申请
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
         public Model.WebReturn Delete(Model.Sys_UserInfo user, int id)
         {
             using (LinQSqlHelper lqh = new LinQSqlHelper())
             {
                 using (TransactionScope ts = new TransactionScope())
                 {
                     try
                     {
                         Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                         if (!result.ReturnValue)
                         {
                             return result;
                         }

                         var aduit = from a in lqh.Umsdb.VIP_RenewBackAduit
                                     where a.ID == id
                                     select a;

                         if (aduit.Count() == 0)
                         {
                             return new Model.WebReturn() { ReturnValue = false, Message = "该条记录已删除，删除失败！" };
                         }
                         Model.VIP_RenewBackAduit model = aduit.First();
                         if (Convert.ToBoolean(model.Used))
                         {
                             return new Model.WebReturn() { ReturnValue = false, Message = "该审批单已使用，删除失败！" };
                         }

                         lqh.Umsdb.VIP_RenewBackAduit.DeleteOnSubmit(model);

                         Model.VIP_RenewBackAduit_bak sa = new Model.VIP_RenewBackAduit_bak();
                         sa.AduitDate = model.AduitDate;
                         sa.Aduited = model.Aduited;
                         sa.AduitID = model.AduitID;
                         sa.AduitUser = model.AduitUser;
                         sa.ApplyDate = model.ApplyDate;
                         sa.ApplyUser = model.ApplyUser;
                         sa.NewDate = model.NewDate;
                         sa.Point = model.Point;
                         sa.ReNewID = model.ReNewID;
                         sa.User = model.User;
                         sa.UserID = model.UserID;
                         sa.Validity = model.Validity;
                         sa.VIP_ID = model.VIP_ID;
                        
                         sa.HallID = model.HallID;
                         sa.ID = model.ID;
                         sa.Money = model.Money;
                         sa.Note = model.Note;
                         sa.Passed = model.Passed;
                         sa.SysDate = model.SysDate;
                         sa.Used = model.Used;
                         sa.UseDate = model.UseDate;

                         lqh.Umsdb.VIP_RenewBackAduit_bak.InsertOnSubmit(sa);
                         lqh.Umsdb.SubmitChanges();
                         ts.Complete();
                         return new Model.WebReturn() { ReturnValue = true, Message = "删除成功！" };
                     }
                     catch (Exception ex)
                     {
                         return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                     }
                 }
             }
         }
    }
}
