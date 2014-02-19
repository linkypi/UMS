using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.Linq;

using System.Transactions;
using Model;
using System.Text.RegularExpressions;

namespace DAL
{
    /// <summary>
    /// 销售类
    /// </summary>
    public class Pro_SellInfo
    {
        private int MethodID;

        public static bool PassBackAduit(LinQSqlHelper lqh, Model.Pro_SellOffAduitInfo model)
        {
            if (model.Pro_SellInfo_Aduit.Any())
            {
                #region 销售
                #region 生成单号
                string SellID = "";
                lqh.Umsdb.OrderMacker2(model.HallID, ref SellID);
                if (SellID == "")
                {
                    return false;
                }
                
                #endregion

                var sellinfo = model.Pro_SellInfo_Aduit.First();
                


                
                Model.Pro_SellInfo sellinfoNew = new Model.Pro_SellInfo()
                {
                    SellID = SellID,
                    Seller = sellinfo.Seller,
                    SellDate = sellinfo.SellDate,
                    OldID = sellinfo.OldID,
                    UserID = sellinfo.UserID,
                    SysDate = sellinfo.SysDate,
                    Note = sellinfo.Note,
                    HallID = sellinfo.HallID,
                    VIP_ID = sellinfo.VIP_ID,
                    CusName = sellinfo.CusName,
                    CusPhone = sellinfo.CusPhone,
                    CardPay = sellinfo.CardPay,
                    CashPay = sellinfo.CashPay,
                    OffID = sellinfo.OffID,
                    SpecalOffID = sellinfo.SpecalOffID,
                    OffTicketID = sellinfo.OffTicketID,
                    CashTotle = sellinfo.CashTotle,
                    AuditID = sellinfo.AuditID,
                    BillID = sellinfo.BillID,
                    Pro_SellListInfo = sellinfo.Pro_SellListInfo,
                    Pro_SellSpecalOffList = sellinfo.Pro_SellSpecalOffList,
                    Pro_IMEI = model.Pro_IMEI,
                    

                };
                foreach (var proImei in sellinfoNew.Pro_IMEI)
                {
                    proImei.Pro_SellOffAduitInfo = null;

                }

                model.Pro_SellInfo = sellinfoNew;
                foreach (var proSellListInfo in sellinfoNew.Pro_SellListInfo)
                {
                    if (proSellListInfo.VIP_VIPInfo != null)
                    {
                        proSellListInfo.VIP_VIPInfo.Flag = true;
                    }
                }
                return true;

                #endregion
            }

            if (model.Pro_SellBackInfo_Aduit.Any())
            {
                #region 退货

                var sellback = model.Pro_SellBackInfo_Aduit.First();
                if (sellback.Pro_SellInfo == null)
                {
                    return false;
                }
                foreach (var proSellBackList in sellback.Pro_SellBackList)
                {
                    Pro_SellBackList list = proSellBackList;
                    #region 如果有兑券
                    if (!string.IsNullOrEmpty(list.TicketID))
                    {
                        //  BackticketStr.Add(m.Pro_SellListInfo.TicketID);
                       
                        var query=lqh.Umsdb.Pro_CashTicket.Where(p => p.TicketID == list.TicketID);
                        if (query.Any())
                        {
                            var m = query.First();
                            m.IsBack = true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    #endregion
                    if (!string.IsNullOrEmpty(list.IMEI))
                    {

                        var query = lqh.Umsdb.Pro_IMEI.Where(p => p.IMEI == list.IMEI);
                        if (query.Any())
                        {
                            var m = query.First();
                            m.Pro_StoreInfo.ProCount += list.ProCount;
                            m.SellID = null;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var query =
                            lqh.Umsdb.Pro_StoreInfo.Where(p => p.InListID == list.InListID && p.HallID == model.HallID);
                        if (query.Any())
                        {
                            var m = query.First();
                            m.ProCount += list.ProCount;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (proSellBackList.Pro_SellListInfo.VIP_VIPInfo != null)
                    {
                        var vip = proSellBackList.Pro_SellListInfo.VIP_VIPInfo;
                        Model.VIP_VIPInfo_BAK bak=new   VIP_VIPInfo_BAK()
                            {
                                Note = vip.Note,
                                SysDate = vip.SysDate,
                                HallID=vip.HallID,
                                Address=vip.Address,
                                Balance=vip.Balance,
                                Birthday = vip.Birthday,
                                Flag=vip.Flag,
                                IDCard=vip.IDCard,
                                IDCard_ID=vip.IDCard_ID,
                                MemberName=vip.MemberName,
                                MobiPhone=vip.MobiPhone,
                                Point=vip.Point,
                                QQ=vip.QQ,
                                Sex=vip.Sex,
                                StartTime=vip.StartTime,
                                Sys_UserInfo=vip.Sys_UserInfo,
                                TelePhone = vip.TelePhone,
                                TypeID = vip.TypeID,
                                UpdUser = vip.UpdUser,
                                Validity = vip.Validity
                                




                            };

                        lqh.Umsdb.VIP_VIPInfo_BAK.InsertOnSubmit(bak);
                        lqh.Umsdb.VIP_VIPInfo.DeleteOnSubmit(vip);

                    }
                }

                #region 生成单号
                string SellBackID = "";
                //string SellBackHallID
                lqh.Umsdb.OrderMacker2(sellback.Pro_SellInfo.HallID, ref SellBackID);
                if (SellBackID == "")
                {
                    return false;
                }
                
                #endregion

                Model.Pro_SellBack model2 = new Model.Pro_SellBack()
                {
                    
                    SellBackID = SellBackID,
                    SellID=sellback.SellID,
                    UserID = sellback.UserID,
                    UpdUser = sellback.UpdUser,
                    UpdDate = sellback.UpdDate,
                    SysDate = sellback.SysDate,
                    Note = sellback.Note,
                    AduitID = sellback.AduitID,
                    Aduited = sellback.Aduited,
                    BackMoney = sellback.BackMoney,
                    BackID = sellback.BackID,
                    OffTicketID = sellback.OffTicketID,
                    OffTicketPrice = sellback.OffTicketPrice,
                    CashTotle = sellback.CashTotle,
                    BackOffTicketID = sellback.BackOffTicketID,
                    BackOffTicketPrice = sellback.BackOffTicketPrice,
                    CardPay = sellback.CardPay,
                    CashPay = sellback.CashPay,
                    OldCashTotle = sellback.OldCashTotle,
                    BillID = sellback.BillID,
                    ShouldBackCash = sellback.ShouldBackCash,
                    CusName = sellback.CusName,
                    CusPhone = sellback.CusPhone,
                    CusVIPCardID = sellback.CusVIPCardID,
                    NewCashTotle = sellback.NewCashTotle,
                    Pro_SellListInfo = sellback.Pro_SellListInfo,
                    Pro_SellBackList = sellback.Pro_SellBackList,
                    
                    Pro_SellSpecalOffList = sellback.Pro_SellSpecalOffList

                };
                
                model.Pro_SellBack = model2;
                EntitySet<Model.Pro_IMEI> temp=new EntitySet<Pro_IMEI>();
                temp.AddRange(model.Pro_IMEI);
                foreach (var proImei in temp)
                {
                    proImei.Pro_SellInfo = sellback.Pro_SellInfo;
                    proImei.Pro_SellOffAduitInfo = null;
                }
                foreach (var proSellListInfo in model2.Pro_SellListInfo)
                {
                    if (proSellListInfo.VIP_VIPInfo != null)
                    {
                        proSellListInfo.VIP_VIPInfo.Flag = true;
                    }
                }

                return true;

                #endregion
            }

            return false;
        }


        public static bool RollBackAduit(LinQSqlHelper lqh,Model.Pro_SellOffAduitInfo model)
        {

            if (model.Pro_SellInfo_Aduit.Any())
            {
                #region 销售

                var sellinfo = model.Pro_SellInfo_Aduit.First();
                foreach (var proSellListInfo in sellinfo.Pro_SellListInfo)
                {
                    #region 返回庫存
                    if (proSellListInfo.Pro_InOrderList.Pro_StoreInfo.Any(p => p.HallID == sellinfo.HallID))
                    {
                        var storeinfo =
                            proSellListInfo.Pro_InOrderList.Pro_StoreInfo.First(p => p.HallID == sellinfo.HallID);
                        storeinfo.ProCount += proSellListInfo.ProCount;

                        #region 串碼
                        if(proSellListInfo.Pro_ProInfo.NeedIMEI){
                        if (lqh.Umsdb.Pro_IMEI.Any(p => p.IMEI == proSellListInfo.IMEI))
                        {
                            var proimei = lqh.Umsdb.Pro_IMEI.First(p => p.IMEI == proSellListInfo.IMEI);
                            proimei.Pro_SellOffAduitInfo = null;
                        }
                        else
                        {
                            return false;
                        }
                        }
                        #endregion

                        #region 代金券
                        if (! string.IsNullOrEmpty(proSellListInfo.TicketID))
                        {
                            if (lqh.Umsdb.Pro_CashTicket.Any(p => p.TicketID == proSellListInfo.TicketID))
                            {
                                lqh.Umsdb.Pro_CashTicket.DeleteOnSubmit(
                                    lqh.Umsdb.Pro_CashTicket.First(p => p.TicketID == proSellListInfo.TicketID));
                            }
                            else
                            {
                                return false;
                            }
                        }
                        
                        #endregion

                        #region 會員卡

                        if (proSellListInfo.VIP_VIPInfo != null)
                        {
                            var vipinfo = proSellListInfo.VIP_VIPInfo;
                            Model.VIP_VIPInfo_BAK vipinfobak = new VIP_VIPInfo_BAK()
                                {
                                    VIP_VIPInfo = null,
                                    Note = vipinfo.Note,
                                    SysDate = DateTime.Now,
                                    HallID = vipinfo.HallID,
                                    Address = vipinfo.Address,
                                    Balance = vipinfo.Balance,
                                    Birthday = vipinfo.Birthday,
                                    Flag = false,
                                    IDCard = vipinfo.IDCard,
                                    IDCard_ID = vipinfo.IDCard_ID,

                                    MemberName = vipinfo.MemberName,
                                    MobiPhone = vipinfo.MobiPhone,

                                    Point = vipinfo.Point,
                                    QQ = vipinfo.QQ,
                                    Sex = vipinfo.Sex,
                                    StartTime = vipinfo.StartTime,
                                    TelePhone = vipinfo.TelePhone,
                                    TypeID = Convert.ToInt32(vipinfo.TypeID),
                                    UpdUser = vipinfo.UpdUser,
                                    Validity = vipinfo.Validity
                                };
                            lqh.Umsdb.VIP_VIPInfo.DeleteOnSubmit(vipinfo);
                        }


                        #endregion

                        #region 會員免費服務

                        //TODO:暫時不會出現此種情況

                        #endregion

                        #region 可用優惠次數
                        //TODO: 暫不返還
                        #endregion

                        
                    }
                    else
                    {
                        return false;
                    }
                  

                    #endregion
                }
                return true;

                #endregion


            }

            if (model.Pro_SellBackInfo_Aduit.Any())
            {
                #region 退貨

                var sellback = model.Pro_SellBackInfo_Aduit.First();
                if (sellback.Pro_SellInfo == null)
                {
                    return false;
                }
//                foreach (var proSellBackList in sellback.Pro_SellBackList)
//                {
//                   
//                    #region 減庫存
//                    if (proSellBackList.Pro_InOrderList.Pro_StoreInfo.Any(p => p.HallID == sellback.Pro_SellInfo.HallID))
//                    {
//
//                        var storeinfo =
//                            proSellBackList.Pro_InOrderList.Pro_StoreInfo.First(
//                                p => p.HallID == sellback.Pro_SellInfo.HallID);
//                        storeinfo.ProCount -= proSellBackList.ProCount;
//                        #region 串碼
//                        if (proSellBackList.Pro_ProInfo.NeedIMEI){
//                        if (lqh.Umsdb.Pro_IMEI.Any(p => p.IMEI == proSellBackList.IMEI))
//                        {
//                            var imeiinfo = lqh.Umsdb.Pro_IMEI.First(p => p.IMEI == proSellBackList.IMEI);
//                            imeiinfo.Pro_SellInfo = sellback.Pro_SellInfo;
//                            imeiinfo.Pro_SellOffAduitInfo = null;
//                        }
//                        else
//                        {
//                            return false;
//                        }
//                        }
//                        #endregion
//                        #region 代金券
//                        if (!string.IsNullOrEmpty(proSellBackList.TicketID))
//                        {
//                            if (lqh.Umsdb.Pro_CashTicket.Any(p => p.TicketID == proSellBackList.TicketID))
//                            {
//                                var ticketinfo =
//                                    lqh.Umsdb.Pro_CashTicket.First(p => p.TicketID == proSellBackList.TicketID);
//                                ticketinfo.IsBack = false;
//                            }
//                            else
//                            {
//                                return false;
//                            }
//                        }
//                        #endregion
//                        #region 會員
//                        if (proSellBackList.Pro_SellListInfo.VIP_VIPInfo != null)
//                        {
//                            proSellBackList.Pro_SellListInfo.VIP_VIPInfo.Flag = true;
//                        }
//                        #endregion
//
//                        #region 會員免費服務
//
//                        //TODO:暫時不會出現此種情況
//
//                        #endregion
//
//                        #region 可用優惠次數
//                        //TODO: 暫不返還
//                        #endregion
//
//
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                    #endregion 
//                }

                foreach (var proSellListInfo in sellback.Pro_SellListInfo.Where(o => o.OldSellListID == null))
                {
                    #region 返回庫存
                    if (proSellListInfo.Pro_InOrderList.Pro_StoreInfo.Any(p => p.HallID == sellback.Pro_SellInfo.HallID))
                    {
                        var storeinfo =
                            proSellListInfo.Pro_InOrderList.Pro_StoreInfo.First(p => p.HallID == sellback.Pro_SellInfo.HallID);
                        storeinfo.ProCount += proSellListInfo.ProCount;

                        #region 串碼
                        if (proSellListInfo.Pro_ProInfo.NeedIMEI)
                        {
                            if (lqh.Umsdb.Pro_IMEI.Any(p => p.IMEI == proSellListInfo.IMEI))
                            {
                                var proimei = lqh.Umsdb.Pro_IMEI.First(p => p.IMEI == proSellListInfo.IMEI);
                                proimei.Pro_SellOffAduitInfo = null;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        #endregion

                        #region 代金券
                        if (!string.IsNullOrEmpty(proSellListInfo.TicketID))
                        {
                            if (lqh.Umsdb.Pro_CashTicket.Any(p => p.TicketID == proSellListInfo.TicketID))
                            {
                                lqh.Umsdb.Pro_CashTicket.DeleteOnSubmit(
                                    lqh.Umsdb.Pro_CashTicket.First(p => p.TicketID == proSellListInfo.TicketID));
                            }
                            else
                            {
                                return false;
                            }
                        }

                        #endregion
                        #region 會員卡

                        if (proSellListInfo.VIP_VIPInfo != null)
                        {
                            var vipinfo = proSellListInfo.VIP_VIPInfo;
                            Model.VIP_VIPInfo_BAK vipinfobak = new VIP_VIPInfo_BAK()
                            {
                                VIP_VIPInfo = null,
                                Note = vipinfo.Note,
                                SysDate = DateTime.Now,
                                HallID = vipinfo.HallID,
                                Address = vipinfo.Address,
                                Balance = vipinfo.Balance,
                                Birthday = vipinfo.Birthday,
                                Flag = false,
                                IDCard = vipinfo.IDCard,
                                IDCard_ID = vipinfo.IDCard_ID,

                                MemberName = vipinfo.MemberName,
                                MobiPhone = vipinfo.MobiPhone,

                                Point = vipinfo.Point,
                                QQ = vipinfo.QQ,
                                Sex = vipinfo.Sex,
                                StartTime = vipinfo.StartTime,
                                TelePhone = vipinfo.TelePhone,
                                TypeID = Convert.ToInt32(vipinfo.TypeID),
                                UpdUser = vipinfo.UpdUser,
                                Validity = vipinfo.Validity
                            };
                            //lqh.Umsdb.VIP_VIPInfo.DeleteOnSubmit(vipinfo);
                        }


                        #endregion

                        #region 會員免費服務

                        //TODO:暫時不會出現此種情況

                        #endregion

                        #region 可用優惠次數
                        //TODO: 暫不返還
                        #endregion

                        
                    }
                    else
                    {
                        return false;
                    }
                    #endregion
                }
                return true;

                #endregion


            }


            return false;

        }
        public Pro_SellInfo()
        {
            this.MethodID = 51;
        }

        public Pro_SellInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        public EntitySet<Model.Pro_SellListInfo> _NewSellListInfo;
        //public Model.Pro_SellInfo _NewSellInfo;
        public Model.WebReturn GetProPrice(Model.Sys_UserInfo sysUser, List<Model.Pro_SellListInfo> model)
        {
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    List<Model.Pro_SellListInfo> result=new List<Pro_SellListInfo>();
                    foreach (var proSellListInfo in model)
                    {
                        Pro_SellListInfo info = proSellListInfo;
                        var query=lqh.Umsdb.Pro_SellTypeProduct.Where(
                        product => product.ProID == info.ProID && product.SellType == info.SellType);
                    if (query.Any())
                    {
                        proSellListInfo.SellType_Pro_ID = query.First().ID;
                        proSellListInfo.ProPrice = query.First().Price;
                        result.Add(proSellListInfo);
                        //return new WebReturn() {Obj = model, ReturnValue = true};
                    }
                    else
                    {
                       //
                    }

                    }
                    return new WebReturn() {Obj = result, ReturnValue = true};

                }

            }
            catch (Exception ex)
            {
                return new WebReturn() { ReturnValue = false, Message = ex.Message };
            }

        }


        public Model.WebReturn Add_Service(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model)
        {

            Model.WebReturn r = null;
            bool NoError = true;
            this._NewSellListInfo = model.Pro_SellListInfo;
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    DataLoadOptions dataload = new DataLoadOptions();

                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;
                    Model.VIP_VIPInfo vip;
                    #region 验证会员信息
                    if (model.VIP_ID != null && model.VIP_ID != 0)
                    {
                        var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                        where b.ID == model.VIP_ID
                                        select b;
                        if (!vip_query.Any())
                        {
                            return new Model.WebReturn() {ReturnValue = false, Message = "该会员不存在"};
                        }
                        vip = vip_query.First();
                        model.CusName = vip.MemberName;
                        model.CusPhone = vip.MobiPhone;

                        
                    }
                    else
                    {
                         vip = null;
                    }
                    #endregion
                    #region 验证促销 操作人员

                    var user_seller = from b in lqh.Umsdb.Sys_UserInfo
                                      where b.UserID == model.UserID && b.CanLogin == true && b.Flag == true
                                      select b;
                    if (user_seller.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "操作员不存在" };
                    }

                    var user_Oper = from b in lqh.Umsdb.Sys_UserInfo
                                    where b.UserID == model.Seller && b.Flag == true
                                    select b;
                    if (user_Oper.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售员不存在" };
                    }
                    #endregion

                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    //                    r=ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MenthodID, ValidHallIDS,ValidProIDS);
                    //
                    //                    if (r.ReturnValue != true)
                    //                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    //                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                    //                        return new Model.WebReturn() {  ReturnValue=false,  Message=model.HallID+"仓库无权操作"};
                    #endregion

//                    #region 生产单号
//                    string SellID = "";
//                    lqh.Umsdb.OrderMacker(1, "XS", "XS", ref SellID);
//                    if (SellID == "")
//                    {
//                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单生成出错" };
//                    }
//                    model.SellID = SellID;
//
//                    #endregion

                    //存放串码
                    List<string> IMEI = new List<string>();
                    //存放赠品
                    //List<Model.Pro_SellSendInfo> sendList = new List<Model.Pro_SellSendInfo>();
                    //商品 销售类别
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放无串码
                    List<string> ProIDNoIMEI = new List<string>();
                    //存放优惠
                    List<int?> OffID_List = new List<int?>();

                    #region 获取当前订单包含的串号、商品编号

                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        ////有商品限制，而且商品不在权限范围内
                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        }
                        SellType_ProID_List.Add(m.SellType_Pro_ID);
                        //串码
                        if (!string.IsNullOrEmpty(m.IMEI))
                        {
                            if (IMEI.Contains(m.IMEI))
                            {
                                NoError = false;
                                m.Note = m.IMEI + "串码重复";
                                continue;
                            }
                            else
                                IMEI.Add(m.IMEI);
                        }

                        //商品编号
                        if (!ProIDNoIMEI.Contains(m.ProID) && string.IsNullOrEmpty(m.IMEI))
                            ProIDNoIMEI.Add(m.ProID);

                        #region 有赠品
                        //if (m.Pro_SellSendInfo.Count() > 0)
                        //{
                        //    sendList.AddRange(m.Pro_SellSendInfo);
                        //}
                        #endregion

                        if (m.OffID > 0)
                            OffID_List.Add(m.OffID);
                    }
                    #endregion

                    //组合的
                    foreach (Model.Pro_SellSpecalOffList m in model.Pro_SellSpecalOffList)
                    {
                        if (m.SpecalOffID <= 0 || m.SpecalOffID == null)
                        {
                            NoError = false;
                            continue;
                            m.Note = "组合优惠有误";
                            continue;
                        }
                        m.Pro_SellListInfo = null;
                        OffID_List.Add(m.SpecalOffID);
                        //sendList.AddRange(m.Pro_SellSendInfo);
                    }
                    //优惠券的
                    if (model.OffTicketID > 0)
                        OffID_List.Add(model.OffTicketID);

                    //sendList.AddRange(model.Pro_SellSendInfo);

                    #region 有赠品，则获取串码 和 商品编号
                    //if (sendList.Count() > 0)
                    //{
                    //    foreach (Model.Pro_SellSendInfo m in sendList)
                    //    {
                    //        if (!string.IsNullOrEmpty(m.IMEI))
                    //        {
                    //            if (IMEI.Contains(m.IMEI))
                    //            {
                    //                NoError = false;
                    //                m.Note = m.IMEI + "串码重复";
                    //                continue;
                    //            }
                    //            else
                    //                IMEI.Add(m.IMEI);
                    //        }
                    //        if (!ProIDNoIMEI.Contains(m.ProID) && string.IsNullOrEmpty(m.IMEI))
                    //            ProIDNoIMEI.Add(m.ProID);
                    //    }
                    //}
                    #endregion

                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    }

                    //串码类拣货
                    var imeiList = (from b in lqh.Umsdb.Pro_IMEI
                                    where IMEI.Contains(b.IMEI) && b.HallID == model.HallID
                                    select b).ToList();
                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == model.HallID && ProIDNoIMEI.Contains(b.ProID)
                                     orderby b.InListID
                                     select b).ToList();
                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                       group b by b.ProID into temp
                                       select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();






                    #region 验证优惠信息的有效性

                    List<Model.VIP_ProOffList> pro_off_list;
                    if (vip == null)
                    {
                        pro_off_list=new    List<VIP_ProOffList>();
                    }
                    else{
                     pro_off_list = (from b in lqh.Umsdb.VIP_ProOffList
                                        where (OffID_List.Contains(b.VIP_OffList.ID))

                                         && ((((from c in b.VIP_OffList.VIP_VIPOffLIst //会员卡专属优惠
                                                where c.VIPID == model.VIP_ID
                                                select c).Any()
                                                              ||
                                                              (from c in b.VIP_OffList.VIP_VIPTypeOffLIst//会员类别专属优惠
                                                               where vip != null && c.VIPType == vip.TypeID
                                                               select c).Any()
                                                              )
                                                              && (from c in b.VIP_OffList.VIP_HallOffInfo//门店专属优惠
                                                                  where c.HallID == model.HallID
                                                                  select c).Any()
                                                              ))
                                                              && b.VIP_OffList.Flag == true
                                                            && b.VIP_OffList.EndDate >= DateTime.Now
                                                            && b.VIP_OffList.StartDate <= DateTime.Now
                                            // && new int?[] { 0, 1, 2, 3 }.Contains(b.VIP_OffList.Type)
                                                            && b.VIP_OffList.UseLimit < b.VIP_OffList.VIPTicketMaxCount
                                        orderby b.VIP_OffList.OffMoney descending
                                        select b).ToList();
                    }
                    var AllOffList = from b in pro_off_list
                                     select b.VIP_OffList;
                    #endregion

                    #region 生成组合优惠左连接部分
                    var Sepecial_ProOffList = (from b in model.Pro_SellSpecalOffList
                                               join c in pro_off_list
                                               on b.SpecalOffID equals c.OffID
                                               into temp
                                               from d in temp.DefaultIfEmpty()
                                               where d.VIP_OffList.Type == 1
                                               select new
                                               {
                                                   Pro_SellSpecalOffList = b,
                                                   pro_off_list = d

                                               }).ToList();

                    List<Model.VIP_OffList> vip_offList_Update = new List<Model.VIP_OffList>();
                    List<Model.Pro_SellSpecalOffList> SellSpecalOffList = new List<Model.Pro_SellSpecalOffList>();
                    foreach (var s in Sepecial_ProOffList)
                    {
                        //组合优惠是否存在
                        if (s.pro_off_list == null)// && !SellSpecalOffList.Contains(s.Pro_SellSpecalOffList))
                        {
                            NoError = false;
                            //SellSpecalOffList.Add(s.Pro_SellSpecalOffList);
                            s.Pro_SellSpecalOffList.Note = "组合优惠不存在";
                            continue;
                        }
                        //有错误了，继续循环组合优惠，查找所有错误
                        if (!NoError)
                        {
                            continue;
                        }
                        if (!vip_offList_Update.Contains(s.pro_off_list.VIP_OffList))
                        {
                            vip_offList_Update.Add(s.pro_off_list.VIP_OffList);

                            if (s.pro_off_list.VIP_OffList.VIPTicketMaxCount != 99999 && s.pro_off_list.VIP_OffList.VIPTicketMaxCount - s.pro_off_list.VIP_OffList.UseLimit - 1 < 0)
                            {
                                NoError = false;
                                s.Pro_SellSpecalOffList.Note = "组合优惠名额超限";
                            }
                            else
                                s.pro_off_list.VIP_OffList.UseLimit++;
                        }
                    }


                    if (!NoError)//SellSpecalOffList.Count() > 0 ||
                    {
                        //Array.ForEach<Model.Pro_SellSpecalOffList>(SellSpecalOffList.ToArray(), p => { p.Note = "组合优惠不存在"; });
                        return new Model.WebReturn() { ReturnValue = false, Message = "组合优惠信息有误", Obj = model };
                    }
                    #endregion

                    #region 生成验证销售方式、各种价格左连接部分
                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where SellType_ProID_List.Contains(b.ID)
                                            select b).ToList();
                    #endregion


                    #region 左连接单品优惠 组合优惠 销售方式


                    var join_query = from b in model.Pro_SellListInfo
                                     join c in pro_off_list
                                         on new {b.ProID, b.SellType, b.OffID}
                                         equals
                                         new {c.ProID, SellType = c.SellTypeID, c.OffID}
                                         into temp1
                                     from c1 in temp1.DefaultIfEmpty()
                                     //where b.VIP_OffList.Type == 0
                                     join d in Sepecial_ProOffList
                                         on new {ID = b.SpecialID ?? 0, b.ProID, b.ProCount, b.SellType}
                                         equals
                                         new
                                             {
                                                 ID = d.Pro_SellSpecalOffList.ID,
                                                 d.pro_off_list.ProID,
                                                 d.pro_off_list.ProCount,
                                                 SellType = d.pro_off_list.SellTypeID
                                             }
                                         into temp2
                                     from d1 in temp2.DefaultIfEmpty()
                                     join e in Pro_SellTypeList
                                         on b.SellType_Pro_ID equals e.ID
                                         into temp3
                                     from e1 in temp3.DefaultIfEmpty()
                                     join f in imeiList
                                         on b.IMEI equals f.IMEI
                                         into temp4
                                     from f1 in temp4.DefaultIfEmpty()
                                     join g in StoreList__
                                         on b.ProID equals g.ProID
                                         into temp5
                                     from g1 in temp5.DefaultIfEmpty()
                                     select new
                                         {
                                             Pro_SellListInfo = b,
                                             VIP_ProOffList_0 = c1,
                                             VIP_ProOffList_1 = d1 == null ? null : d1.pro_off_list,
                                             Pro_SellTypeProduct = e1,
                                             Pro_IMEI = f1,
                                             Pro_StoreInfo = g1,
                                             Pro_SellSpecalOffList = d1 == null ? null : d1.Pro_SellSpecalOffList,
                                             d1
                                         };
                    #endregion


                    var join_query2 =
                        model.Pro_SellListInfo.GroupJoin(pro_off_list, b => new { b.ProID, b.SellType, b.OffID },
                                                         c => new { c.ProID, SellType = c.SellTypeID, c.OffID },
                                                         (b, temp1) => new { b, temp1 });
                    var temp9 = join_query2.SelectMany(@t => @t.temp1.DefaultIfEmpty(), (@t, c1) => new { @t, c1 });
                    var temp10 = temp9.GroupJoin(Sepecial_ProOffList,
                                        @t =>
                                        new
                                        {
                                            @t.@t.b.Pro_SellSpecalOffList,
                                            @t.@t.b.ProID,
                                            @t.@t.b.ProCount,
                                            @t.@t.b.SellType
                                        },
                                        d =>
                                        new
                                        {
                                            d.Pro_SellSpecalOffList,
                                            d.pro_off_list.ProID,
                                            d.pro_off_list.ProCount,
                                            SellType = d.pro_off_list.SellTypeID
                                        }, (@t, temp2) => new { @t, temp2 });
                    var temp11 = temp10.SelectMany(@t => @t.temp2.DefaultIfEmpty(), (@t, d1) => new { @t, d1 })
                             .GroupJoin(Pro_SellTypeList, @t => @t.@t.@t.@t.b.SellType_Pro_ID, e => e.ID,
                                        (@t, temp3) => new { @t, temp3 });
                    var temp12 = temp11.SelectMany(@t => @t.temp3.DefaultIfEmpty(), (@t, e1) => new { @t, e1 })
                             .GroupJoin(imeiList, @t => @t.@t.@t.@t.@t.@t.b.IMEI, f => f.IMEI,
                                        (@t, temp4) => new { @t, temp4 });
                    var temp13 = temp12.SelectMany(@t => @t.temp4.DefaultIfEmpty(), (@t, f1) => new { @t, f1 })
                             .GroupJoin(StoreList__, @t => @t.@t.@t.@t.@t.@t.@t.@t.b.ProID, g => g.ProID,
                                        (@t, temp5) => new { @t, temp5 });
                    var temp14 = temp13.SelectMany(@t => @t.temp5.DefaultIfEmpty(), (@t, g1) => new
                    {
                        Pro_SellListInfo = @t.@t.@t.@t.@t.@t.@t.@t.@t.b,
                        VIP_ProOffList_0 = @t.@t.@t.@t.@t.@t.@t.@t.c1,
                        VIP_ProOffList_1 = @t.@t.@t.@t.@t.@t.d1.pro_off_list,
                        Pro_SellTypeProduct = @t.@t.@t.@t.e1,
                        Pro_IMEI = @t.@t.f1,
                        Pro_StoreInfo = g1,
                        @t.@t.@t.@t.@t.@t.d1
                    });




                    List<Model.Pro_CashTicket> cashTickList = new List<Model.Pro_CashTicket>();
                    List<Model.Pro_SellListInfo> sellListTemp = new List<Model.Pro_SellListInfo>();

                    List<string> str_ticket = new List<string>();
                    decimal? cashPrice = 0;
                    foreach (var child in join_query)
                    {
                        Sepecial_ProOffList.Remove(child.d1);
                        //SellType_ProID_List.Add(child.SellType_Pro_ID);
                        //ProID.Add(child.Pro_SellListInfo.ProID);
                        //OffID_List.Add(child.OffID);
                        #region 验证单品优惠 组合优惠 商品信息 销售类别 有无串码 单价


                        //验证单品优惠
                        //if (child.Pro_SellListInfo.OffID > 0)
                        //{
                        //    if (child.VIP_ProOffList_0 == null || child.VIP_ProOffList_0.VIP_OffList == null)
                        //    {
                        //        NoError = false;
                        //        child.Pro_SellListInfo.Note = "单品优惠不存在或已过期";
                        //        continue;
                        //    }
                        //}
                        //验证组合优惠
                        //if (child.Pro_SellListInfo.SpecialID > 0)
                        //{
                        //    if (child.VIP_ProOffList_1 == null || child.VIP_ProOffList_1.VIP_OffList == null)
                        //    {
                        //        NoError = false;
                        //        child.Pro_SellListInfo.Note = "组合优惠不存在或已过期或不满足条件";
                        //        child.Pro_SellListInfo.Pro_SellSpecalOffList.Note = "组合优惠不存在或已过期或不满足条件";
                        //        continue;
                        //    }
                        //}
                        //验证销售类别、商品信息
                        if (child.Pro_SellTypeProduct == null || child.Pro_SellTypeProduct.Pro_ProInfo == null)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品不存在或销售类别不正确";
                            continue;
                        }
                        if (child.Pro_SellTypeProduct.Pro_ProInfo.NeedIMEI == true && string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品缺少串码";
                            continue;
                        }
                        if (child.Pro_SellTypeProduct.Pro_ProInfo.NeedIMEI != true && !string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "属于无串码商品";
                            continue;
                        }
                        if (child.Pro_SellListInfo.IsFree)
                        {
                            if (child.Pro_SellListInfo.ProPrice != 0)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "商品的单价有误";
                                continue;
                            }
                        }
                        else
                        if (child.Pro_SellTypeProduct.Price != child.Pro_SellListInfo.ProPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品的单价有误";
                            continue;
                        }
                        #endregion

                        if (!NoError)
                            continue;

                        child.Pro_SellListInfo.LowPrice = child.Pro_SellTypeProduct.LowPrice;
                        child.Pro_SellListInfo.ProCost = child.Pro_SellTypeProduct.ProCost;



//                        #region 验证代金券
//                        if (!string.IsNullOrEmpty(child.Pro_SellListInfo.TicketID))
//                        {
//                            //验证代金券
//                            r = CheckProCashTicket(child.Pro_SellListInfo);
//                            if (r.ReturnValue != true)
//                            {
//                                NoError = false;
//                                continue;
//                            }
//                            cashTickList.Add(new Model.Pro_CashTicket() { Pro_SellListInfo = child.Pro_SellListInfo, TicketID = child.Pro_SellListInfo.TicketID });
//
//                            if (str_ticket.Contains(child.Pro_SellListInfo.TicketID))
//                            {
//                                NoError = false;
//
//                                child.Pro_SellListInfo.Note = child.Pro_SellListInfo.TicketID + "代金券重复";
//
//                                return new Model.WebReturn() { Obj = model, ReturnValue = false, Message = child.Pro_SellListInfo.Note };
//
//                            }
//                            str_ticket.Add(child.Pro_SellListInfo.TicketID);
//                        }
//                        #endregion



                        #region 单品优惠验证

                        child.Pro_SellListInfo.VIP_OffList = child.VIP_ProOffList_0 == null
                                                                 ? null
                                                                 : child.VIP_ProOffList_0.VIP_OffList;

                        r = CheckProOff(child.Pro_SellListInfo);
                        if (r.ReturnValue != true)
                        {
                            NoError = false;
                            continue;
                        }

                        #endregion

                        #region 验证组合优惠
                        child.Pro_SellListInfo.Pro_SellSpecalOffList = child.Pro_SellSpecalOffList;
                        r = CheckSpecialOff(child.Pro_SellListInfo, child.VIP_ProOffList_1);
                        if (r.ReturnValue != true)
                        {
                            NoError = false;
                            continue;
                        }

                        #endregion

                        #region 验证实收
                        decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice - child.Pro_SellListInfo.OffSepecialPrice + child.Pro_SellListInfo.OtherCash;
                        if (real * child.Pro_SellListInfo.ProCount != child.Pro_SellListInfo.CashPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收计算有误";
                            continue;
                        }
                        #endregion



                        cashPrice += child.Pro_SellListInfo.CashPrice;

                        #region 串码类验证
                        if (!string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = CheckIMEI(child.Pro_IMEI);
                            if (!r.ReturnValue)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = r.Message;
                                continue;
                            }
                            child.Pro_SellListInfo.InListID = child.Pro_IMEI.InListID;
                            child.Pro_SellListInfo.ProCost = child.Pro_IMEI.Pro_InOrderList.InitInList.Price;
                            child.Pro_IMEI.Pro_StoreInfo.ProCount--;
                            child.Pro_IMEI.Pro_SellInfo = model;
                        }
                        #endregion



                        #region 非串码类验证
                        if (string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = FitInOrderListIDNoIMEI(child.Pro_SellListInfo, child.Pro_StoreInfo, sellListTemp);
                            if (!r.ReturnValue == true)
                            {
                                NoError = false;
                                continue;
                            }
                        }
                        #endregion

                    }
                    if (!NoError)//有错误
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "提交有误", Obj = model };
                    }
                    if (Sepecial_ProOffList.Count > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "部分组合优惠没有满足条件" };
                    }
                    if (cashPrice != model.CashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误", Obj = model };
                    }
                    #region 存在未拣完货的
                    if (sellListTemp.Count > 0)
                    {
                        r = FitInOrderListIDNoIMEI(sellListTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        model.Pro_SellListInfo.AddRange(sellListTemp);
                    }
                    #endregion

                    #region 验证赠品
                    //if (sendList.Count() > 0)
                    //{
                    //    List<Model.VIP_SendProOffList> SendProOffList = new List<Model.VIP_SendProOffList>();
                    //    List<Model.Pro_SellSendInfo> sendTempList = new List<Model.Pro_SellSendInfo>();
                    //    foreach (Model.VIP_OffList off in AllOffList)
                    //    {
                    //        SendProOffList.AddRange(off.VIP_SendProOffList);
                    //    }
                    //    var aa=from b in sendList
                    //           group b by new{b.OffID,b.ProID}

                    //           into temp
                    //           from c in temp
                    //           select new{ c.OffID,c.ProID,ProCount=temp.Sum(p=>p.ProCount)};
                    //    var send_join_SendProoff=from b in aa
                    //                             join c in SendProOffList
                    //                             on new { b.OffID, b.ProID, PerCount = b.ProCount }
                    //                             equals new{c.OffID,c.ProID,c.PerCount}
                    //                             into temp
                    //                             from d in temp.DefaultIfEmpty()
                    //                             select new {b,d};
                    //    foreach (var m in send_join_SendProoff)
                    //    {
                    //        if (m.d == null || m.b.ProCount + m.d.ProCount > m.d.LimitCount)
                    //        {

                    //            return new Model.WebReturn { ReturnValue = false, Obj = model, Message = "赠品列表有误" };
                    //        }
                    //        else
                    //        {
                    //            m.d.ProCount += m.d.PerCount;
                    //        }
                    //    }
                    //    StoreList__ = (from b in StoreList
                    //                   where b.ProCount > 0
                    //                   group b by b.ProID into temp
                    //                   select temp.Single((p => p.InListID == temp.Min(p2 => p2.InListID)))).ToList();
                    //    var send_join_pro_store = from b in sendList
                    //                              join c in imeiList
                    //                              on b.IMEI equals c.IMEI
                    //                              into temp
                    //                              from d in temp.DefaultIfEmpty()
                    //                              join e in StoreList__
                    //                              on b.ProID equals e.ProID
                    //                              into temp2
                    //                              from e1 in temp2.DefaultIfEmpty()
                    //                              join f in SendProOffList
                    //                              on new { b.OffID, b.ProID }
                    //                              equals new{f.OffID,f.ProID}
                    //                              into temp3
                    //                              from f1 in temp3.DefaultIfEmpty()
                    //                              select new { b,d,e1,f1};

                    //    foreach (var m in send_join_pro_store)
                    //    {
                    //        #region 有串码
                    //        if (!string.IsNullOrEmpty(m.b.IMEI))
                    //        {
                    //            m.b.ProCount = 1;
                    //            r = CheckIMEI(m.d);
                    //            if (!r.ReturnValue == true)
                    //            {
                    //                NoError = false;
                    //                m.b.Note = r.Message;
                    //                continue;
                    //            }
                    //            if (!NoError) continue;
                    //            else
                    //            {
                    //                if (m.d.Pro_StoreInfo.ProCount < 1)
                    //                {
                    //                    NoError = false;
                    //                    m.b.Note = "库存不够";
                    //                    continue;
                    //                }
                    //                m.d.Pro_StoreInfo.ProCount--;
                    //                m.d.Pro_SellInfo = model;
                    //                m.b.ProCost=m.f1.ProCost;
                    //            }

                    //        }
                    //        #endregion
                    //        #region 无串码
                    //        else
                    //        {
                    //           r=  FitInOrderListIDNoIMEI(m.b, m.e1, sendTempList);
                    //           if (!r.ReturnValue == true)
                    //           {
                    //               NoError = false;
                    //               continue;
                    //           }
                    //        }
                    //        #endregion

                    //    }
                    //    #region 存在未拣完货的赠品

                    //    if (sendTempList.Count > 0)
                    //    {
                    //        r = FitInOrderListIDNoIMEI(sendTempList, StoreList);
                    //        if (r.ReturnValue != true)
                    //        {
                    //            r.Obj = model;
                    //            return r;
                    //        }
                    //    }

                    //    #endregion
                    //}
                    #endregion

                    #region 保存代金券
                    var a = from b in lqh.Umsdb.Pro_CashTicket
                            where str_ticket.Contains(b.TicketID) && b.IsBack != true
                            select b;

                    if (a.Count() > 0)
                        return new Model.WebReturn() { ReturnValue = false, Message = a.First().TicketID + "代金券已被使用" };
                    lqh.Umsdb.Pro_CashTicket.InsertAllOnSubmit(cashTickList);
                    #endregion



                    #region 旧代码






                    //验证销售方式、各种价格


                    //var selllist_query = CheckProSellType(model.Pro_SellListInfo.ToList(), SellType_ProID_List, lqh);

                    //var null_query = selllist_query.Where(p => p.Pro_SellTypeProduct==null || p.ProPrice !=p.Pro_SellTypeProduct.Price);
                    //if (null_query.Count() > 0)
                    //{
                    //    model.Pro_SellListInfo.Clear();
                    //    model.Pro_SellListInfo.AddRange(selllist_query);
                    //    return new Model.WebReturn() { Obj = model, ReturnValue = false, Message = null_query.First().ProID + "商品信息或销售方式错误" };
                    //}


                    //创建添加到数据库的 新 实体
                    //Model.Pro_SellInfo InsertSell = new Model.Pro_SellInfo();
                    //InsertSell.Pro_SellListInfo.AddRange(selllist_query);
                    //InsertSell.Pro_SellSpecalOffList.AddRange(model.Pro_SellSpecalOffList);


                    //验证代金券
                    //Model.WebReturn r = CheckProCashTicket(selllist_query.Where(p => !string.IsNullOrEmpty(p.TicketID) && p.Pro_SellTypeProduct.IsTicketUseful == true).ToList(), lqh);

                    //if (r.ReturnValue != true)
                    //{
                    //    return r;
                    //}



                    //验证单品的优惠
                    //r = CheckProOff(selllist_query, pro_off_list.Where(p => p.VIP_OffList.Type == 0).ToList());
                    //if (r.ReturnValue != true)
                    //{
                    //    return r;
                    //}
                    //筛选组合优惠


                    //r = CheckSpecialOff(InsertSell, AllOffList.Where(p => p.Type == 1).Distinct().ToList());
                    //if (r.ReturnValue != true)
                    //{
                    //    return r;
                    //}

                    //验证优惠券

                    #endregion


                    #region 为会员持有的免费服务冲减金额

                    if (vip == null && model.Pro_SellListInfo.Any(p => p.IsFree))
                    {
                        return new WebReturn() {ReturnValue = false, Message = "非会员不能享受免费服务"};
                    }

                    if (vip != null)
                    {
                        foreach (var proSellListInfo in model.Pro_SellListInfo)
                        {
                            if (proSellListInfo.IsFree){



                            if (
                                vip.VIP_VIPService.Any(
                                    service =>
                                    service.ProID == proSellListInfo.ProID &&
                                    service.SCount - service.UsedCount >= proSellListInfo.ProCount))
                            {
                                vip.VIP_VIPService.First(service => service.ProID == proSellListInfo.ProID).UsedCount +=
                                    proSellListInfo.ProCount;
                                //proSellListInfo.IsFree = true;
                                //TODO: BUG
                                if (proSellListInfo.Pro_Sell_Service.Any())
                                {
                                    proSellListInfo.Pro_Sell_Service.First().IsVIPService = true;
                                    proSellListInfo.Pro_Sell_Service.First().VIPService_ProID = proSellListInfo.ProID;
                                }
                            }
                            else
                            {
                                return new WebReturn() { ReturnValue = false, Message = "免费服务错误" };
                            }
                            }
                        }
                    }

                    #endregion


                    #region 验证优惠券 和 实际收入
                    //验证优惠券 和 实际收入 
                    r = CheckTicketOff(model, vip);

                    if (r.ReturnValue != true)
                    {
                        return r;
                    }
                    #endregion




                    #region 生成单号
                    string SellID = "";
                    lqh.Umsdb.OrderMacker2(model.HallID, ref SellID);
                    if (SellID == "")
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单生成出错" };
                    }
                    model.SellID = SellID;
                    #endregion

                    lqh.Umsdb.Pro_SellInfo.InsertOnSubmit(model);


//                    #region 刪去临时表
//                    var needdelete1 =
//                        lqh.Umsdb.Pro_SellListInfo_Temp.Where(
//                            p => model.Pro_SellListInfo.Select(q => q.ID).Contains(p.ID));
//                    var needdelete2 =
//                        lqh.Umsdb.Pro_Sell_Yanbao_temp.Join(lqh.Umsdb.Pro_SellListInfo_Temp, temp => temp.SellListID,
//                                                            temp => temp.ID, (temp, infoTemp) => temp);
//                    lqh.Umsdb.Pro_Sell_Yanbao_temp.DeleteAllOnSubmit(needdelete2);
//                    lqh.Umsdb.Pro_SellListInfo_Temp.DeleteAllOnSubmit(needdelete1);
//                    #endregion

                    lqh.Umsdb.SubmitChanges();


                }
                return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "" + ex.Message };
            }
        }

        public Model.WebReturn Add_temp(Model.Sys_UserInfo sysuser, List<Model.Pro_SellListInfo_Temp> model)
        {
            try
            {
                using (LinQSqlHelper lqh=new LinQSqlHelper())
                {
                    var proIDS = from b in model select b.ProID;
                    var Pro_SellType_ProPrice = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                                 where proIDS.Contains(b.ProID)
                                                 select b).ToList();
                    var Selllist_temp = (from b in model
                                         join
                                         c in Pro_SellType_ProPrice
                                         on new { b.ProID, b.SellType }
                                         equals
                                         new { c.ProID, c.SellType }
                                         select new
                                         {

                                                b=b,
                                                c=c
                                             
                                            
                                         }).ToList();
                    foreach (var result in Selllist_temp)
                    {
                        result.b.SellType_Pro_ID = result.c.ID;
                        result.b.ProPrice = result.b.ProPrice == 0 ? result.c.Price : result.b.ProPrice;
                        result.b.InsertDate = DateTime.Now;
                    }
                    var Selllist_temp2 = Selllist_temp.Select(p => p.b);
                    foreach (var proSellListInfoTemp in Selllist_temp2)
                    {
                        if (proSellListInfoTemp.AnBu > proSellListInfoTemp.ProPrice)
                        {
                            throw new Exception("暗补金额不可超过单价");
                        }
                        switch (proSellListInfoTemp.SellType)
                        {
                            case 2:
                                {
                                    decimal TicketUsed = 0;
                                    string ZSREG = @"^S7[56]0(\d\d)(\d\d)(\d\d)([0-9A-Z]{8})$";
                                    Regex ex = new Regex(ZSREG);
                                    if (!ex.IsMatch(proSellListInfoTemp.TicketID))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "券编码格式不对");
                                    }
                                    MatchCollection match = ex.Matches(proSellListInfoTemp.TicketID);
                                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                                    //string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                                    decimal ticket = proSellListInfoTemp.CashTicket;

                                    if (!DAL.ValidClassInfo.IsDateTime(date))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "券编码格式不对");
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    decimal TicketUsed = 0;

                                    string ZSREG = @"^7[56]020(\d\d)(\d\d)(\d\d)(\d{11})(\+\d){0,1}$";
                                    string ZSREG2 = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{6})(\+\d){0,1}$";
                                    Regex ex = new Regex(ZSREG);
                                    Regex ex2 = new Regex(ZSREG2);
                                    Regex ok;
                                    if (!ex.IsMatch(proSellListInfoTemp.TicketID))
                                    {
                                        if (!ex2.IsMatch(proSellListInfoTemp.TicketID))
                                        {
                                            throw new Exception(proSellListInfoTemp.TicketID + "合约码或购机送费码格式不对");
                                        }
                                        else
                                        {
                                            ok = ex2;
                                        }
                                    }
                                    else
                                    {
                                        ok = ex;
                                    }
                                    MatchCollection match = ok.Matches(proSellListInfoTemp.TicketID);
                                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                                    decimal? ticket = proSellListInfoTemp.CashTicket;
                                    ;
                                    if (!DAL.ValidClassInfo.IsDateTime(date))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "编码格式不对");
                                    }

                                   
                                }

                                break;
                            case 5:
                                {
                                    decimal TicketUsed = 0;
                                    string ZSREG = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{9})$";
                                    Regex ex = new Regex(ZSREG);
                                    if (!ex.IsMatch(proSellListInfoTemp.TicketID))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "券编码格式不对");
                                    }
                                    MatchCollection match = ex.Matches(proSellListInfoTemp.TicketID);
                                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                                    //string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                                    decimal ticket = proSellListInfoTemp.CashTicket;
                                    ;
                                    if (!DAL.ValidClassInfo.IsDateTime(date))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "券编码格式不对");
                                    }

                                    break;
                                }
                            default:

                                break;
                        }
                    }

                    if (Selllist_temp.Count() != model.Count)
                    {
                        return new WebReturn() { ReturnValue = false, Message = "有商品未定价 无法保存" };
                    }
                    lqh.Umsdb.Pro_SellListInfo_Temp.InsertAllOnSubmit(Selllist_temp2);
                    lqh.Umsdb.SubmitChanges();
                    return new WebReturn() { ReturnValue = true, Obj = Selllist_temp2.ToList() };



                }
            }
            catch (Exception ex)
            {
                return new WebReturn(){ReturnValue = false,Message = ex.Message}; 
                
            }
        }

        public Model.WebReturn Add_temp(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model)
        {
            try
            {
                int MenuID = 0;
                
                using (LinQSqlHelper lqh =new LinQSqlHelper())
                {
                    List<Model.Pro_SellListInfo_Temp> sellListInfoTemps = new List<Model.Pro_SellListInfo_Temp>();

                    var proIDS = from b in model.Pro_SellListInfo select b.ProID;
                    var Pro_SellType_ProPrice = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                                 where proIDS.Contains(b.ProID)
                                                 select b).ToList();
                    if ((model.Pro_SellListInfo.Join(Pro_SellType_ProPrice,
                                                     b => new {b.ProID, b.SellType},
                                                     c => new {c.ProID, c.SellType},
                                                     (b, c) => new {b, c})).Any(
                                                         p =>
                                                         p.b.YanBaoModel == null && p.b.ProPrice != 0 &&
                                                         (p.b.ProPrice < p.c.MinPrice || p.b.ProPrice > p.c.MaxPrice))
                        )
                    {
                        throw new Exception("价格有误");
                    }
                    var Selllist_temp = (from b in model.Pro_SellListInfo
                                         join
                                             c in Pro_SellType_ProPrice
                                             on new {b.ProID, b.SellType}
                                             equals
                                             new {c.ProID, c.SellType}
                                         select new Model.Pro_SellListInfo_Temp()
                                             {


                                                 ProID = b.ProID,
                                                 ProCount = b.ProCount,
                                                 SellType = b.SellType,
                                                 CashTicket = b.CashTicket,
                                                 ProPrice = b.ProPrice == 0 ? c.Price : b.ProPrice,

                                                 TicketID = b.TicketID,
                                                 TicketUsed = b.TicketUsed,
                                                 CashPrice = b.CashPrice,
                                                 IMEI = b.IMEI,
                                                 ServiceInfo = b.ServiceInfo,
                                                 Note = b.Note,
                                                 ProCost = b.ProCost,
                                                 LowPrice = b.LowPrice,
                                                 AduidID = b.AduidID,
                                                 AduidedOldPrice = b.AduidedOldPrice,
                                                 OffID = b.OffID,
                                                 OffPoint = b.OffPoint,
                                                 SpecialID = b.SpecialID,
                                                 SellType_Pro_ID = c.ID,
                                                 WholeSaleOffPrice = b.WholeSaleOffPrice,
                                                 BackID = b.BackID,
                                                 OldSellListID = b.OldSellListID,
                                                 IsFree = b.IsFree,
                                                 OldID = b.OldID,
                                                 HallID = model.HallID,
                                                 UserID = sysUser.UserID,
                                                 InsertDate = DateTime.Now,
                                                 AnBu = b.AnBu,
                                                 LieShouPrice = b.LieShouPrice,
                                                 NeedAduit = b.NeedAduit,
                                                 YanBaoModelPrice = Common.Utils.IsZhongshanSellType(b.SellType) ?
                                                     (c.Pro_ProInfo.Pro_SellTypeProduct.Any(p => p.SellType == 1)
                                                         ? c.Pro_ProInfo.Pro_SellTypeProduct.First(p => p.SellType == 1)
                                                            .Price
                                                         : 0):
                                                         (c.Pro_ProInfo.Pro_SellTypeProduct.Any(p => p.SellType == 8)
                                                         ? c.Pro_ProInfo.Pro_SellTypeProduct.First(p => p.SellType == 8)
                                                            .Price
                                                         : 0)
                                                 //
                                             }).ToList();
                    foreach (var proSellListInfoTemp in Selllist_temp)
                    {
                        if (proSellListInfoTemp.AnBu > proSellListInfoTemp.ProPrice)
                        {
                            throw new   Exception("暗补金额不可超过单价");
                        }
                        switch (proSellListInfoTemp.SellType)
                        {
                            case 2:
                                {
                                    decimal TicketUsed = 0;
                                    string ZSREG = @"^S7[56]0(\d\d)(\d\d)(\d\d)([0-9A-Z]{8})$";
                                    Regex ex = new Regex(ZSREG);
                                    if (!ex.IsMatch(proSellListInfoTemp.TicketID))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "券编码格式不对");
                                    }
                                    MatchCollection match = ex.Matches(proSellListInfoTemp.TicketID);
                                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                                    //string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                                    decimal ticket = proSellListInfoTemp.CashTicket;

                                    if (!DAL.ValidClassInfo.IsDateTime(date))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "券编码格式不对");
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    decimal TicketUsed = 0;

                                    string ZSREG = @"^7[56]020(\d\d)(\d\d)(\d\d)(\d{11})(\+\d){0,1}$";
                                    string ZSREG2 = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{6})(\+\d){0,1}$";
                                    Regex ex = new Regex(ZSREG);
                                    Regex ex2 = new Regex(ZSREG2);
                                    Regex ok;
                                    if (!ex.IsMatch(proSellListInfoTemp.TicketID))
                                    {
                                        if (!ex2.IsMatch(proSellListInfoTemp.TicketID))
                                        {
                                            throw new Exception(proSellListInfoTemp.TicketID + "合约码或购机送费码格式不对");
                                        }
                                        else
                                        {
                                            ok = ex2;
                                        }
                                    }
                                    else
                                    {
                                        ok = ex;
                                    }
                                    MatchCollection match = ok.Matches(proSellListInfoTemp.TicketID);
                                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                                    decimal? ticket = proSellListInfoTemp.CashTicket;
                                    ;
                                    if (!DAL.ValidClassInfo.IsDateTime(date))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "编码格式不对");
                                    }


                                }
                                
                                break;
                            case 5:
                                {
                                    decimal TicketUsed = 0;
                                    string ZSREG = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{9})$";
                                    Regex ex = new Regex(ZSREG);
                                    if (!ex.IsMatch(proSellListInfoTemp.TicketID))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "券编码格式不对");
                                    }
                                    MatchCollection match = ex.Matches(proSellListInfoTemp.TicketID);
                                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                                    //string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                                    decimal ticket = proSellListInfoTemp.CashTicket;
                                    ;
                                    if (!DAL.ValidClassInfo.IsDateTime(date))
                                    {
                                        throw new Exception(proSellListInfoTemp.TicketID + "券编码格式不对");
                                    }
                                   
                                    break;
                                }
                            default:
                               
                                break;
                        }
                    }

                    if (Selllist_temp.Count() != model.Pro_SellListInfo.Count)
                    {
                        return new WebReturn() { ReturnValue = false,Message = "有商品未定价 无法保存"};
                    }
                    lqh.Umsdb.Pro_SellListInfo_Temp.InsertAllOnSubmit(Selllist_temp);
                    lqh.Umsdb.SubmitChanges();
                    return new WebReturn() { ReturnValue = true, Obj = Selllist_temp };





                }


            }
            catch (Exception ex)
            {
                return new WebReturn() {ReturnValue = false, Message = ex.Message};
                
            }



            return new WebReturn() {ReturnValue = false};

        }



        public Model.WebReturn Add_(Model.Sys_UserInfo user, Model.Pro_SellInfo model)
        {
            Model.WebReturn r = new WebReturn();
            try
            {
                using (LinQSqlHelper lqh=new LinQSqlHelper())
                {
                    var needdelete =
                        lqh.Umsdb.Pro_SellListInfo_Temp.Where(
                            p => model.Pro_SellListInfo.Select(q => q.ID).Contains(p.ID));
                    lqh.Umsdb.Pro_SellListInfo_Temp.DeleteAllOnSubmit(needdelete);
                    lqh.Umsdb.SubmitChanges();
                    return new WebReturn() {ReturnValue = true};
                }
            }
            catch( Exception ex)
            {
                return new WebReturn(){ReturnValue = false,Message = ex.Message};
            }
        }

        /// <summary>
        /// 新增销售 前台销售，单买 兑券等
        /// </summary>
        /// <remarks></remarks>
        /// <param name="sysUser"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model)
        {
            return Add(sysUser, model, new List<int>());
        }
        public Model.WebReturn Add(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model,List<int> tempidlist)
        {

            Model.WebReturn r = null;
            bool NoError = true;
            this._NewSellListInfo = model.Pro_SellListInfo;
            model.SellDate = DateTime.Now;
            model.SysDate = DateTime.Now;
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    DataLoadOptions dataload = new DataLoadOptions(); 

                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;

                    #region 验证会员信息

                    Model.VIP_VIPInfo vip;
                    if (model.VIP_ID != null && model.VIP_ID != 0)
                    {
                        var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                        where b.ID == model.VIP_ID
                                        select b;
                        if (!vip_query.Any())
                        {
                            return new Model.WebReturn() {ReturnValue = false, Message = "该会员不存在"};
                        }
                        vip = vip_query.First();
                         model.CusName = vip.MemberName;
                        model.CusPhone = vip.MobiPhone;
                    }
                    else
                    {
                        vip = null;
                    }
                    
                   
                    #endregion

                    #region 验证促销 操作人员

                    var user_seller= from b in lqh.Umsdb.Sys_UserInfo
                                              where  b.UserID == model.UserID && b.CanLogin==true && b.Flag==true
                                              select b;
                    if (user_seller.Count() == 0)
                    {
                        return new Model.WebReturn(){ ReturnValue=false, Message="操作员不存在"};
                    }

                    var user_Oper = from b in lqh.Umsdb.Sys_UserInfo
                                          where b.UserID == model.Seller && b.Flag==true
                                          select b;
                    if (user_Oper.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售员不存在" };
                    }
                    #endregion

                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r=ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS,ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                        return new Model.WebReturn() {  ReturnValue=false,  Message="仓库无权操作"};
                    #endregion

//                    #region 生产单号
//                    string SellID="";
//                    lqh.Umsdb.OrderMacker(1, "XS", "XS", ref SellID);
//                        if (SellID=="")
//                    {
//                        return new Model.WebReturn() { ReturnValue = false, Message ="销售单生成出错" };
//                    }
//                    model.SellID = SellID;
//
//                    #endregion

                    //存放串码
                    List<string> IMEI = new List<string>();
                    //存放赠品
                    //List<Model.Pro_SellSendInfo> sendList = new List<Model.Pro_SellSendInfo>();
                    //商品 销售类别
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放无串码
                    List<string> ProIDNoIMEI = new List<string>();
                    //存放优惠
                    List<int?> OffID_List = new List<int?>();

                    #region 获取当前订单包含的串号、商品编号
                    
                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        ////有商品限制，而且商品不在权限范围内
                        //if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        //{
                        //    NoError = false;
                        //    m.Note = "无权操作";
                        //    continue;
                        //}
                        if (!m.SellListInfoCheck(sysUser))
                        {
                            NoError = false;
                            m.Note = "菜单无权操作";
                            continue;
                        }
                        SellType_ProID_List.Add(m.SellType_Pro_ID);
                        //串码
                        if (!string.IsNullOrEmpty(m.IMEI))
                        {
                            if(IMEI.Contains(m.IMEI))
                            {
                               NoError=false;
                               m.Note=m.IMEI+"串码重复";
                               continue;
                            }
                            else 
                                IMEI.Add(m.IMEI);
                        }
                        
                        //商品编号
                        if (!ProIDNoIMEI.Contains(m.ProID) && string.IsNullOrEmpty(m.IMEI))
                            ProIDNoIMEI.Add(m.ProID);

                        #region 有赠品
                        //if (m.Pro_SellSendInfo.Count() > 0)
                        //{
                        //    sendList.AddRange(m.Pro_SellSendInfo);
                        //}
                        #endregion

                        if(m.OffID>0)
                            OffID_List.Add(m.OffID);
                    }
                    #endregion

                    //组合的
                    foreach (Model.Pro_SellSpecalOffList m in model.Pro_SellSpecalOffList)
                    {
                        if (m.SpecalOffID <= 0 || m.SpecalOffID == null)
                        {
                            NoError = false;
                            continue;
                            m.Note = "组合优惠有误";
                            continue;
                           
                        }
                        m.Pro_SellListInfo = null;
                            OffID_List.Add(m.SpecalOffID);
                        //sendList.AddRange(m.Pro_SellSendInfo);
                    }
                    //优惠券的
                    if(model.OffTicketID>0)
                        OffID_List.Add(model.OffTicketID);

                    //sendList.AddRange(model.Pro_SellSendInfo);

                    #region 有赠品，则获取串码 和 商品编号
                    //if (sendList.Count() > 0)
                    //{
                    //    foreach (Model.Pro_SellSendInfo m in sendList)
                    //    {
                    //        if (!string.IsNullOrEmpty(m.IMEI))
                    //        {
                    //            if (IMEI.Contains(m.IMEI))
                    //            {
                    //                NoError = false;
                    //                m.Note = m.IMEI + "串码重复";
                    //                continue;
                    //            }
                    //            else
                    //                IMEI.Add(m.IMEI);
                    //        }
                    //        if (!ProIDNoIMEI.Contains(m.ProID) && string.IsNullOrEmpty(m.IMEI))
                    //            ProIDNoIMEI.Add(m.ProID);
                    //    }
                    //}
                    #endregion

                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    }

                    //串码类拣货
                    var imeiList =( from b in lqh.Umsdb.Pro_IMEI
                                   where IMEI.Contains(b.IMEI) && b.HallID==model.HallID
                                   select b).ToList();
                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                    where b.HallID == model.HallID && ProIDNoIMEI.Contains(b.ProID) && b.ProCount>0
                                    orderby b.InListID
                                    select b).ToList();
                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                      group b by b.ProID into temp
                                      select temp.Single(p=>p.InListID==temp.Min(p2=>p2.InListID))).ToList();


                    var rulelist = lqh.Umsdb.Rules_AllCurrentRulesInfo.Where(p => p.HallID == model.HallID && p.StartDate<DateTime.Now&&p.EndDate>DateTime.Now);
                    


                    #region 验证优惠信息的有效性

                    List<Model.VIP_ProOffList> pro_off_list;
                    List<Model.Package_ProInfo> pro_group_list;
                    if (vip == null)
                    {
                        pro_off_list=(from b in lqh.Umsdb.VIP_ProOffList
                                        where (OffID_List.Contains(b.VIP_OffList.ID))

                                         && (((

                                                               (!b.VIP_OffList.VIP_VIPOffLIst.Any() && !b.VIP_OffList.VIP_VIPTypeOffLIst.Any())))
                                                              && (from c in b.VIP_OffList.VIP_HallOffInfo//门店专属优惠
                                                                  where c.HallID == model.HallID
                                                                  select c).Any()
                                                              )
                                                              && b.VIP_OffList.Flag == true
                                                            && b.VIP_OffList.EndDate >= DateTime.Now
                                                            && b.VIP_OffList.StartDate <= DateTime.Now
                                            // && new int?[] { 0, 1, 2, 3 }.Contains(b.VIP_OffList.Type)
                                                            && b.VIP_OffList.UseLimit < b.VIP_OffList.VIPTicketMaxCount
                                        orderby b.VIP_OffList.OffMoney descending
                                        select b).ToList();

                        pro_group_list = (from b in lqh.Umsdb.Package_ProInfo
                                        where (OffID_List.Contains(b.Package_GroupInfo.VIP_OffList.ID))

                                         && (((

                                                               (!b.Package_GroupInfo.VIP_OffList.VIP_VIPOffLIst.Any() && !b.Package_GroupInfo.VIP_OffList.VIP_VIPTypeOffLIst.Any())))
                                                              && (from c in b.Package_GroupInfo.VIP_OffList.VIP_HallOffInfo//门店专属优惠
                                                                  where c.HallID == model.HallID
                                                                  select c).Any()
                                                              )
                                                              && b.Package_GroupInfo.VIP_OffList.Flag == true
                                                            && b.Package_GroupInfo.VIP_OffList.EndDate >= DateTime.Now
                                                            && b.Package_GroupInfo.VIP_OffList.StartDate <= DateTime.Now
                                            // && new int?[] { 0, 1, 2, 3 }.Contains(b.VIP_OffList.Type)
                                                            && b.Package_GroupInfo.VIP_OffList.UseLimit < b.Package_GroupInfo.VIP_OffList.VIPTicketMaxCount
                                        orderby b.GroupID ascending
                                        select b).ToList();
                    }
                    else
                    {
                        pro_off_list = (from b in lqh.Umsdb.VIP_ProOffList
                                        where (OffID_List.Contains(b.VIP_OffList.ID))

                                         && ((((from c in b.VIP_OffList.VIP_VIPOffLIst //会员卡专属优惠
                                                where c.VIPID == model.VIP_ID
                                                select c).Any()
                                                              ||
                                                              (from c in b.VIP_OffList.VIP_VIPTypeOffLIst//会员类别专属优惠
                                                               where vip != null && c.VIPType == vip.TypeID
                                                               select c).Any()
                                                               ||
                                                               (!b.VIP_OffList.VIP_VIPOffLIst.Any() && !b.VIP_OffList.VIP_VIPTypeOffLIst.Any())

                                                              )
                                                              && (from c in b.VIP_OffList.VIP_HallOffInfo//门店专属优惠
                                                                  where c.HallID == model.HallID
                                                                  select c).Any()
                                                              ))
                                                              && b.VIP_OffList.Flag == true
                                                            && b.VIP_OffList.EndDate >= DateTime.Now
                                                            && b.VIP_OffList.StartDate <= DateTime.Now
                                            // && new int?[] { 0, 1, 2, 3 }.Contains(b.VIP_OffList.Type)
                                                            && b.VIP_OffList.UseLimit < b.VIP_OffList.VIPTicketMaxCount
                                        orderby b.VIP_OffList.OffMoney descending
                                        select b).ToList();
                        pro_group_list = (from b in lqh.Umsdb.Package_ProInfo
                                          where (OffID_List.Contains(b.Package_GroupInfo.VIP_OffList.ID))

                                         && ((((from c in b.Package_GroupInfo.VIP_OffList.VIP_VIPOffLIst //会员卡专属优惠
                                                where c.VIPID == model.VIP_ID
                                                select c).Any()
                                                              ||
                                                              (from c in b.Package_GroupInfo.VIP_OffList.VIP_VIPTypeOffLIst//会员类别专属优惠
                                                               where vip != null && c.VIPType == vip.TypeID
                                                               select c).Any()
                                                               ||
                                                               (!b.Package_GroupInfo.VIP_OffList.VIP_VIPOffLIst.Any() && !b.Package_GroupInfo.VIP_OffList.VIP_VIPTypeOffLIst.Any())

                                                              )
                                                              && (from c in b.Package_GroupInfo.VIP_OffList.VIP_HallOffInfo//门店专属优惠
                                                                  where c.HallID == model.HallID
                                                                  select c).Any()
                                                              ))
                                                              && b.Package_GroupInfo.VIP_OffList.Flag == true
                                                            && b.Package_GroupInfo.VIP_OffList.EndDate >= DateTime.Now
                                                            && b.Package_GroupInfo.VIP_OffList.StartDate <= DateTime.Now
                                            // && new int?[] { 0, 1, 2, 3 }.Contains(b.VIP_OffList.Type)
                                                            && b.Package_GroupInfo.VIP_OffList.UseLimit < b.Package_GroupInfo.VIP_OffList.VIPTicketMaxCount
                                        orderby b.GroupID  ascending
                                        select b).ToList();
                    }
                    var AllOffList = from b in pro_off_list
                                     select b.VIP_OffList;
                    #endregion

                    #region 生成组合优惠左连接部分
                    var Sepecial_ProOffList = (from b in model.Pro_SellSpecalOffList
                                               join c in pro_off_list
                                               on b.SpecalOffID equals c.OffID
                                               into temp
                                               from d in temp
                                               where d.VIP_OffList.Type == 1 //原组合优惠
                                               
                                               select new
                                               {
                                                   Pro_SellSpecalOffList = b,
                                                   pro_off_list = d  

                                               }).ToList();

                    var Group_ProOffList = (from b in model.Pro_SellSpecalOffList
                                            join c in pro_group_list
                                            on b.SpecalOffID equals c.Package_GroupInfo.VIP_OffList.ID
                                            into temp
                                            from d in temp
                                            where (d.Package_GroupInfo.VIP_OffList.Package_SalesNameInfo != null && d.Package_GroupInfo.VIP_OffList.Type >= 4)//新套餐优惠
                                            select new
                                            {
                                                Pro_SellSpecalOffList = b,
                                                pro_off_list = d

                                            }).ToList();

                    List<Model.VIP_OffList> vip_offList_Update = new List<Model.VIP_OffList>();
                    List<Model.Pro_SellSpecalOffList> SellSpecalOffList = new List<Model.Pro_SellSpecalOffList>();
                    #region 验证组合优惠是否有效
                    
                    
                    foreach (var s in Sepecial_ProOffList)
                    {
                        //组合优惠是否存在
                        if (s.pro_off_list == null)// && !SellSpecalOffList.Contains(s.Pro_SellSpecalOffList))
                        {
                            NoError = false;
                            //SellSpecalOffList.Add(s.Pro_SellSpecalOffList);
                            s.Pro_SellSpecalOffList.Note = "组合优惠不存在";
                            continue;
                        }
                        //有错误了，继续循环组合优惠，查找所有错误
                        if (!NoError)
                        {
                            continue;
                        }
                        if (!vip_offList_Update.Contains(s.pro_off_list.VIP_OffList))
                        {
                            vip_offList_Update.Add(s.pro_off_list.VIP_OffList);

                            if (s.pro_off_list.VIP_OffList.VIPTicketMaxCount != 99999 && s.pro_off_list.VIP_OffList.VIPTicketMaxCount - s.pro_off_list.VIP_OffList.UseLimit - 1 < 0)
                            {
                                NoError = false;
                                s.Pro_SellSpecalOffList.Note = "组合优惠名额超限";
                            }
                            else
                                s.pro_off_list.VIP_OffList.UseLimit++;
                        }
                    }


                    if (!NoError)//SellSpecalOffList.Count() > 0 ||
                    {
                        //Array.ForEach<Model.Pro_SellSpecalOffList>(SellSpecalOffList.ToArray(), p => { p.Note = "组合优惠不存在"; });
                        return new Model.WebReturn() { ReturnValue = false, Message = "组合优惠信息有误", Obj = model };
                    }

                    #endregion

                    #region 验证套餐优惠是否有效


                    foreach (var s in Group_ProOffList)
                    {
                        //组合优惠是否存在
                        if (s.pro_off_list == null)// && !SellSpecalOffList.Contains(s.Pro_SellSpecalOffList))
                        {
                            NoError = false;
                            //SellSpecalOffList.Add(s.Pro_SellSpecalOffList);
                            s.Pro_SellSpecalOffList.Note = "套餐优惠不存在";
                            continue;
                        }
                        //有错误了，继续循环组合优惠，查找所有错误
                        if (!NoError)
                        {
                            continue;
                        }
                        if (!vip_offList_Update.Contains(s.pro_off_list.Package_GroupInfo.VIP_OffList))
                        {
                            vip_offList_Update.Add(s.pro_off_list.Package_GroupInfo.VIP_OffList);

                            if (s.pro_off_list.Package_GroupInfo.VIP_OffList.VIPTicketMaxCount != 99999 && s.pro_off_list.Package_GroupInfo.VIP_OffList.VIPTicketMaxCount - s.pro_off_list.Package_GroupInfo.VIP_OffList.UseLimit - 1 < 0)
                            {
                                NoError = false;
                                s.Pro_SellSpecalOffList.Note = "套餐优惠名额超限";
                            }
                            else
                                s.pro_off_list.Package_GroupInfo.VIP_OffList.UseLimit++;
                        }
                    }


                    if (!NoError)//SellSpecalOffList.Count() > 0 ||
                    {
                        //Array.ForEach<Model.Pro_SellSpecalOffList>(SellSpecalOffList.ToArray(), p => { p.Note = "组合优惠不存在"; });
                        return new Model.WebReturn() { ReturnValue = false, Message = "套餐优惠信息有误", Obj = model };
                    }

                    #endregion

                    #endregion

                    #region 生成验证销售方式、各种价格左连接部分
                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where SellType_ProID_List.Contains(b.ID)
                                            select b).ToList();
                    #endregion

                     
                    #region 左连接单品优惠 组合优惠 销售方式


                    var join_query = (from b in model.Pro_SellListInfo
                                     join c in pro_off_list
                                     on new { b.ProID, b.SellType, b.OffID }
                                     equals
                                     new { c.ProID, SellType = c.SellTypeID, c.OffID }
                                     into temp1
                                     from c1 in temp1.DefaultIfEmpty()
                                     //where b.VIP_OffList.Type == 0
                                     join d in Sepecial_ProOffList
                                     on new { ID = b.SpecialID ?? 0, b.ProID, b.ProCount, b.SellType ,b.ProOffListID}
                                     equals
                                     new { ID = d.Pro_SellSpecalOffList.ID, d.pro_off_list.ProID, d.pro_off_list.ProCount, SellType = d.pro_off_list.SellTypeID, ProOffListID=(int?)d.pro_off_list.ID }
                                     into temp2
                                     from d1 in temp2.DefaultIfEmpty()

                                     join dd in Group_ProOffList
                                     on new { ID = b.SpecialID ?? 0,  b.SellType, b.ProOffListID }
                                     equals
                                     new { ID = dd.Pro_SellSpecalOffList.ID,  SellType = dd.pro_off_list.SellType, ProOffListID = (int?)dd.pro_off_list.ID }
                                     into tempdd
                                     from dd1 in tempdd.DefaultIfEmpty()

                                     join e in Pro_SellTypeList
                                     on b.SellType_Pro_ID equals e.ID
                                     into temp3
                                     from e1 in temp3.DefaultIfEmpty()
                                     join f in imeiList
                                     on b.IMEI equals f.IMEI
                                     into temp4
                                     from f1 in temp4.DefaultIfEmpty()
                                     join g in StoreList__
                                     on b.ProID equals g.ProID
                                     into temp5
                                     from g1 in temp5.DefaultIfEmpty()
                                    
                                     select new
                                     {
                                         Pro_SellListInfo = b,
                                         VIP_ProOffList_0 = c1,
                                         VIP_ProOffList_1 = d1 == null ? null : d1.pro_off_list,
                                         VIP_GroupProOffList_1=dd1==null? null:dd1.pro_off_list,
                                         Pro_SellTypeProduct = e1,
                                         Pro_IMEI = f1,
                                         Pro_StoreInfo = g1,
                                         Pro_SellSpecalOffList = d1 == null ? null : d1.Pro_SellSpecalOffList,
                                         d1,dd1

                                     }).ToList();
                    #endregion


                    var join_query2 =
                        model.Pro_SellListInfo.GroupJoin(pro_off_list, b => new {b.ProID, b.SellType, b.OffID},
                                                         c => new {c.ProID, SellType = c.SellTypeID, c.OffID},
                                                         (b, temp1) => new {b, temp1});
                    var temp9=join_query2.SelectMany(@t => @t.temp1.DefaultIfEmpty(), (@t, c1) => new {@t, c1});
                    var temp10=temp9.GroupJoin(Sepecial_ProOffList,
                                        @t =>
                                        new
                                            {
                                                @t.@t.b.Pro_SellSpecalOffList,
                                                @t.@t.b.ProID,
                                                @t.@t.b.ProCount,
                                                @t.@t.b.SellType
                                            },
                                        d =>
                                        new
                                            {
                                                d.Pro_SellSpecalOffList,
                                                d.pro_off_list.ProID,
                                                d.pro_off_list.ProCount,
                                                SellType = d.pro_off_list.SellTypeID
                                            }, (@t, temp2) => new {@t, temp2});
                    var temp11=temp10.SelectMany(@t => @t.temp2.DefaultIfEmpty(), (@t, d1) => new {@t, d1})
                             .GroupJoin(Pro_SellTypeList, @t => @t.@t.@t.@t.b.SellType_Pro_ID, e => e.ID,
                                        (@t, temp3) => new {@t, temp3});
                    var temp12=temp11.SelectMany(@t => @t.temp3.DefaultIfEmpty(), (@t, e1) => new {@t, e1})
                             .GroupJoin(imeiList, @t => @t.@t.@t.@t.@t.@t.b.IMEI, f => f.IMEI,
                                        (@t, temp4) => new {@t, temp4});
                    var temp13=temp12.SelectMany(@t => @t.temp4.DefaultIfEmpty(), (@t, f1) => new {@t, f1})
                             .GroupJoin(StoreList__, @t => @t.@t.@t.@t.@t.@t.@t.@t.b.ProID, g => g.ProID,
                                        (@t, temp5) => new {@t, temp5});
                    var temp14=temp13.SelectMany(@t => @t.temp5.DefaultIfEmpty(), (@t, g1) => new
                                 {
                                     Pro_SellListInfo = @t.@t.@t.@t.@t.@t.@t.@t.@t.b,
                                     VIP_ProOffList_0 = @t.@t.@t.@t.@t.@t.@t.@t.c1,
                                     VIP_ProOffList_1 = @t.@t.@t.@t.@t.@t.d1.pro_off_list,
                                     Pro_SellTypeProduct = @t.@t.@t.@t.e1,
                                     Pro_IMEI = @t.@t.f1,
                                     Pro_StoreInfo = g1,
                                     @t.@t.@t.@t.@t.@t.d1
                                 });

                    
                    
                        
                    List<Model.Pro_CashTicket> cashTickList = new List<Model.Pro_CashTicket>();
                    List<Model.Pro_SellListInfo> sellListTemp = new List<Model.Pro_SellListInfo>();
                    
                    List<string> str_ticket = new List<string>();
                    decimal? cashPrice = 0;
                    List<Model.Pro_IMEI> modifedIMEI=new List<Pro_IMEI>();

                    foreach (var child in join_query)
                    {
                        Sepecial_ProOffList.Remove(child.d1);
                        //SellType_ProID_List.Add(child.SellType_Pro_ID);
                        //ProID.Add(child.Pro_SellListInfo.ProID);
                        //OffID_List.Add(child.OffID);

                        #region 验证单品优惠 组合优惠 商品信息 销售类别 有无串码 单价


                        //验证单品优惠
                        //if (child.Pro_SellListInfo.OffID > 0)
                        //{
                        //    if (child.VIP_ProOffList_0 == null || child.VIP_ProOffList_0.VIP_OffList == null)
                        //    {
                        //        NoError = false;
                        //        child.Pro_SellListInfo.Note = "单品优惠不存在或已过期";
                        //        continue;
                        //    }
                        //}
                        //验证组合优惠
                        //if (child.Pro_SellListInfo.SpecialID > 0)
                        //{
                        //    if (child.VIP_ProOffList_1 == null || child.VIP_ProOffList_1.VIP_OffList == null)
                        //    {
                        //        NoError = false;
                        //        child.Pro_SellListInfo.Note = "组合优惠不存在或已过期或不满足条件";
                        //        child.Pro_SellListInfo.Pro_SellSpecalOffList.Note = "组合优惠不存在或已过期或不满足条件";
                        //        continue;
                        //    }
                        //}
                        //验证销售类别、商品信息
                        if (child.Pro_SellTypeProduct == null || child.Pro_SellTypeProduct.Pro_ProInfo == null)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品不存在或销售类别不正确";
                            continue;
                        }
                        if (child.Pro_SellTypeProduct.Pro_ProInfo.NeedIMEI == true &&
                            string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品缺少串码";
                            continue;
                        }
                        if (child.Pro_SellTypeProduct.Pro_ProInfo.NeedIMEI != true &&
                            !string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "属于无串码商品";
                            continue;
                        }

                        if (
                            child.Pro_SellTypeProduct.Pro_ProInfo.Pro_ClassInfo != null &&
                            child.Pro_SellTypeProduct.Pro_ProInfo.Pro_ClassInfo.Pro_ClassType != null &&
                            child.Pro_SellTypeProduct.Pro_ProInfo.Pro_ClassInfo.Pro_ClassType.ID == 1 &&
                            child.Pro_SellListInfo.YanbaoModelPrice == 0)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "终端的单卖价未定价";
                            continue;
                        }
                        #region 合同
                        if (child.Pro_SellListInfo.Pro_BillInfo != null)
                        {
                            #region 驗證終端串碼

                            var mobileimeiquery = lqh.Umsdb.Pro_IMEI.Where(p => p.IMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI);
                            if (mobileimeiquery.Any())
                            {
                                var imeiinfo = mobileimeiquery.First();
                                if ((imeiinfo.Pro_ProInfo.Pro_ClassTypeID ??
                                     imeiinfo.Pro_ProInfo.Pro_ClassInfo.ClassTypeID) != 1
                                    &&
                                    (imeiinfo.Pro_ProInfo.Pro_ClassTypeID ??
                                     imeiinfo.Pro_ProInfo.Pro_ClassInfo.ClassTypeID) != 5
                                    )
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "对应终端串码不是终端";
                                    continue;
                                }
                            }
                            else
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "对应终端串码不存在";
                                continue;
                            }
                            #endregion

                            #region 判断是否冲突

//                            if (
//                                lqh.Umsdb.Pro_BillInfo.All(
//                                    p =>
//                                        p.MobileIMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI &&
//                                         p.Pro_SellListInfo.Pro_SellBackList.Count == 0 &&
//                                        lqh.Umsdb.Pro_BillConflictInfo.Where(
//                                            q => q.ProID == child.Pro_SellListInfo.ProID)
//                                            .All(w => w.ProID_NotConflict != p.ProID))){}
                            bool flag = true;
                            var imeilist =
                                lqh.Umsdb.Pro_BillInfo.Where(
                                    p =>
                                        p.MobileIMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI &&
                                         p.Pro_SellListInfo.BackID == null).Select(o => o.BillIMEI);
                            var proidlist = lqh.Umsdb.Pro_SellListInfo.Where(
                                p => imeilist.Contains(p.IMEI))
                                .GroupBy(o => o.IMEI)
                                .Select(infos => infos.OrderByDescending(i => i.ID).First()).Where(u => u.Pro_SellBackList.Count == 0).Select(y=>y.ProID);
                            foreach (var VARIABLE in proidlist)
                            {
                                if (lqh.Umsdb.Pro_BillConflictInfo.Any(
                                    q => q.ProID == child.Pro_SellListInfo.ProID && q.ProID_NotConflict == VARIABLE))
                                {

                                }
                                else
                                {
                                    flag = false;
                                }
                            }

                        if (   flag)

                            {
                                #region 对应终端已销售
                                if (
                                    lqh.Umsdb.Pro_SellListInfo.Any(
                                        p => p.IMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI
                                        && p.Pro_SellBackList.Count == 0 && p.BackID == null))
                                {
                                    var temp = lqh.Umsdb.Pro_SellListInfo.OrderByDescending(p => p.ID).First(
                                        p => p.IMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI && p.BackID == null
                                        && p.Pro_SellBackList.Count == 0);
                                    if (temp.Pro_SellInfo == null)
                                    {
                                        NoError = false;
                                        child.Pro_SellListInfo.Note = "对应终端已销售但未审批";
                                        continue;
                                    }
                                    

                                }
                                
                                #endregion
                                #region 对应终端未销售
                                else
                                {
                                    if (
                                        model.Pro_SellListInfo.Any(
                                            p => p.IMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI
                                            && p.Pro_SellBackList.Count == 0))
                                    {
                                       
                                    }
                                    else
                                    {
                                        NoError = false;
                                        child.Pro_SellListInfo.Note = "商品未销售 无法建立合同";
                                        continue;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "合同冲突";
                                continue;
                            }
                            #endregion
                        }
                        #endregion
                        else

                    if (child.Pro_SellListInfo.Pro_Sell_Yanbao == null)
                        {
                            if (child.Pro_SellListInfo.VIP_VIPInfo == null)
                            {
                                if (!string.IsNullOrEmpty(child.Pro_SellListInfo.ChargePhoneNum))
                                {
                                    if (child.Pro_SellTypeProduct.MinPrice > child.Pro_SellListInfo.ProPrice ||
                                   child.Pro_SellTypeProduct.MaxPrice < child.Pro_SellListInfo.ProPrice)
                                    {
                                        NoError = false;
                                        child.Pro_SellListInfo.Note = "商品的单价有误";
                                        continue;
                                    }
                                }
                                else if  (child.Pro_SellTypeProduct.Price != child.Pro_SellListInfo.ProPrice &&
                                    child.Pro_SellListInfo.NeedAduit == false)
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "商品的单价有误";
                                    continue;
                                }
                            }
                            else
                            {
                                var viptypeinfo =
                                    lqh.Umsdb.VIP_VIPType.Where(p => child.Pro_SellListInfo.VIP_VIPInfo.TypeID == p.ID);
                                if (viptypeinfo.Any())
                                {
                                    var viptype = viptypeinfo.First();
                                    if (child.Pro_SellListInfo.ProPrice != viptype.Cost_production)
                                    {
                                        NoError = false;
                                        child.Pro_SellListInfo.Note = "商品的单价有误";
                                        continue;
                                    }
                                }
                                else
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "会员卡的单价错误";
                                    continue;
                                }
                            }
                        }
                        #region 延保
                        else
                        {
                            #region 驗證終端串碼

                            var mobileimeiquery = lqh.Umsdb.Pro_IMEI.Where(p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI);
                            if (mobileimeiquery.Any())
                            {
                                var imeiinfo = mobileimeiquery.First();
                                if ((imeiinfo.Pro_ProInfo.Pro_ClassTypeID ??
                                     imeiinfo.Pro_ProInfo.Pro_ClassInfo.ClassTypeID) != 1

                                    &&
                                    (imeiinfo.Pro_ProInfo.Pro_ClassTypeID ??
                                     imeiinfo.Pro_ProInfo.Pro_ClassInfo.ClassTypeID) != 5
                                    )
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "对应终端串码不是终端";
                                    continue;
                                }
                            }
                            else
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "对应终端串码不存在";
                                continue;
                            }
                            #endregion
                            #region 判断是否已销售过延保
                            if (
                                lqh.Umsdb.Pro_Sell_Yanbao.Any(
                                    p =>
                                    p.MobileIMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI &&

                                    p.Pro_SellListInfo.Pro_SellBackList.Count == 0 && p.Pro_SellListInfo.BackID == null))
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "该商品已销售过延保";
                                continue;
                            }
                            else
                            {
                                decimal modelprice = 0;
                                int? selltype = 0;
                                DateTime? selldate;
                                #region 该商品已销售
                                if (
                                    lqh.Umsdb.Pro_SellListInfo.Any(
                                        p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI && p.Pro_SellBackList.Count == 0 && p.BackID == null))
                                {
                                    var temp = lqh.Umsdb.Pro_SellListInfo.OrderByDescending(p=>p.ID).First(
                                        p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI && p.BackID==null
                                        && p.Pro_SellBackList.Count == 0
                                        );
                                    if (temp.Pro_SellInfo == null)
                                    {
                                        NoError = false;
                                        child.Pro_SellListInfo.Note = "延保对应终端已销售但未审批";
                                        continue;
                                    }
                                    modelprice = temp.YanbaoModelPrice ;
                                    selltype = temp.SellType;
                                    selldate = temp.Pro_SellInfo.SellDate;

                                }
                                
                                #endregion
                                #region 该商品未销售
                                else
                                {
                                    if (
                                        model.Pro_SellListInfo.Any(
                                            p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI
                                            && p.Pro_SellBackList.Count == 0))
                                    {
                                        var temp = model.Pro_SellListInfo.First(
                                            p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI
                                            && p.Pro_SellBackList.Count == 0);
                                        modelprice = temp.YanbaoModelPrice;
                                        selltype = temp.SellType;
                                        selldate = temp.Pro_SellInfo.SellDate;
                                    }
                                    else
                                    {
                                        NoError = false;
                                        child.Pro_SellListInfo.Note = "商品未销售 无法购买延保";
                                        continue;
                                    }
                                }
                                #endregion

                                var IMEItemp =
                                    lqh.Umsdb.Pro_IMEI.First(
                                        p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI);
                                if (lqh.Umsdb.Pro_YanbaoPriceStepInfo.Any(p => p.ID == IMEItemp.Pro_ProInfo.YanBaoModelID))
                                {
                                    #region 商品有指定延保价格的 
                                    if (child.Pro_SellListInfo.ProPrice !=
                                        lqh.Umsdb.Pro_YanbaoPriceStepInfo.First(
                                            p => p.ID == IMEItemp.Pro_ProInfo.YanBaoModelID).ProPrice)
                                    {
                                        NoError = false;
                                        child.Pro_SellListInfo.Note = "商品的单价有误";
                                        continue;
                                    }
                        #endregion
                                }
                                else
                                {
                                    if (
                                        lqh.Umsdb.Pro_YanbaoPriceStepInfo.Any(
                                            p => p.ProID == child.Pro_SellListInfo.ProID && p.StepPrice >= modelprice))
                                    {
                                        var yanbostep = lqh.Umsdb.Pro_YanbaoPriceStepInfo.OrderBy(p=>p.StepPrice).First(
                                            p => p.ProID == child.Pro_SellListInfo.ProID && p.StepPrice >= modelprice);
                                        if (yanbostep.ProPrice !=
                                            child.Pro_SellListInfo.ProPrice)
                                        {
                                            NoError = false;
                                            child.Pro_SellListInfo.Note = "商品的单价有误";
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        NoError = false;
                                        child.Pro_SellListInfo.Note = "延保无法定价 无法销售";
                                        continue;
                                    }
                                }

                                child.Pro_SellListInfo.Pro_Sell_Yanbao.SellType = selltype;
                                child.Pro_SellListInfo.Pro_Sell_Yanbao.MobilePrice = modelprice;
                                child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileDate = selldate;
                               

                            }
                            #endregion

                        }
                        #endregion

                        #region 机配卡
                        if (child.Pro_SellListInfo.Pro_Sell_JiPeiKa != null)
                        {
                            if (!IMEI.Contains(child.Pro_SellListInfo.Pro_Sell_JiPeiKa.IMEI))
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "机配卡对应串码不在本次销售中";
                                continue;
                            }
                            if (
                                model.Pro_SellListInfo.Where(p => p.Pro_Sell_JiPeiKa != null)
                                     .Where(p => p.Pro_Sell_JiPeiKa.IMEI == child.Pro_SellListInfo.Pro_Sell_JiPeiKa.IMEI)
                                     .Count() > 1)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "机配卡对应串码重复";
                                continue;
                            }
                        }
                        #endregion

                        #endregion

                        if (!NoError)
                            continue;

                        child.Pro_SellListInfo.LowPrice=child.Pro_SellTypeProduct.LowPrice;
                        child.Pro_SellListInfo.ProCost=child.Pro_SellTypeProduct.ProCost;
                        
                        

                        #region 验证TicketID
//                        if (child.Pro_SellListInfo.SellType==2){
//                        
                        {
                            
                            r = CheckProCashTicket2(child.Pro_SellListInfo, child.Pro_SellTypeProduct.Pro_ProInfo);
                            if (r.ReturnValue != true)
                            {
                                NoError = false;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(child.Pro_SellListInfo.TicketID)){
                                if (child.Pro_SellTypeProduct.IsTicketUseful != true)
                                {
                                    child.Pro_SellListInfo.Note = "涉及到兑券码/合约码等编码的机型需要在价格管理中设置可兑券";
                                    NoError = false;
                                    continue;
                                }
                            cashTickList.Add(new Model.Pro_CashTicket()
                                {
                                    Pro_SellListInfo = child.Pro_SellListInfo,
                                    TicketID = child.Pro_SellListInfo.TicketID
                                });

                            if (str_ticket.Contains(child.Pro_SellListInfo.TicketID))
                            {
                                NoError = false;

                                child.Pro_SellListInfo.Note = child.Pro_SellListInfo.TicketID + "代金券重复";

                                return new Model.WebReturn()
                                    {
                                        Obj = model,
                                        ReturnValue = false,
                                        Message = child.Pro_SellListInfo.Note
                                    };

                            }
                            str_ticket.Add(child.Pro_SellListInfo.TicketID);
                            }
                        }

                        #endregion

                        #region 单品优惠验证

                        child.Pro_SellListInfo.VIP_OffList = child.VIP_ProOffList_0 == null
                                                                 ? null
                                                                 : child.VIP_ProOffList_0.VIP_OffList;

                        r = CheckProOff(child.Pro_SellListInfo);
                        if (r.ReturnValue != true)
                        {
                            NoError = false;
                            continue;
                        }

                        #endregion

                        #region 验证组合优惠
                        child.Pro_SellListInfo.Pro_SellSpecalOffList = child.Pro_SellSpecalOffList;
                        r = CheckSpecialOff(child.Pro_SellListInfo, child.VIP_ProOffList_1,child.VIP_GroupProOffList_1);
                        if (r.ReturnValue != true)
                        {
                            NoError = false;
                            continue;
                        }
                        
                        #endregion

                        #region 验证套餐优惠是否存在
		                if(child.VIP_ProOffList_1==null && child.Pro_SellListInfo.SpecialID>0)
                        {
                            if(child.VIP_GroupProOffList_1==null)
                            {
                                child.Pro_SellListInfo.Note="套餐优惠不存在";
                                NoError = false;
                                continue;
                            }
                            else if (child.VIP_GroupProOffList_1.ProMainNameID != child.Pro_SellTypeProduct.Pro_ProInfo.Pro_ProMainInfo.Pro_ProNameInfo.ID)
                            {
                                child.Pro_SellListInfo.Note = "该商品不能参加" + child.VIP_GroupProOffList_1.Package_GroupInfo.Package_GroupTypeInfo.GroupName + "的套餐优惠";
                                NoError = false;
                                continue;
                            }
                            else if (child.Pro_SellListInfo.AnBu != 0 ||
                                child.Pro_SellListInfo.AnBuPrice != 0 ||
                                child.Pro_SellListInfo.LieShou != 0 ||
                                child.Pro_SellListInfo.LieShouPrice != 0 ||
                                child.Pro_SellListInfo.OffPrice != 0 ||
                                child.Pro_SellListInfo.OffPoint != 0 ||
                                child.Pro_SellListInfo.OtherOff != 0 || 
                                child.Pro_SellListInfo.WholeSaleOffPrice != 0)
                            {
                                child.Pro_SellListInfo.Note = "套餐优惠不能存在暗补、列收、单品优惠、门店优惠等";
                                NoError = false;
                                continue;
                            }
                            child.Pro_SellListInfo.Salary = child.VIP_GroupProOffList_1.Salary;
                        }
	                        #endregion

                        #region 验证门店优惠
                        if (child.Pro_SellListInfo.OtherOff!=0)
                        {
                            var promain =
                                lqh.Umsdb.Pro_ProInfo.First(p => p.ProID == child.Pro_SellListInfo.ProID).ProMainID;
                        if (
                            !lqh.Umsdb.Off_AduitProInfo.Any(
                                p => p.Off_AduitTypeInfo != null && p.Off_AduitTypeInfo.StartDate < DateTime.Now &&
                                     p.Off_AduitTypeInfo.EndDate > DateTime.Now && p.Off_AduitTypeInfo.Flag &&
                                     p.Off_AduitTypeInfo.Off_AduitHallInfo.Select(q => q.HallID).Contains(model.HallID)
                                     && p.ProMainID == promain && p.Price >= child.Pro_SellListInfo.OtherOff && p.SellType == child.Pro_SellListInfo.SellType
                                 ))
                        {
                            return new WebReturn()
                                {
                                    ReturnValue = false,
                                    Message = "门店优惠无效"
                                };
                        }
                       


                            }
                        #endregion

                        #region 验证暗补
                        if (child.Pro_SellListInfo.AnBuPrice !=
                            ((child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.TicketUsed) <
                            child.Pro_SellListInfo.AnBu
                                ? (child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.TicketUsed)
                                : child.Pro_SellListInfo.AnBu))
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "暗补计算错误";
                            continue;
                        }

                        #endregion

                        #region 验证规则

                        decimal rules = 0;
                        if (child.Pro_SellListInfo.Pro_SellList_RulesInfo.Count > 0)
                        {
                            //child.Pro_SellListInfo.NeedAduit=true;
                            foreach (var proSellListRulesInfo in child.Pro_SellListInfo.Pro_SellList_RulesInfo)
                            {
                                var rule =
                                    rulelist.Where(p => p.Rules_ProMain_ID == proSellListRulesInfo.Rules_ProMain_ID);
                                if (!rule.Any())
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "规则有误";
                                    continue;
                                }
                                var rulemodel = rule.First();
                                if (child.Pro_SellTypeProduct.Pro_ProInfo.ProMainID != rulemodel.ProMainID ||
                                    proSellListRulesInfo.OffPrice > rulemodel.MaxPrice ||
                                    proSellListRulesInfo.OffPrice < rulemodel.MinPrice ||
                                    proSellListRulesInfo.OffPrice < proSellListRulesInfo.RealPrice
                                    )
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "规则有误";
                                    continue;
                                }

                                rules += proSellListRulesInfo.RealPrice;
                                if (proSellListRulesInfo.ShowToCus)
                                {
                                    child.Pro_SellListInfo.RulesShowToCus += proSellListRulesInfo.RealPrice;
                                }
                                else
                                {
                                    child.Pro_SellListInfo.RulesUnShowToCus += proSellListRulesInfo.RealPrice;
                                }
                                if (proSellListRulesInfo.CanGetBack)
                                {
                                    child.Pro_SellListInfo.RulesGetBack += proSellListRulesInfo.RealPrice;

                                }
                                else
                                {
                                    child.Pro_SellListInfo.RulesUnGetBack += proSellListRulesInfo.RealPrice;
                                }

                            }
                            
                        }

                        #endregion

                        #region 验证实收

                        decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.AnBuPrice -
                                        child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice - child.Pro_SellListInfo.OtherOff -
                                        child.Pro_SellListInfo.OffSepecialPrice;
                        if (real < 0) real = 0;
                        real = real +child.Pro_SellListInfo.OtherCash;
                        if (real < child.Pro_SellListInfo.LieShouPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "列收金额不允许超过销售金额";
                            continue;
                        }
                        if (real < rules)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "规则计算有误";
                            continue;
                            
                        }
                        
                        real = real - rules;
                        if (real  != child.Pro_SellListInfo.CashPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收计算有误";
                            continue;
                        }
                        #endregion

//                        #region 验证列收
//                        if ((child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.AnBu -
//                             child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice -
//                             child.Pro_SellListInfo.LieShou) < 0)
//                        {
//                            decimal temp = child.Pro_SellListInfo.LieShou -
//                                           (0 - (child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.AnBu -
//                                                 child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice -
//                                                 child.Pro_SellListInfo.LieShou));
//                            if (temp < 0) temp = 0;
//                            if (temp != child.Pro_SellListInfo.LieShouPrice)
//                            {
//                                NoError = false;
//                                child.Pro_SellListInfo.Note = "列收计算有误";
//                                continue;
//                            }
//                        }
//                        else
//                        {
//                            if (child.Pro_SellListInfo.LieShou != child.Pro_SellListInfo.LieShouPrice)
//                            {
//                                NoError = false;
//                                child.Pro_SellListInfo.Note = "列收计算有误";
//                                continue;
//                            }
//                        }
//                        #endregion

                        cashPrice += child.Pro_SellListInfo.CashPrice*child.Pro_SellListInfo.ProCount;
                        //cashPrice += child.Pro_SellListInfo.OtherCash*child.Pro_SellListInfo.ProCount;
                       // cashPrice -= child.Pro_SellListInfo.LieShouPrice*child.Pro_SellListInfo.ProCount;

                        #region 串码类验证
                        if (!string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r= CheckIMEI(child.Pro_IMEI);
                            if(!r.ReturnValue)
                            {
                                NoError=false;
                                child.Pro_SellListInfo.Note=r.Message;
                                continue;
                            }
                            child.Pro_SellListInfo.InListID = child.Pro_IMEI.InListID;
                            child.Pro_SellListInfo.ProCost = child.Pro_IMEI.Pro_InOrderList.InitInList.Price;
                            child.Pro_IMEI.Pro_StoreInfo.ProCount--;
                            child.Pro_IMEI.Pro_SellInfo = model;
                            modifedIMEI.Add(child.Pro_IMEI);
                        }
                        #endregion

                        

                        #region 非串码类验证
                        if (string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {


                           r= FitInOrderListIDNoIMEI(child.Pro_SellListInfo, child.Pro_StoreInfo, sellListTemp);
                           if (!r.ReturnValue == true)
                           {
                               NoError = false;
                               continue;
                           }
                        }
                        #endregion


                        #region 计算提成

                        if (child.Pro_SellListInfo.Salary == 0 || child.Pro_SellListInfo.Salary == null)
                            //无套餐提成
                        {
                            var today = DateTime.Today;
                            var query = lqh.Umsdb.Sys_SalaryCurrentList.Where(
                                p =>
                                    p.SalaryYear == today.Year && p.SalaryMonth == today.Month &&
                                    p.SalaryDay == today.Day)
                                .Where(
                                    o =>
                                        o.ProID == child.Pro_SellListInfo.ProID &&
                                        o.SellType == child.Pro_SellListInfo.SellType);
                            query =
                                query.Where(
                                    p =>
                                        p.Min >= (child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed) &&
                                        p.Max < (child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed));
                            if (query.Any())
                            {
                                var salarymodel = query.First();
                                decimal childsalary = 0;
                                if (salarymodel.BaseSalary > 0)
                                {
                                    childsalary += salarymodel.BaseSalary;
                                }
                                else
                                {
                                    childsalary += Convert.ToDecimal(child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed)*
                                                   salarymodel.SpecialSalary;
                                }
                                if ((child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed) >
                                    salarymodel.Ratio)
                                {
                                    childsalary += Convert.ToDecimal(child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed - salarymodel.Ratio) *
                                                   salarymodel.OverRatio;
                                }
                                child.Pro_SellListInfo.Salary = childsalary;

                            }
                        }
                        #endregion

                    }

                    foreach (var source in model.Pro_SellListInfo.Where(p=>p.ProCount==0).ToList())
                    {
                        model.Pro_SellListInfo.Remove(source);
                    }
                    if (!NoError)//有错误
                    {
                        return new Model.WebReturn() {  ReturnValue=false, Message="提交有误", Obj=model};
                    }

                    if (Sepecial_ProOffList.Count > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "部分组合优惠没有满足条件" };
                    }
                   
                    #region 套餐验证
                  
                    var join_query_group = from b in join_query
                                           where b.VIP_GroupProOffList_1!=null
                                           group b by new { b.dd1.Pro_SellSpecalOffList,b.dd1.pro_off_list.Package_GroupInfo.VIP_OffList }
                                               into g
                                               select new { 
                                                g.Key.Pro_SellSpecalOffList,
                                                g.Key.VIP_OffList,
                                                join_queryList =g.OrderBy(p=>p.VIP_GroupProOffList_1.Package_GroupInfo.ID)
                                               };
                    foreach (var m in join_query_group)
                    {
                        if (m.VIP_OffList.Package_GroupInfo.Any(p=>p.IsMust==true &&  !m.join_queryList.Any(x => x.VIP_GroupProOffList_1.Package_GroupInfo.ID == p.ID)))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "套餐优惠缺少必选组", Obj = model };
                        }

                        var firstModel = m.join_queryList.First();//用于分摊的第一组商品，终端
                        //除去第一组商品外的其他商品的总价
                        //var Pakageother = m.join_queryList.Where(p=> p!=firstModel).Sum(p=>p.Pro_SellListInfo.ProPrice );
                        ////终端的套餐价格
                        //var AfterOffPrice = m.VIP_OffList.ArriveMoney - Pakageother;

                        decimal pakageAll = 0;//套餐总价
                        decimal pakageAllOff = 0;//套餐总优惠了
                        decimal TicketOff = 0 ;
                        foreach (var x in m.join_queryList)
                        {
                            var model__ = x.Pro_SellListInfo;
                            if (x != firstModel && model__.OtherCash != 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "多收金额只能分摊到第一组商品上", Obj = model };
                            }
                            model__.Pro_SellSpecalOffList = x.dd1.Pro_SellSpecalOffList;
                            pakageAllOff += model__.OffSepecialPrice;
                            pakageAll += model__.ProPrice - model__.TicketUsed - model__.OffSepecialPrice + model__.OtherCash + model__.Pro_SellList_RulesInfo.Sum(p=>p.RealPrice);
                            if (model__.TicketUsed>0){
//                                if (x == firstModel)
//                                {
//                                    var mprice = m.VIP_OffList.ArriveMoney -
//                                                 m.join_queryList.Where(p => p != firstModel)
//                                                     .Sum(d => d.Pro_SellListInfo.CashPrice);
//                                    TicketOff += mprice -
//                                                 (model__.ProPrice - model__.TicketUsed - model__.OffSepecialPrice +
//                                                  model__.OtherCash);
//                                }
//                                else
                                {
                                    TicketOff += model__.TicketUsed;
                                }
                                }
                        }
                        if (pakageAll+TicketOff < m.VIP_OffList.ArriveMoney)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "套餐总价必须为"+m.VIP_OffList.ArriveMoney+"元", Obj = model };
                        }
                        m.Pro_SellSpecalOffList.OffMoney = pakageAllOff;
                    }
                    #endregion

                    if (cashPrice != model.CashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误", Obj = model };
                    }
                    #region 存在未拣完货的
                    if (sellListTemp.Count > 0)
                    {
                        r = FitInOrderListIDNoIMEI( sellListTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        
                        
                    }
                    #endregion

                    #region 验证赠品
                    //if (sendList.Count() > 0)
                    //{
                    //    List<Model.VIP_SendProOffList> SendProOffList = new List<Model.VIP_SendProOffList>();
                    //    List<Model.Pro_SellSendInfo> sendTempList = new List<Model.Pro_SellSendInfo>();
                    //    foreach (Model.VIP_OffList off in AllOffList)
                    //    {
                    //        SendProOffList.AddRange(off.VIP_SendProOffList);
                    //    }
                    //    var aa=from b in sendList
                    //           group b by new{b.OffID,b.ProID}

                    //           into temp
                    //           from c in temp
                    //           select new{ c.OffID,c.ProID,ProCount=temp.Sum(p=>p.ProCount)};
                    //    var send_join_SendProoff=from b in aa
                    //                             join c in SendProOffList
                    //                             on new { b.OffID, b.ProID, PerCount = b.ProCount }
                    //                             equals new{c.OffID,c.ProID,c.PerCount}
                    //                             into temp
                    //                             from d in temp.DefaultIfEmpty()
                    //                             select new {b,d};
                    //    foreach (var m in send_join_SendProoff)
                    //    {
                    //        if (m.d == null || m.b.ProCount + m.d.ProCount > m.d.LimitCount)
                    //        {
                                
                    //            return new Model.WebReturn { ReturnValue = false, Obj = model, Message = "赠品列表有误" };
                    //        }
                    //        else
                    //        {
                    //            m.d.ProCount += m.d.PerCount;
                    //        }
                    //    }
                    //    StoreList__ = (from b in StoreList
                    //                   where b.ProCount > 0
                    //                   group b by b.ProID into temp
                    //                   select temp.Single((p => p.InListID == temp.Min(p2 => p2.InListID)))).ToList();
                    //    var send_join_pro_store = from b in sendList
                    //                              join c in imeiList
                    //                              on b.IMEI equals c.IMEI
                    //                              into temp
                    //                              from d in temp.DefaultIfEmpty()
                    //                              join e in StoreList__
                    //                              on b.ProID equals e.ProID
                    //                              into temp2
                    //                              from e1 in temp2.DefaultIfEmpty()
                    //                              join f in SendProOffList
                    //                              on new { b.OffID, b.ProID }
                    //                              equals new{f.OffID,f.ProID}
                    //                              into temp3
                    //                              from f1 in temp3.DefaultIfEmpty()
                    //                              select new { b,d,e1,f1};

                    //    foreach (var m in send_join_pro_store)
                    //    {
                    //        #region 有串码
                    //        if (!string.IsNullOrEmpty(m.b.IMEI))
                    //        {
                    //            m.b.ProCount = 1;
                    //            r = CheckIMEI(m.d);
                    //            if (!r.ReturnValue == true)
                    //            {
                    //                NoError = false;
                    //                m.b.Note = r.Message;
                    //                continue;
                    //            }
                    //            if (!NoError) continue;
                    //            else
                    //            {
                    //                if (m.d.Pro_StoreInfo.ProCount < 1)
                    //                {
                    //                    NoError = false;
                    //                    m.b.Note = "库存不够";
                    //                    continue;
                    //                }
                    //                m.d.Pro_StoreInfo.ProCount--;
                    //                m.d.Pro_SellInfo = model;
                    //                m.b.ProCost=m.f1.ProCost;
                    //            }

                    //        }
                    //        #endregion
                    //        #region 无串码
                    //        else
                    //        {
                    //           r=  FitInOrderListIDNoIMEI(m.b, m.e1, sendTempList);
                    //           if (!r.ReturnValue == true)
                    //           {
                    //               NoError = false;
                    //               continue;
                    //           }
                    //        }
                    //        #endregion
                           
                    //    }
                    //    #region 存在未拣完货的赠品

                    //    if (sendTempList.Count > 0)
                    //    {
                    //        r = FitInOrderListIDNoIMEI(sendTempList, StoreList);
                    //        if (r.ReturnValue != true)
                    //        {
                    //            r.Obj = model;
                    //            return r;
                    //        }
                    //    }

                    //    #endregion
                    //}
                    #endregion

                    #region 保存代金券
//                    var a = from b in lqh.Umsdb.Pro_CashTicket
//                            where str_ticket.Contains(b.TicketID) && b.IsBack != true
//                            select b;
                    var a = lqh.Umsdb.Pro_CashTicket.Where(p => str_ticket.Contains(p.TicketID) && (p.IsBack==null||p.IsBack==false));
                    if (a.Count() > 0)
                        return new Model.WebReturn() { ReturnValue = false, Message = a.First().TicketID + "代金券已被使用" };
                    lqh.Umsdb.Pro_CashTicket.InsertAllOnSubmit(cashTickList);
                    #endregion



                    #region 旧代码
                    
                    


                    

                    //验证销售方式、各种价格


                    //var selllist_query = CheckProSellType(model.Pro_SellListInfo.ToList(), SellType_ProID_List, lqh);

                    //var null_query = selllist_query.Where(p => p.Pro_SellTypeProduct==null || p.ProPrice !=p.Pro_SellTypeProduct.Price);
                    //if (null_query.Count() > 0)
                    //{
                    //    model.Pro_SellListInfo.Clear();
                    //    model.Pro_SellListInfo.AddRange(selllist_query);
                    //    return new Model.WebReturn() { Obj = model, ReturnValue = false, Message = null_query.First().ProID + "商品信息或销售方式错误" };
                    //}


                    //创建添加到数据库的 新 实体
                    //Model.Pro_SellInfo InsertSell = new Model.Pro_SellInfo();
                    //InsertSell.Pro_SellListInfo.AddRange(selllist_query);
                    //InsertSell.Pro_SellSpecalOffList.AddRange(model.Pro_SellSpecalOffList);

                    
                    //验证代金券
                    //Model.WebReturn r = CheckProCashTicket(selllist_query.Where(p => !string.IsNullOrEmpty(p.TicketID) && p.Pro_SellTypeProduct.IsTicketUseful == true).ToList(), lqh);

                    //if (r.ReturnValue != true)
                    //{
                    //    return r;
                    //}

                   

                    //验证单品的优惠
                    //r = CheckProOff(selllist_query, pro_off_list.Where(p => p.VIP_OffList.Type == 0).ToList());
                    //if (r.ReturnValue != true)
                    //{
                    //    return r;
                    //}
                    //筛选组合优惠


                    //r = CheckSpecialOff(InsertSell, AllOffList.Where(p => p.Type == 1).Distinct().ToList());
                    //if (r.ReturnValue != true)
                    //{
                    //    return r;
                    //}

                    //验证优惠券

                    #endregion

                    #region 验证优惠券 和 实际收入
                    //验证优惠券 和 实际收入 
                    r = CheckTicketOff(model, vip);

                    if (r.ReturnValue != true)
                    {
                        return r;
                    }
                    #endregion

                    

                    List<Model.Print_SellListInfo> returnobjs = null;
                    if (model.Pro_SellListInfo.Any(p => p.NeedAduit) )
                    {

                        EntitySet<Model.Pro_SellSpecalOffList> temp1=new EntitySet<Pro_SellSpecalOffList>();
                        temp1.AddRange(model.Pro_SellSpecalOffList);
                        foreach (var proSellSpecalOffList in temp1)
                        {
                            proSellSpecalOffList.Pro_SellInfo = null;

                        }

                        EntitySet<Model.Pro_SellListInfo> temp =new EntitySet<Pro_SellListInfo>();
                        temp.AddRange(model.Pro_SellListInfo);

                        foreach (var proSellListInfo in temp)
                        {
                            proSellListInfo.Pro_SellInfo = null;
                            if (proSellListInfo.VIP_VIPInfo != null)
                            {
                                proSellListInfo.VIP_VIPInfo.Flag = false;
                            }
                        // temp.Add(proSellListInfo);   

                        }
                        Model.Pro_SellOffAduitInfo aduit_model = new Model.Pro_SellOffAduitInfo()
                        {

                            ApplyDate = DateTime.Now,
                            ApplyUserID = sysUser.UserID,
                            HallID = model.HallID,
                            ApplyNote = model.Note
                        };
                        Pro_SellInfo_Aduit model_aduit=new Pro_SellInfo_Aduit()
                            {
                                Seller = model.Seller,
                                SellDate = model.SellDate,
                                OldID = model.OldID,
                                UserID=model.UserID,
                                SysDate = model.SysDate,
                                Note=model.Note,
                                HallID=model.HallID,
                                VIP_ID = model.VIP_ID,
                                CusName = model.CusName,
                                CusPhone=model.CusPhone,
                                CardPay = model.CardPay,
                                CashPay = model.CashPay,
                                OffID = model.OffID,
                                SpecalOffID = model.SpecalOffID,
                                OffTicketID=model.OffTicketID,
                                CashTotle = model.CashTotle,
                                AuditID = model.AuditID,
                                BillID = model.BillID,
                                Pro_SellListInfo = temp,
                                Pro_SellOffAduitInfo = aduit_model,
                                Pro_SellSpecalOffList = temp1

                            };
                        
                        foreach (var proImei in modifedIMEI)
                        {
                            proImei.Pro_SellInfo = null;
                            proImei.Pro_SellOffAduitInfo = aduit_model;

                        }
                        lqh.Umsdb.Pro_SellInfo_Aduit.InsertOnSubmit(model_aduit);
                        
                    }
                    else{
                        #region 生成单号
                        string SellID = "";
                        lqh.Umsdb.OrderMacker2(model.HallID, ref SellID);
                        if (SellID == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "销售单生成出错" };
                        }
                        model.SellID = SellID;
                        #endregion
                    lqh.Umsdb.Pro_SellInfo.InsertOnSubmit(model);
                        returnobjs=new List<Print_SellListInfo>();
                    
                    }
                    
                    #region 刪去临时表
                    if (tempidlist!=null && tempidlist.Count>0){
                    var needdelete1 =
                        lqh.Umsdb.Pro_SellListInfo_Temp.Where(p=>
                            tempidlist.Contains(p.ID));
                        var needdelete2 =
                            lqh.Umsdb.Pro_Sell_Yanbao_temp.Where(p => tempidlist.Contains(Convert.ToInt32(p.SellListID)));
                        var needdelete3 =
                            lqh.Umsdb.VIP_VIPInfo_Temp.Where(p => tempidlist.Contains(Convert.ToInt32(p.SellListID)));
                        var needdelete4 =
                            lqh.Umsdb.Pro_Sell_JiPeiKa_temp.Where(p => tempidlist.Contains(Convert.ToInt32(p.SellListID)));
                        var needdelete5 =
                            lqh.Umsdb.Pro_BillInfo_temp.Where(p => tempidlist.Contains(Convert.ToInt32(p.SellListID)));
                    lqh.Umsdb.Pro_Sell_Yanbao_temp.DeleteAllOnSubmit(needdelete2);
                    lqh.Umsdb.Pro_SellListInfo_Temp.DeleteAllOnSubmit(needdelete1);
                        lqh.Umsdb.VIP_VIPInfo_Temp.DeleteAllOnSubmit(needdelete3);
                        lqh.Umsdb.Pro_Sell_JiPeiKa_temp.DeleteAllOnSubmit(needdelete4);
                        lqh.Umsdb.Pro_BillInfo_temp.DeleteAllOnSubmit(needdelete5);
                    }
                    #endregion
                    lqh.Umsdb.SubmitChanges();
                    if (returnobjs != null)
                    {
                        returnobjs = lqh.Umsdb.Print_SellListInfo.Where(info => info.系统自增外键编号 == model.ID).ToList();
                    }



                    return new Model.WebReturn() { ReturnValue = true, Message = "保存成功",Obj=returnobjs };
                }

                
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() {  ReturnValue=false, Message=""+ex.Message};
            }
        }

        /// <summary>
        /// 新增批发
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddWholeSale(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            this._NewSellListInfo = model.Pro_SellListInfo;
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    DataLoadOptions dataload = new DataLoadOptions();
                    //d.LoadWith<Model.Pro_ProInfo>(c => c.Pro_SellTypeProduct);

                    dataload.LoadWith<Model.Pro_SellAduit>(c => c.Pro_SellAduitList);
                    //d.LoadWith<Model.VIP_OffList>(c => c.VIP_ProOffList);
                    //d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPOffLIst);
                    //d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPTypeOffLIst);

                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;

                    #region 验证会员信息
                    var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                    select b;
                    if (vip_query.Count() <= 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "该会员不存在" };
                    }
                    var vip = vip_query.First();
                    #endregion

                    #region 验证促销 操作人员

                    var user_seller = from b in lqh.Umsdb.Sys_UserInfo
                                      where b.UserID == model.UserID && b.CanLogin == true && b.Flag == true
                                      select b;
                    if (user_seller.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "操作员不存在" };
                    }

                    var user_Oper = from b in lqh.Umsdb.Sys_UserInfo
                                    where b.UserID == model.Seller && b.Flag == true
                                    select b;
                    if (user_Oper.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售员不存在" };
                    }
                    #endregion

                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

//                    if (r.ReturnValue != true)
//                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = model.HallID + "仓库无权操作" };
                    #endregion

//                    #region 生产单号
//                    string SellID = "";
//                    lqh.Umsdb.OrderMacker(1, "XS", "XS", ref SellID);
//                    if (SellID=="")
//                    {
//                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单生成出错" };
//                    }
//                    model.SellID = SellID;
//
//                    #endregion

                    #region 验证审批单
                    var Sell_Audit_query = from b in lqh.Umsdb.Pro_SellAduit
                                           where b.AduitID==model.AuditID && b.HallID==model.HallID
                                           select b;
                    if (Sell_Audit_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在" };
                    }
                    Model.Pro_SellAduit sellAudit = Sell_Audit_query.First();
                    if (!sellAudit.Aduited == true || !sellAudit.Passed == true || sellAudit.Used == true)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批单审核未通过或者已被使用" };
                    }

                    sellAudit.Used = true;
                    sellAudit.UseDate = DateTime.Now;
                    var selllist_group = from b in model.Pro_SellListInfo
                                           group b by new { b.ProID } into temp
                                           from b1 in temp
                                           select new { b1.ProID,ProCount=temp.Sum(p=>p.ProCount) };
                    var selllist_group_join_auditlist = from b in selllist_group
                                                        join c in sellAudit.Pro_SellAduitList
                                                        on new { b.ProID } equals new { c.ProID }
                                                        into temp
                                                        from c1 in temp.DefaultIfEmpty()
                                                        select new { b,c1};
                    if (selllist_group_join_auditlist.Where(p => p.c1 == null || p.b.ProCount> p.c1.ProCount).Count() > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "部分商品批发数量超过了审批单中的限额" };
                    }
                    #endregion

                    //存放串码
                    List<string> IMEI = new List<string>(); 
                    //商品 销售类别
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放无串码
                    List<string> ProIDNoIMEI = new List<string>(); 

                    #region 获取当前订单包含的串号、商品编号

                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        ////有商品限制，而且商品不在权限范围内
                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        }
                        SellType_ProID_List.Add(m.SellType_Pro_ID);
                        //串码
                        if (!string.IsNullOrEmpty(m.IMEI))
                        {
                            if (IMEI.Contains(m.IMEI))
                            {
                                NoError = false;
                                m.Note = m.IMEI + "串码重复";
                                continue;
                            }
                            else
                                IMEI.Add(m.IMEI);
                        }

                        //商品编号
                        if (!ProIDNoIMEI.Contains(m.ProID) && string.IsNullOrEmpty(m.IMEI))
                            ProIDNoIMEI.Add(m.ProID);

                      
                    }
                    #endregion


                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    }

                    //串码类拣货
                    var imeiList = (from b in lqh.Umsdb.Pro_IMEI
                                    where IMEI.Contains(b.IMEI) && b.HallID == model.HallID
                                    select b).ToList();
                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == model.HallID && ProIDNoIMEI.Contains(b.ProID) && b.ProCount > 0
                                     orderby b.InListID
                                     select b).ToList();
                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                       group b by b.ProID into temp
                                       select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();

             

                    #region 生成验证销售方式、各种价格左连接部分
//                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
//                                            where SellType_ProID_List.Contains(b.ID)
//                                            select b).ToList();

                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                           where
                                               (model.Pro_SellListInfo.Select(p => p.ProID).ToList()).Contains(b.ProID)
                                           select b).ToList();


                    var Pro_SellTypeList2 = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                             where
                                                 (model.Pro_SellListInfo.Select(p => p.ProID).ToList()).Contains(b.ProID) && b.SellType==1
                                             select b).ToList();
                    
                    #endregion


                    #region 左连接单品优惠 组合优惠 销售方式


                    var join_query = from b in model.Pro_SellListInfo
                                     join e in Pro_SellTypeList
                                     //on b.SellType_Pro_ID equals e.ID 
                                     on new
                                         {
                                             b.ProID,b.SellType
                                         } equals new
                                             {
                                                 e.ProID,
                                                 e.SellType
                                             }
                                     into temp3
                                     from e1 in temp3.DefaultIfEmpty()
                                     join f in imeiList
                                     on b.IMEI equals f.IMEI
                                     into temp4
                                     from f1 in temp4.DefaultIfEmpty()
                                     join g in StoreList__
                                     on b.ProID equals g.ProID
                                     into temp5
                                     from g1 in temp5.DefaultIfEmpty()
                                     join h in sellAudit.Pro_SellAduitList
                                     on b.ProID equals h.ProID
                                     into temp6
                                     
                                     from h1 in temp6.DefaultIfEmpty()
                                     
                                     join p1 in Pro_SellTypeList2 on b.ProID equals p1.ProID
                                     into temp7
                                     from t1 in temp7.DefaultIfEmpty()
                                     select new
                                     {
                                         Pro_SellListInfo = b, 
                                         Pro_SellTypeProduct = e1,
                                         Pro_IMEI = f1,
                                         Pro_StoreInfo = g1,
                                         Pro_SellAduitList=h1,
                                         Pro_SellTypeProduct2=t1
                                     };
                    #endregion 

                    List<Model.Pro_SellListInfo> sellListTemp = new List<Model.Pro_SellListInfo>();
                     
                    decimal? cashPrice = 0;
                    foreach (var child in join_query)
                    {
                        //SellType_ProID_List.Add(child.SellType_Pro_ID);
                        //ProID.Add(child.Pro_SellListInfo.ProID);
                        //OffID_List.Add(child.OffID);
                        #region 验证单品优惠 组合优惠 商品信息 销售类别 有无串码 单价

 
                        //验证销售类别、商品信息
                        if (child.Pro_SellTypeProduct == null || child.Pro_SellTypeProduct.Pro_ProInfo == null)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品不存在或销售类别不正确";
                            continue;
                        }
                        if (child.Pro_SellTypeProduct.Pro_ProInfo.NeedIMEI == true && string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品缺少串码";
                            continue;
                        }
                        if (child.Pro_SellTypeProduct.Pro_ProInfo.NeedIMEI != true && !string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "属于无串码商品";
                            continue;
                        }
                        child.Pro_SellListInfo.Pro_SellTypeProduct = child.Pro_SellTypeProduct;
                        child.Pro_SellListInfo.LowPrice = child.Pro_SellTypeProduct.LowPrice;
                        child.Pro_SellListInfo.ProCost = child.Pro_SellTypeProduct.ProCost;
                        child.Pro_SellListInfo.WholeSaleOffPrice = child.Pro_SellAduitList.OffMoney;
                       
                        if (child.Pro_SellTypeProduct2 != null)
                        {
                            child.Pro_SellListInfo.YanbaoModelPrice = child.Pro_SellTypeProduct2.Price;
                        }
                        if (child.Pro_SellTypeProduct.MaxPrice < child.Pro_SellListInfo.ProPrice -child.Pro_SellListInfo.WholeSaleOffPrice || child.Pro_SellTypeProduct.MinPrice > child.Pro_SellListInfo.ProPrice-child.Pro_SellListInfo.WholeSaleOffPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品批发价格单价不在规定区间";
                            continue;
                        }
                        #endregion

                        if (!NoError)
                            continue;







                        #region 计算提成

                        if (child.Pro_SellListInfo.Salary == 0 || child.Pro_SellListInfo.Salary == null)
                        //无套餐提成
                        {
                            var today = DateTime.Today;
                            var query = lqh.Umsdb.Sys_SalaryCurrentList.Where(
                                p =>
                                    p.SalaryYear == today.Year && p.SalaryMonth == today.Month &&
                                    p.SalaryDay == today.Day)
                                .Where(
                                    o =>
                                        o.ProID == child.Pro_SellListInfo.ProID &&
                                        o.SellType == child.Pro_SellListInfo.SellType);
                            query =
                                query.Where(
                                    p =>
                                        p.Min >= (child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed) &&
                                        p.Max < (child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed));
                            if (query.Any())
                            {
                                var salarymodel = query.First();
                                decimal childsalary = 0;
                                if (salarymodel.BaseSalary > 0)
                                {
                                    childsalary += salarymodel.BaseSalary;
                                }
                                else
                                {
                                    childsalary += Convert.ToDecimal(child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed) *
                                                   salarymodel.SpecialSalary;
                                }
                                if ((child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed) >
                                    salarymodel.Ratio)
                                {
                                    childsalary += Convert.ToDecimal(child.Pro_SellListInfo.CashPrice + child.Pro_SellListInfo.TicketUsed - salarymodel.Ratio) *
                                                   salarymodel.OverRatio;
                                }
                                child.Pro_SellListInfo.Salary = childsalary;

                            }
                        }
                        #endregion

 

                        #region 验证实收
                        child.Pro_SellListInfo.TicketUsed = 0;
                        child.Pro_SellListInfo.OffSepecialPrice = 0;
                        child.Pro_SellListInfo.OffPrice = 0;

                        decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.WholeSaleOffPrice ;
                        if (real * child.Pro_SellListInfo.ProCount != child.Pro_SellListInfo.CashPrice * child.Pro_SellListInfo.ProCount)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收计算有误";
                            continue;
                        }
                        #endregion



                        cashPrice += child.Pro_SellListInfo.CashPrice * child.Pro_SellListInfo.ProCount;

                        #region 串码类验证
                        if (!string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = CheckIMEI(child.Pro_IMEI);
                            if (!r.ReturnValue)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = r.Message;
                                continue;
                            }
                            child.Pro_SellListInfo.InListID = child.Pro_IMEI.InListID;
                            child.Pro_IMEI.Pro_StoreInfo.ProCount--;
                            child.Pro_IMEI.Pro_SellInfo = model;
                        }
                        #endregion



                        #region 非串码类验证
                        if (string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = FitInOrderListIDNoIMEI(child.Pro_SellListInfo, child.Pro_StoreInfo, sellListTemp);
                            if (!r.ReturnValue == true)
                            {
                                NoError = false;
                                continue;
                            }
                        }
                        #endregion

                    }
                    if (!NoError)//有错误
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "提交有误", Obj = model };
                    }
                    if (cashPrice != model.CashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误", Obj = model };
                    }
                    #region 存在未拣完货的
                    if (sellListTemp.Count > 0)
                    {
                        r = FitInOrderListIDNoIMEI(sellListTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        model.Pro_SellListInfo.AddRange(sellListTemp);
                    }
                    #endregion

                    #region 生成单号
                    string SellID = "";
                    lqh.Umsdb.OrderMacker2(model.HallID, ref SellID);
                    if (SellID == "")
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单生成出错" };
                    }
                    model.SellID = SellID;
                    #endregion


                    lqh.Umsdb.Pro_SellInfo.InsertOnSubmit(model);

                    lqh.Umsdb.SubmitChanges();





                }
                return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "" + ex.Message };
            }
        }

        /// <summary>
        /// 新增批发
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddWholeSale_temp(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            this._NewSellListInfo = model.Pro_SellListInfo;
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    DataLoadOptions dataload = new DataLoadOptions();
                    //d.LoadWith<Model.Pro_ProInfo>(c => c.Pro_SellTypeProduct);

                    dataload.LoadWith<Model.Pro_SellAduit>(c => c.Pro_SellAduitList);
                    //d.LoadWith<Model.VIP_OffList>(c => c.VIP_ProOffList);
                    //d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPOffLIst);
                    //d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPTypeOffLIst);

                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;

                    #region 验证会员信息
                    var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                    select b;
                    if (vip_query.Count() <= 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "该会员不存在" };
                    }
                    var vip = vip_query.First();
                    #endregion

                    #region 验证促销 操作人员

                    var user_seller = from b in lqh.Umsdb.Sys_UserInfo
                                      where b.UserID == model.UserID && b.CanLogin == true && b.Flag == true
                                      select b;
                    if (user_seller.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "操作员不存在" };
                    }

                    var user_Oper = from b in lqh.Umsdb.Sys_UserOPList
                                    //where b.ID == model.Seller && b.Flag == true
                                    select b;
                    if (user_Oper.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售员不存在" };
                    }
                    #endregion

                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    //                    if (r.ReturnValue != true)
                    //                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = model.HallID + "仓库无权操作" };
                    #endregion

                    #region 生产单号
                    string SellID = "";
                    lqh.Umsdb.OrderMacker(1, "XS", "XS", ref SellID);
                    if (SellID == "")
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单生成出错" };
                    }
                    model.SellID = SellID;

                    #endregion

                    #region 验证审批单
                    var Sell_Audit_query = from b in lqh.Umsdb.Pro_SellAduit
                                           where b.AduitID == model.AuditID && b.HallID == model.HallID
                                           select b;
                    if (Sell_Audit_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在" };
                    }
                    Model.Pro_SellAduit sellAudit = Sell_Audit_query.First();
                    if (!sellAudit.Aduited == true || !sellAudit.Passed == true || sellAudit.Used == true)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批单审核未通过或者已被使用" };
                    }

                    sellAudit.Used = true;
                    sellAudit.UseDate = DateTime.Now;
                    var selllist_group = from b in model.Pro_SellListInfo
                                         group b by new { b.ProID } into temp
                                         from b1 in temp
                                         select new { b1.ProID, ProCount = temp.Sum(p => p.ProCount) };
                    var selllist_group_join_auditlist = from b in selllist_group
                                                        join c in sellAudit.Pro_SellAduitList
                                                        on new { b.ProID } equals new { c.ProID }
                                                        into temp
                                                        from c1 in temp.DefaultIfEmpty()
                                                        select new { b, c1 };
                    if (selllist_group_join_auditlist.Where(p => p.c1 == null || p.b.ProCount > p.c1.ProCount).Count() > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "部分商品批发数量超过了审批单中的限额" };
                    }
                    #endregion

                    //存放串码
                    List<string> IMEI = new List<string>();
                    //商品 销售类别
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放无串码
                    List<string> ProIDNoIMEI = new List<string>();

                    #region 获取当前订单包含的串号、商品编号

                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        ////有商品限制，而且商品不在权限范围内
                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        }
                        SellType_ProID_List.Add(m.SellType_Pro_ID);
                        //串码
                        if (!string.IsNullOrEmpty(m.IMEI))
                        {
                            if (IMEI.Contains(m.IMEI))
                            {
                                NoError = false;
                                m.Note = m.IMEI + "串码重复";
                                continue;
                            }
                            else
                                IMEI.Add(m.IMEI);
                        }

                        //商品编号
                        if (!ProIDNoIMEI.Contains(m.ProID) && string.IsNullOrEmpty(m.IMEI))
                            ProIDNoIMEI.Add(m.ProID);


                    }
                    #endregion


                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    }

                    //串码类拣货
                    var imeiList = (from b in lqh.Umsdb.Pro_IMEI
                                    where IMEI.Contains(b.IMEI) && b.HallID == model.HallID
                                    select b).ToList();
                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == model.HallID && ProIDNoIMEI.Contains(b.ProID)
                                     orderby b.InListID
                                     select b).ToList();
                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                       group b by b.ProID into temp
                                       select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();



                    #region 生成验证销售方式、各种价格左连接部分
                    //                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                    //                                            where SellType_ProID_List.Contains(b.ID)
                    //                                            select b).ToList();

                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where
                                                (model.Pro_SellListInfo.Select(p => p.ProID).ToList()).Contains(b.ProID)
                                            select b).ToList();




                    #endregion


                    #region 左连接单品优惠 组合优惠 销售方式


                    var join_query = from b in model.Pro_SellListInfo
                                     join e in Pro_SellTypeList
                                         //on b.SellType_Pro_ID equals e.ID 
                                     on new
                                     {
                                         b.ProID,
                                         b.SellType
                                     } equals new
                                     {
                                         e.ProID,
                                         e.SellType
                                     }
                                     into temp3
                                     from e1 in temp3.DefaultIfEmpty()
                                     join f in imeiList
                                     on b.IMEI equals f.IMEI
                                     into temp4
                                     from f1 in temp4.DefaultIfEmpty()
                                     join g in StoreList__
                                     on b.ProID equals g.ProID
                                     into temp5
                                     from g1 in temp5.DefaultIfEmpty()
                                     join h in sellAudit.Pro_SellAduitList
                                     on b.ProID equals h.ProID
                                     into temp6
                                     from h1 in temp6.DefaultIfEmpty()

                                     select new
                                     {
                                         Pro_SellListInfo = b,
                                         Pro_SellTypeProduct = e1,
                                         Pro_IMEI = f1,
                                         Pro_StoreInfo = g1,
                                         Pro_SellAduitList = h1
                                     };
                    #endregion

                    List<Model.Pro_SellListInfo> sellListTemp = new List<Model.Pro_SellListInfo>();

                    decimal? cashPrice = 0;
                    foreach (var child in join_query)
                    {
                        //SellType_ProID_List.Add(child.SellType_Pro_ID);
                        //ProID.Add(child.Pro_SellListInfo.ProID);
                        //OffID_List.Add(child.OffID);
                        #region 验证单品优惠 组合优惠 商品信息 销售类别 有无串码 单价


                        //验证销售类别、商品信息
                        if (child.Pro_SellTypeProduct == null || child.Pro_SellTypeProduct.Pro_ProInfo == null)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品不存在或销售类别不正确";
                            continue;
                        }
                        if (child.Pro_SellTypeProduct.Pro_ProInfo.NeedIMEI == true && string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品缺少串码";
                            continue;
                        }
                        if (child.Pro_SellTypeProduct.Pro_ProInfo.NeedIMEI != true && !string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "属于无串码商品";
                            continue;
                        }

                        child.Pro_SellListInfo.LowPrice = child.Pro_SellTypeProduct.LowPrice;
                        child.Pro_SellListInfo.ProCost = child.Pro_SellTypeProduct.ProCost;
                        child.Pro_SellListInfo.WholeSaleOffPrice = child.Pro_SellAduitList.OffMoney;

                        if (child.Pro_SellTypeProduct.MaxPrice < child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.WholeSaleOffPrice || child.Pro_SellTypeProduct.MinPrice > child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.WholeSaleOffPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品批发价格单价有误，必须大于等于" + child.Pro_SellTypeProduct.MinPrice + "且小于等于" + child.Pro_SellTypeProduct.MaxPrice;
                            continue;
                        }
                        #endregion

                        if (!NoError)
                            continue;










                        #region 验证实收
                        child.Pro_SellListInfo.TicketUsed = 0;
                        child.Pro_SellListInfo.OffSepecialPrice = 0;
                        child.Pro_SellListInfo.OffPrice = 0;

                        decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.WholeSaleOffPrice;
                        if (real * child.Pro_SellListInfo.ProCount != child.Pro_SellListInfo.CashPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收计算有误";
                            continue;
                        }
                        #endregion



                        cashPrice += child.Pro_SellListInfo.CashPrice;

                        #region 串码类验证
                        if (!string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = CheckIMEI(child.Pro_IMEI);
                            if (!r.ReturnValue)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = r.Message;
                                continue;
                            }
                            child.Pro_IMEI.Pro_StoreInfo.ProCount--;
                            child.Pro_IMEI.Pro_SellInfo = model;
                        }
                        #endregion



                        #region 非串码类验证
                        if (string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = FitInOrderListIDNoIMEI(child.Pro_SellListInfo, child.Pro_StoreInfo, sellListTemp);
                            if (!r.ReturnValue == true)
                            {
                                NoError = false;
                                continue;
                            }
                        }
                        #endregion

                    }
                    if (!NoError)//有错误
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "提交有误", Obj = model };
                    }
                    if (cashPrice != model.CashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误", Obj = model };
                    }
                    #region 存在未拣完货的
                    if (sellListTemp.Count > 0)
                    {
                        r = FitInOrderListIDNoIMEI(sellListTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        model.Pro_SellListInfo.AddRange(sellListTemp);
                    }
                    #endregion


                    List<Model.Pro_SellListInfo_Temp> sellListInfoTemps = new List<Model.Pro_SellListInfo_Temp>();

                    foreach (var proSellListInfo in model.Pro_SellListInfo)
                    {
                        Model.Pro_SellListInfo_Temp t = new Model.Pro_SellListInfo_Temp();

                        t.ProID = proSellListInfo.ProID;
                        t.ProCost = proSellListInfo.ProCount;
                        t.SellType = proSellListInfo.SellType;
                        t.CashTicket = proSellListInfo.CashTicket;
                        t.ProPrice = proSellListInfo.ProPrice;
                        t.TicketID = proSellListInfo.TicketID;
                        t.TicketUsed = proSellListInfo.TicketUsed;
                        t.CashPrice = proSellListInfo.CashPrice;
                        t.IMEI = proSellListInfo.IMEI;
                        t.ServiceInfo = proSellListInfo.ServiceInfo;
                        t.Note = proSellListInfo.Note;
                        t.ProCost = proSellListInfo.ProCost;
                        t.LowPrice = proSellListInfo.LowPrice;
                        t.AduidID = proSellListInfo.AduidID;
                        t.AduidedOldPrice = proSellListInfo.AduidedOldPrice;
                        t.OffID = proSellListInfo.OffID;
                        t.OffPoint = proSellListInfo.OffPoint;
                        t.SpecialID = proSellListInfo.SpecialID;
                        t.SellType_Pro_ID = proSellListInfo.SellType_Pro_ID;
                        t.WholeSaleOffPrice = proSellListInfo.WholeSaleOffPrice;
                        t.BackID = proSellListInfo.BackID;
                        t.OldSellListID = proSellListInfo.OldSellListID;
                        t.IsFree = proSellListInfo.IsFree;
                        t.ProCount = proSellListInfo.ProCount;


                        sellListInfoTemps.Add(t);

                    }
                    lqh.Umsdb.Pro_SellListInfo_Temp.InsertAllOnSubmit(sellListInfoTemps);
                    lqh.Umsdb.SubmitChanges();
//
//                    lqh.Umsdb.Pro_SellInfo.InsertOnSubmit(model);
//
//                    lqh.Umsdb.SubmitChanges();





                }
                return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "" + ex.Message };
            }
        }



        public Model.WebReturn AddGXYamBao_Temp(Model.Sys_UserInfo userinfo, List<Model.Pro_SellListInfo_Temp> models)
        {
            try
            {
                
                List<Model.Pro_SellListInfo_Temp> resulsobj=new List<Model.Pro_SellListInfo_Temp>();
                using (LinQSqlHelper lqh=new LinQSqlHelper())
                
                {
                    DataLoadOptions d = new DataLoadOptions();
                    d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.Pro_Sell_Yanbao_temp);
                    d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.VIP_VIPInfo_Temp);
                    d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.Pro_Sell_JiPeiKa_temp);
                    d.LoadWith<Model.Pro_SellListInfo_Temp>(temp => temp.Pro_BillInfo_temp);
                    if ((userinfo.Sys_RoleInfo.Sys_Role_Menu_ProInfo.Where(p => p.MenuID == 101).GroupJoin(lqh.Umsdb.Pro_ProInfo, b => b.ClassID, c => c.Pro_ClassID,
                                                                                                           (b, Production) => new {b, Production})
                                 .SelectMany(@t => @t.Production.DefaultIfEmpty())).Where(p => p != null).Count(p => p.ProID == lqh.Umsdb.Sys_Option.First(option => option.ClassName == "GXYanBao").Value) != 1)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "用户权限错误" };
                }
                    List<Model.Pro_SellListInfo_Temp> listinfo = new List<Model.Pro_SellListInfo_Temp>();
                    foreach (var proSellListInfo in models)
                    {
                        var yanbaomodel = proSellListInfo.Pro_Sell_Yanbao_temp;



                        //Model.Pro_SellListInfo_Temp sl = new Model.Pro_SellListInfo_Temp();
                        //sl.ProID = userinfo.Sys_RoleInfo.Sys_Role_Menu_ProInfo.First(p=>p.MenuID==101).ProID;
                        //TODO: BUG
                        proSellListInfo.ProCount = 1;
                        proSellListInfo.SellType = 1;
                        proSellListInfo.OldID = proSellListInfo.OldID;
                        proSellListInfo.IMEI = proSellListInfo.Pro_Sell_Yanbao_temp.BillID;
                        if (
                            !lqh.Umsdb.Pro_IMEI.Any(
                                p =>
                                p.IMEI == proSellListInfo.IMEI && p.SellID == null && p.ProID == proSellListInfo.ProID && p.HallID==proSellListInfo.HallID))
                        {
                            throw new Exception("合同号错误");
                        }
                        if (lqh.Umsdb.Pro_Sell_Yanbao.Any(
                            p =>
                            p.MobileIMEI == proSellListInfo.Pro_Sell_Yanbao_temp.MobileIMEI &&
                            p.Pro_SellListInfo.Pro_SellBackList.Count == 0))
                        {
                            throw new Exception("串号 " + proSellListInfo.Pro_Sell_Yanbao_temp.MobileIMEI + " 已销售过延保");
                        }
                    lqh.Umsdb.Pro_SellListInfo_Temp.InsertOnSubmit(proSellListInfo);
                        
//                        yanbaomodel.Pro_SellListInfo_Temp = sl;
//                        lqh.Umsdb.Pro_Sell_Yanbao_temp.InsertOnSubmit(yanbaomodel);
//                        lqh.Umsdb.SubmitChanges();

//                        yanbaomodel.Pro_SellListInfo = null;
//                        //yanbaomodel.SellListID = sl.ID;                        
//                        lqh.Umsdb.Pro_Sell_Yanbao.InsertOnSubmit(yanbaomodel);
//                        lqh.Umsdb.SubmitChanges();
//                        lqh.Umsdb.ExecuteCommand("update pro_sell_yanbao set selllistid = " + sl.ID + " where ID=" +
//                                               yanbaomodel.ID);
                        resulsobj.Add(proSellListInfo);

                    }
                    lqh.Umsdb.SubmitChanges();
                    return new Model.WebReturn() { ReturnValue = true, Message = "", Obj = resulsobj };

                    //lqh.Umsdb.Pro_Sell_Yanbao.InsertAllOnSubmit(models);
                    
                    

                }
            }
            catch (Exception ex)
            {

                return new Model.WebReturn() { ReturnValue = false, Message = "" + ex.Message };
            }
            
        }


        /// <summary>
        /// 广信延保
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddGXYanBao(Model.Sys_UserInfo sysUser,Model.Pro_SellInfo model)
        { 
            Model.WebReturn r = null;
            bool NoError = true;
            this._NewSellListInfo = model.Pro_SellListInfo;
         
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {

                    List<string> ProIDS = new List<string>();
                    List<Model.Pro_SellListInfo> selllistTemp = new List<Model.Pro_SellListInfo>();
                    List<int?> SellType_ProID_List = new List<int?>();
                    List<string> MobileIMEIList = new List<string>();
                    //存放串码
                    List<string> IMEI = new List<string>();


                    #region 验证会员信息
                    if (model.VIP_ID > 0)
                    {
                        var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                        select b;
                        if (vip_query.Count() <= 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该会员不存在" };
                        }
                        var vip = vip_query.First();
                        model.CusName = vip.MemberName;
                        model.CusPhone = vip.MobiPhone;
                    }
                    if (string.IsNullOrEmpty(model.CusName) || string.IsNullOrEmpty(model.CusPhone))
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "客户姓名、电话必填" };
                    }
                    #endregion

                    #region 验证促销 操作人员

                    var user_seller = from b in lqh.Umsdb.Sys_UserInfo
                                      where b.UserID == model.UserID && b.CanLogin == true && b.Flag == true
                                      select b;
                    if (user_seller.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "操作员不存在" };
                    }

                    var user_Oper = from b in lqh.Umsdb.Sys_UserInfo
                                    where b.UserID == model.Seller && b.Flag == true
                                    select b;
                    if (user_Oper.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售员不存在" };
                    }
                    #endregion

                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = model.HallID + "仓库无权操作" };
                    #endregion

                    #region 获取所有商品型号
                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        if (m.Pro_Sell_Yanbao == null || string.IsNullOrEmpty(m.Pro_Sell_Yanbao.MobileIMEI))
                        {
                            NoError = false;
                            m.Note = "宜安保的明细不能为空";
                            continue;
                        }
                        else MobileIMEIList.Add(m.Pro_Sell_Yanbao.MobileIMEI);

                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        }
                        if (!SellType_ProID_List.Contains(m.SellType_Pro_ID))
                            SellType_ProID_List.Add(m.SellType_Pro_ID);
                        //串码
                        if (!string.IsNullOrEmpty(m.IMEI))
                        {
                            if (IMEI.Contains(m.IMEI))
                            {
                                NoError = false;
                                m.Note = m.IMEI + "串码重复";
                                continue;
                            }
                            else
                                IMEI.Add(m.IMEI);
                        }
                        //商品编号
                        if (!ProIDS.Contains(m.ProID) && string.IsNullOrEmpty(m.IMEI))
                            ProIDS.Add(m.ProID);
                    }
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    }
                    #endregion

                    #region 生成验证销售方式、各种价格左连接部分
                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where SellType_ProID_List.Contains(b.ID)
                                            select b).ToList();
                    #endregion


                    #region 库存
                    //串码类拣货
                    var imeiList = (from b in lqh.Umsdb.Pro_IMEI
                                    where IMEI.Contains(b.IMEI) && b.HallID == model.HallID
                                    select b).ToList();


                    #region 延保的终端串码销售记录

                    var MobileIMEIList_SellList = (from b in lqh.Umsdb.Pro_SellListInfo
                                                  where MobileIMEIList.Contains(b.IMEI)
                                                  group b by b.IMEI into temp
                                                  select temp.Single(p=>p.ID==temp.Max(p2=>p2.ID))).ToList();
                    #endregion

                    //非串码
                    //var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                    //                 where b.HallID == model.HallID && (ProIDS.Contains(b.ProID))
                    //                 orderby b.InListID
                    //                 select b).ToList();

                    //按照商品编号 取批次号最小的
                    //var StoreList__ = (from b in StoreList
                    //                   group b by b.ProID into temp
                    //                   select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();
                    #endregion

                    #region 延保的价格区间
                    var Yanbao_Step= (from b in lqh.Umsdb.Pro_YanbaoPriceStepInfo
                                   where ProIDS.Contains(b.ProID) 
                                   group b by b.ProID into temp
                                   from b1 in temp.DefaultIfEmpty()
                                   select new{b1.ProID,Pro_YanbaoPriceStepInfo= temp.Where(p=> 1==1)}).ToList();
                     
                    //var Yanbao_Step_Max=from b in Yanbao_Step
                    #endregion

                    #region 重复购买延保的终端串码
                    
                    var MobileIMEIAgain=(from b in lqh.Umsdb.Pro_Sell_Yanbao
                                         where MobileIMEIList.Contains(b.MobileIMEI) && (b.BackListID == 0 || b.BackListID == null)
                                        select b).ToList();

                    #endregion

                    #region 左连接库存 拣货 减库存  免费的服务

                    var SellList_join_Store = from b in model.Pro_SellListInfo
                                              //join c in StoreList__
                                              //on b.ProID equals c.ProID
                                              //into temp
                                              //from c1 in temp.DefaultIfEmpty()
                                              join d in Pro_SellTypeList
                                              on new { ID = (int)b.SellType_Pro_ID, b.ProID, b.SellType }
                                              equals new { d.ID, d.ProID, d.SellType }
                                              into temp2
                                              from d1 in temp2.DefaultIfEmpty()
                                              join e in Yanbao_Step
                                              on b.ProID equals e.ProID
                                              into temp3
                                              from e1 in temp3.DefaultIfEmpty()
                                              join f in imeiList
                                              on b.IMEI equals f.IMEI
                                              into temp4
                                              from f1 in temp4
                                              join g in MobileIMEIList_SellList
                                              on b.Pro_Sell_Yanbao.MobileIMEI equals g.IMEI
                                              into temp5
                                              from g1 in temp5.DefaultIfEmpty()
                                              join h in MobileIMEIAgain
                                              on b.Pro_Sell_Yanbao.MobileIMEI equals h.MobileIMEI
                                              into temp6
                                              from h1 in temp6
                                              select new { Pro_SellListInfo = b, Pro_SellTypeProduct = d1, Pro_YanbaoPriceStepInfo = e1, Pro_IMEI = f1, OldPro_SellListInfo = g1, Pro_Sell_Yanbao =h1};
                    decimal? cashTotle = 0;
                    foreach (var m in SellList_join_Store)
                    {
                        
                        if (string.IsNullOrEmpty(m.Pro_SellListInfo.IMEI))
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "合同号不能为空";
                            continue;
                        }
                        if (m.Pro_SellTypeProduct == null)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "销售方式不存在";
                            continue;
                        }
                        if (m.Pro_YanbaoPriceStepInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "未定义终端的价格区间，无法获取延保价格";
                            continue;
                        }
                        if (m.OldPro_SellListInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "终端未销售，未能获取销售价格";
                            continue;
                        }
                        if (m.Pro_Sell_Yanbao != null)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "此终端串码已购买延保";
                            continue;
                        }
                        if (!NoError) continue;
                        //
                        var MinStep = (from b in m.Pro_YanbaoPriceStepInfo.Pro_YanbaoPriceStepInfo
                                             where b.StepPrice >= m.Pro_SellListInfo.Pro_Sell_Yanbao.MobilePrice
                                             select b).OrderBy(p=>p.StepPrice);
                        if(MinStep.Count()==0)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "终端的价格不在延保的价格区间";
                            continue;
                        }
                        Model.Pro_YanbaoPriceStepInfo yanbaoFirst=MinStep.First();
                        if (m.Pro_SellListInfo.ProPrice != yanbaoFirst.ProPrice)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "延保的价格不对，终端价格为" + m.Pro_SellListInfo.Pro_Sell_Yanbao.MobilePrice + ",延保价格应为" + yanbaoFirst.ProPrice;
                            continue;
                        }


                        m.Pro_SellListInfo.OffPoint = 0;
                        m.Pro_SellListInfo.CashTicket = 0;
                        //m.Pro_SellListInfo.IMEI = null;
                        m.Pro_SellListInfo.LowPrice = yanbaoFirst.LowPrice;
                        m.Pro_SellListInfo.ProCost = yanbaoFirst.ProCost;
                        m.Pro_SellListInfo.SpecialID = 0;
                        m.Pro_SellListInfo.TicketID = null;
                        m.Pro_SellListInfo.ProCount = 1;
                        //m.Pro_SellListInfo.TicketUsed = 0;
                        //m.Pro_SellListInfo.WholeSaleOffPrice = 0;
                        if (m.Pro_SellListInfo.CashPrice != m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.WholeSaleOffPrice - m.Pro_SellListInfo.TicketUsed - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.OffPrice)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "实收金额不对";
                            continue;
                        }
                        

                        cashTotle += m.Pro_SellListInfo.CashPrice;
                        if (!NoError) continue;

                        #region 串码类验证
                       
                            r = CheckIMEI(m.Pro_IMEI);
                            if (!r.ReturnValue)
                            {
                                NoError = false;
                                m.Pro_SellListInfo.Note = r.Message;
                                continue;
                            }
                            m.Pro_IMEI.Pro_StoreInfo.ProCount--;
                            m.Pro_IMEI.Pro_SellInfo = model;

                         
                        #endregion
                         



                    }
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    }
                    #endregion

                    

                    #region 验证最后总收入
                    if (model.CashTotle != cashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "总收入有误" };
                    }
                    if (model.CashTotle - model.OffTicketPrice != model.CardPay + model.CashPay)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "刷卡、现金收入合计有误" };
                    }
                    #endregion
                    lqh.Umsdb.Pro_SellInfo.InsertOnSubmit(model);
                    lqh.Umsdb.SubmitChanges();

                    return r = new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
                }
            }
            catch (Exception ex)
            {
                return r = new Model.WebReturn() { ReturnValue=false , Message="服务器错误"+ex.Message };
            }
        }


        /// <summary>
        /// 空中充值
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn AddAirChargeTemp(Model.Sys_UserInfo sysUser, List<Model.Pro_SellListInfo_Temp> models)
        {
            Model.WebReturn r = null;
            bool NoError = true;
//            this._NewSellInfo = model;
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {

                    #region 验证Proinfo

                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_ProInfo>()
                                where models.Select(p => p.ProID).Contains(b.ProID)
                                select b;
                    if (!query.Any())
                    {
                        return new WebReturn() { ReturnValue = false, Message = "商品错误"};
                    }
                    #endregion

                    var proIDS = models.Select(p => p.ProID);
                    var Pro_SellType_ProPrice = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                                 where proIDS.Contains(b.ProID)
                                                 select b).ToList();
                    foreach (var proSellListInfoTemp in models)
                    {
                        var temp = Pro_SellType_ProPrice.First(
                            p => p.ProID == proSellListInfoTemp.ProID && p.SellType == proSellListInfoTemp.SellType);
                        proSellListInfoTemp.ProPrice =
                            temp.Price;
                        proSellListInfoTemp.SellType_Pro_ID = temp.ID;
                    }
                    lqh.Umsdb.Pro_SellListInfo_Temp.InsertAllOnSubmit(models);
                    lqh.Umsdb.SubmitChanges();
                }
                return new WebReturn() {ReturnValue = true,Obj = models};
            }
            catch (Exception ex)
            {
                return new WebReturn() { ReturnValue = false ,Message = ex.Message};
            }

            return new WebReturn() {ReturnValue = false};

        }

        /// <summary>
        /// 空中充值 
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddAirCharge(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            this._NewSellListInfo = model.Pro_SellListInfo;

            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    List<string> ProIDS = new List<string>();
                    List<Model.Pro_SellListInfo> selllistTemp=new List<Model.Pro_SellListInfo>();
                    List<int?> SellType_ProID_List=new List<int?>();
                    #region 验证会员信息
                    if (model.VIP_ID > 0)
                    {
                        var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                        select b;
                        if (vip_query.Count() <= 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该会员不存在" };
                        }
                        var vip = vip_query.First();
                        model.CusName = vip.MemberName;
                        model.CusPhone = vip.MobiPhone;
                    }
                    if (string.IsNullOrEmpty(model.CusName) || string.IsNullOrEmpty(model.CusPhone))
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "客户姓名、电话必填" };
                    }
                    #endregion

                    #region 验证促销 操作人员

                    var user_seller = from b in lqh.Umsdb.Sys_UserInfo
                                      where b.UserID == model.UserID && b.CanLogin == true && b.Flag == true
                                      select b;
                    if (user_seller.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "操作员不存在" };
                    }

                    var user_Oper = from b in lqh.Umsdb.Sys_UserInfo
                                    where b.UserID == model.Seller && b.Flag == true
                                    select b;
                    if (user_Oper.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售员不存在" };
                    }
                    #endregion

                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = model.HallID + "仓库无权操作" };
                    #endregion

                    #region 获取所有号码，即商品型号
                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        }
                        if (!ProIDS.Contains(m.ProID))
                            ProIDS.Add(m.ProID);
                        if(!SellType_ProID_List.Contains(m.SellType_Pro_ID))
                            SellType_ProID_List.Add(m.SellType_Pro_ID);
                    }
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    } 
                    #endregion

                    #region 生成验证销售方式、各种价格左连接部分
                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where SellType_ProID_List.Contains(b.ID)
                                            select b).ToList();
                    #endregion

                    #region 库存
                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == model.HallID && (ProIDS.Contains(b.ProID))
                                     orderby b.InListID
                                     select b).ToList();

                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                       group b by b.ProID into temp
                                       select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();
                    #endregion

                    #region 左连接库存 拣货 减库存

                    var SellList_join_Store = from b in model.Pro_SellListInfo
                                              join c in StoreList__
                                              on b.ProID equals c.ProID
                                              into temp
                                              from c1 in temp.DefaultIfEmpty()
                                              join d in Pro_SellTypeList
                                              on new{ ID=(int)b.SellType_Pro_ID,b.ProID,b.SellType }
                                              equals new{ d.ID,d.ProID,d.SellType}
                                              into temp2
                                              from d1 in temp2.DefaultIfEmpty()
                                              select new { Pro_SellListInfo=b,Pro_StoreInfo=c1 , Pro_SellTypeProduct=d1};
                    decimal? cashTotle = 0;
                    foreach (var m in SellList_join_Store)
                    {
                        if (m.Pro_StoreInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "库存不足";
                            continue;
                        }
                        if(m.Pro_SellTypeProduct==null)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "销售方式不存在";
                            continue;
                        }
                        m.Pro_SellListInfo.ProPrice=m.Pro_SellTypeProduct.Price;
                        m.Pro_SellListInfo.OffPoint=0;
                        m.Pro_SellListInfo.CashTicket=0;
                        m.Pro_SellListInfo.IMEI=null;
                        m.Pro_SellListInfo.LowPrice=m.Pro_SellTypeProduct.LowPrice;
                        m.Pro_SellListInfo.ProCost=m.Pro_SellTypeProduct.ProCost;
                        m.Pro_SellListInfo.SpecialID=0;
                        m.Pro_SellListInfo.TicketID=null;
                        m.Pro_SellListInfo.TicketUsed=0;
                        m.Pro_SellListInfo.WholeSaleOffPrice=0;
                        if (m.Pro_SellListInfo.CashPrice != m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.WholeSaleOffPrice - m.Pro_SellListInfo.TicketUsed - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.OffPrice)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "实收金额不对";
                            continue;
                        }
                        cashTotle += m.Pro_SellListInfo.CashPrice;
                        if(NoError)
                        {
                            r = FitInOrderListIDNoIMEI(m.Pro_SellListInfo, m.Pro_StoreInfo, selllistTemp);
                            if (!r.ReturnValue == true)
                            {
                                NoError = false;
                                continue;
                            }
                        }

                    }
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    } 
                    #endregion

                    #region 存在未拣完货的
                    if (selllistTemp.Count > 0)
                    {
                        r = FitInOrderListIDNoIMEI(selllistTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        model.Pro_SellListInfo.AddRange(selllistTemp);
                    }
                    #endregion

                    #region 验证最后总收入
                    if (model.CashTotle != cashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "总收入有误" };
                    }
                    if (model.CashTotle - model.OffTicketPrice != model.CardPay + model.CashPay)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "刷卡、现金收入合计有误" };
                    }
                    #endregion
                    lqh.Umsdb.Pro_SellInfo.InsertOnSubmit(model);
                    lqh.Umsdb.SubmitChanges();

                    return r = new Model.WebReturn() { ReturnValue=true, Message="保存成功" };
                }
            }
            catch (Exception ex)
            {
                return r = new Model.WebReturn() { ReturnValue = false, Message = "服务器错误" + ex.Message };
            }
        }

        /// <summary>
        /// 售后
        /// </summary>
        /// <param name="sysUser"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddAfterSale(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model)
        { 
            Model.WebReturn r = null;
            bool NoError = true;
            this._NewSellListInfo = model.Pro_SellListInfo;

            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    List<string> ProIDS = new List<string>();
                    List<Model.Pro_SellListInfo> selllistTemp = new List<Model.Pro_SellListInfo>();
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放串码
                    List<string> IMEI = new List<string>();


                    #region 验证会员信息
                    if (model.VIP_ID > 0)
                    {
                        var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                        select b;
                        if (vip_query.Count() <= 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该会员不存在" };
                        }
                        var vip = vip_query.First();
                        model.CusName = vip.MemberName;
                        model.CusPhone = vip.MobiPhone;
                    }
                    if (string.IsNullOrEmpty(model.CusName) || string.IsNullOrEmpty(model.CusPhone))
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "客户姓名、电话必填" };
                    }
                    #endregion

                    #region 验证促销 操作人员

                    var user_seller = from b in lqh.Umsdb.Sys_UserInfo
                                      where b.UserID == model.UserID && b.CanLogin == true && b.Flag == true
                                      select b;
                    if (user_seller.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "操作员不存在" };
                    }

                    var user_Oper = from b in lqh.Umsdb.Sys_UserInfo
                                    where b.UserID == model.Seller && b.Flag == true
                                    select b;
                    if (user_Oper.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售员不存在" };
                    }
                    #endregion

                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = model.HallID + "仓库无权操作" };
                    #endregion

                    #region 获取所有商品型号
                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        } 
                        if (!SellType_ProID_List.Contains(m.SellType_Pro_ID))
                            SellType_ProID_List.Add(m.SellType_Pro_ID);
                        //串码
                        if (!string.IsNullOrEmpty(m.IMEI))
                        {
                            if (IMEI.Contains(m.IMEI))
                            {
                                NoError = false;
                                m.Note = m.IMEI + "串码重复";
                                continue;
                            }
                            else
                                IMEI.Add(m.IMEI);
                        }
                        //商品编号
                        if (!ProIDS.Contains(m.ProID) && string.IsNullOrEmpty(m.IMEI))
                            ProIDS.Add(m.ProID);
                    }
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    }
                    #endregion

                    #region 生成验证销售方式、各种价格左连接部分
                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where SellType_ProID_List.Contains(b.ID)
                                            select b).ToList();
                    #endregion


                    #region 库存
                    //串码类拣货
                    var imeiList = (from b in lqh.Umsdb.Pro_IMEI
                                    where IMEI.Contains(b.IMEI) && b.HallID == model.HallID
                                    select b).ToList();

                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == model.HallID && (ProIDS.Contains(b.ProID))
                                     orderby b.InListID
                                     select b).ToList();

                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                       group b by b.ProID into temp
                                       select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();
                    #endregion

                    #region 会员免费的服务
                    var Vip_Free = from b in lqh.Umsdb.VIP_VIPService
                                   where b.VIPID == model.VIP_ID && b.SCount > 0
                                   select b;
                    #endregion

                    #region 左连接库存 拣货 减库存  免费的服务

                    var SellList_join_Store = from b in model.Pro_SellListInfo
                                              join c in StoreList__
                                              on b.ProID equals c.ProID
                                              into temp
                                              from c1 in temp.DefaultIfEmpty()
                                              join d in Pro_SellTypeList
                                              on new { ID = (int)b.SellType_Pro_ID, b.ProID, b.SellType }
                                              equals new { d.ID, d.ProID, d.SellType }
                                              into temp2
                                              from d1 in temp2.DefaultIfEmpty()
                                              join e in Vip_Free
                                              on b.ProID equals e.ProID
                                              into temp3
                                              from e1 in temp3.DefaultIfEmpty()
                                              join f in imeiList
                                              on b.IMEI equals f.IMEI
                                              into temp4
                                              from f1 in temp4.DefaultIfEmpty()
                                              select new { Pro_SellListInfo = b, Pro_StoreInfo = c1, Pro_SellTypeProduct = d1 ,VIP_VIPServer= e1, Pro_IMEI= f1};
                    decimal? cashTotle = 0;
                    foreach (var m in SellList_join_Store)
                    {
                        if (m.Pro_StoreInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "库存不足";
                            continue;
                        }
                        if (m.Pro_SellTypeProduct == null)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "销售方式不存在";
                            continue;
                        }
                        
                        m.Pro_SellListInfo.ProPrice = m.Pro_SellTypeProduct.Price;
                        m.Pro_SellListInfo.OffPoint = 0;
                        m.Pro_SellListInfo.CashTicket = 0;
                        //m.Pro_SellListInfo.IMEI = null;
                        m.Pro_SellListInfo.LowPrice = m.Pro_SellTypeProduct.LowPrice;
                        m.Pro_SellListInfo.ProCost = m.Pro_SellTypeProduct.ProCost;
                        m.Pro_SellListInfo.SpecialID = 0;
                        m.Pro_SellListInfo.TicketID = null;
                        m.Pro_SellListInfo.TicketUsed = 0;
                        m.Pro_SellListInfo.WholeSaleOffPrice = 0;
                        if (m.Pro_SellListInfo.CashPrice != m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.WholeSaleOffPrice - m.Pro_SellListInfo.TicketUsed - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.OffPrice)
                        {
                            NoError = false;
                            m.Pro_SellListInfo.Note = "实收金额不对";
                            continue;
                        }
                        if (NoError)
                        {
                            if (m.Pro_SellListInfo.IsFree == true)
                            {
                                if (m.VIP_VIPServer == null || m.VIP_VIPServer.SCount <= 0)
                                {
                                    NoError = false;
                                    m.Pro_SellListInfo.Note = "该会员免费服务功能已用完";
                                    continue;
                                }
                                if (m.Pro_SellListInfo.CashPrice != 0)
                                {
                                    NoError = false;
                                    m.Pro_SellListInfo.Note = "免费服务实收金额应为0";
                                    continue;
                                }
                                m.VIP_VIPServer.SCount -= m.Pro_SellListInfo.ProCount;
                            }
                        }

                        cashTotle += m.Pro_SellListInfo.CashPrice;
                        if (!NoError) continue;

                        #region 串码类验证
                        if (!string.IsNullOrEmpty(m.Pro_SellListInfo.IMEI))
                        {
                            r = CheckIMEI(m.Pro_IMEI);
                            if (!r.ReturnValue)
                            {
                                NoError = false;
                                m.Pro_SellListInfo.Note = r.Message;
                                continue;
                            }
                            m.Pro_IMEI.Pro_StoreInfo.ProCount--;
                            m.Pro_IMEI.Pro_SellInfo = model;

                        }
                        #endregion
                        #region 非串码类验证
                        if (string.IsNullOrEmpty(m.Pro_SellListInfo.IMEI))
                        {
                            r = FitInOrderListIDNoIMEI(m.Pro_SellListInfo, m.Pro_StoreInfo, selllistTemp);
                            if (!r.ReturnValue == true)
                            {
                                NoError = false;
                                continue;
                            }
                        }
                        #endregion
                          
                        

                    }
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    }
                    #endregion

                    #region 存在未拣完货的
                    if (selllistTemp.Count > 0)
                    {
                        r = FitInOrderListIDNoIMEI(selllistTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        model.Pro_SellListInfo.AddRange(selllistTemp);
                    }
                    #endregion

                    #region 验证最后总收入
                    if (model.CashTotle != cashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "总收入有误" };
                    }
                    if (model.CashTotle - model.OffTicketPrice != model.CardPay + model.CashPay)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "刷卡、现金收入合计有误" };
                    }
                    #endregion
                    lqh.Umsdb.Pro_SellInfo.InsertOnSubmit(model);
                    lqh.Umsdb.SubmitChanges();

                    return r = new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
                }
            }
            catch (Exception ex)
            {
                return r = new Model.WebReturn() { ReturnValue = false, Message = "服务器错误" + ex.Message };
            }
        }



        /// <summary>
        ///  对未拣货的商品 进行拣货1
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sellListTemp"></param>
        /// <param name="StoreList"></param>
        /// <returns></returns>
        public Model.WebReturn FitInOrderListIDNoIMEI(List<Model.Pro_SellListInfo> sellListTemp,List<Model.Pro_StoreInfo> StoreList)
        {
            Model.WebReturn r=null;
            //model.Pro_SellListInfo.AddRange(sellListTemp);
            


            while (sellListTemp.Count > 0)
            {
                var StoreList__ = (from b in StoreList
                                   where b.ProCount > 0
                                   group b by b.ProID into temp
                                   select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();
                var Join_query = (from b in sellListTemp
                                 join c in StoreList__
                                 on b.ProID equals  c.ProID
                                 select new
                                 {
                                     Pro_SellListInfo=b,
                                     Pro_StoreInfo=c
                                 }).ToList();
                if (Join_query.Count != sellListTemp.Count)
                {
                    return new WebReturn(){Message = "库存不足以本次销售",ReturnValue = false};
                }
                sellListTemp.Clear();
                foreach (var m in Join_query)
                {
                    r= FitInOrderListIDNoIMEI(m.Pro_SellListInfo, m.Pro_StoreInfo, sellListTemp);
                    if (r.ReturnValue != true)
                    {
                        return r;
                    }
                    this._NewSellListInfo.Add(m.Pro_SellListInfo);

                }
            }

            return new Model.WebReturn() {  ReturnValue=true};

        }
       /// <summary>
        /// 对未拣货的商品 进行拣货2
       /// </summary>
       /// <param name="child"></param>
       /// <param name="store"></param>
       /// <param name="sellListTemp"></param>
       /// <returns></returns>
        public Model.WebReturn FitInOrderListIDNoIMEI(Model.Pro_SellListInfo child, Model.Pro_StoreInfo store, List<Model.Pro_SellListInfo> sellListTemp)
        {
            
                if (store == null)
                { 
                    child.Note = "库存不足本次销售";
                    return new Model.WebReturn() { ReturnValue=false };
                }
                //如果第一个记录不够库存
                if (store.ProCount - child.ProCount < 0)
                {

                    #region 拆除不够库存的selllist

                    Model.Pro_SellListInfo mySelllist = new Model.Pro_SellListInfo()
                    {
                        CashTicket = child.CashTicket,
                        LowPrice = child.LowPrice,
                        Note = child.Note,
                        OffID = child.OffID,
                        OffPoint = child.OffPoint,
                        OffPrice = child.OffPrice,
                        OffSepecialPrice = child.OffSepecialPrice,
                        //Pro_ProInfo = child.Pro_ProInfo,
                        //Pro_SellInfo=child.Pro_SellInfo,
                       // Pro_SellSpecalOffList = child.Pro_SellSpecalOffList,
                        ProCost = child.ProCost,
                        ProID = child.ProID,
                        SellType = child.SellType,
                        SellType_Pro_ID = child.SellType_Pro_ID,
                        SpecialID = child.SpecialID,
                        TicketID = child.TicketID,
                        TicketUsed = child.TicketUsed,
                        //VIP_OffList = child.VIP_OffList,
                        ProPrice = child.ProPrice,
                        ProCount = child.ProCount - store.ProCount,
                        WholeSaleOffPrice = child.WholeSaleOffPrice,
                        CashPrice = child.CashPrice,
                        AnBuPrice = child.AnBuPrice,
                        LieShouPrice = child.LieShouPrice,
                        AnBu = child.AnBu,
                        OtherCash = child.OtherCash,
                        OtherOff=child.OtherOff,
                        YanbaoModelPrice = child.YanbaoModelPrice,
                        NeedAduit = child.NeedAduit
                    };
                    if (child.Pro_SellSpecalOffList!=null)
                    child.Pro_SellSpecalOffList.Pro_SellListInfo.Add(mySelllist);
                    if (child.VIP_OffList!=null)
                    child.VIP_OffList.Pro_SellListInfo.Add(mySelllist);
                    //model.Pro_SellListInfo.AddRange(sellListTemp);

                    //child.Pro_SellInfo.Pro_SellListInfo.Add(mySelllist);
                    sellListTemp.Add(mySelllist);
                    child.CashPrice = child.CashPrice;
                    child.ProCount = store.ProCount;
                    
                    store.ProCount -= store.ProCount;
                    #endregion

                }
                //第一个记录够库存
                else
                {
                    store.ProCount -= child.ProCount;
                    if (!this._NewSellListInfo.Contains(child))
                    {
                        this._NewSellListInfo.Add(child);
                    }
                }
                child.InListID = store.InListID;
           child.ProCost = store.Pro_InOrderList.InitInList.Price;
                return new Model.WebReturn() {  ReturnValue=true };
            
        }
        /// <summary>
        /// 对未拣货的赠品 进行拣货1
        /// </summary>
        /// <param name="semdListTemp">待拣货的赠品</param>
        /// <param name="StoreList">待拣货的库存</param>
        /// <returns></returns>
        public Model.WebReturn FitInOrderListIDNoIMEI(List<Model.Pro_SellSendInfo> sendListTemp, List<Model.Pro_StoreInfo> StoreList)
        {
            //Model.WebReturn r = new Model.WebReturn() {  ReturnValue=true};
              
            //var StoreList__ = (from b in StoreList
            //                   where b.ProCount > 0
            //                   group b by b.ProID into temp
            //                   select temp.Single((p => p.InListID == temp.Min(p2 => p2.InListID)))).ToList();
            //while (sendListTemp.Count > 0)
            //{
            //    var Join_query = (from b in sendListTemp
            //                      join c in StoreList__
            //                      on b.ProID equals c.ProID
            //                      select new
            //                      {
            //                          Pro_SellSendInfo = b,
            //                          Pro_StoreInfo = c
            //                      }).ToList();
            //    sendListTemp.Clear();
            //    foreach (var m in Join_query)
            //    {
            //        r = FitInOrderListIDNoIMEI(m.Pro_SellSendInfo, m.Pro_StoreInfo, sendListTemp);
            //        if (r.ReturnValue != true)
            //        {
            //            return r;
            //        }
            //    }
            //}

            return new Model.WebReturn() { ReturnValue = true };

        }
        /// <summary>
        /// 对未拣货的赠品 进行拣货2
        /// </summary>
        /// <param name="child"></param>
        /// <param name="store"></param>
        /// <param name="sellListTemp"></param>
        /// <returns></returns>
        public Model.WebReturn FitInOrderListIDNoIMEI222(Model.Pro_SellSendInfo child, Model.Pro_StoreInfo store, List<Model.Pro_SellSendInfo> SendListTemp)
        {

            //if (store == null)
            //{
            //    child.Note = "库存不足本次销售";
            //    return new Model.WebReturn() { ReturnValue = false };
            //}
            ////如果第一个记录不够库存
            //if (child.ProCount - store.ProCount < 0)
            //{

            //    #region 拆除不够库存的赠品列表

            //    Model.Pro_SellSendInfo mySelllist = new Model.Pro_SellSendInfo()
            //    {
            //        Note = child.Note,
            //        OffID = child.OffID,
            //        Pro_SellInfo = child.Pro_SellInfo,
            //        Pro_SellListInfo = child.Pro_SellListInfo,
            //        Pro_SellSpecalOffList = child.Pro_SellSpecalOffList,
            //        ProCost = child.ProCost,
            //        ProCount = child.ProCount - store.ProCount,
            //        ProID = child.ProID,
            //        VIP_SendProOffList = child.VIP_SendProOffList,
            //        Pro_SellBack=child.Pro_SellBack
            //    };
            //    SendListTemp.Add(mySelllist);
            //    child.ProCount -= store.ProCount;
            //    store.ProCount = 0;
            //    #endregion

            //}
            ////第一个记录够库存
            //else
            //{
            //    store.ProCount -= child.ProCount;
            //}
            //child.InOrderList = store.InListID;
            return new Model.WebReturn() { ReturnValue = true };

        }

        /// <summary>
        /// 使用优惠券，满多少减多少
        /// </summary>
        /// <param name="vip"></param>
        /// <param name="sell"></param>
        /// <param name="offList"></param>
        /// <returns></returns>
        public Model.WebReturn CheckTicketOff(Model.Pro_SellInfo model, Model.VIP_VIPInfo vip)
        {
            if (model.OffTicketID == 0 || model.OffTicketID == null||vip==null)
            {
                if (model.OffTicketPrice != 0 && model.OffTicketPrice != null)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = model.OffTicketID + "未使用优惠券，却优惠了金额"+model.OffTicketPrice };
                }
                return new Model.WebReturn() {  ReturnValue=true};
            }

            var a = from b in vip.VIP_OffTicket

                    where b.Used != true && b.ID == model.OffTicketID
                    && b.VIP_ID == vip.ID && model.CashTotle >= b.VIP_OffList.ArriveMoney
                    && b.VIP_OffList.Flag == true
                    && b.VIP_OffList.EndDate >= DateTime.Now
                    && b.VIP_OffList.StartDate <= DateTime.Now
                    && b.VIP_OffList.Type == 3                                     
                    select b;
            if (a.Count() <= 0)
            {

                return new Model.WebReturn() { ReturnValue = false, Message = model.OffTicketID + "优惠券使用出错", Obj = model };

            }
            var  a_one = a.First();
            a_one.Used = true;
            a_one.UseDate = DateTime.Now;
            

            model.OffTicketPrice = model.CashTotle >= a_one.VIP_OffList.OffMoney ? a_one.VIP_OffList.OffMoney : model.CashTotle;
            
            //decimal? i = model.CashTotle - a_one.VIP_OffList.OffMoney >= 0 ? model.CashTotle - a_one.VIP_OffList.OffMoney : 0;
            if (model.CashTotle - model.OffTicketPrice != model.CardPay + model.CashPay)
                return new Model.WebReturn() { ReturnValue = false, Message = "实收总金额有误", Obj = model };


            return new Model.WebReturn() {  ReturnValue=true };
            
        }
        /// <summary>
        /// 循环 组合优惠列表，过滤
        /// </summary>
        /// <param name="selllist"></param>
        /// <param name="prooff_List"></param>
        /// <param name="SellSpecalOffList"></param>
        /// <returns></returns>
        public Model.WebReturn CheckSpecialOff(Model.Pro_SellInfo sell, List<Model.VIP_OffList> offList)
        {
            List<Model.Pro_SellListInfo> list_temp = new List<Model.Pro_SellListInfo>();
            foreach (Model.Pro_SellSpecalOffList m in sell.Pro_SellSpecalOffList)
            {
                offList.Where(p=>p.ID==m.SpecalOffID);
                if (offList.Count == 0)
                {
                    return new Model.WebReturn() { ReturnValue=false, Message=m.SpecalOffID +"组合优惠错误" };
                }
                m.VIP_OffList = offList.First();

               Model.WebReturn r= SellListToOff(m, sell.Pro_SellListInfo.ToList());
               if (r.ReturnValue != true) return r;
               list_temp.AddRange((List<Model.Pro_SellListInfo>)r.Obj);
            }
            return new Model.WebReturn() { ReturnValue=true , Obj=list_temp };
        }
        public Model.WebReturn CheckSpecialOff(Model.Pro_SellListInfo model, Model.VIP_ProOffList ProOff_1)
        {
            //if(model.OffSepecialPrice!=ProOff_1.AfterOffPrice
            if (model.SpecialID == null || model.SpecialID == 0)
            {
                if (model.OffSepecialPrice != 0 && model.OffSepecialPrice != null)
                {
                    model.Note = "组合优惠条件不符";
                    return new Model.WebReturn() { ReturnValue = false };
                }
                return new Model.WebReturn() { ReturnValue = true };
            }
            else if (ProOff_1 == null )
            {
                model.Note = "组合优惠或套餐不存在";
                return new Model.WebReturn() { ReturnValue = false };
            }
            //model.Pro_SellSpecalOffList = ProOff_1;
           
            decimal? i = model.ProPrice - model.TicketUsed - model.AnBuPrice - model.OffPrice;

            if (i - ProOff_1.AfterOffPrice >= 0)
            {
                if (model.OffSepecialPrice != i - ProOff_1.AfterOffPrice)
                {
                    model.Note = "组合优惠了的金额为" + ProOff_1.AfterOffPrice;
                    return new Model.WebReturn() { ReturnValue = false };
                }
            }
            else
            {
                if (model.OffSepecialPrice != 0)
                {
                    model.Note = "组合优惠了的金额为" + 0;
                    return new Model.WebReturn() { ReturnValue = false };
                }
            }
            //            i = i - model.OffSepecialPrice;
            //
            //            if (i != model.CashPrice)
            //            {
            //                model.Note = "组合优惠侯，实收金额计算有误,正确为"+ i;
            //                return new Model.WebReturn() { ReturnValue = false };
            //            }
            model.Pro_SellSpecalOffList.OffMoney += model.OffSepecialPrice;
            return new Model.WebReturn() { ReturnValue = true };
        }
        public Model.WebReturn CheckSpecialOff(Model.Pro_SellListInfo model,Model.VIP_ProOffList ProOff_1,Model.Package_ProInfo groupProOff)
        {
            //if(model.OffSepecialPrice!=ProOff_1.AfterOffPrice
            if ((model.SpecialID == null || model.SpecialID==0) && groupProOff==null)
            {
                if (model.OffSepecialPrice != 0 )
                {
                    model.Note = "未使用组合优惠，却有组合优惠金额";
                    return new Model.WebReturn() {  ReturnValue=false}; 
                }
                return new Model.WebReturn() {  ReturnValue=true};
            }
            else if (ProOff_1 == null && groupProOff==null)
            {
                model.Note = "组合优惠或套餐不存在";
                return new Model.WebReturn() { ReturnValue = false }; 
            }
            //model.Pro_SellSpecalOffList = ProOff_1;
            //如果不是组合优惠
            if ( ProOff_1==null||ProOff_1.VIP_OffList.Type != 1 ) 
                return new Model.WebReturn() { ReturnValue = true };
            decimal? i = model.ProPrice-model.TicketUsed-model.AnBuPrice -  model.OffPrice;

            if (i - ProOff_1.AfterOffPrice >= 0)
            {
                if (model.OffSepecialPrice != i-ProOff_1.AfterOffPrice)
                {
                    model.Note = "组合优惠了的金额为" + ProOff_1.AfterOffPrice;
                    return new Model.WebReturn() { ReturnValue = false };
                }
            }
            else
            {
                if (model.OffSepecialPrice != 0)
                {
                    model.Note = "组合优惠了的金额为" + 0;
                    return new Model.WebReturn() { ReturnValue = false };
                }
            }
//            i = i - model.OffSepecialPrice;
//
//            if (i != model.CashPrice)
//            {
//                model.Note = "组合优惠侯，实收金额计算有误,正确为"+ i;
//                return new Model.WebReturn() { ReturnValue = false };
//            }
            model.Pro_SellSpecalOffList.OffMoney += model.OffSepecialPrice;
            return new Model.WebReturn() { ReturnValue = true };
        }
        /// <summary>
        /// 组合优惠 匹配 过程
        /// </summary>
        /// <param name="m"></param>
        /// <param name="selllist"></param>
        /// <returns></returns>
        private Model.WebReturn SellListToOff(Model.Pro_SellSpecalOffList m, List<Model.Pro_SellListInfo> selllist)
        {
            List<Model.Pro_SellListInfo> tempList = new List<Model.Pro_SellListInfo>();
            for (int i = 0; i < m.VIP_OffList.VIP_ProOffList.Count(); i++)
            {
                Model.VIP_ProOffList proOff = m.VIP_OffList.VIP_ProOffList[i];
                for (int j = 0; j < proOff.ProCount; j++)
                {
                    var a = from b in selllist
                            where b.SellType == proOff.SellTypeID
                            && b.ProID == proOff.ProID && b.ProCount >= 1
                            select b;
                    if (a.Count() <= 0)
                        return new Model.WebReturn() { ReturnValue = false, Message = m.VIP_OffList.Name + ",组合优惠信息有误:销售类别" + proOff.Pro_SellType.Name + "," + proOff.Pro_ProInfo.ProName + "不满足组合数量" };

                    var c = a.First();
                    decimal single = c.CashPrice / c.ProCount;

                     Model.Pro_SellListInfo selllist_one = new Model.Pro_SellListInfo() { 
                    CashPrice=single,
                    CashTicket=c.CashTicket,
                    InListID=c.InListID,
                    LowPrice=c.LowPrice,
                    OffID=c.OffID,
                    Note=c.Note,
                    OffPoint=c.OffPoint,
                    ProCost=c.ProCost,
                    TicketUsed=c.TicketUsed,
                    TicketID=c.TicketID,
                    ProPrice=c.ProPrice,
                    SellType=c.SellType,
                    ProCount=c.ProCount,
                    ProID=c.ProID

                    };

                    c.CashPrice = single * (c.ProCount - 1);
                    c.ProCount = c.ProCount - 1;

                    Model.Pro_SellIMEIList imei = null;
                    if (c.Pro_SellIMEIList.Count > 0)
                    {
                        imei = c.Pro_SellIMEIList.First();
                        imei.CashPrice = single;
                        c.Pro_SellIMEIList.Remove(imei);
                        m.Pro_SellIMEIList.Add(imei);
                        
                    }
                    else
                        imei = new Model.Pro_SellIMEIList()
                        {
                            CashPrice=single,
                            Pro_SellSpecalOffList=m
                        };
                     

                    selllist_one.Pro_SellIMEIList.Add(imei);
                    tempList.Add(selllist_one);

                }
            }
            selllist.RemoveAll(p=>p.ProCount==0);
            tempList.AddRange(selllist);
            return new Model.WebReturn() { ReturnValue=true , Obj=tempList };
        }
        /// <summary>
        /// 验证单品优惠信息
        /// </summary>
        /// <param name="selllist"></param>
        /// <param name="off_List"></param>
        /// <returns></returns>
        public Model.WebReturn CheckProOff(List<Model.Pro_SellListInfo> selllist, List<Model.VIP_ProOffList> prooff_List)
        {
            
            var SellList_query = from b in selllist
                                 join c in prooff_List
                                 on new { b.ProID, SellType = b.SellType }
                                 equals
                                 new { c.ProID, SellType = c.SellTypeID }
                                 
                                 into temp
                                 from d in temp.DefaultIfEmpty()
                                 where d.VIP_OffList.Type==0
                                 select new Model.Pro_SellListInfo
                                 {
                                     CashPrice = b.CashPrice,
                                     CashTicket = b.CashTicket,
                                     TicketID = b.TicketID,
                                     TicketUsed = b.TicketUsed,
                                     InListID = b.InListID,
                                     LowPrice = b.LowPrice,
                                     ProCost = b.ProCost,
                                     ProPrice = b.ProPrice,
                                     Note = b.Note,
                                     OffID = b.OffID,
                                     VIP_OffList=d.VIP_OffList,
                                     OffPoint=b.OffPoint,
                                     ProID = b.ProID,
                                     ProCount = b.ProCount,
                                     SellType = b.SellType,
                                     Pro_ProInfo =  b.Pro_ProInfo,
                                     Pro_SellType = b.Pro_SellType,

                                 };
            //单品优惠 
            foreach (Model.Pro_SellListInfo m in SellList_query)
            {
                Model.WebReturn r=null;
                if (m.OffID > 0)
                {
                    r = MoneyOrRateOrPoint(m);
                    if (r.ReturnValue != true) return r;
                }
                else
                {
                    m.OffPoint = 0;
                } 
            }
            //
            return new Model.WebReturn() { ReturnValue = true, Message = "成功" };

           
        }
        /// <summary>
        /// 单品验证，
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Model.WebReturn CheckProOff(Model.Pro_SellListInfo m)
        {
            #region 未使用优惠
            if (m.OffID == 0 || m.OffID==null)
            {
                if ( (m.OffPoint != 0 && m.OffPoint != null )|| (m.OffPrice != 0 && m.OffPrice != null))
                {
                    m.Note = "未使用优惠，但是积分和优惠金额不为0";
                    return new Model.WebReturn() { ReturnValue = false };
                }
                return new Model.WebReturn() { ReturnValue = true };
            }
            else if(m.VIP_OffList==null)
            {
                m.Note = "优惠不存在";
                return new Model.WebReturn() { ReturnValue = false, Message = "优惠不存在" };
            }
            #endregion
                
                
                Model.WebReturn r = null;
                //优惠
                //if(m.VIP_OffList==null && m.Pro_SellSendInfo.Count()>0)
                //{
                //    return new Model.WebReturn(){ ReturnValue=false, Message="优惠不存在"};
                //}
                r = MoneyOrRateOrPoint(m);
                if (r.ReturnValue != true) return r;
       
                if (m.VIP_OffList.VIPTicketMaxCount != 99999 && m.VIP_OffList.VIPTicketMaxCount - m.VIP_OffList.UseLimit<m.ProCount)
                {
                    m.Note = "优惠数量超限额";

                    return new Model.WebReturn {  ReturnValue=false};
                }
                
                m.VIP_OffList.UseLimit += m.ProCount;

                //赠品
                
                //if (m.Pro_SellSendInfo.Count() > 0)
                //{ 
                //    var aa=from b in m.Pro_SellSendInfo
                //          group b by new { b.SendProOffID, b.ProID } into temp
                //          from c in temp
                //          select new {c.SendProOffID,c.ProID, ProCount= temp.Sum(p=>p.ProCount)};
                //    var bb = from b in aa
                //             join c in m.VIP_OffList.VIP_SendProOffList
                //             on new { ID = (int)b.SendProOffID, b.ProID, PerCount = b.ProCount }
                //             equals new { c.ID, c.ProID, c.PerCount } into temp
                //             from d in temp.DefaultIfEmpty()
                //             select new { b,d };
                //    foreach (var b in bb)
                //    {
                //        if (b.d == null || b.d.ProCount + b.b.ProCount <= b.d.LimitCount)
                //        {
                //            m.Note = "赠品列表有误或者赠品已送完";
                //            return new Model.WebReturn() { ReturnValue = false, Message = m.Note };
                //        }
                //        else b.d.ProCount +=b.b.ProCount;
                //    }
                //    sendList.AddRange(m.Pro_SellSendInfo);
                //}

                    
                return new Model.WebReturn() { ReturnValue = true };
        }
        /// <summary>
        /// 计算优惠金额
        /// </summary>
        /// <param name="m"></param>
        /// <param name="off"></param>
        /// <returns></returns>
        private Model.WebReturn  MoneyOrRateOrPoint(Model.Pro_SellListInfo m )
        {
            decimal? OffPrice = 0;
            Model.VIP_OffList off = m.VIP_OffList;
            if (off == null)
            {
                OffPrice = 0;
                //return new Model.WebReturn { ReturnValue = false, Message = m.OffID + "优惠信息有误" };

            }
//            else if (off.OffRate > 0)
//            {
//                OffPrice =m.ProPrice - (m.ProPrice -m.TicketUsed) * off.OffRate;
//                //m.CashPrice = m.ProPrice * off.OffRate;
//
//            }
//            else if (off.OffMoney > 0)
//            {
//                OffPrice = (m.ProPrice - m.TicketUsed) - off.OffMoney >= 0 ? (m.ProPrice - m.TicketUsed) - off.OffMoney : (m.ProPrice - m.TicketUsed);
//                //m.CashPrice = (m.CashPrice - off.OffMoney >= 0 ? m.CashPrice - off.OffMoney : 0);
//
//            }
//            else if (off.OffPoint > 0 && off.MinPoint <= m.OffPoint / m.ProCount && m.OffPoint / m.ProCount <= off.MaxPoint)
//            {
//                OffPrice = off.OffPointMoney * m.OffPoint / off.OffPoint;
//
//                if ((m.ProPrice - m.TicketUsed) - OffPrice < 0) return new Model.WebReturn() { ReturnValue = false, Message = m.Pro_ProInfo.ProName + "积分使用超限,产品当前结算单价为" + (m.ProPrice - m.TicketUsed) };
//                //else OffPrice = i;
//            }
//            else
//            {
//                m.Note = m.OffID + "优惠信息有无";
//                return new Model.WebReturn() { ReturnValue = false, Message = m.Note };
//            }
            try
            {
                var prooff = off.VIP_ProOffList.First(p => p.ProID == m.ProID);
                OffPrice =m.ProPrice*(1-prooff.Rate)+  prooff.OffMoney;
            }
            catch (Exception)
            {
                return new Model.WebReturn() { ReturnValue = false, Message ="优惠信息无效" };
            }
            if (OffPrice != m.OffPrice)
            {
                m.Note = m.ProID + "单品优惠金额计算有误";
                return new Model.WebReturn() { ReturnValue = false, Message =  m.Note};
            }
            return new Model.WebReturn() { ReturnValue = true, Message = "成功" };
        }
        /// <summary>
        /// 验证销售方式的有效性
        /// </summary>
        /// <returns></returns>
        public List<Model.Pro_SellListInfo> CheckProSellType(List<Model.Pro_SellListInfo> sellList, List<int?> SellType_Pro_IDs, LinQSqlHelper lqh)
        {
            //var ProID_List = from b in sellList
            //                 select b.ProID;
            //var Pro_List =( from b in lqh.Umsdb.Pro_ProInfo
                           //where ProID_List.Contains(b.ProID)
                           //select b).ToList();
            var Pro_SellTypeList=(from b in lqh.Umsdb.Pro_SellTypeProduct
                                  where SellType_Pro_IDs.Contains(b.ID)
                                 select b).ToList();
            
            var SellList_query=from b in sellList
                               join  c in Pro_SellTypeList 
                               on new {b.ProID,b.SellType} equals new {c.ProID,c.SellType}  
                               into temp
                               from d in temp.DefaultIfEmpty()
 
                               select new Model.Pro_SellListInfo{
                                   CashPrice = (d == null ? 0 : d.Price),
                                   CashTicket=b.CashTicket,
                                   TicketID=b.TicketID,
                                   TicketUsed=b.TicketUsed,
                                   InListID=b.InListID,
                                   LowPrice=b.LowPrice,
                                   ProCost=b.ProCost,
                                   OffPoint=b.OffPoint,
                                   //ProPrice = (d == null ? null : d.Price),
                                   Note = GetErrorMsg(b,d),
                                   OffID=b.OffID,
                                   ProID=b.ProID,
                                   ProCount=b.ProCount, 
                                   SellType=b.SellType,
                                   Pro_ProInfo = (d == null ? null : d.Pro_ProInfo),
                                   Pro_SellType = (d == null ? null : d.Pro_SellType),
                                   Pro_SellTypeProduct=d

                               };
            return SellList_query.ToList();
        }
        /// <summary>
        /// 验证代金券的有效性
        /// </summary> 
        /// <returns></returns>
        public Model.WebReturn CheckProCashTicket(List<Model.Pro_SellListInfo> sellList, LinQSqlHelper lqh)
        {

            List<Model.Pro_CashTicket> cashTickList=new List<Model.Pro_CashTicket>();
            List<string> str_ticket = new List<string>();
            #region MyRegion
            //var SelllistTemp = from b in model.Pro_SellListInfo
            //                   join c in query_Pro_SellTypeProduct
            //                   on new { b.ProID, b.SellType }
            //                   equals
            //                   new { c.ProID, c.SellType }
            //                   where (!string.IsNullOrEmpty(b.TicketID) && c.IsTicketUseful==true)
            //                   || string.IsNullOrEmpty(b.TicketID)
            //                   select new Model.Pro_SellListInfo
            //                   {
            //                       CashPrice=b.CashPrice,
            //                       CashTicket=b.CashTicket,
            //                       LowPrice=b.LowPrice,
            //                       InListID=b.InListID,
            //                       Note=b.Note,
            //                       OffID=b.OffID,
            //                       ProCost=b.ProCost,
            //                       TicketUsed=b.TicketUsed,
            //                       ProID=b.ProID,
            //                       ProCount=b.ProCount, 
            //                       ProPrice=c.Price,
            //                       SellType=b.SellType,
            //                       TicketID=b.TicketID

            //                   };
            ////不存在销售方式或者部分机型不存在销售方式
            //if (SelllistTemp.Count() ==0|| SelllistTemp.Where(p => p.ProPrice == null).Count() > 0)
            //{
            //    return false;
            //}
            //return false;
            #endregion
            foreach (Model.Pro_SellListInfo model in sellList)
            {
                cashTickList.Add(new Model.Pro_CashTicket() {  Pro_SellListInfo=model, TicketID=model.TicketID});

                str_ticket.Add(model.TicketID);

                Model.Pro_ProInfo band = model.Pro_ProInfo;



                string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                decimal ticket = model.CashTicket; ;
                if (DAL.ValidClassInfo.IsDateTime(date))
                {
                    return new Model.WebReturn() { Message= model.TicketID + "券编码格式不对", ReturnValue=false};
                }
                if (Convert.ToDateTime(date) < band.SepDate && band.BeforeRate == 0)
                { 
                    return new Model.WebReturn() { Message =  band.SepDate + "之前的券不能兑换" + band.ProName, ReturnValue = false };
                }
                else if (Convert.ToDateTime(date) > band.SepDate && band.AfterRate == 0)
                { 
                    return new Model.WebReturn() { Message =  band.SepDate + "之后的券不能兑换" + band.ProName, ReturnValue = false };
                }
                else if (Convert.ToDateTime(date) < band.SepDate)
                    ticket += band.BeforeRate;
                else
                    ticket += band.AfterRate;


                if (model.CashTicket < band.TicketLevel)//小于临界值
                {
                    ticket += band.BeforeTicket;
                    model.TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                    //lbl.Text += "1";

                }
                else//大于等于临界值
                {
                    ticket += band.AfterTicket;
                    if (band.NeedMoreorLess==true)//需要补差
                        model.TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                    else
                    {
                        model.TicketUsed = model.ProPrice;//<= ticket ? ticket : model.ProPrice;
                        //if(band.TicketLevel> 0)
                        //    model.ProPrice = model.TicketPrice;
                    } 
                }
                model.CashPrice = model.ProPrice - model.TicketUsed;
            }
            var a = from b in lqh.Umsdb.Pro_CashTicket
                    where str_ticket.Contains(b.TicketID) &&  b.IsBack!=true
                    select b;

            if(a.Count()>0)
                return new Model.WebReturn() { ReturnValue=false, Message=a.First().TicketID+"代金券已被使用" };
            lqh.Umsdb.Pro_CashTicket.InsertAllOnSubmit(cashTickList);
            return new Model.WebReturn() { ReturnValue = true};
            
        }

        public Model.WebReturn CheckProCashTicket2(Model.Pro_SellListInfo model, Model.Pro_ProInfo band)
        {
            
            switch (model.SellType)
            {
                case 16:
                case 14:
                case 13:
                case 10:
                {
                    if (string.IsNullOrEmpty(model.TicketID))
                    {
                        return new Model.WebReturn() {ReturnValue = true};
                        break;
                    }
                    else
                    {
                        try
                        {
                            {
                                string ZSREG = @"^7[56]020(\d\d)(\d\d)(\d\d)(\d{11})(\+\d){0,1}$";
                                string ZSREG2 = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{6})(\+\d){0,1}$";
                                Regex ex = new Regex(ZSREG);
                                Regex ex2 = new Regex(ZSREG2);
                                Regex ok;
                                if (!ex.IsMatch(model.TicketID))
                                {
                                    if (!ex2.IsMatch(model.TicketID))
                                    {
                                        throw new Exception(model.TicketID + "合约码或购机送费码格式不对");
                                    }
                                    else
                                    {
                                        ok = ex2;
                                    }
                                }
                                else
                                {
                                    ok = ex;
                                }
                                MatchCollection match = ok.Matches(model.TicketID);
                                string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" +
                                              match[0].Groups[3];
                                decimal? ticket = model.CashTicket;
                                ;
                                if (!DAL.ValidClassInfo.IsDateTime(date))
                                {
                                    model.Note = model.TicketID + "合约码或购机送费码格式不对";
                                    return new Model.WebReturn() {Message = model.Note, ReturnValue = false};
                                }
                                return new Model.WebReturn() {ReturnValue = true};
                                break;
                            }
                        }
                        catch 
                        {
                             goto case 2;
                        }
                       
                    }
                }

                case 2:
                case 7:
                case 9:
                    {
                    decimal? TicketUsed = 0;

                        
                    string ZSREG = @"^S7[56]0(\d\d)(\d\d)(\d\d)([0-9A-Z]{8})$";
                    Regex ex = new Regex(ZSREG);
                    
                    if (!ex.IsMatch(model.TicketID+""))
                    {
                        throw new Exception(model.TicketID + "券编码格式不对");
                    }
                    MatchCollection match = ex.Matches(model.TicketID);
                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                    decimal? ticket = model.CashTicket;
                    ;
                    if (!DAL.ValidClassInfo.IsDateTime(date))
                    {
                        model.Note = model.TicketID + "券编码格式不对";
                        return new Model.WebReturn() {Message = model.Note, ReturnValue = false};
                    }
                    if (Convert.ToDateTime(date) < band.SepDate && !band.BeforeSep)
                    {
                        model.Note = band.SepDate + "之前的券不能兑换" + band.ProName;
                        return new Model.WebReturn() {Message = model.Note, ReturnValue = false};
                    }
                    else if (Convert.ToDateTime(date) > band.SepDate && !band.AfterSep)
                    {
                        model.Note = band.SepDate + "之后的券不能兑换" + band.ProName;
                        return new Model.WebReturn() {Message = model.Note, ReturnValue = false};
                    }
                    else if (Convert.ToDateTime(date) < band.SepDate)
                        ticket += band.BeforeRate;
                    else
                        ticket += band.AfterRate;


                    if (model.CashTicket < band.TicketLevel) //小于临界值
                    {
                        ticket += band.BeforeTicket;
                        TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                        //lbl.Text += "1";

                    }
                    else //大于等于临界值
                    {
                        ticket += band.AfterTicket;
                        if (band.NeedMoreorLess == true) //需要补差
                            TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                        else
                        {
                            TicketUsed = model.ProPrice; //<= ticket ? ticket : model.ProPrice;
                            //if(band.TicketLevel> 0)
                            //    model.ProPrice = model.TicketPrice;
                        }
                    }
                    //model.CashPrice = model.ProPrice - model.TicketUsed;
                    if (model.TicketUsed != TicketUsed)
                    {
                        model.Note = model.TicketID + "实际兑换值不正确，只能兑换" + TicketUsed;
                        return new Model.WebReturn() {ReturnValue = false, Message = model.Note};
                    }
                    return new Model.WebReturn() {ReturnValue = true};
                    break;
                    }
                case 11:
                case 12:
                case 4:
                    {
                        string ZSREG = @"^7[56]020(\d\d)(\d\d)(\d\d)(\d{11})(\+\d){0,1}$";
                        string ZSREG2 = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{6})(\+\d){0,1}$";
                        Regex ex = new Regex(ZSREG);
                        Regex ex2 = new Regex(ZSREG2);
                        Regex ok;
                        if (!ex.IsMatch(model.TicketID + ""))
                        {
                            if (!ex2.IsMatch(model.TicketID + ""))
                            {
                                throw new Exception(model.TicketID + "合约码或购机送费码格式不对");
                            }
                            else
                            {
                                ok = ex2;
                            }
                        }
                        else
                        {
                            ok = ex;
                        }
                        MatchCollection match = ok.Matches(model.TicketID + "");
                        string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                        decimal? ticket = model.CashTicket;
                        ;
                        if (!DAL.ValidClassInfo.IsDateTime(date))
                        {
                            model.Note = model.TicketID + "合约码或购机送费码格式不对";
                            return new Model.WebReturn() { Message = model.Note, ReturnValue = false };
                        }
                        return new Model.WebReturn() { ReturnValue = true };
                        break;
                    }
                
                case 5:
                    {
                        
                    
                    decimal? TicketUsed = 0;



                    string ZSREG = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{9})$";
                    Regex ex = new Regex(ZSREG);
                    if (!ex.IsMatch(model.TicketID + ""))
                    {
                        throw new Exception(model.TicketID + "券编码格式不对");
                    }
                    MatchCollection match = ex.Matches(model.TicketID + "");
                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                    decimal? ticket = model.CashTicket;
                    ;
                    if (!DAL.ValidClassInfo.IsDateTime(date))
                    {
                        model.Note = model.TicketID + "券编码格式不对";
                        return new Model.WebReturn() {Message = model.Note, ReturnValue = false};
                    }
//                    if (Convert.ToDateTime(date) < band.SepDate && !band.BeforeSep)
//                    {
//                        model.Note = band.SepDate + "之前的券不能兑换" + band.ProName;
//                        return new Model.WebReturn() {Message = model.Note, ReturnValue = false};
//                    }
//                    else if (Convert.ToDateTime(date) > band.SepDate && !band.AfterSep)
//                    {
//                        model.Note = band.SepDate + "之后的券不能兑换" + band.ProName;
//                        return new Model.WebReturn() {Message = model.Note, ReturnValue = false};
//                    }
//                    else if (Convert.ToDateTime(date) < band.SepDate)
//                        ticket += band.BeforeRate;
//                    else
//                        ticket += band.AfterRate;
//
//
//                    if (model.CashTicket < band.TicketLevel) //小于临界值
//                    {
//                        ticket += band.BeforeTicket;
//                        TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
//                        //lbl.Text += "1";
//
//                    }
//                    else //大于等于临界值
//                    {
//                        ticket += band.AfterTicket;
//                        if (band.NeedMoreorLess == true) //需要补差
//                            TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
//                        else
//                        {
//                            TicketUsed = model.ProPrice; //<= ticket ? ticket : model.ProPrice;
//                            //if(band.TicketLevel> 0)
//                            //    model.ProPrice = model.TicketPrice;
//                        }
//                    }
//                    //model.CashPrice = model.ProPrice - model.TicketUsed;
//                    if (model.TicketUsed != TicketUsed)
//                    {
//                        model.Note = model.TicketID + "实际兑换值不正确，只能兑换" + TicketUsed;
//                        return new Model.WebReturn() {ReturnValue = false, Message = model.Note};
//                    }
                    return new Model.WebReturn() {ReturnValue = true};
                   
                    break;
                    }
                default:
                    return new Model.WebReturn() {ReturnValue = true};
            }

        }

        public WebReturn GetCashTicketUsed(Model.Pro_SellListInfo model, Model.Pro_ProInfo band)
        {
            try
            {
                return new WebReturn() {ReturnValue = true, Obj = CheckProCashTicket(model, band)};
            }
            catch (Exception ex)
            {
                return new WebReturn()
                {
                    ReturnValue = false,
                    Obj = 0,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// 获取优惠时计算代金券 实际使用
        /// </summary>
        /// <param name="model"></param>
        /// <param name="band"></param>
        /// <returns></returns>
        public decimal CheckProCashTicket(Model.Pro_SellListInfo model,Model.Pro_ProInfo band)
        {

            if (band == null)
            {
                //model.TicketID = null;
                //model.TicketUsed = 0;
                //return 0;
                throw new Exception("商品不存在");
            }
            switch (model.SellType)
            {
                case 14:
                case 16:
                case 13:
                case 10:
                {
                    if (string.IsNullOrEmpty(model.TicketID))
                    {
                         return 0;
                    break;
                    }
                    else
                    {
                        goto case 2;
                    }
                }

                case 2:
                case 7:
                case 9:
                    {decimal TicketUsed = 0;
                    string ZSREG = @"^S7[56]0(\d\d)(\d\d)(\d\d)([0-9A-Z]{8})$";
                    Regex ex = new Regex(ZSREG);
                    if (!ex.IsMatch(model.TicketID+""))
                    {
                        throw new Exception(model.TicketID + "券编码格式不对");
                    }
                    MatchCollection match = ex.Matches(model.TicketID);
                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                    //string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                    decimal ticket = model.CashTicket;
                 
                    if (!DAL.ValidClassInfo.IsDateTime(date))
                    {
                        throw new Exception(model.TicketID + "券编码格式不对");
                    }
                    if (Convert.ToDateTime(date) < band.SepDate && !band.BeforeSep)
                    {
                        throw new Exception(band.SepDate + "之前的券不能兑换" + band.ProName);
                    }
                    else if (Convert.ToDateTime(date) > band.SepDate && !band.AfterSep)
                    {
                        throw new Exception(band.SepDate + "之后的券不能兑换" + band.ProName);
                    }
                    else if (Convert.ToDateTime(date) < band.SepDate)
                        ticket += band.BeforeRate;
                    else
                        ticket += band.AfterRate;


                    if (model.CashTicket < band.TicketLevel) //小于临界值
                    {
                        ticket += band.BeforeTicket;
                        TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                        //lbl.Text += "1";

                    }
                    else //大于等于临界值
                    {
                        ticket += band.AfterTicket;
                        if (band.NeedMoreorLess == true) //需要补差
                            TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
                        else
                        {
                            TicketUsed = model.ProPrice; //<= ticket ? ticket : model.ProPrice;
                            //if(band.TicketLevel> 0)
                            //    model.ProPrice = model.TicketPrice;
                        }
                    }
                    //model.CashPrice = model.ProPrice - model.TicketUsed;

                    return TicketUsed;
                    break;
            }
                case 11:
                case 12:
                case 4:
                    {
                        string ZSREG = @"^7[56]020(\d\d)(\d\d)(\d\d)(\d{11})(\+\d){0,1}$";
                        string ZSREG2 = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{6})(\+\d){0,1}$";
                        Regex ex = new Regex(ZSREG);
                        Regex ex2 = new Regex(ZSREG2);
                        Regex ok;
                        if (!ex.IsMatch(model.TicketID))
                        {
                            if (!ex2.IsMatch(model.TicketID))
                            {
                                throw new Exception(model.TicketID + "合约码或购机送费码格式不对");
                            }
                            else
                            {
                                ok = ex2;
                            }
                        }
                        else
                        {
                            ok = ex;
                        }
                        MatchCollection match = ok.Matches(model.TicketID);
                        string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                        decimal? ticket = model.CashTicket;
                        ;
                        if (!DAL.ValidClassInfo.IsDateTime(date))
                        {
                            throw new Exception(model.TicketID + "编码格式不对");
                        }
                        return 0;
                        break;
                    }

                case 5:
                    {decimal TicketUsed = 0;
                    string ZSREG = @"^7[56]0(\d\d)(\d\d)(\d\d)(\d{9})$";
                    Regex ex = new Regex(ZSREG);
                    if (!ex.IsMatch(model.TicketID))
                    {
                        throw new Exception(model.TicketID + "券编码格式不对");
                    }
                    MatchCollection match = ex.Matches(model.TicketID);
                    string date = "20" + match[0].Groups[1] + "-" + match[0].Groups[2] + "-" + match[0].Groups[3];
                    //string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                    decimal ticket = model.CashTicket;
                    ;
                    if (!DAL.ValidClassInfo.IsDateTime(date))
                    {
                        throw new Exception(model.TicketID + "券编码格式不对");
                    }
//                    if (Convert.ToDateTime(date) < band.SepDate && band.BeforeRate == 0)
//                    {
//                        throw new Exception(band.SepDate + "之前的券不能兑换" + band.ProName);
//                    }
//                    else if (Convert.ToDateTime(date) > band.SepDate && band.AfterRate == 0)
//                    {
//                        throw new Exception(band.SepDate + "之后的券不能兑换" + band.ProName);
//                    }
//                    else if (Convert.ToDateTime(date) < band.SepDate)
//                        ticket += band.BeforeRate;
//                    else
//                        ticket += band.AfterRate;
//
//
//                    if (model.CashTicket < band.TicketLevel) //小于临界值
//                    {
//                        ticket += band.BeforeTicket;
//                        TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
//                        //lbl.Text += "1";
//
//                    }
//                    else //大于等于临界值
//                    {
//                        ticket += band.AfterTicket;
//                        if (band.NeedMoreorLess == true) //需要补差
//                            TicketUsed = model.ProPrice <= ticket ? model.ProPrice : ticket;
//                        else
//                        {
//                            TicketUsed = model.ProPrice; //<= ticket ? ticket : model.ProPrice;
//                            //if(band.TicketLevel> 0)
//                            //    model.ProPrice = model.TicketPrice;
//                        }
//                    }
//                    //model.CashPrice = model.ProPrice - model.TicketUsed;
//
//                    return TicketUsed;
                        return 0;
                    break;
                    }
                default:
                    return 0;
                    break;
            }
        }
        #region 获取优惠
        /// <summary>
        /// 获取优惠
        /// </summary>
        /// <remarks></remarks>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        public Model.WebReturn GetSellOff(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model)
        {

            //if (model == null) return new Model.WebReturn();
            string Msg = "";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                DataLoadOptions d = new DataLoadOptions();
                d.LoadWith<Model.VIP_OffList>(c => c.VIP_HallOffInfo);
                d.LoadWith<Model.VIP_OffList>(c => c.VIP_ProOffList);
                d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPOffLIst);
                d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPTypeOffLIst);
                d.LoadWith<Model.VIP_OffList>(c => c.VIP_SendProOffList);
                lqh.Umsdb.LoadOptions = d;

                try
                {
                    int VIPID = 0;
                    int VIPTypeID = 0;
                    if (model.VIP_ID != null)
                    {
                        var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                        where model.VIP_ID == b.ID
                                        select b;
                        if (vip_query.Count() <= 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该会员不存在" };
                        }
                        var vip = vip_query.First();
                        VIPID = vip.ID;
                        VIPTypeID = Convert.ToInt32(vip.TypeID);
                    }

                    List<string> proIDS = new List<string>();
                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        proIDS.Add(m.ProID);
                        if (m.Pro_SellList_RulesInfo == null || m.Pro_SellList_RulesInfo.Count == 0)
                        {
                        m.Pro_SellList_RulesInfo = new EntitySet<Pro_SellList_RulesInfo>();
                        }
                    }
                    proIDS.Distinct();


                    var Pro_SellType_ProPrice = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                                 where proIDS.Contains(b.ProID)
                                                 select b).ToList();
                    #region 旧
                    
                    
                    //var Selllist_temp = (from b in model.Pro_SellListInfo
                    //                     join
                    //                     c in Pro_SellType_ProPrice
                    //                     on new { b.ProID, b.SellType }
                    //                     equals
                    //                     new { c.ProID, c.SellType }
                    //                     select new Model.Pro_SellListInfo
                    //                     {
                    //                         ID = b.ID,
                    //                         ProID = b.ProID,
                    //                         ProPrice = b.ProPrice,
                    //                         SellType = b.SellType,
                    //                         TicketUsed = CheckProCashTicket(b, c.Pro_ProInfo),
                    //                         TicketID = b.TicketID,
                    //                         Note = b.Note,
                    //                         CashTicket = b.CashTicket,
                    //                         ProCount = b.ProCount,
                    //                         SellType_Pro_ID = c.ID,
                    //                         IMEI = b.IMEI,
                    //                         Pro_Sell_Yanbao = b.Pro_Sell_Yanbao,
                    //                         Pro_Sell_JiPeiKa = b.Pro_Sell_JiPeiKa,
                    //                         VIP_VIPInfo = b.VIP_VIPInfo,
                    //                         SellID = b.SellID,
                    //                         SellID_Temp = b.SellID_Temp,
                    //                         InListID = b.InListID,
                    //                         OffPrice = b.OffPrice,
                    //                         OffSepecialPrice = b.OffSepecialPrice,
                    //                         OffID = b.OffID,
                    //                         OldSellListID = b.OldSellListID,
                    //                         ProCost = b.ProCost,
                    //                         OtherCash = b.OtherCash,
                    //                         Salary = b.Salary,
                    //                         AnBu = b.AnBu,
                    //                         LieShouPrice = b.LieShouPrice,
                    //                         OtherOff = b.OtherOff,
                    //                         AnBuPrice = b.AnBuPrice,
                    //                         NeedAduit =b.NeedAduit,
                    //                         YanbaoModelPrice=b.YanbaoModelPrice
                    //                         //Pro_SellTypeProduct=c
                    //                     }).ToList();

                    //model.Pro_SellListInfo.Clear();
                    //model.Pro_SellListInfo.AddRange(Selllist_temp);
                    //Selllist_temp = (from b in Selllist_temp
                    //                 group b by new
                    //                 {
                    //                     b.ProID,
                    //                     b.ProPrice,
                    //                     b.SellType,
                    //                     b.IMEI
                    //                 }
                    //                     into temp
                    //                     select new Model.Pro_SellListInfo
                    //                     {
                    //                         SellType = temp.Key.SellType,
                    //                         ProID = temp.Key.ProID,
                    //                         ProCount = temp.Sum(p => p.ProCount),
                    //                         ProPrice = temp.Key.ProPrice,
                    //                         IMEI = temp.Key.IMEI
                    //                     }).ToList();
                    #endregion
                    var x=from b in model.Pro_SellListInfo
                           join
                           c in Pro_SellType_ProPrice
                           on new { b.ProID, b.SellType }
                           equals
                           new { c.ProID, c.SellType }
                           select new 
                           {
                                b,
                                c
                           };
                    foreach (var m in x)
                    {
                        m.b.ProPrice = m.b.ProPrice!=0?m.b.ProPrice: m.c.Price;
                        m.b.SellType_Pro_ID = m.c.ID;
                        m.b.TicketUsed = CheckProCashTicket(m.b, m.c.Pro_ProInfo);
                        m.b.YanbaoModelPrice = m.c.Pro_ProInfo.Pro_SellTypeProduct.Any(p => p.SellType == 1)
                            ? m.c.Pro_ProInfo.Pro_SellTypeProduct.First(p => p.SellType == 1)
                                .Price
                            : 0;
                     
                    }

                    decimal? totle = model.Pro_SellListInfo.Sum(p => p.ProPrice * p.ProCount);
                    //var proOff_query = from c in lqh.Umsdb.VIP_ProOffList
                    //        join d in lqh.Umsdb.Pro_SellListInfo
                    //        on new { c.ProID, SellType = c.SellTypeID, c.ProCount }
                    //            equals
                    //           new { d.ProID, d.SellType, d.ProCount }
                    //        select new { AAA = d.VIP_OffList };

                    var AllVIPOff_query = (from b in lqh.Umsdb.VIP_OffList

                                           where ((((from c in b.VIP_VIPOffLIst //会员卡专属优惠
                                                     where c.VIPID == model.VIP_ID
                                                     select c).Any()
                                                  ||
                                                  (from c in b.VIP_VIPTypeOffLIst//会员类别专属优惠
                                                   where c.VIPType == VIPTypeID
                                                   select c).Any()
                                                  ||
                                                  (!b.VIP_VIPOffLIst.Any() && !b.VIP_VIPTypeOffLIst.Any()))
                                                  && (from c in b.VIP_HallOffInfo//门店专属优惠
                                                      where c.HallID == model.HallID
                                                      select c).Any()
                                                  )
                                                  || (
                                                  b.Type == 2 &&
                                                  (from c in b.VIP_OffTicket
                                                   where c.VIP_ID == model.VIP_ID
                                                         && (c.Used == false || c.Used == null)
                                                         && c.OffID == b.ID
                                                         && c.VIP_OffList.ArriveMoney <= totle
                                                   select c
                                                  ).Any()
                                                  ))
                                                  && b.Flag == true
                                                && b.EndDate >= model.SellDate
                                                && b.StartDate <= model.SellDate
                                                && new int?[] { 0, 1, 2 }.Contains(b.Type)
                                                && b.UseLimit < b.VIPTicketMaxCount
                                                && !b.VIP_ProOffList.All(p => !proIDS.Contains(p.ProID))
                                           select b);


                    var AllProOff_query = (from b in lqh.Umsdb.VIP_ProOffList
                                           where (from c in AllVIPOff_query where c.ID == b.OffID select c).Count() > 0
                                           select b).ToList();
                    //AllProOff_query.First().VIP_OffList.VIP_SendProOffList
                    var AllProOff_Join_SellList = from b in AllProOff_query

                                                  join c in model.Pro_SellListInfo
                                                  on new { b.ProID, SellType = b.SellTypeID }
                                                  equals
                                                  new { c.ProID, SellType = c.SellType }
                                                  into g
                                                  from c1 in g.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      b.VIP_OffList,
                                                      HasPro = c1 == null ? false : true && c1.ProCount >= b.ProCount ? true : false,
                                                      C = c1
                                                  }
                                                ;
                    var FitSingleProOff_query = (from b in AllProOff_Join_SellList
                                                 where (b.VIP_OffList.Type == 0 && b.HasPro)//符合单品的活动，只要有一个单品符合，该优惠就符合要求
                                                 select b.VIP_OffList.ID).Distinct();
                    var UnFitMultProOff_query = (from b in AllProOff_Join_SellList
                                                 where (b.VIP_OffList.Type == 1 && b.HasPro == false)//不符合组合活动，只要有一个单品不符合，该优惠就不符合要求
                                                 select b.VIP_OffList.ID).Distinct();

                    var AllFitOff_query = from b in AllVIPOff_query
                                          where (b.Type == 0 && FitSingleProOff_query.Contains(b.ID))
                                          || (b.Type == 1 && !UnFitMultProOff_query.Contains(b.ID))
                                          select b;

                    var promainids =
                       lqh.Umsdb.Pro_ProInfo.Where(p => model.Pro_SellListInfo.Select(o => o.ProID).Contains(p.ProID)).Select(p => p.ProMainID).ToList();
                    var allrules =
                        lqh.Umsdb.Rules_AllCurrentRulesInfo.Where(p => p.HallID == model.HallID)
                            .Where(p => promainids.Contains(p.ProMainID) &&p.StartDate<DateTime.Now&&p.EndDate>DateTime.Now ).Distinct()
                            .ToList();


                    Model.WebReturn r = new Model.WebReturn() { ReturnValue = true, Message = "获取成功" };

                    r.Obj = model;
                    r.ArrList.Add(AllFitOff_query.ToList());
                    r.ArrList.Add(allrules);

                    return r;
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "系统错误，" + ex.Message };
                }

            }
            //throw new Exception();
        }


        /// <summary>
        /// 获取优惠
        /// </summary>
        /// <remarks></remarks>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        public Model.WebReturn GetSellOff(Model.Sys_UserInfo sysUser, Model.Pro_SellInfo model,List<int> TempIDS)
        {

            //if (model == null) return new Model.WebReturn();
            string Msg = "";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                DataLoadOptions d = new DataLoadOptions();
                //d.LoadWith<Model.VIP_OffList>(c => c.VIP_HallOffInfo);
                d.LoadWith<Model.VIP_OffList>(c => c.VIP_ProOffList);
                //d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPOffLIst);
                //d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPTypeOffLIst);
                //d.LoadWith<Model.VIP_OffList>(c => c.VIP_SendProOffList);
                lqh.Umsdb.LoadOptions = d;

                try
                {
                    int VIPID = 0;
                    int VIPTypeID = 0;
                    if (model.VIP_ID != null)
                    {
                        var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                        where model.VIP_ID == b.ID
                                        select b;
                        if (vip_query.Count() <= 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该会员不存在" };
                        }
                        var vip = vip_query.First();
                        VIPID = vip.ID;
                        VIPTypeID = Convert.ToInt32(vip.TypeID);
                    }

                    List<string> proIDS = new List<string>();
                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        proIDS.Add(m.ProID);
                        if (m.Pro_SellList_RulesInfo == null || m.Pro_SellList_RulesInfo.Count==0)
                        {
                            m.Pro_SellList_RulesInfo = new EntitySet<Pro_SellList_RulesInfo>();
                        }
                    }
                    proIDS.Distinct();


                    var Pro_SellType_ProPrice = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                                 where proIDS.Contains(b.ProID)
                                                 select b).ToList();
                    #region 旧
                    
                    
                    //var Selllist_temp = (from b in model.Pro_SellListInfo
                    //                     join
                    //                     c in Pro_SellType_ProPrice
                    //                     on new { b.ProID, b.SellType }
                    //                     equals
                    //                     new { c.ProID, c.SellType }
                    //                     select new Model.Pro_SellListInfo
                    //                     {
                    //                         ID = b.ID,
                    //                         ProID = b.ProID,
                    //                         ProPrice = b.ProPrice,
                    //                         SellType = b.SellType,
                    //                         TicketUsed = CheckProCashTicket(b, c.Pro_ProInfo),
                    //                         TicketID = b.TicketID,
                    //                         Note = b.Note,
                    //                         CashTicket = b.CashTicket,
                    //                         ProCount = b.ProCount,
                    //                         SellType_Pro_ID = c.ID,
                    //                         IMEI = b.IMEI,
                    //                         Pro_Sell_Yanbao = b.Pro_Sell_Yanbao,
                    //                         Pro_Sell_JiPeiKa = b.Pro_Sell_JiPeiKa,
                    //                         VIP_VIPInfo = b.VIP_VIPInfo,
                    //                         SellID = b.SellID,
                    //                         SellID_Temp = b.SellID_Temp,
                    //                         InListID = b.InListID,
                    //                         OffPrice = b.OffPrice,
                    //                         OffSepecialPrice = b.OffSepecialPrice,
                    //                         OffID = b.OffID,
                    //                         OldSellListID = b.OldSellListID,
                    //                         ProCost = b.ProCost,
                    //                         OtherCash = b.OtherCash,
                    //                         Salary = b.Salary,
                    //                         AnBu = b.AnBu,
                    //                         LieShouPrice= b.LieShouPrice,
                    //                         OtherOff = b.OtherOff,
                    //                         AnBuPrice = b.AnBuPrice,
                    //                         YanbaoModelPrice = b.YanbaoModelPrice,
                    //                         NeedAduit = b.NeedAduit
                    //                         //Pro_SellTypeProduct=c
                    //                     }).ToList();

                    //model.Pro_SellListInfo.Clear();
                    //model.Pro_SellListInfo.AddRange(Selllist_temp);
                    //Selllist_temp = (from b in Selllist_temp
                    //                 group b by new
                    //                 {
                    //                     b.ProID,
                    //                     b.ProPrice,
                    //                     b.SellType,
                    //                     b.IMEI
                    //                 }
                    //                     into temp
                    //                     select new Model.Pro_SellListInfo
                    //                     {
                    //                         SellType = temp.Key.SellType,
                    //                         ProID = temp.Key.ProID,
                    //                         ProCount = temp.Sum(p => p.ProCount),
                    //                         ProPrice = temp.Key.ProPrice,
                    //                         IMEI = temp.Key.IMEI
                    //                     }).ToList();
                    #endregion
                    var x = from b in model.Pro_SellListInfo
                            join
                            c in Pro_SellType_ProPrice
                            on new { b.ProID, b.SellType }
                            equals
                            new { c.ProID, c.SellType }
                            select new
                            {
                                b,
                                c
                            };
                    foreach (var m in x)
                    {
                        m.b.ProPrice = m.b.ProPrice != 0 ? m.b.ProPrice : m.c.Price;
                        m.b.SellType_Pro_ID = m.c.ID;
                        m.b.TicketUsed = CheckProCashTicket(m.b, m.c.Pro_ProInfo);
                        m.b.YanbaoModelPrice = Common.Utils.IsZhongshanSellType(m.c.SellType)
                            ? (m.c.Pro_ProInfo.Pro_SellTypeProduct.Any(p => p.SellType == 1)
                                ? m.c.Pro_ProInfo.Pro_SellTypeProduct.First(p => p.SellType == 1)
                                    .Price
                                : 0)
                            : (m.c.Pro_ProInfo.Pro_SellTypeProduct.Any(p => p.SellType == 8)
                                ? m.c.Pro_ProInfo.Pro_SellTypeProduct.First(p => p.SellType == 8)
                                    .Price
                                : 0);

                    }

                    decimal? totle = model.Pro_SellListInfo.Sum(p => p.ProPrice * p.ProCount);
                    //var proOff_query = from c in lqh.Umsdb.VIP_ProOffList
                    //        join d in lqh.Umsdb.Pro_SellListInfo
                    //        on new { c.ProID, SellType = c.SellTypeID, c.ProCount }
                    //            equals
                    //           new { d.ProID, d.SellType, d.ProCount }
                    //        select new { AAA = d.VIP_OffList };
                     #region 结算中的临时销售数据
                    if(TempIDS==null) TempIDS=new List<int>();
                    var AllTemp_query = from b in lqh.Umsdb.Pro_SellListInfo_Temp
                                        where TempIDS.Contains(b.ID)
                                        select  b;
	                #endregion
                    var AllVIPOff_query = (from b in lqh.Umsdb.VIP_OffList

                                           where ((((from c in b.VIP_VIPOffLIst //会员卡专属优惠
                                                     where c.VIPID == model.VIP_ID
                                                     select c).Any()
                                                  ||
                                                  (from c in b.VIP_VIPTypeOffLIst//会员类别专属优惠
                                                   where c.VIPType == VIPTypeID
                                                   select c).Any()
                                                  ||
                                                  (!b.VIP_VIPOffLIst.Any() && !b.VIP_VIPTypeOffLIst.Any()))
                                                  && (from c in b.VIP_HallOffInfo//门店专属优惠
                                                      where c.HallID == model.HallID
                                                      select c).Any()
                                                  )
                                                  || (
                                                  b.Type == 2 &&
                                                  (from c in b.VIP_OffTicket
                                                   where c.VIP_ID == model.VIP_ID
                                                         && (c.Used == false || c.Used == null)
                                                         && c.OffID == b.ID
                                                         && c.VIP_OffList.ArriveMoney <= totle
                                                   select c
                                                  ).Any()
                                                  ))
                                                  && b.Flag == true
                                                && b.EndDate >= model.SellDate
                                                && b.StartDate <= model.SellDate
                                                && new int?[] { 0, 1, 2 }.Contains(b.Type)
                                                && b.UseLimit < b.VIPTicketMaxCount
                                                && (!b.VIP_ProOffList.All(p => !proIDS.Contains(p.ProID)) || b.Type==2 )//优惠券的 或者 任何包含结算商品的活动
                                                && (!(from b1 in b.VIP_ProOffList
                                                      where b.Type == 1
                                                      join c1 in AllTemp_query
                                                      on new { b1.ProID, b1.SellTypeID, b1.ProCount }
                                                      equals new { c1.ProID, SellTypeID = c1.SellType, c1.ProCount }
                                                      into gtemp
                                                      from c2 in gtemp.DefaultIfEmpty()
                                                      select c2
                                                      ).Any(p => p == null))
                                                 
                                           select b);


                    //var AllProOff_query = (from b in lqh.Umsdb.VIP_ProOffList
                    //                       where (from c in AllVIPOff_query where c.ID == b.OffID select c).Count() > 0
                    //                       select b);
                   
                    

                    //AllProOff_query.First().VIP_OffList.VIP_SendProOffList
                     

                    //var AllProOff_Join_SellList = from b in AllProOff_query

                    //                              join c in AllTemp_query
                    //                              on new { b.ProID, SellType = b.SellTypeID }
                    //                              equals
                    //                              new { c.ProID, SellType = c.SellType }
                    //                              into g
                    //                              from c1 in g.DefaultIfEmpty()
                    //                              select new
                    //                              {
                    //                                  b.VIP_OffList,
                    //                                  HasPro = c1 == null ? false : true  
                    //                              }
                    //                            ;
                    //var FitSingleProOff_query = (from b in AllProOff_Join_SellList
                    //                             where (b.VIP_OffList.Type == 0 && b.HasPro)//符合单品的活动，只要有一个单品符合，该优惠就符合要求
                    //                             select b.VIP_OffList.ID);
                    //var UnFitMultProOff_query = (from b in AllProOff_Join_SellList
                    //                             where (b.VIP_OffList.Type == 1 && b.HasPro == false)//不符合组合活动，只要有一个单品不符合，该优惠就不符合要求
                    //                             select b.VIP_OffList.ID);

                    //var AllFitOff_query__ = from b in AllVIPOff_query 
 
                    //                      //where (b.Type == 0 && FitSingleProOff_query.Contains(b.ID))
                    //                      //|| (b.Type == 1 && !UnFitMultProOff_query.Contains(b.ID))
                    //                      select b;

                    var promainids =
                       lqh.Umsdb.Pro_ProInfo.Where(p => model.Pro_SellListInfo.Select(o => o.ProID).Contains(p.ProID)).Select(p => p.ProMainID).ToList();
                    var allrules =
                        lqh.Umsdb.Rules_AllCurrentRulesInfo.Where(p => p.HallID == model.HallID)
                            .Where(p => promainids.Contains(p.ProMainID) && p.StartDate < DateTime.Now && p.EndDate > DateTime.Now).Distinct()
                            .ToList();

                    Model.WebReturn r = new Model.WebReturn() { ReturnValue = true, Message = "获取成功" };

                    r.Obj = model;
                    r.ArrList.Add(AllVIPOff_query.ToList());
                    r.ArrList.Add(allrules);
                   

                    return r;
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "系统错误，" + ex.Message };
                }

            }
            //throw new Exception();
        }
        #endregion
        

        public Model.WebReturn GetInorderListID(Model.Pro_SellInfo model, LinQSqlHelper lqh)
        {
            var imeiList = from b in model.Pro_SellListInfo
                           select b.IMEI;

            //串码类拣货
            var Pro_IME_List = (from b in lqh.Umsdb.Pro_IMEI
                               where imeiList.Contains(b.IMEI)
                               select b).ToList();

            

            //if(Pro_IME_List.Where(""))


            return null;
        }

        private string GetErrorMsg(Model.Pro_SellListInfo model,Model.Pro_SellTypeProduct p)
        {
            if (p.Pro_ProInfo == null) return "商品型号不正确";
            else if (p.Pro_SellType == null) return "销售类别不存在";
            else if (model.ProPrice != p.Price) return "销售类别的单价不正确";
            else return model.Note;
        }

        public Model.WebReturn CheckIMEI(Model.Pro_IMEI imei)
        {
            Model.WebReturn r = new Model.WebReturn() {  ReturnValue=true};
            if (imei == null)
            { 
                r.Message= "串码不存在";
                r.ReturnValue = false;
            }
            else if (imei.Pro_ProInfo == null)
            {
                r.Message = "商品不存在";
                r.ReturnValue = false;
            }
            else if (!imei.Pro_ProInfo.NeedIMEI == true)
            {
                r.Message = "属于无串码商品";
                r.ReturnValue = false;
            }
            else if (imei.SellID > 0 || imei.OutID > 0 || imei.BorowID > 0 || imei.RepairID > 0||imei.AuditID>0||imei.AssetID>0)
            {
                r.Message = "串码已处理";
                r.ReturnValue = false;
            }
            else if (imei.Pro_StoreInfo == null || imei.Pro_StoreInfo.ProCount - 1 < 0)
            {
                r.Message = "库存不足";
                r.ReturnValue = false;
            }
            return r;
        }

        /// <summary>
        /// 查询
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

                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0 || pageParam.PageSize != 30)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var aduit_query = from b in lqh.Umsdb.View_Pro_SellInfo
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {

                            case "Applyed":
                                Model.ReportSqlParams_String apply = (Model.ReportSqlParams_String)item;
                                if (apply.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.Applyed == apply.ParamValues
                                                  select b;
                                }
                                break;

                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                if (mm5.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.SysDate >= mm5.ParamValues
                                                  select b;
                                }
                                break;
                            case "EndTime":
                                Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)item;
                                if (mm6.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.SysDate <= mm6.ParamValues
                                                  select b;
                                }
                                break;
                            case "Seller":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where para.ParamValues.Contains(b.Seller.ToString())
                                                  select b;
                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                if (para1.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
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
                        aduit_query = from b in aduit_query
                                      where ValidHallIDS.Contains(b.HallID)
                                      orderby b.SellDate descending
                                      select b;
                    }
                    else
                    {
                        aduit_query = from b in aduit_query
                                      orderby b.SellDate descending
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

                        List<Model.View_Pro_SellInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_Pro_SellInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
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

        public Model.WebReturn GetModel(Model.Sys_UserInfo user, string SellID)
        {
            
            try
            {

                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                   //TODO:权限
                    DataLoadOptions dl=new DataLoadOptions();
                    dl.LoadWith<Model.Pro_SellInfo>(info => info.Pro_SellListInfo);
                    dl.LoadWith<Model.Pro_SellInfo>(info=>info.Pro_SellSpecalOffList);
                    dl.LoadWith<Model.Pro_SellInfo>(info=>info.Pro_SellBackAduit);
                    dl.LoadWith<Model.Pro_SellListInfo>(info=>info.Pro_SellList_RulesInfo);
                    dl.LoadWith<Model.Pro_SellListInfo>(info => info.Pro_Sell_Service);
                  

                    dl.LoadWith<Model.Pro_SellBackAduit>(aduit => aduit.Pro_SellBackAduitList);

                    lqh.Umsdb.LoadOptions = dl;
                    if (lqh.Umsdb.Pro_SellInfo.Any(p => p.SellID == SellID))
                    {
                        var results = lqh.Umsdb.Pro_SellInfo.First(p => p.SellID == SellID);

                        if (results.Pro_SellBack.Any())
                        {

                            var backinfo = results.Pro_SellBack.OrderByDescending(p => p.ID).First();
                            results.CashTotle = backinfo.NewCashTotle;

                            var lists =
                                backinfo.Pro_SellListInfo.ToList();
                            results.Pro_SellListInfo = new EntitySet<Pro_SellListInfo>();
                            results.Pro_SellListInfo.AddRange(lists);


                        }
                        else
                        {
                            //results.Pro_SellListInfo = results.Pro_SellListInfo.ToList();
                        }
                        return new WebReturn()
                        {
                            Obj = results,
                            ReturnValue = true
                        };
                    }
                    else
                    {
                        return new WebReturn() {ReturnValue = false, Message = "无该销售单"};

                    }
                   

                }

            }
            catch (Exception ex)
            {
                return new WebReturn() {ReturnValue = false, Message = ex.Message};
            }
        }
    }
}
