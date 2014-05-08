using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class Pro_ChangeProInfo
    {
        private int _MethodID;

        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public Pro_ChangeProInfo()
        {
            this.MethodID = 0;
        }

        public Pro_ChangeProInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
        #region 新增转换单
        /// <summary>
        /// 转类别
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_ChangeProInfo model)
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
                        if (ValidHallIDS.Count() > 0 && !ValidHallIDS.Contains(model.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作 " + que.First().HallName };
                        }
                        #endregion

                        //生成单号 存储过程OrderMacker

                        string msg = "";
                        lqh.Umsdb.OrderMacker(model.Pro_ChangeProListInfo.Count(), "CL", "CL", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成编号出错" };
                        }
                        string[] InListIDStr = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (InListIDStr.Count() != model.Pro_ChangeProListInfo.Count())
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成商品编号数量与商品数量不一致" };
                        }
                        #region 添加到Pro_ChangeProInfo

                        model.SysDate = DateTime.Now;
                        model.UserID = user.UserID;
                        model.ChangeDate = DateTime.Now;

                        for (int i = 0; i < model.Pro_ChangeProListInfo.Count(); i++)
                        {
                            model.Pro_ChangeProListInfo[i].NewInListID = InListIDStr[i];
                        }
                        lqh.Umsdb.Pro_ChangeProInfo.InsertOnSubmit(model);
                        //  lqh.Umsdb.SubmitChanges();
                        #endregion

                        #region 更新串码和库存
                        foreach (var Item in model.Pro_ChangeProListInfo)
                        {
                            #region 新增入库明细
                            var queryInOrder = (from b in lqh.Umsdb.Pro_InOrderList
                                                where b.InListID == Item.InListID
                                                select b).ToList();
                            if (queryInOrder.Count() != 1)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "获取原批次号出错，请联系管理员！" };
                            }
                            Model.Pro_InOrderList NewList = new Pro_InOrderList();
                            NewList.Price = queryInOrder[0].Price;
                            NewList.InOrderID = queryInOrder[0].InOrderID;
                            NewList.Pro_InOrderID = 0;

                            NewList.InListID = Item.NewInListID;
                            var init = from a in lqh.Umsdb.Pro_InOrderList
                                       where a.InListID == Item.InListID && a.ProID == Item.OldProID
                                       select a;
                            if (init.Count() == 0)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "获取原批次号出错，请联系管理员！" };
                            }

                            NewList.InitInListID = init.First().InitInListID;


                            NewList.ProID = Item.NewProID;
                            NewList.ProCount = Item.ProCount;
                            NewList.Pro_InOrderIMEI = new System.Data.Linq.EntitySet<Pro_InOrderIMEI>();
                            if (Item.Pro_ChangeIMEIInfo != null)
                            {
                                foreach (var item in Item.Pro_ChangeIMEIInfo)
                                {
                                    Model.Pro_InOrderIMEI OrderIMEI = new Pro_InOrderIMEI() { IMEI = item.IMEI };
                                    NewList.Pro_InOrderIMEI.Add(OrderIMEI);
                                }
                            }
                            #region 添加库存
                            NewList.Pro_StoreInfo = new System.Data.Linq.EntitySet<Pro_StoreInfo>
                            {
                                new Pro_StoreInfo()
                                {
                                ProID = Item.NewProID,
                                HallID = model.HallID,
                                InListID = Item.NewInListID,
                                ProCount = Item.ProCount
                                }
                            };
                            #endregion

                            #endregion                          
                            #region 有串码更新串码
                            if (Item.Flag==false)
                            {

                                if (Item.Pro_ChangeIMEIInfo != null && Item.Pro_ChangeIMEIInfo.Count() > 0)
                                {
                                    var IMEIList = (from b in Item.Pro_ChangeIMEIInfo
                                                    select b.IMEI).ToList();
                                    var DelIMEI = from b in lqh.Umsdb.Pro_IMEI
                                                  where IMEIList.Contains(b.IMEI) && (b.OutID == null || b.OutID == 0) && (b.BorowID == null || b.BorowID == 0)
                                              && (b.RepairID == null || b.RepairID == 0) && (b.AuditID == null || b.AuditID == 0)
                                              && b.HallID == model.HallID
                                                  select b;
                                    if (DelIMEI.Count() != Item.ProCount)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "有串码商品转无串码商品时无法删除原来串码，串码存在其他操作！" };
                                    }
                                    // 验证商品是否和前台商品一致
                                    List<string> ProList = (from b in DelIMEI
                                                            select b.ProID).Distinct().ToList();
                                    if (ProList.Count() != 1)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "无此商品或存在多个相同商品！" };
                                    }
                                    if (Item.OldProID != ProList[0])
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "前后台商品不一致！" };
                                    }
                                    lqh.Umsdb.Pro_IMEI.DeleteAllOnSubmit(DelIMEI);//删除串码 
                                    List<Model.Pro_IMEI_Deleted> delList = new List<Pro_IMEI_Deleted>();
                                    foreach (var deliemi in DelIMEI)
                                    {
                                        Model.Pro_IMEI_Deleted del = new Pro_IMEI_Deleted() { IMEI = deliemi.IMEI };
                                        delList.Add(del);
                                    }
                                    lqh.Umsdb.Pro_IMEI_Deleted.InsertAllOnSubmit(delList);//删除串码 
                                    lqh.Umsdb.SubmitChanges();
                                }
                                
                            }
                            else
                            {
                                List<string> IMEIList = (from b in Item.Pro_ChangeIMEIInfo
                                                         select b.IMEI).ToList();

                                List<Model.Pro_IMEI> ProIMEI = (from b in lqh.Umsdb.Pro_IMEI
                                                                where IMEIList.Contains(b.IMEI)
                                                                select b).ToList();
                   
                                var query = from b in lqh.Umsdb.Pro_ProInfo
                                            where b.ProID == Item.OldProID
                                            select b;

                                if (query.Count() == 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "部分商品不存在！" };
                                }
                                if (query.First().NeedIMEI == false)
                                {
                                    if (ProIMEI.Count() > 0)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "部分串码已存在！" };
                                    }

                                    NewList.Pro_IMEI = new System.Data.Linq.EntitySet<Model.Pro_IMEI>();
                                    foreach (var iemiItem in IMEIList)
                                    {
                                        Pro_IMEI imei = new Pro_IMEI() { IMEI = iemiItem, ProID = Item.NewProID, HallID = model.HallID, InListID = Item.NewInListID };
                                        NewList.Pro_IMEI.Add(imei);
                                    }

                                }
                                else
                                {
                                    if (IMEIList.Count() != ProIMEI.Count())
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "部分串码不存在！" };
                                    }
                                    ProIMEI = (from b in ProIMEI
                                               where b.OutID == null && b.BorowID == null && b.RepairID == null && b.HallID == model.HallID
                                               select b).ToList();
                                    if (IMEIList.Count() != ProIMEI.Count())
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "部分串码存在其他操作！" };
                                    }
                                    List<string> ProList = (from b in ProIMEI
                                                            select b.ProID).Distinct().ToList();
                                    if (ProList.Count() != 1)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "无此商品或存在多个相同商品！" };
                                    }
                                    if (Item.OldProID != ProList[0])
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "该串码已经被操作！" };
                                    }
                                    foreach (var ItemIMEI in ProIMEI)
                                    {
                                        ItemIMEI.ProID = Item.NewProID;
                                        ItemIMEI.InListID = Item.NewInListID;
                                    }
                                }
                            }
                            lqh.Umsdb.Pro_InOrderList.InsertOnSubmit(NewList);
                            #endregion


                            List<Model.Pro_StoreInfo> Store = (from b in lqh.Umsdb.Pro_StoreInfo
                                                               where b.InListID == Item.InListID && b.HallID == model.HallID && b.ProID == Item.OldProID
                                                               select b).ToList();
                            if (Store.Count() != 1)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "该库存已被删除，请联系管理员！" };
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
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "转换成功！" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "转换失败！" };
                    }
                }
            }
        }
        #endregion
    }
}
