using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace DAL
{
    public class Sys_UserInfo : Sys_InitParentInfo
    {


        private int _MethodID;
        private int MenuID = 51;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Sys_UserInfo()
        {
            this.MethodID = 0;
        }

        public Sys_UserInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
        private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
            new Model.ReportSqlParams_String(){ParamName="UserName" },
            new Model.ReportSqlParams_String(){ParamName="RealName" },
            new Model.ReportSqlParams_DataTime(){ParamName="StartDate" },
            new Model.ReportSqlParams_DataTime(){ParamName="EndDate" },
            new Model.ReportSqlParams_String(){ParamName="Flag"},
        
        };

        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }
        #region 获取员工实体

        public Model.WebReturn Add(Model.Sys_UserInfo user, List<Model.Sys_UserInfo> userList)
        {
            if (userList == null || userList.Count() > 1)
                return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "无参数或数据出错" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region  判断数据有效性



                        //List<string> UserIDList = (from b in userList
                        //                           select b.UserID).Distinct().ToList();

                        //List<string> DtpIDList = (from b in userList
                        //                          select b.DtpID).Distinct().ToList();
                        //if (RealNameList.Count() != userList.Count())
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "数据出错！" };
                        //}
                        //if (DtpIDList.Count() != userList.Count())
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "数据出错！" };
                        //}
                        #endregion
                        #region  员工是否已经存在
                        /*List<string> RealNameList = (from b in userList
                        //                             select b.RealName).Distinct().ToList();
                        //List<Model.Sys_UserInfo> query = (from b in lqh.Umsdb.Sys_UserInfo
                        //                                  where RealNameList.Contains(b.RealName)
                        //                                  select b).ToList();
                        //if (query.Count() > 0)
                        //{
                        //    return new Model.WebReturn() { Obj = query, ReturnValue = false, Message = "已登记员工不在此处入职" };
                        //}
                         */
                        #endregion
                        #region  验证帐号是否存在
                        if (!string.IsNullOrEmpty(userList[0].UserName))
                        {
                            List<Model.Sys_UserInfo> query = (from b in lqh.Umsdb.Sys_UserInfo
                                                              where b.UserName == userList[0].UserName
                                                              select b).ToList();
                            if (query.Count() > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "存在用户名" };
                            }
                        }
                        #endregion
                        #region 不存在的员工操作
                        int count = userList.Count();
                        string msg = "";
                        lqh.Umsdb.OrderMacker(count, "YG", "YG", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "生成员工编号出错" };
                        }
                        string[] InListIDStr = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (InListIDStr.Count() != userList.Count())
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "生成员工编号数量与员工数量不一致" };
                        }

                        for (int i = 0; i < userList.Count(); i++)
                        {
                            if (userList[i].Sys_UserOPList == null || userList[i].Sys_UserOPList.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "入职资料未填写" };
                            }
                            userList[i].UserID = InListIDStr[i];
                            userList[i].Flag = true;
                            //userList[i].CanLogin = false;
                        }
                        #endregion

                        #region 完成操作
                        lqh.Umsdb.Sys_UserInfo.InsertAllOnSubmit(userList);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "入职成功！" };
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "新增失败！" };
                        throw ex;
                    }

                }
            }
        }
        


        /// <summary>
        /// 获取调入实体  158
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region 权限
                    //List<string> ValidHallIDS = new List<string>();
                    ////有权限的商品
                    //List<string> ValidProIDS = new List<string>();

                    //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    //if (ret.ReturnValue != true)
                    //{ return ret; }

                    #endregion

                    if (pageParam == null)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();
                    #region 将传入的参数与指定的参数了左连接

                    var param_join = from b in pageParam.ParamList
                                     join c in this.ParamList
                                     on new { b.ParamName, t = b.GetType() }
                                     equals
                                     new { c.ParamName, t = c.GetType() }
                                     into temp1
                                     from c1 in temp1.DefaultIfEmpty()
                                     select new
                                     {
                                         ParamFront = b,
                                         ParamBehind = c1
                                     };

                    #endregion

                    #region 获取数据

                    var inorder_query = (from b in lqh.Umsdb.View_Sys_UserInfo
                                         select b).ToList();
                    List<Model.View_Sys_UserInfo> userInfo = new List<Model.View_Sys_UserInfo>();
                    foreach (var m in param_join)
                    {
                        switch (m.ParamFront.ParamName)
                        {
                            case "UserName":
                                Model.ReportSqlParams_String mm1 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm1.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = (from b in inorder_query
                                                     where b.UserName.Contains(mm1.ParamValues)
                                                     select b).ToList();
                                    break;
                                }
                            case "RealName":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = (from b in inorder_query
                                                     where b.RealName.Contains(mm.ParamValues)
                                                     select b).ToList();
                                    break;
                                }
                            case "StartDate":
                                Model.ReportSqlParams_DataTime mmT1 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mmT1.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = (from b in inorder_query
                                                     where b.CreateDate >= mmT1.ParamValues
                                                     select b).ToList();
                                    break;
                                }
                            case "EndDate":
                                Model.ReportSqlParams_DataTime mmT2 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mmT2.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = (from b in inorder_query
                                                     where b.CreateDate <= mmT2.ParamValues
                                                     select b).ToList();
                                    break;
                                }
                            case "Flag":
                                Model.ReportSqlParams_String mmL1 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mmL1.ParamValues))
                                    break;
                                else
                                {
                                  
                                    if (mmL1.ParamValues == "Now")
                                    {
                                        inorder_query = (from b in inorder_query
                                                         where b.Flag == true && b.LeaveDate==null
                                                         select b).ToList();
                                    }
                                    if (mmL1.ParamValues == "Leave")
                                    {
                                        //List<string> UserIDList = (from b in inorder_query
                                        //                 where b.Flag == true
                                        //                 select b.UserID).ToList();

                                        //inorder_query = (from b in inorder_query
                                        //                 where b.Flag == false&&!UserIDList.Contains(b.UserID)
                                        //                 select b).ToList();
                                        inorder_query =( from a  in inorder_query
                                                        where a.Flag==false || a.Flag== null
                                                        //&& 
                                                        select a).ToList();

                                        //foreach (var Item in inorder_query)
                                        //{

                                        //    var query = from b in userInfo
                                        //                where b.UserID == Item.UserID
                                        //                select b;
                                        //    if (query.Count() == 0)
                                        //    {
                                        //        userInfo.Add(Item);
                                        //    }
                                        //    else if (query.Count() > 1)
                                        //    {
                                        //        return new Model.WebReturn() { ReturnValue = false, Message = "获取失败，筛选员工列表时出错！" };
                                        //    }
                                        //    else
                                        //    {
                                        //        if (query.First().LeaveDate < Item.LeaveDate)
                                        //        {
                                        //            userInfo.Remove(query.First());
                                        //            userInfo.Add(Item);
                                        //        }
                                        //    }

                                        //}
                                        //inorder_query.Clear();
                                        //inorder_query.AddRange(userInfo);
                                    }
                                    //if (mmL1.ParamValues == "All")
                                    //{
                                    //    List<Model.View_Sys_UserInfo> HasUser= (from b in inorder_query
                                    //                     where b.Flag == true
                                    //                     select b).ToList();

                                    //    List<string> UserIDList = (from b in HasUser
                                    //                               select b.UserID).ToList();

                                    //    inorder_query = (from b in inorder_query
                                    //                     where b.Flag == false && !UserIDList.Contains(b.UserID)
                                    //                     select b).ToList();

                                    //    foreach (var Item in inorder_query)
                                    //    {

                                    //        var query = from b in userInfo
                                    //                    where b.UserID == Item.UserID
                                    //                    select b;
                                    //        if (query.Count() == 0)
                                    //        {
                                    //            userInfo.Add(Item);
                                    //        }
                                    //        else if (query.Count() > 1)
                                    //        {
                                    //            return new Model.WebReturn() { ReturnValue = false, Message = "获取失败，筛选员工列表时出错！" };
                                    //        }
                                    //        else
                                    //        {
                                    //            if (query.First().LeaveDate < Item.LeaveDate)
                                    //            {
                                    //                userInfo.Remove(query.First());
                                    //                userInfo.Add(Item);
                                    //            }
                                    //        }

                                    //    }
                                    //    inorder_query.Clear();
                                    //    inorder_query.AddRange(userInfo);
                                    //    inorder_query.AddRange(HasUser);
                                    //}
                                    break;
                                }

                            default: break;
                        }
                    }
                    #endregion

                    #region 排序

                    inorder_query = (from b in inorder_query
                                     orderby b.SysDate descending
                                     select b).ToList();
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数
                    int pagecount = 0;
                    if (pageParam.PageSize > 0)
                    {
                        pagecount = pageParam.RecordCount / pageParam.PageSize;
                    }

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.View_Sys_UserInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        if (pageParam.PageSize > 0)
                        {

                            List<Model.View_Sys_UserInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                            pageParam.Obj = list;

                        }
                        else
                        {
                            pageParam.Obj = inorder_query.ToList();
                        }
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
                //}
            }
        }
        #endregion

        #region 未登记员工入职    156

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newUser">新用户</param>
        /// <param name="userReminds">新用户定制提醒菜单列表</param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Sys_UserInfo newUser,List<int> userReminds)
        {
            if (newUser == null)
                return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "无参数或数据出错" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region  判断数据有效性



                        //List<string> UserIDList = (from b in userList
                        //                           select b.UserID).Distinct().ToList();

                        //List<string> DtpIDList = (from b in userList
                        //                          select b.DtpID).Distinct().ToList();
                        //if (RealNameList.Count() != userList.Count())
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "数据出错！" };
                        //}
                        //if (DtpIDList.Count() != userList.Count())
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "数据出错！" };
                        //}
                        #endregion
                        #region  员工是否已经存在
                        /*List<string> RealNameList = (from b in userList
                        //                             select b.RealName).Distinct().ToList();
                        //List<Model.Sys_UserInfo> query = (from b in lqh.Umsdb.Sys_UserInfo
                        //                                  where RealNameList.Contains(b.RealName)
                        //                                  select b).ToList();
                        //if (query.Count() > 0)
                        //{
                        //    return new Model.WebReturn() { Obj = query, ReturnValue = false, Message = "已登记员工不在此处入职" };
                        //}
                         */
                        #endregion
                        #region  验证帐号是否存在
                        if (!string.IsNullOrEmpty(newUser.UserName))
                        {
                            List<Model.Sys_UserInfo> query = (from b in lqh.Umsdb.Sys_UserInfo
                                                              where b.UserName == newUser.UserName
                                                              select b).ToList();
                            if (query.Count() > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "存在用户名" };
                            }
                        }
                        #endregion
                        #region 不存在的员工操作
                     
                        string msg = "";
                        lqh.Umsdb.OrderMacker(1, "YG", "YG", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "生成员工编号出错" };
                        }
                       // string[] InListIDStr = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        //if (InListIDStr.Count() != newUser.Count())
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "生成员工编号数量与员工数量不一致" };
                        //}

                        //for (int i = 0; i < newUser.Count(); i++)
                        //{
                            if (newUser.Sys_UserOPList == null || newUser.Sys_UserOPList.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "入职资料未填写" };
                            }
                            newUser.UserID = msg;
                            newUser.Flag = true;
                            //userList[i].CanLogin = false;
                       // }
                        #endregion

                        #region  添加用户提醒

                        //验证当前用户提醒是否有效
                        var remindList =  from b in lqh.Umsdb.Sys_RemindList
                                        where userReminds.Contains((int)b.ID) && b.IsInTime == true
                                        && b.Flag == true
                                        select b;
                        if (remindList.Count() != userReminds.Count)
                        {
                            return new Model.WebReturn() { ReturnValue=false,Message="部分提醒已禁用，请刷新提醒列表！"};
                        }

                        List<Model.Sys_UserRemindList> urlist = new List<Model.Sys_UserRemindList>();
                        foreach(var item in remindList)
                        {
                            Model.Sys_UserRemindList remind = new Model.Sys_UserRemindList();
                            remind.Remind = item.ID;
                            remind.UserID = newUser.UserID;
                            urlist.Add(remind);
                        }
                        lqh.Umsdb.Sys_UserRemindList.InsertAllOnSubmit(urlist);
                        #endregion 

                        #region 完成操作
                                     
                        lqh.Umsdb.Sys_UserInfo.InsertOnSubmit(newUser);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "入职成功！" };
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "新增失败！" };
                        throw ex;
                    }

                }
            }
        }
        #endregion

        #region  再入职或离职 270        原方法 Add 156
        public Model.WebReturn Reinstated(Model.Sys_UserInfo user, Model.Sys_UserInfo model)
        {

            if (model == null)
                return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "无参数" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(model.UserID))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                        }
                        if (model.Sys_UserOPList == null || model.Sys_UserOPList.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                        }
                        var query = (from b in lqh.Umsdb.Sys_UserInfo
                                     where b.UserID == model.UserID
                                     select b).ToList();
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "找不到该员工的资料" };
                        }
                        Model.Sys_UserInfo UserList = query.First();

                        #region  重新入职

                        Model.Sys_UserOPList OPList = model.Sys_UserOPList.First();
                        if (OPList.Flag == false)
                        {
                            int query_Exist = (from b in lqh.Umsdb.Sys_UserOPList
                                               where b.UserID == OPList.UserID && b.OpID == OPList.OpID && b.Flag == true
                                               select b).Count();
                            if (query_Exist > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "该员工是在职状态，无法重新入职" };
                            }
                            if (OPList.CreateDate == null)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "未填写入职时间" };
                            }

                            UserList.Flag = true;
                            UserList.DtpID = model.DtpID;
                            Model.Sys_UserOPList NewOPList = new Model.Sys_UserOPList();
                            NewOPList.Flag = true;
                            NewOPList.OpID = OPList.OpID;
                            NewOPList.UpdUserID = user.UserID;
                            NewOPList.UserID = OPList.UserID;
                            NewOPList.HallID = OPList.HallID;
                            NewOPList.CreateDate = OPList.CreateDate;

                            lqh.Umsdb.Sys_UserOPList.InsertOnSubmit(NewOPList);
                        }
                        #endregion
                        #region 离职
                        if (OPList.Flag == true && OPList.ID >= 0)
                        {
                            var query_OP = (from b in lqh.Umsdb.Sys_UserOPList
                                            where b.ID == OPList.ID
                                            select b).ToList();
                            if (query_OP.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "找不到该员工的资料" };
                            }
                            Model.Sys_UserOPList OP = query_OP.First();
                            OP.LeaveDate = DateTime.Now;
                            UserList.Flag = false;
                            OP.Flag = false;
                        }
                        #endregion

                        #region 完成操作
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "操作成功！" };
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "操作失败！" };
                        throw ex;
                    }

                }
            }
        }
        #endregion

        #region 员工资料修改
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Sys_UserInfo model,List<int> add ,List<int> del )
        {
            if (model == null)
                return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "无参数" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(model.UserID))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                        }

                        var query = (from b in lqh.Umsdb.Sys_UserInfo
                                     where b.UserID == model.UserID
                                     select b).ToList();
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "找不到该员工的资料" };
                        }
                        if (!string.IsNullOrEmpty(model.UserName))
                        {
                            var query1 = (from b in lqh.Umsdb.Sys_UserInfo
                                          where b.UserName == model.UserName && b.UserID != model.UserID
                                          select b).ToList();
                            if (query.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "存在用户名，不能修改！" };
                            }
                        }

                        #region  修改资料
                        Model.Sys_UserInfo NewUser = query.First();
                        NewUser.SysDate = DateTime.Now;
                        NewUser.UpdUserID = user.UserID;
                        NewUser.CanLogin = model.CanLogin;
                        NewUser.IsBoss = model.IsBoss;
                        NewUser.IsDefault = model.IsDefault;
                        NewUser.AuditOffPrice = model.AuditOffPrice;
                        NewUser.BorowAduitPrice = model.BorowAduitPrice;
                        if (model.DtpID!=0)
                        {
                            NewUser.DtpID = model.DtpID;
                        }
                        NewUser.Flag = true;
                        NewUser.AduitLimit = model.AduitLimit;
                        NewUser.CancelLimit = model.CancelLimit;
                        NewUser.RealName = model.RealName;
                        if (model.RoleID > 0)
                        {
                            NewUser.RoleID = model.RoleID;
                        }
                        if (!string.IsNullOrEmpty(model.UserName))
                            NewUser.UserName = model.UserName;
                        if (!string.IsNullOrEmpty(model.UserPwd))
                            NewUser.UserPwd = model.UserPwd;
                        #endregion



                        Model.Sys_UserOPList OPList = model.Sys_UserOPList.First();

                        var query_Exist = (from b in lqh.Umsdb.Sys_UserOPList
                                           where b.UserID == model.UserID && b.Flag == true
                                           select b).ToList();
                        if (query_Exist.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该员工是离职状态或不存在" };
                        }
                        if (OPList.OpID > 0)
                        {
                            query_Exist.First().OpID = OPList.OpID;
                        }
                        if (!string.IsNullOrEmpty(OPList.HallID))
                        {
                            query_Exist.First().HallID = OPList.HallID;
                        }

                        #region  修改用户定制的提醒

                      
                        //var remind = from a in lqh.Umsdb.Sys_RemindList
                        //             where a.IsInTime == true && a.Flag == true && add.Contains((int)a.ID)
                        //             select a;
                        //if (remind.Count() != add.Count)
                        //{
                        //    return new Model.WebReturn() { ReturnValue=false,Message="部分提醒无效，请刷新提醒列表！"};
                        //}

                       
                        var uremind = from a in lqh.Umsdb.Sys_UserRemindList
                                      join b in lqh.Umsdb.Sys_RemindList
                                      on a.Remind equals b.ID
                                      where del.Contains((int)b.ID) && a.UserID==model.UserID
                                      select a;

                        //删除用户所有定制
                        lqh.Umsdb.Sys_UserRemindList.DeleteAllOnSubmit(uremind);

                        //添加新定制
                        var all = from x in lqh.Umsdb.Sys_RemindList
                                  where add.Contains((int)x.ID) && x.IsInTime == true && x.Flag == true
                                  select x;
                        
                        List<Model.Sys_UserRemindList> userRemindList = new List<Model.Sys_UserRemindList>();
                        foreach (var item in all)
                        {
                            Model.Sys_UserRemindList u = new Model.Sys_UserRemindList();
                            u.UserID = model.UserID;
                            u.Remind = item.ID;
                            userRemindList.Add(u);
                        }
                        lqh.Umsdb.Sys_UserRemindList.InsertAllOnSubmit(userRemindList);


                        #endregion 

                        #region 完成操作
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功！" };
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "修改失败！" };
                        throw ex;
                    }

                }

            }
        }
        #endregion


        public List<Model.Sys_UserInfo> GetList(Model.Sys_UserInfo sysUser, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_UserInfo>()

                                where b.Flag == true
                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}
                    //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    //arr.AddRange(query_area);
                    var temp1 = query.ToList();
                    foreach (var sysUserInfo in temp1)
                    {
                        sysUserInfo.UserPwd = "";//删去密码
                    }
                    return temp1.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Sys_UserInfo>();
                }
            }
        }

        public Model.WebReturn UpDatePwd(Model.Sys_UserInfo user, string NewPwd)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var users = from b in lqh.Umsdb.Sys_UserInfo
                                where b.UserName.ToLower() == user.UserName.ToLower()
                                && b.UserPwd.ToLower() == user.UserPwd.ToLower()
                                select b;
                    if (users.Count() == 0)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "用户名或密码错误" };
                    }
                    Model.Sys_UserInfo firstUser = users.First();
                    firstUser.UserPwd = NewPwd;
                    firstUser.UpdUserID = firstUser.UserID;
                    firstUser.UpTime = DateTime.Now;
                    lqh.Umsdb.SubmitChanges();
                    return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "修改成功" };
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
            }
        }
    }
}