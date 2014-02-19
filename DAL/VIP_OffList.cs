using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class VIP_OffList
    {
        private int _MethodID;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public VIP_OffList()
        {
            this.MethodID = 0;
        }

        public VIP_OffList(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
        private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
            new Model.ReportSqlParams_String(){ParamName="OffName" },
            new Model.ReportSqlParams_String(){ParamName="OffFlag" },
            new Model.ReportSqlParams_String(){ParamName="OffUpdUser"}
        };

        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }
        #region 获取优惠表头
        /// <summary>
        /// 获取调入实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam, string type)
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

                    if (pageParam == null)
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


                    var inorder_query = from b in lqh.Umsdb.View_OffList
                                        where b.Type == int.Parse(type)
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
                            case "OffName":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OffName.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "OffFlag":
                                Model.ReportSqlParams_String mmL = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mmL.ParamValues))
                                    break;
                                else
                                {
                                    if (mmL.ParamValues == "All")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.OffFlag == true
                                                        select b;
                                    }
                                    if (mmL.ParamValues == "Now")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.OffFlag == true && b.EndDate >= DateTime.Now && b.StartDate <= DateTime.Now
                                                        select b;
                                    }
                                    if (mmL.ParamValues == "NoStart")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.OffFlag == true && b.StartDate >= DateTime.Now
                                                        select b;
                                    }
                                    if (mmL.ParamValues == "End")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.OffFlag == true && (b.EndDate < DateTime.Now || b.UnOver == false)
                                                        select b;
                                    }
                                    break;
                                }

                            case "OffUpdUser":
                                Model.ReportSqlParams_String mm0 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm0.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.UpdUser.Contains(mm0.ParamValues)
                                                    select b;
                                    break;
                                }

                            default: break;
                        }
                    }
                    #endregion
                    var OffListID_List = (from b in inorder_query
                                          orderby b.UpdDate descending
                                          select b.OffID).Distinct();

                    pageParam.RecordCount = OffListID_List.Count();

                    #region 判断是否超过总页数
                    int pagecount = 0;
                    if (pageParam.PageSize > 0)
                    {
                        pagecount = pageParam.RecordCount / pageParam.PageSize;
                    }

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<int> Offlist = OffListID_List.Take(pageParam.PageSize).ToList();
                        List<Model.View_OffList> list = inorder_query.Where(p => Offlist.Contains(p.OffID)).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        if (pageParam.PageSize > 0)
                        {
                            List<int> Offlist = OffListID_List.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                            List<Model.View_OffList> list = inorder_query.Where(p => Offlist.Contains(p.OffID)).ToList();
                            pageParam.Obj = list;
                        }
                        else
                            pageParam.Obj = inorder_query.ToList();
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

        #region 获取优惠实体
        /// <summary>
        /// 获取调入实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, int OffID)
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

                    if (OffID == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }

                    #region 获取数据


                    var inorder_query = (from b in lqh.Umsdb.View_VIP_OffList
                                         where b.OffID == OffID
                                         select b).ToList();
                    if (inorder_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "无数据" };
                    }
                    #endregion


                    return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = inorder_query };
                }

                catch
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取数据失败！" };

                }
            }
        }
        #endregion

        #region 获取优惠实体
        /// <summary>
        /// 获取调入实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, int OffID, bool IsNew)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    DataLoadOptions dataload = new DataLoadOptions();

                    dataload.LoadWith<Model.VIP_OffList>(c => c.VIP_HallOffInfo);
                    dataload.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPTypeOffLIst);
                    dataload.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPOffLIst);
                    lqh.Umsdb.LoadOptions = dataload;

                    if (OffID == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }

                    #region 获取数据


                    var inorder_query = (from b in lqh.Umsdb.VIP_OffList
                                         where b.ID == OffID
                                         select b).ToList();
                    if (inorder_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "无数据" };
                    }

                    Model.OffModel offModel = new Model.OffModel();
                    var query_Pro = from b in lqh.Umsdb.View_ProOffList
                                    where b.OffID == OffID
                                    select b;
                    if (query_Pro.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "商品无数据" };
                    }
                    offModel.ProModel = new List<Model.ProModel>();
                    foreach (var ProItem in query_Pro)
                    {
                        Model.ProModel pro = new Model.ProModel();
                        pro.ProClassName = ProItem.ClassName;
                        pro.ProTypeName = ProItem.TypeName;
                        pro.ProName = ProItem.ProName;
                        pro.ProFormat = ProItem.ProFormat;
                        pro.SellTypeName = ProItem.Name;
                        if (ProItem.Rate != null)
                        {
                            pro.OffRate = (decimal)ProItem.Rate;
                        }
                        if (ProItem.ProOffMoney != null)
                        {
                            pro.OffMoney = (decimal)ProItem.ProOffMoney;
                        }
                        if (ProItem.Point != null)
                        {
                            pro.OffPoint = (decimal)ProItem.Point;
                        }
                        if (ProItem.AfterOffPrice != null)
                        {
                            pro.AfterPrice = (decimal)ProItem.AfterOffPrice;
                        }
                        if (ProItem.Salary != null)
                        {
                            pro.Salary = (decimal)ProItem.Salary;
                        }
                        offModel.ProModel.Add(pro);
                    }

                    List<Model.Pro_HallInfo> HallInfo = (from b in inorder_query[0].VIP_HallOffInfo
                                                         join c in lqh.Umsdb.Pro_HallInfo on b.HallID equals c.HallID
                                                         select c).ToList();
                    if (HallInfo.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "仓库无数据" };
                    }
                    offModel.HallModel = new List<Model.HallModel>();
                    foreach (var HallItem in HallInfo)
                    {
                        Model.HallModel Hall = new Model.HallModel() { HallName = HallItem.HallName };
                        offModel.HallModel.Add(Hall);
                    }

                    if (inorder_query[0].VIP_VIPOffLIst != null && inorder_query[0].VIP_VIPOffLIst.Count() > 0)
                    {
                        List<Model.VIP_VIPInfo> VIPInfo = (from b in inorder_query[0].VIP_VIPOffLIst
                                                           join c in lqh.Umsdb.VIP_VIPInfo on b.VIPID equals c.ID
                                                           select c).ToList();

                        offModel.VIPModel = new List<Model.VIPModel>();
                        foreach (var Item in VIPInfo)
                        {
                            Model.VIPModel vip = new Model.VIPModel() { IMEI = Item.IMEI, VIPName = Item.MemberName };
                            offModel.VIPModel.Add(vip);
                        }
                    }

                    if (inorder_query[0].VIP_VIPTypeOffLIst != null && inorder_query[0].VIP_VIPTypeOffLIst.Count() > 0)
                    {
                        List<Model.VIP_VIPType> VIPType = (from b in inorder_query[0].VIP_VIPTypeOffLIst
                                                           join c in lqh.Umsdb.VIP_VIPType on b.VIPType equals c.ID
                                                           select c).ToList();

                        offModel.VIPTypeModel = new List<Model.VIPTypeModel>();
                        foreach (var Item in VIPType)
                        {
                            Model.VIPTypeModel viptype = new Model.VIPTypeModel() { VIPTypeName = Item.Name };
                            offModel.VIPTypeModel.Add(viptype);
                        }
                    }

                    #endregion

                    return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = offModel };
                }

                catch
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取数据失败！" };

                }
            }
        }
        #endregion

        #region 更新优惠
        /// <summary>
        /// 新增单品优惠活动
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        public Model.WebReturn Update(Model.Sys_UserInfo user, List<Model.VIP_OffList> model_List)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List

            //插入Sys_RoleInfo Sys_RoleMethod 
            //
            //返回
            if (model_List == null) return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证前台数据的有效性
                        var query1 = from b in model_List
                                     where b.UnOver == true && b.EndDate >= DateTime.Now
                                     select b;
                        if (query1.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该活动正在进行不能进行其它操作！" };
                        }
                        var query2 = from b in model_List
                                     where b.EndDate <= DateTime.Now || b.StartDate <= DateTime.Now
                                     select b;
                        if (query2.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "活动开始时间和结束时间必须大于当前日期！" };
                        }
                        #endregion
                        #region 获取数据
                        DataLoadOptions d = new DataLoadOptions();
                        d.LoadWith<Model.VIP_OffList>(c => c.VIP_HallOffInfo);
                        d.LoadWith<Model.VIP_OffList>(c => c.VIP_ProOffList);
                        d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPOffLIst);
                        d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPTypeOffLIst);
                        d.LoadWith<Model.VIP_OffList>(c => c.VIP_SendProOffList);
                        lqh.Umsdb.LoadOptions = d;

                        List<int> OffID_List = (from b in model_List
                                                select b.ID).ToList();

                        var query = from b in lqh.Umsdb.VIP_OffList
                                    //from c in b.VIP_HallOffInfo
                                    //from r in b.VIP_ProOffList
                                    //from e in b.VIP_VIPOffLIst
                                    //from f in b.VIP_VIPTypeOffLIst
                                    where OffID_List.Contains(b.ID)
                                    select b;
                        #endregion
                        List<Model.VIP_OffList> OffList = new List<Model.VIP_OffList>();
                        foreach (var model in query)
                        {
                            #region 表头
                            Model.VIP_OffList Off = new Model.VIP_OffList();
                            Off.ArriveCount = model.ArriveCount;
                            Off.ArriveMoney = model.ArriveMoney;
                            Off.DiscountInfo = model.DiscountInfo;
                            Off.DiscountPic = model.DiscountPic;
                            Off.DiscountSynopsis = model.DiscountSynopsis;
                            var queryModel = from b in model_List
                                             where b.ID == model.ID
                                             select b;
                            if (queryModel.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "数据有误，请联系管理员！" };
                            }
                            Off.EndDate = queryModel.First().EndDate;
                            Off.Flag = true;
                            Off.HaveTop = true;
                            Off.MaxPoint = model.MaxPoint;
                            Off.MinPoint = model.MinPoint;
                            Off.Name = model.Name;
                            Off.Note = model.Note;
                            Off.OffMoney = model.OffMoney;
                            Off.OffPoint = model.OffPoint;
                            Off.OffPointMoney = model.OffPointMoney;
                            Off.OffRate = model.OffRate;
                            Off.SendPoint = model.SendPoint;
                            Off.SendTicket = model.SendTicket;
                            Off.StartDate = queryModel.First().StartDate;
                            Off.Type = model.Type;
                            Off.UnOver = true;
                            Off.UpdDate = DateTime.Now;
                            Off.UpdUser = user.UserID;
                            Off.UseLimit = model.UseLimit;
                            Off.VIPTicketMaxCount = model.VIPTicketMaxCount;
                            #endregion
                            #region  商品明细
                            Off.VIP_ProOffList = new System.Data.Linq.EntitySet<Model.VIP_ProOffList>();
                            if (model.VIP_ProOffList != null)
                            {
                                foreach (var ProItem in model.VIP_ProOffList)
                                {
                                    Model.VIP_ProOffList list = new Model.VIP_ProOffList();
                                    list.AfterOffPrice = ProItem.AfterOffPrice;
                                    list.Note = ProItem.Note;
                                    list.ProCount = ProItem.ProCount;
                                    list.ProID = ProItem.ProID;
                                    list.SellTypeID = ProItem.SellTypeID;
                                    list.OffMoney = ProItem.OffMoney;
                                    list.Point = ProItem.Point;
                                    list.Price = ProItem.Price;
                                    list.Rate = ProItem.Rate;
                                    list.Salary = ProItem.Salary;

                                    Off.VIP_ProOffList.Add(list);
                                }
                            }
                            #endregion
                            #region  仓库明细
                            Off.VIP_HallOffInfo = new System.Data.Linq.EntitySet<Model.VIP_HallOffInfo>();
                            if (model.VIP_HallOffInfo != null)
                            {
                                foreach (var Item in model.VIP_HallOffInfo)
                                {
                                    Model.VIP_HallOffInfo list = new Model.VIP_HallOffInfo();
                                    list.HallID = Item.HallID;
                                    Off.VIP_HallOffInfo.Add(list);
                                }
                            }
                            #endregion

                            #region  会员类型明细
                            Off.VIP_VIPTypeOffLIst = new System.Data.Linq.EntitySet<Model.VIP_VIPTypeOffLIst>();
                            if (model.VIP_VIPTypeOffLIst != null)
                            {
                                foreach (var Item in model.VIP_VIPTypeOffLIst)
                                {
                                    Model.VIP_VIPTypeOffLIst list = new Model.VIP_VIPTypeOffLIst();
                                    list.Note = Item.Note;
                                    list.VIPType = Item.VIPType;
                                    Off.VIP_VIPTypeOffLIst.Add(list);
                                }
                            }
                            #endregion

                            #region  会员明细
                            Off.VIP_VIPOffLIst = new System.Data.Linq.EntitySet<Model.VIP_VIPOffLIst>();
                            if (model.VIP_VIPOffLIst != null)
                            {
                                foreach (var Item in model.VIP_VIPOffLIst)
                                {
                                    Model.VIP_VIPOffLIst list = new Model.VIP_VIPOffLIst();
                                    list.Note = Item.Note;
                                    list.VIPID = Item.VIPID;
                                    Off.VIP_VIPOffLIst.Add(list);
                                }
                            }
                            #endregion
                            OffList.Add(Off);
                            model.Flag = false;
                        }
                        if (OffList.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "无数据，请联系管理员" };
                        }
                        lqh.Umsdb.VIP_OffList.InsertAllOnSubmit(OffList);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "启用成功" };

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                        //throw ex;
                    }

                }

            }
        }
        #endregion

        #region 删除优惠
        /// <summary>
        /// 新增单品优惠活动
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        public Model.WebReturn Delete(Model.Sys_UserInfo user, List<Model.VIP_OffList> model_List)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List

            //插入Sys_RoleInfo Sys_RoleMethod 
            //
            //返回
            if (model_List == null) return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
            string Msg = "";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证前台数据的有效性
                        var query1 = from b in model_List
                                     where b.Flag == true
                                     select b;
                        if (query1.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该活动已删除不能进行其它操作！" };
                        }
                        //var query2 = from b in model_List
                        //             where b.EndDate <= DateTime.Now || b.StartDate <= DateTime.Now
                        //             select b;
                        //if (query2.Count() > 0)
                        //{
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "活动开始时间和结束时间必须大于当前日期！" };
                        //}
                        #endregion
                        #region 获取数据
                        //DataLoadOptions d = new DataLoadOptions();
                        //d.LoadWith<Model.VIP_OffList>(c => c.VIP_HallOffInfo);
                        //d.LoadWith<Model.VIP_OffList>(c => c.VIP_ProOffList);
                        //d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPOffLIst);
                        //d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPTypeOffLIst);
                        //d.LoadWith<Model.VIP_OffList>(c => c.VIP_SendProOffList);
                        //lqh.Umsdb.LoadOptions = d;

                        List<int> OffID_List = (from b in model_List
                                                select b.ID).ToList();

                        var query = from b in lqh.Umsdb.VIP_OffList
                                    where OffID_List.Contains(b.ID)
                                    select b;
                        #endregion
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "无数据，请联系管理员" };
                        }
                        foreach (var model in query)
                        {

                            model.Flag = false;
                        }

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "已删除" };

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                        //throw ex;
                    }

                }

            }
        }
        #endregion

        #region 新增优惠
        /// <summary>
        /// 新增组合优惠活动
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, List<Model.VIP_OffList> model_list)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List

            //插入Sys_RoleInfo Sys_RoleMethod 
            //
            //返回
            if (model_list == null || model_list.Count() == 0) return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        foreach (var model in model_list)
                        {
                            #region 商品


                            #endregion
                            List<int?> viptypeId = new List<int?>();
                            List<int?> vipid = new List<int?>();
                            List<string> proid = new List<string>();
                            List<int?> selltypeid = new List<int?>();
                            // bool NoError = true;
                            #region 会员是否正确

                            //if ((model.VIP_VIPTypeOffLIst == null || model.VIP_VIPTypeOffLIst.Count() == 0)
                            //    && (model.VIP_VIPOffLIst == null || model.VIP_VIPOffLIst.Count() == 0))
                            //{
                            //    model.Note = "必须选择一类会员或者指定会员";
                            //    return new Model.WebReturn() { ReturnValue = false, Message = "必须选择一类会员或者指定会员", Obj = model_list };
                            //}

                            #region 选了会员类别
                            //if (model.VIP_VIPTypeOffLIst != null && model.VIP_VIPTypeOffLIst.Count > 0)
                            //{
                            //    viptypeId = (from b in model.VIP_VIPTypeOffLIst
                            //                 select b.VIPType).ToList();

                            //    var viptypeId_query = (from b in lqh.Umsdb.VIP_VIPType
                            //                           where viptypeId.Contains(b.ID)
                            //                           select b).ToList();

                            //    var viptypeId_query_join = from b in model.VIP_VIPTypeOffLIst
                            //                               join c in viptypeId_query
                            //                               on b.VIPType equals c.ID
                            //                               into temp1
                            //                               from c1 in temp1.DefaultIfEmpty()
                            //                               select new
                            //                               {
                            //                                   b,
                            //                                   c1
                            //                               };
                            //    foreach (var m in viptypeId_query_join)
                            //    {
                            //        if (m.c1 == null)
                            //        {
                            //            NoError = false;
                            //            m.b.Note = "会员类别不存在";
                            //            continue;
                            //        }
                            //    }
                            //    if (!NoError)
                            //    {
                            //        return new Model.WebReturn() { ReturnValue = false, Obj = model };
                            //    }
                            //}
                            #endregion

                            #region 选了指定会员
                            ////if (model.VIP_VIPOffLIst != null && model.VIP_VIPOffLIst.Count > 0)
                            ////{
                            ////    vipid = (from b in model.VIP_VIPOffLIst
                            ////             select b.VIPID).ToList();

                            ////    var vipid_query = (from b in lqh.Umsdb.VIP_VIPType
                            ////                       where viptypeId.Contains(b.ID)
                            ////                       select b).ToList();

                            ////    var vipid_query_join = from b in model.VIP_VIPOffLIst
                            ////                           join c in vipid_query
                            ////                               on b.VIPID equals c.ID
                            ////                               into temp1
                            ////                           from c1 in temp1.DefaultIfEmpty()
                            ////                           select new
                            ////                           {
                            ////                               b,
                            ////                               c1
                            ////                           };
                            ////    foreach (var m in vipid_query_join)
                            ////    {
                            ////        if (m.c1 == null)
                            ////        {
                            ////            NoError = false;
                            ////            m.b.Note = "会员不存在";
                            ////            continue;
                            ////        }
                            ////    }
                            ////    if (!NoError)
                            ////    {
                            ////        return new Model.WebReturn() { ReturnValue = false, Obj = model };
                            ////    }
                            ////}
                            #endregion

                            #endregion



                            #region
                            model.UnOver = true;
                            model.ArriveCount = 0;
                            model.Flag = true;
                            model.UseLimit = 0;
                            if (model.EndDate == null || model.StartDate == null || model.StartDate < DateTime.Now || model.StartDate >= model.EndDate || model.EndDate <= DateTime.Now)
                            {
                                model.Note = "活动的起止日期有误";
                                return new Model.WebReturn() { ReturnValue = false, Message = "活动的起止日期有误", Obj = model_list };
                            }
                            if (model.SendPoint < 0)
                            {
                                model.Note = "积分可以不送，但不能负的";
                                return new Model.WebReturn() { ReturnValue = false, Message = "积分可以不送，但不能负的", Obj = model_list };

                            }
                            if (model.VIPTicketMaxCount <= 0)
                            {
                                model.Note = "活动名额不能小于0";
                                return new Model.WebReturn() { ReturnValue = false, Message = "活动名额不能小于0", Obj = model_list };
                            }
                            #region 单品
                            if (model.Type == 0)
                            {
                                model.ArriveMoney = 0;
                                decimal offPrice = 0;
                                if (model.OffMoney <= 0 && model.OffPoint <= 0 && model.OffRate <= 0 && model.SendPoint <= 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "必须制定一种优惠方式", Obj = model_list };
                                }
                                if (model.OffMoney > 0)
                                {
                                    model.OffPoint = 0;
                                    model.OffPointMoney = 0;
                                    model.MaxPoint = 0;
                                    model.MinPoint = 0;
                                    offPrice = model.OffMoney;
                                }
                                if (model.OffRate > 0)
                                {
                                    if (model.OffRate > 1)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "折扣不能大于1", Obj = model_list };
                                    }
                                    model.OffPoint = 0;
                                    model.OffPointMoney = 0;
                                    model.MaxPoint = 0;
                                    model.MinPoint = 0;
                                    model.OffMoney = 0;

                                }
                                if (model.SendPoint > 0)
                                {
                                    model.OffPoint = 0;
                                    model.OffPointMoney = 0;
                                    model.MaxPoint = 0;
                                    model.MinPoint = 0;
                                    model.OffMoney = 0;
                                    model.OffRate = 0;
                                }
                                if (model.OffPoint > 0)
                                {
                                    model.OffMoney = 0;
                                    model.OffRate = 0;
                                    if (model.OffPoint > model.MaxPoint || model.OffPoint < model.MinPoint)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "积分兑换的范围不能超过最大值和最小值", Obj = model_list };
                                    }
                                    offPrice = model.OffPointMoney;
                                }
                                #region 验证商品
                                var query_Pro = from b in model.VIP_ProOffList
                                                where b.Rate == 0 && b.OffMoney == 0 && b.Point == 0
                                                select b;
                                if (query_Pro.Count() > 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "存在商品的折扣，价格，积分兑换同时为0的商品！", Obj = model_list };
                                }
                                #endregion
                            }
                            #endregion
                            #region 组合
                            else if (model.Type == 1)
                            {

                                model.ArriveMoney = 0;
                                model.OffMoney = 0;
                                model.OffPoint = 0;
                                model.OffPointMoney = 0;
                                model.OffRate = 0;
                                model.MaxPoint = 0;
                                model.MinPoint = 0;
                            }
                            #endregion
                            #region 优惠券
                            else if (model.Type == 2)
                            {
                                if (model.ArriveMoney <= 0 || model.OffMoney <= 0 || model.OffMoney > model.ArriveMoney)
                                {
                                    model.Note = "优惠券的使用条件或者优惠的金额有误";
                                    return new Model.WebReturn() { ReturnValue = false, Message = "优惠券的使用条件或者优惠的金额有误", Obj = model_list };
                                }
                            }
                            #endregion
                            else
                            {
                                model.Note = "优惠活动只能是单品活动、组合活动或者优惠券";
                                return new Model.WebReturn() { ReturnValue = false, Message = "单品优惠商品必选", Obj = model_list };
                            }
                            #endregion
                            if (model.Type == 0 || model.Type == 1)
                            {
                                #region 商品是否正确
                                if (model.VIP_ProOffList == null || model.VIP_ProOffList.Count() == 0)
                                {
                                    model.Note = "优惠商品必选";
                                    return new Model.WebReturn() { ReturnValue = false, Message = "优惠商品必选", Obj = model_list };
                                }
                                //var proid_query = from b in lqh.Umsdb.Pro_SellTypeProduct
                                //                  select b;
                                //foreach (var m in model.VIP_ProOffList)
                                //{
                                //    proid_query = from b in proid_query
                                //                  where b.ProID == m.ProID && b.SellType == m.SellTypeID
                                //                  select b;

                                //}
                                //var proid_query_list = proid_query.ToList();

                                //var VIP_ProOffList_join_proid_query_list = from b in model.VIP_ProOffList
                                //                                           join c in proid_query_list
                                //                                           on new { b.ProID, SellType = b.SellTypeID }
                                //                                           equals
                                //                                           new { c.ProID, c.SellType }
                                //                                           into temp1
                                //                                           from c1 in temp1.DefaultIfEmpty()
                                //                                           select
                                //                                           new { b, c1 };
                                //foreach (var m in VIP_ProOffList_join_proid_query_list)
                                //{
                                //    if (m.c1 == null)
                                //    {
                                //        NoError = false;
                                //        m.b.Note = "商品指定的销售方式不存在";
                                //        continue;
                                //    }
                                //    if (model.Type == 1 && m.b.AfterOffPrice < 0)
                                //    {
                                //        NoError = false;
                                //        m.b.Note = "组合商品的最终套餐价格不能低于0元";
                                //        continue;
                                //    }
                                //    else m.b.AfterOffPrice = 0;
                                //    if (m.b.ProCount <= 0)
                                //    {
                                //        NoError = false;
                                //        m.b.Note = "满足活动所需的最少商品数量不能小于等于0";
                                //        continue;
                                //    }
                                //}
                                //if (!NoError)
                                //{
                                //    return new Model.WebReturn() { ReturnValue = true, Message = "新增失败", Obj = model_list };
                                //}
                                #endregion

                            }
                            model.HaveTop = true;
                            model.UpdUser = user.UserID;
                            model.UpdDate = DateTime.Now;

                            lqh.Umsdb.VIP_OffList.InsertOnSubmit(model);
                        }

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "新增成功" };

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                        //throw ex;
                    }

                }

            }
        }
        #endregion

        /// <summary>
        /// 生成优惠券
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddTicketOff(Model.VIP_OffList model, LinQSqlHelper lqh)
        {

            //#region 生成优惠券
            //if (model.VIPTicketMaxCount == 99999) return new Model.WebReturn() {  ReturnValue=true, Message="生成成功"};

            //List<Model.VIP_OffTicket> tickets = new List<Model.VIP_OffTicket>();
            //string msg = "";
            //lqh.Umsdb.OrderMacker(model.VIPTicketMaxCount, "XJ", "XJ", ref msg);
            //if (string.IsNullOrEmpty(msg))
            //{
            //    model.Note = "生成优惠券编码出错";
            //    return new Model.WebReturn() { ReturnValue = true, Message = "生成优惠券编码出错", Obj = model };
            //}
            //string[] strs = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //if (strs.Length != model.VIPTicketMaxCount)
            //{
            //    model.Note = "生成优惠券编码出错";
            //    return new Model.WebReturn() { ReturnValue = true, Message = "生成优惠券编码出错", Obj = model };
            //}
            //for (int i = 0; i < model.VIPTicketMaxCount; i++)
            //{
            //    tickets.Add(new Model.VIP_OffTicket()
            //    {
            //        Flag = true,
            //        Name = model.Name,
            //        Note = model.Note,
            //        Source = "手动派发优惠券",
            //        TicketID = strs[i],
            //        Used = false,
            //        VIP_OffList = model
            //    });
            //}
            ////lqh.Umsdb.VIP_OffTicket.InsertAllOnSubmit(tickets);
            //#endregion
            return new Model.WebReturn() { ReturnValue = true, Message = "生成优惠券成功" };
        }

        #region 新增套餐

        /// <summary>
        /// 添加组合优惠活动  148
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.VIP_OffList model,bool delTemplate,int tempID)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List


            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        model.Flag = true; //|| model.StartDate < DateTime.Now
                        if (model.EndDate == null || model.StartDate == null  || model.StartDate >= model.EndDate || model.EndDate <= DateTime.Now)
                        {
                            model.Note = "活动的起止日期有误";
                            return new Model.WebReturn() { ReturnValue = false, Message = "活动的起止日期有误", Obj = model };
                        }

                        if (model.VIPTicketMaxCount <= 0)
                        {
                            model.Note = "活动名额不能小于0";
                            return new Model.WebReturn() { ReturnValue = false, Message = "活动名额不能小于0" };
                        }
                        #endregion
                        model.UpdUser = user.UserID;
                        model.UpdDate = DateTime.Now;
                        foreach (var item in model.Package_GroupInfo)
                        {
                            var gt = from a in lqh.Umsdb.Package_GroupTypeInfo
                                     where a.ID == item.GroupID
                                     select a;
                            if (gt.Count() > 0)
                            {
                                item.GroupName = gt.First().GroupName;
                            }
                        }

                        //删除模版
                        if (delTemplate)
                        {
                            var temp = from a in lqh.Umsdb.VIP_OffList
                                       where a.ID == tempID
                                       select a;
                            if (temp.Count() > 0)
                            {
                               // return new WebReturn() { ReturnValue=false,Message="指定模版不存在！"};
                                temp.First().Flag = false;
                            }
                        }
                        
                        lqh.Umsdb.VIP_OffList.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "新增成功！" };

                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "新增失败！" };
                    }

                }

            }
        }


        /// <summary>
        /// 套餐一级审批 300
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Aduit(Model.Sys_UserInfo user, int id, bool passed, string note1)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List


            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var model = from a in lqh.Umsdb.VIP_OffListAduitHeader
                                    where a.ID == id
                                    select a;
                        if (model.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在！" };
                        }

                        if (model.First().Aduited1 == true||model.First().Aduited==true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单已审批" };
                        }
                       
                        Model.VIP_OffListAduitHeader ad = model.First();

                        ad.Aduited1 = true;
                        ad.Passed1 = passed;
                        if (!passed)
                        {
                            ad.Aduited = true;
                            ad.Passed = passed;
                        }
                        ad.AduitUser1 = user.UserID;
                        ad.AduitDate1 = DateTime.Now;
                        ad.AduitNote1 = note1;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "审批成功！" };

                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批失败！" };
                    }

                }

            }
        }

        /// <summary>
        /// 套餐一级审批 300
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Aduit2(Model.Sys_UserInfo user, int id, bool passed, string note2)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List


            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var model = from a in lqh.Umsdb.VIP_OffListAduitHeader
                                    where a.ID == id
                                    select a;
                        if (model.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在！" };
                        }

                        if (model.First().Aduited2 == true || model.First().Aduited == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单已审批" };
                        }

                        Model.VIP_OffListAduitHeader ad = model.First();

                        ad.Aduited2 = true;
                        ad.Passed2 = passed;
                        if (!passed)
                        {
                            ad.Aduited = true;
                            ad.Passed = passed;
                        }
                        ad.AduitUser2 = user.UserID;
                        ad.AduitDate2 = DateTime.Now;
                        ad.AduitNote2 = note2;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "审批成功！" };

                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批失败！" };
                    }

                }

            }
        }


        /// <summary>
        /// 套餐三级审批   308
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Aduit3(Model.Sys_UserInfo user, int id,bool passed,string note3)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List


            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var model = from a in lqh.Umsdb.VIP_OffListAduitHeader
                                    where a.ID == id
                                    select a;
                        if (model.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在！" };
                        }

                        if (model.First().Aduited3 == true||model.First().Aduited == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单已审批" };
                        }

                        Model.VIP_OffListAduitHeader ad = model.First();
                        ad.Aduited3 = true;
                        ad.Aduited = true;
                        ad.Passed = passed;
                        ad.Passed3 = passed;
                        if (passed)
                        {
                            foreach (var item in ad.VIP_OffListAduit)
                            {
                                Model.VIP_OffList head = new Model.VIP_OffList();
                                head.ArriveCount = item.ArriveCount;
                                head.ArriveMoney = item.ArriveMoney;
                                head.DiscountInfo = item.DiscountInfo;
                                head.DiscountPic = item.DiscountPic;
                                head.DiscountPicbigid = item.DiscountPicbigid;
                                head.DiscountSynopsis = item.DiscountSynopsis;
                                head.EndDate = ad.EndDate;
                                head.Flag = item.Flag;
                                head.HaveTop = item.HaveTop;
                                head.MaxPoint = item.MaxPoint;
                                head.MinPoint = item.MinPoint;
                                head.Name = item.Name;
                                head.Note = item.Note;
                                head.OffMoney = item.OffMoney;
                                head.OffPoint = item.OffPoint;
                                head.VIPTicketMaxCount = item.VIPTicketMaxCount;
                                head.OffPointMoney = item.OffPointMoney;
                                head.OffRate = item.OffRate;
                                head.SendPoint = item.SendPoint;
                                head.SendTicket = item.SendTicket;
                                head.StartDate = ad.StartDate;
                                head.Type = item.Type;
                                head.UnOver = item.UnOver;
                                head.UpdDate = item.UpdDate;
                                head.UpdUser = item.UpdUser;
                                head.UseLimit = item.UseLimit;

                                //添加仓库
                                foreach (var xxd in ad.VIP_HallInfoHeader)
                                {
                                    Model.VIP_HallOffInfo hall = new VIP_HallOffInfo();
                                    hall.HallID = xxd.HallID;
                                    if (head.VIP_HallOffInfo == null)
                                    {
                                        head.VIP_HallOffInfo = new EntitySet<VIP_HallOffInfo>();
                                    }
                                    head.VIP_HallOffInfo.Add(hall);
                                }

                                lqh.Umsdb.VIP_OffList.InsertOnSubmit(head);
                                lqh.Umsdb.SubmitChanges();

                                //关联正式套餐表ID
                                //foreach (var child in item.VIP_VIPOffLIst)
                                //{
                                //    child.OffID = head.ID;
                                //}
                               
                                foreach (var child in item.Package_GroupInfo)
                                {
                                    child.OffID = head.ID;
                                }
                            }
                        }

                        ad.AduitUser3 = user.UserID;
                        ad.AduitDate3 = DateTime.Now;
                        ad.AduitNote3 = note3;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "审批成功！" };

                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批失败！" };
                    }

                }

            }
        }


        /// <summary>
        /// 新增组合优惠活动申请 295
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddApply(Model.Sys_UserInfo user, Model.VIP_OffListAduitHeader model)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List

            //插入Sys_RoleInfo Sys_RoleMethod 
            //
            //返回
            if (model == null) return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        //model.Flag = true;
                        //if (model.EndDate == null || model.StartDate == null || model.StartDate < DateTime.Now || model.StartDate >= model.EndDate || model.EndDate <= DateTime.Now)
                        //{
                        //    model.Note = "活动的起止日期有误";
                        //    return new Model.WebReturn() { ReturnValue = false, Message = "活动的起止日期有误", Obj = model };
                        //}
                        foreach (var item in model.VIP_OffListAduit)
                        {
                            if (item.VIPTicketMaxCount <= 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "套餐 "+item.Name+ " 的活动名额不能小于0" };
                            }
                            item.UpdDate = DateTime.Now;

                            foreach (var child in item.Package_GroupInfo)
                            {
                                var gt = from a in lqh.Umsdb.Package_GroupTypeInfo
                                         where a.ID == child.GroupID
                                         select a;
                                if (gt.Count() > 0)
                                {
                                    child.GroupName = gt.First().GroupName;
                                }
                            }
                        }

                        model.ApplyDate = DateTime.Now;
                        model.SysDate = DateTime.Now;

                        lqh.Umsdb.VIP_OffListAduitHeader.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "申请成功！" };

                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "申请失败！" };
                    }

                }

            }
        }


        /// <summary>
        /// 298
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Model.WebReturn DeleteApply(Model.Sys_UserInfo user,int id )
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {

                        #region  验证用户取消权限

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        if (user.CancelLimit == null || user.CancelLimit == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "权限不足，删除失败！" };
                        }

                        #endregion 

                        var models = from a in lqh.Umsdb.VIP_OffListAduitHeader
                                     where  a.ID == id
                                     select a;
                        if(models.Count()==0)
                        {
                            return   new WebReturn(){ReturnValue =false,Message="该单已删除或不存在！"};
                        }
                        if (models.First().Passed2 == true)
                        {
                            return new WebReturn() {ReturnValue= false,Message="该单已全部审批通过，删除失败！" };
                        }
                        models.First().Deleter = user.UserID;
                        models.First().IsDelete = true;
                        models.First().DeleteDate = DateTime.Now;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue =false,Message=ex.Message };
                    }
                }
            }
        }


        #region 获取套餐表头
        /// <summary>
        /// 获取套餐   149
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

                    if (pageParam == null)
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


                    var inorder_query = from b in lqh.Umsdb.View_OffList
                                        where b.Type>1
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
                            case "Type":
                                Model.ReportSqlParams_String type = (Model.ReportSqlParams_String)m.ParamFront;
                                inorder_query = from b in inorder_query
                                                where b.Type.ToString()== type.ParamValues
                                                select b;
                                break;

                            case "OffName":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.OffName.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "OffFlag":
                                Model.ReportSqlParams_String mmL = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mmL.ParamValues))
                                    break;
                                else
                                {
                                    if (mmL.ParamValues == "All")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.OffFlag == true
                                                        select b;
                                    }
                                    if (mmL.ParamValues == "Now")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.OffFlag == true && b.EndDate >= DateTime.Now && b.StartDate <= DateTime.Now
                                                        select b;
                                    }
                                    if (mmL.ParamValues == "NoStart")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.OffFlag == true && b.StartDate >= DateTime.Now
                                                        select b;
                                    }
                                    if (mmL.ParamValues == "End")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.OffFlag == true && (b.EndDate < DateTime.Now || b.UnOver == false)
                                                        select b;
                                    }
                                    break;
                                }

                            case "RealName":
                                Model.ReportSqlParams_String mm0 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm0.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.RealName.Contains(mm0.ParamValues)
                                                    select b;
                                    break;
                                }

                            default: break;
                        }
                    }
                    #endregion
                    var OffListID_List = (from b in inorder_query
                                          orderby b.UpdDate descending
                                          select b.OffID).Distinct();

                    pageParam.RecordCount = OffListID_List.Count();

                    #region 判断是否超过总页数
                    int pagecount = 0;
                    if (pageParam.PageSize > 0)
                    {
                        pagecount = pageParam.RecordCount / pageParam.PageSize;
                    }

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<int> Offlist = OffListID_List.Take(pageParam.PageSize).ToList();
                        List<Model.View_OffList> list = inorder_query.Where(p => Offlist.Contains(p.OffID)).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        if (pageParam.PageSize > 0)
                        {
                            List<int> Offlist = OffListID_List.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                            List<Model.View_OffList> list = inorder_query.Where(p => Offlist.Contains(p.OffID)).ToList();
                            pageParam.Obj = list;
                        }
                        else
                            pageParam.Obj = inorder_query.ToList();
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


        /// <summary>
        /// 查询审批单 294 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn AduitSearch(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    if (pageParam == null)
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


                    var inorder_query = from b in lqh.Umsdb.View_VIPOffListAduitHeader
                                        select b ;
                    foreach (var m in param_join)
                    {
                        switch (m.ParamFront.ParamName)
                        {
                            case "ApplyDate":
                                Model.ReportSqlParams_DataTime date = (Model.ReportSqlParams_DataTime)m.ParamFront;

                                inorder_query = from b in inorder_query
                                                where b.SysDate >= date.ParamValues
                                                    select b;
                               
                                break;
                         

                            case "Destination":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                inorder_query = from b in inorder_query
                                                where b.Destination.Contains( mm.ParamValues)
                                                select b;
                                break;

                            case "Aduited":
                                Model.ReportSqlParams_Bool aduited = (Model.ReportSqlParams_Bool)m.ParamFront;

                                if (aduited.ParamValues)
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasAduited == true
                                                    select b;
                                }
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasAduited != true
                                                    select b;
                                }
                                break;

                            case "Passed":
                                Model.ReportSqlParams_Bool Passed = (Model.ReportSqlParams_Bool)m.ParamFront;

                                if (Passed.ParamValues)
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasPassed == true
                                                    select b;
                                }
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasPassed != true
                                                    select b;
                                }
                                break;

                            case "Aduited1":
                                Model.ReportSqlParams_Bool aduited1 = (Model.ReportSqlParams_Bool)m.ParamFront;

                                if (aduited1.ParamValues)
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasAduited1 == true
                                                    select b;
                                }
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasAduited1 != true
                                                    select b;
                                }
                                break;

                            case "Passed1":
                                Model.ReportSqlParams_Bool Passed1 = (Model.ReportSqlParams_Bool)m.ParamFront;

                                if (Passed1.ParamValues)
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasPassed1 == true
                                                    select b;
                                }
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasPassed1 != true
                                                    select b;
                                }
                                break;
                            case "Aduited2":
                                Model.ReportSqlParams_Bool aduited2 = (Model.ReportSqlParams_Bool)m.ParamFront;

                                if (aduited2.ParamValues)
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasAduited2 == true
                                                    select b;
                                }
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasAduited2 != true
                                                    select b;
                                }
                                break;

                            case "Passed2":
                                Model.ReportSqlParams_Bool Passed2 = (Model.ReportSqlParams_Bool)m.ParamFront;

                                if (Passed2.ParamValues)
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasPassed2 == true
                                                    select b;
                                }
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasPassed2 != true
                                                    select b;
                                }
                                break;
                            case "Aduited3":
                                Model.ReportSqlParams_Bool aduited3 = (Model.ReportSqlParams_Bool)m.ParamFront;

                                if (aduited3.ParamValues)
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasAduited3 == true
                                                    select b;
                                }
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasAduited3 != true
                                                    select b;
                                }
                                break;

                            case "Passed3":
                                Model.ReportSqlParams_Bool Passed3 = (Model.ReportSqlParams_Bool)m.ParamFront;

                                if (Passed3.ParamValues)
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasPassed3 == true
                                                    select b;
                                }
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.HasPassed3 != true
                                                    select b;
                                }
                                break;

                            case "Creater":
                                Model.ReportSqlParams_String mm0 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm0.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.Creater.Contains(mm0.ParamValues)
                                                    select b;
                                    break;
                                }
                            default: break;
                        }
                    }
                    #endregion
                    inorder_query = from a in inorder_query
                                    orderby a.SysDate descending
                                    select a;
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数
                    int pagecount = 0;
                    if (pageParam.PageSize > 0)
                    {
                        pagecount = pageParam.RecordCount / pageParam.PageSize;
                    }

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in inorder_query.Take(pageParam.PageSize).ToList()
                                      select a;
                        List<Model.View_VIPOffListAduitHeader> list = results.ToList();

                        GetAduitChildren(lqh, list);
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        if (pageParam.PageSize > 0)
                        {
                            var results = from a in inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                          select a;

                            List<Model.View_VIPOffListAduitHeader> list = results.ToList();
                            
                            GetAduitChildren(lqh, list);
                            pageParam.Obj = list;
                        }
                        else
                        {
                            List<Model.View_VIPOffListAduitHeader> list = inorder_query.ToList();
                            GetAduitChildren(lqh, list);
                            pageParam.Obj = list; 
                        }
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

        /// <summary>
        /// 获取审批单明细
        /// </summary>
        /// <param name="lqh"></param>
        /// <param name="list"></param>
        private static void GetAduitChildren(LinQSqlHelper lqh, List<Model.View_VIPOffListAduitHeader> list)
        {
            foreach (var item in list)
            {
                item.View_VIPOffListAduit = new List<View_VIPOffListAduit>();
                var aduitList = from a in lqh.Umsdb.View_VIPOffListAduit
                                where a.HeadID == item.ID
                                select a;

                foreach (var child in aduitList)
                {
                    View_VIPOffListAduit vv = new View_VIPOffListAduit();
                    vv.HeadID = child.HeadID;
                    vv.ID = child.ID;
                    vv.Name = child.Name;
                    vv.Note = child.Note;
                    vv.SalesName = child.SalesName;
                    vv.Type = child.Type;
                    vv.VIPTicketMaxCount = child.VIPTicketMaxCount;
                    vv.ArriveMoney = child.ArriveMoney;
                    vv.View_Package_GroupInfo = new List<View_Package_GroupInfo>();
                    var groupList = from a in lqh.Umsdb.View_Package_GroupInfo
                                    where a.TempOffID == child.ID
                                    select a;
                    foreach (var xxd in groupList)
                    {
                        View_Package_GroupInfo vp = new View_Package_GroupInfo();
                        vp.TempOffID = xxd.TempOffID;
                        vp.SubNote = xxd.SubNote;
                        vp.SellTypeName = xxd.SellTypeName;
                        vp.SellTypeID = xxd.SellTypeID;
                        vp.Note = xxd.Note;
                        vp.IsMust = xxd.IsMust;
                        vp.ID = xxd.ID;
                        vp.GroupName = xxd.GroupName;

                        vv.View_Package_GroupInfo.Add(vp);
                    }
                    item.View_VIPOffListAduit.Add(vv);
                }
            }
        }

        #region 获取套餐资源 
        /// <summary>
        /// 获取套餐资源
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    DataLoadOptions dl=new DataLoadOptions();
                    dl.LoadWith<Model.VIP_OffList>(o=>o.Package_GroupInfo);
                    dl.LoadWith<Model.VIP_OffList>(o => o.Package_SalesNameInfo);
                    dl.LoadWith<Model.Package_GroupInfo>(o => o.Package_ProInfo);
                    lqh.Umsdb.LoadOptions = dl;

                    var query = (from b in lqh.Umsdb.VIP_OffList
                                 join d in lqh.Umsdb.Package_SalesNameInfo
                                 on b.Type equals d.ID
                                 join c in lqh.Umsdb.Package_GroupInfo
                                on b.ID equals c.OffID
                                 join x in lqh.Umsdb.Package_GroupTypeInfo
                                 on c.GroupID equals x.ID   //0123 是 单品优惠 组合优惠 优惠券 和 满级送 
                                 where b.Type >= 4 
                                 select b).Distinct<Model.VIP_OffList>().ToList();

                    if (query == null || query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "获取失败" };
                    }
                    var list = (from a in query
                             select a).Distinct<Model.VIP_OffList>();
                    return new Model.WebReturn() { Obj = list.ToList(), ReturnValue = true, Message = "获取成功" };
                }
                catch
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取失败" };
                }
            }
        }
        #endregion

        #region 获取套餐详情
        /// <summary>
        /// 获取  149
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, int OffID, string a )
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    DataLoadOptions dataload = new DataLoadOptions();

                    dataload.LoadWith<Model.VIP_OffList>(c => c.VIP_HallOffInfo);
                    dataload.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPTypeOffLIst);
                    dataload.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPOffLIst);
                    dataload.LoadWith<Model.VIP_OffList>(c => c.Package_GroupInfo);
                    dataload.LoadWith<Model.Package_GroupInfo>(c => c.Package_ProInfo);
                    lqh.Umsdb.LoadOptions = dataload;

                    if (OffID == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }

                    #region 获取数据

                    var inorder_query = (from b in lqh.Umsdb.VIP_OffList
                                         join d in lqh.Umsdb.Package_SalesNameInfo
                                         on b.Type equals d.ID
                                         join c in lqh.Umsdb.Package_GroupInfo
                                        on b.ID equals c.OffID
                                         join x in lqh.Umsdb.Package_GroupTypeInfo
                                         on c.GroupID equals x.ID
                                         where b.ID == OffID
                                        && b.Type >= 4 && b.Flag == true 
                                         select b).ToList();
                    if (inorder_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "无数据" };
                    }
                    Model.VIP_OffList Off = inorder_query.First();
                    Model.OffModel offModel = new Model.OffModel();
                    offModel.Name = Off.Name;
                    offModel.ID = Off.ID;
                    offModel.Note = Off.Note;
                    offModel.SalesName = Off.Package_SalesNameInfo.SalesName;
                    offModel.ArriveMoney = Off.ArriveMoney;
                    offModel.StartDate = Convert.ToDateTime(Off.StartDate);
                    offModel.EndDate = Convert.ToDateTime(Off.EndDate);
                  
                    offModel.VIPTicketMaxCount = Convert.ToInt32(Off.VIPTicketMaxCount);
                    offModel.CreatName = Off.UpdUser;

                    var query_Gro = from b in inorder_query[0].Package_GroupInfo
                                    join x in lqh.Umsdb.Pro_SellType on b.SellType equals x.ID
                                    join d in lqh.Umsdb.Package_GroupTypeInfo 
                                    on b.GroupID equals d.ID
                                    where b.OffID == OffID 
                                    select new
                                    {
                                        b.GroupID,
                                        d.GroupName,
                                        b.IsMust,
                                        b.Note,
                                        b.SellType,
                                        x.Name,
                                        b.Package_ProInfo
                                    };
                    if (query_Gro.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "商品无数据" };
                    }
                    offModel.GrounpInfo = new List<Model.GrounpInfo>();
                    foreach (var GrounpItem in query_Gro)
                    {
                        if (GrounpItem.Package_ProInfo == null || GrounpItem.Package_ProInfo.Count() == 0)
                        {
                            continue;
                        }
                        Model.GrounpInfo Gro = new Model.GrounpInfo();
                        Gro.GroupID =(int) GrounpItem.GroupID;
                        Gro.GrounpName = GrounpItem.GroupName;
                        Gro.IsMustName = GrounpItem.IsMust==true?"是":"否";
                        Gro.Note = GrounpItem.Note;
                        Gro.SellType = Convert.ToInt32(GrounpItem.SellType);
                        Gro.SellTypeName = GrounpItem.Name;
                        Gro.ProModel = new List<Model.ProModel>(); 
                        foreach (var Item in GrounpItem.Package_ProInfo)
                        {
                            Model.ProModel Pro = new Model.ProModel();
                            Pro.ProMainID = Convert.ToInt32(Item.ProMainNameID);
                            Pro.SellTypeID = Convert.ToInt32(GrounpItem.SellType);
                            Pro.SellTypeName = GrounpItem.Name;
                            Pro.Salary = Convert.ToDecimal(Item.Salary);
                            Gro.ProModel.Add(Pro);
                        }
                        offModel.GrounpInfo.Add(Gro);                    
                   }

                    List<Model.Pro_HallInfo> HallInfo = (from b in inorder_query[0].VIP_HallOffInfo
                                                         join c in lqh.Umsdb.Pro_HallInfo on b.HallID equals c.HallID
                                                         select c).ToList();
                    if (HallInfo.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "仓库无数据" };
                    }
                    offModel.HallModel = new List<Model.HallModel>();
                    foreach (var HallItem in HallInfo)
                    {
                        Model.HallModel Hall = new Model.HallModel() { HallName = HallItem.HallName,HallID=HallItem.HallID};
                        offModel.HallModel.Add(Hall);
                    }

                    if (inorder_query[0].VIP_VIPOffLIst != null && inorder_query[0].VIP_VIPOffLIst.Count() > 0)
                    {
                        List<Model.VIP_VIPInfo> VIPInfo = (from b in inorder_query[0].VIP_VIPOffLIst
                                                           join c in lqh.Umsdb.VIP_VIPInfo on b.VIPID equals c.ID
                                                           select c).ToList();

                        offModel.VIPModel = new List<Model.VIPModel>();
                        foreach (var Item in VIPInfo)
                        {
                            Model.VIPModel vip = new Model.VIPModel() { IMEI = Item.IMEI, VIPName = Item.MemberName, ID=Item.ID};
                            offModel.VIPModel.Add(vip);
                        }
                    }

                    if (inorder_query[0].VIP_VIPTypeOffLIst != null && inorder_query[0].VIP_VIPTypeOffLIst.Count() > 0)
                    {
                        List<Model.VIP_VIPType> VIPType = (from b in inorder_query[0].VIP_VIPTypeOffLIst
                                                           join c in lqh.Umsdb.VIP_VIPType on b.VIPType equals c.ID
                                                           select c).ToList();

                        offModel.VIPTypeModel = new List<Model.VIPTypeModel>();
                        foreach (var Item in VIPType)
                        {
                            Model.VIPTypeModel viptype = new Model.VIPTypeModel() { VIPTypeName = Item.Name, TypeID=Item.ID };
                            offModel.VIPTypeModel.Add(viptype);
                        }
                    }

                    #endregion

                    return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = offModel };
                }

                catch
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取数据失败！" };

                }
            }
        }

        /// <summary>
        /// 获取审批单详情   296
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetAduitDetail(Model.Sys_UserInfo user, int OffID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //DataLoadOptions dataload = new DataLoadOptions();
                    //dataload.LoadWith<Model.View_VIPOffListAduitHeader>(c => c.View_VIPOffListAduit);
                    //dataload.LoadWith<Model.View_VIPOffListAduit>(c => c.View_Package_GroupInfo);
                  
                    //lqh.Umsdb.LoadOptions = dataload;

                    if (OffID == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }

                    //dataload.LoadWith<Model.VIP_OffListAduit>(c => c.VIP_HallOffInfo);
                    //dataload.LoadWith<Model.VIP_OffListAduit>(c => c.VIP_VIPTypeOffLIst);
                    //dataload.LoadWith<Model.VIP_OffListAduit>(c => c.VIP_VIPOffLIst);
                    //dataload.LoadWith<Model.VIP_OffListAduit>(c => c.Package_GroupInfo);
                    //dataload.LoadWith<Model.Package_GroupInfo>(c => c.Package_ProInfo);
                    //lqh.Umsdb.LoadOptions = dataload;

                    //if (OffID == 0)
                    //{
                    //    return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    //}

                    #region 获取数据

                    //var inorder_query = (from b in lqh.Umsdb.VIP_OffListAduit
                    //                     join d in lqh.Umsdb.Package_SalesNameInfo
                    //                     on b.Type equals d.ID
                    //                     join c in lqh.Umsdb.Package_GroupInfo
                    //                    on b.ID equals c.TempOffID
                    //                     join x in lqh.Umsdb.Package_GroupTypeInfo
                    //                     on c.GroupID equals x.ID
                    //                     where b.ID == OffID
                    //                    && b.Type >= 4 
                    //                     select b).ToList();
                    //if (inorder_query.Count() == 0)
                    //{
                    //    return new Model.WebReturn() { ReturnValue = false, Message = "无数据" };
                    //}
                    //Model.VIP_OffListAduit Off = inorder_query.First();
                    //Model.OffModel offModel = new Model.OffModel();
                    //offModel.Name = Off.Name;
                    //offModel.Note = Off.Note;
                    ////offModel    .ApplyNote = Off.ApplyNote;
                    //offModel.SalesName = Off.Package_SalesNameInfo.SalesName;
                    //offModel.ArriveMoney = Off.ArriveMoney;
                    //offModel.StartDate = Convert.ToDateTime(Off.StartDate);
                    //offModel.EndDate = Convert.ToDateTime(Off.EndDate);

                    //offModel.VIPTicketMaxCount = Convert.ToInt32(Off.VIPTicketMaxCount);
                    //offModel.CreatName = Off.UpdUser;

                    //var query_Gro = from b in inorder_query[0].Package_GroupInfo
                    //                join x in lqh.Umsdb.Pro_SellType on b.SellType equals x.ID
                    //                join d in lqh.Umsdb.Package_GroupTypeInfo
                    //                on b.GroupID equals d.ID
                    //                where b.TempOffID == OffID 
                    //                select new
                    //                {
                    //                    b.GroupID,
                    //                    d.GroupName,
                    //                    b.IsMust,
                    //                    b.Note,
                    //                    b.SellType,
                    //                    x.Name,
                    //                    b.Package_ProInfo
                    //                };
                    //if (query_Gro.Count() == 0)
                    //{
                    //    return new Model.WebReturn() { ReturnValue = false, Message = "商品无数据" };
                    //}
                    //offModel.GrounpInfo = new List<Model.GrounpInfo>();
                    //foreach (var GrounpItem in query_Gro)
                    //{
                    //    if (GrounpItem.Package_ProInfo == null || GrounpItem.Package_ProInfo.Count() == 0)
                    //    {
                    //        continue;
                    //    }
                    //    Model.GrounpInfo Gro = new Model.GrounpInfo();
                    //    Gro.GroupID = (int)GrounpItem.GroupID;
                    //    Gro.GrounpName = GrounpItem.GroupName;
                    //    Gro.IsMustName = GrounpItem.IsMust == true ? "是" : "否";
                    //    Gro.Note = GrounpItem.Note;
                    //    Gro.SellType = Convert.ToInt32(GrounpItem.SellType);
                    //    Gro.SellTypeName = GrounpItem.Name;
                    //    Gro.ProModel = new List<Model.ProModel>();
                    //    foreach (var Item in GrounpItem.Package_ProInfo)
                    //    {
                    //        Model.ProModel Pro = new Model.ProModel();
                    //        Pro.ProMainID = Convert.ToInt32(Item.ProMainNameID);
                    //        Pro.SellTypeID = Convert.ToInt32(GrounpItem.SellType);
                    //        Pro.SellTypeName = GrounpItem.Name;
                    //        Pro.Salary = Convert.ToDecimal(Item.Salary);
                    //        Gro.ProModel.Add(Pro);
                    //    }
                    //    offModel.GrounpInfo.Add(Gro);
                    //}

                    //List<Model.Pro_HallInfo> HallInfo = (from b in inorder_query[0].VIP_HallOffInfo
                    //                                     join c in lqh.Umsdb.Pro_HallInfo on b.HallID equals c.HallID
                    //                                     select c).ToList();
                    //if (HallInfo.Count() == 0)
                    //{
                    //    return new Model.WebReturn() { ReturnValue = false, Message = "仓库无数据" };
                    //}
                    //offModel.HallModel = new List<Model.HallModel>();
                    //foreach (var HallItem in HallInfo)
                    //{
                    //    Model.HallModel Hall = new Model.HallModel() { HallName = HallItem.HallName, HallID = HallItem.HallID };
                    //    offModel.HallModel.Add(Hall);
                    //}

                    //if (inorder_query[0].VIP_VIPOffLIst != null && inorder_query[0].VIP_VIPOffLIst.Count() > 0)
                    //{
                    //    List<Model.VIP_VIPInfo> VIPInfo = (from b in inorder_query[0].VIP_VIPOffLIst
                    //                                       join c in lqh.Umsdb.VIP_VIPInfo on b.VIPID equals c.ID
                    //                                       select c).ToList();

                    //    offModel.VIPModel = new List<Model.VIPModel>();
                    //    foreach (var Item in VIPInfo)
                    //    {
                    //        Model.VIPModel vip = new Model.VIPModel() { IMEI = Item.IMEI, VIPName = Item.MemberName, ID = Item.ID };
                    //        offModel.VIPModel.Add(vip);
                    //    }
                    //}

                    //if (inorder_query[0].VIP_VIPTypeOffLIst != null && inorder_query[0].VIP_VIPTypeOffLIst.Count() > 0)
                    //{
                    //    List<Model.VIP_VIPType> VIPType = (from b in inorder_query[0].VIP_VIPTypeOffLIst
                    //                                       join c in lqh.Umsdb.VIP_VIPType on b.VIPType equals c.ID
                    //                                       select c).ToList();

                    //    offModel.VIPTypeModel = new List<Model.VIPTypeModel>();
                    //    foreach (var Item in VIPType)
                    //    {
                    //        Model.VIPTypeModel viptype = new Model.VIPTypeModel() { VIPTypeName = Item.Name, TypeID = Item.ID };
                    //        offModel.VIPTypeModel.Add(viptype);
                    //    }
                    //}

                    #endregion

                   // return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = offModel };
                    return new Model.WebReturn() { ReturnValue = true, Message = "获取成功" };
                }

                catch
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取数据失败！" };

                }
            }
        }

        #endregion

        #region 删除优惠
        /// <summary>
        /// 新增单品优惠活动
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, List<Model.View_OffList> model_List)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List

            //插入Sys_RoleInfo Sys_RoleMethod 
            
            //返回
            if (model_List == null) return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
               
                        #region 获取数据
 
                        List<int> OffID_List = (from b in model_List
                                                select b.OffID).ToList();

                        var query = from b in lqh.Umsdb.VIP_OffList
                                    where OffID_List.Contains(b.ID)
                                    select b;
                        #endregion
                        if (query.Count() != OffID_List.Count())
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "数据有误，请联系管理员" };
                        }
                        foreach (var model in query)
                        {

                            model.Flag = false;
                            model.UpdUser = user.UserID;
                            model.UpdDate = DateTime.Now;
                        }

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "已删除" };

                    }
                    catch 
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                    }

                }

            }
        }

        #endregion

        #region 停止
        /// <summary>
        /// 290
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idList"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public Model.WebReturn Stop(Model.Sys_UserInfo user, List<int> idList, string enddate)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 权限验证

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                        if (!result.ReturnValue)
                        { return result; }

                        List<string> hallids = new List<string>();

                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion

                        var valoff = from a in lqh.Umsdb.VIP_OffList
                                       where idList.Contains(a.ID) && a.Flag == true
                                       select a;
                        if (valoff.Count() != idList.Count)
                        {
                            List<int> list = new List<int>();
                            foreach (var item in valoff)
                            {
                                list.Add(item.ID);
                            }
                            //inorder_query = from b in inorder_query
                            //                where b.OffFlag == true && (b.EndDate < DateTime.Now || b.UnOver == false)
                            //                select b;

                            var rest = from a in idList
                                       join b in lqh.Umsdb.VIP_OffList on a equals b.ID
                                       where !list.Contains(a)
                                       select b;
                            return new Model.WebReturn() { Message = "套餐：" + rest.First().Name + "不存在！", ReturnValue = false };
                        }
                        foreach (var item in valoff)
                        {
                            item.EndDate = Convert.ToDateTime(enddate);
                        }
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                    }
                }
            }
        }

        #endregion
    }
}
