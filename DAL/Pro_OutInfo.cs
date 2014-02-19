using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.Linq;

namespace DAL
{
    /// <summary>
    /// 调拨
    /// </summary>
    public class Pro_OutInfo
    {
        private int _MethodID;

        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Pro_OutInfo()
        {
            this.MethodID = 0;
        }

        public Pro_OutInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
        private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
            new Model.ReportSqlParams_String(){ParamName="OutOrderID" },
            new Model.ReportSqlParams_String(){ParamName="Pro_HallID"},
            new Model.ReportSqlParams_String(){ParamName="OldID"}, 
            new Model.ReportSqlParams_String(){ParamName="UserName"},
            new Model.ReportSqlParams_String(){ParamName="SysDate_start"},
            new Model.ReportSqlParams_String(){ParamName="SysDate_end"},

           new Model.ReportSqlParams_String(){ParamName="ClassName"},
           new Model.ReportSqlParams_String(){ParamName="TypeName"},
           new Model.ReportSqlParams_String(){ParamName="ProName"},

            new Model.ReportSqlParams_String(){ParamName="Note"}
        };

        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }
        #region 获取调入实体
        /// <summary>
        /// 获取调入实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //using (TransactionScope ts = new TransactionScope())
                //{
                try
                {

                    #region 权限

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                    if (!result.ReturnValue)
                    { return result; }

                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0 || pageParam.PageSize == 0)
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

                    var inorder_query = from b in lqh.Umsdb.Pro_OutModel
                                        join c in lqh.Umsdb.Sys_Role_Menu_HallInfo on b.Pro_HallID equals c.HallID
                                        where c.RoleID == user.RoleID
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
                            case "OutOrderID":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OutOrderID.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }

                            case "Aduit":
                                Model.ReportSqlParams_String mm0 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm0.ParamValues))
                                    break;
                                else
                                {
                                    if (mm0.ParamValues == "All")
                                    {
                                        inorder_query = from b in inorder_query
                                                        select b;
                                    }
                                    else
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.Aduit.Contains(mm0.ParamValues)
                                                        select b;
                                    }
                                    break;
                                }
                            case "FromHallName":
                                Model.ReportSqlParams_ListString mm1 = (Model.ReportSqlParams_ListString)m.ParamFront;
                                if (mm1.ParamValues == null || mm1.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm1.ParamValues.Contains(b.FromHallName)
                                                    select b;
                                    break;
                                }

                            case "Pro_HallName":
                                Model.ReportSqlParams_ListString mm2 = (Model.ReportSqlParams_ListString)m.ParamFront;
                                if (mm2.ParamValues == null || mm2.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm2.ParamValues.Contains(b.Pro_HallName)
                                                    select b;
                                    break;
                                }

                            case "OldID":
                                Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm3.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OldID.Contains(mm3.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "UserName":
                                Model.ReportSqlParams_String mm4 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm4.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.FromUserName.Contains(mm4.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "SysDate_start":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm5.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.SysDate >= mm5.ParamValues
                                                    select b;
                                    break;
                                }
                            case "SysDate_end":
                                Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm6.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.SysDate <= mm6.ParamValues
                                                    select b;
                                    break;
                                }
                            case "Note":
                                Model.ReportSqlParams_String mm7 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm7.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.Note.Contains(mm7.ParamValues)
                                                    select b;
                                    break;
                                }
                            default: break;
                        }
                    }
                    #endregion

                    #region 过滤仓库

                    if (ValidHallIDS.Count() > 0)
                        inorder_query = from b in inorder_query
                                        where ValidHallIDS.Contains(b.Pro_HallID)
                                        orderby b.SysDate descending
                                        select b;

                    else
                        inorder_query = from b in inorder_query
                                        orderby b.SysDate descending
                                        select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.Pro_OutModel> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.Pro_OutModel> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
                //}
            }
        }
        #endregion

        #region 获取调出实体
        /// <summary>
        /// 获取调出实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetDCModel(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //using (TransactionScope ts = new TransactionScope())
                //{
                try
                {

                    #region 权限
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0 || pageParam.PageSize == 0)
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

                    var inorder_query = from b in lqh.Umsdb.Pro_OutModel
                                        join r in lqh.Umsdb.Sys_Role_Menu_HallInfo on b.FromHallID equals r.HallID
                                        where r.RoleID == user.RoleID
                                        select b;
                    foreach (var m in param_join)
                    {
                  
                        switch (m.ParamFront.ParamName)
                        {
                            case "OutOrderID":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OutOrderID.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "Aduit":
                                Model.ReportSqlParams_String mm0 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm0.ParamValues))
                                    break;
                                else
                                {
                                    if (mm0.ParamValues == "All")
                                    {
                                        inorder_query = from b in inorder_query
                                                        select b;
                                    }
                                    else
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.Aduit.Contains(mm0.ParamValues)
                                                        select b;
                                    }
                                    break;
                                }
                            case "FromHallName":
                                Model.ReportSqlParams_ListString mm1 = (Model.ReportSqlParams_ListString)m.ParamFront;
                                if (mm1.ParamValues == null || mm1.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm1.ParamValues.Contains(b.FromHallName)
                                                    select b;
                                    break;
                                }

                            case "Pro_HallName":
                                Model.ReportSqlParams_ListString mm2 = (Model.ReportSqlParams_ListString)m.ParamFront;
                                if (mm2.ParamValues == null || mm2.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm2.ParamValues.Contains(b.Pro_HallName)
                                                    select b;
                                    break;
                                }

                            case "OldID":
                                Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm3.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OldID.Contains(mm3.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "UserName":
                                Model.ReportSqlParams_String mm4 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm4.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.FromUserName.Contains(mm4.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "SysDate_start":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm5.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OutDate >= mm5.ParamValues
                                                    select b;
                                    break;
                                }
                            case "SysDate_end":
                                Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm6.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OutDate <= mm6.ParamValues
                                                    select b;
                                    break;
                                }
                            case "Note":
                                Model.ReportSqlParams_String mm7 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm7.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.Note.Contains(mm7.ParamValues)
                                                    select b;
                                    break;
                                }
                            default: break;
                        }
                    }

                    #endregion

                    #region 过滤仓库
                    if (ValidHallIDS.Count() > 0)
                        inorder_query = from b in inorder_query
                                        where ValidHallIDS.Contains(b.FromHallID)
                                        orderby b.SysDate descending
                                        select b;

                    else
                        inorder_query = from b in inorder_query
                                        orderby b.SysDate descending
                                        select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.Pro_OutModel> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.Pro_OutModel> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
                //}
            }
        }
        #endregion

        #region 获取调拨明细
        /// <summary>
        /// 获取调拨明细
        /// </summary>
        /// <param name="user"></param>
        /// <param name="hallid"></param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user, int OutID)
        {
            //返回
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var query = (from b in lqh.Umsdb.View_OutOrderList
                                     where b.OutID == OutID
                                     select b).ToList();
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "无数据" };
                        }
                        return new Model.WebReturn() { Obj = query, ReturnValue = true, Message = "已获取" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false };
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region 获取调拨串码明细
        /// <summary>
        /// 获取调拨串码明细
        /// </summary>
        /// <param name="user"></param>
        /// <param name="hallid"></param>
        /// <returns></returns>
        public Model.WebReturn GetCMList(Model.Sys_UserInfo user, int OutListID)
        {
            //返回
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var query = (from b in lqh.Umsdb.View_OutIMEI
                                     where b.OutListID == OutListID
                                     select b).ToList();
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "无数据" };
                        }
                        return new Model.WebReturn() { Obj = query, ReturnValue = true, Message = "已获取" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false };
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region 新增调拨单

        /// <summary>
        /// 调拨
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_OutInfo model, List<string> s)
        {
            //插入表头 
            //插入明细
            //插入串号明细
            //减少库存
            //更新串号表
            //
            //返回
            if (model == null) return new Model.WebReturn();
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region "验证用户操作仓库  商品的权限 "
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.FromHallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.Pro_HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作 " + que.First().HallName };
                        }

                        #region 检验仓库
                        #endregion 
                        //验证商品权限
                        if (ValidProIDS.Count > 0)
                        {
                            List<string> classids = new List<string>();
                            foreach (var item in model.Pro_OutOrderList)
                            {
                                var queryc = from p in lqh.Umsdb.Pro_ProInfo
                                             where p.ProID == item.ProID.ToString()
                                             select p;
                                classids.Add(queryc.First().Pro_ClassID.ToString());
                            }
                            foreach (var child in classids)
                            {
                                if (!ValidProIDS.Contains(child))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                                }
                            }
                        }
                        #endregion

                        //生成单号 存储过程OrderMacker
                        string msg = null;
                        lqh.Umsdb.OrderMacker(1, "DB", "DB", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成单号出错" };
                        }
                        model.OutOrderID = msg;
                        model.ToDate = DateTime.Now;//服务器时间
                        model.OutDate = DateTime.Now;
                        lqh.Umsdb.Pro_OutInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        List<string> InListIDList = new List<string>();
                        foreach (var i in model.Pro_OutOrderList)
                        {
                            //减库存
                            var minus = from b in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                        where b.InListID == i.InListID && b.HallID == model.FromHallID&&b.ProID==i.ProID
                                        select b;
                            if (minus.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "该仓库库存不足！" };
                            }
                            Model.Pro_StoreInfo store = minus.First();
                            store.ProCount -= i.ProCount;
                            if (store.ProCount < 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "该仓库库存不足！" };
                            }
                            if (!string.IsNullOrEmpty(i.InListID)&!InListIDList.Contains(i.InListID))
                                InListIDList.Add(i.InListID);
                        }
                        int count = (from b in lqh.Umsdb.Pro_InOrderList
                                    where InListIDList.Contains(b.InListID)
                                    select b.InListID).Count();
                        if (count != InListIDList.Count)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分批次号无效！" };
                        }
                        //更新串码表
                        var query = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                    where s.Contains(b.IMEI) //&&(b.OutID==null||b.OutID==0)&&(b.BorowID==null||b.BorowID==0)&&(b.RepairID==null||b.RepairID==0)&&(b.SellID==null||b.SellID==0)
                                    select b;
                       
                        //if (query.Count() != s.Count())
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "存在串码不在库！" };
                        //}
                        foreach (var item in query)
                        {
                            Common.Utils.CheckIMEI(item);
                        }
                        foreach (var imei in query)
                        {
                            imei.OutID = model.ID;
                            imei.State = 1;
                        }
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "调拨成功" };
                    }
                    catch(Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false ,Message=ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 导入调拨单  276
        /// </summary>
        /// <param name="user"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn BatchAdd(Model.Sys_UserInfo user, List<Model.Pro_OutInfo> models)
        {
            //插入表头 
            //插入明细
            //插入串号明细
            //减少库存
            //更新串号表
            //
            //返回
            if (models == null) return new Model.WebReturn();
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region  验证用户操作仓库  商品的权限 

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                        if (!result.ReturnValue)
                        { return result; }

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        List<string> classids = new List<string>();
                        List<string> hallids = new List<string>();

                        var cids = from a in models
                                select a.Pro_OutOrderList into p
                                from d in p
                                join b in lqh.Umsdb.Pro_ProInfo
                                on d.ProID equals b.ProID
                                select b;
                        foreach (var item in cids)
                        {
                            classids.Add(item.Pro_ClassID.ToString());
                        }

                        foreach (var item in models)
                        {
                            hallids.Add(item.FromHallID);
                        }

                        #region 验证仓库

                        if (ValidHallIDS.Count() > 0 )
                        {
                            var que = from h in hallids
                                      join p in lqh.Umsdb.Pro_HallInfo
                                      on h equals p.HallID
                                      where !ValidHallIDS.Contains(h)
                                      select p;
                            if (que.Count() > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作 " + que.First().HallName };
                            }
                        }

                        #endregion

                        #region  验证商品权限

                        if (ValidProIDS.Count > 0)
                        {
                            var que = from c in classids
                                      join p in lqh.Umsdb.Pro_ClassInfo
                                      on c equals p.ClassID.ToString()
                                      where !ValidProIDS.Contains(c)
                                      select p;
                           
                            if (que.Count()>0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = que.First().ClassName + "商品无权操作" };
                            }
                        }

                        #endregion

                        #endregion

                        //生成单号 存储过程OrderMacker

                        List<string> imeis = new List<string>();
                        
                        foreach (var item in models)
                        {
                            string msg = null;
                            lqh.Umsdb.OrderMacker(1, "DB", "DB", ref msg);
                            if (string.IsNullOrEmpty(msg))
                            {
                                return new WebReturn() { ReturnValue = false, Message = "生成单号出错" };
                            }
                            item.SysDate = DateTime.Now;
                            item.OutOrderID = msg;
                            item.OutDate = DateTime.Now;//服务器时间
                            foreach (var child in item.Pro_OutOrderList)
                            {
                                if (child.Pro_OutOrderIMEI != null)
                                {
                                    foreach (var xxd in child.Pro_OutOrderIMEI)
                                    {
                                        imeis.Add(xxd.IMEI);
                                    }
                                }
                            }
                        }
                        //验证串码
                        var query = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                    where imeis.Contains(b.IMEI) 
                                    //&&( (b.OutID > 0) ||
                                    //(b.BorowID > 0) ||
                                    //(b.RepairID > 0) ||
                                    //(b.SellID > 0) ||
                                    // (b.VIPID > 0) ||
                                    //(b.AuditID > 0) ||
                                    //(b.AssetID > 0) ||
                                    //(b.BJID > 0) || (b.PJID > 0))
                                    select b;
                        //if (query.Count() >0 )
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "有部分串码存在其他操作，调拨失败！" };
                        //}
                        foreach (var item in query)
                        {
                             WebReturn ret2 =  Common.Utils.CheckIMEI(item);
                             if (ret2.ReturnValue == false)
                             {
                                 return ret2;
                             }
                        }


                        lqh.Umsdb.Pro_OutInfo.InsertAllOnSubmit(models);
                        lqh.Umsdb.SubmitChanges();

                        #region  提交完成后更新串码表OutID

                        if (imeis.Count != 0)
                        {
                            foreach (var item in models)
                            {
                                var imeiList = from a in lqh.Umsdb.Pro_OutInfo
                                               join c in lqh.Umsdb.Pro_OutOrderList
                                               on a.ID equals c.OutID
                                               join d in lqh.Umsdb.Pro_OutOrderIMEI
                                               on c.OutListID equals d.OutListID
                                               join b in lqh.Umsdb.Pro_IMEI
                                               on d.IMEI equals b.IMEI
                                               where imeis.Contains(b.IMEI) && a.ID == item.ID
                                                && (b.OutID == null || b.OutID == 0) &&
                                                (b.BorowID == null || b.BorowID == 0) &&
                                                (b.RepairID == null || b.RepairID == 0) &&
                                                (b.SellID == null || b.SellID == 0) &&
                                                 (b.VIPID == null || b.VIPID == 0) && (b.AssetID == null || b.AssetID == 0)
                                               select b;
                                if (imeiList.Count() == 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "有部分串码存在其他操作，调拨失败！" };
                                }
                                else
                                {
                                    foreach (var child in imeiList)
                                    {
                                        child.OutID = item.ID;
                                        child.State = 1;
                                    }
                                }
                            }
                        }

                        #endregion 

                        List<string> InListIDList = new List<string>();
                        foreach (var child in models)
                        {
                            foreach (var item in child.Pro_OutOrderList)
                            {
                                //减库存
                                var minus = from b in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                            where b.HallID == child.FromHallID && b.ProID == item.ProID
                                            && b.InListID == item.InListID
                                            && b.ProCount>0
                                            select b;
                                if (minus.Count() == 0)
                                {
                                    var h = from a in lqh.Umsdb.Pro_HallInfo
                                            where a.HallID == child.FromHallID
                                            select a;
                                    return new Model.WebReturn() { ReturnValue = false, Message = h.First().HallName + " 库存不足！" };
                                }

                                decimal pcount = item.ProCount;
                                foreach (var dd in minus)
                                {
                                    if (dd.ProCount > pcount) //库存充足直接减
                                    {
                                        dd.ProCount -= pcount;
                                        pcount = 0;
                                        break;
                                    }
                                    else
                                    {
                                        pcount -= dd.ProCount;
                                        dd.ProCount = 0;
                                    }
                                }
                                
                                if (pcount > 0)
                                {
                                    var h = from a in lqh.Umsdb.Pro_HallInfo
                                            where a.HallID == child.FromHallID
                                            select a;
                                    return new Model.WebReturn() { ReturnValue = false, Message = h.First().HallName + " 库存不足！" };
                                }
                            }
                        }
                        //int count = (from b in lqh.Umsdb.Pro_InOrderList
                        //             where InListIDList.Contains(b.InListID)
                        //             select b.InListID).Count();
                        //if (count != InListIDList.Count)
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "部分批次号无效，调拨失败！" };
                        //}
                       
                  
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "调拨成功！" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                    }
                }
            }
        }

        #endregion

        #region 单个接受
        /// <summary>
        /// 单个接受 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Accept(Model.Sys_UserInfo user, Model.Pro_OutInfo model)
        {
            //更新调拨单审核信息 
            //加库存
            //更新串号表
            //
            //返回
            if (model.Audit == true)
                return new WebReturn() { ReturnValue = false, Message = "已接收，不能进行其它操作" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {

                        DataLoadOptions dataload = new DataLoadOptions();
                        dataload.LoadWith<Model.Pro_OutInfo>(c => c.Pro_OutOrderList);
                        dataload.LoadWith<Model.Pro_OutOrderList>(c => c.Pro_OutOrderIMEI);
                        lqh.Umsdb.LoadOptions = dataload;
                        #region 验证用户操作仓库  商品的权限
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.Pro_HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.Pro_HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                        }

                        //验证商品权限
                        if (ValidProIDS.Count > 0)
                        {
                            List<string> classids = new List<string>();
                            foreach (var item in model.Pro_OutOrderList)
                            {
                                var queryc = from p in lqh.Umsdb.Pro_ProInfo
                                             where p.ProID == item.ProID.ToString()
                                             select p;
                                classids.Add(queryc.First().Pro_ClassID.ToString());
                            }
                            foreach (var child in classids)
                            {
                                if (!ValidProIDS.Contains(child))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                                }
                            }
                        }
                        #endregion

                        #region 表头


                        var query = from b in lqh.Umsdb.Pro_OutInfo
                                    where b.OutOrderID == model.OutOrderID&&(b.IsDelete==null||b.IsDelete==false)
                                    select b;
                        if (query.Count() == 0 || query == null)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "不存在调拨单" };
                        }
                        Model.Pro_OutInfo outHead = new Model.Pro_OutInfo();
                        outHead = query.First();
                        if (outHead.Audit == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "调拨单不能重复接收！" };
                        }
                        outHead.ToDate = DateTime.Now;
                        outHead.ToUserID = user.UserID;
                        outHead.Audit = true;
                        #endregion

                        #region 明细
                        if (outHead.Pro_OutOrderList == null || outHead.Pro_OutOrderList.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "单据的商品明细为空" };
                        }
                        foreach (var next in outHead.Pro_OutOrderList)
                        {
                            if (next.ProCount == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "调拨数量为0！" };
                            }
                            var queryStore = from b in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                             where b.InListID == next.InListID && b.ProID == next.ProID && b.HallID == outHead.Pro_HallID
                                             select b;
                            //该仓库不存在该批次
                            if (queryStore.Count() == 0)
                            {
                                //更新库存

                                Model.Pro_StoreInfo store = new Model.Pro_StoreInfo();
                                store.InListID = next.InListID;
                                store.ProID = next.ProID;
                                store.HallID = outHead.Pro_HallID;
                                store.ProCount = next.ProCount;
                                lqh.Umsdb.Pro_StoreInfo.InsertOnSubmit(store);
                            }
                            else
                            {
                                //更新库存
                                Model.Pro_StoreInfo store1 = queryStore.First();
                                store1.ProCount += next.ProCount;
                                //更新串码表                          
                            }
                            var InlistIMEI = (from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                              where b.InListID == next.InListID && b.OutID == model.ID
                                              select b).ToList();

                            if (InlistIMEI.Count() != next.Pro_OutOrderIMEI.Count())
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "串码数量不一致！" };
                            }
                            foreach (var InIMEI in InlistIMEI)
                            {
                                InIMEI.OutID = null;
                                InIMEI.State = 0;
                                InIMEI.HallID = outHead.Pro_HallID;
                            }
                        }
                        #endregion
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "接受成功" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false };
                    }
                }
            }
        }
        #endregion

        #region 单个取消
        /// <summary>
        /// 单个取消
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //验证是否超时，user中有功能的操作时限，Admin除外，也即是 roleid=1
        //更新调拨单审取消信息
        //加库存
        //更新串号表
        //
        //返回
        public Model.WebReturn Cancel(Model.Sys_UserInfo user, Model.Pro_OutInfo model)
        {

            if (model.Audit == true)
                return new WebReturn() { ReturnValue = false, Message = "已接收，不能进行其它操作" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        DataLoadOptions dataload = new DataLoadOptions();
                        dataload.LoadWith<Model.Pro_OutInfo>(c => c.Pro_OutOrderList);
                        dataload.LoadWith<Model.Pro_OutOrderList>(c => c.Pro_OutOrderIMEI);
                        lqh.Umsdb.LoadOptions = dataload;
                        #region 验证用户操作仓库  商品的权限
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.FromHallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.Pro_HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                        }

                        //验证商品权限
                        if (ValidProIDS.Count() > 0)
                        {
                            List<string> classids = new List<string>();
                            foreach (var item in model.Pro_OutOrderList)
                            {
                                var queryc = from p in lqh.Umsdb.Pro_ProInfo
                                             where p.ProID == item.ProID.ToString()
                                             select p;
                                classids.Add(queryc.First().Pro_ClassID.ToString());
                            }
                            foreach (var child in classids)
                            {
                                if (!ValidProIDS.Contains(child))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                                }
                            }
                        }
                        #endregion

                        #region 验证取消时限
                        try
                        {
                            TimeSpan a = new TimeSpan((int)user.CancelLimit, 0, 0, 0);
                            if (user.UserID != "1" && DateTime.Now.Subtract(a) > model.OutDate)
                            {
                                string Msg = "已超时，该操作无法取消";
                                return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = Msg };
                            }
                        }
                        catch
                        { }
                        #endregion

                        #region 表头


                        var query = (from b in lqh.Umsdb.Pro_OutInfo
                                     where b.OutOrderID == model.OutOrderID && (b.IsDelete == null || b.IsDelete == false)
                                     select b).ToList();
                        if (query.Count() == 0 || query == null)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "不存在调拨单" };
                        }

                        Model.Pro_OutInfo outHead = new Model.Pro_OutInfo();
                        outHead = query.First();
                        if (outHead.Audit == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "调拨单已接收，无法取消,再次查询刷新状态！" };
                        }
                        if (outHead.IsDelete == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "调拨单已取消，无法再次取消！" };
                        }
                        outHead.IsDelete = true;
                        outHead.DeleteDate = model.DeleteDate == null ? DateTime.Now : model.DeleteDate;
                        outHead.Deleter = user.UserID;
                        #endregion
                        #region 明细
                        List<Model.Pro_OutOrderIMEI> total = new List<Pro_OutOrderIMEI>();
                        foreach (var next in outHead.Pro_OutOrderList)
                        {
                            var queryStore = from b in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                             where b.InListID == next.InListID && b.ProID == next.ProID && b.HallID == outHead.FromHallID
                                             select b;
                            //库存不存在该批次
                            if (queryStore == null || queryStore.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "库存出错，请联系管理员" };
                            }
                            //更新库存
                            Model.Pro_StoreInfo store = queryStore.First();
                            store.ProCount += next.ProCount;

                            if (next.Pro_OutOrderIMEI != null)
                                total.AddRange(next.Pro_OutOrderIMEI);
                        }
                        var InlistIMEI = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                         where b.OutID == outHead.ID
                                         select b;
                        if (InlistIMEI.Count() != total.Count())
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分串码不存在，请联系管理员" };
                        }
                        foreach (var InIMEI in InlistIMEI)
                        {
                            InIMEI.OutID = null;
                            InIMEI.State = 0;
                        }
                        #endregion
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "取消成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }
        #endregion


        #region 调拨单拣货  275

        public Model.WebReturn ProCheck(Model.Sys_UserInfo user,List<Model.OutImportModel> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region  验证仓库是否存在

                    List<string> oldlist = new List<string>();
                    foreach (var item in models)
                    {
                        if (!oldlist.Contains(item.ToHall))
                        {
                            oldlist.Add(item.ToHall);
                        }
                        if (!oldlist.Contains(item.FromHall))
                        {
                            oldlist.Add(item.FromHall);
                        }
                    }

                    var list = from a in oldlist
                               join h in lqh.Umsdb.Pro_HallInfo
                               on a equals h.HallName
                               select new 
                               {
                                   h.HallName
                               };
                    List<string> halls = new List<string>();
                    foreach (var item in list)
                    {
                        if (!halls.Contains(item.HallName))
                        {
                            halls.Add(item.HallName);
                        }
                    }

                    var diff = from a in oldlist
                               where !halls.Contains(a)
                               select a;
                    if (diff.Count() > 0)
                    {
                        string msg = "";
                        int indexs = 1;
                        foreach (var item in diff)
                        {
                            msg += item;
                            if (indexs < diff.Count())
                            {
                                msg += " , ";
                            }
                            indexs++;
                        }
                        return new WebReturn() { ReturnValue = false, Message = "仓库不存在：" + msg+"。拣货失败！" };
                    }

                    #endregion 

                    #region 权限验证

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                    if (!result.ReturnValue)
                    { return result; }

                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #region  过滤仓库
                    //验证仓库权限
                    var hallids = (from a in models
                                   join h in lqh.Umsdb.Pro_HallInfo
                                   on a.FromHall equals h.HallName
                                   select h).Distinct().ToList();

                    if (ValidHallIDS.Count > 0)
                    {
                        foreach (var item in hallids)
                        {
                            if (!ValidHallIDS.Contains(item.HallID))
                            {
                                var que = from h in lqh.Umsdb.Pro_HallInfo
                                          where h.HallID == item.HallID
                                          select h;
                                return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                            }
                        }
                    }
                    #endregion

                    #region  过滤商品

                    //验证商品权限
                    var proclassids = (from a in models
                                       join p in lqh.Umsdb.Pro_ProInfo
                                       on a.ProID equals p.ProID
                                       join c in lqh.Umsdb.Pro_ClassInfo
                                       on p.Pro_ClassID equals c.ClassID
                                       where !string.IsNullOrEmpty(a.ProID)
                                       select c.ClassID).Distinct().ToList();

                    var proclassids2 = (from a in models
                                        join m in lqh.Umsdb.Pro_IMEI
                                        on a.IMEI equals m.IMEI
                                       join p in lqh.Umsdb.Pro_ProInfo
                                       on m.ProID equals p.ProID
                                       join c in lqh.Umsdb.Pro_ClassInfo
                                       on p.Pro_ClassID equals c.ClassID
                                       where !string.IsNullOrEmpty(a.IMEI)
                                       select c.ClassID).Distinct().ToList();
                    proclassids.AddRange(proclassids2);
                
                    if (ValidProIDS.Count > 0)
                    {
                        foreach (var item in proclassids)
                        {
                            if (!ValidProIDS.Contains(item.ToString()))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                            }
                        }
                    }
                    #endregion

                    #endregion 

                    List<OutImportModel> retList = new List<OutImportModel>();
                    List<OutImportModel> checkTotalList = new List<OutImportModel>();

                

                    #region  验证串码

                    List<string> imeis = new List<string>();

                    var valimei = from a in models
                                  where !string.IsNullOrEmpty(a.IMEI)
                                  select a;
                    foreach (var item in valimei)
                    {
                       imeis.Add(item.IMEI);
                    }
                    if (imeis.Count > 0)
                    {
                        var valIMEIs = from e in lqh.Umsdb.Pro_IMEI
                                       where imeis.Contains(e.IMEI)
                                       && Nullable.Equals(e.OutID, null) && Nullable.Equals(e.BorowID, null)
                                       && Nullable.Equals(e.RepairID, null) && Nullable.Equals(e.VIPID, null)
                                       && Nullable.Equals(e.SellID, null) && Nullable.Equals(e.AssetID,null)
                                       select e;
                        if (valIMEIs.Count() != imeis.Count)
                        {
                            List<string> ilist = new List<string>();
                            foreach (var ss in valIMEIs)
                            {
                                ilist.Add(ss.IMEI);
                            }
                            var i = from a in imeis
                                    where !ilist.Contains(a)
                                    select a;
                            string msg = " ";
                            int ct = 0;
                            foreach (var xx in i)
                            {
                                ct++;
                                msg += xx;
                                if (ct < i.Count())
                                {
                                    msg += " , ";
                                }
                            }
                            if (i.Count() > 0)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "以下串码存在其他操作：" + msg };
                            }
                        }
                    }

                    #endregion 

                    #region  收集非串码  各个仓库各商品的总量

                    var pross2 = (from d in models
                                 join a in lqh.Umsdb.Pro_ProInfo
                                 on d.ProID equals a.ProID
                                 join c in lqh.Umsdb.Pro_ClassInfo
                                 on a.Pro_ClassID equals c.ClassID
                                 join t in lqh.Umsdb.Pro_TypeInfo
                                  on a.Pro_TypeID equals t.TypeID
                                  join h in lqh.Umsdb.Pro_HallInfo
                                  on d.FromHall equals h.HallName
                                  join f in lqh.Umsdb.Pro_HallInfo
                                  on d.ToHall equals f.HallName
                                 where  a.NeedIMEI == false 
                                 select new
                                 {
                                     d.OldID,       d.ProCount,
                                     c.ClassName,   a.NeedIMEI,
                                     d.Note,        a.ProName,
                                     FromHallName = h.HallName,FromHallID=h.HallID,
                                     ToHall = f.HallName,
                                     t.TypeID,     a.ProID,
                                     d.FromUser,    d.IMEI,
                                     t.TypeName,   c.ClassID,
                                 }).Distinct().ToList();

                    foreach (var child in pross2)
                    {
                        AddTotal(checkTotalList,child.FromHallID,child.ProID,
                            child.ProCount,child.FromHallName,child.ProName);
                    }

                    #endregion

                    #region 收集串码    各个仓库各商品的总量

                    var IMEIs2 = from d in models
                                   join e in lqh.Umsdb.Pro_IMEI
                                   on d.IMEI.ToUpper() equals e.IMEI.ToUpper()   //根据串码获取商品编码
                                   join a in lqh.Umsdb.Pro_ProInfo
                                   on e.ProID equals a.ProID
                                   join c in lqh.Umsdb.Pro_ClassInfo
                                   on a.Pro_ClassID equals c.ClassID
                                   join t in lqh.Umsdb.Pro_TypeInfo
                                   on a.Pro_TypeID equals t.TypeID
                                   join s in lqh.Umsdb.Pro_StoreInfo
                                   on a.ProID equals s.ProID
                                   join h in lqh.Umsdb.Pro_HallInfo
                                   on d.FromHall equals h.HallName
                                 join to in lqh.Umsdb.Pro_HallInfo
                                 on d.ToHall equals to.HallName
                                 where h.Flag == true && a.NeedIMEI == true && s.ProCount > 0
                                 && e.IMEI == d.IMEI && s.HallID == h.HallID && e.InListID == s.InListID
                                 && !string.IsNullOrEmpty(d.IMEI)
                                   select new
                                   {
                                       d.OldID,    c.ClassID,
                                       d.Note,     c.ClassName,
                                       d.FromUser, t.TypeID,
                                       d.ProCount, t.TypeName,
                                       a.ProName,  a.NeedIMEI,
                                       d.IMEI,     s.ProID,
                                       s.InListID,
                                       StoreCount = s.ProCount,
                                       FromHallID = h.HallID,
                                      ToHallID = to.HallID,
                                       FromHallName = h.HallName,
                                      ToHallName = to.HallName
                                   };

                       foreach (var item in IMEIs2)
                       {
                           AddTotal(checkTotalList, item.FromHallID, item.ProID, 1 
                               ,item.FromHallName,item.ProName);
                       }

                   #endregion 

                    #region  检验调拨总库存是否充足
                    foreach(var item in checkTotalList)
                    {
                        var valcount = (from a in lqh.Umsdb.Pro_StoreInfo
                                        where a.ProID == item.ProID && a.HallID == item.FromHallID
                                        select a.ProCount).Sum();
                        if (valcount < item.ProCount)
                        {
                            return new WebReturn() { ReturnValue=false,Message=item.FromHall+" 商品 "+item.ProName+" 库存不足！"};
                        }
                    }
                    #endregion 

                    #region 拣货成功后 获取返回串码数据
                    List<string> imeiList = new List<string>();
                   
                   var IMEIs = from d in models
                            join e in lqh.Umsdb.Pro_IMEI
                            on d.IMEI equals e.IMEI   //根据串码获取商品编码
                            join a in lqh.Umsdb.Pro_ProInfo
                            on e.ProID equals a.ProID
                            join c in lqh.Umsdb.Pro_ClassInfo
                            on a.Pro_ClassID equals c.ClassID
                            join t in lqh.Umsdb.Pro_TypeInfo
                            on a.Pro_TypeID equals t.TypeID
                        join s in lqh.Umsdb.Pro_StoreInfo
                        on a.ProID equals s.ProID 
                        join h in lqh.Umsdb.Pro_HallInfo
                        on d.FromHall equals h.HallName
                        join to in lqh.Umsdb.Pro_HallInfo
                        on d.ToHall equals to.HallName
                        where  h.Flag == true && a.NeedIMEI==true && s.ProCount>0
                        && e.IMEI == d.IMEI && Nullable.Equals(e.OutID, null) && Nullable.Equals(e.BorowID, null)
                        && Nullable.Equals(e.RepairID, null) && Nullable.Equals(e.VIPID , null) && Nullable.Equals(e.AssetID,null)
                        &&  Nullable.Equals(e.SellID,null) && !string.IsNullOrEmpty(d.IMEI)
                        && s.HallID == h.HallID && e.InListID == s.InListID
                        select new
                        {
                            d.OldID,     c.ClassID,
                            d.Note,      c.ClassName,
                            d.FromUser,  t.TypeID,
                            d.ProCount,  t.TypeName,
                            a.ProName,   a.NeedIMEI,
                            d.IMEI,      s.ProID,
                            s.InListID,StoreCount = s.ProCount,
                            FromHallID = h.HallID,
                            ToHallID = to.HallID,
                            FromHallName = h.HallName,
                            ToHallName = to.HallName
                        };
                   OutImportModel om = null;
                    foreach (var item in IMEIs)
                    {
                        imeiList.Add(item.IMEI);
                        om = new OutImportModel();
                        om.NeedIMEI = true;
                        om.ProID = item.ProID;
                        om.FromHall = item.FromHallName;
                        om.ToHall = item.ToHallName;
                        om.ProName = item.ProName;
                        om.OldID = item.OldID;
                        om.InListID = item.InListID;
                        om.ProCount = 1;
                        om.IMEI = item.IMEI;
                        om.FromHallID = item.FromHallID;
                        om.ToHallID = item.ToHallID;
                        var imei = from a in lqh.Umsdb.Pro_IMEI
                                    where a.IMEI == item.IMEI && !string.IsNullOrEmpty(a.RepairID.ToString())
                                    select a;
                        om.Success = true;
                        om.CheckNote = "成功";
                        retList.Add(om);
                    }

                    #endregion 

                    #region  拣货成功后 获取非串码数据

                    var pross = (from d in models
                                 join a in lqh.Umsdb.Pro_ProInfo
                                 on d.ProID equals a.ProID
                                 join c in lqh.Umsdb.Pro_ClassInfo
                                 on a.Pro_ClassID equals c.ClassID
                                 join t in lqh.Umsdb.Pro_TypeInfo
                                  on a.Pro_TypeID equals t.TypeID
                                 join h in lqh.Umsdb.Pro_HallInfo
                                 on d.FromHall equals h.HallName
                                 join s in lqh.Umsdb.Pro_StoreInfo
                                  on new { a.ProID, h.HallID } equals new { s.ProID, s.HallID }
                                 join to in lqh.Umsdb.Pro_HallInfo
                                 on d.ToHall equals to.HallName
                                 where h.Flag == true && a.NeedIMEI == false && s.ProCount > 0
                                 && s.HallID == h.HallID
                                 select new
                                 {
                                     d.OldID,        t.TypeID,
                                     d.ProCount,     s.ProID,
                                     c.ClassName,    d.FromUser,
                                     a.NeedIMEI,     d.IMEI,
                                     d.Note,         t.TypeName,
                                     a.ProName,      s.InListID, c.ClassID,
                                     StoreCount = s.ProCount,
                                     FromHallID = h.HallID,
                                     ToHallID = to.HallID,
                                     FromHallName = h.HallName,
                                     ToHallName = to.HallName
                                 }).Distinct().ToList();

                    List<string> pros = new List<string>();

                    foreach (var ff in pross)
                    {
                        var val = from a in models
                                  where a.ProID==ff.ProID && a.FromHall==ff.FromHallName
                                  && a.ProCount>0  //&& a.ToHall == ff.ToHallName
                                  select a ;
                        decimal scount = ff.StoreCount;
                        foreach (var item in val)
                        {
                            var h = from a in lqh.Umsdb.Pro_HallInfo
                                    where a.HallName == item.ToHall
                                    select a;
                            Model.Pro_HallInfo tohall = h.First();

                            if (item.ProCount -scount >= 0)
                            {
                                om = new OutImportModel();
                                om.ProCount = scount;
                                item.ProCount -= scount;
                                om.ProID = ff.ProID;
                                om.ProName = ff.ProName;
                                om.OldID = ff.OldID;
                                om.InListID = ff.InListID;
                                om.FromHallID = ff.FromHallID;
                                om.FromHall = ff.FromHallName;
                                om.ToHall = tohall.HallName;
                                om.ToHallID = tohall.HallID;
                                om.Success = true;
                                om.CheckNote = "成功";
                                retList.Add(om);
                                break;
                            }
                            else
                            {
                                om = new OutImportModel();
                                om.ProCount = item.ProCount;
                                scount -= item.ProCount;
                                item.ProCount = 0;
                                om.ProID = ff.ProID;
                                om.ProName = ff.ProName;
                                om.OldID = ff.OldID;
                                om.InListID = ff.InListID;
                                om.FromHallID = ff.FromHallID;
                                om.FromHall = ff.FromHallName;
                                om.ToHall = tohall.HallName;
                                om.ToHallID = tohall.HallID;
                                om.Success = true;
                                om.CheckNote = "成功";
                                retList.Add(om);
                            }
                        }
                     
                    }
                    //foreach (var xxs in models)
                    //{
                    //    decimal count = xxs.ProCount;
                    //    var valcount = from a in pross
                    //                   where a.FromHallName == xxs.FromHall && a.ProID == xxs.ProID
                    //                      && a.ToHallName == xxs.ToHall
                    //                      select a;
                    //    foreach (var ff in valcount)
                    //    {
                    //        decimal remindc = Exist(retList, ff.FromHallID, ff.ProID, ff.ProCount, ff.InListID);
                    //        if (remindc == 0)
                    //        {
                                  
                    //        }
                    //        if (count > ff.StoreCount)
                    //        {
                    //            om = new OutImportModel();
                    //            om.ProCount = ff.StoreCount;
                    //            count -= ff.StoreCount;
                                
                    //            om.ProID = ff.ProID;
                    //            om.ProName = ff.ProName;
                    //            om.OldID = ff.OldID;
                    //            om.InListID = ff.InListID;
                    //            om.FromHallID = ff.FromHallID;
                    //            om.FromHall = ff.FromHallName;
                    //            om.ToHall = ff.ToHallName;
                    //            om.ToHallID = ff.ToHallID;
                    //            om.Success = true;
                    //            om.CheckNote = "成功";
                    //            retList.Add(om);
                    //            if (count <= 0)
                    //            { break; }
                    //        }
                    //        else
                    //        {
                    //            om = new OutImportModel();
                    //            om.ProCount =count;
                    //            om.ProID = ff.ProID;
                    //            om.ProName = ff.ProName;
                    //            om.OldID = ff.OldID;
                    //            om.InListID = ff.InListID;
                    //            om.FromHallID = ff.FromHallID;
                    //            om.FromHall = ff.FromHallName;
                    //            om.ToHall = ff.ToHallName;
                    //            om.ToHallID = ff.ToHallID;
                    //            om.Success = true;
                    //            om.CheckNote = "成功";
                    //            retList.Add(om);
                    //            break;
                    //        }
                    //    }
                    //}

                    #endregion 

                    #region  获取拣货不成功的非串码数据

                    //if (pros.Count > 0)
                    //{
                    //    var remines = from a in models
                    //                  join p in lqh.Umsdb.Pro_ProInfo
                    //                  on a.ProID equals p.ProID
                    //                  where !pros.Contains(a.ProID)
                    //                  && p.NeedIMEI == false
                    //                  select a;

                    //    foreach (var item in remines)
                    //    {
                    //        om = new OutImportModel();
                    //        om.ProID = item.ProID;
                    //        om.ProName = item.ProName;
                    //        om.OldID = item.OldID;
                    //        om.InListID = item.InListID;
                    //        om.ProCount = item.ProCount;
                    //        om.FromHallID = item.FromHallID;
                    //        om.FromHall = item.FromHall;
                    //        om.NeedIMEI = false;
                    //        om.ToHall = item.ToHall;
                    //        om.ToHallID = item.ToHallID;
                    //        om.CheckNote = "仓库不存在此商品";
                    //        om.Success = false;
                    //        retList.Add(om);
                    //    }
                    //}
                    #endregion

                    return new WebReturn() { ReturnValue = true,Obj = retList };
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue = false,Message = ex.Message};
                }
            }
        }

        private decimal Exist(List<OutImportModel> models ,string hallid,string proid,decimal procount,string inlistid)
        {
            var ss =( from a in models
                     where a.InListID == inlistid && a.FromHallID == hallid
                     && a.ProID == proid
                     select a.ProCount).Sum();
            return ss == 0 ? ss : procount - ss;

        }


        private void AddTotal(List<OutImportModel> checkTotalList,string fromhallid,
            string proid,decimal count,string hall,string proname)
        {
            if (checkTotalList.Count == 0)
            {
                OutImportModel mm = new OutImportModel();
                mm.FromHallID = fromhallid;
                mm.ProID = proid;
                mm.ProCount = count;
                mm.FromHall = hall;
                mm.ProName = proname;
                checkTotalList.Add(mm);
                return;
            }
            bool finded = false;
            foreach (var xx in checkTotalList)
            {
                if (xx.FromHallID == fromhallid && xx.ProID == proid)
                {
                    finded = true;
                    xx.ProCount += count;
                    break;
                }
            }
            if (!finded)
            {
                OutImportModel mm = new OutImportModel();
                mm.FromHallID = fromhallid;
                mm.ProID = proid;
                mm.ProCount = count;
                mm.FromHall = hall;
                mm.ProName = proname;
                checkTotalList.Add(mm);
            }
        }

        #endregion 

        #region 查询明细
        /// <summary>
        /// 查询调拨明细
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam, bool IsSearch)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //using (TransactionScope ts = new TransactionScope())
                //{
                try
                {

                    #region 权限
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (ret.ReturnValue != true)
                    { return ret; }

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

                    var inorder_query = from b in lqh.Umsdb.View_OutSearch
                                        join c in lqh.Umsdb.Sys_Role_Menu_HallInfo on b.Pro_HallID equals c.HallID
                                        where c.RoleID == user.RoleID && c.MenuID == 12
                                        select b;
                    foreach (var m in param_join)
                    {
                        switch (m.ParamFront.ParamName)
                        {
                            case "OutOrderID":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OutOrderID.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }

                            case "Aduit":
                                Model.ReportSqlParams_String mm0 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm0.ParamValues))
                                    break;
                                else
                                {
                                    if (mm0.ParamValues == "All")
                                    {
                                        inorder_query = from b in inorder_query
                                                        select b;
                                    }
                                    else
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.Aduit.Contains(mm0.ParamValues)
                                                        select b;
                                    }
                                    break;
                                }
                            case "FromHallName":
                                Model.ReportSqlParams_ListString mm1 = (Model.ReportSqlParams_ListString)m.ParamFront;
                                if (mm1.ParamValues == null || mm1.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm1.ParamValues.Contains(b.FromHallName)
                                                    select b;
                                    break;
                                }

                            case "Pro_HallName":
                                Model.ReportSqlParams_ListString mm2 = (Model.ReportSqlParams_ListString)m.ParamFront;
                                if (mm2.ParamValues == null || mm2.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mm2.ParamValues.Contains(b.Pro_HallName)
                                                    select b;
                                    break;
                                }

                            case "OldID":
                                Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm3.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OldID.Contains(mm3.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "UserName":
                                Model.ReportSqlParams_String mm4 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm4.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.FromUserName.Contains(mm4.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "SysDate_start":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm5.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.SysDate >= mm5.ParamValues
                                                    select b;
                                    break;
                                }
                            case "SysDate_end":
                                Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm6.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.SysDate <= mm6.ParamValues
                                                    select b;
                                    break;
                                }

                            case "ClassName":
                                Model.ReportSqlParams_String mm61 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (mm61.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.ClassName == mm61.ParamValues
                                                    select b;
                                    break;
                                }
                            case "TypeName":
                                Model.ReportSqlParams_String mm62 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (mm62.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.TypeName == mm62.ParamValues
                                                    select b;
                                    break;
                                }
                            case "ProName":
                                Model.ReportSqlParams_String mm63 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (mm63.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.ProName == mm63.ParamName
                                                    select b;
                                    break;
                                }


                            case "Note":
                                Model.ReportSqlParams_String mm7 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm7.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.Note.Contains(mm7.ParamValues)
                                                    select b;
                                    break;
                                }
                            default: break;
                        }
                    }
                    #endregion

                    #region 过滤仓库
                    if (ValidHallIDS.Count() > 0)
                        inorder_query = from b in inorder_query
                                        where ValidHallIDS.Contains(b.Pro_HallID)
                                        orderby b.SysDate descending
                                        select b;

                    else
                        inorder_query = from b in inorder_query
                                        orderby b.SysDate descending
                                        select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.View_OutSearch> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        List<Model.View_OutSearch> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
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

    }
}
