using System.Data.Linq;
using System.Net.Configuration;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Pro_SellBackAduit
    {
            private int MethodID;

	    public Pro_SellBackAduit()
	    {
		    this.MethodID = 0;
	    }

        public Pro_SellBackAduit(int MenthodID)
	    {
		    this.MethodID = MenthodID;
	    }

        /// <summary>
        /// 提交退货申请
        /// </summary>
        /// <param name="psa"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_SellBackAduit psa)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region "验证用户操作仓库  商品的权限 "
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS,lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count>0&&!ValidHallIDS.Contains(psa.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == psa.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                        }
                            
                        #endregion


                        #region 验证内容

                        var sellinfoquery = lqh.Umsdb.Pro_SellInfo.Where(p => p.ID == psa.SellID);
                        if (!sellinfoquery.Any()) return new WebReturn() {ReturnValue = false, Message = "无该销售单"};

                        var sellinfo = sellinfoquery.First();
                        var lists = sellinfo.Pro_SellListInfo.ToList();
                        if (sellinfo.Pro_SellBack.Any())
                        {

                            var backinfo = sellinfo.Pro_SellBack.OrderByDescending(p => p.ID).First();
                            

                            lists =
                                backinfo.Pro_SellListInfo.ToList();
                           


                        }
                        var q = psa.Pro_SellBackAduitList.Join(lists, list => list.SellListID, info => info.ID,
                            (list, info) => info);
                        if (q.Count() != psa.Pro_SellBackAduitList.Count)
                        {
                            return new WebReturn()
                            {
                                ReturnValue = false,
                                Message = "申请内容验证失败, 请重试"

                            };
                        }
                        #endregion

                        


                        #region 验证是否重复申请

                        if (lqh.Umsdb.Pro_SellBackAduit.Any(h => h.SellID == psa.SellID && (h.Aduited == false||(h.Passed&&h.Used==false))))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该销售单已有退货申请" };
                   
                        }

                        #endregion

                        

                        string aduitid = null;
                        lqh.Umsdb.OrderMacker(1, "SBA", "SBA", ref aduitid);
                        if (aduitid == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "退货单生成出错" };
                        }

                        psa.AduitID = aduitid;
                        psa.Aduited = false;
                        psa.Passed = false;
                        psa.Used = false;
                       

                        lqh.Umsdb.Pro_SellBackAduit.InsertOnSubmit(psa);

                        //更新Pro_SellInfo 的审批单AduitID
                        //var query = from s in lqh.Umsdb.Pro_SellInfo
                        //            where s.ID == sellinfoID
                        //            select s;
                        //query.First().AuditID = aduitid;

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Obj = psa.SellID };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false,Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 退货审批
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn Aduit(Model.Sys_UserInfo user,Model.Pro_SellBackAduit modelx)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        var aduit = from a in lqh.Umsdb.Pro_SellBackAduit
                                    where a.ID == modelx.ID
                                    select a;

                        if (aduit.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false,Message="无数据可审批，审批失败！" };
                        }


                        #region 验证仓库权限 
                        List<string> hallidlist = new List<string>();

                        hallidlist.Add(aduit.First().HallID);
                            
                        #region "验证用户操作仓库  商品的权限 "
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS,lqh);

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

                        #endregion

                        #region  验证商品权限
                        List<int> proidlist = new List<int>();

                        var alist = from d in lqh.Umsdb.Pro_ProInfo
                                    join a in lqh.Umsdb.Pro_SellListInfo on d.ProID equals a.ProID
                                    join b in lqh.Umsdb.Pro_SellBackAduitList on a.ID equals b.SellListID
                                    join c in lqh.Umsdb.Pro_SellBackAduit on b.AduitID equals c.ID
                                    where c.ID == modelx.ID
                                    select d;
                        if (alist.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "审批明细不存在，删除失败！" };
                        }
                        foreach (var item in alist)
                        {
                            proidlist.Add((int)item.Pro_ClassID);
                        }
                        if (ValidProIDS.Count > 0)
                        {
                            foreach (var item in proidlist)
                            {
                                if (!ValidProIDS.Contains(item.ToString()))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作，删除失败！" };
                                }
                            }
                        }
                        #endregion 

                        #region   验证审批单价是否有效

                        var list = from a in lqh.Umsdb.Pro_SellBackAduitList
                                   where a.AduitID == modelx.ID
                                    select a;
                        if (list.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false,Message ="审批明细不存在，审批失败！"};
                        }
                        
                        foreach (var child in modelx.Pro_SellBackAduitList)
                        {
                            foreach (var itemx in list)
                            {
                                if (child.ID == itemx.ID)
                                {
                                    if (child.AduitBackPrice > itemx.BackPrice || child.AduitBackPrice < 0)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "请确保审批单价在申请单价范围内！" };
                                    }

                                    if (child.AduitBackPrice == null)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "审批单价不能为空！" };
                                    }
                                }
                            }
                        }
                        #endregion 


                        List<int> listID = new List<int>();
                    
                        var query = from a in lqh.Umsdb.Pro_SellBackAduit
                                    where a.ID == modelx.ID
                                    select a;
                        Model.Pro_SellBackAduit model = query.First();
                        listID.Add(modelx.ID);
                        if (query.Count() != 0)
                        {
                            model.AduitDate = modelx.AduitDate;
                            model.AduitUser = modelx.AduitUser;
                            model.Aduited = modelx.Aduited;
                            model.Note = modelx.Note;
                            model.Passed = modelx.Passed;
                            model.AduitMoney = modelx.AduitMoney;
                            var sblist = from a in lqh.Umsdb.Pro_SellBackAduitList
                                       where a.AduitID == modelx.ID
                                        select a;
                            foreach (var itemx in modelx.Pro_SellBackAduitList)
                            {
                                foreach (var child in sblist)
                                {
                                    if (itemx.ID == child.ID)
                                    {
                                        child.AduitBackPrice = itemx.AduitBackPrice;
                                        break;
                                    }
                                }
                            }
                            lqh.Umsdb.SubmitChanges();
                        }
                        
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true ,Obj=listID};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false,Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 获取待审批列表
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="countOnePage">每页提取的数量</param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user, int pageIndex, int countOnePage)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    int? total = 0;
                    List<Model.GetSellBackAduitListResult> results =
                    lqh.Umsdb.GetSellBackAduitList(pageIndex, countOnePage, ref total).ToList();

                    if (results.Count() != 0)
                    {
                        List<Model.AduitModel> models = new List<Model.AduitModel>();
                        Model.AduitModel am = null;

                        foreach (var item in results)
                        {
                            am = new Model.AduitModel();
                            am.ID = int.Parse(item.ID.ToString());
                            am.AduitID = item.AduitID;
                            am.ApplyUser = item.ApplyUser;
                            am.ApplyDate = DateTime.Parse(item.ApplyDate.ToString());
                            am.HallID = item.HallID;
                            am.HallName = item.HallName;
                            am.Money = decimal.Parse(item.AduitMoney.ToString());
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

                    var aduit_query = from b in lqh.Umsdb.View_Pro_SellBackAduit
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "Aduited":
                                Model.ReportSqlParams_Bool mm = (Model.ReportSqlParams_Bool)item;

                                aduit_query = from b in aduit_query
                                              where b.HasAduited == mm.ParamValues
                                              select b;
                                break;

                            case "Passed":
                                Model.ReportSqlParams_Bool pass = (Model.ReportSqlParams_Bool)item;

                                aduit_query = from b in aduit_query
                                              where b.HasPassed == pass.ParamValues
                                              select b;
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
                                                      where b.SysDate >= mm5.ParamValues &&
                                                      b.SysDate <= DateTime.Parse(mm6.ParamValues.ToString()).AddDays(1)
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
                                                  where para.ParamValues.Contains(b.ApplyUser)
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

                        List<Model.View_Pro_SellBackAduit> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_Pro_SellBackAduit> aduitList = results.ToList();

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



        public Model.WebReturn GetAduitInfoList(Model.Sys_UserInfo user, int sellid)
        {


            try
            {
                using (LinQSqlHelper lqh=new LinQSqlHelper())
                {
                    var q = lqh.Umsdb.Pro_SellBackAduitList.Where(list => list.Pro_SellBackAduit.SellID == sellid);
                    return new WebReturn() {ReturnValue = true, Obj = q.ToList()};
                }
            }
            catch (Exception ex)
            {
                return new WebReturn() {ReturnValue = false, Message = ex.Message};

            }
            
        }


        /// <summary>
        /// 获取退货单明细
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn GetSellBackDetail(Model.Sys_UserInfo user, int SellID,int aduitid)
        {
            try
            {

                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    //TODO:权限
                    //DataLoadOptions dl = new DataLoadOptions();
                    //dl.LoadWith<Model.Pro_SellInfo>(info => info.Pro_SellListInfo);
                    //dl.LoadWith<Model.Pro_SellInfo>(info => info.Pro_SellSpecalOffList);
                    //dl.LoadWith<Model.Pro_SellInfo>(info => info.Pro_SellBackAduit);
                    //lqh.Umsdb.LoadOptions = dl;
                    //var results = lqh.Umsdb.Pro_SellInfo.First(p => p.ID == SellID);
                    List<View_ProSellBackAduitDetail> models = new List<View_ProSellBackAduitDetail>();

                    var aduit = from a in lqh.Umsdb.Pro_SellBackAduit
                                where a.ID  == aduitid
                                select a;

                    if (aduit.Count() > 0)
                    {
                        var slist = from a in lqh.Umsdb.View_ProSellBackAduitDetail
                                    where a.AduitID == aduitid //&& a.BackCount!=0
                                    select a;

                        if (slist.Count() > 0)
                        {
                            models.AddRange(slist.ToList());
                        }
                        //Model.Pro_SellBackAduit model = aduit.First();
                        //var back = from s in lqh.Umsdb.Pro_SellBack
                        //           where s.SellID == model.SellID// && s.AduitID == model.AduitID
                        //           //&& Convert.ToBoolean(model.Used )==false
                        //orderby s.ID descending
                        //           select s;
                        //if (back.Count() > 0)
                        //{
                        //    var backlist = from a in lqh.Umsdb.View_ProSellBackAduitDetail
                        //                   where a.BackID == back.First().ID //&& a.BackCount != 0
                        //                   select a;
                        //    if (backlist.Count() > 0)
                        //    {
                        //        models.AddRange(backlist.ToList());
                        //    }
                        //}
                        //else
                        //{
                        //    var slist = from a in lqh.Umsdb.View_ProSellBackAduitDetail
                        //                where a.SellID == model.SellID //&& a.BackCount!=0
                        //                 select a;

                        //    if (slist.Count() > 0)
                        //    {
                        //        models.AddRange(slist.ToList());
                        //    }
                        //}
                    }
                    else
                    {
                        return new Model.WebReturn() { ReturnValue = false};
                    }
                    if (models.Count != 0)
                    {
                        if (!Convert.ToBoolean(models[0].Aduited))
                        {
                            foreach (var item in models)
                            {
                                item.AduitBackPrice = item.BackPrice;
                            }
                        }
                    }
                  

                    //获取特殊优惠
                    ArrayList arr = new ArrayList();

                    var off =  from o in lqh.Umsdb.View_SellBackOffList
                               where o.AduitID==aduitid
                               select o;
                    if(off.Count()>0)
                    {
                        arr.Add(off.ToList());
                    }
                    else
                    {
                        arr.Add(new List<Model.View_SellBackOffList>());
                    }
                    return new WebReturn()
                    {
                        Obj = models,
                        ArrList =arr,
                        ReturnValue = true
                    };

                }

            }
            catch (Exception ex)
            {
                return new WebReturn() { ReturnValue = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// 删除申请 211
        /// </summary>
        /// <param name="user"></param>
        /// <param name="aduitIDs"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, List<int> aduitIDs)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region  验证用户删除权限


                        if (user.CancelLimit == null || user.CancelLimit == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "权限不足，删除失败！" };
                        }

                        #endregion 

                        #region 验证用户权限 
                        List<string> hallidlist = new List<string>();

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                   
                        #endregion

                        var aduit = from a in lqh.Umsdb.Pro_SellBackAduit
                                    where  aduitIDs.Contains(a.ID)
                                    select a;
                      
                        if (aduit.Count() != aduitIDs.Count)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分记录已删除，删除失败！" };
                        } 
                       // Model.Pro_SellBackAduit sb = aduit.First();
                        var userlist = from a in aduit
                                       where a.Used == true
                                       select a;

                        if (userlist.Count()>0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分审批单已使用，删除失败！" };
                        }
                        foreach (var item in aduit)
                        {
                            hallidlist.Add(item.HallID);
                        }
                       

                        #region   验证仓库权限
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

                        #region  验证商品权限
                        List<int> proidlist = new List<int>();

                        var alist = from d in lqh.Umsdb.Pro_ProInfo 
                                    join a in lqh.Umsdb.Pro_SellListInfo on d.ProID equals a.ProID
                                    join b in lqh.Umsdb.Pro_SellBackAduitList on a.ID equals b.SellListID
                                    join c in lqh.Umsdb.Pro_SellBackAduit on b.AduitID equals c.ID
                                    where aduitIDs.Contains(c.ID)
                                    select d;
                        if (alist.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue=false,Message="审批明细不存在，删除失败！"};
                        }
                        foreach (var item in alist)
                        {
                            proidlist.Add((int)item.Pro_ClassID);
                        }
                        if (ValidProIDS.Count > 0)
                        {
                            foreach (var item in proidlist)
                            {
                                if (!ValidProIDS.Contains(item.ToString()))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作，删除失败！" };
                                }
                            }
                        }
                        #endregion 

                        lqh.Umsdb.Pro_SellBackAduit.DeleteAllOnSubmit(aduit);
                        List<Model.Pro_SellBackAduit_bak> list = new List<Pro_SellBackAduit_bak>();
                        foreach (var sb in aduit)
                        {
                            Model.Pro_SellBackAduit_bak bak = new Model.Pro_SellBackAduit_bak();
                            bak.AduitDate = sb.AduitDate;
                            bak.Aduited = sb.Aduited;
                            bak.AduitID = sb.AduitID;
                            bak.AduitMoney = sb.AduitMoney == null ? 0 : sb.AduitMoney;
                            bak.AduitUser = sb.AduitUser;
                            bak.ApplyDate = sb.ApplyDate;
                            bak.ApplyMoney = sb.ApplyMoney;
                            bak.ApplyUser = sb.ApplyUser;
                            bak.CusName = sb.CusName;
                            bak.CusPhone = sb.CusPhone;
                            bak.HallID = sb.HallID;
                            bak.ID = sb.ID;
                            bak.Note = sb.Note;
                            bak.Passed = sb.Passed;
                            bak.SellID = sb.SellID;
                            bak.SysDate = sb.SysDate;
                            bak.Used = sb.Used;
                            bak.UseDate = sb.UseDate;
                            bak.VIPID = sb.VIPID;
                            list.Add(bak);
                        }
                        lqh.Umsdb.Pro_SellBackAduit_bak.InsertAllOnSubmit(list);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功！" };
                    }
                    catch (System.Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }
        
    }
}
