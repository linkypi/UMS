using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
   public  class VIP_VIPBackAduit
    {
         private int MethodID;

	    public VIP_VIPBackAduit()
	    {
		    this.MethodID = 0;
	    }

        public VIP_VIPBackAduit(int MenthodID)
	    {
		    this.MethodID = MenthodID;
	    }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.VIP_VIPBackAduit model)
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

                        #region 验证权限

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS,lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        //有仓库限制，而且仓库不在权限范围内
                  
                        if (ValidHallIDS.Count>0&& !ValidHallIDS.Contains(model.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                        }
                        #endregion

                        string aduitid = "";
                        lqh.Umsdb.OrderMacker(1, "VBA", "VBA", ref aduitid);
                        if (aduitid == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单生成出错" };
                        }
                        model.AduitID = aduitid;
                        model.Aduited = false;
                        model.Used = false;
                        //model.Passed = false;

                        lqh.Umsdb.VIP_VIPBackAduit.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Obj = model.AduitID };
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
        public Model.WebReturn Aduit(Model.Sys_UserInfo user,Model.VIP_VIPBackAduit model)
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

                        #region 验证权限

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS,lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        //有仓库限制，而且仓库不在权限范围内

                        if (ValidHallIDS.Count>0&& !ValidHallIDS.Contains(model.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                        }
                        #endregion

                        var query = from aduit in lqh.Umsdb.VIP_VIPBackAduit
                                    where aduit.ID == model.ID
                                    select aduit;

                        if (query.Count() != 0)
                        {
                            query.First().AduitDate = model.AduitDate;
                            query.First().AduitUser = model.AduitUser;
                            query.First().Aduited = model.Aduited;
                            query.First().Note = model.Note;
                            //query.First().Passed = model.Passed;
                            lqh.Umsdb.SubmitChanges();
                        }
                        
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false ,Message=ex.Message};
                    }
                }
            }

        }

        #region 获取审批单（退卡的时候使用，请不要删除）
        public Model.WebReturn GetModel(Model.Sys_UserInfo user,Model.VIP_VIPBackAduit Aduit)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.VIP_VIPBackAduit
                                where b.AduitID == Aduit.AduitID
                                select b;
                    if (query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "不存在或审批单已使用" };
                    }
                    var query_VIP = from b in lqh.Umsdb.VIP_VIPBackAduit
                                where b.VIP_ID == Aduit.VIP_ID
                                select b;
                    if (query_VIP.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批单不属于该会员" };
                    }
                    return new Model.WebReturn() { Obj = query.First(), ReturnValue = true, Message = "已返回" };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取失败" };
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取待审批列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user, int pageIndex, int pageSize)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    int? total = 0;
                    List<Model.GetVIPBackAduitListResult> results =
                        lqh.Umsdb.GetVIPBackAduitList(pageIndex, pageSize, ref total).ToList();

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
                            am.ApplyDate = DateTime.Parse(item.ApplyDate.ToString());
                            am.HallID = item.HallID;
                            am.HallName = item.HallName;
                            am.Money = decimal.Parse(item.Money.ToString());
                            models.Add(am);
                        }
                        ArrayList arrList = new ArrayList();
                        arrList.Add(total);
                        return new Model.WebReturn() { Obj = models, ReturnValue = true, ArrList = arrList };
                    }
                    return new Model.WebReturn() { ReturnValue = false };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false };
                }
            }
        }


       public Model.WebReturn ApplySearch(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
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

                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var back_query = from b in lqh.Umsdb.View_VIPBackApply
                                      select b;

                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "Applyed":

                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)item;
                                if (mm.ParamValues == "Yes")
                                {
                                    back_query = from re in back_query
                                                 where re.Applyed=="Y"
                                                  select re;
                                }
                                else
                                {
                                    back_query = from re in back_query
                                                 where re.Applyed == "N"
                                                  select re;
                                }
                                break;

                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                if (mm5.ParamValues != null)
                                {
                                    back_query = from b in back_query
                                                  where b.SysDate >= mm5.ParamValues
                                                  select b;
                                }
                                break;
                            case "EndTime":
                                Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)item;
                                if (mm6.ParamValues != null)
                                {
                                    back_query = from b in back_query
                                                 where b.SysDate <= Convert.ToDateTime(mm6.ParamValues).AddDays(1)
                                                 select b;
                                }
                                break;

                            case "IMEI":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    back_query = from b in back_query
                                                  where b.IMEI.Contains(para.ParamValues)
                                                  select b;
                                }
                                break;
                            case "MobiPhone":
                                Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                if (para1.ParamValues != null)
                                {
                                    back_query = from b in back_query
                                                  where para1.ParamValues.Contains(b.MobiPhone)
                                                  select b;
                                }
                                break;

                            case "IDCard":
                                Model.ReportSqlParams_String para2 = (Model.ReportSqlParams_String)item;
                                if (para2.ParamValues != null)
                                {
                                    back_query = from b in back_query
                                                  where b.IDCard.Contains(para2.ParamValues)
                                                  select b;
                                }
                                break;
                            case "MemberName":
                                Model.ReportSqlParams_String name = (Model.ReportSqlParams_String)item;
                                if (name.ParamValues != null)
                                {
                                    back_query = from b in back_query
                                                  where b.MemberName.Contains(name.ParamValues)
                                                  select b;
                                }
                                break;
                        }
                    }

                    #endregion

                    #region 过滤仓库

                    //if (ValidHallIDS.Count() > 0)
                    //{
                    //    renew_query = from b in renew_query
                    //                    where ValidHallIDS.Contains(b.HallID)
                    //                    orderby b.SysDate descending
                    //                    select b;
                    //}
                    //else
                    //{
                    //    renew_query = from b in renew_query
                    //                    orderby b.SysDate descending
                    //                    select b;
                    //}
                    #endregion

                    pageParam.RecordCount = back_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from r in back_query.Take(pageParam.PageSize).ToList()
                                      //  where r.UserID == user.UserID && r.UserID == user.UserID
                                      select r;

                        List<Model.View_VIPBackApply> list = results.ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    else
                    {
                        var results = from r in back_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      //where r.UserID == user.UserID && r.UserID == user.UserID
                                      select r;

                        List<Model.View_VIPBackApply> list = results.ToList();
                        pageParam.Obj = list;
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

       public Model.WebReturn AduitSearch(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
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

                   var back_query = from b in lqh.Umsdb.View_VIPBackAduit
                                    select b;

                   foreach (var item in pageParam.ParamList)
                   {
                       switch (item.ParamName)
                       {
                           case "Aduited":
                               Model.ReportSqlParams_String aduit = (Model.ReportSqlParams_String)item;

                               back_query = from b in back_query
                                             where b.Aduited == aduit.ParamValues
                                             select b;
                               break;

                           case "Used":
                               Model.ReportSqlParams_String use = (Model.ReportSqlParams_String)item;

                               back_query = from b in back_query
                                             where b.Used == use.ParamValues
                                             select b;
                               break;

                           case "StartTime":
                               Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                               if (mm5.ParamValues != null)
                               {
                                   back_query = from b in back_query
                                                where b.SysDate >= mm5.ParamValues
                                                select b;
                               }
                               break;
                           case "EndTime":
                               Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)item;
                               if (mm6.ParamValues != null)
                               {
                                   back_query = from b in back_query
                                                where b.SysDate <= Convert.ToDateTime(mm6.ParamValues).AddDays(1)
                                                select b;
                               }
                               break;

                           case "IMEI":
                               Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                               if (para.ParamValues != null)
                               {
                                   back_query = from b in back_query
                                                where b.IMEI.Contains(para.ParamValues)
                                                select b;
                               }
                               break;
                           case "MobiPhone":
                               Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                               if (para1.ParamValues != null)
                               {
                                   back_query = from b in back_query
                                                where para1.ParamValues.Contains(b.MobiPhone)
                                                select b;
                               }
                               break;

                           case "IDCard":
                               Model.ReportSqlParams_String para2 = (Model.ReportSqlParams_String)item;
                               if (para2.ParamValues != null)
                               {
                                   back_query = from b in back_query
                                                where b.IDCard.Contains(para2.ParamValues)
                                                select b;
                               }
                               break;
                           case "MemberName":
                               Model.ReportSqlParams_String name = (Model.ReportSqlParams_String)item;
                               if (name.ParamValues != null)
                               {
                                   back_query = from b in back_query
                                                where b.MemberName.Contains(name.ParamValues)
                                                select b;
                               }
                               break;
                       }
                   }

                   #endregion

                   pageParam.RecordCount = back_query.Count();

                   #region 判断是否超过总页数

                   int pagecount = pageParam.RecordCount / pageParam.PageSize;

                   if (pageParam.PageIndex > pagecount)
                   {
                       pageParam.PageIndex = 0;
                       var results = from r in back_query.Take(pageParam.PageSize).ToList()
                                     //  where r.UserID == user.UserID && r.UserID == user.UserID
                                     select r;

                       List<Model.View_VIPBackAduit> list = results.ToList();
                       pageParam.Obj = list;
                       return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                   }
                   else
                   {
                       var results = from r in back_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                     //where r.UserID == user.UserID && r.UserID == user.UserID
                                     select r;

                       List<Model.View_VIPBackAduit> list = results.ToList();
                       pageParam.Obj = list;
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

    }
}
