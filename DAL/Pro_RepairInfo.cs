using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 送修
    /// </summary>
    public class Pro_RepairInfo
    {
         private int MethodID;

	    public Pro_RepairInfo()
	    {
		    this.MethodID = 0;
	    }

        public Pro_RepairInfo(int MenthodID)
	    {
		    this.MethodID = MenthodID;
	    }


        /// <summary>
        /// 送修
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_RepairInfo model)
        {
            bool NoError = true;
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "实体为空" };

            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    DataLoadOptions dataload = new DataLoadOptions();

                    //dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return result; }

                 

                    #region 验证用户操作仓库  商品的权限 
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS,lqh);

                    if (ret.ReturnValue != true)
                    { return ret; }
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count>0&&!ValidHallIDS.Contains(model.HallID))
                    {
                        var que = from h in lqh.Umsdb.Pro_HallInfo
                                  where h.HallID == model.HallID
                                  select h;
                        return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                    }

                    //验证商品权限
                    if (ValidProIDS.Count > 0)
                    {
                        List<string> classids = new List<string>();
                        foreach (var item in model.Pro_RepairListInfo)
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


                    string msg = "";
                    List<string> IMEIList = new List<string>();
                    List<string> ProIDS = new List<string>();
                    List<string> InorderListID = new List<string>();
                    List<Model.Pro_IMEI> pro_imei_list = new List<Model.Pro_IMEI>();
                    List<Model.Pro_StoreInfo> pro_store_list = new List<Pro_StoreInfo>();
                    //生成单号 存储过程OrderMacker
                    //插入表头
                     

                    #region 表头
                    lqh.Umsdb.OrderMacker(1, "SX", "SX", ref msg);
                    if (string.IsNullOrEmpty(msg))
                    {
                        return new WebReturn() { ReturnValue = false, Message = "生成单号出错" };
                    }
                    model.RepairID = msg;
                    model.IsReturn = false;
                    #endregion

                    #region 明细 
                    if (model.Pro_RepairListInfo == null || model.Pro_RepairListInfo.Count() == 0)
                    {
                        return new WebReturn() { ReturnValue = false, Message = "送修的商品明细为空" };
                    }
                    foreach (var m in model.Pro_RepairListInfo)
                    {
                        ProIDS.Add(m.ProID);
                        if (!string.IsNullOrEmpty(m.IMEI))
                        {
                            if (IMEIList.Contains(m.IMEI) || m.ProCount !=1)
                            {
                                NoError = false;

                                m.Note = "串码重复或者有串码时 数量不为1";

                                continue;
                            }
                            IMEIList.Add(m.IMEI);
                        }
                        if (string.IsNullOrEmpty(m.InListID))
                        {
                            NoError = false;
                            m.Note = "批次号不能为空";
                            continue;
                        }
                        InorderListID.Add(m.InListID);
                    }
                    if (!NoError)
                    {
                        return new WebReturn() {  ReturnValue=false, Obj=model, Message="送修出错"};
                    }
                    
                    #endregion

                    #region 获取机型信息  库存信息 串码信息
                    var Pro_list = (from b in lqh.Umsdb.Pro_ProInfo
                                    where ProIDS.Contains(b.ProID)
                                    select b).ToList();
                    var Pro_IMEI_ = (from b in lqh.Umsdb.Pro_IMEI
                                    where IMEIList.Contains(b.IMEI) 
                                    select b).ToList();
                    var pro_storeInfo = (from b in lqh.Umsdb.Pro_StoreInfo
                                         where b.HallID == model.HallID && InorderListID.Contains(b.InListID)
                                         select b).ToList();
                    #endregion

                    #region 左连接型号

                    foreach (var item in model.Pro_RepairListInfo)
                    {
                        if (item.IMEI==null)
                        {
                            item.IMEI = string.Empty;
                        }
                    }
               
                    var InList_join_Pro_imei_store = from b in model.Pro_RepairListInfo//imeilist
                                                    join c in Pro_list
                                                    on b.ProID equals c.ProID
                                                    into temp1
                                                    from c1 in temp1.DefaultIfEmpty()
                                                    join d in Pro_IMEI_.DefaultIfEmpty()
                                                    on  (b.IMEI+"").ToLower() equals (d==null?"":d.IMEI +"").ToLower()
                                                    into temp2
                                                    from d1 in temp2.DefaultIfEmpty()
                                                    join e in pro_storeInfo
                                                    on b.InListID equals e.InListID
                                                    into temp3
                                                    from e1 in temp3.DefaultIfEmpty()
                                                    select new { Pro_RepairListInfo = b, Pro_ProInfo = c1, Pro_IMEI = d1, Pro_StoreInfo = e1 };


                        #region 验证批次 商品信息 库存 串码


                        foreach (var m in InList_join_Pro_imei_store)
                        {

                            //var m = InList_join_Pro[k];

                            if (m.Pro_ProInfo == null)
                            {
                                NoError = false;
                                m.Pro_RepairListInfo.Note = "机型不存在";
                                continue;
                            }

                            #region 验证批次号
                            if (m.Pro_StoreInfo == null || m.Pro_StoreInfo.ProCount < m.Pro_RepairListInfo.ProCount)
                            {
                                NoError = false;
                                m.Pro_RepairListInfo.Note = "库存不足";
                                continue;
                            }

                            #endregion

                            #region 验证串码有效性



                            if (string.IsNullOrEmpty(m.Pro_RepairListInfo.IMEI))
                            {
                                if (m.Pro_ProInfo.NeedIMEI == true)//有串码的机型
                                {
                                    NoError = false;
                                    m.Pro_RepairListInfo.Note = "必须提供串码";
                                    continue;
                                }
                            }
                            else
                            {

                                if (m.Pro_ProInfo.NeedIMEI != true)
                                {
                                    NoError = false;
                                    m.Pro_RepairListInfo.Note = "属于无串码商品";
                                    continue;
                                }
                              
                                if (m.Pro_IMEI == null ||
                                    (m.Pro_IMEI.BorowID != 0 && m.Pro_IMEI.BorowID != null) ||
                                    (m.Pro_IMEI.OutID != 0 && m.Pro_IMEI.OutID != null) ||
                                    (m.Pro_IMEI.RepairID != 0 && m.Pro_IMEI.RepairID != null) ||
                                    (m.Pro_IMEI.SellID != 0 && m.Pro_IMEI.SellID != null) ||
                                    (m.Pro_IMEI.VIPID != 0 && m.Pro_IMEI.VIPID != null)||
                                        (m.Pro_IMEI.AssetID != 0 && m.Pro_IMEI.AssetID != null)
                                    )
                                {
                                    NoError = false;
                                    m.Pro_RepairListInfo.Note = "串码已处理";
                                    continue;
                                }
                                if (m.Pro_IMEI.HallID != model.HallID || m.Pro_IMEI.ProID != m.Pro_RepairListInfo.ProID || m.Pro_RepairListInfo.InListID != m.Pro_IMEI.InListID)
                                {
                                    NoError = false;
                                    m.Pro_RepairListInfo.Note = "串码的商品型号不对，此串码属于" + m.Pro_IMEI.ProID + ",批次号" + m.Pro_IMEI.InListID;
                                    continue;
                                }
                                m.Pro_IMEI.Pro_RepairListInfo = m.Pro_RepairListInfo;
                                m.Pro_IMEI.State = 1;
                            }

                            #endregion

                            //减库存
                            m.Pro_StoreInfo.ProCount -= m.Pro_RepairListInfo.ProCount;
                            if (m.Pro_StoreInfo.ProCount < 0)
                            {
                                return new WebReturn() { ReturnValue=false,Message="库存不足，送修失败！"};
                            }
                        }
                        #endregion

                    #endregion

                    if (!NoError)
                    {
                        return new WebReturn() { ReturnValue = false, Message = "送修出错", Obj = model };
                    }

                    model.IsDelete = false;
                    model.IsReceive = false;
                    //插入串码明细                    
                    lqh.Umsdb.Pro_RepairInfo.InsertOnSubmit(model);
                    lqh.Umsdb.SubmitChanges();

                    return new Model.WebReturn() { ReturnValue = true, Message = "送修成功" };

                }

            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "系统错误" + ex.Message };
            }  
        }

        /// <summary>
        /// 获取指定用户未归还的送修单
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Model.WebReturn GetRepairListByUID(Model.Sys_UserInfo user, int pageSize, int pageIndex, string uid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        int? total = 0;
                         List<Model.GetRepairListByUIDResult> results  =  lqh.Umsdb.GetRepairListByUID(pageSize,pageIndex,uid,ref total).ToList();

                        if (results.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false};
                        }

                        List<Model.RepairModel> models = new List<Model.RepairModel>();
                        Model.RepairModel rm = null;

                        foreach (var item in results)
                        {
                            rm = new Model.RepairModel();
                            rm.ID =int.Parse( item.ID.ToString());
                            rm.HallID = item.HallID;
                            rm.HallName = item.HallName;
                            rm.Note = item.Note;
                            rm.SysDate = Convert.ToDateTime(item.SysDate.ToString());
                            rm.RepairDate = Convert.ToDateTime(item.RepairDate.ToString());
                            rm.RepairID = item.RepairID;
                            rm.UserName = item.UserName;
                            rm.UserID = item.UserID;
                            models.Add(rm);
                        }
                        ArrayList arrlist = new ArrayList();
                        //double page = double.Parse((total / (pageSize * 1.0)).ToString()); Math.Ceiling(page )
                        arrlist.Add(total);
                        
                        return new Model.WebReturn() {Obj=models, ReturnValue = true,ArrList=arrlist };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                    }
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="user"></param>
        /// <param name="?"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn Search(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
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

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return new WebReturn() { ReturnValue = false, Obj  = pageParam }; }


                    if (pageParam == null || pageParam.PageIndex < 0 )
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var repair_query = from b in lqh.Umsdb.View_Pro_RepairInfo
                                        select b;

                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "IsReturn":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)item;

                                       repair_query = from b in repair_query
                                                      where b.IsReturn == mm.ParamValues
                                                      select b;
                                   
                                    break;

                            case "IsReceive":
                                    Model.ReportSqlParams_String receive = (Model.ReportSqlParams_String)item;

                                    repair_query = from b in repair_query
                                                   where b.IsReceive == receive.ParamValues
                                                   select b;

                                    break;

                            case "StartTime":
                                    Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                    if (mm5.ParamValues != null)
                                    {
                                        Model.ReportSqlParams_DataTime mm6 = new ReportSqlParams_DataTime();
                                        mm6.ParamValues = DateTime.Now;
                                        foreach (var xxd in pageParam.ParamList)
                                        {
                                            if (xxd.ParamName == "EndTime")
                                            {
                                                mm6 = (Model.ReportSqlParams_DataTime)xxd;
                                                break;
                                            }
                                        }
                                        if (mm5.ParamValues == mm6.ParamValues)
                                        {
                                            repair_query = from b in repair_query
                                                           where b.SysDate >= mm5.ParamValues && 
                                                           b.SysDate < DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                                                           select b;
                                        }
                                        else
                                        {
                                            repair_query = from b in repair_query
                                                           where b.SysDate >= mm5.ParamValues && b.SysDate <= mm6.ParamValues
                                                           select b;
                                        }
                                    }
                                    break;
                        
                            case "UserName":
                                    Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                    if (para.ParamValues != null)
                                    {
                                        repair_query = from b in repair_query
                                                      where b.UserName.Contains(para.ParamValues)
                                                      select b;
                                    }
                                    break;
                            case "HallID":
                                    Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                    if (para1.ParamValues != null)
                                    {
                                        repair_query = from b in repair_query
                                                      where para1.ParamValues.Contains(b.HallID)
                                                      select b;
                                    }
                                    break;
                        }
                    }

                    #endregion

                    #region 过滤仓库

                    if (ValidHallIDS.Count() > 0)
                    {
                        repair_query = from b in repair_query
                                       where ValidHallIDS.Contains(b.HallID)
                                       orderby b.SysDate descending
                                       select b;
                    }
                    else
                    {
                        repair_query = from b in repair_query
                                       orderby b.SysDate descending
                                       select b;
                    }
                    #endregion
                    pageParam.RecordCount = repair_query.Count();

                    #region 判断是否超过总页数

                    int pagecount =pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from r in repair_query.Take(pageParam.PageSize).ToList()
                                     select r;

                        List<Model.View_Pro_RepairInfo> list = results.ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    else
                    {
                        var results = from r in repair_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                   
                                     select r;

                        List<Model.View_Pro_RepairInfo> list = results.ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
               catch(Exception ex)
               {
                     return new WebReturn() { ReturnValue = false, Message=ex.Message};
               }
             }
        }

        /// <summary>
        /// 获取送修单明细
        /// </summary>
        /// <param name="repairid"></param>
        /// <returns></returns>
        public Model.WebReturn GetRepairListInfoByRID(Model.Sys_UserInfo user, int repairid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var query = from list in lqh.Umsdb.View_RepaireRetList
                                    where  list.RepairID == repairid
                                    select list;
                        if (query.Count() != 0)
                        {
                            return new Model.WebReturn() { ReturnValue = true, Obj = query.ToList() };
                        }
                        else
                        {
                            return new Model.WebReturn() { ReturnValue = false,Obj = new List<Model.View_RepaireRetList>()};
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                    }
                }
            }
        }


        /// <summary>
        /// 返库取消和接收查询
        /// </summary>
        /// <param name="user"></param>
        /// <param name="?"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn CancelSearch(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
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

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return new WebReturn() { ReturnValue = false, Obj = pageParam }; }

                    if (pageParam == null || pageParam.PageIndex < 0 )
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var repair_query = from b in lqh.Umsdb.View_RepairInfo
                                       select b;

                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "IsReturn":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)item;


                                repair_query = from b in repair_query
                                               where b.IsReturn == mm.ParamValues
                                               select b;

                                break;

                            case "IsReceive":
                                Model.ReportSqlParams_String receive = (Model.ReportSqlParams_String)item;


                                repair_query = from b in repair_query
                                               where b.IsReceive == receive.ParamValues
                                               select b;

                                break;

                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                if (mm5.ParamValues != null)
                                {
                                    Model.ReportSqlParams_DataTime mm6 = new ReportSqlParams_DataTime() ;
                                    mm6.ParamValues = DateTime.Now;
                                    foreach (var xxd in pageParam.ParamList)
                                    {
                                        if (xxd.ParamName == "EndTime")
                                        {
                                            mm6 = (Model.ReportSqlParams_DataTime)xxd;
                                            break;
                                        }
                                    }
                                    if (mm5.ParamValues == mm6.ParamValues)
                                    {
                                        repair_query = from b in repair_query
                                                       where b.SysDate >= mm5.ParamValues && b.SysDate <DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                                                       select b;
                                    }
                                    else
                                    {
                                        repair_query = from b in repair_query
                                                       where b.SysDate >= mm5.ParamValues && b.SysDate <= mm6.ParamValues
                                                       select b;
                                    }
                                }
                                break;
                      
                            case "UserName":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    repair_query = from b in repair_query
                                                   where b.UserName.Contains(para.ParamValues)
                                                   select b;
                                }
                                break;

                            case "OldID":
                                Model.ReportSqlParams_String old = (Model.ReportSqlParams_String)item;
                                if (old.ParamValues != null)
                                {
                                    repair_query = from b in repair_query
                                                   where b.OldID.Contains(old.ParamValues)
                                                   select b;
                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                if (para1.ParamValues != null)
                                {
                                    repair_query = from b in repair_query
                                                   where para1.ParamValues.Contains(b.HallID)
                                                   select b;
                                }
                                break;
                        }
                    }

                    #endregion

                    #region 过滤仓库

                    if (ValidHallIDS.Count() > 0)
                    {
                        repair_query = from b in repair_query
                                       where ValidHallIDS.Contains(b.HallID)
                                       orderby b.SysDate descending
                                       select b;
                    }
                    else
                    {
                        repair_query = from b in repair_query
                                       orderby b.SysDate descending
                                       select b;
                    }
                    #endregion

                    pageParam.RecordCount = repair_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from r in repair_query.Take(pageParam.PageSize).ToList()
                                      select r;

                  
                        List<Model.View_RepairInfo> list = results.ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    else
                    {
                        var results = from r in repair_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                               
                                      select r;

                        List<Model.View_RepairInfo> list = results.ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }


        /// <summary>
        /// 获取送修单明细  包括所有串码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="repairID"></param>
        /// <returns></returns>
        public Model.WebReturn GetDetailByID(Model.Sys_UserInfo user, int repairID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var returnList = from r in lqh.Umsdb.Pro_RepairInfo
                                     join list in lqh.Umsdb.Pro_RepairListInfo on r.ID equals list.RepairID
                                     join p in lqh.Umsdb.Pro_ProInfo
                                     on list.ProID equals p.ProID
                                     join t in lqh.Umsdb.Pro_TypeInfo on p.Pro_TypeID equals t.TypeID
                                     join c in lqh.Umsdb.Pro_ClassInfo on p.Pro_ClassID equals c.ClassID
                                     where r.ID == repairID
                                     select new
                                     {
                                         list.ProID,
                                         list.ProCount,
                                         list.Note,
                                         list.InListID,
                                         list.RepairListID,
                                         p.ProName,
                                         p.ProFormat,
                                         t.TypeName,
                                         c.ClassName,
                                         list.IMEI
                                     };

                    if (returnList.Count() == 0)
                    {
                        return new WebReturn() { ReturnValue = false, Message = "该单无详情" };
                    }
                    List<CancelListModel> models = new List<CancelListModel>();

                    CancelListModel rlm = null;
                    foreach (var item in returnList)
                    {
                        rlm = new CancelListModel();
                        rlm.ClassName = item.ClassName;
                        rlm.InListID = item.InListID;
                        rlm.Note = item.Note;
                        rlm.ProCount = decimal.Round(item.ProCount, 2);
                        rlm.ProID = item.ProID;
                        rlm.ProName = item.ProName;
                        rlm.TypeName = item.TypeName;
                        rlm.ImeiList = new List<IMEIModel>();
                        rlm.IMEI = item.IMEI;
                        rlm.ProFormat = item.ProFormat;
                        
                        //var imei = from im in lqh.Umsdb.Pro_ReturnOrderIMEI
                        //           where im.ReturnListID == item.RepairListID
                        //           select im;

                        //IMEIModel orimei = null;
                        //foreach (var child in imei)
                        //{
                        //    orimei = new IMEIModel();
                        //    orimei.NewIMEI = child.IMEI;
                        //    orimei.Note = child.Note;

                        //    rlm.ImeiList.Add(orimei);
                        //}
                        models.Add(rlm);
                    }
                    return new WebReturn() { ReturnValue = true, Obj = models };
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }

       /// <summary>
        ///取消 送修
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, int repairID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region  验证用户审批权限


                        if (user.CancelLimit == null || user.CancelLimit == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "权限不足，取消失败！" };
                        }

                        #endregion 

                        #region 获取送修详情
                        var query = from b in lqh.Umsdb.Pro_RepairInfo
                                    where b.ID == repairID && (b.IsDelete == false || Nullable<bool>.Equals(b.IsDelete, null))
                                    select b;
                        if (query.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该单不存在，取消失败！" };
                        }
                        if (Convert.ToBoolean(query.First().IsDelete))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该送修单已取消！" };
                        }
                        if (Convert.ToBoolean(query.First().IsReceive))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该送修单已接收，无法取消！" };
                        }

                        Model.Pro_RepairInfo model = query.First();

                        var detail = from det in lqh.Umsdb.Pro_RepairListInfo
                                     join b in lqh.Umsdb.Pro_RepairInfo
                                     on det.RepairID equals b.ID
                                     where b.ID == repairID
                                     select det;
                        List<Model.Pro_RepairListInfo> repairList = detail.ToList();

                        #endregion

                        #region 权限验证

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS,lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count>0&&!ValidHallIDS.Contains(model.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                        }

                        //验证商品权限
                        if (ValidProIDS.Count > 0)
                        {
                            List<string> classids = new List<string>();
                            foreach (var item in repairList)
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

                        #region  验证取消是否超时

                        if (Convert.ToDecimal(user.CancelLimit) != 0)
                        {
                            var intime = from retn in lqh.Umsdb.Pro_RepairInfo
                                         where retn.ID == repairID
                                         select retn;
                            DateTime rdate = DateTime.Parse(intime.First().RepairDate.ToString());
                            TimeSpan dateDiff = DateTime.Now.Subtract(rdate);

                            if (dateDiff.TotalHours > (double)user.CancelLimit)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "取消超时,取消失败！" };
                            }
                        }

                        #endregion 


                        foreach (var item in repairList)
                        { 
                           // 更新串码表

                            if (!string.IsNullOrEmpty(item.IMEI))
                            {
                                var imei = lqh.Umsdb.Pro_IMEI.Where(p => p.IMEI == item.IMEI);
                                if (imei.Count() != 0)
                                {
                                    imei.First().RepairID = null;
                                    imei.First().State = 0;
                                }
                            }
                            //加库存
                            var store = from s in lqh.Umsdb.Pro_StoreInfo
                                        where s.HallID == query.First().HallID && s.InListID == item.InListID
                                        && s.ProID == item.ProID
                                        select s;
                            if (store.Count() <= 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "库存不足，接收有误" };
                            }
                            store.First().ProCount += item.ProCount;
                        }
                     
                        model.IsDelete = true;
                        model.DeleteDate = DateTime.Now;
                        model.Deleter = user.UserName;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue = true, Message = "取消成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="user"></param>
        /// <param name="repairID"></param>
        /// <returns></returns>
        public Model.WebReturn Receive(Model.Sys_UserInfo user, int repairID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region  获取送修明细

                        var query = from b in lqh.Umsdb.Pro_RepairInfo
                                    where b.ID == repairID && b.IsReturn == false && b.IsReceive == false
                                    select b;

                        if (query.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "送修单不存在或该送修单已接收" };
                        }
                        if (Convert.ToBoolean(query.First().IsDelete))
                        {
                            return new Model.WebReturn() { ReturnValue = false,Message="送修单已取消，接收失败！"};
                        }
                        var detail = from det in lqh.Umsdb.Pro_RepairListInfo
                                     join b in lqh.Umsdb.Pro_RepairInfo
                                     on det.RepairID equals b.ID
                                     join p in lqh.Umsdb.Pro_ProInfo
                                     on det.ProID equals p.ProID
                                     where b.ID == repairID
                                     select new
                                     {
                                         det.RepairListID,
                                         det.RepairID,
                                         det.ProCount,
                                         det.InListID,
                                         p.NeedIMEI,
                                         det.ProID
                                     };

                        #endregion

                        query.First().IsReceive = true;
                        query.First().Receiver = user.UserID;
                        query.First().RecvTime = DateTime.Now;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue = true, Message = "接收成功" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }

        }
    }
}
