using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 会员身份类别
    /// </summary>
    public class VIP_IDCardType : Sys_InitParentInfo
    {
        private int _MethodID;
        private int MenuID = 51;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public VIP_IDCardType()
        {
            this.MethodID = 0;
        }

        public VIP_IDCardType(int MenthodID)
        {
            this.MethodID = MenthodID;
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
        #region 获取身份类型实体
        /// <summary>
        /// 获取身份类型实体
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

                    var inorder_query = from b in lqh.Umsdb.VIP_IDCardType
                                        //join c in lqh.Umsdb.Sys_Role_Menu_HallInfo on b.Pro_HallID equals c.HallID
                                        //where c.RoleID == user.RoleID && c.MenuID == 12
                                        select b;
                    foreach (var m in param_join)
                    {
                        //此处出现问题拉
                        //if (m.ParamBehind == null)//不存在字段
                        //{
                        //    continue;
                        //}
                        //new Model.ReportSqlParams(){ParamName="InOrderID" },
                        //new Model.ReportSqlParams(){ParamName="Pro_HallID"},
                        //new Model.ReportSqlParams(){ParamName="OldID"}, 
                        //new Model.ReportSqlParams(){ParamName="UserName"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_start"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_end"},
                        //new Model.ReportSqlParams(){ParamName="Note"}

                        switch (m.ParamFront.ParamName)
                        {
                            case "ID":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm.ParamValues.Contains(b.ID.ToString())
                                                    select b;
                                    break;
                                }

                            case "Name":
                                Model.ReportSqlParams_String mm0 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm0.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.Name.Contains(mm0.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "Falg":
                                Model.ReportSqlParams_ListString mm1 = (Model.ReportSqlParams_ListString)m.ParamFront;
                                if (mm1.ParamValues == null || mm1.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm1.ParamValues.Contains(b.Flag.ToString())
                                                    select b;
                                    break;
                                }
                            default: break;
                        }
                    }
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
                        List<Model.VIP_IDCardType> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.VIP_IDCardType> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
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
        #region 添加证件类别
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.VIP_IDCardType model)
        {

            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断数据有效性
                        if (string.IsNullOrEmpty(model.Name))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未填写证件类型名称！" };
                        }
                        #endregion
                        #region 判断是否存在证件类型名称

                        int query = (from b in lqh.Umsdb.VIP_IDCardType
                                     where b.Name == model.Name
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "证件类型已存在" };
                        }
                        #endregion
                        model.Flag = true;
                        lqh.Umsdb.VIP_IDCardType.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "成功添加！" };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion
        #region 修改证件类别
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.VIP_IDCardType model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否存在证件类型名称
                        if (!string.IsNullOrEmpty(model.Name))
                        {
                            int query = (from b in lqh.Umsdb.VIP_IDCardType
                                         where b.Name == model.Name
                                         select b).Count();
                            if (query > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "证件类型已存在！" };
                            }
                        }
                        #endregion
                        var IDCardType = from b in lqh.Umsdb.VIP_IDCardType
                                         where b.ID == model.ID
                                         select b;
                        if (IDCardType.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "证件类型不存在！" };
                        }
                        Model.VIP_IDCardType IDCard = IDCardType.First();
                        if (!string.IsNullOrEmpty(model.Name))
                            IDCard.Name = model.Name;
                        IDCard.Note = model.Note;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = IDCard, ReturnValue = true, Message = "修改成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion


        #region 删除证件类别
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.VIP_IDCardType model)
        {
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        #region 判断是否使用证件类型名称
                        int query = (from b in lqh.Umsdb.VIP_VIPInfo
                                     where b.IDCard_ID == model.ID
                                     select b).Count();
                        if (query > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "证件类型已使用，不能删除！" };
                        }
                        #endregion
                        var IDCardType = from b in lqh.Umsdb.VIP_IDCardType
                                         where b.ID == model.ID
                                         select b;
                        if (IDCardType.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "证件类型不存在！" };
                        }
                        Model.VIP_IDCardType IDCard = IDCardType.First();
                        lqh.Umsdb.VIP_IDCardType.DeleteOnSubmit(IDCard);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = IDCard, ReturnValue = true, Message = "删除成功！" };
                    }
                }
            }
            catch
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错！" };
            }
        }
        #endregion
        public List<Model.VIP_IDCardType> GetList(Model.Sys_UserInfo user, DateTime dt)
        {

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.VIP_IDCardType>()
                                where b.Flag == true
                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.VIP_IDCardType>();
                }
            }
        }

    }


}
