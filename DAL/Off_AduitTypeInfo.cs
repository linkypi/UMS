using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public  class Off_AduitTypeInfo
    {
      private int _MethodID;

        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Off_AduitTypeInfo()
        {
            this.MethodID = 0;
        }

        public Off_AduitTypeInfo(int MenthodID)
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
        #region 新增门店优惠
        /// <summary>
        /// 门店优惠
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Off_AduitTypeInfo model)
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
                        #region 插入表头


                        if (model.StartDate < DateTime.Now)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "开始时间必须大于当前时间！" };
                        }
                        if (model.StartDate > model.EndDate)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "开始时间需小于结束时间！" };
                        }
 
                        model.AddDate = DateTime.Now;
                        model.AddUserID = user.UserID;
                        model.Flag = true;
                        
                        #endregion

                        lqh.Umsdb.Off_AduitTypeInfo.InsertOnSubmit(model);               
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "新增门店优惠成功！" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "新增失败！" };
                    }
                }
            }
        }
        #endregion
        #region 修改门店优惠
        /// <summary>
        /// 门店优惠
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, int OffID,Model.Off_AduitTypeInfo model)
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
                        #region 插入表头
                        var query = from b in lqh.Umsdb.Off_AduitTypeInfo
                                    where b.ID == OffID&&b.Flag==true
                                    select b;
                        if (query.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该门店优惠不存在或已经被删除！" };
                        }
                        query.First().Flag = false;
                        //if (model.StartDate < DateTime.Now)
                        //{
                        //    return new WebReturn() { ReturnValue = false, Message = "开始时间必须大于当前时间！" };
                        //}
                        if (model.StartDate > model.EndDate)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "开始时间需小于结束时间！" };
                        }
                        model.OldAduitType = query.First().OldAduitType;
                        model.AddDate = DateTime.Now;
                        model.AddUserID = user.UserID;
                        model.Flag = true;

                        #endregion

                        lqh.Umsdb.Off_AduitTypeInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "修改门店优惠成功！" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "修改失败！" };
                    }
                }
            }
        }
        #endregion
        #region 删除门店优惠
        /// <summary>
        /// 门店优惠
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, List<int> OffID)
        {
            //插入表头 
            //插入明细
            //插入串号明细
            //减少库存
            //更新串号表
            //
            //返回
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 插入表头
                        var query = (from b in lqh.Umsdb.Off_AduitTypeInfo
                                    where OffID.Contains(b.ID) && b.Flag == true
                                    select b).ToList();
                        if (query.Count() != OffID.Count())
                        {
                            return new WebReturn() { ReturnValue = false, Message = "部分门店优惠不存在或已经被删除！" };
                        }
                        foreach (var Item in query)
                        {
                            Item.Flag = false;
                        }
                     
                        #endregion

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "删除门店优惠成功！" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "删除失败！" };
                    }
                }
            }
        }
        #endregion
        #region 获取优惠表头
        /// <summary>
        /// 获取优惠
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


                    var inorder_query = from b in lqh.Umsdb.View_Off_AduitTypeInfo
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
                            case "Name":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.Name.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "Flag":
                                Model.ReportSqlParams_String mmL = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mmL.ParamValues))
                                    break;
                                else
                                {
                                    if (mmL.ParamValues == "All")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.Flag == true
                                                        select b;
                                    }
                                    if (mmL.ParamValues == "Now")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.Flag == true && b.EndDate >= DateTime.Now && b.StartDate <= DateTime.Now
                                                        select b;
                                    }
                                    if (mmL.ParamValues == "NoStart")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.Flag == true && b.StartDate >= DateTime.Now
                                                        select b;
                                    }
                                    if (mmL.ParamValues == "End")
                                    {
                                        inorder_query = from b in inorder_query
                                                        where b.Flag == true && b.EndDate < DateTime.Now
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
                                                    where b.AddUserID.Contains(mm0.ParamValues)
                                                    select b;
                                    break;
                                }

                            default: break;
                        }
                    }
                    #endregion

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
                        List<Model.View_Off_AduitTypeInfo> list  = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        if (pageParam.PageSize > 0)
                        {
                            List<Model.View_Off_AduitTypeInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
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


                    var inorder_query = (from b in lqh.Umsdb.Off_AduitTypeInfo
                                         where b.ID == OffID
                                         select b).ToList();
                    if (inorder_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "无数据" };
                    }

                    Model.OffModel offModel = new Model.OffModel();
                    var query_Pro = from b in lqh.Umsdb.View_Off_AduitProInfo
                                    where b.AduitTypeID == OffID
                                    select b;
                    if (query_Pro.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "商品无数据" };
                    }
                    offModel.ProModel = new List<Model.ProModel>();
                    foreach (var ProItem in query_Pro)
                    {
                        Model.ProModel pro = new Model.ProModel();
                        pro.ProID = ProItem.ProMainID;
                        pro.ProClassName = ProItem.ClassName;
                        pro.ProTypeName = ProItem.TypeName;
                        pro.ProName = ProItem.ProMainName;
                      
                        pro.OffPrice = ProItem.Price==null?0:(decimal)ProItem.Price;
                        pro.SellTypeName = ProItem.Name;
                        pro.SellTypeID = ProItem.SellType;
                        offModel.ProModel.Add(pro);
                    }

                    List<Model.Pro_HallInfo> HallInfo = (from b in inorder_query[0].Off_AduitHallInfo
                                                         join c in lqh.Umsdb.Pro_HallInfo on b.HallID equals c.HallID
                                                         select c).ToList();
                    if (HallInfo.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "仓库无数据" };
                    }
                    offModel.HallModel = new List<Model.HallModel>();
                    foreach (var HallItem in HallInfo)
                    {
                        Model.HallModel Hall = new Model.HallModel() { HallName = HallItem.HallName, HallID=HallItem.HallID};
                        offModel.HallModel.Add(Hall);
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
    }
}
