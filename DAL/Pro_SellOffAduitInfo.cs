using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public  class Pro_SellOffAduitInfo
    {
        private int _MethodID;
   
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Pro_SellOffAduitInfo()
        {
            this.MethodID = 0;
        }

        public Pro_SellOffAduitInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        /// <summary>
        /// 查询审批单
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

                    var cuser = from a in lqh.Umsdb.Sys_UserInfo
                                where a.UserID == user.UserID
                                select a;

                    if (cuser.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = pageParam };
                    }
                    user = cuser.First();

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    {
                        return result;
                    }

                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品     无须商品权限
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

                    var aduit_query = from b in lqh.Umsdb.View_SellOffAduitInfo
                                      where b.IsAduited + "" == "N" //&& ValidHallIDS.Contains(b.HallID)
                                      select b;
                    var Hall_Role=from b in lqh.Umsdb.Sys_Role_Menu_HallInfo
                                  select b;
                    var Users=from b in lqh.Umsdb.Sys_UserInfo 
                              select b;

                    aduit_query = from a in aduit_query
                                      where  (user.IsDefault==true  && ( a.NextPrice==0 || 
                                      (!Users.Any(p => p.AuditOffPrice == a.NextPrice &&
                                          Hall_Role.Any(x => x.HallID == a.HallID && p.RoleID == x.RoleID)))))
                                          || a.NextPrice ==user.AuditOffPrice
                                      select a;
                    
                

                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                  
                            case "ApplyUser":
                                Model.ReportSqlParams_String bt = (Model.ReportSqlParams_String)item;
                                if (bt.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.ApplyUser.Contains(bt.ParamValues)
                                                  select b;
                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_String para1 = (Model.ReportSqlParams_String)item;
                                if (para1.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HallID.Contains(para1.ParamValues)
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

                        List<Model.View_SellOffAduitInfo> list = results.ToList();

                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()

                                      select a;
                        List<Model.View_SellOffAduitInfo> list = results.ToList();

                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }


        /// <summary>
        /// 查询审批单
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn CancelSearch(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region 权限
                    var cuser = from a in lqh.Umsdb.Sys_UserInfo
                                where a.UserID == user.UserID
                                select a;

                    if (cuser.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = pageParam };
                    }
                    user = cuser.First();

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    {
                        return result;
                    }

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

                    var aduit_query = from b in lqh.Umsdb.View_SellOffAduitInfo
                                // where b.ApplyUser == user.UserName
                                      select b;

                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                if (mm5.ParamValues != null)
                                {
                                    Model.ReportSqlParams_DataTime mm6 = new Model.ReportSqlParams_DataTime();
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
                                                       where Convert.ToDateTime(b.ApplyDate) >= mm5.ParamValues &&
                                                       Convert.ToDateTime(b.ApplyDate) < DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                                                       select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where Convert.ToDateTime(b.ApplyDate) >= mm5.ParamValues && Convert.ToDateTime(b.ApplyDate) <= mm6.ParamValues
                                                       select b;
                                    }
                                }
                                break;
                       

                            case "IsAduited":
                                Model.ReportSqlParams_String at = (Model.ReportSqlParams_String)item;
                                if (at.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsAduited.Contains(at.ParamValues)
                                                  select b;
                                }

                                break;
                            case "ApplyUser":
                                Model.ReportSqlParams_String bt = (Model.ReportSqlParams_String)item;
                                if (bt.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.ApplyUser.Contains(bt.ParamValues)
                                                  select b;
                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_String para1 = (Model.ReportSqlParams_String)item;
                                if (para1.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HallID.Contains(para1.ParamValues)
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

                        List<Model.View_SellOffAduitInfo> models = results.ToList();

                        pageParam.Obj = models;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()

                                      select a;
                        List<Model.View_SellOffAduitInfo> models = results.ToList();

                        pageParam.Obj = models;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }

        /// <summary>
        /// 获取审批明细
        /// </summary>
        /// <param name="user"></param>
        /// <param name="aduitID"></param>
        /// <param name="isSellAduitID"></param>
        /// <param name="ID">SellID 或者 BackID</param>
        /// <returns></returns>
        public Model.WebReturn GetDetail(Model.Sys_UserInfo user, int aduitID, bool isSellAduitID, int ID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //var cuser = from a in lqh.Umsdb.Sys_UserInfo
                    //            where a.UserID == user.UserID
                    //            select a;

                    //if (cuser.Count() == 0)
                    //{
                    //    return new Model.WebReturn() { ReturnValue = false ,ArrList = new ArrayList()};
                    //}
                    //user = cuser.First();
                    //获取用户审批记录
                   var infolist = from a in  lqh.Umsdb.View_SellOffAduitInfoList 
                                  where a.AduitID == aduitID
                                  select a ;

                    List<Model.View_SellOffAduitInfoList> models1 = infolist.ToList();

                    //获取申请明细
                    List<Model.View_SellOffAduitProList> models2 = new List<Model.View_SellOffAduitProList>();
                  
                    var selllist = from b in lqh.Umsdb.View_SellOffAduitProList
                                    join a in lqh.Umsdb.Pro_SellInfo_Aduit on b.SellAduitID equals a.ID
                                    join c in lqh.Umsdb.Pro_SellOffAduitInfo on a.OffAduit equals c.ID
                                where c.ID == aduitID   && (b.OldSellListID ==null || b.OldSellListID==0)
                                select b;
                    models2 = selllist.ToList();
                    
                  
                    var backlist = from b in lqh.Umsdb.View_SellOffAduitProList
                                    join a in lqh.Umsdb.Pro_SellBackInfo_Aduit on b.BackAduitID equals a.ID
                                    join c in lqh.Umsdb.Pro_SellOffAduitInfo on a.OffAduit equals c.ID
                                   where c.ID == aduitID && (b.OldSellListID == null || b.OldSellListID == 0)
                                select b;
                    models2.AddRange(backlist.ToList());
                    
                    ArrayList arr = new ArrayList();
                    arr.Add(models1);
                    arr.Add(models2);
                    return new Model.WebReturn() { ReturnValue = true, ArrList = arr };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passed"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.WebReturn Aduit(Model.Sys_UserInfo user,Model.Pro_SellOffAduitInfoList selloffaduit, Model.Pro_SellOffAduitInfo psa)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var cuser = from a in lqh.Umsdb.Sys_UserInfo
                                    where a.UserID == user.UserID
                                    select a;

                        if (cuser.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Obj = "当前用户不存在，审批失败！" };
                        }
                        user = cuser.First();

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }


                        var aduitmodel = from a in lqh.Umsdb.Pro_SellOffAduitInfo
                                         where a.ID == psa.ID
                                         select a;
                        if (aduitmodel.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue=false,Message="审批单不存在，审批失败！"};
                        }
                        Model.Pro_SellOffAduitInfo model = aduitmodel.First();

                        #region 验证用户操作菜单  仓库  商品的权限

                        List<string> hallidlist = new List<string>();

                        hallidlist.Add(model.HallID);

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count > 0)
                        {
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

                        #region   验证是否已审批 

                        if (model.IsAduited)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单已审批，审批失败！" };
                        }
                        if (model.IsPass)
                        {
                            return new Model.WebReturn() { ReturnValue = false,Message="审批单已通过，审批失败！"};
                        }
                       
                        #endregion 

                        #region   查询该单是否符合条件 (插入外界代码)

                         var Hall_Role=from b in lqh.Umsdb.Sys_Role_Menu_HallInfo
                                  select b;
                         var Users=from b in lqh.Umsdb.Sys_UserInfo 
                              select b;

                         var  aduitlist = from a in lqh.Umsdb.View_SellOffAduitInfo
                                      where  (user.IsDefault==true  && ( a.NextPrice==0 || 
                                      (!Users.Any(p => p.AuditOffPrice == a.NextPrice &&
                                          Hall_Role.Any(x => x.HallID == a.HallID && p.RoleID == x.RoleID)))))
                                          || a.NextPrice ==user.AuditOffPrice 
                                      select a;
                         aduitlist = from a in aduitlist
                                     where a.IsAduited == "N" && a.ID == model.ID
                                     select a;
                         if (aduitlist.Count() == 0)
                         {
                             return new Model.WebReturn() { ReturnValue = true, Message = "无权操作审批单，审批失败！" };
                         }
                        

                        #endregion

                        #region  若 IsBoss = true  直接完结

                        if (Convert.ToBoolean(user.IsBoss))
                        {
                            Model.WebReturn rett = AduitPassed(user, psa, model, lqh);
                            if (!rett.ReturnValue)
                            {
                                return rett;
                            }
                            lqh.Umsdb.Pro_SellOffAduitInfoList.InsertOnSubmit(selloffaduit);
                            lqh.Umsdb.SubmitChanges();
                            ts.Complete();
                            return new Model.WebReturn() { ReturnValue = true, Message = "审批成功！" };
                        }

                        #endregion

                        #region   判断用户是否具有所有权限

                        List<decimal> aduitOffPrice = new List<decimal>();

                        var sell1 = from a in lqh.Umsdb.Pro_SellListInfo
                                   join b in lqh.Umsdb.Pro_SellInfo_Aduit
                                   on a.SellAduitID equals b.ID
                                   where b.OffAduit == model.ID 
                                   orderby a.OtherOff
                                   select a;


                        var back1 = from a in lqh.Umsdb.Pro_SellListInfo
                                   join b in lqh.Umsdb.Pro_SellBackInfo_Aduit
                                   on a.BackAduitID equals b.ID
                                    where b.OffAduit == model.ID 
                                   orderby a.OtherOff
                                   select a;
                        if (sell1.Count() == 0 && back1.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到销售明细，审批失败！" };
                        }

                        foreach (var item in back1)
                        {
                            aduitOffPrice.Add(item.OtherOff);
                        }
                        foreach (var item in sell1)
                        {
                            aduitOffPrice.Add(item.OtherOff);
                        }

                        bool allPermission = false;   //所有档次权限

                        var max = from a in aduitOffPrice
                                  orderby a descending
                                  select a;
                        if (max.Count() != 0)
                        {
                            if (user.AuditOffPrice >= max.First())
                            {
                                allPermission = true;
                            }
                        }
                   
                        #endregion

                        //若用户具有所有权限则完结此单
                        if (allPermission)
                        {
                           bool rett = Pro_SellInfo.PassBackAduit( lqh, model);
                            if (!rett)
                            {
                                return new Model.WebReturn() { ReturnValue=false,Message="审批失败"}; ;
                            }
                            //标记此审批单完结
                            model.IsPass = true;
                            model.AduitUserID = user.UserID;
                            model.AduitDate = DateTime.Now;
                            model.IsAduited = true;
                            model.AduitNote = psa.AduitNote;
                        }
                        //若用户只有部分权限则指派到下一档次
                        else
                        {
                            #region  获取当前审批明细中大于当前用户优惠的最小档次   (插入外界代码)

                            var sell = from a in lqh.Umsdb.Pro_SellListInfo
                                       join b in lqh.Umsdb.Pro_SellInfo_Aduit
                                       on a.SellAduitID equals b.ID
                                       where b.OffAduit == model.ID && a.OtherOff > user.AuditOffPrice
                                       orderby a.OtherOff ascending
                                       select a;


                            var back = from a in lqh.Umsdb.Pro_SellListInfo
                                       join b in lqh.Umsdb.Pro_SellBackInfo_Aduit
                                       on a.BackAduitID equals b.ID
                                       where b.OffAduit == model.ID && a.OtherOff > user.AuditOffPrice
                                       orderby a.OtherOff ascending
                                       select a;
                            //获取当期审批明细中大于当前用户优惠的最小档次
                             decimal conform =0;

                             if (sell.Count() != 0 && back.Count() != 0)
                             {
                                 conform = sell.First().OtherOff < back.First().OtherOff ? sell.First().OtherOff : back.First().OtherOff;
                             }
                             else if (sell.Count() != 0)
                             {
                                 conform = sell.First().OtherOff;
                             }
                             else if (back.Count() != 0)
                             {
                                 conform = back.First().OtherOff;
                             }

                            #endregion

                            #region  获取有权限的下一档次用户   插入外界代码

                            var menuinfo = from a in lqh.Umsdb.Sys_MethodInfo
                                           where a.MethodID == MethodID
                                           select a.Sys_MenuInfo;

                            var nextusers = from a in lqh.Umsdb.Sys_UserInfo
                                            where a.AuditOffPrice >= conform && a.Flag == true
                                            && lqh.Umsdb.Sys_Role_Menu_HallInfo.Any(p => p.RoleID == a.RoleID && p.HallID == model.HallID
                                           ) && lqh.Umsdb.Sys_Role_MenuInfo.Any(b => b.RoleID == a.RoleID && b.MenuID == menuinfo.First().MenuID)
                                            orderby a.AuditOffPrice ascending
                                            select a;

                            #endregion

                            if (nextusers.Count() != 0)
                            {
                                model.NextPrice = (decimal)nextusers.First().AuditOffPrice;
                            }
                            else
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "未能找到下一档次的用户，审批失败！" };
                            }
                        }



                        lqh.Umsdb.Pro_SellOffAduitInfoList.InsertOnSubmit(selloffaduit);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue=true,Message="审批成功！"};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }

                }
            }
        }

        /// <summary>
        /// 验证用户菜单及仓库权限
        /// </summary>
        /// <param name="aduitPrice"></param>
        /// <param name="hallid"></param>
        /// <param name="lqh"></param>
        /// <returns></returns>
        public bool ValidateUser(decimal aduitPrice,string hallid ,LinQSqlHelper lqh)
        {
            var users = from a in lqh.Umsdb.Sys_UserInfo
                        where a.AuditOffPrice == aduitPrice && a.AuditOffPrice > 0
                         select a;

            if (users.Count() != 0)
            {
                List<string> ValidHallIs = new List<string>();
                //有权限的商品
                List<string> ValidPros = new List<string>();

                foreach (var item in users)
                {
                    Model.WebReturn res = ValidClassInfo.GetHall_ProIDFromRole(item, this.MethodID, ValidHallIs, ValidPros);

                    if (res.ReturnValue != true)
                    {
                        return false;
                    }

                    if (ValidHallIs.Count > 0)
                    {
                        if (!ValidHallIs.Contains(hallid))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="user"></param>
        /// <param name="psa"></param>
        /// <param name="model"></param>
        /// <param name="lqh"></param>
        /// <returns></returns>
        private Model.WebReturn AduitPassed(Model.Sys_UserInfo user, Model.Pro_SellOffAduitInfo psa,Model.Pro_SellOffAduitInfo model, LinQSqlHelper lqh)
        {
            #region  添加新数据

            #region 生成单号
            string SellID = "";
            lqh.Umsdb.OrderMacker2(model.HallID, ref SellID);
            if (SellID == "")
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "销售单生成出错" };
            }
            #endregion

            #region  获取明细

            var sellinfo = from  b in lqh.Umsdb.Pro_SellInfo_Aduit
                       where b.OffAduit == model.ID 
                       select b;

            var backinfo = from  b in lqh.Umsdb.Pro_SellBackInfo_Aduit
                       where b.OffAduit == model.ID 
                       select b;

            if (sellinfo.Count() == 0 && backinfo.Count() == 0)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "未能找到审批单，审批失败！" };
            }

            #endregion 

            #region  售货

            foreach (var prosell in sellinfo)
            {
                Model.Pro_SellInfo ps = new Model.Pro_SellInfo();
                ps.BillID = prosell.BillID;
                ps.CardPay = prosell.CardPay;
                ps.CashPay = prosell.CashPay;
                ps.CashTotle = prosell.CashTotle;
                ps.CusName = prosell.CusName;
                ps.CusPhone = prosell.CusPhone;
                ps.HallID = prosell.HallID;
                ps.Note = prosell.Note;
                ps.OffID = prosell.OffID;
                ps.OffTicketID = prosell.OffTicketID;
                ps.OffTicketPrice = prosell.OffTicketPrice;
                ps.OldID = prosell.OldID;
                ps.SellDate = prosell.SellDate;
                ps.Seller = prosell.Seller;
                ps.SellID = SellID;
                ps.SpecalOffID = prosell.SpecalOffID;
                ps.SysDate = prosell.SysDate;
                ps.UserID = prosell.UserID;

                ps.VIP_ID = prosell.VIP_ID;
                ps.VIP_OffList = prosell.VIP_OffList;
                ps.VIP_OffTicket = prosell.VIP_OffTicket;
                lqh.Umsdb.Pro_SellInfo.InsertOnSubmit(ps);

                lqh.Umsdb.SubmitChanges();

                //更新Pro_SellOffAduitInfo表中的SellID
                model.SellID = ps.ID;
                //更新销售明细中的SellID
                var sell = from a in lqh.Umsdb.Pro_SellListInfo
                           where a.SellAduitID == prosell.ID
                            select a;
                if (sell.Count() == 0)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "未能找到销售明细，审批失败！" };
                }
                foreach (var item in sell)
                {
                    item.SellID = ps.ID;
                    if (item.VIP_VIPInfo != null)
                    {
                        item.VIP_VIPInfo.Flag = true;
                    }
                }
                #region  标记串码表的sellID
                var selllist = from a in lqh.Umsdb.Pro_SellListInfo
                               where a.SellAduitID == prosell.ID && a.IMEI != null
                                select a;
                if (selllist.Count() != 0)
                {
                    foreach (var item in selllist)
                    {
                        if (string.IsNullOrEmpty(item.IMEI))
                        {
                            continue;
                        }
                        var imei = from b in lqh.Umsdb.Pro_IMEI
                                    where b.InListID == item.InListID
                                        && b.ProID == item.ProID
                                        && b.IMEI == item.IMEI
                                    where b.SellID == null && b.RepairID == null &&
                                    b.VIPID == null && b.OutID == null && b.BorowID == null && (b.AssetID == null || b.AssetID == 0)
                                    select b;
                        if (imei.Count() != 0)
                        {
                            imei.First().SellID = item.SellID;
                            imei.First().AuditID = null;
                        }
                        else
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "串码" + item.IMEI + "不存在或存在其他操作，审批失败！" };
                        }
                    }
                }
                #endregion

                lqh.Umsdb.SubmitChanges();
            }

            #endregion 

            #region   退货

            foreach (var sellback in backinfo)
            {
                //var backaduit = from a in lqh.Umsdb.Pro_SellBackInfo_Aduit
                //                where a.ID == model.BackID
                //                select a;
                //if (backaduit.Count() == 0)
                //{
                //    return new Model.WebReturn() { ReturnValue = false, Message = "为能找到审批单，审批失败！" };
                //}
                //Model.Pro_SellBackInfo_Aduit sellback = backaduit.First();

                Model.Pro_SellBack back = new Model.Pro_SellBack();
                back.BillID = sellback.BillID;
                back.CardPay = sellback.CardPay;
                back.CashPay = sellback.CashPay;
                back.CashTotle = sellback.CashTotle;
                back.CusName = sellback.CusName;
                back.CusPhone = sellback.CusPhone;
                back.Aduited = sellback.Aduited;
                back.Note = sellback.Note;
                back.BackID = sellback.BackID;
                back.OffTicketID = sellback.OffTicketID;
                back.OffTicketPrice = sellback.OffTicketPrice;
                back.BackMoney = sellback.BackMoney;
                back.BackOffTicketID = sellback.BackOffTicketID;
                back.BackOffTicketPrice = sellback.BackOffTicketPrice;
                back.SellID = sellback.SellID;
                back.CusVIPCardID = sellback.CusVIPCardID;
                back.SysDate = sellback.SysDate;
                back.UserID = sellback.UserID;

                back.NewCashTotle = sellback.NewCashTotle;
                back.OldCashTotle = sellback.OldCashTotle;
                back.SellBackID = sellback.SellBackID;
                back.ShouldBackCash = sellback.ShouldBackCash;
                back.UpdDate = sellback.UpdDate;
                back.UpdUser = sellback.UpdUser;
                back.UserID = sellback.UserID;

                lqh.Umsdb.Pro_SellBack.InsertOnSubmit(back);

                lqh.Umsdb.SubmitChanges();

                //更新Pro_SellOffAduitInfo表中的BackID
                model.BackID = back.ID;
                //更新销售明细中的SellID
                var sell = from a in lqh.Umsdb.Pro_SellListInfo
                           where a.BackAduitID == sellback.ID
                           select a;

                if (sell.Count() == 0)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "未能找到销售明细，审批失败！" };
                }

                foreach (var item in sell)
                {
                    item.BackID = back.ID;
                    if (item.VIP_VIPInfo != null)
                    {
                        item.VIP_VIPInfo.Flag = true;
                    }
                }
                #region  标记串码表的sellID
                var selllist = from a in lqh.Umsdb.Pro_SellListInfo
                               where a.BackAduitID == sellback.ID && a.IMEI != null
                               select a;
                if (selllist.Count() != 0)
                {
                    foreach (var item in selllist)
                    {
                        if (string.IsNullOrEmpty(item.IMEI))
                        {
                            continue;
                        }
                        var imei = from b in lqh.Umsdb.Pro_IMEI
                                   where b.InListID == item.InListID
                                     && b.ProID == item.ProID
                                     && b.IMEI == item.IMEI
                                   where b.SellID == null && b.RepairID == null &&
                                   b.VIPID == null && b.OutID == null && b.BorowID == null && (b.AssetID == null || b.AssetID == 0)
                                   select b;
                        if (imei.Count() != 0)
                        {
                            imei.First().SellID = item.SellID;
                            imei.First().AuditID = null;
                        }
                        else
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "串码" + item.IMEI + "存在其他操作，审批失败！" };
                        }
                    }
                }
                #endregion

                lqh.Umsdb.SubmitChanges();
            }

            #endregion 

            #endregion

            #region 标记审批通过

            model.IsAduited = true;
            model.AduitDate = DateTime.Now;
            model.AduitUserID = user.UserID;
            model.AduitNote = psa.AduitNote;
            model.IsPass = true;
            return new Model.WebReturn() { ReturnValue = true};
            #endregion
        }

        /// <summary>
        /// 审批不通过
        /// </summary>
        /// <param name="user"></param>
        /// <param name="psa"></param>
        /// <param name="model"></param>
        /// <param name="lqh"></param>
        /// <returns></returns>
        public Model.WebReturn AduitUnPassed(Model.Sys_UserInfo user, Model.Pro_SellOffAduitInfoList list, int id, string note)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var cuser = from a in lqh.Umsdb.Sys_UserInfo
                                    where a.UserID == user.UserID
                                    select a;

                        if (cuser.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Obj = "当前用户不存在，审批失败！" };
                        }
                        user = cuser.First();

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                   
                        var aduit = from a in lqh.Umsdb.Pro_SellOffAduitInfo
                                    where a.ID == id
                                    select a;
                        if (aduit.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在，审批失败！" };
                        }
                        Model.Pro_SellOffAduitInfo model = aduit.First();

                        if (model.NextPrice > user.AuditOffPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "优惠权限不足，审批失败！" };
                        }
                        if (model.IsAduited)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "申请已审批，审批失败！" };
                        }


                        #region 验证用户操作菜单  仓库的权限

                        List<string> hallidlist = new List<string>();

                        hallidlist.Add(model.HallID);

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品  无须商品权限
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count > 0)
                        {
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


                        bool rert = Pro_SellInfo.RollBackAduit(lqh, model);
                        if (!rert)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批失败！" };
                        }
                        model.IsAduited = true;
                        model.AduitDate = DateTime.Now;
                        model.AduitUserID = user.UserID;
                        model.AduitNote = note;
                        model.IsPass = false;
                        lqh.Umsdb.Pro_SellOffAduitInfoList.InsertOnSubmit(list);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "审批成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 取消申请
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Aduitid"></param>
        /// <returns></returns>
        public Model.WebReturn CancelApply(Model.Sys_UserInfo user, Model.Pro_SellOffAduitInfoList list, int id,string note)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var cuser = from a in lqh.Umsdb.Sys_UserInfo
                                    where a.UserID == user.UserID
                                    select a;

                        if (cuser.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Obj = "当前用户不存在，取消失败！" };
                        }
                        user = cuser.First();

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region  验证用户取消权限


                        if (user.CancelLimit == null || user.CancelLimit == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "权限不足，取消失败！" };
                        }

                        #endregion 

                        
                        var aduit = from a in lqh.Umsdb.Pro_SellOffAduitInfo
                                    where a.ID == id
                                    select a;
                        if (aduit.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在，取消失败！" };
                        }
                        Model.Pro_SellOffAduitInfo model = aduit.First();

                        if (model.NextPrice > user.AuditOffPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "优惠权限不足，取消失败！" };
                        }
                        if (model.IsAduited)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "申请已审批，取消失败！" };
                        }
                     

                        #region 验证用户操作菜单  仓库的权限 

                        List<string> hallidlist = new List<string>();

                        hallidlist.Add(model.HallID);
                        
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品  无须商品权限
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count > 0)
                        {
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


                        #region  验证取消是否超时

                        if (Convert.ToDecimal(user.CancelLimit) != 0)
                        {
                            var intime = from a in lqh.Umsdb.Pro_SellOffAduitInfo
                                            where a.ID == id
                                            select a;
                            DateTime rdate = DateTime.Parse(intime.First().ApplyDate.ToString());
                            TimeSpan dateDiff = DateTime.Now.Subtract(rdate);

                            if (dateDiff.TotalHours > (double)user.CancelLimit)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "取消超时,取消失败！" };
                            }
                        }
                      
                        #endregion 

                       bool rert = Pro_SellInfo.RollBackAduit(lqh,model);
                       if (!rert)
                       {
                           return new Model.WebReturn() { ReturnValue = false, Message = "取消失败！" };
                       }
                       model.IsAduited = true;
                       model.AduitDate = DateTime.Now;
                       model.AduitUserID = user.UserID;
                       model.AduitNote =note;
                       model.IsPass = false;
                       lqh.Umsdb.Pro_SellOffAduitInfoList.InsertOnSubmit(list);
                       lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "取消成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue =false,Message=ex.Message};
                    }
                }
            }
        }

    }
}
