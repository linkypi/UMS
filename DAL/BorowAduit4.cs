using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class BorowAduit4
    {
             private int MethodID;

        public BorowAduit4()
        {
            this.MethodID = 0;
        }

        public BorowAduit4(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        /// 4级审批   286
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
                            if (query.Count()==0)
                            {
                                return new Model.WebReturn() {ReturnValue=false,Message="未能找到审批单，三级审批失败！" };
                            }
                            Model.Pro_BorowAduit aduitModel = query.First();

                       
                            if (!Convert.ToBoolean(aduitModel.Aduited3))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 未进行三级审批，审批失败！" };
                            }
                          
                            if (Convert.ToBoolean(aduitModel.Aduited4))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 已审批，审批失败！" };
                            }
                            if (Convert.ToBoolean(aduitModel.Aduited))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 已审批，审批失败！" };
                            }
                          
                           
                                aduitModel.AduitDate4 = item.AduitDate4;
                                aduitModel.AduitUser4 = item.AduitUser4;
                                aduitModel.Aduited4 = true;
                                aduitModel.Note4 = item.Note4;
                                aduitModel.Passed4 = item.Passed4;
                                aduitModel.Aduited = true;
                                aduitModel.Passed = item.Passed4;

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
        /// 查询  287
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

                    var aduit_query = from b in lqh.Umsdb.View_BorowAduit4
                                      where b.HasAduited3 == true && b.HasPassed3 == true &&
                                      b.HasAduited != true && b.HasPassed != true
                                        select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "Aduited4":
                                   Model.ReportSqlParams_Bool mm = (Model.ReportSqlParams_Bool)item;

                                   if (mm.ParamValues)
                                   {
                                       aduit_query = from b in aduit_query
                                                     where b.HasAduited4 == true
                                                     select b;
                                   }
                                   else
                                   {
                                       aduit_query = from b in aduit_query
                                                     where b.HasAduited4 != true
                                                     select b;
                                   }
                                    break;

                            case "Passed4":
                                    Model.ReportSqlParams_Bool pass = (Model.ReportSqlParams_Bool)item;

                                    if (pass.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed4 == true
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.HasPassed4 != true
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

                        List<Model.View_BorowAduit4> aduitList  = results.ToList();
                       
                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                                 select a;

                        List<Model.View_BorowAduit4> aduitList = results.ToList();
                   
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
    }
}
