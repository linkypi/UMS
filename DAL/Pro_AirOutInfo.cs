using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class Pro_AirOutInfo
    {
        private int _MethodID;

        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Pro_AirOutInfo()
        {
            this.MethodID = 0;
        }

        public Pro_AirOutInfo(int MenthodID)
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
        #region 新增空冲调拨单
        /// <summary>
        /// 转类别
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_AirOutInfo model)
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
                                      where h.HallID == model.FromHallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作 " + que.First().HallName };
                        }
                        #endregion

                        #region 插入表头
                        string message = "";
                        lqh.Umsdb.OrderMacker(1, "DB", "DB", ref message);
                        if (string.IsNullOrEmpty(message))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成编号出错" };
                        }
                        model.FromUserID = user.UserID;
                        model.SysDate = DateTime.Now;
                        model.UserID = user.UserID;
                        model.OutDate = DateTime.Now;
                        model.OutOrderID = message;
                        #endregion

                        #region 添加到Pro_AirOutListInfo
                        string msg = "";
                        lqh.Umsdb.OrderMacker(model.Pro_AirOutListInfo.Count(), "CL", "CL", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成编号出错" };
                        }
                        string[] InListIDStr = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (InListIDStr.Count() != model.Pro_AirOutListInfo.Count())
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成商品编号数量与商品数量不一致" };
                        }


                        List<string> ProList = new List<string>();
                        for (int i = 0; i < model.Pro_AirOutListInfo.Count(); i++)
                        {
                            model.Pro_AirOutListInfo[i].NewInListID = InListIDStr[i];
                            if(!ProList.Contains(model.Pro_AirOutListInfo[i].NewProID))
                                 ProList.Add(model.Pro_AirOutListInfo[i].NewProID);      
                        }

                        //var NewStore = from b in lqh.Umsdb.Pro_StoreInfo
                        //               where ProList.Contains(b.ProID) && b.HallID == model.Pro_HallID && b.ProCount >= 0                             
                        //               select b;
                        //if(NewStore.Count()==0)
                        //{
                        //      return new WebReturn() { ReturnValue = false, Message = "空冲号码不存在该仓库或无库存！" };
                        //}
                        //var NewProList = (from b in NewStore
                        //                  select b.ProID).Distinct();
                        //if (ProList.Count() != NewProList.Count())
                        //{
                        //    return new WebReturn() { ReturnValue = false, Message = "部分空冲号码不存在该仓库或无库存！" };
                        //}
                        lqh.Umsdb.Pro_AirOutInfo.InsertOnSubmit(model);
                        #endregion

                        #region 更新库存
                        foreach (var Item in model.Pro_AirOutListInfo)
                        {
                            #region 新增入库明细(接收空冲调拨才需要)
                            //var queryInOrder = (from b in lqh.Umsdb.Pro_InOrderList
                            //                    where b.InListID == Item.InListID
                            //                    select b).ToList();
                            //if (queryInOrder.Count() != 1)
                            //{
                            //    return new Model.WebReturn() { ReturnValue = false, Message = "获取原批次号出错，请联系管理员！" };
                            //}
                            //Model.Pro_InOrderList NewList = new Pro_InOrderList();
                            //// NewList.Price = queryInOrder[0].Price;
                            //NewList.InOrderID = queryInOrder[0].InOrderID;
                            //NewList.Pro_InOrderID = 0;

                            //NewList.InListID = Item.NewInListID;
                            //NewList.InitInListID = Item.InListID;

                            //NewList.ProID = Item.NewProID;
                            //NewList.ProCount = Item.ProCount;

                       
                            //NewList.Pro_StoreInfo = new System.Data.Linq.EntitySet<Pro_StoreInfo>
                            // {
                            //     new Pro_StoreInfo()
                            //     {
                            //     ProID = Item.NewProID,
                            //     HallID = model.FromHallID,
                            //     InListID = Item.NewInListID,
                            //     ProCount = Item.ProCount
                            //     }
                            // };
                            //lqh.Umsdb.Pro_InOrderList.InsertOnSubmit(NewList);
                            #endregion

                            var Store = from b in lqh.Umsdb.Pro_StoreInfo
                                        where b.InListID == Item.InListID && b.HallID == model.FromHallID && b.ProID == Item.OldProID
                                        select b;
                            if (Store.Count() != 1)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "该库存已被删除，请联系管理员！" };
                            }
                            if (Item.ProCount <= 0)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "空冲数量需大于0！" };
                            }
                            Store.First().ProCount -= Item.ProCount;
                            if (Store.First().ProCount < 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "该库存已不足，请联系管理员！" };
                            }
                   
                        }
                        #endregion
                 
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "空冲调拨成功！" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "空冲调拨失败！" };
                    }
                }
            }
        }
        #endregion

        #region 获取调入实体
        /// <summary>
        /// 获取调入实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetAcceptModel(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
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

                    var inorder_query = from b in lqh.Umsdb.View_AirOutInfo
                                        join c in lqh.Umsdb.Sys_Role_Menu_HallInfo on b.Pro_HallID equals c.HallID
                                        where c.RoleID == user.RoleID&&(b.IsDelete==null||b.IsDelete==false)
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
                                                        where b.Audit.Contains(mm0.ParamValues)
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
                                                    where mm2.ParamValues.Contains(b.ToHallName)
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
                        List<Model.View_AirOutInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.View_AirOutInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
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
        /// 获取调入实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetCanceltModel(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
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

                    var inorder_query = from b in lqh.Umsdb.View_AirOutInfo
                                        join c in lqh.Umsdb.Sys_Role_Menu_HallInfo on b.FromHallID equals c.HallID
                                        where c.RoleID == user.RoleID&&(b.IsDelete==null||b.IsDelete==false)
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
                                                        where b.Audit.Contains(mm0.ParamValues)
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
                                                    where mm2.ParamValues.Contains(b.ToHallName)
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
                        List<Model.View_AirOutInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.View_AirOutInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
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
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, int OutID)
        {
            //返回
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var query = (from b in lqh.Umsdb.View_AirOutListModel
                                     where b.ID == OutID
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

        #region 单个接受
        /// <summary>
        /// 单个接受 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AcceptAirDB(Model.Sys_UserInfo user, Model.Pro_AirOutInfo model)
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
                        #endregion

                        #region 表头


                        var query = from b in lqh.Umsdb.Pro_AirOutInfo
                                    where b.OutOrderID == model.OutOrderID&&(b.IsDelete==null||b.IsDelete==false)
                                    select b;
                        if (query.Count() == 0 || query == null)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "不存在该空冲调拨单" };
                        }
                        Model.Pro_AirOutInfo outHead = query.First();
                        if (outHead.Audit == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "调拨单不能重复接收！" };
                        }
                        outHead.ToDate = DateTime.Now;
                        outHead.ToUserID = user.UserID;
                        outHead.Audit = true;
                        #endregion

                        #region 明细
                        if (outHead.Pro_AirOutListInfo == null || outHead.Pro_AirOutListInfo.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "单据的商品明细为空" };
                        }
                        List<string> ProList = new List<string>();
                        #region 判断接收仓库的号码是否有库存
                        var NewStore = (from b in lqh.Umsdb.Pro_StoreInfo
                                       where ProList.Contains(b.ProID) && b.HallID == model.Pro_HallID && b.ProCount >= 0
                                       select b).ToList();
                        //if (NewStore.Count() == 0)
                        //{
                        //    return new WebReturn() { ReturnValue = false, Message = "空冲号码不存在该仓库或无库存！" };
                        //}
                        //var NewProList = (from b in NewStore
                        //                  select b.ProID).Distinct();
                        //if (ProList.Count() != NewProList.Count())
                        //{
                        //    return new WebReturn() { ReturnValue = false, Message = "部分空冲号码不存在该仓库或无库存！" };
                        //}
                        #endregion
                        foreach (var Item in outHead.Pro_AirOutListInfo)
                        {
                            if (Item.ProCount == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "调拨数量为0！" };
                            }
                            var first_one = NewStore.Where(p => p.ProID == Item.NewProID);
                            Model.Pro_StoreInfo store_model = null;

                            if (first_one.Count() == 0)
                            {
                                store_model = new Pro_StoreInfo
                                {
                                    ProID = Item.NewProID,
                                    HallID = model.Pro_HallID,
                                    InListID = Item.NewInListID,
                                    ProCount = Item.ProCount
                                };
                                lqh.Umsdb.Pro_StoreInfo.InsertOnSubmit(store_model);
                            }
                            else
                            {
                                store_model = first_one.First();
                                store_model.ProCount = store_model.ProCount + Item.ProCount;
                            }
                            #region 新增入库明细(接收空冲调拨才需要)
                            var queryInOrder = (from b in lqh.Umsdb.Pro_InOrderList
                                                where b.InListID == Item.InListID
                                                select b).ToList();
                            if (queryInOrder.Count() != 1)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "获取原批次号出错，请联系管理员！" };
                            }
                            Model.Pro_InOrderList NewList = new Pro_InOrderList();
                            // NewList.Price = queryInOrder[0].Price;
                            NewList.InOrderID = queryInOrder[0].InOrderID;
                            NewList.Pro_InOrderID = 0;

                            NewList.InListID = Item.NewInListID;
                            NewList.InitInListID = Item.InListID;

                            NewList.ProID = Item.NewProID;
                            NewList.ProCount = Item.ProCount;


                            //NewList.Pro_StoreInfo = new System.Data.Linq.EntitySet<Pro_StoreInfo>
                            // {
                            //     new Pro_StoreInfo()
                            //     {
                            //     ProID = Item.NewProID,
                            //     HallID = model.Pro_HallID,
                            //     InListID = Item.NewInListID,
                            //     ProCount = Item.ProCount
                            //     }
                            // };
                            lqh.Umsdb.Pro_InOrderList.InsertOnSubmit(NewList);
                            #endregion 
                            if (!ProList.Contains(Item.NewProID))                       
                               ProList.Add(Item.NewProID);
                        }
                       
                 
                        
                        #endregion
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "接受成功" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "数据出错" };
                    }
                }
            }
        }
        #endregion

        #region 单个取消
        /// <summary>
        /// 单个接受 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn CanceltAirDB(Model.Sys_UserInfo user, Model.Pro_AirOutInfo model)
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
                        dataload.LoadWith<Model.Pro_AirOutInfo>(c => c.Pro_AirOutListInfo);
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
                                      where h.HallID == model.FromHallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                        }
                        #endregion

                        #region 表头


                        var query = from b in lqh.Umsdb.Pro_AirOutInfo
                                    where b.OutOrderID == model.OutOrderID&&(b.IsDelete==null||b.IsDelete==false)
                                    select b;
                        if (query.Count() == 0 || query == null)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "不存在该空冲调拨单" };
                        }
                        Model.Pro_AirOutInfo outHead = query.First();
                        if (outHead.Audit == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "已接收，不能进行其它操作！" };
                        }
                        outHead.IsDelete = true;
                        outHead.Deleter = user.UserID;
                        outHead.DeleteDate = DateTime.Now;
                        #endregion
                        foreach (var next in outHead.Pro_AirOutListInfo)
                        {
                            var queryStore = from b in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                             where b.InListID == next.InListID && b.ProID == next.OldProID && b.HallID == outHead.FromHallID
                                             select b;
                            //库存不存在该批次
                            if (queryStore == null || queryStore.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "库存出错，请联系管理员" };
                            }
                            //更新库存
                            Model.Pro_StoreInfo store = queryStore.First();
                            store.ProCount += next.ProCount;
                        }
   
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "取消成功" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false,Message="数据出错" };
                    }
                }
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
        public Model.WebReturn GetAirPro(Model.Sys_UserInfo user,string HallID)
        {
            //返回
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        List<Model.Pro_ProInfo> query = ( from  b in lqh.Umsdb.Pro_StoreInfo
                                      where b.HallID==HallID
                                     join c in lqh.Umsdb.Pro_ProInfo on b.ProID equals c.ProID
                                     where c.Pro_ClassID==97&&c.Pro_TypeID==3
                                     select c).Distinct().ToList();
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "无数据" };
                        }
                        return new Model.WebReturn() { Obj = query, ReturnValue = true, Message = "已获取" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false,Message="加载空中充值商品失败！" };
                        throw ex;
                    }
                }
            }
        }
        #endregion
    }
}
