using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Pro_SellType : Sys_InitParentInfo
    {

        private int _MethodID;
        //private int MenuID = 51;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Pro_SellType()
        {
            this.MethodID = 0;
        }

        public Pro_SellType(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
        //private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
        //    new Model.ReportSqlParams_String(){ParamName="ID" },
        //    new Model.ReportSqlParams_String(){ParamName="Name"},
        //    new Model.ReportSqlParams_String(){ParamName="Flag"}, 
        //};
        //public List<Model.ReportSqlParams> ParamList
        //{
        //    get { return _paramList; }
        //    set { _paramList = value; }
        //}
        #region 获取商品类型实体
        /// <summary>
        /// 获取类型实体
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

                    #region 获取数据

                    var inorder_query = from b in lqh.Umsdb.Pro_SellType
                                        where b.IsDelete == false || b.IsDelete == null
                                        select b;

                    #endregion

                    #region 排序
                    inorder_query = from b in inorder_query
                                    orderby b.ID descending
                                    select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.Pro_SellType> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.Pro_SellType> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错 ！" };

                }
            }
        }
        #endregion

        #region 修改商品类别
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Pro_SellType model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否存在类别名称
                        if (!string.IsNullOrEmpty(model.Name))
                        {
                            int query = (from b in lqh.Umsdb.Pro_SellType
                                         where b.Name == model.Name
                                         select b).Count();
                            if (query > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "销售类别名称已存在！" };
                            }
                        }
                        #endregion

                        var queryArea = from b in lqh.Umsdb.Pro_SellType
                                        where b.ID == model.ID
                                        select b;
                        if (queryArea.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "销售类别不存在！" };
                        }
                        Model.Pro_SellType SellType = queryArea.First();
                        #region 商品类别是否在使用中
                        if (SellType.IsDelete == false || SellType.IsDelete == null)
                        {
                            if (model.IsDelete == true)
                            {
                                var queryFlag = from b in lqh.Umsdb.Pro_SellTypeProduct
                                                where b.SellType == model.ID
                                                select b;
                                if (queryFlag.Count() == 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "销售类别正在使用中，不能修改使用状态！" };
                                }
                            }
                        }
                        #endregion
                        if (!string.IsNullOrEmpty(model.Name))
                            SellType.Name = model.Name;

                        SellType.Note = model.Note;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = SellType, ReturnValue = true, Message = "修改成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        #region 新增商品类别
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_SellType model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否存在商品类别
                        if (string.IsNullOrEmpty(model.Name))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未填销售类别名称！" };
                        }
                        #endregion


                        #region 判断数据有效性
                        int query = (from b in lqh.Umsdb.Pro_SellType
                                     where b.Name == model.Name
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "销售类别已存在！" };
                        }
                        #endregion

                        model.IsDelete = false;
                        lqh.Umsdb.Pro_SellType.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "新增成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        #region 删除商品类别
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.Pro_SellType model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否使用商品类别
                        int query = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                     where b.SellType == model.ID
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "销售类别正在使用，不能删除！" };
                        }
                        #endregion
                        var queryArea = from b in lqh.Umsdb.Pro_SellType
                                        where b.ID == model.ID
                                        select b;
                        if (queryArea.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "销售类别不存在！" };
                        }
                        Model.Pro_SellType SellType = queryArea.First();
                        lqh.Umsdb.Pro_SellType.DeleteOnSubmit(SellType);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = SellType, ReturnValue = true, Message = "删除成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        public List<Model.Pro_SellType> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_SellType>()
                                where b.IsDelete == false
                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Pro_SellType>();
                }
            }
        }
    }
}
