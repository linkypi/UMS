using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class ASP_ErrorInfo : Sys_InitParentInfo
    {
        private int MethodID;

        public ASP_ErrorInfo()
        {
            this.MethodID = 0;
        }

        public ASP_ErrorInfo(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        public List<Model.ASP_ErrorInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.ASP_ErrorInfo>()
                                select b;

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.ASP_ErrorInfo>();
                }
            }
        }

        /// <summary>
        /// 342
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetAll(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
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

                    var aduit_query = from b in lqh.Umsdb.GetTable<Model.View_ASPErrInfo>()
                                      select b;
                    #endregion

                  
                    pageParam.RecordCount = aduit_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in aduit_query.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPErrInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPErrInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
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
        /// 344
        /// </summary>
        /// <param name="user"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user,Model.ASP_ErrorInfo err)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var list = from a in lqh.Umsdb.ASP_ErrorInfo
                                   where a.ErrorName == err.ErrorName
                                   select a;
                        if (list.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue=false,Message="已存在相同故障现象，添加失败！"};
                        }
                       // string msg = null;
                       // lqh.Umsdb.OrderMacker(1, "RERR", "RERR", ref msg);
                        //if (msg == "")
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "审批单生成出错" };
                        //}
                    
                        lqh.Umsdb.ASP_ErrorInfo.InsertOnSubmit(err);
                        lqh.Umsdb.SubmitChanges();
                        err.ErrorID = "RERR" + err.ID.ToString();
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                         return new Model.WebReturn() { ReturnValue = true, Message = "添加成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 343
        /// </summary>
        /// <param name="user"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, List<Model.ASP_ErrorInfo> errs)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var ids = from a in errs
                                  select a.ID;

                        var list = from a in lqh.Umsdb.ASP_ErrorInfo
                                   where ids.Contains(a.ID )
                                   select a;
                        if (list.Count() != errs.Count)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分故障已删除，删除失败！" };
                        }
                        lqh.Umsdb.ASP_ErrorInfo.DeleteAllOnSubmit(list);
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

        /// <summary>
        /// 345
        /// </summary>
        /// <param name="user"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.ASP_ErrorInfo err)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var list = from a in lqh.Umsdb.ASP_ErrorInfo
                                   where a.ID == err.ID
                                   select a;
                        if (list.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "故障现象不存在，修改失败！" };
                        }
                        list.First().ErrorName = err.ErrorName;
                        list.First().TypeID = err.TypeID;
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
