using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class ASP_RepairInfo
    {
	    private int MethodID;

        public ASP_RepairInfo()
        {
            this.MethodID = 0;
        }

        public ASP_RepairInfo(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        /// 维修  324
        /// </summary>
        /// <param name="user"></param>
        /// <param name="repinfo"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.ASP_RepairInfo repinfo)
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

                        List<int> classids = new List<int>();
                        foreach (var item in repinfo.ASP_CurrentOrder_BackupPhoneInfo)
                        {
                            var queey = from p in lqh.Umsdb.Pro_ProInfo
                                        where p.ProID == item.ProID
                                        select p;
                            classids.Add((int)queey.First().Pro_ClassID);
                        }
                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #region  过滤仓库
                        //if (ValidHallIDS.Count > 0)
                        //{
                        //    if (!ValidHallIDS.Contains(repinfo.HallID))
                        //    {
                        //        var que = from h in lqh.Umsdb.Pro_HallInfo
                        //                  where h.HallID == recinfo.HallID
                        //                  select h;
                        //        return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                        //    }
                        //}
                        #endregion

                        #region  过滤商品
                        if (ValidProIDS.Count > 0)
                        {
                            foreach (var item in classids)
                            {
                                if (!ValidProIDS.Contains(item.ToString()))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                                }
                            }
                        }
                        #endregion

                        #endregion 

                        #region 获取 CurrentOrder  验证备机及配件   若已经存在维修单 则修改 否则新增

                        var corder = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                     where a.ID == repinfo.OrderID
                                     select a;
                      
                        if (corder.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue=false,Message="未能找到指定维修单，保存失败！"};
                        }
                        Model.ASP_CurrentOrderInfo order = corder.First();
                        bool lack = false;
                        repinfo.IsLack = false;
                        order.RepKind = repinfo.RepKind;

                        #region    若已经存在维修单 则修改 否则新增

                        if (order.CurrentRepairID > 0)
                        {
                            //若已经存在维修单 则修改
                            #region  修改维修单

                            var rep = from a in lqh.Umsdb.ASP_RepairInfo
                                      where a.ID == order.CurrentRepairID
                                      select a;
                            if (rep.Count() > 0)
                            {
                                Model.ASP_RepairInfo arep = rep.First();
                                arep.WorkMoney = repinfo.WorkMoney;
                                arep.ProMoney = repinfo.ProMoney;
                                arep.RepairNote = repinfo.RepairNote;
                                arep.LackNote = repinfo.LackNote;
                                arep.Chk_InOut = repinfo.Chk_InOut;
                                arep.HasRepaired = true;

                                #region   更新备机信息
                                if (repinfo.ASP_CurrentOrder_BackupPhoneInfo == null)
                                {
                                    repinfo.ASP_CurrentOrder_BackupPhoneInfo = new System.Data.Linq.EntitySet<ASP_CurrentOrder_BackupPhoneInfo>();
                                }
                                if (order.HasBJ == false && repinfo.ASP_CurrentOrder_BackupPhoneInfo.Count > 0)
                                {
                                    arep.ASP_CurrentOrder_BackupPhoneInfo = new System.Data.Linq.EntitySet<ASP_CurrentOrder_BackupPhoneInfo>();
                                    arep.ASP_CurrentOrder_BackupPhoneInfo.AddRange(repinfo.ASP_CurrentOrder_BackupPhoneInfo);
                                }

                                #endregion
                                lqh.Umsdb.SubmitChanges();
                            }

                            #endregion

                            #region 更新配件

                            repinfo.ID = Convert.ToInt32(order.CurrentRepairID);
                            List<int> ids = new List<int>();
                            foreach (var item in repinfo.ASP_CurrentOrder_Pros)
                            {
                                ids.Add(item.ID);
                            }
                            var oldPros = from a in lqh.Umsdb.ASP_RepairInfo
                                          join b in lqh.Umsdb.ASP_CurrentOrder_Pros on a.ID equals b.RepairID
                                          where a.ID == repinfo.ID && ids.Contains(b.ID) == false
                                          select b;
                            if (oldPros.Count() > 0)
                            {
                                lqh.Umsdb.ASP_CurrentOrder_Pros.DeleteAllOnSubmit(oldPros);
                            }

                            //更新配件后判断是否仍然缺料
                            bool stillLack = false;
                            foreach (var childx in repinfo.ASP_CurrentOrder_Pros)
                            {
                                if (childx.IsLack == true)
                                {
                                    stillLack = true;
                                    break;
                                }
                            }
                            repinfo.IsLack = stillLack;

                            #endregion

                            #region 更新故障

                            List<int> errs = new List<int>();
                            foreach (var item in repinfo.ASP_CurrentOrder_ErrorInfo)
                            {
                                errs.Add(item.ErrorID);
                            }
                            var errInfo = from a in lqh.Umsdb.ASP_CurrentOrder_ErrorInfo
                                          where a.RepairID == repinfo.ID && errs.Contains(a.ErrorID) == false
                                          select a;
                            if (errInfo.Count() > 0)
                            {
                                lqh.Umsdb.ASP_CurrentOrder_ErrorInfo.DeleteAllOnSubmit(errInfo);
                            }

                            #endregion

                            #region 标记送修方式及是否仍缺料

                            var repairs = from m in lqh.Umsdb.ASP_RepairInfo
                                          where m.ID == repinfo.ID
                                          select m;
                            if (repairs.Count() == 0)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "未能找到指定数据，保存失败！" };
                            }
                            Model.ASP_RepairInfo model = repairs.First();
                            model.IsLack = stillLack;
                            if (repinfo.Chk_RType == "送厂维修")
                            {
                                model.NeedToFact = true;
                            }
                            else
                            {
                                model.NeedToFact = false;
                            }

                            #endregion
                        }
                        else
                        {
                            //否则新增
                            #region 复制数据

                            if (order.HasBJ == true)
                            {
                                repinfo.HasBJ = false;
                            }
                            else
                            {
                                repinfo.HasBJ = true;
                            }

                            if (order.HasBJ == true)
                            {
                                repinfo.BJ_Money = order.BJ_Money;
                                repinfo.BJDate = order.BJ_Date;
                                repinfo.BJHallID = order.BJ_HallID;
                                repinfo.BJUserID = order.BJ_UserID;
                            }

                            repinfo.Chk_FID = order.Chk_FID;
                            repinfo.Chk_InOut = order.Chk_InOut;
                            repinfo.Chk_Note = order.Chk_Note;
                            repinfo.ChkDate = order.ChkDate;
                            repinfo.ChkNote = order.ChkNote;
                            repinfo.ChkUserID = order.ChkUserID;
                            repinfo.Cus_Add = order.Cus_Add;
                            repinfo.Cus_CPC = order.Cus_CPC;
                            repinfo.Cus_CusType = order.Cus_CusType;
                            repinfo.Cus_Email = order.Cus_Email;
                            repinfo.Cus_Name = order.Cus_Name;
                            repinfo.Cus_Phone = order.Cus_Phone;
                            repinfo.Cus_Phone2 = order.Cus_Phone2;
                            repinfo.Cus_VIPID = order.Cus_VIPID;
                            repinfo.HallID = order.HallID;
                            repinfo.HasBJ = order.HasBJ;
                            repinfo.OldID = order.OldID;
                            repinfo.Pro_Bill = order.Pro_Bill;
                            repinfo.Pro_BuyT = order.Pro_BuyT;
                            repinfo.Pro_Color = order.Pro_Color;
                            repinfo.Pro_Error = order.Pro_Error;
                            repinfo.Pro_GetT = order.Pro_GetT;
                            repinfo.Pro_HeaderIMEI = order.Pro_HeaderIMEI;
                            repinfo.Pro_IMEI = order.Pro_IMEI;
                            repinfo.Pro_Name = order.Pro_Name;
                            repinfo.Pro_Note = order.Pro_Note;
                            repinfo.Pro_Other = order.Pro_Other;
                            repinfo.Pro_OutSide = order.Pro_OutSide;
                            repinfo.Pro_Seq = order.Pro_Seq;
                            repinfo.Pro_SN = order.Pro_SN;
                            repinfo.Pro_Type = order.Pro_Type;
                            repinfo.Receiver = order.Receiver;
                            repinfo.RepairCount = order.RepairCount;
                            repinfo.Repairer = order.Repairer;
                            repinfo.Sender = order.Sender;
                            repinfo.SenderPhone = order.SenderPhone;
                            #endregion

                            order.Chk_RType = repinfo.Chk_RType;
                            repinfo.IsLack = lack;
                            repinfo.SysDate = DateTime.Now;
                            lqh.Umsdb.ASP_RepairInfo.InsertOnSubmit(repinfo);

                        }
                        lqh.Umsdb.SubmitChanges();

                        #endregion 

                        #region 验证配件信息

                        string msg = "";

                        foreach (var item in repinfo.ASP_CurrentOrder_Pros)
                        {
                            if (item.IsLack == true)
                            {
                                lack = true;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(item.IMEI))
                            {
                                #region 验证串码
                                var list = from a in lqh.Umsdb.Pro_IMEI
                                           where a.IMEI.ToUpper() == item.IMEI.ToUpper()
                                           select a;
                                if (list.Count() == 0)
                                {
                                    return new Model.WebReturn() { Message = "配件串码 " + item.IMEI + " 不存在！", ReturnValue = false };
                                }

                                Model.Pro_IMEI imei = list.First();
                                //验证串码
                                Model.WebReturn ret1 = Common.Utils.CheckIMEI(imei);
                                if (ret1.ReturnValue == false)
                                {
                                    item.IsLack = true;
                                    ret1.ArrList = new System.Collections.ArrayList() { true };
                                    return ret1;
                                }
                                imei.PJID = repinfo.ID;
                                imei.State = 1;

                                #endregion

                                #region  验证库存是否充足

                                var store = from a in lqh.Umsdb.Pro_StoreInfo
                                            where a.ProID == item.ProID && a.HallID == repinfo.BJHallID && a.InListID == item.InListID
                                            && a.ProCount > 0
                                            select a;
                                if (store.Count() == 0)
                                {
                                    var p = from a in lqh.Umsdb.Pro_ProInfo
                                            where a.ProID == item.ProID
                                            select a;
                                    return new WebReturn() { ReturnValue = false, Message = "商品" + p.First().ProName + "的库存不足！" };
                                }
                                #endregion

                                //库存充足则减库存
                                store.First().ProCount -= 1;
                                lqh.Umsdb.SubmitChanges();
                            }
                        }

                        #endregion

                        #region 验证备机串码

                        if (order.HasBJ == false)
                        {
                            if (repinfo.ASP_CurrentOrder_BackupPhoneInfo.Count > 0)
                            {
                                repinfo.HasBJ = true;
                                repinfo.BJDate = DateTime.Now;

                                foreach (var item in repinfo.ASP_CurrentOrder_BackupPhoneInfo)
                                {
                                    #region  验证串码
                                    if (!string.IsNullOrEmpty(item.IMEI))
                                    {
                                        var list = from a in lqh.Umsdb.Pro_IMEI
                                                   where a.IMEI == item.IMEI
                                                   select a;
                                        if (list.Count() == 0)
                                        {
                                            return new Model.WebReturn() { Message = "备机串码 " + item.IMEI + " 不存在！", ReturnValue = false };
                                        }
                                        Model.Pro_IMEI imei = list.First();
                                        //验证串码
                                        Model.WebReturn ret1 = Common.Utils.CheckIMEI(imei);
                                        if (ret1.ReturnValue == false)
                                        {
                                            //ret1.ArrList = new System.Collections.ArrayList() { true };
                                            return ret1;
                                        }
                                        imei.BJID = repinfo.ID;
                                        imei.State = 1;
                                    }
                                    #endregion

                                    #region  验证库存是否充足

                                    var store = from a in lqh.Umsdb.Pro_StoreInfo
                                                where a.ProID == item.ProID && a.HallID == repinfo.BJHallID && a.InListID == item.InListID
                                                && a.ProCount > 0
                                                select a;
                                    if (store.Count() == 0)
                                    {
                                        var p = from a in lqh.Umsdb.Pro_ProInfo
                                                where a.ProID == item.ProID
                                                select a;
                                        return new WebReturn() { ReturnValue = false, Message = "商品" + p.First().ProName + "的库存不足！" };
                                    }
                                    //库存充足则减库存
                                    store.First().ProCount -= 1;
                                    lqh.Umsdb.SubmitChanges();
                                    #endregion
                                }
                            }
                        }
                        else
                        {  //若原始单已经备机则copy一份数据到RepairInfo
                            repinfo.ASP_CurrentOrder_BackupPhoneInfo = new System.Data.Linq.EntitySet<ASP_CurrentOrder_BackupPhoneInfo>();

                            foreach (var item in order.ASP_CurrentOrder_BackupPhoneInfo)
                            {
                                Model.ASP_CurrentOrder_BackupPhoneInfo model = new ASP_CurrentOrder_BackupPhoneInfo();
                                model.IMEI = item.IMEI;
                                model.InListID = item.InListID;
                                model.NewIMEI = item.NewIMEI;
                                model.NewProID = item.NewProID;
                                model.OrderID = item.OrderID;
                                model.ProCount = item.ProCount;
                                model.ProID = item.ProID;
                                model.ReceiveID = item.ReceiveID;
                                repinfo.ASP_CurrentOrder_BackupPhoneInfo.Add(model);
                            }
                        }
                        #endregion

                        if (lack == true)
                        {
                            repinfo.HasRepaired = false;
                        }
                        else
                        {
                            repinfo.HasRepaired = true;
                        }
                        lqh.Umsdb.SubmitChanges();

                        #endregion

                        #region  标记CurrentOrder 的 HasRepaired

                        order.HasRepaired = repinfo.HasRepaired;
                        if (Convert.ToInt32(order.CurrentRepairID) == 0)
                        {
                            order.CurrentRepairID = repinfo.ID;
                        }

                        #endregion

                        #region  
                        //if (repinfo.IsLack == false)
                        //{
                        //    #region  标记配件串码已使用  并减库存
                        //    foreach (var item in repinfo.ASP_CurrentOrder_Pros)
                        //    {
                        //        //若商品缺料则不标记
                        //        if (item.IsLack == true || string.IsNullOrEmpty(item.IMEI))
                        //        {
                        //            continue;
                        //        }
                        //        var list = from a in lqh.Umsdb.Pro_IMEI
                        //                   where a.IMEI.ToUpper() == item.IMEI.ToUpper()
                        //                   select a;
                        //        if (list.Count() == 0)
                        //        {
                        //            return new Model.WebReturn() { Message = "配件串码 " + item.IMEI + " 不存在！", ReturnValue = false };
                        //        }

                        //        Model.Pro_IMEI imei = list.First();
                        //        imei.PJID = repinfo.ID;
                        //        imei.State = 1;

                        //        var store = from a in lqh.Umsdb.Pro_StoreInfo
                        //                    where a.ProID == item.ProID && a.HallID == repinfo.RepairerHallID && a.InListID == item.InListID
                        //                    && a.ProCount > 0
                        //                    select a;
                        //        if (store.Count() == 0)
                        //        {
                        //            item.IsLack = true;
                        //            var p = from a in lqh.Umsdb.Pro_ProInfo
                        //                    where a.ProID == item.ProID
                        //                    select a;
                        //            msg += p.First().ProName + " ";
                        //        }

                        //        //库存充足则减库存
                        //        store.First().ProCount -= 1;
                        //        lqh.Umsdb.SubmitChanges();
                        //    }

                        //    #endregion

                        //    #region 标记串码已备机   并减库存

                        //    foreach (var item in repinfo.ASP_CurrentOrder_BackupPhoneInfo)
                        //    {
                        //        if (string.IsNullOrEmpty(item.IMEI))
                        //        {
                        //            continue;
                        //        }
                        //        var list = from a in lqh.Umsdb.Pro_IMEI
                        //                   where a.IMEI.ToUpper() == item.IMEI.ToUpper()
                        //                   select a;
                        //        if (list.Count() == 0)
                        //        {
                        //            return new Model.WebReturn() { Message = "备机串码 " + item.IMEI + " 不存在！", ReturnValue = false };
                        //        }
                        //        list.First().BJID = repinfo.ID;
                        //        list.First().State = 1;

                        //        var store = from a in lqh.Umsdb.Pro_StoreInfo
                        //                    where a.ProID == item.ProID && a.HallID == repinfo.BJHallID && a.InListID == item.InListID
                        //                    && a.ProCount > 0
                        //                    select a;
                        //        if (store.Count() == 0)
                        //        {
                        //            var p = from a in lqh.Umsdb.Pro_ProInfo
                        //                    where a.ProID == item.ProID
                        //                    select a;
                        //            return new WebReturn() { ReturnValue = false, Message = "商品" + p.First().ProName + "的库存不足！" };
                        //        }

                        //        //库存充足则减库存
                        //        store.First().ProCount -= 1;
                        //        lqh.Umsdb.SubmitChanges();
                        //    }
                        //    #endregion
                        //}

#endregion 

                        if (repinfo.Chk_RType == "送厂维修")
                        {
                            repinfo.RpState = (int)Common.RepairState.WFToFac;
                            order.RpState = (int)Common.RepairState.WFToFac;
                        }
                        else
                        {
                            if (repinfo.IsLack==false)
                            {
                                repinfo.RpState = (int)Common.RepairState.WFZhiJian;
                                order.RpState = (int)Common.RepairState.WFZhiJian;
                            }
                        }

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true ,Message="保存成功！"};   
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() {ReturnValue =false,Message = ex.Message };   
                    }
                }
            }
        }

		/// <summary>
		/// 325
		/// </summary>
		/// <param name="user"></param>
		/// <param name="pageParam"></param>
		/// <returns></returns>
        public Model.WebReturn Search(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region 权限

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return new WebReturn() { ReturnValue = false, Obj = pageParam }; }

                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var aduit_query = from b in lqh.Umsdb.View_ASPRepairInfo
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "CHK_FID":
                                Model.ReportSqlParams_String fid = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.Chk_FID.ToString() == fid.ParamValues
                                              select b;

                                break;

                            case "IsFetchAduit":
                                Model.ReportSqlParams_Bool IsFetchAduit = (Model.ReportSqlParams_Bool)item;

                                if (IsFetchAduit.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsFetchAduit == IsFetchAduit.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsFetchAduit == null || b.IsFetchAduit == false
                                                  select b;
                                }
                                break;

                            case "FetchHasAuditedOrUnNeedAudit":

                                aduit_query = from b in aduit_query
                                            where (b.IsFetchAduit == true)||(b.FetchNeedAudit==null || b.FetchNeedAudit==false)
                                            select b;
                                
                                break;
                            case "FetchAuditPassed":
                                Model.ReportSqlParams_Bool FetchAuditPassed = (Model.ReportSqlParams_Bool)item;

                                if (FetchAuditPassed.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.FetchAuditPassed == FetchAuditPassed.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.FetchAuditPassed == null || b.FetchAuditPassed == false
                                                  select b;
                                }
                                break;
                            case "IsLack":
                                Model.ReportSqlParams_Bool IsLack = (Model.ReportSqlParams_Bool)item;

                                if (IsLack.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsLack == IsLack.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsLack == null || b.IsLack == false
                                                  select b;
                                }
                                break;
                            case "FetchNeedAudit":
                                Model.ReportSqlParams_Bool FetchNeedAudit = (Model.ReportSqlParams_Bool)item;

                                if (FetchNeedAudit.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.FetchNeedAudit == FetchNeedAudit.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.FetchNeedAudit == null || b.FetchNeedAudit == false
                                                  select b;
                                }
                                break;
                            case "HasCallBack":
                                Model.ReportSqlParams_Bool HasCallBack = (Model.ReportSqlParams_Bool)item;

                                if (HasCallBack.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasCallBack == HasCallBack.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasCallBack == null || b.HasCallBack == false
                                                  select b;
                                }
                                break;
                            case "HasRepaired":
                                Model.ReportSqlParams_Bool repaired = (Model.ReportSqlParams_Bool)item;

                                if (repaired.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasRepaired == repaired.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasRepaired == null || b.HasRepaired == false
                                                  select b;
                                }
                                break;

                            case "HasAudited":
                                Model.ReportSqlParams_Bool HasAudited = (Model.ReportSqlParams_Bool)item;

                                if (HasAudited.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAudited == HasAudited.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAudited==null || b.HasAudited ==false
                                                  select b;
                                }

                                break;
                            case "HasFetch":
                                Model.ReportSqlParams_Bool HasFetch = (Model.ReportSqlParams_Bool)item;

                                if (HasFetch.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasFetch == HasFetch.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasFetch==null || b.HasFetch ==false
                                                  select b;
                                }
                                break;
                            case "ZJPassed":
                                 Model.ReportSqlParams_Bool pass = (Model.ReportSqlParams_Bool)item;

                                 if (pass.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.ZJPassed == pass.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.ZJPassed == null || b.ZJPassed == false  //质检不通过则此单完结
                                                  select b;
                                }
                                break;

                            case  "IsPassed":
                                Model.ReportSqlParams_Bool IsPassed = (Model.ReportSqlParams_Bool)item;

                                if (IsPassed.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsPassed == IsPassed.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsPassed == null || b.IsPassed == false
                                                  select b;
                                }
                                break;
                            case "NeedToFact":
                                Model.ReportSqlParams_Bool NeedToFact = (Model.ReportSqlParams_Bool)item;

                                if (NeedToFact.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.NeedToFact == NeedToFact.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.NeedToFact == null || b.NeedToFact == false
                                                  select b;
                                }
                                break;
                            case "IsBack":
                                Model.ReportSqlParams_Bool IsBack = (Model.ReportSqlParams_Bool)item;

                                if (IsBack.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsBack == IsBack.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsBack == null || b.IsPassed == false
                                                  select b;
                                }
                                break;

                            case "NoToFactOrToFacIsBack":  //送厂后已返厂 或者  无需送厂已维修的  （质检过滤此条件）
                               
                                    aduit_query = from b in aduit_query
                                                  where (b.NeedToFact ==false &&b.HasRepaired==true)
                                                  || (b.IsToFact==true && b.IsBack ==true )
                                                  select b;
                              
                                break;

                            case "IsToFact":
                                Model.ReportSqlParams_Bool istofact = (Model.ReportSqlParams_Bool)item;

                                if (istofact.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsToFact == istofact.ParamValues
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.IsToFact == null || b.IsToFact == false
                                                  select b;
                                }

                                break;
                            case "SysDate":
                                Model.ReportSqlParams_DataTime mm = (Model.ReportSqlParams_DataTime)item;

                                aduit_query = from b in aduit_query
                                              where b.SysDate >= mm.ParamValues
                                              select b;

                                break;
                            case "OldID":
                                Model.ReportSqlParams_String oldid = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.OldID.Contains(oldid.ParamValues)
                                              select b;
                                break;
                            case "HallID":
                                Model.ReportSqlParams_String pass1 = (Model.ReportSqlParams_String)item;


                                aduit_query = from b in aduit_query
                                              where b.HallID == pass1.ParamValues
                                              select b;

                                break;

                            case "Cus_Name":
                                Model.ReportSqlParams_String mm2 = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.Cus_Name.Contains(mm2.ParamValues)
                                              select b;
                                break;

                            case "Cus_Phone":
                                Model.ReportSqlParams_String pass2 = (Model.ReportSqlParams_String)item;


                                aduit_query = from b in aduit_query
                                              where b.Cus_Phone.Contains(pass2.ParamValues)
                                              select b;

                                break;

                            case "Pro_IMEI":
                                Model.ReportSqlParams_String imei = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.Pro_IMEI == imei.ParamValues
                                              select b;
                                break;

                            case "VIP_IMEI": //会员卡号
                                Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.IMEI == mm3.ParamValues
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
                                        aduit_query = from b in aduit_query
                                                      where b.SysDate >= mm5.ParamValues &&
                                                      b.SysDate < DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.SysDate >= mm5.ParamValues
                                                      && b.SysDate <= DateTime.Parse(mm6.ParamValues.ToString()).AddDays(1)
                                                      select b;
                                    }
                                }
                                break;

                            case "EndTime":
                                Model.ReportSqlParams_DataTime ed = (Model.ReportSqlParams_DataTime)item;
                                if (ed.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.SysDate < DateTime.Parse(ed.ParamValues.ToString()).AddDays(1)
                                                  select b;
                                }
                                break;
                        }
                    }

                    #endregion

                    #region 过滤仓库

                    if (ValidHallIDS.Count() > 0)
                    {
                        aduit_query = from b in aduit_query
                                      where ValidHallIDS.Contains(b.HallID)
                                      orderby b.SysDate descending
                                      select b;
                    }
                    else
                    {
                        aduit_query = from b in aduit_query
                                      orderby b.SysDate descending
                                      select b;
                    }
                    #endregion
                    pageParam.RecordCount = aduit_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in aduit_query.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPRepairInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPRepairInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion

                    return new Model.WebReturn() { ReturnValue = true };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Message = ex.Message, ReturnValue = false };
                }
            }

        }

        /// <summary>
        /// 326
        /// </summary>
        /// <param name="user"></param>
        /// <param name="repairid"></param>
        /// <returns></returns>
        public Model.WebReturn GetDetail(Model.Sys_UserInfo user, int repairid)  
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //受理故障
                    var errinfo = from a in lqh.Umsdb.ASP_CurrentOrder_ErrorInfo
                                  join b in lqh.Umsdb.ASP_ErrorInfo on a.ErrorID equals b.ID
                                  join c in lqh.Umsdb.ASP_CurrentOrderInfo on a.OrderID equals c.ID
                                  where c.CurrentRepairID == repairid
                                  select b;


                    //维修故障
                    var list = from a in lqh.Umsdb.ASP_CurrentOrder_ErrorInfo
                               join b in lqh.Umsdb.ASP_ErrorInfo on a.ErrorID equals b.ID
                               where a.RepairID == repairid
                               select b;
                    //配件
                    var pjinfo = from a in lqh.Umsdb.View_ASPCurrentOrderPros
                                 join b in lqh.Umsdb.ASP_RepairInfo
                                 on a.RepairID equals b.ID
                                 where a.RepairID == repairid 
                                 select a;
                    //备机
                    List<Model.View_BJModels> models = new List<View_BJModels>();

                    var rep = from b in lqh.Umsdb.ASP_RepairInfo
                              where b.ID == repairid
                              select b;
                    if (rep.Count() > 0)
                    {
                        Model.ASP_RepairInfo repair = rep.First();
                        var bjinfo = from a in lqh.Umsdb.View_BJModels
                                        where a.RepairID  == repairid
                                        select a;
                        if (bjinfo.Count() != 0)
                        {
                            models.AddRange(bjinfo.ToList());
                        }
                        
                    }
                    List<Model.View_ASPCurrentOrderPros> list2 = pjinfo.Count() == 0 ? new List<Model.View_ASPCurrentOrderPros>() : pjinfo.ToList();
                    return new WebReturn() { ReturnValue = true, Obj = list.ToList(), ArrList = new System.Collections.ArrayList() { list2,models,errinfo.ToList() } };
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue = false, Message = ex.Message };
                }

            }
        }

   
        /// <summary>
        /// 送厂  327
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Model.WebReturn AddToFact(Model.Sys_UserInfo user,List<Model.ASP_RepairInfo> list)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var ids = from a in list
                                  select a.ID;
                        var repinfo = from a in lqh.Umsdb.ASP_RepairInfo
                                      where ids.Contains(a.ID)
                                      select a;
                        if (repinfo.Count() != list.Count)
                        {
                            return new WebReturn() { ReturnValue  =false,Message="数据有误，部分维修单不存在！"};
                        }

                        foreach (var item in list)
                        {
                            foreach (var rm in repinfo)
                            {
                                if (rm.ID == item.ID)
                                {
                                    rm.ToDate = DateTime.Now;
                                    rm.ToUserID = user.UserID;
                                    rm.FacName = item.FacName;
                                    rm.FacInListID = item.FacInListID;
                                    rm.RpState = (int)Common.RepairState.WFBack;

                                    var corder = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                                 where a.ID == rm.OrderID
                                                 select a;
                                    if (corder.Count() == 0)
                                    {
                                        return new WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，保存失败！" };
                                    }
                                  
                                    Model.ASP_CurrentOrderInfo order = corder.First();
                                    if (rm.IsToFact == true)
                                    {
                                        return new WebReturn() { ReturnValue=false,Message="送修单 "+order.ServiceID+" 已送厂！"};
                                    }
                                    rm.IsToFact = true;
                                    order.IsToFact = true;
                                    order.ToDate = DateTime.Now;
                                    order.ToUserID = user.UserID;
                                    order.FacName = item.FacName;
                                    order.FacInListID = item.FacInListID;
                                    order.RpState = (int)Common.RepairState.WFBack;
                                    break;
                                }
                            }
                        }
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue = true,Message="送厂成功！"};   
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue =false,Message = ex.Message};   
                    }
                }
            }
        }

        /// <summary>
        /// 返厂  328     返厂后直接进入质检
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Model.WebReturn BackFromFact(Model.Sys_UserInfo user, List<Model.ASP_RepairInfo> list)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var ids = from a in list
                                  select a.ID;
                        var repinfo = from a in lqh.Umsdb.ASP_RepairInfo
                                      where ids.Contains(a.ID)
                                      select a;
                        if (repinfo.Count() != list.Count)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "数据有误，部分维修单不存在！" };
                        }

                        foreach (var item in list)
                        {
                            foreach (var rm in repinfo)
                            {
                                if (rm.ID == item.ID)
                                {
                                  
                                    rm.HasRepaired = true;
                                  
                                    rm.BackDate = DateTime.Now;
                                    rm.BackUserID = user.UserID;
                                    rm.BackInListID = item.BackInListID;
                                    rm.NewIMEI = item.NewIMEI;
                                    rm.NewSN = item.NewSN;
                                    rm.RpState = (int)Common.RepairState.WFZhiJian;

                                    var corder = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                                 where a.ID == rm.OrderID
                                                 select a;
                                    if (corder.Count() == 0)
                                    {
                                        return new WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，保存失败！" };
                                    }
                                  
                                    Model.ASP_CurrentOrderInfo order = corder.First();
                                    if (rm.IsToFact != true)
                                    {
                                        return new WebReturn() { ReturnValue = false, Message = "维修单 " + order.ServiceID + " 未送厂！" };
                                    }
                                    if (rm.IsBack == true)
                                    {
                                        return new WebReturn() { ReturnValue=false,Message="维修单 "+order.ServiceID+" 已返厂！"};
                                    }
                                    rm.IsBack = true;
                                    order.HasRepaired = true;
                                    order.IsBack = true;
                                    order.BackDate = DateTime.Now;
                                    order.BackUserID = user.UserID;
                                    order.BackInListID = item.BackInListID;
                                    order.NewIMEI = item.NewIMEI;
                                    order.NewSN = item.NewSN;
                                    order.RpState = (int)Common.RepairState.WFZhiJian;

                                    break;
                                }
                            }
                        }
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue = true,Message="返厂成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 质检 329
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Model.WebReturn ZhiJian(Model.Sys_UserInfo user, List<Model.View_ASPRepairInfo> list)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var rinfo = from a in lqh.Umsdb.ASP_RepairInfo
                                    where list.Select(p => p.ID).ToList().Contains(a.ID)
                                    select a;
                        if(rinfo.Count()==0)
                        {
                            return new WebReturn(){ ReturnValue=false,Message="数据有误，部分维修单不存在！"};
                        }
                        List<Model.ASP_ZJInfo> zjlist = new List<Model.ASP_ZJInfo>();

                        foreach (var item in list)
                        {
                            foreach (var child in rinfo)
                            {
                                if (item.ID == child.ID)
                                {
                                    child.ZJDate = DateTime.Now;
                                    child.ZJNote = item.ZJNote;
                                    child.ZJPassed = item.ZJPassed;
                                    child.ZJUserID = user.UserID;

                                    if (item.ZJPassed == true)
                                    {
                                        child.RpState = (int)Common.RepairState.WFShenHe;
                                    }
                                    else
                                    {
                                        child.Repairer = item.Repairer;
                                        child.RpState = (int)Common.RepairState.WFRepaire;
                                        child.HasRepaired = false;
                                    }
                                    var corder = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                                 where a.ID == child.OrderID
                                                 select a;
                                    if (corder.Count() == 0)
                                    {
                                        return new WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，审批失败！" };
                                    }

                                    Model.ASP_CurrentOrderInfo order = corder.First();
                                    order.ZJDate = DateTime.Now;
                                    order.ZJNote = item.ZJNote;
                                    order.ZJPassed = item.ZJPassed;
                                    order.ZJUserID = user.UserID;
                                    if (item.ZJPassed == true)
                                    {
                                        order.RpState = (int)Common.RepairState.WFShenHe;
                                    }
                                    else
                                    {
                                        order.Repairer = item.Repairer;
                                        order.RpState = (int)Common.RepairState.WFRepaire;
                                        order.HasRepaired = false;
                                    }
                                   

                                    Model.ASP_ZJInfo ZJInfo = new Model.ASP_ZJInfo();
                                    ZJInfo.IsAudit = item.ZJPassed;
                                    ZJInfo.Note = item.ZJNote;
                                    ZJInfo.RepairID = child.ID;
                                    ZJInfo.OrderID = order.ID;
                                    ZJInfo.RepairorID = child.Repairer;
                                    ZJInfo.SysDate = DateTime.Now;
                                    ZJInfo.UserID = user.UserID;
                                    if (item.ZJPassed == false)
                                    {
                                        ZJInfo.Finished = true;
                                    }
                                    zjlist.Add(ZJInfo);

                                    #region  若质检不通过则将备机、配件加回库存 清除串码标记

                                    if (item.ZJPassed == false)
                                    {
                                        order.HasRepaired = null;
                                        order.NeedToFact = null;
                                        order.IsBack = null;
                                        order.BackDate = null;
                                        order.BackNote = null;
                                        order.BackUserID = null;
                                        order.ToDate = null;
                                        order.ToUserID = null;
                                        order.IsToFact = null;
                                        order.ZJNote = item.ZJNote;

                                        child.RpState = null;
                                        order.RpState = (int)Common.RepairState.WFRepaire;

                                        #region 返还备机

                                        var bak = from a in lqh.Umsdb.ASP_CurrentOrder_BackupPhoneInfo
                                                   where a.RepairID == child.ID
                                                   select a;
                                        if (bak.Count() > 0)
                                        {
                                            foreach (var childx in bak)
                                            {
                                                if (!string.IsNullOrEmpty(childx.IMEI))
                                                {
                                                    var imei = from a in lqh.Umsdb.Pro_IMEI
                                                               where a.IMEI.ToUpper() == childx.IMEI.ToUpper()
                                                               select a;
                                                    if (imei.Count() == 0)
                                                    {
                                                        return new WebReturn() { ReturnValue = false, Message = "串码不存在：" + childx.IMEI + " ,审批失败！" };
                                                    }
                                                    imei.First().State = 0;
                                                    imei.First().BJID = null;
                                                }
                                                var store = from a in lqh.Umsdb.Pro_StoreInfo
                                                            where a.ProID == childx.ProID && a.InListID == childx.InListID
                                                            && a.HallID == child.HallID
                                                            select a;
                                                if (store.Count() == 0)
                                                {
                                                    return new WebReturn() { ReturnValue = false, Message = "库存不足！" };
                                                }
                                                store.First().ProCount += 1;
                                            }
                                        }
                                        lqh.Umsdb.ASP_CurrentOrder_BackupPhoneInfo.DeleteAllOnSubmit(bak);

                                        #endregion 

                                        #region 返还配件

                                        var pros = from a in lqh.Umsdb.ASP_CurrentOrder_Pros
                                                   where a.RepairID == child.ID
                                                   select a;
                                        if (pros.Count() > 0)
                                        {
                                            //return new WebReturn() { ReturnValue = false, Message = "未能找到指定配件数据，审批失败！" };

                                            foreach (var xx in pros)
                                            {
                                                if (!string.IsNullOrEmpty(xx.IMEI))
                                                {
                                                    var imei = from a in lqh.Umsdb.Pro_IMEI
                                                               where a.IMEI.ToUpper() == xx.IMEI.ToUpper()
                                                               select a;
                                                    if (imei.Count() == 0)
                                                    {
                                                        return new WebReturn() { ReturnValue = false, Message = "串码不存在：" + xx.IMEI + " ,审批失败！" };
                                                    }
                                                    imei.First().State = 0;
                                                    imei.First().PJID = null;
                                                }
                                                var store = from a in lqh.Umsdb.Pro_StoreInfo
                                                            where a.ProID == xx.ProID && a.InListID == xx.InListID
                                                            && a.HallID == child.HallID
                                                            select a;
                                                if (store.Count() == 0)
                                                {
                                                    return new WebReturn() { ReturnValue = false, Message = "库存不足！" };
                                                }
                                                store.First().ProCount += 1;
                                            }
                                        }

                                        #endregion 
                                    }
                                    #endregion 

                                    break;
                                }
                            }
                        }
                        lqh.Umsdb.ASP_ZJInfo.InsertAllOnSubmit(zjlist);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new WebReturn() { ReturnValue = true ,Message ="保存成功！"};
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue=false,Message=ex.Message};   
                    }
                }
            }
        }

        /// <summary>
        /// 审核 333
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Model.WebReturn ShenHe(Model.Sys_UserInfo user, List<Model.View_ASPZJInfo> list)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var rinfo = from a in lqh.Umsdb.ASP_RepairInfo
                                    //join b in lqh.Umsdb.ASP_CurrentOrderInfo
                                    //on a.OrderID equals b.ID
                                    where list.Select(p => p.ID).ToList().Contains(a.ID)
                                    select a;
                        if (rinfo.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "数据有误，部分维修单不存在！" };
                        }

                        List<Model.ASP_ManaAuditinfo> mlist = new List<Model.ASP_ManaAuditinfo>();
                       // bool hasPassed = false;
                       // string msg = "";
                        foreach (var item in list)
                        {
                            foreach (var child in rinfo)
                            {
                              
                                if (item.ID == child.ID)
                                {
                                    if (child.IsPassed == true)
                                    {
                                        //hasPassed = true;
                                       // msg += item.ServiceID+"  ";
                                        return new WebReturn() { ReturnValue = false, Message = "受理单：" + item.ServiceID + " 已审核通过！" };
                                    }
                                    child.ChkDate = DateTime.Now;
                                    child.ChkNote = item.ChkNote;
                                    child.IsPassed = item.IsPassed;

                                    child.ChkUserID = user.UserID;

                                    if (item.IsPassed==true)
                                    {
                                        child.RpState = (int)Common.RepairState.WFGetMobile;
                                    }
                                    else
                                    {
                                        child.ZJPassed = false;
                                        child.IsPassed = null;
                                        child.RpState = (int)Common.RepairState.WFZhiJian;
                                    }

                                    var corder = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                                 where a.ID == child.OrderID
                                                 select a;
                                    if (corder.Count() == 0)
                                    {
                                        return new WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，保存失败！" };
                                    }

                                    Model.ASP_CurrentOrderInfo order = corder.First();
                                    order.ChkDate = DateTime.Now;
                                    order.ChkNote = item.ChkNote;
                                    order.IsPassed = item.IsPassed;
                                    order.ChkUserID = user.UserID;
                                    if (item.IsPassed == true)
                                    {
                                        order.RpState = (int)Common.RepairState.WFGetMobile;
                                    }
                                    else
                                    {
                                        order.ZJPassed = false;
                                        order.IsPassed = null;
                                        order.RpState = (int)Common.RepairState.WFZhiJian;
                                    }

                                    Model.ASP_ManaAuditinfo mana = new Model.ASP_ManaAuditinfo();
                                    mana.IsAudit = item.IsPassed;
                                    mana.Note = item.ChkNote;
                                    mana.OrderID = order.ID;
                                    mana.SysDate = DateTime.Now;
                                    mana.UserID = user.UserID;
                                    mana.RepaireID = child.ID;
                                    mana.ZJID = item.ZJID;
                                    if (item.IsPassed == false)
                                    {
                                        mana.Finished = true;
                                        var zj = from a in lqh.Umsdb.ASP_ZJInfo
                                                 where a.ID == item.ZJID
                                                 select a;
                                        if (zj.Count() > 0)
                                        {
                                            zj.First().Finished = true;
                                        }
                                        lqh.Umsdb.SubmitChanges();
                                    }
                                    mlist.Add(mana);
                                    break;
                                }
                            }
                        }
                        //if (hasPassed)
                        //{
                        //    return new WebReturn() { ReturnValue = false, Message = "受理单：" + msg + " 已审核通过！" };
                        //}
                        lqh.Umsdb.ASP_ManaAuditinfo.InsertAllOnSubmit(mlist);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new WebReturn() { ReturnValue = true, Message = "保存成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue=false,Message = ex.Message};   
                    }
                }
            }

        }

        /// <summary>
        /// 取机  334
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, Model.View_ASPSHInfo model,
            List<Model.View_BJModels> bjinfo, Model.ASP_FethchInfo fetch,bool passed,Model.VIP_VIPInfo vip)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var rep = from a in lqh.Umsdb.ASP_RepairInfo
                                  where a.ID == model.ID 
                                  select a;
                        if (rep.Count() == 0)
                        {
                            return new WebReturn(){ ReturnValue =false,Message="单号不存在，保存失败！"};
                        }
                        Model.ASP_RepairInfo m = rep.First();

                        if (m.HasFetch == true)
                        {
                            return new WebReturn() { ReturnValue=false,Message="客户已取机！"};
                        }
                        
                        m.FetchNote = model.FetchNote;
                        m.FetchUserID = user.UserID;
                        m.FetchDate = DateTime.Now;
                        m.FetchNeedAudit = model.FetchNeedAudit;
                        m.RealPay = fetch.RealPay;
                        m.ShouldPay = fetch.ShouldPay;
                       
                        m.IsPassed = passed;
                        if (passed && model.FetchNeedAudit==false)
                        {
                            m.HasFetch = true;
                            m.RpState = (int)Common.RepairState.WFAudit;
                        }
                        else if (passed && model.FetchNeedAudit == true)
                        {
                            m.HasFetch = true;
                            m.RpState = (int)Common.RepairState.WFBJAudit;
                        }
                        else
                        {
                            m.HasFetch = false;
                            m.RpState = (int)Common.RepairState.WFShenHe;
                        }
                     

                        #region CurrentOrder

                        var corder = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                     where a.ID == m.OrderID
                                     select a;
                        if (corder.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，保存失败！" };
                        }

                        Model.ASP_CurrentOrderInfo order = corder.First();
                        order.FetchNote = model.FetchNote;
                        order.FetchUserID = user.UserID;
                        order.FetchDate = DateTime.Now;
                        order.FetchNeedAudit = model.FetchNeedAudit;
                       
                        order.IsPassed = passed;
                        order.ProMoney = m.ProMoney;
                        order.WorkMoney = m.WorkMoney;
                        order.ShouldPay = m.ShouldPay;
                        order.RealPay = m.RealPay;
                        order.GzMoney = m.GzMoney;
                        order.GzType = m.GzType;
                        if (passed && model.FetchNeedAudit == false)
                        {
                            order.HasFetch = true;
                            order.RpState = (int)Common.RepairState.WFAudit;
                        }
                        else if (passed && model.FetchNeedAudit == true)
                        {
                            order.HasFetch = true;
                            order.RpState = (int)Common.RepairState.WFBJAudit;
                        }
                        else
                        {
                            order.HasFetch = false;
                            order.RpState = (int)Common.RepairState.WFShenHe;
                        }
                        #endregion

                        #region 标记返还备机串码
                        if (passed)
                        {
                            var list = (from b in bjinfo
                                        select b.ID).ToList();

                            var bjlist = from a in lqh.Umsdb.ASP_CurrentOrder_BackupPhoneInfo
                                         where list.Contains(a.ID)
                                         select a;
                            foreach (var item in bjlist)
                            {
                                foreach (var child in bjinfo)
                                {
                                    if (string.IsNullOrEmpty(child.NewIMEI)) { break; }
                                    if (item.ID == child.ID)
                                    {
                                        item.NewIMEI = child.NewIMEI;
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion

                        fetch.SysDate = DateTime.Now;
                        Model.ASP_GetPhoneInfo gp = new Model.ASP_GetPhoneInfo();
                        gp.IsAudit = passed;
                        gp.ManaAID = model.SHID;
                        gp.Note = model.FetchNote;
                        gp.OrderID = order.ID;
                        gp.SysDate = DateTime.Now;
                        gp.UserID = user.UserID;
                        gp.CardID = vip.IMEI;
                        gp.VIPTypeID = vip.TypeID;
                        if (passed == false)
                        {
                            gp.Finished = true;
                            //var aa = from a in lqh.Umsdb.ASP_ManaAuditinfo
                            //         where a.ID == model.SHID
                            //         select a;
                            //if (aa.Count() > 0)
                            //{
                            //    aa.First().Finished = true;
                            //}
                            //lqh.Umsdb.SubmitChanges();
                        }

                        lqh.Umsdb.ASP_GetPhoneInfo.InsertOnSubmit(gp);
                        lqh.Umsdb.ASP_FethchInfo.InsertOnSubmit(fetch);
                        lqh.Umsdb.SubmitChanges();

                        #region 若备机返库不需要审批则直接加回库存

                        if (m.FetchNeedAudit == false && passed)
                        {
                            foreach (var item in bjinfo)
                            {
                                #region 加库存
                                var store = from a in lqh.Umsdb.Pro_StoreInfo
                                            where a.ProID == item.ProID && a.HallID == m.BJHallID && a.InListID == item.InListID
                                            select a;
                                if (store.Count() > 0)
                                {
                                    store.First().ProCount += 1;
                                }
                                else
                                {
                                    Model.Pro_StoreInfo st = new Pro_StoreInfo();
                                    st.ProID = item.ProID;
                                    st.ProCount = 1;
                                    st.HallID = m.BJHallID;

                                    lqh.Umsdb.Pro_StoreInfo.InsertOnSubmit(st);
                                }
                                lqh.Umsdb.SubmitChanges();
                                #endregion

                                #region 恢复串码标记
                                if (item.NeedIMEI == false) { continue; }

                                var imei = from a in lqh.Umsdb.Pro_IMEI
                                           where a.IMEI.ToUpper() == item.IMEI.ToUpper()
                                           select a;
                                if (imei.Count() == 0)
                                {
                                    return new WebReturn() { ReturnValue = false, Message = "串码不存在：" + item.IMEI.ToUpper() };
                                }
                                imei.First().BJID = null;
                                imei.First().State = 0;
                                //有新串码
                                //if (!string.IsNullOrEmpty(item.NewIMEI))
                                //{
                                //    Model.Pro_IMEI_Deleted m2 = new Pro_IMEI_Deleted();
                                //    m2.IMEI = imei.First().IMEI;

                                //    Model.Pro_IMEI m1 = new Pro_IMEI();
                                //    m1.ProID = item.ProID;
                                //    m1.InListID = item.InListID;
                                //    m1.IMEI = item.NewIMEI;
                                //    m1.HallID = m.BJHallID;

                                //    lqh.Umsdb.Pro_IMEI_Deleted.InsertOnSubmit(m2);
                                //    lqh.Umsdb.Pro_IMEI.DeleteOnSubmit(imei.First());
                                //    lqh.Umsdb.Pro_IMEI.InsertOnSubmit(m1);
                                //}

                                #endregion
                            }
                        }
                       
                        #endregion 

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();


                        return new WebReturn() { ReturnValue = true, Message = "保存成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue =false,Message = ex.Message};   
                    }
                }
            }

        }

        /// <summary>
        /// 审计 335
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AfterSaleAudit(Model.Sys_UserInfo user, Model.View_ASPRepairInfo model,Model.ASP_AduitInfo audit) 
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var list = from a in lqh.Umsdb.ASP_RepairInfo
                                   where a.ID == model.ID
                                   select a;
                        if (list.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue=false,Message="受理单不存在，保存失败！"};
                        }

                        Model.ASP_RepairInfo m = list.First();
                        m.AuditDate = DateTime.Now;
                        m.HasAudited = true;
                        m.AuditLowMoney = model.AuditLowMoney;
                        m.AuditMoney = model.AuditMoney;
                        m.AuditNote = model.AuditNote;
                        m.AuditUserID = user.UserID;
                        m.RpState = (int)Common.RepairState.Finished;
                        var corder = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                     where a.ID == m.OrderID
                                     select a;
                        if (corder.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，保存失败！" };
                        }

                        Model.ASP_CurrentOrderInfo order = corder.First();
                        order.AuditDate = DateTime.Now;
                        order.HasAudited = true;
                        order.AuditLowMoney = model.AuditLowMoney;
                        order.AuditMoney = model.AuditMoney;
                        order.AuditNote = model.AuditNote;
                        order.AuditUserID = user.UserID;
                        order.CurrentRepairID = null;
                        order.RpState = (int)Common.RepairState.Finished;

                        audit.SySDate = DateTime.Now;
                        audit.UserID = user.UserID;
                      
                        lqh.Umsdb.ASP_AduitInfo.InsertOnSubmit(audit);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new WebReturn() { ReturnValue = true, Message = "保存成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue=false,Message=ex.Message};   
                    }
                }
            }
        }

        /// <summary>
        /// 回访  336
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn CallBack(Model.Sys_UserInfo user, int repairid , Model.ASP_CallBackInfo cb)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var list = from a in lqh.Umsdb.ASP_RepairInfo
                                   where a.ID == repairid
                                   select a;
                        if (list.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "受理单不存在，保存失败！" };
                        }

                        Model.ASP_RepairInfo m = list.First();
                        m.HasCallBack = true;
                        m.RpState = (int)Common.RepairState.WFAudit;

                        var corder = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                     where a.ID == m.OrderID
                                     select a;
                        if (corder.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，保存失败！" };
                        }

                        Model.ASP_CurrentOrderInfo order = corder.First();
                        order.HasCallBack = true;
                        order.RpState = (int)Common.RepairState.WFAudit;

                        cb.SysDate = DateTime.Now;
                        cb.UserID = user.UserID;

                        lqh.Umsdb.ASP_CallBackInfo.InsertOnSubmit(cb);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new WebReturn() { ReturnValue = true, Message = "保存成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 获取备机明细 337 备机返库审批
        /// </summary>
        /// <param name="user"></param>
        /// <param name="repairID"></param>
        /// <returns></returns>
        public Model.WebReturn GetBJDetail(Model.Sys_UserInfo user, int repairID)
        {
            using (LinQSqlHelper lqh     = new LinQSqlHelper())
            {
                try
                {
                    var list = from a in lqh.Umsdb.View_BJModels
                               where a.RepairID == repairID
                               select a;


                    return new WebReturn() { ReturnValue = true,Obj= list.ToList(), Message = "" };   
                }
                catch (Exception ex)
                {
                    return new WebReturn() {ReturnValue =false,Message=ex.Message };   
                }
            }
        }


        /// <summary>
        /// 归还备机审批    338
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn BJReturnAudit(Model.Sys_UserInfo user, Model.View_ASPGetPhoneInfo model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var list = from a in lqh.Umsdb.ASP_RepairInfo
                                   where a.ID == model.ID
                                   select a;
                        if (list.Count() > 0)
                        {
                            Model.ASP_RepairInfo repInfo = list.First();
                            if (repInfo.IsFetchAduit == true)
                            {
                                return new WebReturn() { ReturnValue=false,Message="该审批单已审批！"};
                            }

                            repInfo.IsFetchAduit = true;
                            repInfo.FetchAuditPassed = model.FetchAuditPassed;
                            repInfo.FetchAduitDate = DateTime.Now;
                            repInfo.FetchAduitUserID = user.UserID;
                            repInfo.FetchAuditNote = model.FetchAuditNote;

                            #region CurerentOrder

                            var corder = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                         where a.ID == repInfo.OrderID
                                         select a;
                            if (corder.Count() == 0)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，保存失败！" };
                            }

                            Model.ASP_CurrentOrderInfo order = corder.First();
                            order.IsFetchAduit = true;
                            order.FetchAuditPassed = model.FetchAuditPassed;
                            order.FetchAduitDate = DateTime.Now;
                            order.FetchAduitUserID = user.UserID;
                            order.FetchAuditNote = model.FetchAuditNote;

                            #endregion 

                            Model.ASP_BackPhoneAuditinfo audit = new ASP_BackPhoneAuditinfo();
                            audit.GetID = model.GPID;
                            audit.IsAudit = model.FetchAuditPassed;
                            audit.Note = model.AuditNote;
                            audit.OrderID = order.ID;
                            audit.SysDate = DateTime.Now;
                            audit.UserID = user.UserID;
                            if (model.FetchAuditPassed == false)
                            {
                                audit.Finished = true;
                                var aa = from a in lqh.Umsdb.ASP_GetPhoneInfo
                                         where a.ID == model.GPID
                                         select a;
                                if (aa.Count() > 0)
                                {
                                    aa.First().Finished = true;
                                }
                                repInfo.HasFetch = false;
                                lqh.Umsdb.SubmitChanges();
                            }
                            lqh.Umsdb.ASP_BackPhoneAuditinfo.InsertOnSubmit(audit);
                            lqh.Umsdb.SubmitChanges();

                            //归还备机审批通过则加回库存 清空串码标记
                            if (repInfo.FetchAuditPassed == true)
                            {
                                repInfo.IsFetchAduit = true;
                                repInfo.FetchAuditPassed = true;

                               // List<Model.ASP_CurrentOrder_BackupPhoneInfo> bjinfo = new List<ASP_CurrentOrder_BackupPhoneInfo>();
                                string hallid = repInfo.BJHallID;
                                var pros = from a in lqh.Umsdb.ASP_CurrentOrder_BackupPhoneInfo
                                           where a.RepairID == repInfo.ID
                                           select a;
                              
                                if (pros.Count() > 0)
                                {

                                    foreach (var item in pros)
                                    {
                                        if (string.IsNullOrEmpty(item.NewIMEI))
                                        {
                                            continue;
                                        }
                                        var imei = from a in lqh.Umsdb.Pro_IMEI
                                                   where a.ProID == item.ProID && a.IMEI.ToUpper() == item.IMEI.ToUpper()
                                                   && a.HallID == hallid
                                                   select a;

                                        #region  验证串码
                                        if (imei.Count() == 0) {
                                            return new WebReturn() { ReturnValue=false,Message="串码: "+item.IMEI+" 不存在！"};
                                        }
                                        Model.Pro_IMEI m = imei.First();
                                        if (m == null)
                                        {
                                            return new WebReturn() { Message = "串码不存在: " + item.IMEI + " ！", ReturnValue = false };

                                        }
                                        else if (m.Pro_ProInfo == null)
                                        {
                                            return new WebReturn() { Message = "串码：" + item.IMEI + " 的商品不存在！", ReturnValue = false };

                                        }
                                        else if (!m.Pro_ProInfo.NeedIMEI == true)
                                        {
                                            return new WebReturn() { Message = "串码：" + item.IMEI + " 属于无串码商品！", ReturnValue = false };

                                        }


                                        m.BJID = null;
                                        m.State = null;
                                        #endregion

                                        #region 新增串码  删除旧串码

                                        Model.Pro_IMEI im = new Pro_IMEI();
                                        im.IMEI = item.NewIMEI;
                                        im.ProID = item.ProID;
                                        im.HallID = hallid;
                                        //string inlistid = "";
                                        //lqh.Umsdb.OrderMacker(1, "RKL", "RKL", ref inlistid);
                                        //if (string.IsNullOrEmpty(inlistid))
                                        //{
                                        //    return new WebReturn() { ReturnValue = false, Message = "生成明细单号出错" };
                                        //}
                                        im.InListID = item.InListID;
                                        lqh.Umsdb.Pro_IMEI.InsertOnSubmit(im);
                                        lqh.Umsdb.SubmitChanges();
                                        // imei.First().NEW_IMEI_ID = im.ID;

                                        Model.Pro_IMEI_Deleted del = new Pro_IMEI_Deleted();
                                        del.IMEI = imei.First().IMEI;
                                        lqh.Umsdb.Pro_IMEI_Deleted.InsertOnSubmit(del);

                                        //删除旧串码
                                        lqh.Umsdb.Pro_IMEI.DeleteOnSubmit(m);
                                        #endregion

                                        #region  更新库存

                                        var store = from a in lqh.Umsdb.Pro_StoreInfo
                                                    where a.HallID == hallid && a.ProID == item.ProID && a.InListID == item.InListID
                                                    select a;
                                        if (store.Count() == 0)
                                        {
                                            Model.Pro_StoreInfo s = new Pro_StoreInfo();
                                            s.InListID = item.InListID;
                                            s.ProID = item.ProID;
                                            s.HallID = hallid;
                                            s.ProCount = 1;
                                            lqh.Umsdb.Pro_StoreInfo.InsertOnSubmit(s);
                                        }
                                        else
                                        {
                                            store.First().ProCount += 1;
                                        }
                                        lqh.Umsdb.SubmitChanges();

                                        #endregion

                                    }
                                }
                            }
                            else  //不通過則返回取機處
                            {
                                repInfo.HasFetch = null;
                                repInfo.IsFetchAduit = null;
                                repInfo.FetchAuditPassed = null;

                            }

                        }
                        else
                        {
                            return new WebReturn() { Message = "未能找到售后维修单，审批失败！", ReturnValue = false };   
                        }
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { Message = "审批成功！", ReturnValue = true }; 
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { Message=ex.Message,ReturnValue=false};   
                    }
                }
            }
        }

    }
}
