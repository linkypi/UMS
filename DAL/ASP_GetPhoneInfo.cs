using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace DAL
{
   public class ASP_GetPhoneInfo
    {
          private int MethodID;

        public ASP_GetPhoneInfo()
        {
            this.MethodID = 0;
        }

        public ASP_GetPhoneInfo(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        /// 359
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

                    var aduit_query = from b in lqh.Umsdb.View_ASPGetPhoneInfo
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "Finished":
                                Model.ReportSqlParams_Bool Finished = (Model.ReportSqlParams_Bool)item;

                                if (Finished.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.Finished == Finished.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.Finished == null || b.Finished == false // 质检不通过则此单完结
                                                  select b;
                                }
                                break;

                            case "IsFetchAduit":
                                Model.ReportSqlParams_Bool IsFetchAduit = (Model.ReportSqlParams_Bool)item;

                                if (IsFetchAduit.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsFetchAduit == IsFetchAduit.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsFetchAduit == null || b.IsFetchAduit == false
                                                  select b;
                                }
                                break;


                            case "FetchAuditPassed":
                                Model.ReportSqlParams_Bool FetchAuditPassed = (Model.ReportSqlParams_Bool)item;

                                if (FetchAuditPassed.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.FetchAuditPassed == FetchAuditPassed.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.FetchAuditPassed == null || b.FetchAuditPassed == false
                                                  select b;
                                }
                                break;
                            case "HasFetch":
                                Model.ReportSqlParams_Bool HasFetch = (Model.ReportSqlParams_Bool)item;

                                if (HasFetch.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasFetch == HasFetch.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasFetch == null || b.HasFetch == false
                                                  select b;
                                }
                                break;
                            case "FetchNeedAudit":
                                Model.ReportSqlParams_Bool pass = (Model.ReportSqlParams_Bool)item;

                                if (pass.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.FetchNeedAudit == pass.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.FetchNeedAudit == null || b.FetchNeedAudit ==false
                                                  select b;
                                }
                                break;

                            case "IsPassed":
                                Model.ReportSqlParams_Bool IsPassed = (Model.ReportSqlParams_Bool)item;

                                if (IsPassed.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsPassed == IsPassed.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsPassed == null || b.IsPassed == false
                                                  select b;
                                }
                                break;

                            case "IsAudit":
                                Model.ReportSqlParams_Bool IsAudit = (Model.ReportSqlParams_Bool)item;

                                if (IsAudit.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsAudit == IsAudit.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsAudit == null || b.IsAudit == false
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

                        List<Model.View_ASPGetPhoneInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPGetPhoneInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion

                    return new Model.WebReturn() { ReturnValue = true };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Message = ex.Message, ReturnValue = false };
                }
            }

        }

    }
}
