using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class ASP_Dealer : Sys_InitParentInfo
    {
         private int MethodID;

        public ASP_Dealer()
        {
            this.MethodID = 0;
        }

        public ASP_Dealer(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        public List<Model.ASP_Dealer> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.ASP_Dealer>()
                                where b.IsDelete==false
                                select b;

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.ASP_Dealer>();
                }
            }
        }

        /// <summary>
        /// 379
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public WebReturn Search(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
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

                    var aduit_query = from b in lqh.Umsdb.GetTable<Model.ASP_Dealer>()
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

                        List<Model.ASP_Dealer> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.ASP_Dealer> aduitList = results.ToList();

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
        /// 377
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dl"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.ASP_Dealer dl)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        var query = from b in lqh.Umsdb.GetTable<Model.ASP_Dealer>()
                                    where b.Dealer == dl.Dealer && b.IsDelete==false
                                    select b;
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该经销商名称已存在！" };
                        }
                        dl.SysDate = DateTime.Now;
                        dl.IsDelete = false;
                        lqh.Umsdb.ASP_Dealer.InsertOnSubmit(dl);
                        lqh.Umsdb.SubmitChanges();
                        tx.Complete();
                        return new Model.WebReturn() { ReturnValue=true,Message="添加成功！"};

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 378
        /// </summary>
        /// <param name="user"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.ASP_Dealer d)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        var query = from b in lqh.Umsdb.GetTable<Model.ASP_Dealer>()
                                    where b.ID == d.ID
                                    select b;
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定销商名称！" };
                        }

                        Model.ASP_Dealer model = query.First();
                        if (model.IsDelete == true)
                        {
                            return new Model.WebReturn() { ReturnValue=false,Message="该经销商已删除，修改失败！"};
                        }

                        model.Note = d.Note;
                        model.Phone = d.Phone;
                        model.UserName = d.UserName;
                        model.Dealer = d.Dealer;
                   
                        lqh.Umsdb.SubmitChanges();
                        tx.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功！" };

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 376
        /// </summary>
        /// <param name="user"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public Model.WebReturn Delete (Model.Sys_UserInfo user, int id )
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        var query = from b in lqh.Umsdb.GetTable<Model.ASP_Dealer>()
                                    where b.ID == id
                                    select b;
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定销商名称！" };
                        }

                        Model.ASP_Dealer model = query.First();
                        if (model.IsDelete == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该经销商已删除，删除失败！" };
                        }

                       // model.Note = note;
                        model.Deleter = user.UserID;
                        model.DeleteTime = DateTime.Now;
                        model.IsDelete = true;
                        lqh.Umsdb.SubmitChanges();
                        tx.Complete();
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
