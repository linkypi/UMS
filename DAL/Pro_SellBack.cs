using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Common;
using Model;

namespace DAL
{
    public class Pro_SellBack
    {
        private int MenthodID;

        public Pro_SellBack()
        {
            this.MenthodID = 0;
        }

        public Pro_SellBack(int MenthodID)
        {
            this.MenthodID = MenthodID;
        }


//
//        public Model.WebReturn Add_(Model.Sys_UserInfo user, Model.Pro_SellBack model)
//        {
//            //此方法暂时少用Linq查询维持可读性
//
//            try
//            {
//                using (LinQSqlHelper lqh=new    LinQSqlHelper())
//                {
//
//                    Model.Pro_SellInfo OldSellInfo;//原销售单
//                    List<Model.Pro_SellListInfo> OldSellListInfo=new List<Pro_SellListInfo>();//原销售记录
//                    List<Model.Pro_SellSpecalOffList> OldSellSpecalOffList=new List<Pro_SellSpecalOffList>();//原特殊优惠列表
//
//
//                    DataLoadOptions dataload = new DataLoadOptions();
//                    dataload.LoadWith<Model.Pro_SellBack>(c => c.Pro_SellListInfo);
//
//                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
//                    lqh.Umsdb.LoadOptions = dataload;
//           
//                    #region 验证是否可以不用审批即可取消
//                    if (model.BackMoney > user.CancelLimit)//需要审批
//                    {
//                        if (string.IsNullOrEmpty(model.AduitID))
//                        {
//                            return new Model.WebReturn() { ReturnValue = false, Message = "退款金额超过" + user.CancelLimit + ",需要审批单" };
//                        }
//                        var backauditlist = (from b in lqh.Umsdb.Pro_SellBackAduit
//                                             where b.AduitID == model.AduitID && b.Aduited == true && b.Passed == true && b.Used != true
//                                             select b).ToList();
//                        if (backauditlist.Count() == 0)
//                        {
//                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批单不存在或未审核或审核不通过或已使用" };
//                        }
//                        Model.Pro_SellBackAduit firstBackAudit = backauditlist.First();
//                        if (model.BackMoney > firstBackAudit.AduitMoney)
//                        {
//                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批金额" + firstBackAudit.AduitMoney + "小于当前退款金额" + model.BackMoney };
//                        }
//                        firstBackAudit.Used = true;
//                        firstBackAudit.UseDate = DateTime.Now;
//
//                    }
//                    else model.AduitID = null;
//                    model.Aduited = true;
//
//                    #endregion
//
//                    var Sell_query = from b in lqh.Umsdb.Pro_SellInfo
//                                     where b.ID == model.SellID
//                                     select b;
//                    if (!Sell_query.Any())
//                    {
//                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单不存在" };
//                    }
//                    OldSellInfo = Sell_query.First();
//
//
//                    if (temp.Pro_SellBack.Any())
//                    {
//                        //曾经存在退货的
//                        var temp = OldSellInfo.Pro_SellBack.OrderByDescending(p => p.ID).First();
//                        OldSellListInfo.AddRange(temp.Pro_SellListInfo);
//                        OldSellSpecalOffList.AddRange(temp.Pro_SellSpecalOffList);
//
//                    }
//                    else
//                    {
//                        OldSellInfo 
//                    }
//                    
//                //TODO: 提取退货记录
//
//                //TODO: 计算退货优惠
//
//                //TODO: 计算退货金额
//
//
//                //TODO: 返回库存
//
//                //TODO: 未退货部分与新增商品再销售
//
//
//                //TODO: 验证优惠
//
//
//                //TODO: 减库存
//
//
//                //TDOO: 保存
//
//                }
//
//                return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
//            }
//            catch (Exception ex)
//            {
//                return new WebReturn() { ReturnValue = false, Message = ex.Message };
//            }
//
//
//
//            return new WebReturn() {ReturnValue = false, Message = "失败"};
//        }


        /// <summary>
        /// 新增退货 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_SellBack model, List<int> tempidlist)
        { 
            Model.WebReturn r = null;
            bool NoError = true;
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {
                    model.UpdDate = DateTime.Now;
                    model.SysDate = DateTime.Now;

                    List<Model.Pro_SellListInfo> sellList = new List<Model.Pro_SellListInfo>();
                    //退货的代金券编码列表
                    List<string> BackticketStr = new List<string>();
                    //退货的组合优惠ID
                    List<int> backSepecailList = new List<int>();
                    //退货的销售明细ID
                    List<int> backsellListIDs = new List<int>();
                    //旧销售明细 没有使用组合优惠的列表
                    List<Model.Pro_SellListInfo> OldSellList_needOff = new List<Model.Pro_SellListInfo>();
                    //旧销售明细 不需要重新选优惠的
                    List<Model.Pro_SellListInfo> OldSellList_NotneedOff = new List<Model.Pro_SellListInfo>();

                    //旧销售明细 组合优惠列表
                    List<Model.Pro_SellSpecalOffList> sellSepecailList=new List<Model.Pro_SellSpecalOffList>();
                    //商品 销售类别
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放串码
                    List<string> IMEI = new List<string>();
                    //存放退貨串碼
                    List<string> IMEI_B=new     List<string>();
                    //存放无串码
                    List<string> ProIDNoIMEI = new List<string>();
                    //存放赠品
                    //List<Model.Pro_SellSendInfo> sendList = new List<Model.Pro_SellSendInfo>();
                    //存放优惠
                    List<int?> OffID_List = new List<int?>();
                    //退回的优惠
                    List<int?> backOffID_List = new List<int?>();

                    List<Model.Pro_IMEI> BackIMEIModels=new List<Pro_IMEI>();
                    List<Model.Pro_IMEI> SellIMEIModels=new List<Pro_IMEI>();

                    DataLoadOptions dataload = new DataLoadOptions();
                    dataload.LoadWith<Model.Pro_SellBack>(c => c.Pro_SellListInfo);
 
                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;

                    if (!String.IsNullOrEmpty(model.AduitID))
                    {
                        var backauditlist = (from b in lqh.Umsdb.Pro_SellBackAduit
                                             where
                                                 b.AduitID == model.AduitID && b.Aduited == true && b.Passed == true &&
                                                 b.Used != true
                                             select b).ToList();
                        if (backauditlist.Count() == 0)
                        {
                            return new Model.WebReturn() {ReturnValue = false, Message = "审批单错误：审批单不存在或未审核或审核不通过或已使用"};
                        }
                        Model.Pro_SellBackAduit firstBackAudit = backauditlist.First();
                        if (model.BackMoney > firstBackAudit.AduitMoney)
                        {
                            return new Model.WebReturn()
                            {
                                ReturnValue = false,
                                Message = "审批单错误：审批金额" + firstBackAudit.AduitMoney + "小于当前退款金额" + model.BackMoney
                            };
                        }
                         firstBackAudit.Used = true;
                        firstBackAudit.UseDate = DateTime.Now;
                    }
                    else

                    #region 验证是否可以不用审批即可取消
                    if ( user.AduitLimit==null || model.BackMoney > user.AduitLimit) //需要审批
                    {
                        
                            return new Model.WebReturn()
                                {
                                    ReturnValue = false,
                                    Message = "退款金额超过" + (user.AduitLimit??0) + ",需要审批单"
                                };
                       
                        
                       
                       

                    }
                   
                    model.Aduited = true;
                
                    #endregion

                    #region 获取销售单 验证权限 
                    #region 原销售单
                    var Sell_query = from b in lqh.Umsdb.Pro_SellInfo
                                     where b.ID == model.SellID
                                     select b;
                    if (Sell_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单不存在" };
                    }
                    Model.Pro_SellInfo sell = Sell_query.First();
                    #endregion
                    

                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(user, this.MenthodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(sell.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message ="仓库无权操作" };
                    #endregion

                    #region 如果尚未退货，则以最开始的销售单为基础数据，否者 以最后一次退货单为基础数据
                    
                    

                    Model.Pro_SellBack sellback = null;
                    var Sell_query2 = sell.Pro_SellBack.ToList();

                    
                    if (Sell_query2.Count() == 0)
                    {
                        sellList.AddRange(sell.Pro_SellListInfo);
                        model.BackID = 0;
                        sellSepecailList.AddRange(sell.Pro_SellSpecalOffList);
                        if(model.OldCashTotle!=sell.CashTotle-sell.OffTicketPrice)
                        {
                            return new Model.WebReturn() {  ReturnValue=false, Message="原销售单实收有误"};
                        }
                        if(model.BackOffTicketPrice !=sell.OffTicketPrice ||(model.BackOffTicketID!=null&& model.BackOffTicketID != sell.OffTicketID))
                        {
                            return new Model.WebReturn() {  ReturnValue=false, Message="本次退回的优惠券与原销售单不一致"};
                        }
                    }
                    else
                    {
                        int MaxID=Sell_query2.Max(p=>p.ID);
                        sellback = Sell_query2.Find(p => p.ID == MaxID);
                        sellList.AddRange(sellback.Pro_SellListInfo);
                        sellSepecailList.AddRange(sellback.Pro_SellSpecalOffList);
                        model.BackID = sellback.ID;
                        if (model.OldCashTotle != sellback.NewCashTotle - sellback.OffTicketPrice)
                        {
                            return new Model.WebReturn() {  ReturnValue=false, Message="原销售单实收有误"};
                        }
                        if(model.BackOffTicketPrice !=sellback.OffTicketPrice || model.BackOffTicketID != sellback.OffTicketID)
                        {
                            return new Model.WebReturn() {  ReturnValue=false, Message="本次退回的优惠券与原销售单不一致"};
                        }
                    }
                    #endregion

                      
                                                
                    #endregion

                    #region 需要退货的商品批次号
                    List<string> back_InorderListID = (from b in model.Pro_SellBackList
                                                       select b.InListID).ToList();


                    #endregion 


                    #region 獲取退貨串號

                    foreach (var m in model.Pro_SellBackList)
                    {
                        if (!string.IsNullOrEmpty(m.IMEI))
                        {
                            if (IMEI_B.Contains(m.IMEI))
                            {
                                NoError = false;
                                m.Note = m.IMEI + "串码重复";
                                continue;
                            }
                            else
                                IMEI_B.Add(m.IMEI);
                        }
                    }
                    #endregion


                    #region 获取当前换货记录的串号、商品编号

                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        if (m.ID > 0) continue;//不是新增的记录
                        ////有商品限制，而且商品不在权限范围内
//                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
//                        {
//                            NoError = false;
//                            m.Note = "无权操作";
//                            continue;
//                        }
                        if(m.ID<=0)
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

                        if (m.OffID!=null){
                            OffID_List.Add(m.OffID);
                        }
                    }
                    //组合的
                    foreach (Model.Pro_SellSpecalOffList m in model.Pro_SellSpecalOffList)
                    {
                        OffID_List.Add(m.SpecalOffID);
                        //sendList.AddRange(m.Pro_SellSendInfo);
                    }
                    //优惠券的
                    if (model.OffTicketID!=null){
                        OffID_List.Add(model.OffTicketID);
                    }
                    #endregion

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
                                    where IMEI.Contains(b.IMEI) && b.HallID == sell.HallID
                                    select b).ToList();
                    var imei_b_List =
                        (from b in lqh.Umsdb.Pro_IMEI where IMEI_B.Contains(b.IMEI) && b.HallID == sell.HallID select b)
                            .ToList();
                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == sell.HallID && (ProIDNoIMEI.Contains(b.ProID) || back_InorderListID.Contains(b.InListID)) 
                                     orderby b.InListID
                                     select b).ToList();


                  

                    #region 退货记录

                    var SellList_join_BackList = from b in model.Pro_SellBackList 
                                                 join c in sellList
                                                 on new { SellListID = (int)b.SellListID }
                                                 equals
                                                    new { SellListID = c.ID } 
                                                 into temp
                                                 from c1 in temp.DefaultIfEmpty()
                                                 join d in StoreList
                                                 on b.InListID equals d.InListID
                                                 into temp2
                                                 from d1 in temp2.DefaultIfEmpty()
                                                 join e in imei_b_List
                                                 on b.IMEI equals e.IMEI
                                                 into temp3
                                                 from e1 in temp3.DefaultIfEmpty()
                                                 select new { Pro_SellBackList = b, Pro_SellListInfo = c1,Pro_StoreInfo=d1,Pro_IMEI=e1 };

                    decimal? backListtotle = 0;
                    List<Model.VIP_VIPInfo> viplists=new List<Model.VIP_VIPInfo>();

                    foreach (var m in SellList_join_BackList)
                    {
                            if (m.Pro_SellListInfo == null)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "退货的记录SellListID有误";
                                continue;
                            }
                            if (backsellListIDs.Contains((int)m.Pro_SellBackList.SellListID))
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "退货的记录SellListID重复";
                                continue;
                            }
                            else backsellListIDs.Add((int)m.Pro_SellBackList.SellListID);
                            if(m.Pro_SellBackList.ProCount<=0)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "退货的数量必须大于0";
                                continue;
                            }
                            if (m.Pro_SellListInfo.ProCount < m.Pro_SellBackList.ProCount)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "退货数量超限，并未购买这么多商品";
                                continue;
                            }
                            
                            #region 复制退货数据
                            if (m.Pro_SellBackList.IMEI != m.Pro_SellListInfo.IMEI ||
                            m.Pro_SellBackList.InListID != m.Pro_SellListInfo.InListID ||
                            m.Pro_SellBackList.LowPrice != m.Pro_SellListInfo.LowPrice ||
                            m.Pro_SellBackList.OffID != m.Pro_SellListInfo.OffID ||
                            m.Pro_SellBackList.OffPoint != m.Pro_SellListInfo.OffPoint ||
                            m.Pro_SellBackList.OffPrice != m.Pro_SellListInfo.OffPrice ||
                            m.Pro_SellBackList.OffSepecialPrice != m.Pro_SellListInfo.OffSepecialPrice ||
                            m.Pro_SellBackList.ProCost != m.Pro_SellListInfo.ProCost ||
                            m.Pro_SellBackList.ProID != m.Pro_SellListInfo.ProID ||
                            m.Pro_SellBackList.ProPrice != m.Pro_SellListInfo.ProPrice ||
                            m.Pro_SellBackList.SellType != m.Pro_SellListInfo.SellType ||
                            m.Pro_SellBackList.SellType_Pro_ID != m.Pro_SellListInfo.SellType_Pro_ID ||
                            m.Pro_SellBackList.SpecialID != m.Pro_SellListInfo.SpecialID ||
                            m.Pro_SellBackList.TicketID != m.Pro_SellListInfo.TicketID ||
                            m.Pro_SellBackList.TicketUsed != m.Pro_SellListInfo.TicketUsed ||
                            m.Pro_SellBackList.WholeSaleOffPrice != m.Pro_SellListInfo.WholeSaleOffPrice||
                                m.Pro_SellBackList.OtherCash!=m.Pro_SellListInfo.OtherCash||
                                m.Pro_SellBackList.AnBu!=m.Pro_SellListInfo.AnBu||
                                m.Pro_SellBackList.LieShou!=m.Pro_SellListInfo.LieShou||
                                m.Pro_SellBackList.LieShouPrice!=m.Pro_SellListInfo.LieShouPrice||
                                m.Pro_SellBackList.OtherOff!=m.Pro_SellListInfo.OtherOff||
                                m.Pro_SellBackList.AnBuPrice!=m.Pro_SellListInfo.AnBuPrice)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "退货记录的详情与销售记录不相同";
                                continue;
                            }
                            #endregion
                            #region 验证退货记录的退还金额
                        decimal rules = m.Pro_SellListInfo.Pro_SellList_RulesInfo.Sum(p => p.RealPrice);
                        decimal temp = (m.Pro_SellBackList.ProPrice - m.Pro_SellBackList.AnBuPrice -
                                        m.Pro_SellBackList.OffPrice - m.Pro_SellBackList.TicketUsed - m.Pro_SellBackList.OffSepecialPrice-
                                        m.Pro_SellBackList.OtherOff - m.Pro_SellBackList.WholeSaleOffPrice );
                        if (temp < 0) temp = 0;
                        if (m.Pro_SellBackList.ShouldBackCash != temp  +m.Pro_SellBackList.OtherCash - rules)
                            { 
                                NoError = false;
                                m.Pro_SellBackList.Note = "退货记录的退还金额不正确";
                                continue;
                            }
                        backListtotle += m.Pro_SellBackList.CashPrice  * m.Pro_SellBackList.ProCount;
                        backListtotle += m.Pro_SellBackList.OffSepecialPrice*m.Pro_SellBackList.ProCount;
                        //backListtotle += m.Pro_SellBackList.OtherCash*m.Pro_SellBackList.ProCount;
                            #endregion
                       
                            

                            #region 有特殊优惠  单品优惠 将优惠的id 放入列表
                            if (m.Pro_SellBackList.SpecialID > 0)
                            {
                                for (int i = 0; i < m.Pro_SellBackList.ProCount; i++)
                                {
                                    if (m.Pro_SellBackList.OffID != null && m.Pro_SellBackList.OffID>0)
                                    backOffID_List.Add(m.Pro_SellBackList.OffID);
                                }

                                    if (!backSepecailList.Contains((int)m.Pro_SellBackList.SpecialID))
                                    {backSepecailList.Add((int)m.Pro_SellBackList.SpecialID);
                                    backListtotle -=
                                  sellSepecailList.First(p => p.ID == m.Pro_SellBackList.SpecialID).OffMoney;
                                    }
                                backOffID_List.Add(sellSepecailList.First(p=>p.ID==m.Pro_SellBackList.SpecialID).SpecalOffID);
                               
                            }
                            
                            #endregion

                            #region 加回库存
                            if (!NoError) continue;
                            //有串码
                        if (!model.Pro_SellListInfo.Any(p=>p.NeedAduit)){
                            #region 如果有兑券
                            if (!string.IsNullOrEmpty(m.Pro_SellListInfo.TicketID))
                            {
                                BackticketStr.Add(m.Pro_SellListInfo.TicketID);
                            }
                            #endregion
                        if (!string.IsNullOrEmpty(m.Pro_SellListInfo.IMEI))
                        {
                            if (m.Pro_IMEI == null)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "串号不存在";
                                continue;
                            }
                            m.Pro_IMEI.Pro_StoreInfo.ProCount += m.Pro_SellListInfo.ProCount;
                            m.Pro_IMEI.SellID = null;
                            BackIMEIModels.Add(m.Pro_IMEI);
                        }
                        else
                        {
                            if (m.Pro_StoreInfo == null)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "退货的批次号不存在";
                                continue;
                            }
                            m.Pro_StoreInfo.ProCount += m.Pro_SellBackList.ProCount;
                        }
                        }
                        #endregion
                        #region 會員
                        if (m.Pro_SellListInfo.VIP_VIPInfo != null)
                        {
                            viplists.Add(m.Pro_SellListInfo.VIP_VIPInfo);
                            if (m.Pro_SellListInfo.VIP_VIPInfo.Pro_SellInfo.Any(p => p.Pro_SellBack.Count == 0))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "会员有消费记录, 不允许退会员", Obj = model };
                            }
                            if (m.Pro_SellListInfo.VIP_VIPInfo.Pro_SellInfo.Where(p => p.Pro_SellBack.Count > 0).Any(q=>q.Pro_SellBack.OrderByDescending(o=>o.ID).First().Pro_SellListInfo.Count>0))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "会员有消费记录, 不允许退会员", Obj = model };
                            }
                        }

                        #endregion

                    }
                    #endregion
                    if (!NoError)
                    {
                        return new Model.WebReturn() {  ReturnValue=false, Message="退货记录验证出错" , Obj=model };
                    }
                   
                    #region 验证总的退款金额
                    if (model.BackMoney != backListtotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "本次退货记录的总金额计算有误"};
                    }
                    #endregion

                    #region 退还代金券
                    var ticket = from b in lqh.Umsdb.Pro_CashTicket
                                 where BackticketStr.Contains(b.TicketID)  && (b.IsBack==false || b.IsBack==null)
                                 select b;
                    if (ticket.Count() != BackticketStr.Count())
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message =   "部分退还的代金券不存在" };
                    }
                    foreach (var t_ in ticket)
                    {
                        if (t_.IsBack == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = t_.TicketID+ "代金券已经退还"  };
                        }
                        t_.IsBack = true;
                    }
                    //lqh.Umsdb.Pro_CashTicket.DeleteAllOnSubmit(ticket);
                    //lqh.Umsdb.SubmitChanges();
                    #endregion

                    #region 获取需要重新计算组合优惠的或者不需要重新计算组合优惠的
                    var SellList_Join_SellBackList = from b in sellList
                                                     join c in model.Pro_SellBackList
                                                     on b.ID equals c.SellListID
                                                     into temp
                                                     from c1 in temp.DefaultIfEmpty()
                                                     select new { Pro_SellListInfo=b ,Pro_SellBackList=c1};

                    foreach (var m in SellList_Join_SellBackList)
                    {
                        #region 被取消组合优惠的旧记录

                        if (m.Pro_SellBackList != null && m.Pro_SellListInfo.ProCount > m.Pro_SellBackList.ProCount)
                        {
//                            if (m.Pro_SellBackList != null && m.Pro_SellListInfo.SpecialID != null &&
//                                m.Pro_SellListInfo.SpecialID != 0)
//                            {

                                Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                                    {

                                        ID = m.Pro_SellListInfo.ID,
                                        IMEI = m.Pro_SellListInfo.IMEI,
                                        InListID = m.Pro_SellListInfo.InListID,
                                        LowPrice = m.Pro_SellListInfo.LowPrice,
                                        Note = m.Pro_SellListInfo.Note,
                                        OffID = m.Pro_SellListInfo.OffID,
                                        OffPoint = m.Pro_SellListInfo.OffPoint,
                                        OffPrice = m.Pro_SellListInfo.OffPrice,
                                        OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                        ProCost = m.Pro_SellListInfo.ProCost,
                                        ProCount = m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount,
                                        ProID = m.Pro_SellListInfo.ProID,
                                        ProPrice = m.Pro_SellListInfo.ProPrice,
                                        SellType = m.Pro_SellListInfo.SellType,
                                        SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                        SpecialID = 0,
                                        WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                        CashPrice =
                                            (m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount)*
                                            (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice -
                                             m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice),
                                        OldSellListID = m.Pro_SellListInfo.ID,
                                    };
                                if (m.Pro_SellListInfo.OldSellListID > 0)
                                    myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                                else
                                    myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                                OldSellList_needOff.Add(myselllist);

                            //}
                        }
                            #endregion
                            #region 没有退货单参与组合优惠的 或没有任何优惠的

                        else if (m.Pro_SellBackList == null &&( m.Pro_SellListInfo.SpecialID == null || m.Pro_SellListInfo.SpecialID == 0 ||
                                 backSepecailList.Contains(Convert.ToInt32(m.Pro_SellListInfo.SpecialID))))
                        {

                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                                {
                                    ID = m.Pro_SellListInfo.ID,
                                    IMEI = m.Pro_SellListInfo.IMEI,
                                    InListID = m.Pro_SellListInfo.InListID,
                                    LowPrice = m.Pro_SellListInfo.LowPrice,
                                    Note = m.Pro_SellListInfo.Note,
                                    OffID = m.Pro_SellListInfo.OffID,
                                    OffPoint = m.Pro_SellListInfo.OffPoint,
                                    OffPrice = m.Pro_SellListInfo.OffPrice,
                                    OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                    ProCost = m.Pro_SellListInfo.ProCost,
                                    ProCount = m.Pro_SellListInfo.ProCount,
                                    ProID = m.Pro_SellListInfo.ProID,
                                    ProPrice = m.Pro_SellListInfo.ProPrice,
                                    SellType = m.Pro_SellListInfo.SellType,
                                    SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                    SpecialID = 0,
                                    WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                    CashPrice =
                                        (m.Pro_SellListInfo.ProCount)*
                                        (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice -
                                         m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice),
                                    OldSellListID = m.Pro_SellListInfo.ID,
                                };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_needOff.Add(myselllist);

                        }
                            #endregion
                            #region 没有退货，也没有参与组合优惠的,存入不需要再分配优惠的列表中

                        else if (m.Pro_SellBackList == null && (m.Pro_SellListInfo.SpecialID != null &&
                                  !backSepecailList.Contains(Convert.ToInt32(m.Pro_SellListInfo.SpecialID))))
                        {
                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                                {
                                    ID = m.Pro_SellListInfo.ID,
                                    IMEI = m.Pro_SellListInfo.IMEI,
                                    InListID = m.Pro_SellListInfo.InListID,
                                    LowPrice = m.Pro_SellListInfo.LowPrice,
                                    Note = m.Pro_SellListInfo.Note,
                                    OffID = m.Pro_SellListInfo.OffID,
                                    OffPoint = m.Pro_SellListInfo.OffPoint,
                                    OffPrice = m.Pro_SellListInfo.OffPrice,
                                    OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                    ProCost = m.Pro_SellListInfo.ProCost,
                                    ProCount = m.Pro_SellListInfo.ProCount,
                                    ProID = m.Pro_SellListInfo.ProID,
                                    ProPrice = m.Pro_SellListInfo.ProPrice,
                                    SellType = m.Pro_SellListInfo.SellType,
                                    SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                    SpecialID = 0,
                                    WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                    CashPrice = m.Pro_SellListInfo.CashPrice,
                                    //(m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice)
                                    CashTicket = m.Pro_SellListInfo.CashTicket,
                                    TicketID = m.Pro_SellListInfo.TicketID,
                                    TicketUsed = m.Pro_SellListInfo.TicketUsed
                                };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_NotneedOff.Add(myselllist);
                        }
                            #endregion

//                            #region 本身无组合优惠的记录
//
//                        else
//                        {
//                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
//                                {
//                                    ID = m.Pro_SellListInfo.ID,
//                                    IMEI = m.Pro_SellListInfo.IMEI,
//                                    InListID = m.Pro_SellListInfo.InListID,
//                                    LowPrice = m.Pro_SellListInfo.LowPrice,
//                                    Note = m.Pro_SellListInfo.Note,
//                                    OffID = m.Pro_SellListInfo.OffID,
//                                    OffPoint = m.Pro_SellListInfo.OffPoint,
//                                    OffPrice = m.Pro_SellListInfo.OffPrice,
//                                    OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
//                                    ProCost = m.Pro_SellListInfo.ProCost,
//                                    ProCount = m.Pro_SellListInfo.ProCount,
//                                    ProID = m.Pro_SellListInfo.ProID,
//                                    ProPrice = m.Pro_SellListInfo.ProPrice,
//                                    SellType = m.Pro_SellListInfo.SellType,
//                                    SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
//                                    SpecialID = null,
//                                    WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
//                                    CashPrice = m.Pro_SellListInfo.CashPrice,
//                                    //(m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice)
//                                    CashTicket = m.Pro_SellListInfo.CashTicket,
//                                    TicketID = m.Pro_SellListInfo.TicketID,
//                                    TicketUsed = m.Pro_SellListInfo.TicketUsed
//                                };
//                            if (m.Pro_SellListInfo.OldSellListID > 0)
//                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
//                            else
//                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
//                            OldSellList_needOff.Add(myselllist);
//                        }
//
//                        #endregion

                    }
                    #endregion


                    //OldSellList_needOff.AddRange(model.Pro_SellListInfo);
                    //额外加入新销售的记录

                    #region 验证实际的需要退的组合优惠 、 前台传入的需要退的组合优惠 、 原销售记录包含的组合优惠

                    if (backSepecailList.Count() > 0 || model.Pro_SellSpecalOffList.Count()>0 )
                    {
//                        if (backSepecailList.Count() != model.Pro_SellSpecalOffList.Count())
//                        {
//                            return new Model.WebReturn() { ReturnValue = false, Message = "失效的组合优惠数量有误", Obj = model };   
//                        }
                        //TODO: FIX
                        var backsepecial_backSepecailList_oldSepecail = from b in model.Pro_SellBackSpecalOffList
                                                                        join c in backSepecailList
                                                                        on b.ID equals c
                                                                        into temp1
                                                                        from c1 in temp1.DefaultIfEmpty()
                                                                        join d in sellSepecailList
                                                                        on b.ID equals d.ID
                                                                        into temp2
                                                                        from d1 in temp2.DefaultIfEmpty()
                                                                        select new { back_SepecailList=b,c1,Old_sepecailList=d1 };
                        List<int> tempid = new List<int>();
                        foreach (var m in backsepecial_backSepecailList_oldSepecail)
                        {
                            if (tempid.Contains(m.back_SepecailList.ID))
                            {
                                NoError = false;
                                m.back_SepecailList.Note = "组合优惠重复";
                                continue;
                            }
                            tempid.Add(m.back_SepecailList.ID);
                            if (m.c1 == null || m.c1==0)
                            {
                                NoError = false;
                                m.back_SepecailList.Note = "失效的组合优惠多了";
                                continue;
                            }
                            if (m.Old_sepecailList == null)
                            {
                                NoError = false;
                                m.back_SepecailList.Note = "失效的组合优惠不存在";
                                continue;
                            }
                            if (NoError)
                            {
                                //m.Old_sepecailList.Pro_SellBack = model;
                            }
                        }
                        if (!NoError)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "退货记录验证出错", Obj = model };
                        }
                    }

                    #endregion

                    if (model.Pro_SellListInfo.Any()){
                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                        group b by b.ProID into temp
                                        select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID) && p.ProCount>0)).ToList();


                    #region 验证可以重新计算优惠的列表 与 前台传入列表是否相同： 数量 和 字段
//
//                    var model_SellList_group = from b in model.Pro_SellListInfo
//                                               where b.ID > 0 
//                                               group b by b.ID
//                                                   into temp1
//                                                   from b1 in temp1.DefaultIfEmpty()
//                                                   select new { b1.ID,ProCount=temp1.Sum(p=>p.ProCount) };
//                    
//                  
//
//                    
//                    if (model_SellList_group.Count() != OldSellList_needOff.Count())
//                    {
//                        return new Model.WebReturn() { ReturnValue=false,Message="重新计算的销售明细记录不正确" };
//                    }
//                    var OldSellList_needOff_join_model_SellList_group
//                        = from b in OldSellList_needOff
//                          join c in model_SellList_group
//                          on b.ID equals c.ID
//                          into temp1
//                          from c1 in temp1
//                          select new {  Pro_SellListInfo=b, c1};
//                     if(OldSellList_needOff_join_model_SellList_group.Where(p=>p.Pro_SellListInfo.ProCount!=p.c1.ProCount).Count()>0)
//                    {
//                        return new Model.WebReturn() { ReturnValue = false, Message = "重新计算的销售明细记录数量不正确" };
//                    }
//                    #region 验证需要重新计算的销售记录字段
//                    var model_SellList_join_OldSellList_needOff = from b in model.Pro_SellListInfo
//                                                                  where b.ID > 0
//                                                                  join c in OldSellList_needOff
//                                                                  on b.ID equals c.ID
//                                                                  into temp1
//                                                                  from c1 in temp1.DefaultIfEmpty()
//                                                                  select new { Pro_SellListInfo = b, Pro_SellListInfo2=c1};
//                    foreach (var m in model_SellList_join_OldSellList_needOff)
//                    {
//                        if (m.Pro_SellListInfo2.IMEI != m.Pro_SellListInfo.IMEI ||
//                            m.Pro_SellListInfo2.InListID != m.Pro_SellListInfo.InListID ||
////                            m.Pro_SellListInfo2.LowPrice != m.Pro_SellListInfo.LowPrice ||
//                            m.Pro_SellListInfo2.OffID != m.Pro_SellListInfo.OffID ||
//                            m.Pro_SellListInfo2.OffPoint != m.Pro_SellListInfo.OffPoint ||
//                            m.Pro_SellListInfo2.OffPrice != m.Pro_SellListInfo.OffPrice ||
//                           // m.Pro_SellListInfo2.OffSepecialPrice != m.Pro_SellListInfo.OffSepecialPrice ||
//                            m.Pro_SellListInfo2.ProCost != m.Pro_SellListInfo.ProCost ||
//                            m.Pro_SellListInfo2.ProID != m.Pro_SellListInfo.ProID ||
//                            m.Pro_SellListInfo2.ProPrice != m.Pro_SellListInfo.ProPrice ||
//                            m.Pro_SellListInfo2.SellType != m.Pro_SellListInfo.SellType ||
//                            m.Pro_SellListInfo2.SellType_Pro_ID != m.Pro_SellListInfo.SellType_Pro_ID ||
////                            m.Pro_SellListInfo2.SpecialID != m.Pro_SellListInfo.SpecialID ||
//                            m.Pro_SellListInfo2.TicketID != m.Pro_SellListInfo.TicketID ||
//                            m.Pro_SellListInfo2.TicketUsed != m.Pro_SellListInfo.TicketUsed ||
//                            m.Pro_SellListInfo2.WholeSaleOffPrice != m.Pro_SellListInfo.WholeSaleOffPrice)
//                        {
//                            NoError = false;
//                            m.Pro_SellListInfo.Note = "只能重新计算组合优惠";
//                            continue;
//                        }
//                    }
//                    #endregion
//                    if (!NoError)
//                    {
//                        return new Model.WebReturn() { ReturnValue = false, Message = "重新计算的销售记录有误", Obj = model };
//                    }
                    #endregion
//


                    #region 真正生成销售新单部分


                    #region 验证优惠信息的有效性

                    var pro_off_list_temp = (from b in lqh.Umsdb.VIP_ProOffList
                                   where OffID_List.Contains(b.VIP_OffList.ID)
                                   select b).ToList();
                    var pro_off_list_temp1 = (from b in pro_off_list_temp
                                    where
                                        ((((from c in b.VIP_OffList.VIP_VIPOffLIst
                                            //会员卡专属优惠
                                            where c.VIPID == sell.VIP_ID
                                            select c).Any()
                                           ||
                                           (from c in b.VIP_OffList.VIP_VIPTypeOffLIst
                                            //会员类别专属优惠
                                            where c.VIPType == sell.VIP_VIPInfo.TypeID
                                            select c).Any()
                                          )
                                          && (from c in b.VIP_OffList.VIP_HallOffInfo
                                              //门店专属优惠
                                              where c.HallID == sell.HallID
                                              select c).Any()
                                         ))
                                        &&
                                        (b.VIP_OffList.Flag == true ||
                                         (b.VIP_OffList.Flag != true && b.VIP_OffList.UpdDate > sell.SysDate))
                                        && b.VIP_OffList.EndDate >= sell.SysDate
                                        && b.VIP_OffList.StartDate <= sell.SysDate
                                        // && new int?[] { 0, 1, 2, 3 }.Contains(b.VIP_OffList.Type)
                                        && b.VIP_OffList.UseLimit < b.VIP_OffList.VIPTicketMaxCount
                                    select b).ToList();
                    var pro_off_list_temp2 = (from b in lqh.Umsdb.VIP_ProOffList
                                    where backOffID_List.Contains(b.VIP_OffList.ID)
                                    select b).ToList();
                    var pro_off_list_temp3 = pro_off_list_temp1.Union(pro_off_list_temp2).ToList();
                        List<VIP_ProOffList> pro_off_list;
                        if (pro_off_list_temp3.Any())
                        {
                            pro_off_list = pro_off_list_temp3.OrderByDescending(p => p.VIP_OffList.OffMoney).ToList();
                        }
                        else
                        {
                            pro_off_list=new List<VIP_ProOffList>();
                        }
//                    var pro_off_list = (from b in lqh.Umsdb.VIP_ProOffList
//                                        where ((OffID_List.Contains(b.VIP_OffList.ID))
//
//                                         && ((((from c in b.VIP_OffList.VIP_VIPOffLIst //会员卡专属优惠
//                                                where c.VIPID == sell.VIP_ID
//                                                select c).Any()
//                                                              ||
//                                                              (from c in b.VIP_OffList.VIP_VIPTypeOffLIst//会员类别专属优惠
//                                                               where c.VIPType == sell.VIP_VIPInfo.TypeID
//                                                               select c).Any()
//                                                              )
//                                                              && (from c in b.VIP_OffList.VIP_HallOffInfo//门店专属优惠
//                                                                  where c.HallID == sell.HallID
//                                                                  select c).Any()
//                                                              ))
//                                                              && (b.VIP_OffList.Flag == true || (b.VIP_OffList.Flag!=true && b.VIP_OffList.UpdDate > sell.SysDate))
//                                                            && b.VIP_OffList.EndDate >= sell.SysDate
//                                                            && b.VIP_OffList.StartDate <= sell.SysDate
//                                            // && new int?[] { 0, 1, 2, 3 }.Contains(b.VIP_OffList.Type)
//                                                            && b.VIP_OffList.UseLimit < b.VIP_OffList.VIPTicketMaxCount) || backOffID_List.Contains(b.VIP_OffList.ID)
//                                        orderby b.VIP_OffList.OffMoney descending
//                                        select b).ToList();
                    var AllOffList = from b in pro_off_list
                                     select b.VIP_OffList;
                    var rulelist = lqh.Umsdb.Rules_AllCurrentRulesInfo.Where(p => p.HallID == model.Pro_SellInfo.HallID && p.StartDate < DateTime.Now && p.EndDate > DateTime.Now);
                    
                    #endregion

//                    #region 退回优惠后， 更新可使用的剩余优惠数量
//                    var AllOffList_join_backOffID = from b in AllOffList
//                                                    join c in backOffID_List
//                                                    on b.ID equals c
//                                                    into temp
//                                                    from c1 in temp.DefaultIfEmpty()
//                                                    select new { VIP_OffList=b , c1=FitBackOffToOffList(b,c1)};
//                    if (AllOffList_join_backOffID.Where(p => p.c1 == null).Count() > 0)
//                    {
//                        return new Model.WebReturn() { ReturnValue=false, Message="失效的优惠不存在" };
//                    }
//                    #endregion

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
                    foreach (var s in Sepecial_ProOffList)
                    {
                        //组合优惠是否存在
                        if (s.pro_off_list == null )
                        {
                            NoError = false; 
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


                    if ( !NoError)
                    {
                        
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
                                         join o in lqh.Umsdb.Pro_SellListInfo
                                         on b.ID
                                         equals o.ID
                                         into tempo
                                         from o1 in tempo.DefaultIfEmpty()

                                         join c in pro_off_list
                                             on new {b.ProID, b.SellType, b.OffID}
                                             equals
                                             new {c.ProID, SellType = c.SellTypeID, c.OffID}
                                             into temp1
                                         from c1 in temp1.DefaultIfEmpty()
                                         //where b.VIP_OffList.Type == 0
                                         join d in Sepecial_ProOffList
                                             on new {ID=b.SpecialID ?? 0, b.ProID, b.ProCount, b.SellType}
                                             equals
                                             new
                                                 {
                                                     d.Pro_SellSpecalOffList.ID,
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
                                                 d1,
                                                 o=o1
                                             };


                    #endregion
                    List<Model.Pro_CashTicket> cashTickList = new List<Model.Pro_CashTicket>();
                    List<Model.Pro_SellListInfo> sellListTemp = new List<Model.Pro_SellListInfo>();
                    List<string> str_ticket = new List<string>();
                    decimal? cashPrice = 0;

                    DAL.Pro_SellInfo Dal_sell=new Pro_SellInfo();
                    Dal_sell._NewSellListInfo = model.Pro_SellListInfo;
                        
                    foreach (var child in join_query)
                    {
                        Sepecial_ProOffList.Remove(child.d1);
                        if (child.Pro_SellListInfo.ID > 0) //重新计算的销售明细
                        {
                            child.Pro_SellListInfo.Pro_SellSpecalOffList = child.Pro_SellSpecalOffList;
                            if (child.o.Pro_Sell_JiPeiKa != null)
                            {
                                var a = Utils.Clone(child.o.Pro_Sell_JiPeiKa);
                                a.ID = 0;
                                child.Pro_SellListInfo.Pro_Sell_JiPeiKa = a;
                            }
                            if (child.o.Pro_Sell_Yanbao != null)
                            {
                                var a = Utils.Clone(child.o.Pro_Sell_Yanbao);
                                a.ID = 0;
                                child.Pro_SellListInfo.Pro_Sell_Yanbao = a;
                            }
                            if (child.o.Pro_Sell_Service != null && child.o.Pro_Sell_Service.Count > 0)
                            {
                                child.Pro_SellListInfo.Pro_Sell_Service = new EntitySet<Pro_Sell_Service>();
                                foreach (Pro_Sell_Service proSellService in child.o.Pro_Sell_Service)
                                {
                                    var a = Utils.Clone(proSellService);
                                    a.ID = 0;
                                    child.Pro_SellListInfo.Pro_Sell_Service.Add(a);
                                }
                            }
                            if (child.o.VIP_VIPInfo != null)
                            {
                                var a = Utils.Clone(child.o.VIP_VIPInfo);
                                a.ID = 0;
                                child.Pro_SellListInfo.VIP_VIPInfo = a;
                                child.o.VIP_VIPInfo.Flag = false;
                            }
//                            r = Dal_sell.CheckSpecialOff(child.Pro_SellListInfo, child.VIP_ProOffList_1);
//                            if (r.ReturnValue != true)
//                            {
//                                NoError = false;
//                                continue;
//                            }
                        }
                        else
                        {
                            #region 验证单品优惠 组合优惠 商品信息 销售类别 有无串码 单价
                            //验证单品优惠
//                            if (child.VIP_ProOffList_0 == null || child.VIP_ProOffList_0.VIP_OffList == null)
//                            {
//                                NoError = false;
//                                child.Pro_SellListInfo.Note = "单品优惠不存在或已过期";
//                                continue;
//                            }
                            //验证组合优惠
//                            if (child.VIP_ProOffList_1 == null || child.VIP_ProOffList_1.VIP_OffList == null)
//                            {
//                                NoError = false;
//                                child.Pro_SellListInfo.Note = "组合优惠不存在或已过期或不满足条件";
//                                child.Pro_SellListInfo.Pro_SellSpecalOffList.Note = "组合优惠不存在或已过期或不满足条件";
//                                continue;
//                            }
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
                            #region 合同
                            if (child.Pro_SellListInfo.Pro_BillInfo != null)
                            {
                                #region 判断是否冲突

                                if (
                                    lqh.Umsdb.Pro_BillInfo.All(
                                        p =>
                                            p.MobileIMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI &&
                                             p.Pro_SellListInfo.Pro_SellBackList.Count == 0 &&
                                            lqh.Umsdb.Pro_BillConflictInfo.Where(
                                                q => q.ProID == child.Pro_SellListInfo.ProID)
                                                .All(w => w.ProID_NotConflict != p.ProID)))
                                {
                                    #region 对应终端已销售
                                    if (
                                        lqh.Umsdb.Pro_SellListInfo.Any(
                                            p => p.IMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI))
                                    {
                                        var temp = lqh.Umsdb.Pro_SellListInfo.OrderByDescending(p => p.ID).First(
                                            p => p.IMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI && p.BackID == null);
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
                                                p => p.IMEI == child.Pro_SellListInfo.Pro_BillInfo.MobileIMEI))
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
                            #endregion 合同
                            else

                            if (child.Pro_SellListInfo.Pro_Sell_Yanbao == null)
                            {
                                   
                                if (child.Pro_SellTypeProduct.Price != child.Pro_SellListInfo.ProPrice && child.Pro_SellListInfo.NeedAduit==false)
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "商品的单价有误";
                                    continue;
                                }
                            }
                            #region 延保
                            else
                            {
                                #region 判断是否已销售过延保
                                if (
                                    lqh.Umsdb.Pro_Sell_Yanbao.Any(
                                        p =>
                                        p.MobileIMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI &&
                                        p.Pro_SellListInfo.Pro_SellBackList.Count==0))
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "该商品已销售过延保";
                                    continue;
                                }
                                else
                                {
                                    decimal modelprice = 0;
                                    #region 该商品已销售
                                    if (
                                        lqh.Umsdb.Pro_SellListInfo.Any(
                                            p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI))
                                    {
                                        var temp = lqh.Umsdb.Pro_SellListInfo.OrderByDescending(p => p.ID).First(
                                        p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI && p.BackID == null);
                                        modelprice = temp.YanbaoModelPrice;

                                    }

                                    #endregion
                                    #region 该商品未销售
                                    else
                                    {
                                        if (
                                            model.Pro_SellListInfo.Any(
                                                p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI))
                                        {
                                            var temp = model.Pro_SellListInfo.First(
                                                p => p.IMEI == child.Pro_SellListInfo.Pro_Sell_Yanbao.MobileIMEI);
                                            modelprice = temp.YanbaoModelPrice;
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
                                                p => p.ProID == IMEItemp.Pro_ProInfo.ProID && p.StepPrice >= modelprice))
                                        {
                                            if (lqh.Umsdb.Pro_YanbaoPriceStepInfo.First(
                                                p => p.ProID == IMEItemp.Pro_ProInfo.ProID && p.StepPrice >= modelprice).ProPrice !=
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


                                }
                                #endregion

                            }
                            #endregion
                            #endregion

                            if (!NoError)
                                continue;



                            child.Pro_SellListInfo.LowPrice = child.Pro_SellTypeProduct.LowPrice;
                            child.Pro_SellListInfo.ProCost = child.Pro_SellTypeProduct.ProCost;



                            #region 验证代金券
                            if (!string.IsNullOrEmpty(child.Pro_SellListInfo.TicketID))
                            {
                                //验证代金券
                                r =Dal_sell.CheckProCashTicket2(child.Pro_SellListInfo,child.Pro_SellTypeProduct.Pro_ProInfo);
                                if (r.ReturnValue != true)
                                {
                                    NoError = false;
                                    continue;
                                }
                                cashTickList.Add(new Model.Pro_CashTicket() { Pro_SellListInfo = child.Pro_SellListInfo, TicketID = child.Pro_SellListInfo.TicketID });

                                if (str_ticket.Contains(child.Pro_SellListInfo.TicketID))
                                {
                                    NoError = false;

                                    child.Pro_SellListInfo.Note = child.Pro_SellListInfo.TicketID + "代金券重复";

                                    return new Model.WebReturn() { Obj = model, ReturnValue = false, Message = child.Pro_SellListInfo.Note };

                                }
                                str_ticket.Add(child.Pro_SellListInfo.TicketID);
                            }
                            #endregion



                            #region 单品优惠验证

                            child.Pro_SellListInfo.VIP_OffList = child.VIP_ProOffList_0 == null
                                                                 ? null
                                                                 : child.VIP_ProOffList_0.VIP_OffList;

                            r =Dal_sell.CheckProOff(child.Pro_SellListInfo);
                            if (r.ReturnValue != true)
                            {
                                NoError = false;
                                continue;
                            }

                            #endregion

                            #region 验证组合优惠
                            child.Pro_SellListInfo.Pro_SellSpecalOffList = child.Pro_SellSpecalOffList;
                            r = Dal_sell.CheckSpecialOff(child.Pro_SellListInfo, child.VIP_ProOffList_1);
                            if (r.ReturnValue != true)
                            {
                                NoError = false;
                                continue;
                            }
                            #endregion
                            #region 验证门店优惠
                            if (child.Pro_SellListInfo.OtherOff != 0)
                            {
                                var promain =
                                    lqh.Umsdb.Pro_ProInfo.First(p => p.ProID == child.Pro_SellListInfo.ProID).ProMainID;
                                if (
                                    !lqh.Umsdb.Off_AduitProInfo.Any(
                                        p => p.Off_AduitTypeInfo != null && p.Off_AduitTypeInfo.StartDate < DateTime.Now &&
                                             p.Off_AduitTypeInfo.EndDate > DateTime.Now && p.Off_AduitTypeInfo.Flag &&
                                             p.Off_AduitTypeInfo.Off_AduitHallInfo.Select(q => q.HallID).Contains(sell.HallID)
                                             && p.ProMainID == promain && p.Price >= child.Pro_SellListInfo.OtherOff && p.SellType==child.Pro_SellListInfo.SellType
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
                           // cashPrice += child.Pro_SellListInfo.CashPrice;
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
                                        proSellListRulesInfo.OffPrice > proSellListRulesInfo.RealPrice
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

                            decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.AnBuPrice -
                                            child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice - child.Pro_SellListInfo.OtherOff -
                                            child.Pro_SellListInfo.OffSepecialPrice;
                            if (real < 0) real = 0;
                            real = real + child.Pro_SellListInfo.OtherCash;
                            if (real < child.Pro_SellListInfo.LieShouPrice)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "列收价格不允许超过销售单价";
                                continue;
                            }
                            if (real < rules)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "规则计算有误";
                                continue;

                            }

                            real = real - rules;
                            if (real != child.Pro_SellListInfo.CashPrice)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "实收计算有误";
                                continue;
                            }
                            #endregion
                            #region 串码类验证
                            if (!string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                            {
                                r = Dal_sell.CheckIMEI(child.Pro_IMEI);
                                if (!r.ReturnValue)
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = r.Message;
                                    continue;
                                }
                                child.Pro_SellListInfo.InListID = child.Pro_IMEI.InListID;
                                child.Pro_SellListInfo.ProCost = child.Pro_IMEI.Pro_InOrderList.InitInList.Price;
                                child.Pro_IMEI.Pro_StoreInfo.ProCount--;
                                child.Pro_IMEI.Pro_SellInfo = sell;
                                child.Pro_IMEI.SellID = model.SellID;
                                SellIMEIModels.Add(child.Pro_IMEI);
                            }
                            #endregion



                            #region 非串码类验证
                            if (string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                            {
                                //Dal_sell._NewSellInfo.Pro_SellListInfo = model.Pro_SellListInfo;
                                r = Dal_sell.FitInOrderListIDNoIMEI(child.Pro_SellListInfo, child.Pro_StoreInfo, sellListTemp);
                                if (!r.ReturnValue == true)
                                {
                                    NoError = false;
                                    continue;
                                }

                            }
                            #endregion
                        }
//                        #region 验证实收
//                        decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice - child.Pro_SellListInfo.OffSepecialPrice;
//                        if (real * child.Pro_SellListInfo.ProCount != child.Pro_SellListInfo.CashPrice)
//                        {
//                            NoError = false;
//                            child.Pro_SellListInfo.Note = "实收计算有误";
//                            continue;
//                        }
//                        #endregion



                        

                        

                    }
                       
                    if (!NoError)//有错误
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "提交有误", Obj = model };
                    }
                    if (Sepecial_ProOffList.Count > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "部分组合优惠没有满足条件" };
                    }
                    #region 存在未拣完货的
                    if (sellListTemp.Count > 0)
                    {
                        r = Dal_sell.FitInOrderListIDNoIMEI(sellListTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        model.Pro_SellListInfo.AddRange(sellListTemp);
                    }
                    #endregion
                    #region 将不需要重新计算优惠的列表合并到新的销售表单中

                    model.Pro_SellListInfo.AddRange(OldSellList_NotneedOff);

                    cashPrice += model.Pro_SellListInfo.Sum(p => p.CashPrice*p.ProCount);
//                        cashPrice -= model.Pro_SellListInfo.Sum(p => p.LieShouPrice*p.ProCount);
//                        cashPrice += model.Pro_SellListInfo.Sum(p => p.OtherCash*p.ProCount);
//                         foreach (var proSellSpecalOffList in model.Pro_SellSpecalOffList)
//                         {
//                             
//                             cashPrice -= proSellSpecalOffList.OffMoney;
//                         }
                    #endregion

                    if (cashPrice != model.NewCashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误" };
                    }
                     

                    

                    #region 验证赠品
                    //if (sendList.Count() > 0)
                    //{
                    //    List<Model.VIP_SendProOffList> SendProOffList = new List<Model.VIP_SendProOffList>();
                    //    List<Model.Pro_SellSendInfo> sendTempList = new List<Model.Pro_SellSendInfo>();
                    //    foreach (Model.VIP_OffList off in AllOffList)
                    //    {
                    //        SendProOffList.AddRange(off.VIP_SendProOffList);
                    //    }
                    //    var aa = from b in sendList
                    //             group b by new { b.OffID, b.ProID }

                    //                 into temp
                    //                 from c in temp
                    //                 select new { c.OffID, c.ProID, ProCount = temp.Sum(p => p.ProCount) };
                    //    var send_join_SendProoff = from b in aa
                    //                               join c in SendProOffList
                    //                               on new { b.OffID, b.ProID, PerCount = b.ProCount }
                    //                               equals new { c.OffID, c.ProID, c.PerCount }
                    //                               into temp
                    //                               from d in temp.DefaultIfEmpty()
                    //                               select new { b, d };
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
                    //                              equals new { f.OffID, f.ProID }
                    //                              into temp3
                    //                              from f1 in temp3.DefaultIfEmpty()
                    //                              select new { b, d, e1, f1 };

                    //    foreach (var m in send_join_pro_store)
                    //    {
                    //        #region 有串码
                    //        if (!string.IsNullOrEmpty(m.b.IMEI))
                    //        {
                    //            m.b.ProCount = 1;
                    //            r =Dal_sell.CheckIMEI(m.d);
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
                    //                m.d.SellID = model.SellID;
                    //                m.b.ProCost = m.f1.ProCost;
                    //            }

                    //        }
                    //        #endregion
                    //        #region 无串码
                    //        else
                    //        {
                    //            r = Dal_sell.FitInOrderListIDNoIMEI(m.b, m.e1, sendTempList);
                    //            if (!r.ReturnValue == true)
                    //            {
                    //                NoError = false;
                    //                continue;
                    //            }
                    //        }
                    //        #endregion

                    //    }
                    //    #region 存在未拣完货的赠品

                    //    if (sendTempList.Count > 0)
                    //    {
                    //        r = Dal_sell.FitInOrderListIDNoIMEI(sendTempList, StoreList);
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
                    var tickets = from b in lqh.Umsdb.Pro_CashTicket
                            where str_ticket.Contains(b.TicketID) && (b.IsBack ==null || b.IsBack==false) && !BackticketStr.Contains(b.TicketID)
                            select b;

                    if (tickets.Count() > 0)
                        return new Model.WebReturn() { ReturnValue = false, Message = tickets.First().TicketID + "代金券已被使用" };
                    lqh.Umsdb.Pro_CashTicket.InsertAllOnSubmit(cashTickList);
                    #endregion

                   

                    #region 验证优惠券 和 实际收入
                    //验证优惠券 和 实际收入 
                    r =CheckTicketOff(model, sell.VIP_VIPInfo,sell.SysDate);

                    if (r.ReturnValue != true)
                    {
                        return r;
                    }
                    #endregion


                    #endregion

                    }



                    List<Model.Print_SellListInfo> returnobj = null;
                    if (model.Pro_SellListInfo.Any(p => p.NeedAduit))
                    {
                        EntitySet<Model.Pro_SellSpecalOffList> temp1 = new EntitySet<Pro_SellSpecalOffList>();
                        temp1.AddRange(model.Pro_SellSpecalOffList);
                        foreach (var proSellSpecalOffList in temp1)
                        {
                            proSellSpecalOffList.Pro_SellInfo = null;

                        }
                        EntitySet<Model.Pro_SellListInfo> temp = new EntitySet<Pro_SellListInfo>();
                        EntitySet<Model.Pro_SellBackList> temp2 = new EntitySet<Pro_SellBackList>();
                        temp.AddRange(model.Pro_SellListInfo);
                        temp2.AddRange(model.Pro_SellBackList);
                        foreach (var proSellListInfo in temp)
                        {
                            proSellListInfo.Pro_SellBack = null;
                            // temp.Add(proSellListInfo);   
                            if (proSellListInfo.VIP_VIPInfo != null)
                            {
                                proSellListInfo.VIP_VIPInfo.Flag = false;
                            }

                        }
                        foreach (var proSellBackList in temp2)
                        {
                            proSellBackList.Pro_SellBack = null;
                        }
                        Model.Pro_SellOffAduitInfo aduit = new Model.Pro_SellOffAduitInfo()
                        {
                           
                            ApplyDate = DateTime.Now,
                            ApplyUserID = user.UserID,
                            HallID = sell.HallID,
                            ApplyNote = sell.Note

                        };
                    Pro_SellBackInfo_Aduit model2=new Pro_SellBackInfo_Aduit()
                        {
                            SellBackID = model.SellBackID,
                            UserID=model.UserID,
                            UpdUser = model.UpdUser,
                            UpdDate=model.UpdDate,
                            SysDate=model.SysDate,
                            Note=model.Note,
                            AduitID=model.AduitID,
                            Aduited = model.Aduited,
                            BackMoney=model.BackMoney,
                            BackID=model.BackID,
                            OffTicketID = model.OffTicketID,
                            OffTicketPrice=model.OffTicketPrice,
                            CashTotle=model.CashTotle,
                            BackOffTicketID = model.BackOffTicketID,
                            BackOffTicketPrice = model.BackOffTicketPrice,
                            CardPay =model.CardPay,
                            CashPay=model.CashPay,
                            OldCashTotle = model.OldCashTotle,
                            BillID=model.BillID,
                            ShouldBackCash=model.ShouldBackCash,
                            CusName=model.CusName,
                            CusPhone = model.CusPhone,
                            CusVIPCardID = model.CusVIPCardID,
                            NewCashTotle=model.NewCashTotle,
                            Pro_SellListInfo = temp,
                            Pro_SellBackList = temp2,
                            Pro_SellOffAduitInfo = aduit,
                            Pro_SellSpecalOffList = temp1,
                            SellID = model.SellID
                            
                        };
                    
                        foreach (var backImeiModel in BackIMEIModels)
                        {
                            backImeiModel.Pro_SellInfo = sell;
                            backImeiModel.Pro_SellOffAduitInfo = aduit;

                        }
                        foreach (var sellImeiModel in SellIMEIModels)
                        {
                            sellImeiModel.Pro_SellInfo = null;
                            sellImeiModel.Pro_SellOffAduitInfo = aduit;
                        }

                        lqh.Umsdb.Pro_SellBackInfo_Aduit.InsertOnSubmit(model2);
                    }
                    else{
                        #region 生成单号
                        string SellBackID = "";
                        //string SellBackHallID
                        lqh.Umsdb.OrderMacker2(sell.HallID, ref SellBackID);
                        if (SellBackID == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "单号生成出错" };
                        }
                        model.SellBackID = SellBackID;
                        #endregion
                        returnobj = lqh.Umsdb.Print_SellListInfo.Where(p => p.系统自增外键编号 == model.ID).ToList();

                    lqh.Umsdb.Pro_SellBack.InsertOnSubmit(model);
                        foreach (Model.VIP_VIPInfo vip in viplists)
                        {
                            lqh.Umsdb.VIP_VIPInfo_BAK.InsertOnSubmit(new VIP_VIPInfo_BAK()
                            {
                                Note = vip.Note,
                                SysDate = vip.SysDate,
                                HallID = vip.HallID,
                                Address = vip.Address,
                                Balance = vip.Balance,
                                Birthday = vip.Birthday,
                                Flag = vip.Flag,
                                IDCard = vip.IDCard,
                                IDCard_ID = vip.IDCard_ID,
                                MemberName = vip.MemberName,
                                MobiPhone = vip.MobiPhone,
                                Point = vip.Point,
                                QQ = vip.QQ,
                                Sex = vip.Sex,
                                StartTime = vip.StartTime,
                                Sys_UserInfo = vip.Sys_UserInfo,
                                TelePhone = vip.TelePhone,
                                TypeID = vip.TypeID,
                                UpdUser = vip.UpdUser,
                                Validity = vip.Validity
                                

                            });
                            lqh.Umsdb.VIP_VIPInfo.DeleteOnSubmit(vip);
                        }
                    }

                    #region 刪去临时表
                    if (tempidlist != null && tempidlist.Count > 0)
                    {
                        var needdelete1 =
                            lqh.Umsdb.Pro_SellListInfo_Temp.Where(p =>
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
                    return new Model.WebReturn()
                        {
                            ReturnValue = true,
                            Message = "保存成功",
                            Obj = returnobj
                        };

                }
                
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue=false, Message="服务器错误" };
            }
        }

        /// <summary>
        /// 批发退货 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddWholeSale(Model.Sys_UserInfo user, Model.Pro_SellBack model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {



                    List<Model.Pro_SellListInfo> sellList = new List<Model.Pro_SellListInfo>();
                    //退货的代金券编码列表
                    //List<string> BackticketStr = new List<string>();
                    //退货的组合优惠ID
                    //List<int> backSepecailList = new List<int>();
                    //退货的销售明细ID
                    List<int> backsellListIDs = new List<int>();
                    //旧销售明细 没有使用组合优惠的列表
                    //List<Model.Pro_SellListInfo> OldSellList_needOff = new List<Model.Pro_SellListInfo>();
                    //旧销售明细 不需要重新选优惠的
                    List<Model.Pro_SellListInfo> OldSellList_NotneedOff = new List<Model.Pro_SellListInfo>();

                    //旧销售明细 组合优惠列表
                    //List<Model.Pro_SellSpecalOffList> sellSepecailList = new List<Model.Pro_SellSpecalOffList>();
                    //商品 销售类别
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放串码
                    List<string> IMEI = new List<string>();
                    //存放无串码
                    List<string> ProIDNoIMEI = new List<string>();
                    //存放赠品
                    //List<Model.Pro_SellSendInfo> sendList = new List<Model.Pro_SellSendInfo>();
                    //存放优惠
                    //List<int?> OffID_List = new List<int?>();
                    //退回的优惠
                    //List<int?> backOffID_List = new List<int?>();

                    DataLoadOptions dataload = new DataLoadOptions();
                    dataload.LoadWith<Model.Pro_SellBack>(c => c.Pro_SellListInfo);

                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;

                    #region 验证是否可以不用审批即可取消
                    if (model.BackMoney > user.CancelLimit)//需要审批
                    {
                        if (string.IsNullOrEmpty(model.AduitID))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "退款金额超过" + user.CancelLimit + ",需要审批单" };
                        }
                        var backauditlist = (from b in lqh.Umsdb.Pro_SellBackAduit
                                             where b.AduitID == model.AduitID && b.Aduited == true && b.Passed == true && b.Used != true
                                             select b).ToList();
                        if (backauditlist.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批单不存在或未审核或审核不通过或已使用" };
                        }
                        Model.Pro_SellBackAduit firstBackAudit = backauditlist.First();
                        if (model.BackMoney > firstBackAudit.AduitMoney)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批金额" + firstBackAudit.AduitMoney + "小于当前退款金额" + model.BackMoney };
                        }
                        firstBackAudit.Used = true;
                        firstBackAudit.UseDate = DateTime.Now;

                    }
                    else model.AduitID = null;
                    model.Aduited = true;

                    #endregion

                    #region 获取销售单 验证权限
                    #region 原销售单
                    var Sell_query = from b in lqh.Umsdb.Pro_SellInfo
                                     where b.ID == model.SellID
                                     select b;
                    if (Sell_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单不存在" };
                    }
                    Model.Pro_SellInfo sell = Sell_query.First();
                    #endregion


                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(user, this.MenthodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(sell.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = sell.HallID + "仓库无权操作" };
                    #endregion

                    #region 如果尚未退货，则以最开始的销售单为基础数据，否者 以最后一次退货单为基础数据



                    Model.Pro_SellBack sellback = null;
                    var Sell_query2 = sell.Pro_SellBack.ToList();


                    if (Sell_query2.Count() == 0)
                    {
                        sellList.AddRange(sell.Pro_SellListInfo);
                        model.BackID = 0;
                        //sellSepecailList.AddRange(sell.Pro_SellSpecalOffList);
                        if (model.OldCashTotle != sell.CashTotle - sell.OffTicketPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "原销售单实收有误" };
                        }
                        if (model.BackOffTicketPrice != sell.OffTicketPrice || model.BackOffTicketID != sell.OffTicketID)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "本次退回的优惠券与原销售单不一致" };
                        }
                    }
                    else
                    {
                        int MaxID = Sell_query2.Max(p => p.ID);
                        sellback = Sell_query2.Find(p => p.ID == MaxID);
                        sellList.AddRange(sellback.Pro_SellListInfo);
                        //sellSepecailList.AddRange(sellback.Pro_SellSpecalOffList);
                        model.BackID = sellback.ID;
                        if (model.OldCashTotle != sellback.CashTotle - sellback.OffTicketPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "原销售单实收有误" };
                        }
                        if (model.BackOffTicketPrice != sellback.OffTicketPrice || model.BackOffTicketID != sellback.OffTicketID)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "本次退回的优惠券与原销售单不一致" };
                        }
                    }
                    #endregion



                    #endregion

                    #region 需要退货的商品批次号
                    List<string> back_InorderListID = (from b in model.Pro_SellBackList
                                                       select b.InListID).ToList();


                    #endregion


                    #region 获取当前换货记录的串号、商品编号

                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        ////有商品限制，而且商品不在权限范围内
                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        }
                        if (m.ID > 0)
                        {
                            NoError = false;
                            m.Note = "ID不能大于0，除非需要重新计算";
                            continue;  
                        }
                        else  
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

                        //OffID_List.Add(m.OffID);
                    }
                    //组合的
                    //foreach (Model.Pro_SellSpecalOffList m in model.Pro_SellSpecalOffList)
                    //{
                    //    OffID_List.Add(m.SpecalOffID);
                    //    //sendList.AddRange(m.Pro_SellSendInfo);
                    //}
                    //优惠券的
                    //OffID_List.Add(model.OffTicketID);
                    #endregion

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
                                    where IMEI.Contains(b.IMEI) && b.HallID == sell.HallID
                                    select b).ToList();

                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == sell.HallID && (ProIDNoIMEI.Contains(b.ProID) || back_InorderListID.Contains(b.InListID))
                                     orderby b.InListID
                                     select b).ToList();

                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                       group b by b.ProID into temp
                                       select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();




                    #region 退货记录

                    var SellList_join_BackList = from b in model.Pro_SellBackList
                                                 join c in sellList
                                                 on new { SellListID = (int)b.SellListID }
                                                 equals
                                                    new { SellListID = c.ID }
                                                 into temp
                                                 from c1 in temp.DefaultIfEmpty()
                                                 join d in StoreList
                                                 on b.InListID equals d.InListID
                                                 into temp2
                                                 from d1 in temp2.DefaultIfEmpty()
                                                 join e in imeiList
                                                 on b.IMEI equals e.IMEI
                                                 into temp3
                                                 from e1 in temp3.DefaultIfEmpty()
                                                 select new { Pro_SellBackList = b, Pro_SellListInfo = c1, Pro_StoreInfo = d1, Pro_IMEI = e1 };

                    decimal? backListtotle = 0;
                    foreach (var m in SellList_join_BackList)
                    {
                        if (m.Pro_SellListInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的记录SellListID有误";
                            continue;
                        }
                        if (backsellListIDs.Contains((int)m.Pro_SellBackList.SellListID))
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的记录SellListID重复";
                            continue;
                        }
                        else backsellListIDs.Add((int)m.Pro_SellBackList.SellListID);
                        if (m.Pro_SellBackList.ProCount <= 0)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的数量必须大于0";
                            continue;
                        }
                        if (m.Pro_SellListInfo.ProCount < m.Pro_SellBackList.ProCount)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货数量超限，并未购买这么多商品";
                            continue;
                        }

                        #region 复制退货数据
                        if (m.Pro_SellBackList.IMEI != m.Pro_SellListInfo.IMEI ||
                        m.Pro_SellBackList.InListID != m.Pro_SellListInfo.InListID ||
                        m.Pro_SellBackList.LowPrice != m.Pro_SellListInfo.LowPrice ||
                        m.Pro_SellBackList.OffID != m.Pro_SellListInfo.OffID ||
                        m.Pro_SellBackList.OffPoint != m.Pro_SellListInfo.OffPoint ||
                        m.Pro_SellBackList.OffPrice != m.Pro_SellListInfo.OffPrice ||
                        m.Pro_SellBackList.OffSepecialPrice != m.Pro_SellListInfo.OffSepecialPrice ||
                        m.Pro_SellBackList.ProCost != m.Pro_SellListInfo.ProCost ||
                        m.Pro_SellBackList.ProID != m.Pro_SellListInfo.ProID ||
                        m.Pro_SellBackList.ProPrice != m.Pro_SellListInfo.ProPrice ||
                        m.Pro_SellBackList.SellType != m.Pro_SellListInfo.SellType ||
                        m.Pro_SellBackList.SellType_Pro_ID != m.Pro_SellListInfo.SellType_Pro_ID ||
                        m.Pro_SellBackList.SpecialID != m.Pro_SellListInfo.SpecialID ||
                        m.Pro_SellBackList.TicketID != m.Pro_SellListInfo.TicketID ||
                        m.Pro_SellBackList.TicketUsed != m.Pro_SellListInfo.TicketUsed ||
                        m.Pro_SellBackList.WholeSaleOffPrice != m.Pro_SellListInfo.WholeSaleOffPrice)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货记录的详情与销售记录不相同";
                            continue;
                        }
                        #endregion
                        #region 验证退货记录的退还金额
                        if (m.Pro_SellBackList.CashPrice != m.Pro_SellBackList.ProCount * (m.Pro_SellBackList.ProPrice - m.Pro_SellBackList.OffPrice - m.Pro_SellBackList.OffSepecialPrice - m.Pro_SellBackList.TicketUsed))
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货记录的退还金额不正确";
                            continue;
                        }
                        backListtotle += m.Pro_SellBackList.CashPrice;
                        #endregion

                        #region 如果有兑券
                        //if (!string.IsNullOrEmpty(m.Pro_SellListInfo.TicketID))
                        //{
                        //    BackticketStr.Add(m.Pro_SellListInfo.TicketID);
                        //}
                        #endregion

                        #region 有特殊优惠  单品优惠 将优惠的id 放入列表
                        //if (m.Pro_SellBackList.SpecialID > 0)
                        //{
                        //    for (int i = 0; i < m.Pro_SellBackList.ProCount; i++)
                        //    {
                        //        backOffID_List.Add(m.Pro_SellBackList.OffID);
                        //    }

                        //    if (!backSepecailList.Contains((int)m.Pro_SellBackList.SpecialID))
                        //        backSepecailList.Add((int)m.Pro_SellBackList.SpecialID);
                        //    backOffID_List.Add(m.Pro_SellListInfo.SpecialID);
                        //}

                        #endregion

                        #region 加回库存
                        if (!NoError) continue;
                        //有串码
                        if (!string.IsNullOrEmpty(m.Pro_SellListInfo.IMEI))
                        {
                            if (m.Pro_IMEI == null)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "串号不存在";
                                continue;
                            }

                        }
                        if (m.Pro_StoreInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的批次号不存在";
                            continue;
                        }
                        m.Pro_StoreInfo.ProCount += m.Pro_SellBackList.ProCount;


                        #endregion


                    }
                    #endregion
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "退货记录验证出错", Obj = model };
                    }

                    #region 验证总的退款金额
                    if (model.BackMoney != backListtotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "本次退货记录的总金额计算有误" };
                    }
                    #endregion

                    

                    #region 不需要重新计算的
                    var SellList_Join_SellBackList = from b in sellList
                                                     join c in model.Pro_SellBackList
                                                     on b.ID equals c.SellListID
                                                     into temp
                                                     from c1 in temp.DefaultIfEmpty()
                                                     select new { Pro_SellListInfo = b, Pro_SellBackList = c1 };
                    foreach (var m in SellList_Join_SellBackList)
                    {
                        #region 被取消组合优惠的旧记录
                        if (m.Pro_SellBackList != null && m.Pro_SellListInfo.ProCount > m.Pro_SellBackList.ProCount)
                        {


                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                            {
                                ID = m.Pro_SellBackList.ID,
                                IMEI = m.Pro_SellListInfo.IMEI,
                                InListID = m.Pro_SellListInfo.InListID,
                                LowPrice = m.Pro_SellListInfo.LowPrice,
                                Note = m.Pro_SellListInfo.Note,
                                OffID = m.Pro_SellListInfo.OffID,
                                OffPoint = m.Pro_SellListInfo.OffPoint,
                                OffPrice = m.Pro_SellListInfo.OffPrice,
                                OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                ProCost = m.Pro_SellListInfo.ProCost,
                                ProCount = m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount,
                                ProID = m.Pro_SellListInfo.ProID,
                                ProPrice = m.Pro_SellListInfo.ProPrice,
                                SellType = m.Pro_SellListInfo.SellType,
                                SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                SpecialID = 0,
                                WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                CashPrice = (m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice),
                                //OldSellListID=m.Pro_SellListInfo.ID,
                            };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_NotneedOff.Add(myselllist);

                        }
                        #endregion

                        #region 没有退货，也没有参与组合优惠的,存入不需要再分配优惠的列表中
                        else
                        {
                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                            {
                                ID = m.Pro_SellBackList.ID,
                                IMEI = m.Pro_SellListInfo.IMEI,
                                InListID = m.Pro_SellListInfo.InListID,
                                LowPrice = m.Pro_SellListInfo.LowPrice,
                                Note = m.Pro_SellListInfo.Note,
                                OffID = m.Pro_SellListInfo.OffID,
                                OffPoint = m.Pro_SellListInfo.OffPoint,
                                OffPrice = m.Pro_SellListInfo.OffPrice,
                                OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                ProCost = m.Pro_SellListInfo.ProCost,
                                ProCount = m.Pro_SellListInfo.ProCount,
                                ProID = m.Pro_SellListInfo.ProID,
                                ProPrice = m.Pro_SellListInfo.ProPrice,
                                SellType = m.Pro_SellListInfo.SellType,
                                SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                SpecialID = 0,
                                WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                CashPrice = m.Pro_SellListInfo.CashPrice,//(m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice)
                                CashTicket = m.Pro_SellListInfo.CashTicket,
                                TicketID = m.Pro_SellListInfo.TicketID,
                                TicketUsed = m.Pro_SellListInfo.TicketUsed
                            };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_NotneedOff.Add(myselllist);
                        }
                        #endregion

                         

                    }
                    #endregion


                   


                    #region 验证新销售的产品列表 与 前台传入列表是否相同： 数量 和 字段

                   
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "重新计算的销售记录有误", Obj = model };
                    }
                    #endregion



                    #region 真正生成销售新单部分



                    #region 生成验证销售方式、各种价格左连接部分
                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where SellType_ProID_List.Contains(b.ID)
                                            select b).ToList();
                    #endregion


                    #region 左连 销售方式


                    var join_query = from b in model.Pro_SellListInfo
                                     //join c in pro_off_list
                                     //on new { b.ProID, b.SellType, b.OffID }
                                     //equals
                                     //new { c.ProID, SellType = c.SellTypeID, c.OffID }
                                     //into temp1
                                     //from c1 in temp1.DefaultIfEmpty()
                                     //where b.VIP_OffList.Type == 0
                                     //join d in Sepecial_ProOffList
                                     //on new { b.Pro_SellSpecalOffList, b.ProID, b.ProCount, b.SellType }
                                     //equals
                                     //new { d.Pro_SellSpecalOffList, d.pro_off_list.ProID, d.pro_off_list.ProCount, SellType = d.pro_off_list.SellTypeID }
                                     //into temp2
                                     //from d1 in temp2.DefaultIfEmpty()
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
                                         //VIP_ProOffList_0 = c1,
                                         //VIP_ProOffList_1 = d1.pro_off_list,
                                         Pro_SellTypeProduct = e1,
                                         Pro_IMEI = f1,
                                         Pro_StoreInfo = g1,
                                         //d1
                                     };
                    #endregion
                    List<Model.Pro_CashTicket> cashTickList = new List<Model.Pro_CashTicket>();
                    List<Model.Pro_SellListInfo> sellListTemp = new List<Model.Pro_SellListInfo>();
                    List<string> str_ticket = new List<string>();
                    decimal? cashPrice = 0;

                    DAL.Pro_SellInfo Dal_sell = new Pro_SellInfo();

                    foreach (var child in join_query)
                    {
                        
                            #region 验证 商品信息 销售类别 有无串码 单价
                            
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
                            child.Pro_SellListInfo.IsFree = false;
                            child.Pro_SellListInfo.OffID = 0;
                            child.Pro_SellListInfo.OffPoint = 0;
                            child.Pro_SellListInfo.OffPrice = 0;
                            child.Pro_SellListInfo.OffSepecialPrice = 0;
                            child.Pro_SellListInfo.OldSellListID = 0;
                            child.Pro_SellListInfo.ProID = child.Pro_SellTypeProduct.ProID;
                            child.Pro_SellListInfo.ProPrice = child.Pro_SellTypeProduct.Price;
                            //child.Pro_SellListInfo.SellID = child.Pro_SellTypeProduct.SellType;
                            child.Pro_SellListInfo.SpecialID = 0;
                            child.Pro_SellListInfo.TicketID = null;
                            child.Pro_SellListInfo.TicketUsed = 0;
                            child.Pro_SellListInfo.VIP_OffList = null;
                        
                            

                            cashPrice += child.Pro_SellListInfo.CashPrice;

                            if (child.Pro_SellListInfo.CashPrice != child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.WholeSaleOffPrice - child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffSepecialPrice - child.Pro_SellListInfo.OffPrice)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = "实收金额不对";
                                continue;
                            }

                            #region 串码类验证
                            if (!string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                            {
                                r = Dal_sell.CheckIMEI(child.Pro_IMEI);
                                if (!r.ReturnValue)
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = r.Message;
                                    continue;
                                }
                                child.Pro_IMEI.Pro_StoreInfo.ProCount--;
                                //child.Pro_IMEI.Pro_SellInfo = model;
                                child.Pro_IMEI.SellID = model.SellID;
                            }
                            #endregion



                            #region 非串码类验证
                            if (string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                            {
                                r = Dal_sell.FitInOrderListIDNoIMEI(child.Pro_SellListInfo, child.Pro_StoreInfo, sellListTemp);
                                if (!r.ReturnValue == true)
                                {
                                    NoError = false;
                                    continue;
                                }
                            }
                            #endregion
                        
                        #region 验证实收
                        decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice - child.Pro_SellListInfo.OffSepecialPrice;
                        if (real * child.Pro_SellListInfo.ProCount != child.Pro_SellListInfo.CashPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收计算有误";
                            continue;
                        }
                        #endregion







                    }
                    if (!NoError)//有错误
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "提交有误", Obj = model };
                    }

                    #region 将不需要重新计算优惠的列表合并到新的销售表单中

                    model.Pro_SellListInfo.AddRange(OldSellList_NotneedOff);

                    cashPrice += OldSellList_NotneedOff.Sum(p => p.CashPrice);


                    #endregion

                    if (cashPrice != model.CashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误" };
                    }


                    #region 存在未拣完货的
                    if (sellListTemp.Count > 0)
                    {
                        r = Dal_sell.FitInOrderListIDNoIMEI(sellListTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        model.Pro_SellListInfo.AddRange(sellListTemp);
                    }
                    #endregion

                    #region 验证赠品
                     
                    #endregion

                    #region 保存代金券
                    //var a = from b in lqh.Umsdb.Pro_CashTicket
                    //        where str_ticket.Contains(b.TicketID) && b.IsBack != true && !BackticketStr.Contains(b.TicketID)
                    //        select b;

                    //if (a.Count() > 0)
                    //    return new Model.WebReturn() { ReturnValue = false, Message = a.First().TicketID + "代金券已被使用" };
                    //lqh.Umsdb.Pro_CashTicket.InsertAllOnSubmit(cashTickList);
                    #endregion



                    #region 验证优惠券 和 实际收入
                    //验证优惠券 和 实际收入 
                    if (model.CashTotle - model.OffTicketPrice - model.OldCashTotle != model.CardPay + model.CashPay)
                        return new Model.WebReturn() { ReturnValue = true, Message = "客户补差不正确，应该为"+ (model.CashTotle - model.OffTicketPrice - model.OldCashTotle )};
                    #endregion
                    
                    #region 验证审批单
                    var Sell_Audit_query = from b in lqh.Umsdb.Pro_SellAduit
                                           where b.AduitID == sell.AuditID && b.HallID == sell.HallID
                                           select b;
                    if (Sell_Audit_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在" };
                    }

                    Model.Pro_SellAduit sellAudit = Sell_Audit_query.First();
                    

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

                    #endregion

                    lqh.Umsdb.Pro_SellBack.InsertOnSubmit(model);

                    lqh.Umsdb.SubmitChanges();
                }
                return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务器错误" };
            }
        }

        /// <summary>
        /// 空中充值退货
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddAirCharge(Model.Sys_UserInfo user, Model.Pro_SellBack model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {



                    List<Model.Pro_SellListInfo> sellList = new List<Model.Pro_SellListInfo>();
                    //退货的代金券编码列表
                    //List<string> BackticketStr = new List<string>();
                    //退货的组合优惠ID
                    //List<int> backSepecailList = new List<int>();
                    //退货的销售明细ID
                    List<int> backsellListIDs = new List<int>();
                    //旧销售明细 没有使用组合优惠的列表
                    //List<Model.Pro_SellListInfo> OldSellList_needOff = new List<Model.Pro_SellListInfo>();
                    //旧销售明细 不需要重新选优惠的
                    List<Model.Pro_SellListInfo> OldSellList_NotneedOff = new List<Model.Pro_SellListInfo>();

                    //旧销售明细 组合优惠列表
                    //List<Model.Pro_SellSpecalOffList> sellSepecailList = new List<Model.Pro_SellSpecalOffList>();
                    //商品 销售类别
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放串码
                    //List<string> IMEI = new List<string>();
                    //存放无串码
                    List<string> ProIDNoIMEI = new List<string>();
                    //存放赠品
                    //List<Model.Pro_SellSendInfo> sendList = new List<Model.Pro_SellSendInfo>();
                    //存放优惠
                    //List<int?> OffID_List = new List<int?>();
                    //退回的优惠
                    //List<int?> backOffID_List = new List<int?>();

                    DataLoadOptions dataload = new DataLoadOptions();
                    dataload.LoadWith<Model.Pro_SellBack>(c => c.Pro_SellListInfo);

                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;

                    #region 验证是否可以不用审批即可取消
                    if (model.BackMoney > user.CancelLimit)//需要审批
                    {
                        if (string.IsNullOrEmpty(model.AduitID))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "退款金额超过" + user.CancelLimit + ",需要审批单" };
                        }
                        var backauditlist = (from b in lqh.Umsdb.Pro_SellBackAduit
                                             where b.AduitID == model.AduitID && b.Aduited == true && b.Passed == true && b.Used != true
                                             select b).ToList();
                        if (backauditlist.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批单不存在或未审核或审核不通过或已使用" };
                        }
                        Model.Pro_SellBackAduit firstBackAudit = backauditlist.First();
                        if (model.BackMoney > firstBackAudit.AduitMoney)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批金额" + firstBackAudit.AduitMoney + "小于当前退款金额" + model.BackMoney };
                        }
                        firstBackAudit.Used = true;
                        firstBackAudit.UseDate = DateTime.Now;

                    }
                    else model.AduitID = null;
                    model.Aduited = true;

                    #endregion

                    #region 获取销售单 验证权限
                    #region 原销售单
                    var Sell_query = from b in lqh.Umsdb.Pro_SellInfo
                                     where b.ID == model.SellID
                                     select b;
                    if (Sell_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单不存在" };
                    }
                    Model.Pro_SellInfo sell = Sell_query.First();
                    #endregion


                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(user, this.MenthodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(sell.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = sell.HallID + "仓库无权操作" };
                    #endregion

                    #region 如果尚未退货，则以最开始的销售单为基础数据，否者 以最后一次退货单为基础数据



                    Model.Pro_SellBack sellback = null;
                    var Sell_query2 = sell.Pro_SellBack.ToList();


                    if (Sell_query2.Count() == 0)
                    {
                        sellList.AddRange(sell.Pro_SellListInfo);
                        model.BackID = 0;
                        //sellSepecailList.AddRange(sell.Pro_SellSpecalOffList);
                        if (model.OldCashTotle != sell.CashTotle - sell.OffTicketPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "原销售单实收有误" };
                        }
                        if (model.BackOffTicketPrice != sell.OffTicketPrice || model.BackOffTicketID != sell.OffTicketID)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "本次退回的优惠券与原销售单不一致" };
                        }
                    }
                    else
                    {
                        int MaxID = Sell_query2.Max(p => p.ID);
                        sellback = Sell_query2.Find(p => p.ID == MaxID);
                        sellList.AddRange(sellback.Pro_SellListInfo);
                        //sellSepecailList.AddRange(sellback.Pro_SellSpecalOffList);
                        model.BackID = sellback.ID;
                        if (model.OldCashTotle != sellback.CashTotle - sellback.OffTicketPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "原销售单实收有误" };
                        }
                        if (model.BackOffTicketPrice != sellback.OffTicketPrice || model.BackOffTicketID != sellback.OffTicketID)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "本次退回的优惠券与原销售单不一致" };
                        }
                    }
                    #endregion



                    #endregion

                    #region 需要退货的商品批次号
                    List<string> back_InorderListID = (from b in model.Pro_SellBackList
                                                       select b.InListID).ToList();


                    #endregion


                    #region 获取当前换货记录的串号、商品编号

                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        ////有商品限制，而且商品不在权限范围内
                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        }
                        if (m.ID > 0)
                        {
                            NoError = false;
                            m.Note = "ID不能大于0，除非需要重新计算";
                            continue;
                        }
                        else
                            SellType_ProID_List.Add(m.SellType_Pro_ID);
                        //串码
                        if (!string.IsNullOrEmpty(m.IMEI))
                        {
                            NoError = false;
                            m.Note = "无需串码";
                            continue;
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
 

                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == sell.HallID && (ProIDNoIMEI.Contains(b.ProID) || back_InorderListID.Contains(b.InListID))
                                     orderby b.InListID
                                     select b).ToList();

                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                       group b by b.ProID into temp
                                       select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();




                    #region 退货记录

                    var SellList_join_BackList = from b in model.Pro_SellBackList
                                                 join c in sellList
                                                 on new { SellListID = (int)b.SellListID }
                                                 equals
                                                    new { SellListID = c.ID }
                                                 into temp
                                                 from c1 in temp.DefaultIfEmpty()
                                                 join d in StoreList
                                                 on b.InListID equals d.InListID
                                                 into temp2
                                                 from d1 in temp2.DefaultIfEmpty()
                                                 //join e in imeiList
                                                 //on b.IMEI equals e.IMEI
                                                 //into temp3
                                                 //from e1 in temp3
                                                 select new { Pro_SellBackList = b, Pro_SellListInfo = c1, Pro_StoreInfo = d1 };

                    decimal? backListtotle = 0;
                    foreach (var m in SellList_join_BackList)
                    {
                        if (m.Pro_SellListInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的记录SellListID有误";
                            continue;
                        }
                        if (backsellListIDs.Contains((int)m.Pro_SellBackList.SellListID))
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的记录SellListID重复";
                            continue;
                        }
                        else backsellListIDs.Add((int)m.Pro_SellBackList.SellListID);
                        if (m.Pro_SellBackList.ProCount <= 0)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的数量必须大于0";
                            continue;
                        }
                        if (m.Pro_SellListInfo.ProCount < m.Pro_SellBackList.ProCount)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货数量超限，并未购买这么多商品";
                            continue;
                        }

                        #region 复制退货数据
                        if (m.Pro_SellBackList.IMEI != m.Pro_SellListInfo.IMEI ||
                        m.Pro_SellBackList.InListID != m.Pro_SellListInfo.InListID ||
                        m.Pro_SellBackList.LowPrice != m.Pro_SellListInfo.LowPrice ||
                        m.Pro_SellBackList.OffID != m.Pro_SellListInfo.OffID ||
                        m.Pro_SellBackList.OffPoint != m.Pro_SellListInfo.OffPoint ||
                        m.Pro_SellBackList.OffPrice != m.Pro_SellListInfo.OffPrice ||
                        m.Pro_SellBackList.OffSepecialPrice != m.Pro_SellListInfo.OffSepecialPrice ||
                        m.Pro_SellBackList.ProCost != m.Pro_SellListInfo.ProCost ||
                        m.Pro_SellBackList.ProID != m.Pro_SellListInfo.ProID ||
                        m.Pro_SellBackList.ProPrice != m.Pro_SellListInfo.ProPrice ||
                        m.Pro_SellBackList.SellType != m.Pro_SellListInfo.SellType ||
                        m.Pro_SellBackList.SellType_Pro_ID != m.Pro_SellListInfo.SellType_Pro_ID ||
                        m.Pro_SellBackList.SpecialID != m.Pro_SellListInfo.SpecialID ||
                        m.Pro_SellBackList.TicketID != m.Pro_SellListInfo.TicketID ||
                        m.Pro_SellBackList.TicketUsed != m.Pro_SellListInfo.TicketUsed ||
                        m.Pro_SellBackList.WholeSaleOffPrice != m.Pro_SellListInfo.WholeSaleOffPrice)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货记录的详情与销售记录不相同";
                            continue;
                        }
                        #endregion
                        #region 验证退货记录的退还金额
                        if (m.Pro_SellBackList.CashPrice != m.Pro_SellBackList.ProCount * (m.Pro_SellBackList.ProPrice - m.Pro_SellBackList.OffPrice - m.Pro_SellBackList.OffSepecialPrice - m.Pro_SellBackList.TicketUsed))
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货记录的退还金额不正确";
                            continue;
                        }
                        backListtotle += m.Pro_SellBackList.CashPrice;
                        #endregion

                        #region 如果有兑券
                        //if (!string.IsNullOrEmpty(m.Pro_SellListInfo.TicketID))
                        //{
                        //    BackticketStr.Add(m.Pro_SellListInfo.TicketID);
                        //}
                        #endregion

                        #region 有特殊优惠  单品优惠 将优惠的id 放入列表
                        //if (m.Pro_SellBackList.SpecialID > 0)
                        //{
                        //    for (int i = 0; i < m.Pro_SellBackList.ProCount; i++)
                        //    {
                        //        backOffID_List.Add(m.Pro_SellBackList.OffID);
                        //    }

                        //    if (!backSepecailList.Contains((int)m.Pro_SellBackList.SpecialID))
                        //        backSepecailList.Add((int)m.Pro_SellBackList.SpecialID);
                        //    backOffID_List.Add(m.Pro_SellListInfo.SpecialID);
                        //}

                        #endregion

                        #region 加回库存
                        if (!NoError) continue;
                        
                        if (m.Pro_StoreInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的批次号不存在";
                            continue;
                        }
                        m.Pro_StoreInfo.ProCount += m.Pro_SellBackList.ProCount;


                        #endregion


                    }
                    #endregion
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "退货记录验证出错", Obj = model };
                    }

                    #region 验证总的退款金额
                    if (model.BackMoney != backListtotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "本次退货记录的总金额计算有误" };
                    }
                    #endregion



                    #region 不需要重新计算的
                    var SellList_Join_SellBackList = from b in sellList
                                                     join c in model.Pro_SellBackList
                                                     on b.ID equals c.SellListID
                                                     into temp
                                                     from c1 in temp.DefaultIfEmpty()
                                                     select new { Pro_SellListInfo = b, Pro_SellBackList = c1 };
                    foreach (var m in SellList_Join_SellBackList)
                    {
                        #region 被取消组合优惠的旧记录
                        if (m.Pro_SellBackList != null && m.Pro_SellListInfo.ProCount > m.Pro_SellBackList.ProCount)
                        {


                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                            {
                                ID = m.Pro_SellBackList.ID,
                                IMEI = m.Pro_SellListInfo.IMEI,
                                InListID = m.Pro_SellListInfo.InListID,
                                LowPrice = m.Pro_SellListInfo.LowPrice,
                                Note = m.Pro_SellListInfo.Note,
                                OffID = m.Pro_SellListInfo.OffID,
                                OffPoint = m.Pro_SellListInfo.OffPoint,
                                OffPrice = m.Pro_SellListInfo.OffPrice,
                                OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                ProCost = m.Pro_SellListInfo.ProCost,
                                ProCount = m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount,
                                ProID = m.Pro_SellListInfo.ProID,
                                ProPrice = m.Pro_SellListInfo.ProPrice,
                                SellType = m.Pro_SellListInfo.SellType,
                                SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                SpecialID = 0,
                                WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                CashPrice = (m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice),
                                //OldSellListID=m.Pro_SellListInfo.ID,
                            };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_NotneedOff.Add(myselllist);

                        }
                        #endregion

                        #region 没有退货，也没有参与组合优惠的,存入不需要再分配优惠的列表中
                        else
                        {
                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                            {
                                ID = m.Pro_SellBackList.ID,
                                IMEI = m.Pro_SellListInfo.IMEI,
                                InListID = m.Pro_SellListInfo.InListID,
                                LowPrice = m.Pro_SellListInfo.LowPrice,
                                Note = m.Pro_SellListInfo.Note,
                                OffID = m.Pro_SellListInfo.OffID,
                                OffPoint = m.Pro_SellListInfo.OffPoint,
                                OffPrice = m.Pro_SellListInfo.OffPrice,
                                OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                ProCost = m.Pro_SellListInfo.ProCost,
                                ProCount = m.Pro_SellListInfo.ProCount,
                                ProID = m.Pro_SellListInfo.ProID,
                                ProPrice = m.Pro_SellListInfo.ProPrice,
                                SellType = m.Pro_SellListInfo.SellType,
                                SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                SpecialID = 0,
                                WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                CashPrice = m.Pro_SellListInfo.CashPrice,//(m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice)
                                CashTicket = m.Pro_SellListInfo.CashTicket,
                                TicketID = m.Pro_SellListInfo.TicketID,
                                TicketUsed = m.Pro_SellListInfo.TicketUsed
                            };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_NotneedOff.Add(myselllist);
                        }
                        #endregion



                    }
                    #endregion





                    #region 验证新销售的产品列表 与 前台传入列表是否相同： 数量 和 字段


                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "重新计算的销售记录有误", Obj = model };
                    }
                    #endregion



                    #region 真正生成销售新单部分



                    #region 生成验证销售方式、各种价格左连接部分
                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where SellType_ProID_List.Contains(b.ID)
                                            select b).ToList();
                    #endregion


                    #region 左连 销售方式


                    var join_query = from b in model.Pro_SellListInfo
                                     //join c in pro_off_list
                                     //on new { b.ProID, b.SellType, b.OffID }
                                     //equals
                                     //new { c.ProID, SellType = c.SellTypeID, c.OffID }
                                     //into temp1
                                     //from c1 in temp1.DefaultIfEmpty()
                                     //where b.VIP_OffList.Type == 0
                                     //join d in Sepecial_ProOffList
                                     //on new { b.Pro_SellSpecalOffList, b.ProID, b.ProCount, b.SellType }
                                     //equals
                                     //new { d.Pro_SellSpecalOffList, d.pro_off_list.ProID, d.pro_off_list.ProCount, SellType = d.pro_off_list.SellTypeID }
                                     //into temp2
                                     //from d1 in temp2.DefaultIfEmpty()
                                     join e in Pro_SellTypeList
                                     on b.SellType_Pro_ID equals e.ID
                                     into temp3
                                     from e1 in temp3.DefaultIfEmpty()
                                     //join f in imeiList
                                     //on b.IMEI equals f.IMEI
                                     //into temp4
                                     //from f1 in temp4.DefaultIfEmpty()
                                     join g in StoreList__
                                     on b.ProID equals g.ProID
                                     into temp5
                                     from g1 in temp5.DefaultIfEmpty()
                                     select new
                                     {
                                         Pro_SellListInfo = b,
                                         //VIP_ProOffList_0 = c1,
                                         //VIP_ProOffList_1 = d1.pro_off_list,
                                         Pro_SellTypeProduct = e1,
                                         //Pro_IMEI = f1,
                                         Pro_StoreInfo = g1,
                                         //d1
                                     };
                    #endregion
                    List<Model.Pro_CashTicket> cashTickList = new List<Model.Pro_CashTicket>();
                    List<Model.Pro_SellListInfo> sellListTemp = new List<Model.Pro_SellListInfo>();
                    List<string> str_ticket = new List<string>();
                    decimal? cashPrice = 0;

                    DAL.Pro_SellInfo Dal_sell = new Pro_SellInfo();

                    foreach (var child in join_query)
                    {

                        #region 验证 商品信息 销售类别 有无串码 单价

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
                        child.Pro_SellListInfo.IsFree = false;
                        child.Pro_SellListInfo.OffID = 0;
                        child.Pro_SellListInfo.OffPoint = 0;
                        child.Pro_SellListInfo.OffPrice = 0;
                        child.Pro_SellListInfo.OffSepecialPrice = 0;
                        child.Pro_SellListInfo.OldSellListID = 0;
                        child.Pro_SellListInfo.ProID = child.Pro_SellTypeProduct.ProID;
                        child.Pro_SellListInfo.ProPrice = child.Pro_SellTypeProduct.Price;
                        child.Pro_SellListInfo.SellID = child.Pro_SellTypeProduct.SellType;
                        child.Pro_SellListInfo.SpecialID = 0;
                        child.Pro_SellListInfo.TicketID = null;
                        child.Pro_SellListInfo.TicketUsed = 0;
                        child.Pro_SellListInfo.VIP_OffList = null;



                        cashPrice += child.Pro_SellListInfo.CashPrice;

                      

                        //#region 串码类验证
                        //if (!string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        //{
                        //    r = Dal_sell.CheckIMEI(child.Pro_IMEI);
                        //    if (!r.ReturnValue)
                        //    {
                        //        NoError = false;
                        //        child.Pro_SellListInfo.Note = r.Message;
                        //        continue;
                        //    }
                        //    child.Pro_IMEI.Pro_StoreInfo.ProCount--;
                        //    //child.Pro_IMEI.Pro_SellInfo = model;
                        //    child.Pro_IMEI.SellID = model.SellID;
                        //}
                        //#endregion



                        #region 非串码类验证
                        if (string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = Dal_sell.FitInOrderListIDNoIMEI(child.Pro_SellListInfo, child.Pro_StoreInfo, sellListTemp);
                            if (!r.ReturnValue == true)
                            {
                                NoError = false;
                                continue;
                            }
                        }
                        #endregion

                        #region 验证实收
                        decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice - child.Pro_SellListInfo.OffSepecialPrice;
                        if (real * child.Pro_SellListInfo.ProCount != child.Pro_SellListInfo.CashPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收计算有误";
                            continue;
                        }
                        #endregion







                    }
                    if (!NoError)//有错误
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "提交有误", Obj = model };
                    }

                    #region 将不需要重新计算优惠的列表合并到新的销售表单中

                    model.Pro_SellListInfo.AddRange(OldSellList_NotneedOff);

                    cashPrice += OldSellList_NotneedOff.Sum(p => p.CashPrice);


                    #endregion

                    if (cashPrice != model.NewCashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误" };
                    }


                    #region 存在未拣完货的
                    if (sellListTemp.Count > 0)
                    {
                        r = Dal_sell.FitInOrderListIDNoIMEI(sellListTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        model.Pro_SellListInfo.AddRange(sellListTemp);
                    }
                    #endregion

                    #region 验证赠品

                    #endregion

                    #region 保存代金券
                    //var a = from b in lqh.Umsdb.Pro_CashTicket
                    //        where str_ticket.Contains(b.TicketID) && b.IsBack != true && !BackticketStr.Contains(b.TicketID)
                    //        select b;

                    //if (a.Count() > 0)
                    //    return new Model.WebReturn() { ReturnValue = false, Message = a.First().TicketID + "代金券已被使用" };
                    //lqh.Umsdb.Pro_CashTicket.InsertAllOnSubmit(cashTickList);
                    #endregion



                    #region 验证优惠券 和 实际收入
                    //验证优惠券 和 实际收入 
                    if (model.CashTotle - model.OffTicketPrice - model.OldCashTotle != model.CardPay + model.CashPay)
                        return new Model.WebReturn() { ReturnValue = true, Message = "客户补差不正确，应该为" + (model.CashTotle - model.OffTicketPrice - model.OldCashTotle) };
                    #endregion

                    #region 验证审批单
                    var Sell_Audit_query = from b in lqh.Umsdb.Pro_SellAduit
                                           where b.AduitID == sell.AuditID && b.HallID == sell.HallID
                                           select b;
                    if (Sell_Audit_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在" };
                    }

                    Model.Pro_SellAduit sellAudit = Sell_Audit_query.First();


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

                    #endregion

                    lqh.Umsdb.Pro_SellBack.InsertOnSubmit(model);

                    lqh.Umsdb.SubmitChanges();
                }
                return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务器错误" };
            }
        }

        /// <summary>
        /// 售后退货 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddAfterSale(Model.Sys_UserInfo user, Model.Pro_SellBack model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {



                    List<Model.Pro_SellListInfo> sellList = new List<Model.Pro_SellListInfo>();
                    //退货的代金券编码列表
                    //List<string> BackticketStr = new List<string>();
                    //退货的组合优惠ID
                    //List<int> backSepecailList = new List<int>();
                    //退货的销售明细ID
                    List<int> backsellListIDs = new List<int>();
                    //旧销售明细 没有使用组合优惠的列表
                    //List<Model.Pro_SellListInfo> OldSellList_needOff = new List<Model.Pro_SellListInfo>();
                    //旧销售明细 不需要重新选优惠的
                    List<Model.Pro_SellListInfo> OldSellList_NotneedOff = new List<Model.Pro_SellListInfo>();

                    //旧销售明细 组合优惠列表
                    //List<Model.Pro_SellSpecalOffList> sellSepecailList = new List<Model.Pro_SellSpecalOffList>();
                    //商品 销售类别
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放串码
                    List<string> IMEI = new List<string>();
                    //存放无串码
                    List<string> ProIDNoIMEI = new List<string>();
                    //存放赠品
                    //List<Model.Pro_SellSendInfo> sendList = new List<Model.Pro_SellSendInfo>();
                    //存放优惠
                    //List<int?> OffID_List = new List<int?>();
                    //退回的优惠
                    //List<int?> backOffID_List = new List<int?>();

                    DataLoadOptions dataload = new DataLoadOptions();
                    dataload.LoadWith<Model.Pro_SellBack>(c => c.Pro_SellListInfo);

                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;

                    #region 验证是否可以不用审批即可取消
                    if (model.BackMoney > user.CancelLimit)//需要审批
                    {
                        if (string.IsNullOrEmpty(model.AduitID))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "退款金额超过" + user.CancelLimit + ",需要审批单" };
                        }
                        var backauditlist = (from b in lqh.Umsdb.Pro_SellBackAduit
                                             where b.AduitID == model.AduitID && b.Aduited == true && b.Passed == true && b.Used != true
                                             select b).ToList();
                        if (backauditlist.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批单不存在或未审核或审核不通过或已使用" };
                        }
                        Model.Pro_SellBackAduit firstBackAudit = backauditlist.First();
                        if (model.BackMoney > firstBackAudit.AduitMoney)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批金额" + firstBackAudit.AduitMoney + "小于当前退款金额" + model.BackMoney };
                        }
                        firstBackAudit.Used = true;
                        firstBackAudit.UseDate = DateTime.Now;

                    }
                    else model.AduitID = null;
                    model.Aduited = true;

                    #endregion

                    #region 获取销售单 验证权限
                    #region 原销售单
                    var Sell_query = from b in lqh.Umsdb.Pro_SellInfo
                                     where b.ID == model.SellID
                                     select b;
                    if (Sell_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单不存在" };
                    }
                    Model.Pro_SellInfo sell = Sell_query.First();
                    #endregion


                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(user, this.MenthodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(sell.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = sell.HallID + "仓库无权操作" };
                    #endregion

                    #region 如果尚未退货，则以最开始的销售单为基础数据，否者 以最后一次退货单为基础数据



                    Model.Pro_SellBack sellback = null;

                    var Sell_query2 = sell.Pro_SellBack.ToList();


                    if (Sell_query2.Count() == 0)
                    {
                        sellList.AddRange(sell.Pro_SellListInfo);
                        model.BackID = 0;
                        //sellSepecailList.AddRange(sell.Pro_SellSpecalOffList);
                        if (model.OldCashTotle != sell.CashTotle - sell.OffTicketPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "原销售单实收有误" };
                        }
                        if (model.BackOffTicketPrice != sell.OffTicketPrice || model.BackOffTicketID != sell.OffTicketID)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "本次退回的优惠券与原销售单不一致" };
                        }
                    }
                    else
                    {
                        int MaxID = Sell_query2.Max(p => p.ID);
                        sellback = Sell_query2.Find(p => p.ID == MaxID);
                        sellList.AddRange(sellback.Pro_SellListInfo);
                        //sellSepecailList.AddRange(sellback.Pro_SellSpecalOffList);
                        model.BackID = sellback.ID;
                        if (model.OldCashTotle != sellback.CashTotle - sellback.OffTicketPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "原销售单实收有误" };
                        }
                        if (model.BackOffTicketPrice != sellback.OffTicketPrice || model.BackOffTicketID != sellback.OffTicketID)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "本次退回的优惠券与原销售单不一致" };
                        }
                    }
                    #endregion



                    #endregion

                    #region 需要退货的商品批次号
                    List<string> back_InorderListID = (from b in model.Pro_SellBackList
                                                       select b.InListID).ToList();


                    #endregion

                    #region 会员免费的服务
                    var Vip_Free = from b in lqh.Umsdb.VIP_VIPService
                                   where b.VIPID == sell.VIP_ID && b.SCount > 0
                                   select b;
                    #endregion

                    #region 获取当前换货记录的串号、商品编号

                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        ////有商品限制，而且商品不在权限范围内
                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        }
                        if (m.ID > 0)
                        {
                            NoError = false;
                            m.Note = "ID不能大于0，除非需要重新计算";
                            continue;
                        }
                        else
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

                        //OffID_List.Add(m.OffID);
                    }
                    //组合的
                    //foreach (Model.Pro_SellSpecalOffList m in model.Pro_SellSpecalOffList)
                    //{
                    //    OffID_List.Add(m.SpecalOffID);
                    //    //sendList.AddRange(m.Pro_SellSendInfo);
                    //}
                    //优惠券的
                    //OffID_List.Add(model.OffTicketID);
                    #endregion

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
                                    where IMEI.Contains(b.IMEI) && b.HallID == sell.HallID
                                    select b).ToList();

                    //非串码
                    var StoreList = (from b in lqh.Umsdb.Pro_StoreInfo
                                     where b.HallID == sell.HallID && (ProIDNoIMEI.Contains(b.ProID) || back_InorderListID.Contains(b.InListID))
                                     orderby b.InListID
                                     select b).ToList();

                    //按照商品编号 取批次号最小的
                    var StoreList__ = (from b in StoreList
                                       group b by b.ProID into temp
                                       select temp.Single(p => p.InListID == temp.Min(p2 => p2.InListID))).ToList();




                    #region 退货记录

                    var SellList_join_BackList = from b in model.Pro_SellBackList
                                                 join c in sellList
                                                 on new { SellListID = (int)b.SellListID }
                                                 equals
                                                    new { SellListID = c.ID }
                                                 into temp
                                                 from c1 in temp.DefaultIfEmpty()
                                                 join d in StoreList
                                                 on b.InListID equals d.InListID
                                                 into temp2
                                                 from d1 in temp2.DefaultIfEmpty()
                                                 join e in imeiList
                                                 on b.IMEI equals e.IMEI
                                                 into temp3
                                                 from e1 in temp3.DefaultIfEmpty()
                                                 join f in Vip_Free
                                                 on b.ProID equals f.ProID
                                                 into temp4
                                                 from f1 in temp4.DefaultIfEmpty()
                                                 select new { Pro_SellBackList = b, Pro_SellListInfo = c1, Pro_StoreInfo = d1, Pro_IMEI = e1,VIP_VIPServer= f1 };

                    decimal? backListtotle = 0;
                    foreach (var m in SellList_join_BackList)
                    {
                        if (m.Pro_SellListInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的记录SellListID有误";
                            continue;
                        }
                        if (backsellListIDs.Contains((int)m.Pro_SellBackList.SellListID))
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的记录SellListID重复";
                            continue;
                        }
                        else backsellListIDs.Add((int)m.Pro_SellBackList.SellListID);
                        if (m.Pro_SellBackList.ProCount <= 0)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的数量必须大于0";
                            continue;
                        }
                        if (m.Pro_SellListInfo.ProCount < m.Pro_SellBackList.ProCount)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货数量超限，并未购买这么多商品";
                            continue;
                        }
                        if (!NoError) continue;

                        if (m.Pro_SellListInfo.IsFree == true)
                        {
                            if (m.VIP_VIPServer == null)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "系统出错，该会员的免费服务数据不存在";
                                continue;
                            }
                            m.VIP_VIPServer.SCount += m.Pro_SellListInfo.ProCount;
                        }
                        #region 复制退货数据
                        if (m.Pro_SellBackList.IMEI != m.Pro_SellListInfo.IMEI ||
                        m.Pro_SellBackList.InListID != m.Pro_SellListInfo.InListID ||
                        m.Pro_SellBackList.LowPrice != m.Pro_SellListInfo.LowPrice ||
                        m.Pro_SellBackList.OffID != m.Pro_SellListInfo.OffID ||
                        m.Pro_SellBackList.OffPoint != m.Pro_SellListInfo.OffPoint ||
                        m.Pro_SellBackList.OffPrice != m.Pro_SellListInfo.OffPrice ||
                        m.Pro_SellBackList.OffSepecialPrice != m.Pro_SellListInfo.OffSepecialPrice ||
                        m.Pro_SellBackList.ProCost != m.Pro_SellListInfo.ProCost ||
                        m.Pro_SellBackList.ProID != m.Pro_SellListInfo.ProID ||
                        m.Pro_SellBackList.ProPrice != m.Pro_SellListInfo.ProPrice ||
                        m.Pro_SellBackList.SellType != m.Pro_SellListInfo.SellType ||
                        m.Pro_SellBackList.SellType_Pro_ID != m.Pro_SellListInfo.SellType_Pro_ID ||
                        m.Pro_SellBackList.SpecialID != m.Pro_SellListInfo.SpecialID ||
                        m.Pro_SellBackList.TicketID != m.Pro_SellListInfo.TicketID ||
                        m.Pro_SellBackList.TicketUsed != m.Pro_SellListInfo.TicketUsed ||
                        m.Pro_SellBackList.WholeSaleOffPrice != m.Pro_SellListInfo.WholeSaleOffPrice)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货记录的详情与销售记录不相同";
                            continue;
                        }
                        #endregion
                        #region 验证退货记录的退还金额
                        if (m.Pro_SellBackList.CashPrice != m.Pro_SellBackList.ProCount * (m.Pro_SellBackList.ProPrice - m.Pro_SellBackList.OffPrice - m.Pro_SellBackList.OffSepecialPrice - m.Pro_SellBackList.TicketUsed))
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货记录的退还金额不正确";
                            continue;
                        }
                        backListtotle += m.Pro_SellBackList.CashPrice;
                        #endregion

                        

                        #region 加回库存
                        if (!NoError) continue;
                        //有串码
                        if (!string.IsNullOrEmpty(m.Pro_SellListInfo.IMEI))
                        {
                            if (m.Pro_IMEI == null)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "串号不存在";
                                continue;
                            }

                        }
                        if (m.Pro_StoreInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的批次号不存在";
                            continue;
                        }
                        m.Pro_StoreInfo.ProCount += m.Pro_SellBackList.ProCount;


                        #endregion


                    }
                    #endregion
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "退货记录验证出错", Obj = model };
                    }

                    #region 验证总的退款金额
                    if (model.BackMoney != backListtotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "本次退货记录的总金额计算有误" };
                    }
                    #endregion



                    #region 不需要重新计算的
                    var SellList_Join_SellBackList = from b in sellList
                                                     join c in model.Pro_SellBackList
                                                     on b.ID equals c.SellListID
                                                     into temp
                                                     from c1 in temp.DefaultIfEmpty()
                                                     select new { Pro_SellListInfo = b, Pro_SellBackList = c1 };
                    foreach (var m in SellList_Join_SellBackList)
                    {
                        #region 被取消组合优惠的旧记录
                        if (m.Pro_SellBackList != null && m.Pro_SellListInfo.ProCount > m.Pro_SellBackList.ProCount)
                        {


                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                            {
                                ID = m.Pro_SellBackList.ID,
                                IMEI = m.Pro_SellListInfo.IMEI,
                                InListID = m.Pro_SellListInfo.InListID,
                                LowPrice = m.Pro_SellListInfo.LowPrice,
                                Note = m.Pro_SellListInfo.Note,
                                OffID = m.Pro_SellListInfo.OffID,
                                OffPoint = m.Pro_SellListInfo.OffPoint,
                                OffPrice = m.Pro_SellListInfo.OffPrice,
                                OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                ProCost = m.Pro_SellListInfo.ProCost,
                                ProCount = m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount,
                                ProID = m.Pro_SellListInfo.ProID,
                                ProPrice = m.Pro_SellListInfo.ProPrice,
                                SellType = m.Pro_SellListInfo.SellType,
                                SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                SpecialID = 0,
                                WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                CashPrice = (m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice),
                                //OldSellListID=m.Pro_SellListInfo.ID,
                            };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_NotneedOff.Add(myselllist);

                        }
                        #endregion

                        #region 没有退货，也没有参与组合优惠的,存入不需要再分配优惠的列表中
                        else
                        {
                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                            {
                                ID = m.Pro_SellBackList.ID,
                                IMEI = m.Pro_SellListInfo.IMEI,
                                InListID = m.Pro_SellListInfo.InListID,
                                LowPrice = m.Pro_SellListInfo.LowPrice,
                                Note = m.Pro_SellListInfo.Note,
                                OffID = m.Pro_SellListInfo.OffID,
                                OffPoint = m.Pro_SellListInfo.OffPoint,
                                OffPrice = m.Pro_SellListInfo.OffPrice,
                                OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                ProCost = m.Pro_SellListInfo.ProCost,
                                ProCount = m.Pro_SellListInfo.ProCount,
                                ProID = m.Pro_SellListInfo.ProID,
                                ProPrice = m.Pro_SellListInfo.ProPrice,
                                SellType = m.Pro_SellListInfo.SellType,
                                SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                SpecialID = 0,
                                WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                CashPrice = m.Pro_SellListInfo.CashPrice,//(m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice)
                                CashTicket = m.Pro_SellListInfo.CashTicket,
                                TicketID = m.Pro_SellListInfo.TicketID,
                                TicketUsed = m.Pro_SellListInfo.TicketUsed
                            };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_NotneedOff.Add(myselllist);
                        }
                        #endregion



                    }
                    #endregion





                    #region 验证新销售的产品列表 与 前台传入列表是否相同： 数量 和 字段


                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "重新计算的销售记录有误", Obj = model };
                    }
                    #endregion



                    #region 真正生成销售新单部分



                    #region 生成验证销售方式、各种价格左连接部分
                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where SellType_ProID_List.Contains(b.ID)
                                            select b).ToList();
                    #endregion


                    #region 左连 销售方式


                    var join_query = from b in model.Pro_SellListInfo
                                     //join c in pro_off_list
                                     //on new { b.ProID, b.SellType, b.OffID }
                                     //equals
                                     //new { c.ProID, SellType = c.SellTypeID, c.OffID }
                                     //into temp1
                                     //from c1 in temp1.DefaultIfEmpty()
                                     //where b.VIP_OffList.Type == 0
                                     //join d in Sepecial_ProOffList
                                     //on new { b.Pro_SellSpecalOffList, b.ProID, b.ProCount, b.SellType }
                                     //equals
                                     //new { d.Pro_SellSpecalOffList, d.pro_off_list.ProID, d.pro_off_list.ProCount, SellType = d.pro_off_list.SellTypeID }
                                     //into temp2
                                     //from d1 in temp2.DefaultIfEmpty()
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
                                     join h in Vip_Free
                                              on b.ProID equals h.ProID
                                              into temp6
                                     from h1 in temp6.DefaultIfEmpty()
                                     select new
                                     {
                                         Pro_SellListInfo = b,
                                         //VIP_ProOffList_0 = c1,
                                         //VIP_ProOffList_1 = d1.pro_off_list,
                                         Pro_SellTypeProduct = e1,
                                         Pro_IMEI = f1,
                                         Pro_StoreInfo = g1,
                                         VIP_VIPServer=h1
                                         //d1
                                     };
                    #endregion
                    List<Model.Pro_CashTicket> cashTickList = new List<Model.Pro_CashTicket>();
                    List<Model.Pro_SellListInfo> sellListTemp = new List<Model.Pro_SellListInfo>();
                    List<string> str_ticket = new List<string>();
                    decimal? cashPrice = 0;

                    DAL.Pro_SellInfo Dal_sell = new Pro_SellInfo();

                    foreach (var child in join_query)
                    {

                        #region 验证 商品信息 销售类别 有无串码 单价

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
                        child.Pro_SellListInfo.IsFree = false;
                        child.Pro_SellListInfo.OffID = 0;
                        child.Pro_SellListInfo.OffPoint = 0;
                        child.Pro_SellListInfo.OffPrice = 0;
                        child.Pro_SellListInfo.OffSepecialPrice = 0;
                        child.Pro_SellListInfo.OldSellListID = 0;
                        child.Pro_SellListInfo.ProID = child.Pro_SellTypeProduct.ProID;
                        child.Pro_SellListInfo.ProPrice = child.Pro_SellTypeProduct.Price;
                        child.Pro_SellListInfo.SellID = child.Pro_SellTypeProduct.SellType;
                        child.Pro_SellListInfo.SpecialID = 0;
                        child.Pro_SellListInfo.TicketID = null;
                        child.Pro_SellListInfo.TicketUsed = 0;
                        child.Pro_SellListInfo.VIP_OffList = null;

                        #region 免费服务
                        
                        
                        if (child.Pro_SellListInfo.IsFree == true)
                            {
                                if (child.VIP_VIPServer == null || child.VIP_VIPServer.SCount <= 0)
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "该会员免费服务功能已用完";
                                    continue;
                                }
                                if (child.Pro_SellListInfo.CashPrice != 0)
                                {
                                    NoError = false;
                                    child.Pro_SellListInfo.Note = "免费服务实收金额应为0";
                                    continue;
                                }
                                child.VIP_VIPServer.SCount -= child.Pro_SellListInfo.ProCount;
                            }

                        #endregion

                        cashPrice += child.Pro_SellListInfo.CashPrice;

                        if (cashPrice != model.CashTotle)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误", Obj = model };
                        }

                        #region 串码类验证
                        if (!string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = Dal_sell.CheckIMEI(child.Pro_IMEI);
                            if (!r.ReturnValue)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = r.Message;
                                continue;
                            }
                            child.Pro_IMEI.Pro_StoreInfo.ProCount--;
                            //child.Pro_IMEI.Pro_SellInfo = model;
                            child.Pro_IMEI.SellID = model.SellID;
                        }
                        #endregion



                        #region 非串码类验证
                        if (string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = Dal_sell.FitInOrderListIDNoIMEI(child.Pro_SellListInfo, child.Pro_StoreInfo, sellListTemp);
                            if (!r.ReturnValue == true)
                            {
                                NoError = false;
                                continue;
                            }
                        }
                        #endregion

                        #region 验证实收
                        decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice - child.Pro_SellListInfo.OffSepecialPrice;
                        if (real * child.Pro_SellListInfo.ProCount != child.Pro_SellListInfo.CashPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收计算有误";
                            continue;
                        }
                        #endregion







                    }
                    if (!NoError)//有错误
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "提交有误", Obj = model };
                    }

                    #region 将不需要重新计算优惠的列表合并到新的销售表单中

                    model.Pro_SellListInfo.AddRange(OldSellList_NotneedOff);

                    cashPrice += OldSellList_NotneedOff.Sum(p => p.CashPrice);


                    #endregion

                    if (cashPrice != model.CashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误" };
                    }


                    #region 存在未拣完货的
                    if (sellListTemp.Count > 0)
                    {
                        r = Dal_sell.FitInOrderListIDNoIMEI(sellListTemp, StoreList);
                        if (r.ReturnValue != true)
                        {
                            r.Obj = model;
                            return r;
                        }
                        model.Pro_SellListInfo.AddRange(sellListTemp);
                    }
                    #endregion

                    #region 验证赠品

                    #endregion

                    #region 保存代金券
                    //var a = from b in lqh.Umsdb.Pro_CashTicket
                    //        where str_ticket.Contains(b.TicketID) && b.IsBack != true && !BackticketStr.Contains(b.TicketID)
                    //        select b;

                    //if (a.Count() > 0)
                    //    return new Model.WebReturn() { ReturnValue = false, Message = a.First().TicketID + "代金券已被使用" };
                    //lqh.Umsdb.Pro_CashTicket.InsertAllOnSubmit(cashTickList);
                    #endregion



                    #region 验证优惠券 和 实际收入
                    //验证优惠券 和 实际收入 
                    if (model.CashTotle - model.OffTicketPrice - model.OldCashTotle != model.CardPay + model.CashPay)
                        return new Model.WebReturn() { ReturnValue = true, Message = "客户补差不正确，应该为" + (model.CashTotle - model.OffTicketPrice - model.OldCashTotle) };
                    #endregion

                     

                    #endregion




                    lqh.Umsdb.Pro_SellBack.InsertOnSubmit(model);

                    lqh.Umsdb.SubmitChanges();
                }
                return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务器错误" };
            }
        }


        /// <summary>
        /// 延保退货 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddGXYanBao(Model.Sys_UserInfo user, Model.Pro_SellBack model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            try
            {
                using (LinQSqlHelper lqh = new LinQSqlHelper())
                {

                    List<string> ProIDS = new List<string>(); 

                    List<string> MobileIMEIList = new List<string>();

                    List<Model.Pro_SellListInfo> sellList = new List<Model.Pro_SellListInfo>();
                    //退货的代金券编码列表
                    //List<string> BackticketStr = new List<string>();
                    //退货的组合优惠ID
                    //List<int> backSepecailList = new List<int>();
                    //退货的销售明细ID
                    List<int> backsellListIDs = new List<int>();
                    //旧销售明细 没有使用组合优惠的列表
                    //List<Model.Pro_SellListInfo> OldSellList_needOff = new List<Model.Pro_SellListInfo>();
                    //旧销售明细 不需要重新选优惠的
                    List<Model.Pro_SellListInfo> OldSellList_NotneedOff = new List<Model.Pro_SellListInfo>();

                    //旧销售明细 组合优惠列表
                    //List<Model.Pro_SellSpecalOffList> sellSepecailList = new List<Model.Pro_SellSpecalOffList>();
                    //商品 销售类别
                    List<int?> SellType_ProID_List = new List<int?>();
                    //存放串码
                    List<string> IMEI = new List<string>();
                    //存放无串码
                    //List<string> ProIDNoIMEI = new List<string>();
                    //存放赠品
                    //List<Model.Pro_SellSendInfo> sendList = new List<Model.Pro_SellSendInfo>();
                    //存放优惠
                    //List<int?> OffID_List = new List<int?>();
                    //退回的优惠
                    //List<int?> backOffID_List = new List<int?>();

                    DataLoadOptions dataload = new DataLoadOptions();
                    dataload.LoadWith<Model.Pro_SellBack>(c => c.Pro_SellListInfo);

                    dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                    lqh.Umsdb.LoadOptions = dataload;

                    #region 验证是否可以不用审批即可取消
                    if (model.BackMoney > user.CancelLimit)//需要审批
                    {
                        if (string.IsNullOrEmpty(model.AduitID))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "退款金额超过" + user.CancelLimit + ",需要审批单" };
                        }
                        var backauditlist = (from b in lqh.Umsdb.Pro_SellBackAduit
                                             where b.AduitID == model.AduitID && b.Aduited == true && b.Passed == true && b.Used != true
                                             select b).ToList();
                        if (backauditlist.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批单不存在或未审核或审核不通过或已使用" };
                        }
                        Model.Pro_SellBackAduit firstBackAudit = backauditlist.First();
                        if (model.BackMoney > firstBackAudit.AduitMoney)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单错误：审批金额" + firstBackAudit.AduitMoney + "小于当前退款金额" + model.BackMoney };
                        }
                        firstBackAudit.Used = true;
                        firstBackAudit.UseDate = DateTime.Now;

                    }
                    else model.AduitID = null;
                    model.Aduited = true;

                    #endregion

                    #region 获取销售单 验证权限
                    #region 原销售单
                    var Sell_query = from b in lqh.Umsdb.Pro_SellInfo
                                     where b.ID == model.SellID
                                     select b;
                    if (Sell_query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售单不存在" };
                    }
                    Model.Pro_SellInfo sell = Sell_query.First();
                    #endregion


                    #region 验证仓库 、商品权限
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    r = ValidClassInfo.GetHall_ProIDFromRole(user, this.MenthodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(sell.HallID))
                        return new Model.WebReturn() { ReturnValue = false, Message = sell.HallID + "仓库无权操作" };
                    #endregion

                    #region 如果尚未退货，则以最开始的销售单为基础数据，否者 以最后一次退货单为基础数据



                    Model.Pro_SellBack sellback = null;
                    var Sell_query2 = sell.Pro_SellBack.ToList();


                    if (Sell_query2.Count() == 0)
                    {
                        sellList.AddRange(sell.Pro_SellListInfo);
                        model.BackID = 0;
                        //sellSepecailList.AddRange(sell.Pro_SellSpecalOffList);
                        if (model.OldCashTotle != sell.CashTotle - sell.OffTicketPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "原销售单实收有误" };
                        }
                        if (model.BackOffTicketPrice != sell.OffTicketPrice || model.BackOffTicketID != sell.OffTicketID)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "本次退回的优惠券与原销售单不一致" };
                        }
                    }
                    else
                    {
                        int MaxID = Sell_query2.Max(p => p.ID);
                        sellback = Sell_query2.Find(p => p.ID == MaxID);
                        sellList.AddRange(sellback.Pro_SellListInfo);
                        //sellSepecailList.AddRange(sellback.Pro_SellSpecalOffList);
                        model.BackID = sellback.ID;
                        if (model.OldCashTotle != sellback.CashTotle - sellback.OffTicketPrice)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "原销售单实收有误" };
                        }
                        if (model.BackOffTicketPrice != sellback.OffTicketPrice || model.BackOffTicketID != sellback.OffTicketID)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "本次退回的优惠券与原销售单不一致" };
                        }
                    }
                    #endregion



                    #endregion

                    #region 需要退货的商品批次号
                    List<string> back_InorderListID = (from b in model.Pro_SellBackList
                                                       select b.InListID).ToList();


                    #endregion


                    #region 获取当前换货记录的串号、商品编号

                    foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                    {
                        if (m.Pro_Sell_Yanbao == null || string.IsNullOrEmpty(m.Pro_Sell_Yanbao.MobileIMEI))
                        {
                            NoError = false;
                            m.Note = "宜安保的明细不能为空";
                            continue;
                        }
                        else MobileIMEIList.Add(m.Pro_Sell_Yanbao.MobileIMEI);

                        if (string.IsNullOrEmpty(m.IMEI))
                        {
                            NoError = false;
                            m.Note = "合同号必填";
                            continue;
                        }
                        ////有商品限制，而且商品不在权限范围内
                        if (ValidProIDS.Count() > 0 && !ValidProIDS.Contains(m.ProID))
                        {
                            NoError = false;
                            m.Note = "无权操作";
                            continue;
                        }
                        if (m.ID > 0)
                        {
                            NoError = false;
                            m.Note = "ID不能大于0，除非需要重新计算";
                            continue;
                        }
                        else
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
                    
                    #endregion
  
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = model, Message = "提交错误" };
                    }

                    //串码类拣货
                    var imeiList = (from b in lqh.Umsdb.Pro_IMEI
                                    where IMEI.Contains(b.IMEI) && b.HallID == sell.HallID
                                    select b).ToList();

 


                    #region 退货记录

                    var SellList_join_BackList = from b in model.Pro_SellBackList
                                                 join c in sellList
                                                 on new { SellListID = (int)b.SellListID }
                                                 equals
                                                    new { SellListID = c.ID }
                                                 into temp
                                                 from c1 in temp.DefaultIfEmpty()
                                                 //join d in StoreList
                                                 //on b.InListID equals d.InListID
                                                 //into temp2
                                                 //from d1 in temp2.DefaultIfEmpty()
                                                 join e in imeiList
                                                 on b.IMEI equals e.IMEI
                                                 into temp3
                                                 from e1 in temp3.DefaultIfEmpty()
                                                 select new { Pro_SellBackList = b, Pro_SellListInfo = c1, Pro_IMEI = e1 };

                    decimal? backListtotle = 0;
                    foreach (var m in SellList_join_BackList)
                    {
                        if (m.Pro_SellListInfo == null)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的记录SellListID有误";
                            continue;
                        }
                        if (backsellListIDs.Contains((int)m.Pro_SellBackList.SellListID))
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的记录SellListID重复";
                            continue;
                        }
                        else backsellListIDs.Add((int)m.Pro_SellBackList.SellListID);
                        if (m.Pro_SellBackList.ProCount <= 0)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货的数量必须大于0";
                            continue;
                        }
                        if (m.Pro_SellListInfo.ProCount < m.Pro_SellBackList.ProCount)
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货数量超限，并未购买这么多商品";
                            continue;
                        }
                        
                        #region 复制退货数据
                        if (m.Pro_SellBackList.IMEI != m.Pro_SellListInfo.IMEI ||
                        m.Pro_SellBackList.InListID != m.Pro_SellListInfo.InListID ||
                        m.Pro_SellBackList.LowPrice != m.Pro_SellListInfo.LowPrice ||
                        m.Pro_SellBackList.OffID != m.Pro_SellListInfo.OffID ||
                        m.Pro_SellBackList.OffPoint != m.Pro_SellListInfo.OffPoint ||
                        m.Pro_SellBackList.OffPrice != m.Pro_SellListInfo.OffPrice ||
                        m.Pro_SellBackList.OffSepecialPrice != m.Pro_SellListInfo.OffSepecialPrice ||
                        m.Pro_SellBackList.ProCost != m.Pro_SellListInfo.ProCost ||
                        m.Pro_SellBackList.ProID != m.Pro_SellListInfo.ProID ||
                        m.Pro_SellBackList.ProPrice != m.Pro_SellListInfo.ProPrice ||
                        m.Pro_SellBackList.SellType != m.Pro_SellListInfo.SellType ||
                        m.Pro_SellBackList.SellType_Pro_ID != m.Pro_SellListInfo.SellType_Pro_ID ||
                        m.Pro_SellBackList.SpecialID != m.Pro_SellListInfo.SpecialID ||
                        m.Pro_SellBackList.TicketID != m.Pro_SellListInfo.TicketID ||
                        m.Pro_SellBackList.TicketUsed != m.Pro_SellListInfo.TicketUsed ||
                        m.Pro_SellBackList.WholeSaleOffPrice != m.Pro_SellListInfo.WholeSaleOffPrice )
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货记录的详情与销售记录不相同";
                            continue;
                        }
                        //标记延保明细 已经退货
                        m.Pro_SellListInfo.Pro_Sell_Yanbao.Pro_SellBackList = m.Pro_SellBackList;

                        #endregion
                        #region 验证退货记录的退还金额
                        if (m.Pro_SellBackList.CashPrice != m.Pro_SellBackList.ProCount * (m.Pro_SellBackList.ProPrice - m.Pro_SellBackList.OffPrice - m.Pro_SellBackList.OffSepecialPrice - m.Pro_SellBackList.TicketUsed))
                        {
                            NoError = false;
                            m.Pro_SellBackList.Note = "退货记录的退还金额不正确";
                            continue;
                        }
                        backListtotle += m.Pro_SellBackList.CashPrice;
                        #endregion

                        #region 如果有兑券
                        //if (!string.IsNullOrEmpty(m.Pro_SellListInfo.TicketID))
                        //{
                        //    BackticketStr.Add(m.Pro_SellListInfo.TicketID);
                        //}
                        #endregion

                        #region 有特殊优惠  单品优惠 将优惠的id 放入列表
                        //if (m.Pro_SellBackList.SpecialID > 0)
                        //{
                        //    for (int i = 0; i < m.Pro_SellBackList.ProCount; i++)
                        //    {
                        //        backOffID_List.Add(m.Pro_SellBackList.OffID);
                        //    }

                        //    if (!backSepecailList.Contains((int)m.Pro_SellBackList.SpecialID))
                        //        backSepecailList.Add((int)m.Pro_SellBackList.SpecialID);
                        //    backOffID_List.Add(m.Pro_SellListInfo.SpecialID);
                        //}

                        #endregion

                        #region 加回库存
                        if (!NoError) continue;
                        //有串码
                        if (!string.IsNullOrEmpty(m.Pro_SellListInfo.IMEI))
                        {
                            if (m.Pro_IMEI == null)
                            {
                                NoError = false;
                                m.Pro_SellBackList.Note = "串号不存在";
                                continue;
                            }

                        }
                         


                        #endregion


                    }
                    #endregion
                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "退货记录验证出错", Obj = model };
                    }

                    #region 验证总的退款金额
                    if (model.BackMoney != backListtotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "本次退货记录的总金额计算有误" };
                    }
                    #endregion



                    #region 不需要重新计算的
                    var SellList_Join_SellBackList = from b in sellList
                                                     join c in model.Pro_SellBackList
                                                     on b.ID equals c.SellListID
                                                     into temp
                                                     from c1 in temp.DefaultIfEmpty()
                                                     select new { Pro_SellListInfo = b, Pro_SellBackList = c1 };
                    foreach (var m in SellList_Join_SellBackList)
                    {
                        #region 被取消组合优惠的旧记录
                        if (m.Pro_SellBackList != null && m.Pro_SellListInfo.ProCount > m.Pro_SellBackList.ProCount)
                        {


                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                            {
                                ID = m.Pro_SellBackList.ID,
                                IMEI = m.Pro_SellListInfo.IMEI,
                                InListID = m.Pro_SellListInfo.InListID,
                                LowPrice = m.Pro_SellListInfo.LowPrice,
                                Note = m.Pro_SellListInfo.Note,
                                OffID = m.Pro_SellListInfo.OffID,
                                OffPoint = m.Pro_SellListInfo.OffPoint,
                                OffPrice = m.Pro_SellListInfo.OffPrice,
                                OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                ProCost = m.Pro_SellListInfo.ProCost,
                                ProCount = m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount,
                                ProID = m.Pro_SellListInfo.ProID,
                                ProPrice = m.Pro_SellListInfo.ProPrice,
                                SellType = m.Pro_SellListInfo.SellType,
                                SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                SpecialID = 0,
                                WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                CashPrice = (m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice),
                                //OldSellListID=m.Pro_SellListInfo.ID,
                            };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_NotneedOff.Add(myselllist);

                        }
                        #endregion

                        #region 没有退货，也没有参与组合优惠的,存入不需要再分配优惠的列表中
                        else
                        {
                            Model.Pro_SellListInfo myselllist = new Model.Pro_SellListInfo()
                            {
                                ID = m.Pro_SellBackList.ID,
                                IMEI = m.Pro_SellListInfo.IMEI,
                                InListID = m.Pro_SellListInfo.InListID,
                                LowPrice = m.Pro_SellListInfo.LowPrice,
                                Note = m.Pro_SellListInfo.Note,
                                OffID = m.Pro_SellListInfo.OffID,
                                OffPoint = m.Pro_SellListInfo.OffPoint,
                                OffPrice = m.Pro_SellListInfo.OffPrice,
                                OffSepecialPrice = m.Pro_SellListInfo.OffSepecialPrice,
                                ProCost = m.Pro_SellListInfo.ProCost,
                                ProCount = m.Pro_SellListInfo.ProCount,
                                ProID = m.Pro_SellListInfo.ProID,
                                ProPrice = m.Pro_SellListInfo.ProPrice,
                                SellType = m.Pro_SellListInfo.SellType,
                                SellType_Pro_ID = m.Pro_SellListInfo.SellType_Pro_ID,
                                SpecialID = 0,
                                WholeSaleOffPrice = m.Pro_SellListInfo.WholeSaleOffPrice,
                                CashPrice = m.Pro_SellListInfo.CashPrice,//(m.Pro_SellListInfo.ProCount - m.Pro_SellBackList.ProCount) * (m.Pro_SellListInfo.ProPrice - m.Pro_SellListInfo.OffPrice - m.Pro_SellListInfo.OffSepecialPrice - m.Pro_SellListInfo.WholeSaleOffPrice)
                                CashTicket = m.Pro_SellListInfo.CashTicket,
                                TicketID = m.Pro_SellListInfo.TicketID,
                                TicketUsed = m.Pro_SellListInfo.TicketUsed
                            };
                            if (m.Pro_SellListInfo.OldSellListID > 0)
                                myselllist.OldSellListID = m.Pro_SellListInfo.OldSellListID;
                            else
                                myselllist.OldSellListID = m.Pro_SellListInfo.ID;
                            OldSellList_NotneedOff.Add(myselllist);
                        }
                        #endregion



                    }
                    #endregion





                    #region 验证新销售的产品列表 与 前台传入列表是否相同： 数量 和 字段


                    if (!NoError)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "重新计算的销售记录有误", Obj = model };
                    }
                    #endregion



                    #region 真正生成销售新单部分

                    #region 延保的终端串码销售记录

                    var MobileIMEIList_SellList = (from b in lqh.Umsdb.Pro_SellListInfo
                                                   where MobileIMEIList.Contains(b.IMEI)
                                                   group b by b.IMEI into temp
                                                   select temp.Single(p => p.ID == temp.Max(p2 => p2.ID))).ToList();
                    #endregion

                    #region 生成验证销售方式、各种价格左连接部分
                    var Pro_SellTypeList = (from b in lqh.Umsdb.Pro_SellTypeProduct
                                            where SellType_ProID_List.Contains(b.ID)
                                            select b).ToList();
                    #endregion

                    #region 延保的价格区间
                    var Yanbao_Step = (from b in lqh.Umsdb.Pro_YanbaoPriceStepInfo
                                       where ProIDS.Contains(b.ProID)
                                       group b by b.ProID into temp
                                       from b1 in temp.DefaultIfEmpty()
                                       select new { b1.ProID, Pro_YanbaoPriceStepInfo = temp.Where(p => 1 == 1) }).ToList();

                    //var Yanbao_Step_Max=from b in Yanbao_Step
                    #endregion

                    #region 重复购买延保的终端串码

                    var MobileIMEIAgain = (from b in lqh.Umsdb.Pro_Sell_Yanbao
                                           where MobileIMEIList.Contains(b.MobileIMEI) && (b.BackListID == 0 || b.BackListID == null)
                                           select b).ToList();

                    #endregion
                    #region 左连 销售方式


                    var join_query = from b in model.Pro_SellListInfo
                                     //join c in pro_off_list
                                     //on new { b.ProID, b.SellType, b.OffID }
                                     //equals
                                     //new { c.ProID, SellType = c.SellTypeID, c.OffID }
                                     //into temp1
                                     //from c1 in temp1.DefaultIfEmpty()
                                     //where b.VIP_OffList.Type == 0
                                     //join d in Sepecial_ProOffList
                                     //on new { b.Pro_SellSpecalOffList, b.ProID, b.ProCount, b.SellType }
                                     //equals
                                     //new { d.Pro_SellSpecalOffList, d.pro_off_list.ProID, d.pro_off_list.ProCount, SellType = d.pro_off_list.SellTypeID }
                                     //into temp2
                                     //from d1 in temp2.DefaultIfEmpty()
                                     join e in Pro_SellTypeList
                                     on b.SellType_Pro_ID equals e.ID
                                     into temp3
                                     from e1 in temp3.DefaultIfEmpty()
                                     join f in imeiList
                                     on b.IMEI equals f.IMEI
                                     into temp4
                                     from f1 in temp4.DefaultIfEmpty()
                                     //join g in StoreList__
                                     //on b.ProID equals g.ProID
                                     //into temp5
                                     //from g1 in temp5.DefaultIfEmpty()
                                     join g in Yanbao_Step
                                              on b.ProID equals g.ProID
                                              into temp33
                                     from g1 in temp33.DefaultIfEmpty()
                                     join i in MobileIMEIList_SellList
                                              on b.Pro_Sell_Yanbao.MobileIMEI equals i.IMEI
                                              into temp5
                                     from i1 in temp5.DefaultIfEmpty()
                                     join h in MobileIMEIAgain
                                     on b.Pro_Sell_Yanbao.MobileIMEI equals h.MobileIMEI
                                     into temp6
                                     from h1 in temp6
                                     select new
                                     {
                                         Pro_SellListInfo = b,
                                         //VIP_ProOffList_0 = c1,
                                         //VIP_ProOffList_1 = d1.pro_off_list,
                                         Pro_SellTypeProduct = e1,
                                         Pro_IMEI = f1,
                                         //Pro_StoreInfo = g1,
                                         //d1
                                         Pro_YanbaoPriceStepInfo = g1,
                                         OldPro_SellListInfo = i1,
                                         Pro_Sell_Yanbao = h1
                                     };
                    #endregion
                    List<Model.Pro_CashTicket> cashTickList = new List<Model.Pro_CashTicket>();
                    List<Model.Pro_SellListInfo> sellListTemp = new List<Model.Pro_SellListInfo>();
                    List<string> str_ticket = new List<string>();
                    decimal? cashPrice = 0;

                    DAL.Pro_SellInfo Dal_sell = new Pro_SellInfo();

                    foreach (var child in join_query)
                    {

                        #region 验证 商品信息 销售类别 有无串码 单价

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
                        if (child.Pro_SellTypeProduct.Price != child.Pro_SellListInfo.ProPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "商品的单价有误";
                            continue;
                        }
                        #endregion

                        if (child.Pro_YanbaoPriceStepInfo == null)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "未定义终端的价格区间，无法获取延保价格";
                            continue;
                        }
                        if (child.OldPro_SellListInfo == null)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "终端未销售，未能获取销售价格";
                            continue;
                        }
                        if (child.Pro_Sell_Yanbao != null)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "此终端串码已购买延保";
                            continue;
                        }
                        if (!NoError) continue;
                        //
                        var MinStep = (from b in child.Pro_YanbaoPriceStepInfo.Pro_YanbaoPriceStepInfo
                                       where b.StepPrice >= child.Pro_SellListInfo.Pro_Sell_Yanbao.MobilePrice
                                       select b).OrderBy(p => p.StepPrice);
                        if (MinStep.Count() == 0)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "终端的价格不在延保的价格区间";
                            continue;
                        }
                        Model.Pro_YanbaoPriceStepInfo yanbaoFirst = MinStep.First();
                        if (child.Pro_SellListInfo.ProPrice != yanbaoFirst.ProPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "延保的价格不对，终端价格为" + child.Pro_SellListInfo.Pro_Sell_Yanbao.MobilePrice + ",延保价格应为" + yanbaoFirst.ProPrice;
                            continue;
                        }

                        child.Pro_SellListInfo.OffPoint = 0;
                        child.Pro_SellListInfo.CashTicket = 0;
                        //m.Pro_SellListInfo.IMEI = null;
                        child.Pro_SellListInfo.LowPrice = yanbaoFirst.LowPrice;
                        child.Pro_SellListInfo.ProCost = yanbaoFirst.ProCost;
                        child.Pro_SellListInfo.SpecialID = 0;
                        child.Pro_SellListInfo.TicketID = null;
                        child.Pro_SellListInfo.ProCount = 1;
                        //m.Pro_SellListInfo.TicketUsed = 0;
                        //m.Pro_SellListInfo.WholeSaleOffPrice = 0;
                        if (child.Pro_SellListInfo.CashPrice != child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.WholeSaleOffPrice - child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffSepecialPrice - child.Pro_SellListInfo.OffPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收金额不对";
                            continue;
                        }

                        cashPrice += child.Pro_SellListInfo.CashPrice;

                        if (child.Pro_SellListInfo.CashPrice != child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.WholeSaleOffPrice - child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffSepecialPrice - child.Pro_SellListInfo.OffPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收金额不对";
                            continue;
                        }

                        #region 串码类验证
                        if (!string.IsNullOrEmpty(child.Pro_SellListInfo.IMEI))
                        {
                            r = Dal_sell.CheckIMEI(child.Pro_IMEI);
                            if (!r.ReturnValue)
                            {
                                NoError = false;
                                child.Pro_SellListInfo.Note = r.Message;
                                continue;
                            }
                            child.Pro_IMEI.Pro_StoreInfo.ProCount--;
                            //child.Pro_IMEI.Pro_SellInfo = model;
                            child.Pro_IMEI.SellID = model.SellID;
                        }
                        #endregion



                       

                        #region 验证实收
                        decimal? real = child.Pro_SellListInfo.ProPrice - child.Pro_SellListInfo.TicketUsed - child.Pro_SellListInfo.OffPrice - child.Pro_SellListInfo.OffSepecialPrice;
                        if (real * child.Pro_SellListInfo.ProCount != child.Pro_SellListInfo.CashPrice)
                        {
                            NoError = false;
                            child.Pro_SellListInfo.Note = "实收计算有误";
                            continue;
                        }
                        #endregion







                    }
                    if (!NoError)//有错误
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "提交有误", Obj = model };
                    }

                    #region 将不需要重新计算优惠的列表合并到新的销售表单中

                    model.Pro_SellListInfo.AddRange(OldSellList_NotneedOff);

                    cashPrice += OldSellList_NotneedOff.Sum(p => p.CashPrice);


                    #endregion

                    if (cashPrice != model.CashTotle)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "订单总金额计算有误" };
                    }


                    

                  



                    #region 验证优惠券 和 实际收入
                    //验证优惠券 和 实际收入 
                    if (model.CashTotle - model.OffTicketPrice - model.OldCashTotle != model.CardPay + model.CashPay)
                        return new Model.WebReturn() { ReturnValue = true, Message = "客户补差不正确，应该为" + (model.CashTotle - model.OffTicketPrice - model.OldCashTotle) };
                    #endregion
 

                    #endregion

                    lqh.Umsdb.Pro_SellBack.InsertOnSubmit(model);

                    lqh.Umsdb.SubmitChanges();
                }
                return new Model.WebReturn() { ReturnValue = true, Message = "保存成功" };
            }
            catch (Exception ex)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "服务器错误" };
            }
        }

        /// <summary>
        /// 将退回的优惠数量 加回原优惠限额
        /// </summary>
        /// <param name="offlist"></param>
        /// <param name="c1"></param>
        public int? FitBackOffToOffList(Model.VIP_OffList offlist, int? c1)
        {
            if (c1 == null)
                return c1;
            else offlist.UseLimit -= 1;
            return c1;
        }
        /// <summary>
        /// 使用优惠券，满多少减多少
        /// </summary>
        /// <param name="vip"></param>
        /// <param name="sell"></param>
        /// <param name="offList"></param>
        /// <returns></returns>
        public Model.WebReturn CheckTicketOff(Model.Pro_SellBack model,Model.VIP_VIPInfo vip,DateTime? sysDate)
        {
            
            
            if (model.BackOffTicketID == model.OffTicketID)
            {
                if (model.OffTicketPrice != model.BackOffTicketPrice)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = model.OffTicketID + "优惠券使用金额有误" };
                }
            }
            else
            {
                

                var a = (from b in vip.VIP_OffTicket

                        where (b.Used != true && b.ID == model.OffTicketID
                        && b.VIP_ID == vip.ID && model.CashTotle >= b.VIP_OffList.ArriveMoney
                        && b.VIP_OffList.Flag == true
                        && b.VIP_OffList.EndDate >= sysDate
                        && b.VIP_OffList.StartDate <= sysDate
                        && b.VIP_OffList.Type == 3) || b.ID==model.BackOffTicketID
                        select b).ToList();

                #region 退掉原优惠券
		        if(model.BackOffTicketID>0)
                {
                    var a_1 = a.Where(p=>p.ID==model.BackOffTicketID);
                    if(a_1.Count()==0)
                        return new Model.WebReturn() { ReturnValue = false, Message = model.BackOffTicketID + "退回的优惠券不存在" };
                    Model.VIP_OffTicket t = a_1.First();
                    if (t.Used != true)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = model.BackOffTicketID + "退回的优惠券不处于已使用状态" };
                    }
                    t.Used = false;
                    t.UseDate = null;
                }
	            #endregion
                if (model.OffTicketID > 0)
                {
                    var a_1 = a.Where(p => p.ID == model.OffTicketID);
                    if (a_1.Count() == 0)
                        return new Model.WebReturn() { ReturnValue = false, Message = model.OffTicketID + "优惠券不存在" };
                    Model.VIP_OffTicket t = a_1.First();
                    if (t.Used == true)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = model.OffTicketID + "退优惠券已使用" };
                    }
                    t.Used = true;
                    t.UseDate = DateTime.Now;
                    if (model.OffTicketPrice != t.VIP_OffList.OffMoney)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = model.OffTicketPrice + "退优惠券优惠金额有误" };
                    }
                   

                }
                
            }
            if (model.NewCashTotle + model.ShouldBackCash - model.BackMoney - model.OffTicketPrice - model.OldCashTotle != model.CardPay + model.CashPay)
                return new Model.WebReturn() { ReturnValue = false, Message = "实收总金额有误" };

            return new Model.WebReturn() { ReturnValue = true };
        }
    }
}
