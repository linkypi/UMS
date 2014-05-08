using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class ASP_ReceiveInfo
    {
        private int MethodID;

        public ASP_ReceiveInfo()
        {
            this.MethodID = 0;
        }

        public ASP_ReceiveInfo(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        ///指派维修师  380
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rid"></param>
        /// <param name="reper"></param>
        /// <returns></returns>
        public WebReturn Dispatch(Model.Sys_UserInfo user, Model.View_ASPReceiveInfo asprec,List<Model.ASP_ErrorInfo> errs)
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
                        //    if (!ValidHallIDS.Contains(user.Sys_RoleInfo))
                        //    {
                        //        var que = from h in lqh.Umsdb.Pro_HallInfo
                        //                  where h.HallID == recinfo.HallID
                        //                  select h;
                        //        return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                        //    }
                        //}
                        #endregion

                      
                        #endregion 

                        var recinfo = from a in lqh.Umsdb.ASP_ReceiveInfo
                                      where a.ID == asprec.ID
                                      select a;

                        if (recinfo.Count()== 0)
                        {
                            return new WebReturn() { ReturnValue=false,Message="未能找到指定受理单！"};
                        }

                        Model.ASP_ReceiveInfo rec = recinfo.First();
                        
                        Model.ASP_CurrentOrderInfo cur = rec.ASP_CurrentOrderInfo.First();
                        if (cur.HasRepaired == true && cur.Repairer != asprec.Repairer)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该单已维修，不可再次指派维修师！" };
                        }
                        #region  
                        cur.SaveTime = DateTime.Now;
                        cur.SaveUser = user.UserID;
                        cur.OldRepairer = cur.Repairer;
                        cur.Repairer = asprec.Repairer;
                        cur.Pro_Name = asprec.Pro_Name;
                        cur.OldID = asprec.OldID;
                        cur.Pro_Note = asprec.Pro_Note;
                        cur.Pro_Other = asprec.Pro_Other;
                        cur.Pro_OutSide = asprec.Pro_OutSide;
                        cur.Pro_Type = asprec.Pro_Type;
                        cur.Position = asprec.Position;
                        cur.Pro_Color = asprec.ProFormat;
                        cur.Pro_Type = asprec.Pro_Type;
                        //cur.RepairNote = asprec.re
                        cur.Cus_Add = asprec.Cus_Add;
                        cur.Cus_Email = asprec.Cus_Email;
                        cur.Cus_Name = asprec.Cus_Name;
                        cur.Cus_Phone = asprec.Cus_Phone;
                        cur.Cus_Phone2 = asprec.Cus_Phone2;
                        cur.Pro_BuyT = asprec.Pro_BuyT;
                        cur.Pro_Bill = asprec.Pro_Bill;

                        rec.Pro_BuyT = asprec.Pro_BuyT;
                        rec.Pro_Bill = asprec.Pro_Bill;
                        rec.Pro_Note = asprec.Pro_Note;
                        rec.HasDispatch = true;
                        rec.ProID = asprec.ProID;
                        rec.Pro_Name = asprec.Pro_Name;
                        rec.Pro_Color = asprec.ProFormat;
                        rec.Pro_Type = asprec.Pro_Type;
                        rec.OldID = asprec.OldID;
                        rec.Repairer = asprec.Repairer;
                        rec.DispatchDate = DateTime.Now;
                        rec.Dispatcher = user.UserID;
                        rec.Cus_Add = asprec.Cus_Add;
                        rec.Cus_Email = asprec.Cus_Email;
                        rec.Cus_Name = asprec.Cus_Name;
                        rec.Cus_Phone = asprec.Cus_Phone;
                        rec.Cus_Phone2 = asprec.Cus_Phone2;
                        #endregion 

                        #region  更新故障

                        if (errs.Count > 0)
                        {
                            var errinfo = from a in lqh.Umsdb.ASP_CurrentOrder_ErrorInfo
                                      where a.ReceiveID == asprec.ID
                                      select a;
                            if (errinfo.Count() > 0)
                            {
                                lqh.Umsdb.ASP_CurrentOrder_ErrorInfo.DeleteAllOnSubmit(errinfo);
                            }
                            List<ASP_CurrentOrder_ErrorInfo> list = new List<ASP_CurrentOrder_ErrorInfo>();
                            foreach (var item in errs)
                            {
                                ASP_CurrentOrder_ErrorInfo a = new ASP_CurrentOrder_ErrorInfo();
                                a.ReceiveID = asprec.ID;
                                a.ErrorID = item.ID;
                                list.Add(a);
                            }
                            lqh.Umsdb.ASP_CurrentOrder_ErrorInfo.InsertAllOnSubmit(list);
                        }
                        #endregion

                        if (cur.RpState == 0 && string.IsNullOrEmpty(asprec.Repairer) == false)
                        {
                            cur.RpState = (int)Common.RepairState.WFRepaire;
                        }

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue = true, Message = "保存成功！" };
                    }
                    catch (Exception x)
                    {
                        return new WebReturn() { ReturnValue = false, Message =x.Message  };
                    }
                   
                }
            }
        }

        /// <summary>
        /// 320
        /// </summary>
        /// <param name="user"></param>
        /// <param name="recinfo"></param>
        /// <param name="checkinfo"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.ASP_ReceiveInfo recinfo,bool addDealer,Model.ASP_Dealer dealer)
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
                        foreach (var item in recinfo.ASP_CurrentOrder_BackupPhoneInfo)
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
                        if (ValidHallIDS.Count > 0)
                        {
                            if (!ValidHallIDS.Contains(recinfo.HallID))
                            {
                                var que = from h in lqh.Umsdb.Pro_HallInfo
                                          where h.HallID == recinfo.HallID
                                          select h;
                                return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName };
                            }
                        }
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

                        if (recinfo.HasDispatch == true)
                        {
                            recinfo.DispatchDate = DateTime.Now;
                        
                            var reper = from a in lqh.Umsdb.Sys_UserInfo
                                        where a.UserID == recinfo.Repairer
                                        select a;

                            if (reper.Count()==0)
                            {
                                return new WebReturn() { ReturnValue=false,Message="指定维修师不存在，保存失败！"};
                            }
                        }

                        #region  验证主板是否正在维修中

                        //var aa = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                        //         where a.Pro_IMEI.ToUpper() == recinfo.Pro_IMEI.ToUpper() && (a.HasAudited == false || a.HasAudited == null)
                        //         select a;
                        var aa = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                 where a.Pro_HeaderIMEI.ToUpper() == recinfo.Pro_HeaderIMEI.ToUpper() && (a.HasAudited == false || a.HasAudited == null)
                                 select a;
                        if (aa.Count() > 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该主板维修中，保存失败！" };
                        }
                        #endregion 

                        #region  新增经销商

                        if (addDealer)
                        {
                            var deler = from a in lqh.Umsdb.ASP_Dealer
                                        where a.Dealer == dealer.Dealer
                                        select a;
                            if (deler.Count() > 0)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "该经销商名称已存在！" };
                            }
                            dealer.IsDelete = false;
                            lqh.Umsdb.ASP_Dealer.InsertOnSubmit(dealer);
                            lqh.Umsdb.SubmitChanges();
                            recinfo.DealerID = dealer.ID;
                        }

                        #endregion  

                        if (recinfo.ASP_CurrentOrder_BackupPhoneInfo.Count > 0)
                        {
                            recinfo.HasBJ = true;
                            recinfo.BJ_Date = DateTime.Now;
                        }
                       
                        recinfo.SysDate = DateTime.Now;
                        string msg = null;
                        lqh.Umsdb.OrderMacker(1, "RID", "RID", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "受理单号生成出错！" };
                        }
                        recinfo.ServiceID = msg;
                        recinfo.Flag = true;

                        //if (recinfo.ASP_CurrentOrder_BackupPhoneInfo.Count > 0)
                        //{
                        //    lqh.Umsdb.ASP_CurrentOrder_BackupPhoneInfo.InsertAllOnSubmit( recinfo.ASP_CurrentOrder_BackupPhoneInfo);
                        //}
                        List<int> eids = new List<int>();
                        foreach (var item in recinfo.ASP_CurrentOrder_ErrorInfo)
                        {
                            eids.Add(item.ErrorID);
                        }

                       
                        var errs = from a in lqh.Umsdb.ASP_ErrorInfo
                                   where eids.Contains(a.ID)
                                   select a;
                        if (errs.Count() > 0)
                        {
                            int index = 1;
                            foreach (var item in errs)
                            {
                                recinfo.ErrsID += item.ErrorID;
                                recinfo.Errors += item.ErrorName;
                                if (index < errs.Count())
                                {
                                    recinfo.Errors += "/";
                                    recinfo.ErrsID += " , ";
                                }
                                index++;
                            }
                        }
                        
                        lqh.Umsdb.ASP_ReceiveInfo.InsertOnSubmit(recinfo);
                        lqh.Umsdb.SubmitChanges();

                        #region  验证备机串码

                        //foreach (var item in recinfo.ASP_CurrentOrder_BackupPhoneInfo)
                        //{
                        //    if (string.IsNullOrEmpty(item.IMEI)) { continue; }
                        //    var list = from a in lqh.Umsdb.Pro_IMEI
                        //               where a.IMEI.ToUpper() == item.IMEI.ToUpper()
                        //               select a;
                        //    if (list.Count() == 0)
                        //    {
                        //        return new Model.WebReturn() { Message = "备机串码 " + item.IMEI + " 不存在！", ReturnValue = false };
                        //    }
                        //    list.First().BJID = recinfo.ID;
                        //    list.First().State = 1;
                        //}
                        foreach (var item in recinfo.ASP_CurrentOrder_BackupPhoneInfo)
                        {
                            #region 验证串码是否存在及其机型是否匹配

                            if (!string.IsNullOrEmpty(item.IMEI))
                            {
                                var list = from a in lqh.Umsdb.Pro_IMEI
                                           where a.IMEI.ToUpper() == item.IMEI.ToUpper()
                                           select a;
                                if (list.Count() == 0)
                                {
                                    return new Model.WebReturn() { Message = "备机串码 " + item.IMEI + " 不存在！", ReturnValue = false };
                                }
                                Model.Pro_IMEI imei = list.First();
                                //验证串码

                                Model.WebReturn ret1 = Common.Utils.CheckIMEI(imei);
                                if (ret1.ReturnValue == false)//缺料且缺料不保存则返回
                                {
                                    return ret1;
                                }
                                imei.BJID = item.ID;
                                imei.State = 1;
                            }
                            #endregion

                            #region  验证库存是否充足

                            var store = from a in lqh.Umsdb.Pro_StoreInfo
                                        where a.ProID == item.ProID && a.HallID == recinfo.BJ_HallID && a.InListID == item.InListID
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

                        #endregion 

                        #region CurrentOrder

                        Model.ASP_CurrentOrderInfo order = new Model.ASP_CurrentOrderInfo();
                        order.ReceiveID = recinfo.ID;
                        order.HasDispatch = recinfo.HasDispatch;
                        order.DispatchDate = recinfo.DispatchDate;
                        order.Repairer = recinfo.Repairer;
                        order.HasBJ = recinfo.HasBJ;
                        order.PredictDate = recinfo.PredictDate;
                        order.DealerID = recinfo.DealerID;
                        if (order.HasDispatch==true)
                        {
                            order.RpState = (int)Common.RepairState.WFRepaire; ;
                        }
                        else
                        {
                            order.RpState = (int)Common.RepairState.WFDispatch; ;
                        }
                        if (order.HasBJ==true)
                        {
                            order.BJ_Date = DateTime.Now;
                        }
                        order.BJ_Money = recinfo.BJ_Money;
                        order.BJ_UserID = recinfo.BJ_UserID;
                        order.BJ_HallID = recinfo.BJ_HallID;
                        order.Chk_FID = recinfo.Chk_FID;
                        order.Chk_InOut = recinfo.Chk_InOut;
                        order.Chk_Note = recinfo.Chk_Note;
                        order.Chk_Price = recinfo.Chk_price; // 客户限价
                        order.Cus_Add = recinfo.Cus_Add;
                        order.Cus_CPC = recinfo.Cus_CPC;
                        order.Cus_CusType = recinfo.Cus_CusType;
                        order.Cus_Email = recinfo.Cus_Email;
                        order.Cus_Name = recinfo.Cus_Name;
                        order.HallID = recinfo.HallID;
                        order.Cus_VIPID = recinfo.Cus_VIPID;
                        order.Cus_Phone = recinfo.Cus_Phone;
                        order.Cus_Phone2 = recinfo.Cus_Phone2;
                        order.OldID = recinfo.OldID;
                        order.RepairCount = recinfo.RepairCount;
                        order.SysDate = DateTime.Now;
                        
                        order.Receiver = recinfo.Receiver;
                        order.Sender = recinfo.Sender;
                        order.SenderPhone = recinfo.Sender_Phone;
                        order.Pro_Bill = recinfo.Pro_Bill;
                        order.Pro_BuyT = recinfo.Pro_BuyT;
                        order.Pro_Color = recinfo.Pro_Color;
                        order.Pro_Error = recinfo.Pro_Error;
                        order.Pro_GetT = recinfo.Pro_GetT;
                        order.Pro_IMEI = recinfo.Pro_IMEI;
                        order.Pro_HeaderIMEI = recinfo.Pro_HeaderIMEI;
                        order.Pro_Name = recinfo.Pro_Name;
                        order.Pro_Note = recinfo.Pro_Note;
                        order.Pro_Other = recinfo.Pro_Other;
                        order.Pro_OutSide = recinfo.Pro_OutSide;
                        order.Pro_Seq = recinfo.Pro_Seq;
                        order.Pro_SN = recinfo.Pro_SN;
                        order.Pro_Type = recinfo.Pro_Type;
                        order.ServiceID = recinfo.ServiceID;
                        //故障信息
                        order.ASP_CurrentOrder_ErrorInfo = new System.Data.Linq.EntitySet<Model.ASP_CurrentOrder_ErrorInfo>();
                        foreach (var item in recinfo.ASP_CurrentOrder_ErrorInfo)
                        {
                            Model.ASP_CurrentOrder_ErrorInfo er = new Model.ASP_CurrentOrder_ErrorInfo();
                            er.ErrorID = item.ErrorID;
                            order.ASP_CurrentOrder_ErrorInfo.Add(er);
                        }

                        //备机信息
                        order.ASP_CurrentOrder_BackupPhoneInfo = new System.Data.Linq.EntitySet<Model.ASP_CurrentOrder_BackupPhoneInfo>();
                      
                        foreach (var item in recinfo.ASP_CurrentOrder_BackupPhoneInfo)
                        {
                            Model.ASP_CurrentOrder_BackupPhoneInfo bp = new ASP_CurrentOrder_BackupPhoneInfo();
                            bp.IMEI = item.IMEI;
                            bp.InListID = item.InListID;
                            bp.NewIMEI = item.NewIMEI;
                            bp.NewProID = item.NewProID;
                            bp.ProCount = item.ProCount;
                            bp.ProID = item.ProID;
                            order.ASP_CurrentOrder_BackupPhoneInfo.Add(bp);
                        }
                        lqh.Umsdb.ASP_CurrentOrderInfo.InsertOnSubmit(order);
                        lqh.Umsdb.SubmitChanges();
                     
                        #endregion 

                        var retlist = from a in lqh.Umsdb.View_ASPReceiveInfo
                                  where a.ID == recinfo.ID
                                  select a;
                       List< Model.View_ASPReceiveInfo> retObj = retlist.ToList();

                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "添加成功！", Obj = retObj };
                    }
                    catch (Exception ez)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message=ez.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 332
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

                    var aduit_query = from b in lqh.Umsdb.View_ASPReceiveInfo
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "RpState":

                                Model.ReportSqlParams_String rep = (Model.ReportSqlParams_String)item;
                                aduit_query = from b in aduit_query
                                              where b.RpState == rep.ParamValues
                                              select b;
                                break;

                            case "Delete":
                                Model.ReportSqlParams_Bool del = (Model.ReportSqlParams_Bool)item;

                                if (del.ParamValues == true)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.Flag ==true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.Flag == null|| b.Flag==false
                                                  select b;
                                }
                                break;
                            case "SysDate":
                                Model.ReportSqlParams_DataTime mm = (Model.ReportSqlParams_DataTime)item;

                                aduit_query = from b in aduit_query
                                              where b.SysDate >= mm.ParamValues
                                              select b;

                                break;//Repairer
                            case "Repairer":
                                Model.ReportSqlParams_String reper = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.Repairer == reper.ParamValues
                                              select b;

                                break;

                            case "Dealer":
                                Model.ReportSqlParams_String d = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where (b.DealerID == null ? "" : b.DealerID.ToString()) == d.ParamValues
                                              select b;

                                break;
                                
                            case "OldID":
                                Model.ReportSqlParams_String pass = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.OldID.Contains(pass.ParamValues)
                                              select b;
                                break;
                            case "HallID":
                                Model.ReportSqlParams_ListString pass1 = (Model.ReportSqlParams_ListString)item;

                                aduit_query = from b in aduit_query
                                              where pass1.ParamValues.Contains(b.HallID)
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
                                              where b.Pro_HeaderIMEI.Contains(imei.ParamValues)
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

                        List<Model.View_ASPReceiveInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPReceiveInfo> aduitList = results.ToList();

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
        /// 获取维修次数  339
        /// </summary>
        /// <param name="user"></param>
        /// <param name="imei">主板串码</param>
        /// <returns></returns>
        public Model.WebReturn GetRepairedCount(Model.Sys_UserInfo user,string imei)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //查询指定串码是否维修中
                    var aa=  from a in lqh.Umsdb.ASP_CurrentOrderInfo
                             where a.Pro_IMEI == imei && (a.HasAudited == false || a.HasAudited == null)
                             select a;
                    if (aa.Count() > 0)
                    {
                        return new WebReturn() { ReturnValue = true, Obj = 1, ArrList = new System.Collections.ArrayList() { } };
                    }

                    //否则获取维修次数
                    var list = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                               where a.Pro_IMEI == imei && a.HasAudited == true
                               select a;
                    int count = list.Count();

                    return new WebReturn() { ReturnValue = true, Obj =2,ArrList = new System.Collections.ArrayList(){ count} };
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue=false,Message=ex.Message};   
                }
            }
        }

        /// <summary>
        /// 341
        /// </summary>
        /// <param name="user"></param>
        /// <param name="recid"></param>
        /// <returns></returns>
        public Model.WebReturn GetDetail(Model.Sys_UserInfo user, int recid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //受理故障
                    var errinfo = from a in lqh.Umsdb.ASP_CurrentOrder_ErrorInfo
                                  join b in lqh.Umsdb.ASP_ErrorInfo on a.ErrorID equals b.ID
                                  where a.ReceiveID == recid
                                  select b;
                    //备机
               
                        var bjinfo = from a in lqh.Umsdb.View_BJModels
                                     where a.ReceiveID == recid
                                     select a;


                        List<Model.View_BJModels> bjlist = new List<View_BJModels>();
                        if (bjinfo.Count() != 0)
                        {
                            bjlist.AddRange(bjinfo.ToList());
                        }
                        else
                        {
                            bjinfo = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                     join b in lqh.Umsdb.View_BJModels 
                                     on a.CurrentRepairID equals b.RepairID
                                     where a.ReceiveID == recid
                                     select b;
                            if (bjinfo.Count() != 0)
                            {
                                bjlist.AddRange(bjinfo.ToList());
                            }
                        }

                        #region 回访信息

                        var cbinfo = from a in lqh.Umsdb.ASP_CurrentOrderInfo
                                     join b in lqh.Umsdb.ASP_CallBackInfo
                                     on a.ID equals b.OrderID
                                     where a.ReceiveID == recid
                                     select b;
                        Model.ASP_CallBackInfo cb = new ASP_CallBackInfo();
                        if (cbinfo.Count() > 0)
                        { cb = cbinfo.First(); }
                       

                        #endregion 


                        return new WebReturn() { ReturnValue = true, Obj = errinfo.ToList(), ArrList = new System.Collections.ArrayList() { bjlist,cb } };
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue = false, Message = ex.Message };
                }

            }
        }

   

    }
}
