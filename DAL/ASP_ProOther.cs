using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class ASP_ProOther : Sys_InitParentInfo
    {
        private int MethodID;

        public ASP_ProOther()
        {
            this.MethodID = 0;
        }

        public ASP_ProOther(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        public List<Model.ASP_ProOther> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.ASP_ProOther>()
                                where b.Flag ==true
                                select b;

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.ASP_ProOther>();
                }
            }
        }

        /// <summary>
        /// 352
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

                    var aduit_query = from b in lqh.Umsdb.GetTable<Model.ASP_ProOther>()
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

                        List<Model.ASP_ProOther> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.ASP_ProOther> aduitList = results.ToList();

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
        /// 354
        /// </summary>
        /// <param name="user"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.ASP_ProOther err)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var list = from a in lqh.Umsdb.ASP_ProOther
                                   where a.Name == err.Name
                                   select a;
                        if (list.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue=false,Message="已存在相同附件名称，添加失败！"};
                        }
                       // string msg = null;
                       // lqh.Umsdb.OrderMacker(1, "RERR", "RERR", ref msg);
                        //if (msg == "")
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "审批单生成出错" };
                        //}

                        lqh.Umsdb.ASP_ProOther.InsertOnSubmit(err);
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
        /// 353
        /// </summary>
        /// <param name="user"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, List<Model.ASP_ProOther> errs)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var ids = from a in errs
                                  select a.ID;

                        var list = from a in lqh.Umsdb.ASP_ProOther
                                   where ids.Contains(a.ID )
                                   select a;
                        if (list.Count() != errs.Count)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分附件已删除，删除失败！" };
                        }
                        foreach (var item in list)
                        {
                            item.Flag = false;
                        }
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
        /// 355
        /// </summary>
        /// <param name="user"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.ASP_ProOther err)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var list = from a in lqh.Umsdb.ASP_ProOther
                                   where a.ID == err.ID
                                   select a;
                        if (list.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "附件名称不存在，修改失败！" };
                        }
                        list.First().Name = err.Name;
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
