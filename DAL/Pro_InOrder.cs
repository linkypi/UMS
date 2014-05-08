using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using Model;
using System.Data.Linq;

namespace DAL
{
    public class Pro_InOrder
    {
        private int MethodID;

        public Pro_InOrder()
        {
            this.MethodID = 0;
        }

        public Pro_InOrder(int MethodID)
        {
            this.MethodID = MethodID;

        }
        #region 无用代码
        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_InOrder model,List<string> s)
        //{
        //    //model.Pro_InOrderList[0].pro_st
        //    #region "验证用户操作仓库  商品的权限 "
        //    List<string> ValidHallIDS = new List<string>();
        //    //有权限的商品
        //    List<string> ValidProIDS = new List<string>();

        //    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MenthodID, ValidHallIDS, ValidProIDS);

        //    if (ret.ReturnValue != true)
        //    { return ret; }
        //    //有仓库限制，而且仓库不在权限范围内
        //    if (ValidHallIDS.Count() > 0 && ValidHallIDS.Contains(model.Pro_HallID))
        //    {
        //        return new Model.WebReturn() { ReturnValue = false, Message = model.Pro_HallID + "仓库无权操作" };
        //    }
        //    #endregion

        //    //验证是否可入库
        //    //生成单号 存储过程OrderMacker
        //    //插入表头
        //    //生成批次号 存储过程OrderMacker
        //    //插入明细
        //    //插入串号明细
        //    //插入库存
        //    //插入串码表
        //    //
        //    //返回
        //    if (model == null) return new Model.WebReturn();
        //    string Msg = "";
        //    using (LinQSqlHelper lqh = new LinQSqlHelper())
        //    {
        //        using (TransactionScope ts = new TransactionScope())
        //        {
        //            try
        //            {                    
        //                //生成单号 存储过程OrderMacker
        //                //插入表头
        //                string msg = null;
        //                lqh.Umsdb.OrderMacker(1, "RK", "RK", ref msg);
        //                model.InOrderID=msg;
        //                foreach (var i in model.Pro_InOrderList)
        //                {
        //                    string  message=null;
        //                    i.InOrderID = model.InOrderID; 
        //                    lqh.Umsdb.OrderMacker(1, "RKL", "RKL", ref message);
        //                    i.InListID = message;
        //                //验证是否可入库
        //                    var query_limits = from b in lqh.Umsdb.Pro_HallInfo
        //                                       where  b.HallID==model.Pro_HallID&&
        //                                       b.CanIn != false && b.CanIn != null
        //                                       select b;
        //                    if (query_limits== null|| query_limits.Count()==0)
        //                    {
        //                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "入库失败，该仓库无入库权限" };
        //                    }
        //                    var query_power = from b in lqh.Umsdb.Sys_Role_HallInfo
        //                                      where b.RoleID == user.RoleID&&b.HallID==model.Pro_HallID
        //                                      select b;
        //                    if (query_power== null || query_power.Count() == 0)
        //                    {
        //                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "入库失败，您无此仓库的操作权限" };
        //                    }
        //                    var query = (from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
        //                                    where s.Contains(b.IMEI)
        //                                select b).ToList();
        //                    if (query != null && query.Count() > 0)
        //                    {
        //                        return new Model.WebReturn() { Obj = query,ReturnValue=false,Message="入库失败，请检查串码"};
        //                    }

        //                }
        //                //插入串码明细                    
        //                lqh.Umsdb.Pro_InOrder.InsertOnSubmit(model);
        //                lqh.Umsdb.SubmitChanges();
        //                ts.Complete();
        //                return new Model.WebReturn() { Obj = null, ReturnValue=true, Message = "入库成功" };
        //            }
        //            catch (Exception ex)
        //            {
        //                return new Model.WebReturn() { Obj = false, ReturnValue=false};
        //                throw ex;
        //            }

        //        }

        //    }

        //}
        #endregion

        #region  入库
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_InOrder model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            if (model == null || model.Pro_InOrderList == null || model.Pro_InOrderList.Count() == 0)
                return new Model.WebReturn() { ReturnValue = false, Message = "实体为空" };
            //if(model.InDate==null 

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        DataLoadOptions dataload = new DataLoadOptions();

                        //dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                        lqh.Umsdb.LoadOptions = dataload;


                        #region  验证用户操作仓库

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (!ValidHallIDS.Contains(model.Pro_HallID))
                        {

                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作 " + model.Pro_HallID };
                        }

                        //验证商品权限
                        //if (ValidProIDS.Count > 0)
                        //{
                        //    List<string> classids = new List<string>();
                        //    foreach (var item in model.Pro_InOrderList)
                        //    {
                        //        var queryc = from p in lqh.Umsdb.Pro_ProInfo
                        //                     where p.ProID == item.ProID.ToString()
                        //                     select p;
                        //        classids.Add(queryc.First().Pro_ClassID.ToString());
                        //    }
                        //    foreach (var child in classids)
                        //    {
                        //        if (!ValidProIDS.Contains(child))
                        //        {
                        //            return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                        //        }
                        //    }
                        //}
                        #endregion

                        string errmsg = "";
                        string msg = "";
                        List<string> IMEIList = new List<string>();
                        List<string> ProIDS = new List<string>();
                        List<Model.Pro_InOrderIMEI> inorderImeiList = new List<Pro_InOrderIMEI>();
                        List<Model.Pro_IMEI> pro_imei_list = new List<Model.Pro_IMEI>();
                        List<Model.Pro_StoreInfo> pro_store_list = new List<Pro_StoreInfo>();
                        //生成单号 存储过程OrderMacker
                        //插入表头
                        #region 验证仓库是否可以入库
                        var query_limits = from b in lqh.Umsdb.Pro_HallInfo
                                           where b.HallID == model.Pro_HallID
                                           //b.CanIn == true
                                           select b;
                        if (query_limits.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "入库失败，仓库不存在" + model.Pro_HallID };
                        }
                        if (query_limits.First().CanIn != true)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "入库失败，仓库不能入库" + model.Pro_HallID };
                        }


                        #endregion

                        #region 验证用户
                        var u = from b in lqh.Umsdb.Sys_UserInfo
                                where b.UserID == model.UserID
                                select b;
                        if (u.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "用户不存在" };
                        }
                        #endregion

                        #region 表头
                        lqh.Umsdb.OrderMacker(1, "RK", "RK", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成单号出错" };
                        }
                        model.InOrderID = msg;
                        #endregion
                        #region 明细 InListID
                        //if (model.Pro_InOrderList == null || model.Pro_InOrderList.Count() == 0)
                        //{
                        //    return new WebReturn() { ReturnValue = false, Message = "单据的商品明细为空" };
                        //}
                        int count = model.Pro_InOrderList.Count();
                        ProIDS = (from b in model.Pro_InOrderList
                                  select b.ProID).ToList();
                        msg = "";
                        lqh.Umsdb.OrderMacker(count, "RKL", "RKL", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成明细单号出错" };
                        }
                        string[] InListIDStr = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (InListIDStr.Count() != model.Pro_InOrderList.Count())
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成明细单号数量与明细数量不一致" };
                        }
                        #endregion

                        #region 获取机型信息
                        var Pro_list = (from b in lqh.Umsdb.Pro_ProInfo
                                        where ProIDS.Contains(b.ProID)
                                        select b).ToList();
                        #endregion

                        #region 左连接型号
                        var InList_join_Pro = (from b in model.Pro_InOrderList
                                               join c in Pro_list
                                               on b.ProID equals c.ProID
                                               into temp1
                                               from c1 in temp1.DefaultIfEmpty()
                                               select new { Pro_InOrderList = b,
                                                   RetailPrice= b.RetailPrice,
                                                   Pro_ProInfo = c1 }).ToList();
                        #endregion

                        #region 获取机型 串码 分配批次号
                        #region 构造需要插入的实体
                        Model.Pro_InOrder inorder = new Model.Pro_InOrder()
                        {
                            InOrderID = model.InOrderID,
                            InDate = DateTime.Now,
                            Note = model.Note,
                            OldID = model.OldID,
                            Pro_HallID = model.Pro_HallID,
                            SysDate = DateTime.Now,
                            UserID = model.UserID,
                            
                        };

                        #endregion
                        


                        for (int k = 0; k < InList_join_Pro.Count(); k++)
                        {
                            
                            var m = InList_join_Pro[k];
                            if (m.Pro_InOrderList == null)
                            {
                                NoError = false;
                                m.Pro_InOrderList.Note = "入库明细错误";
                                errmsg = m.Pro_ProInfo.ProName + " 入库明细错误";
                                break; 
                            }
                            if (m.Pro_InOrderList.Price <0 )
                            {
                                NoError = false;
                                m.Pro_InOrderList.Note = "成本不能小于0";
                                errmsg = m.Pro_ProInfo.ProName + " 成本不能小于0";
                                break;
                            }
                            Model.WebReturn webr = ValidProInfo.CheckProInfo(m.Pro_ProInfo, m.Pro_InOrderList.ProCount);
                            if (webr.ReturnValue != true)
                            {
                                NoError = false;
                                m.Pro_InOrderList.Note = webr.Message;
                                break;
                            }
                            if (!ValidProIDS.Contains(m.Pro_ProInfo.Pro_ClassID+""))
                            {
                                NoError = false;
                                m.Pro_InOrderList.Note = "类别无权限"+m.Pro_ProInfo.Pro_ClassID;
                                break;
                            }
                            
                            #region 构造新的入库明细
                            
                            //m.Pro_InOrderList.InListID = InListIDStr[k];
                            //m.Pro_InOrderList.InitInListID = InListIDStr[k];
                            Model.Pro_InOrderList inorderlist_Insert=new Pro_InOrderList(){
                            InitInListID=InListIDStr[k],
                            InListID=InListIDStr[k],
                            Note=m.Pro_InOrderList.Note,
                            ProCount=m.Pro_InOrderList.ProCount,
                            Price=m.Pro_InOrderList.Price,
                            ProID=m.Pro_InOrderList.ProID,
                            InOrderID=inorder.InOrderID,
                            RetailPrice = m.RetailPrice
                            };
                            inorder.Pro_InOrderList.Add(inorderlist_Insert);
                            #endregion

                            #region 验证串码有效性


                            if (m.Pro_InOrderList.Pro_InOrderIMEI == null ||
                                m.Pro_InOrderList.Pro_InOrderIMEI.Count() == 0)
                            {
                                if (m.Pro_ProInfo.NeedIMEI == true)//有串码的机型
                                {
                                    NoError = false;
                                    errmsg = m.Pro_ProInfo.ProName + " 必须提供串码";
                                    m.Pro_InOrderList.Note = "必须提供串码";
                                    break;
                                }
                            }
                            else
                            {
                                if (m.Pro_InOrderList.ProCount != m.Pro_InOrderList.Pro_InOrderIMEI.Count())
                                {
                                    NoError = false;
                                    errmsg = m.Pro_ProInfo.ProName + " 数量与串码数量不一致";
                                    m.Pro_InOrderList.Note = "数量与串码数量不一致";
                                    break;
                                }
                                if (m.Pro_ProInfo.NeedIMEI != true)
                                {
                                    NoError = false;
                                    errmsg = m.Pro_ProInfo.ProName + " 属于无串码商品";
                                    m.Pro_InOrderList.Note = "属于无串码商品";
                                    break;
                                }
                                foreach (var mm in m.Pro_InOrderList.Pro_InOrderIMEI)
                                {
                                    if (string.IsNullOrEmpty(mm.IMEI)
                                        || IMEIList.Contains(mm.IMEI))
                                    {
                                        NoError = false;
                                        errmsg = m.Pro_ProInfo.ProName + " 串码重复或者串码为空";
                                        m.Pro_InOrderList.Note = "串码重复或者串码为空";
                                        break;
                                    }
                                    IMEIList.Add(mm.IMEI);
                                    #region 构造插入的新串码
                                    pro_imei_list.Add(
                                        new Model.Pro_IMEI()
                                        {
                                            IMEI = mm.IMEI,
                                            InListID = inorderlist_Insert.InListID,
                                            HallID = model.Pro_HallID,
                                            ProID = m.Pro_InOrderList.ProID,
                                            AreaAgeDelta = new DateTime(2000,1,1)

                                        }
                                        );
                                    #endregion
                                    
                                    #region 构造新的串码明细
                                    Model.Pro_InOrderIMEI inimei = new Model.Pro_InOrderIMEI()
                                    {
                                        IMEI = mm.IMEI,
                                        Note = mm.Note,
                                        InListID = inorderlist_Insert.InListID
                                    };

                                    #endregion
                                  inorderImeiList.Add(mm);
                                  inorderlist_Insert.Pro_InOrderIMEI.Add(inimei);
                                    
                                }

                            }

                            #endregion

                            inorderlist_Insert.Pro_StoreInfo.Add(
                                    new Model.Pro_StoreInfo() { ProCount = m.Pro_InOrderList.ProCount, HallID = model.Pro_HallID, ProID = m.Pro_InOrderList.ProID }
                                    );
                        }
                        #endregion
                        if (!NoError)
                        {
                            return new WebReturn() { ReturnValue = false, Message = errmsg, Obj = model };
                        }

                        #region 验证串码
                        if (IMEIList.Count() > 0)
                        {
                            //string s = "/"+string.Join("/", IMEIList)+"/";
                            if (IMEIList.Count > 2000)
                            {
                                int loopCount = IMEIList.Count / 2000 + 1;
                                for (int i = 0; i < loopCount; i++)
                                {
                                    var list = IMEIList.Skip(i * 2000).Take(2000).ToList();
                                    var Validimeis = (from b in lqh.Umsdb.Pro_IMEI
                                                      where list.Contains(b.IMEI)
                                                      select b).ToList();
                                    if (Validimeis.Count() > 0)
                                    {
                                        var Pro_IMEI_JOIN_Validimei = from b in inorderImeiList
                                                                      join c in Validimeis
                                                                      on b.IMEI equals c.IMEI
                                                                      into temp1
                                                                      from c2 in temp1.DefaultIfEmpty()
                                                                      select new
                                                                      {
                                                                          b,
                                                                          c2
                                                                      };

                                        foreach (var InOrderListItem in Pro_IMEI_JOIN_Validimei)
                                        {

                                            if (InOrderListItem.c2 != null)
                                            {
                                                InOrderListItem.b.Note = "串码已存在";
                                            }

                                        }
                                        return new Model.WebReturn() { Obj = model, ReturnValue = false, Message = "部分串码已存在" };
                                    }

                                    List<Model.Pro_IMEI> imeilist = pro_imei_list.Skip(i * 2000).Take(2000).ToList();
                                  
                                    lqh.Umsdb.Pro_IMEI.InsertAllOnSubmit(imeilist);
                                }
                            }
                            else
                            {
                                var Validimei = (from b in lqh.Umsdb.Pro_IMEI
                                                where IMEIList.Contains(b.IMEI)
                                                select b).ToList();

                                if (Validimei.Count() > 0)
                                {
                                    var Pro_IMEI_JOIN_Validimei = from b in inorderImeiList
                                                                  join c in Validimei
                                                                  on b.IMEI equals c.IMEI
                                                                  into temp1
                                                                  from c2 in temp1.DefaultIfEmpty()
                                                                  select new
                                                                  {
                                                                      b,
                                                                      c2
                                                                  };

                                    foreach (var InOrderListItem in Pro_IMEI_JOIN_Validimei)
                                    {

                                        if (InOrderListItem.c2 != null)
                                        {
                                            InOrderListItem.b.Note = "串码已存在";
                                        }

                                    }
                                    return new Model.WebReturn() { Obj = model, ReturnValue = false, Message = "部分串码已存在" };
                                }

                                else
                                {
                                    lqh.Umsdb.Pro_IMEI.InsertAllOnSubmit(pro_imei_list);
                                }
                            }
                        }
                        #endregion

                        //插入串码明细    
                        //model.SysDate = DateTime.Now;
                        //model.InDate = model.InDate == null ? DateTime.Now : model.InDate;

                        lqh.Umsdb.Pro_InOrder.InsertOnSubmit(inorder);
                        lqh.Umsdb.SubmitChanges();


                        var outlist = from a in lqh.Umsdb.Report_InlistInfoWithIMEI
                                      where a.入库单号 == inorder.InOrderID
                                      select a;
                        List<Model.Report_InlistInfoWithIMEI> olist = new List<Model.Report_InlistInfoWithIMEI>();
                        if (outlist.Count() > 0)
                        {
                            olist = outlist.ToList();
                        }

                        ts.Complete();
                        return new Model.WebReturn()
                        {
                            ReturnValue = true,
                            Message = "入库成功",
                            Obj = model.InOrderID,
                            ArrList = new System.Collections.ArrayList() { olist }
                        };


                    }
                    catch (Exception ex)
                    {
                        
                        return new Model.WebReturn() { ReturnValue = false, Message = "系统错误" + ex.Message };
                    }
                }
            }

        }
        #endregion

        #region   售后维修入库，直接关联价格  Pro_SellTypeProduct

        /// <summary>
        /// 384
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn AddIn(Model.Sys_UserInfo user, Model.Pro_InOrder model)
        {
            Model.WebReturn r = null;
            bool NoError = true;
            if (model == null || model.Pro_InOrderList == null || model.Pro_InOrderList.Count() == 0)
                return new Model.WebReturn() { ReturnValue = false, Message = "实体为空" };
            //if(model.InDate==null 

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        DataLoadOptions dataload = new DataLoadOptions();

                        //dataload.LoadWith<Model.VIP_VIPInfo>(c => c.VIP_OffTicket);
                        lqh.Umsdb.LoadOptions = dataload;
                     


                        #region  验证用户操作仓库

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (!ValidHallIDS.Contains(model.Pro_HallID))
                        {

                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作 " + model.Pro_HallID };
                        }

                        #endregion

                        string errmsg = "";
                        string msg = "";
                        List<string> IMEIList = new List<string>();
                        List<string> ProIDS = new List<string>();
                        List<Model.Pro_InOrderIMEI> inorderImeiList = new List<Pro_InOrderIMEI>();
                        List<Model.Pro_IMEI> pro_imei_list = new List<Model.Pro_IMEI>();
                        List<Model.Pro_StoreInfo> pro_store_list = new List<Pro_StoreInfo>();
                        //生成单号 存储过程OrderMacker
                        //插入表头
                        #region 验证仓库是否可以入库
                        var query_limits = from b in lqh.Umsdb.Pro_HallInfo
                                           where b.HallID == model.Pro_HallID
                                           //b.CanIn == true
                                           select b;
                        if (query_limits.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "入库失败，仓库不存在" + model.Pro_HallID };
                        }
                        if (query_limits.First().CanIn != true)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "入库失败，仓库不能入库" + model.Pro_HallID };
                        }


                        #endregion

                        #region 验证用户
                        var u = from b in lqh.Umsdb.Sys_UserInfo
                                where b.UserID == model.UserID
                                select b;
                        if (u.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "用户不存在" };
                        }
                        #endregion

                        #region 表头
                        lqh.Umsdb.OrderMacker(1, "RK", "RK", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成单号出错" };
                        }
                        model.InOrderID = msg;
                        #endregion
                        #region 明细 InListID
                        //if (model.Pro_InOrderList == null || model.Pro_InOrderList.Count() == 0)
                        //{
                        //    return new WebReturn() { ReturnValue = false, Message = "单据的商品明细为空" };
                        //}
                        int count = model.Pro_InOrderList.Count();
                        ProIDS = (from b in model.Pro_InOrderList
                                  select b.ProID).ToList();
                        msg = "";
                        lqh.Umsdb.OrderMacker(count, "RKL", "RKL", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成明细单号出错" };
                        }
                        string[] InListIDStr = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (InListIDStr.Count() != model.Pro_InOrderList.Count())
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成明细单号数量与明细数量不一致" };
                        }
                        #endregion

                        #region 获取机型信息
                        var Pro_list = (from b in lqh.Umsdb.Pro_ProInfo
                                        where ProIDS.Contains(b.ProID)
                                        select b).ToList();
                        #endregion

                        #region 左连接型号
                        var InList_join_Pro = (from b in model.Pro_InOrderList
                                               join c in Pro_list
                                               on b.ProID equals c.ProID
                                               into temp1
                                               from c1 in temp1.DefaultIfEmpty()
                                               select new
                                               {
                                                   Pro_InOrderList = b,
                                                   RetailPrice = b.RetailPrice,
                                                   Pro_ProInfo = c1
                                               }).ToList();
                        #endregion

                        #region 获取机型 串码 分配批次号
                        #region 构造需要插入的实体
                        Model.Pro_InOrder inorder = new Model.Pro_InOrder()
                        {
                            InOrderID = model.InOrderID,
                            InDate = DateTime.Now,
                            Note = model.Note,
                            OldID = model.OldID,
                            Pro_HallID = model.Pro_HallID,
                            SysDate = DateTime.Now,
                            UserID = model.UserID,

                        };

                        #endregion


                        List<Model.Pro_SellTypeProduct> pros = new List<Model.Pro_SellTypeProduct>();
                        for (int k = 0; k < InList_join_Pro.Count(); k++)
                        {

                            var m = InList_join_Pro[k];
                            if (m.Pro_InOrderList == null)
                            {
                                NoError = false;
                                m.Pro_InOrderList.Note = "入库明细错误";
                                errmsg = m.Pro_ProInfo.ProName + " 入库明细错误";
                                break;
                            }
                            if (m.Pro_InOrderList.Price < 0)
                            {
                                NoError = false;
                                m.Pro_InOrderList.Note = "成本不能小于0";
                                errmsg = m.Pro_ProInfo.ProName + " 成本不能小于0";
                                break;
                            }
                            Model.WebReturn webr = ValidProInfo.CheckProInfo(m.Pro_ProInfo, m.Pro_InOrderList.ProCount);
                            if (webr.ReturnValue != true)
                            {
                                NoError = false;
                                m.Pro_InOrderList.Note = webr.Message;
                                break;
                            }
                            if (!ValidProIDS.Contains(m.Pro_ProInfo.Pro_ClassID + ""))
                            {
                                NoError = false;
                                m.Pro_InOrderList.Note = "类别无权限" + m.Pro_ProInfo.Pro_ClassID;
                                break;
                            }

                            #region 构造新的入库明细

                            //m.Pro_InOrderList.InListID = InListIDStr[k];
                            //m.Pro_InOrderList.InitInListID = InListIDStr[k];
                            Model.Pro_InOrderList inorderlist_Insert = new Pro_InOrderList()
                            {
                                InitInListID = InListIDStr[k],
                                InListID = InListIDStr[k],
                                Note = m.Pro_InOrderList.Note,
                                ProCount = m.Pro_InOrderList.ProCount,
                                Price = m.Pro_InOrderList.Price,
                                ProID = m.Pro_InOrderList.ProID,
                                InOrderID = inorder.InOrderID,
                                RetailPrice = m.RetailPrice
                            };

                            var existPro = from a in lqh.Umsdb.Pro_SellTypeProduct
                                           where a.ProID == m.Pro_InOrderList.ProID
                                           && a.SellType == 1
                                           select a;
                            if (existPro.Count() == 0)
                            {
                                Model.Pro_SellTypeProduct p = new Model.Pro_SellTypeProduct();
                                p.ProID = m.Pro_InOrderList.ProID;
                                p.ProCost = m.Pro_InOrderList.Price;
                                p.Price = m.RetailPrice;
                                p.SellType = 1;
                                pros.Add(p);
                            }
                       
                            inorder.Pro_InOrderList.Add(inorderlist_Insert);
                            #endregion

                            #region 验证串码有效性


                            if (m.Pro_InOrderList.Pro_InOrderIMEI == null ||
                                m.Pro_InOrderList.Pro_InOrderIMEI.Count() == 0)
                            {
                                if (m.Pro_ProInfo.NeedIMEI == true)//有串码的机型
                                {
                                    NoError = false;
                                    errmsg = m.Pro_ProInfo.ProName + " 必须提供串码";
                                    m.Pro_InOrderList.Note = "必须提供串码";
                                    break;
                                }
                            }
                            else
                            {
                                if (m.Pro_InOrderList.ProCount != m.Pro_InOrderList.Pro_InOrderIMEI.Count())
                                {
                                    NoError = false;
                                    errmsg = m.Pro_ProInfo.ProName + " 数量与串码数量不一致";
                                    m.Pro_InOrderList.Note = "数量与串码数量不一致";
                                    break;
                                }
                                if (m.Pro_ProInfo.NeedIMEI != true)
                                {
                                    NoError = false;
                                    errmsg = m.Pro_ProInfo.ProName + " 属于无串码商品";
                                    m.Pro_InOrderList.Note = "属于无串码商品";
                                    break;
                                }
                                foreach (var mm in m.Pro_InOrderList.Pro_InOrderIMEI)
                                {
                                    if (string.IsNullOrEmpty(mm.IMEI)
                                        || IMEIList.Contains(mm.IMEI))
                                    {
                                        NoError = false;
                                        errmsg = m.Pro_ProInfo.ProName + " 串码重复或者串码为空";
                                        m.Pro_InOrderList.Note = "串码重复或者串码为空";
                                        break;
                                    }
                                    IMEIList.Add(mm.IMEI);
                                    #region 构造插入的新串码
                                    pro_imei_list.Add(
                                        new Model.Pro_IMEI()
                                        {
                                            IMEI = mm.IMEI,
                                            InListID = inorderlist_Insert.InListID,
                                            HallID = model.Pro_HallID,
                                            ProID = m.Pro_InOrderList.ProID,
                                            AreaAgeDelta = new DateTime(2000, 1, 1)

                                        }
                                        );
                                    #endregion

                                    #region 构造新的串码明细
                                    Model.Pro_InOrderIMEI inimei = new Model.Pro_InOrderIMEI()
                                    {
                                        IMEI = mm.IMEI,
                                        Note = mm.Note,
                                        InListID = inorderlist_Insert.InListID
                                    };

                                    #endregion
                                    inorderImeiList.Add(mm);
                                    inorderlist_Insert.Pro_InOrderIMEI.Add(inimei);

                                }

                            }

                            #endregion

                            inorderlist_Insert.Pro_StoreInfo.Add(
                                    new Model.Pro_StoreInfo() { ProCount = m.Pro_InOrderList.ProCount, HallID = model.Pro_HallID, ProID = m.Pro_InOrderList.ProID }
                                    );
                        }
                        #endregion
                        if (!NoError)
                        {
                            return new WebReturn() { ReturnValue = false, Message = errmsg, Obj = model };
                        }

                        #region 验证串码
                        if (IMEIList.Count() > 0)
                        {
                            //string s = "/"+string.Join("/", IMEIList)+"/";
                            if (IMEIList.Count > 2000)
                            {
                                int loopCount = IMEIList.Count / 2000 + 1;
                                for (int i = 0; i < loopCount; i++)
                                {
                                    var list = IMEIList.Skip(i * 2000).Take(2000).ToList();
                                    var Validimeis = (from b in lqh.Umsdb.Pro_IMEI
                                                      where list.Contains(b.IMEI)
                                                      select b).ToList();
                                    if (Validimeis.Count() > 0)
                                    {
                                        var Pro_IMEI_JOIN_Validimei = from b in inorderImeiList
                                                                      join c in Validimeis
                                                                      on b.IMEI equals c.IMEI
                                                                      into temp1
                                                                      from c2 in temp1.DefaultIfEmpty()
                                                                      select new
                                                                      {
                                                                          b,
                                                                          c2
                                                                      };

                                        foreach (var InOrderListItem in Pro_IMEI_JOIN_Validimei)
                                        {

                                            if (InOrderListItem.c2 != null)
                                            {
                                                InOrderListItem.b.Note = "串码已存在";
                                            }

                                        }
                                        return new Model.WebReturn() { Obj = model, ReturnValue = false, Message = "部分串码已存在" };
                                    }

                                    List<Model.Pro_IMEI> imeilist = pro_imei_list.Skip(i * 2000).Take(2000).ToList();

                                    lqh.Umsdb.Pro_IMEI.InsertAllOnSubmit(imeilist);
                                }
                            }
                            else
                            {
                                var Validimei = (from b in lqh.Umsdb.Pro_IMEI
                                                 where IMEIList.Contains(b.IMEI)
                                                 select b).ToList();

                                if (Validimei.Count() > 0)
                                {
                                    var Pro_IMEI_JOIN_Validimei = from b in inorderImeiList
                                                                  join c in Validimei
                                                                  on b.IMEI equals c.IMEI
                                                                  into temp1
                                                                  from c2 in temp1.DefaultIfEmpty()
                                                                  select new
                                                                  {
                                                                      b,
                                                                      c2
                                                                  };

                                    foreach (var InOrderListItem in Pro_IMEI_JOIN_Validimei)
                                    {

                                        if (InOrderListItem.c2 != null)
                                        {
                                            InOrderListItem.b.Note = "串码已存在";
                                        }

                                    }
                                    return new Model.WebReturn() { Obj = model, ReturnValue = false, Message = "部分串码已存在" };
                                }

                                else
                                {
                                    lqh.Umsdb.Pro_IMEI.InsertAllOnSubmit(pro_imei_list);
                                }
                            }
                        }
                        #endregion

                        //插入串码明细    
                        //model.SysDate = DateTime.Now;
                        //model.InDate = model.InDate == null ? DateTime.Now : model.InDate;
                        if (pros.Count > 0)
                        {
                            lqh.Umsdb.Pro_SellTypeProduct.InsertAllOnSubmit(pros);
                        }
                        lqh.Umsdb.Pro_InOrder.InsertOnSubmit(inorder);
                        lqh.Umsdb.SubmitChanges();

                        var outlist = from a in lqh.Umsdb.Report_InlistInfoWithIMEI
                                      where a.入库单号 == inorder.InOrderID
                                      select a;
                        List<Model.Report_InlistInfoWithIMEI> olist = new List<Model.Report_InlistInfoWithIMEI>();
                        if (outlist.Count() > 0)
                        {
                            olist = outlist.ToList();
                        }


                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, 
                            Message = "入库成功", Obj = model.InOrderID,
                              ArrList = new System.Collections.ArrayList() { olist }
                        };


                    }
                    catch (Exception ex)
                    {

                        return new Model.WebReturn() { ReturnValue = false, Message = "系统错误" + ex.Message };
                    }
                }
            }

        }

        #endregion  
    }
}
