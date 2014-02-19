using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 借贷申请 
    /// </summary>
    public class BorowAduit
    {
        private int MethodID;

        public BorowAduit()
        {
            this.MethodID = 0;
        }

        public BorowAduit(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        /// 提交借贷申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user,Model.Pro_BorowAduit model)
        {
              if (model == null) return new Model.WebReturn();

              using (LinQSqlHelper lqh = new LinQSqlHelper())
              {
                  using (TransactionScope ts = new TransactionScope())
                  {
                      try
                      {
                          #region 权限验证
                       
                          Model.WebReturn result = ValidClassInfo.ValidateUser(user,lqh);

                          if (!result.ReturnValue)
                          { return result; }

                          List<int> classids = new List<int>();
                          foreach (var item in model.Pro_BorowAduitList)
                          {
                              var queey = from p in lqh.Umsdb.Pro_ProInfo
                                          where p.ProID == item.ProID
                                          select p;
                              classids.Add((int)queey.First().Pro_ClassID);
                          }
                          //有权限的仓库
                          List<string> ValidHallIDS = new List<string>();
                          //有权限的商品
                          List<string> ValidProIDS = new List<string>();

                          Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                          if (ret.ReturnValue != true)
                          { return ret; }

                          #region  过滤仓库
                          if (ValidHallIDS.Count > 0)
                          {
                                if (!ValidHallIDS.Contains(model.HallID))
                                {
                                    var que = from h in lqh.Umsdb.Pro_HallInfo
                                              where h.HallID == model.HallID
                                              select h;
                                    return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                                }
                          }
                          #endregion

                          #region  过滤商品
                          if (ValidProIDS.Count > 0)
                          {
                              foreach (var item in classids)
                              {
                                  if (!ValidProIDS.Contains(item.ToString()))
                                  {
                                      return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                                  }
                              }
                          }
                          #endregion 

                          #endregion 

                          //若是内部借贷则验证用户是否存在
                          if (model.InternalBorow==true)
                          {
                              var borower = from a in lqh.Umsdb.Sys_UserInfo
                                            where a.UserName == model.Borrower
                                            select a;
                              if (borower.Count() == 0)
                              {
                                  return new WebReturn() { Message = "借贷人不存在，申请失败！", ReturnValue = false };
                              }
                          }
                          string msg=null;
                          lqh.Umsdb.OrderMacker(1,"BSP","BSP",ref msg);
                          if (msg == "")
                          {
                              return new Model.WebReturn() { ReturnValue = false, Message = "审批单生成出错" };
                          }
                          model.AduitID = msg;
                          model.Aduited = false;
                          model.Passed = false;
                          model.ApplyUser = user.UserID;
                         
                          lqh.Umsdb.Pro_BorowAduit.InsertOnSubmit(model);
                          lqh.Umsdb.SubmitChanges();
                          ts.Complete();
                          return new WebReturn() { ReturnValue=true,Obj=model.AduitID};
                      }
                      catch (Exception ex)
                      {
                          return new WebReturn() { ReturnValue = false,Message=ex.Message };
                      }
                  }
              }
        }


        /// <summary>
        /// 拣货
        /// </summary>
        /// <param name="list"></param>
        /// <param name="hid"></param>
        /// <returns></returns>
        public Model.WebReturn CheckPro(Model.Sys_UserInfo user, List<Model.SetSelection> list, string hid)
        {
            if (list == null)
            {
                return new WebReturn() { ReturnValue = false,Obj=new List<Model.SetSelection> ()};
            }
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                List<string> proIDList = new List<string>();

                foreach (var item in list)
                {
                    proIDList.Add(item.Proid);
                }
                var query = from store in lqh.Umsdb.Pro_StoreInfo
                            where store.HallID == hid && proIDList.Contains(store.ProID)
                            group store by store.ProID into table
                            select new {
                                ProID = table.Select( store => store.ProID).Distinct(),
                                ProCount = table.Sum(store => store.ProCount)
                            };
                if (query.Count() != 0)
                {
                    foreach (var ss in list)
                    {
                        foreach (var item in query)
                        {
                            if (ss.Proid == item.ProID.First().ToString() && ss.Countnum <= item.ProCount)
                            {
                                ss.Sucess = true;
                                break;
                            }
                            else 
                            {
                                ss.Note = "库存不足";
                            }
                        }
                    }
                }
                bool flag = true;
                foreach (var item in list)
                {
                    if (!item.Sucess)
                    {
                        flag = false;
                        break;
                    }
                }
                return new WebReturn() { ReturnValue=flag,Obj=list};
            }
        }

          /// <summary>
        /// 审批
        /// </summary>
        /// <param name="user"></param>
        /// <param name="models"></param>
        /// <param name="aduitNum"></param>
        /// <returns></returns>
        public Model.WebReturn Aduit(Model.Sys_UserInfo user,List<Model.Pro_BorowAduit> models)
        {
            if (models == null) return new Model.WebReturn();

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }
                    
                        List<string> classids = new List<string>();
                        List<string> hallids = new List<string>();
                    
                        //更新借贷审批
                        foreach (var item in models)
                        {
                            var query = from aduit in lqh.Umsdb.Pro_BorowAduit
                                        where aduit.ID == item.ID
                                        select aduit;
                            if (query.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "未能找到审批单，审批失败！" };
                            }
                            Model.Pro_BorowAduit aduitModel = query.First();
                            
 
                            
                            if (Convert.ToBoolean(aduitModel.Aduited1))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 已审批，审批失败！" };
                            }
                            if (Convert.ToBoolean(aduitModel.Used))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 已使用，审批失败！" };
                            }
                       
                             
                                aduitModel.AduitDate = item.AduitDate;
                                aduitModel.AduitUser = item.AduitUser;
                                aduitModel.Aduited1 = item.Aduited1;
                                aduitModel.Note1 = item.Note1;
                                aduitModel.Aduited1 = true;
                                aduitModel.Passed1 = item.Passed1;

                                if (item.Passed1 != true || aduitModel.InternalBorow==true)
                                {
                                    aduitModel.Aduited = true;
                                    aduitModel.Passed = item.Passed1;
                                    
                                }
                            
                             
                                
                              
                                //获取待验证的仓库及商品
                                hallids.Add(query.First().HallID);
                                var list = from d in lqh.Umsdb.Pro_BorowAduitList
                                           join p in lqh.Umsdb.Pro_ProInfo 
                                           on d.ProID equals p.ProID
                                           where d.BAduitID == aduitModel.ID
                                           select p;
                                if (list.Count() > 0)
                                {
                                    foreach (var child in list)
                                    {
                                        classids.Add(child.Pro_ClassID.ToString());
                                    }
                                }
                             
                        }

                        #region 权限验证

                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #region  过滤仓库
                        if (ValidHallIDS.Count > 0)
                        {
                            foreach (var item in hallids)
                            {
                                if (!ValidHallIDS.Contains(item))
                                {
                                    var que = from h in lqh.Umsdb.Pro_HallInfo
                                              where h.HallID == item
                                              select h;
                                    return new Model.WebReturn() { ReturnValue = false, Message = "无权操作"+que.First().HallName };
                                }
                            }
                        }
                        #endregion

                        #region  过滤商品
                        if (ValidProIDS.Count > 0)
                        {
                            foreach (var item in classids)
                            {
                                if (!ValidProIDS.Contains(item))
                                {
                                    return new WebReturn() { ReturnValue = false, Message = "无权操作商品"};
                                }
                            }
                        }
                        #endregion

                        #endregion 

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() {ReturnValue = false,Message=ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 获取未审批的列表
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user, int pageIndex, int countOnePage)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    int? total = 0;
                    List<Model.GetBorowAduitListByPageResult> results =
                        lqh.Umsdb.GetBorowAduitListByPage(pageIndex, countOnePage, ref total).ToList();

                    List<AduitModel> aduitList = new List<AduitModel>();
                    AduitModel am = null;
                    AduitListInfo ali = null;

                    int oldID = int.Parse(results.First().ID.ToString());

                    int index = 0;

                    foreach (var item in results)
                    {
                        index++;

                        if (oldID != item.ID || index == 1)
                        {
                            if (index != 1)
                            {
                                aduitList.Add(am);
                            }
                            am = new AduitModel();
                            am.AduitID = item.AduitID;
                            am.ApplyDate = DateTime.Parse(item.ApplyDate.ToString());
                            am.ApplyUser = item.ApplyUser;
                            am.ID = int.Parse(item.ID.ToString());
                            am.Passed = bool.Parse(item.Passed.ToString());
                            am.Note = item.Note;
                            am.HallID = item.HallID;
                            am.HallName = item.HallName;
                        }

                        if (am.AduitListInfo == null)
                        {
                            am.AduitListInfo = new List<AduitListInfo>();
                        }
                        ali = new AduitListInfo();
                        ali.ProCount = int.Parse(item.ProCount.ToString());
                        ali.ProID = item.ProID;
                        ali.ProName = item.ProName;
                        ali.ID = int.Parse(item.ListID.ToString());
                       
                        am.AduitListInfo.Add(ali);

                        oldID = am.ID;
                        if (index == results.Count())
                        {
                            aduitList.Add(am);
                        }
                    }
                    ArrayList arrList = new ArrayList();
                    arrList.Add(total);
                    return new WebReturn() { Obj = aduitList, ArrList = arrList ,ReturnValue=true};
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue=false};
                }
            }
        }

        /// <summary>
        /// 根据审批单获取指定信息
        /// </summary>
        /// <param name="aduitID"></param>
        /// <returns></returns>
        public Model.WebReturn GetBorowAduitInfoByID(Model.Sys_UserInfo user, string aduitID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //获取申请详情
                    List<Model.GetBorowAduitInfoByAIDResult> results =  lqh.Umsdb.GetBorowAduitInfoByAID(aduitID).ToList();
                   
                    var query = from aduit in lqh.Umsdb.Pro_BorowAduit
                                where aduit.AduitID == aduitID
                                select aduit;
                    string borower = string.IsNullOrEmpty(query.First().Borrower) ? "" : query.First().Borrower;

                    var borowinfo = from b in lqh.Umsdb.View_UserBorowInfo
                                    where b.Borrower == borower
                                    select b;

                    //未归还的信息
                    List<View_UserBorowInfo> unBorowInfo = new List<View_UserBorowInfo>();

                    foreach (var item in borowinfo)
                    {
                        if (!Convert.ToBoolean(item.IsReturn))
                        {
                            unBorowInfo.Add(item);
                        }
                    }   
                    //计算信用度   找到当前借贷人未归还的所有数据    总的超期的天数/总未归还记录
                    double credit = 0;
                    var queryRate = from b in lqh.Umsdb.Pro_BorowInfo
                                where Convert.ToBoolean(b.IsReturn)==false
                                && b.Borrower==borower
                                select new { 
                                    b.EstimateReturnTime
                                };

                    if (queryRate.Count() > 0)
                    {
                        foreach (var item in queryRate)
                        {
                            if (item.EstimateReturnTime==null)
                            {
                                continue;
                            }
                            //超出预计归还时间
                            if (DateTime.Now>Convert.ToDateTime(item.EstimateReturnTime))
                            {
                                TimeSpan ts = DateTime.Now - Convert.ToDateTime(item.EstimateReturnTime);
                                credit += ts.TotalDays;
                            }
                        }
                        var bcount = from a in lqh.Umsdb.Pro_BorowInfo
                                     where a.Borrower == borower && a.IsReturn==false
                                     select a;
                        int count = bcount.Count();
                        if (count==0)
                        {
                            credit = 0;
                        }
                        else
                        {
                            credit = credit / (count * 1.0);
                        }
                       
                    }
                 

                    //返回数据
                    ArrayList arr = new ArrayList();
                    arr.Add(unBorowInfo);
                    arr.Add(credit);

                    if (results.Count() != 0)
                    {
                        return new Model.WebReturn() { Obj = results, ReturnValue = true ,ArrList=arr};
                    }
                    return new Model.WebReturn() { ReturnValue = false };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false ,Message=ex.Message};
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

                    var aduit_query = from b in lqh.Umsdb.View_BorowAduit
                                        select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "Aduited1":
                                   Model.ReportSqlParams_Bool mm = (Model.ReportSqlParams_Bool)item;


                                   if (mm.ParamValues)
                                   {
                                       aduit_query = from b in aduit_query
                                                     where b.HasAduited1 == true
                                                     select b;
                                   }
                                   else
                                   {
                                       aduit_query = from b in aduit_query
                                                     where b.HasAduited1 != true
                                                     select b;
                                   }
                                    break;
                            case "Passed":
                                    Model.ReportSqlParams_Bool pass = (Model.ReportSqlParams_Bool)item;

                                    if (pass.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed == true
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed != true
                                                      select b;
                                    }
                                    break;
                            case "Passed1":
                                    Model.ReportSqlParams_Bool pass1 = (Model.ReportSqlParams_Bool)item;

                                    if (pass1.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed1 == true
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed1 != true
                                                      select b;
                                    }
                                    break;
                                ///////////////// 二级  //////////////////////
                            case "Aduited2":
                                    Model.ReportSqlParams_Bool mm2 = (Model.ReportSqlParams_Bool)item;

                                    if (mm2.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasAduited2 == true
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasAduited2 != true
                                                      select b;
                                    }
                                    break;

                            case "Passed2":
                                    Model.ReportSqlParams_Bool pass2 = (Model.ReportSqlParams_Bool)item;

                                    if (pass2.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed2 == true
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed2 != true
                                                      select b;
                                    }
                                    break;
                               ///////////////// 三级  //////////////////////
                            case "Aduited3":
                                    Model.ReportSqlParams_Bool mm3 = (Model.ReportSqlParams_Bool)item;

                                    if (mm3.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasAduited3 == true
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasAduited3 != true
                                                      select b;
                                    }
                                    break;

                            case "Passed3":
                                    Model.ReportSqlParams_Bool pass3 = (Model.ReportSqlParams_Bool)item;

                                    if (pass3.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed3 == true
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed3 != true
                                                      select b;
                                    }
                                    break;
                            case "Used":
                                    Model.ReportSqlParams_Bool use = (Model.ReportSqlParams_Bool)item;

                            
                                    aduit_query = from b in aduit_query
                                                  where b.HasUsed == use.ParamValues
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

                            case "ApplyUser":
                                    Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                    if (para.ParamValues != null)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.ApplyUser.Contains(para.ParamValues.ToString())
                                                      select b;
                                    }
                                    break;

                            case "AduitID":
                                    Model.ReportSqlParams_String para2 = (Model.ReportSqlParams_String)item;
                                    if (para2.ParamValues != null)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.AduitID.Contains(para2.ParamValues)
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

                        List<Model.View_BorowAduit> aduitList  = results.ToList();
                      
                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                                 select a;

                        List<Model.View_BorowAduit> aduitList = results.ToList();
                   
                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue = false, Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 验证审批单
        /// </summary>
        /// <param name="user"></param>
        /// <param name="aduitid"></param>
        /// <returns></returns>
        public Model.WebReturn ValidateAduitID(Model.Sys_UserInfo user,string aduitid)
        {
            //验证权限
            //Model.WebReturn ret = ValidatePower(user, new List<string>() { model.HallID });
            //if (!ret.ReturnValue)
            //{
            //    return ret;
            //}

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var result = from ba in lqh.Umsdb.Pro_BorowAduit
                                 join h in lqh.Umsdb.Pro_HallInfo on ba.HallID equals h.HallID
                                 where ba.AduitID == aduitid
                                 select ba;

                    if (result.Count() == 0)
                    {
                        return new WebReturn() { ReturnValue = false ,Message="审批单不存在"};
                    }
                    else
                    {
                        Model.Pro_BorowAduit ba = result.First();
                        if (!Convert.ToBoolean(ba.Aduited1))
                        {
                            return new WebReturn() { ReturnValue = false,Message="审批单未进行一级审批！" };
                        } 
                        if (!Convert.ToBoolean(ba.Passed1))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "审批单一级审批未通过！" };
                        }
                        if (!Convert.ToBoolean(ba.InternalBorow)&&
                            !Convert.ToBoolean(ba.Aduited) &&
                            !Convert.ToBoolean(ba.Passed))  //内部借贷只需要一级审批
                        {
                            
                            if (!Convert.ToBoolean(ba.Aduited2))
                            {
                                return new WebReturn() { ReturnValue = false, Message = "审批单未进行二级审批！" };
                            }
                            if (!Convert.ToBoolean(ba.Aduited3))
                            {
                                return new WebReturn() { ReturnValue = false, Message = "审批单未进行三级审批！" };
                            }

                            if (!Convert.ToBoolean(ba.Passed2))
                            {
                                return new WebReturn() { ReturnValue = false, Message = "审批单二级审批未通过！" };
                            }
                            if (!Convert.ToBoolean(ba.Passed3))
                            {
                                return new WebReturn() { ReturnValue = false, Message = "审批单三级审批未通过！" };
                            }
                          
                            if (!Convert.ToBoolean(ba.Aduited4))
                            {
                                return new WebReturn() { ReturnValue = false, Message = "审批单未进行四级审批！" };
                            }

                            if (!Convert.ToBoolean(ba.Passed4))
                            {
                                return new WebReturn() { ReturnValue = false, Message = "审批单四级审批未通过！" };
                            }
                            
                        }
                        if (Convert.ToBoolean(ba.Used))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "审批单已使用！" };
                        }
                     

                        List<Model.BorowListModel> models = new List<BorowListModel>();

                        var query = from list in lqh.Umsdb.Pro_BorowAduitList
                                    join p in lqh.Umsdb.Pro_ProInfo on list.ProID equals p.ProID
                                    join t in lqh.Umsdb.Pro_TypeInfo on p.Pro_TypeID equals t.TypeID
                                    join c in lqh.Umsdb.Pro_ClassInfo on p.Pro_ClassID equals c.ClassID
                                    where list.BAduitID == ba.ID
                                    select new 
                                    {
                                        list.ProID,
                                        list.ProCount,
                                        list.ProPrice,
                                        p.ProName,
                                        t.TypeName,
                                        c.ClassName,
                                        p.ProFormat,
                                        p.NeedIMEI,
                                        p.ISdecimals
                                    };
                        Model.BorowListModel ai = null;
                        foreach (var item in query)
                        {
                            ai = new BorowListModel();
                            ai.ClassName = item.ClassName;
                            ai.ProCount =Convert.ToDecimal( item.ProCount.ToString());
                            ai.ProName = item.ProName;
                            ai.ProPrice = (decimal)item.ProPrice;
                            ai.TypeName = item.TypeName;
                            ai.ProFormat = item.ProFormat;
                            ai.ProID = item.ProID;
                            ai.NeedIMEI = item.NeedIMEI;
                            ai.IsDecimal = Convert.ToBoolean(item.ISdecimals);
                            models.Add(ai);
                        }
                        ArrayList arr = new ArrayList();
                        arr.Add(models);
                        return new WebReturn() { ReturnValue = true, Obj = ba, ArrList = arr };
                    }
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue = false ,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 删除借贷申请
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, List<int> ids)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region  验证用户取消权限
                       
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }


                        if (user.CancelLimit == null || user.CancelLimit == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "权限不足，删除失败！" };
                        }

                        #endregion 
                        var aduit = from a in lqh.Umsdb.Pro_BorowAduit
                                    where ids.Contains(a.ID)
                                    select a;

                        if (aduit.Count() != ids.Count)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分记录已删除，删除失败！" };
                        }
                        //Model.Pro_BorowAduit model = aduit.First();
                       

                        lqh.Umsdb.Pro_BorowAduit.DeleteAllOnSubmit(aduit);

                        List<Model.Pro_BorowAduit_bak> list = new List<Pro_BorowAduit_bak>();
                        foreach (var item in aduit)
                        {
                            Model.Pro_BorowAduit model = item as Model.Pro_BorowAduit;
                            if (Convert.ToBoolean(model.Used))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "部分审批单已使用，删除失败！" };
                            }
                            Model.Pro_BorowAduit_bak sa = new Model.Pro_BorowAduit_bak();
                            sa.AduitDate = model.AduitDate;
                            sa.Aduited = model.Aduited;
                            sa.AduitID = model.AduitID;
                            sa.AduitUser = model.AduitUser;
                            sa.ApplyDate = model.ApplyDate;
                            sa.ApplyUser = model.ApplyUser;
                            sa.AduitDate2 = model.AduitDate2;
                            sa.Aduited2 = model.Aduited2;
                            sa.AduitUser2 = model.AduitUser2;
                            sa.AduitUser3 = model.AduitUser3;
                            sa.AduitDate3 = model.AduitDate3;
                            sa.Aduited3 = model.Aduited3;
                            sa.Aduited1 = model.Aduited1;
                            sa.Passed1 = model.Passed1;
                            
                            sa.Borrower = model.Borrower;
                            sa.BorrowType = model.BorrowType;
                            sa.Dept = model.Dept;
                            sa.EstimateReturnTime = model.EstimateReturnTime;
                            sa.MobilPhone = model.MobilPhone;
                            
                            sa.HallID = model.HallID;
                            sa.ID = model.ID;
                            sa.Note = model.Note;
                            sa.Passed = model.Passed;
                            sa.SysDate = model.SysDate;
                            sa.Used = model.Used;
                            sa.UseDate = model.UseDate;
                            sa.TotalMoney = model.TotalMoney;
                            sa.AduitDate4 = model.AduitDate4;
                            sa.Aduited4 = model.Aduited4;
                            sa.AduitUser4 = model.AduitUser4;
                            sa.Passed4 = model.Passed4;
                            sa.Note1 = model.Note1;
                            sa.Note2 = model.Note2;
                            sa.Note3 = model.Note3;
                            sa.Note4 = model.Note4;
                            sa.InternalBorow = model.InternalBorow;

                            list.Add(sa);
                        }
                        lqh.Umsdb.Pro_BorowAduit_bak.InsertAllOnSubmit(list);
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
