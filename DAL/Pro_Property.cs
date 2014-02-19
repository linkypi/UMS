using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 商品属性
    /// </summary>
    public class Pro_Property : Sys_InitParentInfo
    {

        private int MenthodID;

        public Pro_Property()
        {
            this.MenthodID = 0;
        }

        public Pro_Property(int MenthodID)
        {
            this.MenthodID = MenthodID;
        }
        #region 获取属性
        public Model.WebReturn GetModel(Model.Sys_UserInfo user)
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

                    //if (pageParam == null || pageParam.PageIndex < 0 || pageParam.PageSize != 20)
                    //{
                    //    return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    //}

                    #region 获取数据
                    DataLoadOptions d = new DataLoadOptions();
                    d.LoadWith<Model.Pro_Property>(c => c.Pro_PropertyValue);
                    lqh.Umsdb.LoadOptions = d;

                    var inorder_query = (from b in lqh.Umsdb.Pro_Property
                                         orderby b.ID descending
                                         select b).ToList();

                    #endregion
                    return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = inorder_query };
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错 ！" };

                }
            }
        }
        #endregion
        #region 添加属性(正在使用)
        public Model.WebReturn Add(Model.Sys_UserInfo user, List<Model.Pro_Property> model)
        {
            if (model == null || model.Count() == 0)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否存在区域名称
                        foreach (var Item in model)
                        {
                            if (string.IsNullOrEmpty(Item.Cate))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "未填写商品属性名称！" };
                            }
                            Item.Flag = true;
                        }
                        #endregion


                        #region 判断数据有效性
                        List<string> CateList = (from b in model
                                                 select b.Cate).ToList();
                        int query = (from b in lqh.Umsdb.Pro_Property
                                     where CateList.Contains(b.Cate)
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分属性名称已存在！" };
                        }
                        #endregion


                        lqh.Umsdb.Pro_Property.InsertAllOnSubmit(model);
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
        #region 添加属性
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_Property model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否存在区域名称
                        if (string.IsNullOrEmpty(model.Cate))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未填写商品属性名称！" };
                        }
                        #endregion


                        #region 判断数据有效性
                        int query = (from b in lqh.Umsdb.Pro_Property
                                     where b.Cate == model.Cate
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "属性名称已存在！" };
                        }
                        #endregion

                        model.Flag = true;
                        lqh.Umsdb.Pro_Property.InsertOnSubmit(model);
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

        #region 修改属性
        public Model.WebReturn Update(Model.Sys_UserInfo user, List<Model.Pro_Property> model)
        {
            if (model == null || model.Count() == 0)
                return new Model.WebReturn() { ReturnValue = false, Message = "无参数！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        DataLoadOptions d = new DataLoadOptions();
                        d.LoadWith<Model.Pro_Property>(c => c.Pro_PropertyValue);
                        d.LoadWith<Model.Pro_ProInfo>(c => c.Pro_ProProperty_F);
                        lqh.Umsdb.LoadOptions = d;


                        #region 判断是否存在名称
                        List<string> CateList = (from b in model
                                                 where b.Note == "XG"
                                                 select b.Cate).ToList();
                        List<int> XGIDList = (from b in model
                                              where b.Note == "XG"
                                              select b.ID).ToList();
                        if (CateList.Count() > 0)
                        {
                            int count = (from b in lqh.Umsdb.Pro_Property
                                         where CateList.Contains(b.Cate)
                                         select b).Count();
                            if (count > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "属性名称已存在,添加属性值失败！" };
                            }
                            var queryModel = (from b in model
                                              where b.Note == "XG"
                                              orderby b.ID descending
                                              select b).ToList();
                            var query = (from b in lqh.Umsdb.Pro_Property
                                         where XGIDList.Contains(b.ID)
                                         orderby b.ID descending
                                         select b).ToList();
                            if (queryModel.Count() != query.Count())
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "数据出错，请联系管理员！" };
                            }
                            for (int i = 0; i < queryModel.Count(); i++)
                            {
                                query[i].Cate = queryModel[i].Cate;
                                lqh.Umsdb.SubmitChanges();
                            }
                        }
                        #endregion

                        #region 获取数据


                        List<int> IDList = (from b in model
                                            select b.ID).ToList();

                        var queryProperyt = (from b in lqh.Umsdb.Pro_Property
                                             where IDList.Contains(b.ID)
                                             select b).ToList();
                        if (queryProperyt.Count() != model.Count())
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分商品属性不存在！" };
                        }
                        #endregion

                        #region 获取了删除掉的商品属性值
                        foreach (var Item in model)
                        {
                            List<int> ValuesID = (from b in Item.Pro_PropertyValue
                                                  select b.ID).ToList();

                            var queryFirst = from b in queryProperyt
                                             where b.ID == Item.ID
                                             select b;
                            if (queryFirst == null)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "数据出错，请联系管理员！" };
                            }
                            Model.Pro_Property property = queryFirst.First();

                            var queryInUse = (from b in property.Pro_PropertyValue
                                              where b.Pro_PropertyID == Item.ID && !ValuesID.Contains(b.ID)
                                              select b).ToList();

                            #region 判断删除的属性值是否在使用中
                            var ValueIDList = (from b in queryInUse
                                               select b.ID).ToList();
                            if (queryInUse.Count() > 0)
                            {
                                var queryFlag = (from b in lqh.Umsdb.Pro_ProProperty_F
                                                 where ValueIDList.Contains(b.ValueID)
                                                 select b).ToList();
                                if (queryFlag.Count() > 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "删除的属性值正在使用中！" };
                                }
                                lqh.Umsdb.Pro_PropertyValue.DeleteAllOnSubmit(queryInUse);
                            }
                            #endregion

                            #region 插入新增的数据
                            var queryAdd = (from b in Item.Pro_PropertyValue
                                            where b.ID == 0
                                            select b).ToList();
                            if (queryAdd.Count() > 0)
                            {
                                List<Model.Pro_PropertyValue> propertyValueList = new List<Model.Pro_PropertyValue>();
                                foreach (var AddItem in queryAdd)
                                {
                                    Model.Pro_PropertyValue propertyValue = new Model.Pro_PropertyValue();
                                    propertyValue.Pro_PropertyID = AddItem.Pro_PropertyID;
                                    propertyValue.Value = AddItem.Value;
                                    propertyValueList.Add(propertyValue);
                                }
                                lqh.Umsdb.Pro_PropertyValue.InsertAllOnSubmit(propertyValueList);
                            }
                            #endregion

                            #region 修改属性值
                            var queryUpdate = (from b in Item.Pro_PropertyValue
                                               where b.Note == "XG" && b.ID != 0
                                               orderby b.ID descending
                                               select b).ToList();

                            List<int> UpdateIDList = (from b in queryUpdate
                                                      select b.ID).ToList();

                            if (queryUpdate.Count() > 0)
                            {
                                var queryValue = (from b in lqh.Umsdb.Pro_PropertyValue
                                                  where UpdateIDList.Contains(b.ID)
                                                  orderby b.ID descending
                                                  select b).ToList();
                                if (queryValue.Count() != queryUpdate.Count())
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "数据出错，请联系管理员！" };
                                }
                                for (int i = 0; i < queryUpdate.Count(); i++)
                                {
                                    queryValue[i].Value = queryUpdate[i].Value;
                                    lqh.Umsdb.SubmitChanges();
                                }
                                #region 修改商品的商品属性名称
                                List<string> ProIDList = (from b in lqh.Umsdb.Pro_ProProperty_F
                                                          where UpdateIDList.Contains(b.ValueID)
                                                          select b.ProID).Distinct().ToList();

                                var ProList = from b in lqh.Umsdb.Pro_ProInfo
                                              where ProIDList.Contains(b.ProID)
                                              select b;

                                List<Model.Pro_PropertyValue> ValueLsit = (from b in lqh.Umsdb.Pro_PropertyValue select b).ToList();
                                if (ProList.Count() > 0)
                                {
                                    foreach (var ProItem in ProList)
                                    {
                                        List<string> queryName = (from b in ProItem.Pro_ProProperty_F
                                                                  join c in ValueLsit on b.ValueID equals c.ID
                                                                  select c.Value).ToList();
                                        string Property = "";
                                        int i = 0;
                                        foreach (var PropertyItem in queryName)
                                        {
                                            if (i == 0)
                                                Property = PropertyItem;
                                            else
                                                Property += "/" + PropertyItem;
                                            i++;
                                        }
                                        ProItem.ProFormat = Property;
                                    }
                                }
                                #endregion

                            }
                            #endregion

                        }
                        #endregion
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion

        public List<Model.Pro_Property> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_Property>()
                                where b.Flag == true
                                select b;
                    if (query == null || query.Count() == 0)
                    {
                        return null;
                    }
                    //System.Collections.ArrayList arr = new System.Collections.ArrayList();
                    //arr.AddRange(query_user);

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



    }
}
