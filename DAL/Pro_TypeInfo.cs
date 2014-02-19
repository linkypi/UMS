using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{

    public class Pro_TypeInfo : Sys_InitParentInfo
    {


        private int _MethodID;
        //private int MenuID = 51;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Pro_TypeInfo()
        {
            this.MethodID = 0;
        }

        public Pro_TypeInfo(int MenthodID)
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
        #region 获取商品品牌实体
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

                    var inorder_query = from b in lqh.Umsdb.Pro_TypeInfo
                                        select b;

                    #endregion

                    #region 排序
                    inorder_query = from b in inorder_query
                                    orderby b.TypeID descending
                                    select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.Pro_TypeInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.Pro_TypeInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
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

        #region 修改商品品牌
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Pro_TypeInfo model)
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
                        if (!string.IsNullOrEmpty(model.TypeName))
                        {
                            int query = (from b in lqh.Umsdb.Pro_TypeInfo
                                         where b.TypeName == model.TypeName
                                         select b).Count();
                            if (query > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "商品品牌名称已存在！" };
                            }
                        }
                        #endregion

                        var queryArea = from b in lqh.Umsdb.Pro_TypeInfo
                                        where b.TypeID == model.TypeID
                                        select b;
                        if (queryArea.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "商品品牌不存在！" };
                        }
                        Model.Pro_TypeInfo TypeInfo = queryArea.First();
                        
                        if (!string.IsNullOrEmpty(model.TypeName))
                            TypeInfo.TypeName = model.TypeName;
                        TypeInfo.Order = model.Order;
                        TypeInfo.Note = model.Note;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = TypeInfo, ReturnValue = true, Message = "修改成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        #region 新增商品品牌
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_TypeInfo model)
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
                        if (string.IsNullOrEmpty(model.TypeName))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未填商品品牌名称！" };
                        }
                        #endregion


                        #region 判断数据有效性
                        int query = (from b in lqh.Umsdb.Pro_TypeInfo
                                     where b.TypeName == model.TypeName
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "商品品牌已存在！" };
                        }
                        #endregion

                        lqh.Umsdb.Pro_TypeInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "新增成功！" , Obj=model};
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        #region 删除商品品牌
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.Pro_TypeInfo model)
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
                        int query = (from b in lqh.Umsdb.Pro_ProInfo
                                     where b.Pro_TypeID == model.TypeID
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "商品品牌正在使用，不能删除！" };
                        }
                        #endregion
                        var queryArea = from b in lqh.Umsdb.Pro_TypeInfo
                                        where b.TypeID == model.TypeID
                                        select b;
                        if (queryArea.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "商品类别不存在！" };
                        }
                        Model.Pro_TypeInfo TypeInfo = queryArea.First();
                        lqh.Umsdb.Pro_TypeInfo.DeleteOnSubmit(TypeInfo);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = TypeInfo, ReturnValue = true, Message = "删除成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        public List<Model.Pro_TypeInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_TypeInfo>()

                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}
                    //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    //arr.AddRange(query);

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Pro_TypeInfo>();
                }
            }
        }



    }
}
