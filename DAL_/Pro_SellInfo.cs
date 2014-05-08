using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

using System.Transactions;

namespace DAL_
{
    /// <summary>
    /// 销售类
    /// </summary>
    public class Pro_SellInfo
    {
        /// <summary>
        /// 新增销售
        /// </summary>
        /// <remarks></remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.UserInfo user, Model.Pro_SellInfo model)
        {
            
            //model.Pro_SellListInfo[0].VIP_OffList.
            //验证优惠的有效性
            //生成单号
            //
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                DataLoadOptions d = new DataLoadOptions();
                d.LoadWith<Model.Pro_ProInfo>(c => c.Pro_SellTypeProduct);

                d.LoadWith<Model.VIP_OffList>(c => c.VIP_HallOffInfo);
                d.LoadWith<Model.VIP_OffList>(c => c.VIP_ProOffList);
                d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPOffLIst);
                d.LoadWith<Model.VIP_OffList>(c => c.VIP_VIPTypeOffLIst);

                lqh.Umsdb.LoadOptions = d;

                var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                select b;
                if (vip_query.Count() <= 0)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "该会员不存在" };
                }
                var vip = vip_query.First();


                var selllist_query= CheckProSellType(model.Pro_SellListInfo.ToList(), lqh);
                var null_query=selllist_query.Where(p=>p.Pro_ProInfo==null || p.Pro_SellType==null);
                if (null_query.Count() > 0)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = null_query.First().ProID+"商品信息或销售方式错误"};
                }
                Model.WebReturn r= CheckProCashTicket(selllist_query);

                if (r.ReturnValue != true)
                {
                    return r;
                }

                List<int?> OffID_List = new List<int?>();
                foreach (Model.Pro_SellListInfo m in model.Pro_SellListInfo)
                {
                    OffID_List.Add(model.OffID);
                }
                foreach (Model.Pro_SellSpecalOffList m in model.Pro_SellSpecalOffList)
                {
                    OffID_List.Add(m.SpecalOffID);
                }
                OffID_List.Add(model.OffID);
                OffID_List.Add(model.OffTicketID);
                var pro_off_list =( from b in lqh.Umsdb.VIP_ProOffList
                               where OffID_List.Contains(b.ID)
                                && ((((from c in b.VIP_OffList.VIP_VIPOffLIst //会员卡专属优惠
                                    where c.VIPID == model.VIP_ID
                                    select c).Count() > 0
                                                     ||
                                                     (from c in b.VIP_OffList.VIP_VIPTypeOffLIst//会员类别专属优惠
                                                      where c.VIPType == vip.TypeID
                                                      select c).Count() > 0
                                                     )
                                                     && (from c in b.VIP_OffList.VIP_HallOffInfo//门店专属优惠
                                                         where c.HallID == model.HallID
                                                         select c).Count() > 0
                                                     ) )
                                                     && b.VIP_OffList.Flag == true
                                                   && b.VIP_OffList.EndDate >= DateTime.Now
                                                   && b.VIP_OffList.StartDate <= DateTime.Now
                                                   && new int?[] { 0, 1, 2,3 }.Contains(b.VIP_OffList.Type)
                                                   && b.VIP_OffList.UseLimit < b.VIP_OffList.VIPTicketMaxCount
                                   orderby b.VIP_OffList.OffMoney descending
                               select b).ToList();
                //筛选单品的优惠
                r = CheckProOff(selllist_query, pro_off_list.Where(p=>p.VIP_OffList.Type==0).ToList());
                if (r.ReturnValue != true)
                {
                    return r;
                }

                //model.Pro_SellListInfo.Clear();
                //model.Pro_SellListInfo.AddRange(selllist_query);

                 
                 

            
            }
            return null;
        }
        public Model.WebReturn CheckSpecialOff(List<Model.Pro_SellListInfo> selllist, List<Model.VIP_ProOffList> prooff_List,List<Model.Pro_SellSpecalOffList> SellSpecalOffList)
        {
            foreach (Model.Pro_SellSpecalOffList m in SellSpecalOffList)
            { 
                //var selllist_query=from  b in selllist
                //                   join c in prooff_List
                //                   on new { b.ProID,b.SellType}
                //                   equals
                //                   new {c.ProID,SellType=c.SellTypeID}
                //                   into temp
                //                   from d in temp.DefaultIfEmpty()
                //                   where d.VIP_OffList.ID==m.SpecalOffID
                //                   select 
            }
            return null;
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
        /// 计算优惠金额
        /// </summary>
        /// <param name="m"></param>
        /// <param name="off"></param>
        /// <returns></returns>
        private Model.WebReturn  MoneyOrRateOrPoint(Model.Pro_SellListInfo m )
        {
            Model.VIP_OffList off = m.VIP_OffList;
            if (off == null) return new Model.WebReturn { ReturnValue = false, Message = m.OffID + "优惠信息有误" };
            else if (off.OffRate > 0) m.CashPrice = m.ProPrice * off.OffRate;
            else if (off.OffMoney > 0) m.CashPrice = (m.CashPrice - off.OffMoney >= 0 ? m.CashPrice - off.OffMoney : 0);
            else if (off.OffPoint > 0 && off.MinPoint <= m.OffPoint / m.ProCount && m.OffPoint / m.ProCount <= off.MaxPoint)
            {
                decimal? i = m.CashPrice - off.OffPointMoney * m.OffPoint / off.OffPoint;
                if (i < 0) return new Model.WebReturn() { ReturnValue = false, Message = m.Pro_ProInfo.ProName + "积分使用超限" };
                else m.CashPrice = i;
            }
            else return new Model.WebReturn() { ReturnValue = false, Message = m.OffID + "优惠信息有无" };
            return new Model.WebReturn() { ReturnValue = true, Message =  "成功" };
        }
        /// <summary>
        /// 验证销售方式的有效性
        /// </summary>
        /// <returns></returns>
        public List<Model.Pro_SellListInfo> CheckProSellType(List<Model.Pro_SellListInfo> sellList,LinQSqlHelper lqh)
        {
            var ProID_List = from b in sellList
                             select b.ProID;
            //var Pro_List =( from b in lqh.Umsdb.Pro_ProInfo
                           //where ProID_List.Contains(b.ProID)
                           //select b).ToList();
            var Pro_SellTypeList=from b in lqh.Umsdb.Pro_SellTypeProduct
                                 where ProID_List.Contains(b.ProID)
                                 select b;
            
            var SellList_query=from b in sellList
                               join  c in Pro_SellTypeList 
                               on new {b.ProID,b.SellType} equals new {c.ProID,c.SellType}  
                               into temp
                               from d in temp.DefaultIfEmpty()
 
                               select new Model.Pro_SellListInfo{
                                   CashPrice = (d == null ? null : d.Price),
                                   CashTicket=b.CashTicket,
                                   TicketID=b.TicketID,
                                   TicketUsed=b.TicketUsed,
                                   InListID=b.InListID,
                                   LowPrice=b.LowPrice,
                                   ProCost=b.ProCost,
                                   OffPoint=b.OffPoint,
                                   ProPrice = (d == null ? null : d.Price),
                                   Note=b.Note,
                                   OffID=b.OffID,
                                   ProID=b.ProID,
                                   ProCount=b.ProCount, 
                                   SellType=b.SellType,
                                   Pro_ProInfo = (d == null ? null : d.Pro_ProInfo),
                                   Pro_SellType = (d == null ? null : d.Pro_SellType),

                               };
            return SellList_query.ToList();
        }
        /// <summary>
        /// 验证代金券的有效性
        /// </summary> 
        /// <returns></returns>
        public Model.WebReturn CheckProCashTicket( List<Model.Pro_SellListInfo> sellList)
        {
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
                
                Model.Pro_ProInfo band = model.Pro_ProInfo;

                string date = "20" + model.TicketID.Substring(4, 2) + "-" + model.TicketID.Substring(6, 2) + "-" + model.TicketID.Substring(8, 2);
                decimal? ticket = model.CashTicket; ;
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
            return new Model.WebReturn() { ReturnValue=true, Message="代金券验证成功" };
            
        }
        /// <summary>
        /// 获取优惠
        /// </summary>
        /// <remarks></remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetSellOff(Model.UserInfo user, Model.Pro_SellInfo model)
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

                lqh.Umsdb.LoadOptions = d;

                    try
                    {
                        var vip_query = from b in lqh.Umsdb.VIP_VIPInfo
                                        select b;
                        if (vip_query.Count() <= 0)
                        {
                            return new Model.WebReturn() {  ReturnValue = false, Message = "该会员不存在" };
                        }
                        var vip = vip_query.First();
                  
                        var Selllist_temp=from b in  model.Pro_SellListInfo
                                          group b by new 
                                          {
                                              b.ProID,
                                              b.ProPrice,
                                              b.SellType
                                          }
                                          into temp
                                          select new Model.Pro_SellListInfo
                                          {
                                             SellType=temp.Key.SellType,
                                             ProID=temp.Key.ProID,
                                             ProCount=temp.Sum(p=>p.ProCount),
                                             ProPrice=temp.Key.ProPrice
                                          };
                        decimal? totle = Selllist_temp.Sum(p=>p.ProPrice==null?0:p.ProPrice);
                        //var proOff_query = from c in lqh.Umsdb.VIP_ProOffList
                        //        join d in lqh.Umsdb.Pro_SellListInfo
                        //        on new { c.ProID, SellType = c.SellTypeID, c.ProCount }
                        //            equals
                        //           new { d.ProID, d.SellType, d.ProCount }
                        //        select new { AAA = d.VIP_OffList };

                        var AllVIPOff_query = from b in lqh.Umsdb.VIP_OffList

                                              where (( ((from c in b.VIP_VIPOffLIst //会员卡专属优惠
                                                      where c.VIPID == model.VIP_ID
                                                      select c).Count() > 0
                                                     ||
                                                     (from c in b.VIP_VIPTypeOffLIst//会员类别专属优惠
                                                      where c.VIPType == vip.TypeID
                                                      select c).Count() > 0
                                                     )
                                                     && (from c in b.VIP_HallOffInfo//门店专属优惠
                                                         where c.HallID == model.HallID
                                                         select c).Count() > 0
                                                     )
                                                     || (
                                                     b.Type==2 &&
                                                     (from c in b.VIP_OffTicket
                                                         where c.VIP_ID == model.VIP_ID 
                                                         && (c.Used ==false || c.Used==null ) 
                                                         && c.OffTypeID == b.ID
                                                         && c.VIP_OffList.ArriveMoney<=totle
                                                         select c
                                                        ).Count() > 0
                                                     ))
                                                     && b.Flag == true
                                                   && b.EndDate >= DateTime.Now
                                                   && b.StartDate <= DateTime.Now
                                                   && new int?[] { 0, 1, 2 }.Contains(b.Type)
                                                   && b.UseLimit < b.VIPTicketMaxCount 
                                              select b;
                        var AllProOff_query = (from b in lqh.Umsdb.VIP_ProOffList
                                               where (from c in AllVIPOff_query where c.ID==b.OffID select c).Count()>0
                                              select b).ToList();
                        var AllProOff_Join_SellList = from b in AllProOff_query
                                                      
                                                      join  c in Selllist_temp 
                                                      on new { b.ProID, SellType = b.SellTypeID }
                                                      equals
                                                      new { c.ProID, SellType = c.SellType }
                                                       
                                                      select new { 
                                                      b.VIP_OffList,
                                                      HasPro = c.ProCount >= b.ProCount?true:false
                                                      }
                                                    ;
                        var FitSingleProOff_query =( from b in AllProOff_Join_SellList
                                              where (b.VIP_OffList.Type == 0 && b.HasPro)//符合单品的活动，只要有一个单品符合，该优惠就符合要求
                                              select b.VIP_OffList.ID).Distinct();
                        var UnFitMultProOff_query = (from b in AllProOff_Join_SellList
                                                    where (b.VIP_OffList.Type == 1 && b.HasPro==false)//不符合组合活动，只要有一个单品不符合，该优惠就不符合要求
                                                     select b.VIP_OffList.ID).Distinct();
                        var AllFitOff_query = from b in AllVIPOff_query
                                              where (b.Type==0 && FitSingleProOff_query.Contains(b.ID))
                                              || (b.Type==1 &&!UnFitMultProOff_query.Contains(b.ID))
                                              select b;
                        
                                             
                        Model.WebReturn r=  new Model.WebReturn(){ ReturnValue = true, Message = "获取成功" };
                         

                        r.ArrList.AddRange(AllFitOff_query.ToList());

                        return r;
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "系统错误，"+ex.Message };
                    }
                 
            }
            //throw new Exception();
        }
        //public Model.Pro_SellInfo CheckSpecielOff(Model.Pro_SellInfo model)
        //{ 
        //    //特殊优惠 
        //    throw new Exception();
        //}
        //public Model.Pro_SellInfo CheckSellOff(Model.Pro_SellInfo model)
        //{
        //    //整单优惠
        //    throw new Exception();
        //}
        //public Model.Pro_SellInfo CheckTicketOff(Model.Pro_SellInfo model)
        //{
        //    //优惠券优惠
        //    throw new Exception();
        //}
    }
}
