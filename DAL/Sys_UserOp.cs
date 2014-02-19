using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace DAL
{
    public class Sys_UserOp : Sys_InitParentInfo
    {

        private int MenthodID;

        public Sys_UserOp()
        {
            this.MenthodID = 0;
        }

        public Sys_UserOp(int MenthodID)
        {
            this.MenthodID = MenthodID;
        }

        private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
            new Model.ReportSqlParams_String(){ParamName="ID" },
            new Model.ReportSqlParams_String(){ParamName="Name"},
            new Model.ReportSqlParams_String(){ParamName="Flag"}, 
        };
        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }
        #region 职位表信息
        /// <summary>
        /// 获取职位表信息
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

                    if (pageParam == null || pageParam.PageIndex < 0 || pageParam.PageSize != 20)
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

                    var inorder_query = from b in lqh.Umsdb.Sys_UserOp
                                        orderby b.OpID descending
                                        select b;

                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.Sys_UserOp> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.Sys_UserOp> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
            }
        }
        #endregion
        public List<Model.Sys_UserOp> GetList(Model.Sys_UserInfo sysUser, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Sys_UserOp>()
                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}
                    //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    //arr.AddRange(query_area);

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Sys_UserOp>();
                }
            }
        }
        #region 新增
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Sys_UserOp model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证数据
                        var query = (from b in lqh.Umsdb.Sys_UserOp
                                     where b.Name == model.Name
                                     select b).ToList();
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn { Message = "已存在该职位！", ReturnValue = false };
                        }
                        #endregion
                        lqh.Umsdb.Sys_UserOp.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn { Message = "新增成功！", ReturnValue = true };
                    }
                    catch
                    {
                        return new Model.WebReturn { Message = "新增失败！", ReturnValue = false };
                    }
                }
            }
        }
        #endregion

        #region 删除
        public Model.WebReturn Delete(Model.Sys_UserInfo user, List<Model.Sys_UserOp> model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证数据
                        List<int> IDList = (from b in model
                                            select b.OpID).ToList();
                        var query = (from b in lqh.Umsdb.Sys_UserOp
                                     where IDList.Contains(b.OpID)
                                     select b).ToList();
                        if (model.Count() != query.Count())
                        {
                            return new Model.WebReturn { Message = "部分职位不存在！", ReturnValue = false };
                        }
                        #endregion
                        lqh.Umsdb.Sys_UserOp.DeleteAllOnSubmit(query);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn { Message = "删除成功！", ReturnValue = true };
                    }
                    catch
                    {
                        return new Model.WebReturn { Message = "删除失败！", ReturnValue = false };
                    }
                }
            }
        }
        #endregion

        #region 修改
        public Model.WebReturn Update(Model.Sys_UserInfo user, List<Model.Sys_UserOp> model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证数据
                        List<int> IDList = (from b in model
                                            select b.OpID).ToList();
                        var query = (from b in lqh.Umsdb.Sys_UserOp
                                     where IDList.Contains(b.OpID)
                                     select b).ToList();
                        if (model.Count() != query.Count())
                        {
                            return new Model.WebReturn { Message = "部分职位不存在！", ReturnValue = false };
                        }
                        #endregion
                        foreach (var Item in model)
                        {
                            var Op = (from b in query
                                      where b.OpID == Item.OpID
                                      select b).ToList();
                            Model.Sys_UserOp option = Op.First();
                            option.Name = Item.Name;
                            option.Note = Item.Note;
                            lqh.Umsdb.SubmitChanges();
                        }
                        ts.Complete();
                        return new Model.WebReturn { Message = "修改成功！", ReturnValue = true };
                    }
                    catch
                    {
                        return new Model.WebReturn { Message = "修改失败！", ReturnValue = false };
                    }
                }
            }
        }
        #endregion
    }

}