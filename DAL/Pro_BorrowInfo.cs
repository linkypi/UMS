using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.Linq;

namespace DAL
{
    /// <summary>
    /// 借贷
    /// </summary>
    public class Pro_BorowInfo
    {
         private int MethodID;

        public Pro_BorowInfo()
        {
            this.MethodID = 0;
        }

        public Pro_BorowInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        /// <summary>
        /// 借贷
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="aduitNum"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_BorowInfo model)
        {
            if (model == null) return new Model.WebReturn();
            string Msg = "";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region "验证用户操作仓库  商品的权限 "
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count>0&&!ValidHallIDS.Contains(model.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + que.First().HallName  };
                        }

                        //验证操作商品权限
                        if (ValidProIDS.Count > 0)
                        {
                            List<string> classids = new List<string>();
                            foreach (var item in model.Pro_BorowListInfo)
                            {
                                var query = from p in lqh.Umsdb.Pro_ProInfo
                                            where p.ProID == item.ProID.ToString()
                                            select p;
                                classids.Add(query.First().Pro_ClassID.ToString());
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

                         #region  验证审批单

                        var queryAduit = from botro in lqh.Umsdb.Pro_BorowAduit
                                    where   botro.HallID==model.HallID && botro.AduitID == model.AduitID
                                    select botro;
                           
                              Model.Pro_BorowAduit bAduit = queryAduit.First();
                                if (bAduit.Used == true)  
                            {
                                    Msg = "审批单 " + bAduit.AduitID + " 已使用";
                                    return new Model.WebReturn { ReturnValue = false, Obj = null, Message = Msg };
                            }
                                if (bAduit.Aduited == false)
                            {
                                 Msg = "审批单 " + bAduit.AduitID + " 未审批";
                                 return new Model.WebReturn { ReturnValue = false, Obj = null, Message = Msg };
                            }
                          
                            bAduit.Used = false;
                            bAduit.UseDate = DateTime.Now;
                        #endregion

                            #region  检查库存是否充足

                            #region 当前库存
                            List<string> proIDs=(from b in model.Pro_BorowListInfo
                                                     select b.ProID).ToList();
                            //List<Model.Pro_ProInfo> pro_List=(from b in lqh.Umsdb.Pro_ProInfo
                            //                           where proIDs.Contains(b.ProID)
                            //                           select b).ToList();

                            var All_Store=(from b in lqh.Umsdb.Pro_StoreInfo 
                                               where proIDs.Contains(b.ProID) && b.HallID==model.HallID
                                               select b).ToList();

                            var BorrowList_Join_Store = from b in model.Pro_BorowListInfo
                                                        join c in All_Store
                                                        on new { b.ProID, b.InListID } equals new { c.ProID, c.InListID }  into g
                                                        from c1 in g.DefaultIfEmpty()
                                                        //join d in pro_List
                                                        //on b.ProID equals d.ProID into g2
                                                        //from d1 in g2.DefaultIfEmpty()

                                                        select new
                                                        {
                                                            b,
                                                            c1,
                                                            //d1
                                                        };
                                                      

                            #endregion

                            #region 验证库存
                            foreach (var a in BorrowList_Join_Store)
                            {
                                if (a.c1 == null || a.c1.ProCount < a.b.ProCount)
                                {
                                    return new WebReturn() { ReturnValue = false, Message = "保存失败，库存不足" + a.b.ProID };
                                }
                                if (a.c1.Pro_ProInfo == null)
                                {
                                    return new WebReturn (){ ReturnValue=false, Message="保存失败，商品不存在"+a.b.ProID };
                                }
                                if (a.b.ProCount < 0)
                                {
                                    return new WebReturn() { ReturnValue = false, Message = "保存失败，借贷数量不能为负数" + a.b.ProID };
                                }
                                if (a.c1.Pro_ProInfo.ISdecimals)
                                {
                                    if(a.b.ProCount!= Decimal.Truncate(Convert.ToDecimal(a.b.ProCount * 100)) / 100)
                                    {
                                        return new WebReturn (){ ReturnValue=false, Message="保存失败，请保留2位小数"+a.b.ProID };
                                    } 
                                } 
                                else {
                                    if (a.b.ProCount != Convert.ToInt32(a.b.ProCount))
                                    {
                                        return new WebReturn() { ReturnValue = false, Message = "保存失败，请保留正整数" + a.b.ProID };
                                    }
                                }
                                  
                            }

                            #endregion

                            #region  验证串码
                            foreach (var item in model.Pro_BorowListInfo)
                            {
                                var imei = from a in item.Pro_BorowOrderIMEI
                                           join b in lqh.Umsdb.Pro_IMEI
                                           on a.IMEI equals b.IMEI
                                           select b;
                                foreach (var child in imei)
                                {
                                    Model   .WebReturn retrs = Common.Utils.CheckIMEI(child);
                                    if (retrs.ReturnValue == false)
                                    {
                                        return retrs;
                                    }
                                    //if (child.OutID!=null || child.RepairID !=null || child.BorowID!=null||
                                    //    child.SellID!=null || child.VIPID!=null )
                                    //{
                                    //    return new Model.WebReturn() { ReturnValue=false, Message="串码 "+child.IMEI+" 存在其他操作,借贷失败！"};
                                    //}
                                }
                            }
                            
                            #endregion
                            //List<Model.Pro_BackListInfo> borowmodels = new List<Model.Pro_BackListInfo>();

                            //foreach (var item in model.Pro_BorowListInfo)
                            //{
                            //    bool finded = false;
                            //    foreach(var child in borowmodels)
                            //    {
                            //        if (child.ProID==item.ProID && child.InListID == item.InListID)
                            //        {
                            //            finded = true;
                            //            child.ProCount += item.ProCount;
                            //        }
                            //    }
                            //    if (!finded)
                            //    {
                            //        Model.Pro_BackListInfo b = new Model.Pro_BackListInfo();
                            //        b.InListID = item.InListID;
                            //        b.ProCount = item.ProCount;
                            //        b.ProID = item.ProID;
                            //        borowmodels.Add(b);
                            //    }
                            //}

                            //foreach (var item in borowmodels)
                            //{
                            //    var store_borow = from a in lqh.Umsdb.Pro_StoreInfo
                            //                      join b in lqh.Umsdb.Pro_ProInfo
                            //                      on a.ProID equals b.ProID
                            //                      where a.ProID == item.ProID && a.InListID == item.InListID
                            //                      select new
                            //                      {
                            //                          a.ProCount,
                            //                          b.ProName
                            //                      };
                            //    if (store_borow.Count()==0)
                            //    {
                            //        return new Model.WebReturn() { ReturnValue=false,Message="库存不足！"};
                            //    }
                            //    if (store_borow.First().ProCount< item.ProCount)
                            //    {
                            //        return new Model.WebReturn() { ReturnValue = false, Message ="商品"+store_borow.First().ProName+ "库存不足！" };
                            //    }
                            //}


                            #endregion 


                            //单据编号
                            string borowid = string.Empty;
                            lqh.Umsdb.OrderMacker(1, "JD", "JD", ref borowid);
                            if (borowid == "")
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "借贷单生成出错" };
                            }
                            model.BorowID = borowid;
                            model.UserID = user.UserID;
                            model.IsReturn = false;
                            model.IsDelete = false;
                           ////插入明细
                           lqh.Umsdb.Pro_BorowInfo.InsertOnSubmit(model);
                           lqh.Umsdb.SubmitChanges();

                       
                           lqh.Umsdb.BorowBusiness(model.ID);

                           #region 标记审批单已使用

                           var aduit = from a in lqh.Umsdb.Pro_BorowAduit
                                       where a.AduitID == model.AduitID
                                       select a;

                           aduit.First().Used = true;
                           aduit.First().UseDate = DateTime.Now;

                           #endregion 

                           lqh.Umsdb.SubmitChanges();
                           ts.Complete();
                           return new Model.WebReturn() {ReturnValue=true, Obj = true, Message = "借贷成功" };
                    }
                    catch (Exception ex)
                    {
                          return new Model.WebReturn() { ReturnValue = false ,Obj=null,Message=ex.Message};
                    }
                  }

                }

            }

        /// <summary>
        ///  拣货
        /// </summary>
        /// <param name="user"></param>
        /// <param name="unimeiModels">非串码</param>
        /// <param name="models">串码商品</param>
        /// <param name="proidlist"></param>
        /// <param name="hid"></param>
        /// <returns></returns>
        public Model.WebReturn CheckProduct(Model.Sys_UserInfo user, List<Model.BorowListModel> unimeiModels, List<Model.BorowListModel> models, List<BorowListModel> proidlist, string hid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope()) 
                {
                    try
                    {
                        #region 非串码商品拣货
                       
                        List<string> proidlist_unimei = new List<string> ();
                        foreach(var item in unimeiModels)
                        {
                            proidlist_unimei.Add(item.ProID);
                        }
                        var store = (from s in lqh.Umsdb.Pro_StoreInfo
                                    where s.HallID == hid && proidlist_unimei.Contains(s.ProID) && s.ProCount>0
                                    select s).ToList();
                        List<Model.BorowListModel> unmodels = new List<BorowListModel>();

                        //foreach (var item in unimeiModels)
                        //{
                        //    item.AduitCount = item.ProCount;
                        //}

                        foreach(var child in unimeiModels)
                        {
                            foreach (var item in store)
                            {
                                if (child.ProID == item.ProID)
                                {
                                    BorowListModel bl = new BorowListModel();
                                    bl.InListID = item.InListID;
                                    bl.ProID = item.ProID;
                                    bl.ProName = child.ProName;
                                    bl.ClassName = child.ClassName;
                                    bl.TypeName = child.TypeName;
                                    bl.Note = "成功";

                                    if (child.AduitCount <= item.ProCount)
                                    {
                                        bl.ProCount = child.AduitCount;
                                        child.AduitCount = 0;
                                        unmodels.Add(bl);
                                        break;
                                    }
                                    else
                                    {
                                        child.AduitCount -= item.ProCount;
                                        bl.ProCount = item.ProCount;
                                        unmodels.Add(bl);
                                        if (child.ProCount == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        bool flag = true;

                        foreach (var child in unimeiModels)
                        {
                            if (child.AduitCount == 0)
                            {
                                child.Note = "成功";
                            }
                            else
                            {
                                flag = false;
                                child.Note = "失败";
                            }
                        }

                        ArrayList arr = new ArrayList();
                        arr.Add(unimeiModels);
                        arr.Add(unmodels);
                        #endregion

                        #region  串码拣货

                        List<string> imeiList = new List<string>();
                        List<string> proids = new List<string>();

                        foreach (var item in models)
                        {
                            imeiList.Add(item.IMEI.ToUpper());
                        }
                        foreach (var child in proidlist)
                        {
                            proids.Add(child.ProID);
                        }
                        //var results = (from imei in lqh.Umsdb.Pro_IMEI 
                        //              where imei.HallID == hid && imei.OutID == null && imei.RepairID==null && 
                        //              imei.BorowID==null && proidlist.Contains(imei.ProID)// && imei.ReturnID ==null 
                        //              && imeiList.Contains(imei.IMEI) 
                        //               select imei).ToList();

                        foreach (var item in proidlist)
                        {
                               var results = (from imei in lqh.Umsdb.Pro_IMEI 
                                      where imei.HallID == hid && imei.OutID == null && imei.RepairID==null && 
                                      imei.BorowID==null && imei.ProID == item.ProID && imei.VIPID==null && imei.AssetID == null
                                      && imeiList.Contains(imei.IMEI)
                                       select imei).ToList();
                               if (results.Count()>0)
                               {
                                   item.AduitCount = results.Count;
                                   foreach (var child in results)
                                   {
                                       foreach(var x in models)
                                       {
                                         if (child.IMEI.ToUpper()==x.IMEI.ToUpper())
                                         {
                                             //item.AduitCount++;
                                             x.Note = "成功";
                                             x.IMEI = x.IMEI.ToUpper();
                                             x.Sucess = true;
                                             x.ProID = child.ProID;
                                             x.InListID = child.InListID;
                                          }
                                       }
                                   }
                               }
                        } 

                        bool Sucess = true;
                        List<BorowListModel> checkStoreModel = new List<BorowListModel>();
                        foreach (var x in models)
                        {
                            if (x.Note != "成功")
                            {
                                x.Note = "失败";
                                x.Sucess = false;
                                Sucess = false;
                            }
                        }

                        //判断借贷的串码是否都已拣货成功
                        foreach (var x in models)
                        {
                            if (x.Note != "成功")
                            {
                                continue;
                            }
                            if (checkStoreModel.Count == 0)
                            {
                                BorowListModel bl = new BorowListModel();
                                bl.InListID = x.InListID;
                                bl.ProID = x.ProID;
                                bl.AduitCount = 1;
                                checkStoreModel.Add(bl);
                                continue;
                            }
                            bool finded = false;
                            foreach (var item in checkStoreModel)
                            {
                                if (item.InListID==x.InListID && item.ProID==x.ProID)
                                {
                                    item.AduitCount++;
                                    finded = true;
                                    break;
                                }
                            }
                            if (!finded)
                            {
                                BorowListModel bl = new BorowListModel();
                                bl.InListID = x.InListID;
                                bl.ProID = x.ProID;
                                bl.AduitCount = 1;
                                checkStoreModel.Add(bl);
                            }
                        }

                       #region 判断库存是否充足

                        bool shortage = false; 
                        foreach (var item in checkStoreModel)
                        {
                            var storeinfo = from s in lqh.Umsdb.Pro_StoreInfo
                                            where s.HallID == hid && s.ProID == item.ProID
                                            && s.InListID == item.InListID
                                            select s;
                            if ((storeinfo.Count()==0))
                            {
                                shortage = true;
                                //标记串码库存不足
                                foreach (var child in models)
                                {
                                    if (item.ProID == child.ProID && item.InListID==child.InListID )
                                    {
                                        child.Note = "失败";
                                    }
                                }
                              
                            }
                            else if (storeinfo.First().ProCount < item.AduitCount)
                            {
                                int count =(int) storeinfo.First().ProCount;
                                shortage = true;
                                //标记串码库存不足
                                foreach (var child in models)
                                {
                                    if (count == 0)
                                    {
                                        break;
                                    }
                                    if (item.ProID == child.ProID && item.InListID == child.InListID)
                                    {
                                        child.Note = "失败";
                                    }
                                    count--;
                                }
                            }
                        }

                        if (shortage)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "拣货失败，库存不足", Obj = models, ArrList = arr };
                        }
                       #endregion

                        //判断借贷串码数量是否超过指定数量
                        foreach(var item in proidlist)
                        {
                            if (item.AduitCount > item.ProCount)
                            {
                                var p = from pro in lqh.Umsdb.Pro_ProInfo
                                        where pro.ProID == item.ProID
                                        select pro;
                               
                                return new WebReturn() { ReturnValue = false, Message = "商品" + p.First().ProName + "的串码数量已超过借贷的数量", Obj = models, ArrList = arr };
                            }
                        }
                      
                       
                        //bool finded = false ;
                        //foreach (var child in models)
                        //{
                        //    foreach (var item in results)
                        //    {
                        //        if (child.IMEI == item.IMEI)
                        //        {
                        //            finded = true;
                        //            child.Note = "成功";
                        //            child.ProID = item.ProID;
                        //            child.InListID = item.InListID;
                        //            break;
                        //        }
                        //    }
                        //    if (!finded)
                        //    {
                        //        Sucess = false;
                        //        child.Note = "失败";
                        //    }
                        //    finded = false;
                        //}

                        #endregion

                        bool retFlag = false ;
                        if (unimeiModels.Count == 0 && models.Count != 0)
                        {
                            retFlag = Sucess;
                        }
                        if (unimeiModels.Count != 0 && models.Count == 0)
                        {
                            retFlag = flag;
                        }
                        if (unimeiModels.Count != 0 && models.Count != 0)
                        {
                            retFlag =  (Sucess&&flag);
                        }
                        
                        return new Model.WebReturn() { ReturnValue = retFlag, Message = retFlag?"":"拣货失败", Obj = models, ArrList = arr };

                   
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false,Message = ex.Message };
                    }
                    
                }
            }
        }

        /// <summary>
        /// 获取指定用户的所有借贷信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="hid"></param>
        /// <returns></returns>
        public Model.WebReturn GetBorowList(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    int? total=0;
                    List<Model.Report_Borow> results = lqh.Umsdb.Report_Borow.ToList();

                    if (results.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false};
                    }

                    return new Model.WebReturn() { Obj = results, ReturnValue = true };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false };
                }
            }

        }

        /// <summary>
        /// 查询归还信息 71
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn SearchBack(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
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

                    var aduit_query = from b in lqh.Umsdb.View_BorowReturnInfo
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "IsReturn":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.IsReturn == mm.ParamValues
                                              select b;
                           
                                break;
                            case "BorowID":
                                Model.ReportSqlParams_String bid = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.BorowID == bid.ParamValues
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
                                                      where b.SysDate >= mm5.ParamValues && b.SysDate <= mm6.ParamValues
                                                      select b;
                                    }
                                }
                                break;
                       
                            case "UserID":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  join u in lqh.Umsdb.Sys_UserInfo on b.UserID equals u.UserID
                                                  where u.UserName.Contains(para.ParamValues)
                                                  select b;
                                }
                                break;
                            case "BorowType":
                                Model.ReportSqlParams_String bt = (Model.ReportSqlParams_String)item;
                                if (bt.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.BorrowType.Contains(bt.ParamValues)
                                                  select b;
                                }
                                break;
                            case "HallName":
                                Model.ReportSqlParams_String para1 = (Model.ReportSqlParams_String)item;
                                if (para1.ParamValues != null)
                                {
                                    var h = from hall in lqh.Umsdb.Pro_HallInfo
                                            where hall.HallName == para1.ParamValues
                                            select hall;

                                    aduit_query = from b in aduit_query
                                                  where b.HallID.Contains(h.First().HallID)
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

                        List<Model.View_BorowReturnInfo> models = results.ToList();
                     
                        pageParam.Obj = models;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                    
                                      select a;
                        List<Model.View_BorowReturnInfo> models = results.ToList();
               

                        pageParam.Obj = models;
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
        /// 获取借贷详情
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bid"></param>
        /// <returns></returns>
        public Model.WebReturn GetBorowListByID(Model.Sys_UserInfo user, string bid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    int id = int.Parse(bid);
                    List<Model.BorowListModel> models = new List<Model.BorowListModel>();
             
                    
                    var list = from blist in lqh.Umsdb.View_BorowReturnDetail
                               // join p in lqh.Umsdb.Pro_ProInfo on blist.ProID equals p.ProID
                               // join t in lqh.Umsdb.Pro_TypeInfo on p.Pro_TypeID equals t.TypeID
                               // join c in lqh.Umsdb.Pro_ClassInfo on p.Pro_ClassID equals c.ClassID
                               //join r in lqh.Umsdb.Pro_ReturnListInfo on blist.BorowListID equals r.BorowListID
                                where blist.BorowID == id
                                select new
                                {
                                    blist.BID,
                                    blist.InListID,
                                    blist.BorowListID,
                                    blist.ProID,
                                    blist.NeedIMEI,
                                    blist.ProName,
                                    blist.TypeName,
                                    blist.ClassName,
                                    blist.ProCount,
                                    blist.IsReturn,
                                    blist.ProFormat,
                                    blist.RetCount,
                                    blist.UnRetCount
                                };

                        foreach (var item in list)
                        {
                            Model.BorowListModel model = new BorowListModel();

                            model.BID = item.BID;
                            model.ProID = item.ProID;
                            model.NeedIMEI =Convert.ToBoolean( item.NeedIMEI);
                            model.ProName = item.ProName;
                            model.ProCount = Convert.ToDecimal(item.ProCount);
                            model.ClassName = item.ClassName;
                            model.TypeName = item.TypeName;
                            model.InListID = item.InListID;
                            model.BorowListID = item.BorowListID;
                            model.ProFormat = Convert.ToString( item.ProFormat);
                            model.IsReturn = item.IsReturn;
                       
                            var imei = from m in lqh.Umsdb.Pro_BorowOrderIMEI
                                        //join mi in lqh.Umsdb.Pro_IMEI 
                                        //  on m.IMEI equals mi.IMEI
                                        where m.BorowListID == item.BorowListID
                                        //&& ! Nullable<int>.Equals(mi.BorowID,null)
                                        select m;
                            IMEIModel bl = null;

                            int returnCount = 0;
                            if (imei.Count() != 0)
                            {
                                foreach (var child in imei)
                                {
                                    bl = new IMEIModel();
                                    bl.OldIMEI = child.IMEI;
                                    var query = from rem in lqh.Umsdb.Pro_ReturnOrderIMEI
                                                join rl in lqh.Umsdb.Pro_ReturnListInfo on rem.ReturnListID equals rl.ReturnListID
                                                join r in lqh.Umsdb.Pro_ReturnInfo on rl.ReturnID equals r.ID
                                                where rem.IMEI == child.IMEI && r.IsDelete == false
                                                && rl.BorowListID == model.BorowListID
                                                select rem;
                                    if (query.Count() > 0)
                                    {
                                        bl.Note = "Y";
                                        returnCount++;
                                    }
                                    else
                                    {
                                        bl.Note = "N";
                                    }

                                    if (model.IIMEIList == null)
                                    {
                                        model.IIMEIList = new List<IMEIModel>();
                                    }
                                    model.IIMEIList.Add(bl);
                                }
                                model.UnReturnCount = model.ProCount - returnCount;  
                                model.ReturnCount = returnCount;
                            }
                            else
                            {
                                model.UnReturnCount =(decimal) item.UnRetCount;
                                model.ReturnCount = model.ProCount - model.UnReturnCount;
                            }
                          // Convert.ToDecimal(item.ReturnCount);
                            //Convert.ToDecimal(item.UnReturnCount);
                            models.Add(model);
                        }
                        
                       return new Model.WebReturn() { Obj = models, ReturnValue = true };
                    
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false ,Message=ex.Message};
                }
            }

        }

        /// <summary>
        /// 查询借贷信息
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

                    if (pageParam == null || pageParam.PageIndex < 0 )
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var aduit_query = from b in lqh.Umsdb.View_BorowInfo
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "IsReturn":
                                Model.ReportSqlParams_String isreturn = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.IsReturn == isreturn.ParamValues
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
                                                      where b.SysDate >= mm5.ParamValues && b.SysDate <= mm6.ParamValues
                                                      select b;
                                    }
                                }
                                break;
                            case "Borrower":
                                Model.ReportSqlParams_String borower = (Model.ReportSqlParams_String)item;
                                if (borower.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where borower.ParamValues.Contains(b.Borrower)
                                                  select b;

                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_ListString hall = (Model.ReportSqlParams_ListString)item;
                                if (hall.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where hall.ParamValues.Contains(b.HallID)
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

                        List<Model.View_BorowInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_BorowInfo> aduitList = results.ToList();

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

        /// <summary>
        /// 取消借贷
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, int borowID)
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


                        #region 验证用户是否有取消权限

                        if (user.CancelLimit == null || user.CancelLimit==0)
                        {
                            return new WebReturn() { ReturnValue = false, Message="您无权执行该操作,取消失败！"};
                        }

                        #endregion 

                        #region 获取借贷详情

                        var query = from b in lqh.Umsdb.Pro_BorowInfo
                                    where b.ID == borowID 
                                    select b;
                        if (query.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该单不存在或已取消" };
                        }

                        Model.Pro_BorowInfo model = query.First();
                        if(Convert.ToBoolean(model.IsDelete))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该借贷单已取消，取消失败！" };
                        }
                        if (Convert.ToBoolean(model.IsReturn))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该借贷单已归还，取消失败！" };
                        }

                        var detail = from det in lqh.Umsdb.Pro_BorowListInfo
                                     join b in lqh.Umsdb.Pro_BorowInfo
                                     on det.BorowID equals b.ID
                                     where b.ID == borowID
                                     select det;
                        List<Model.Pro_BorowListInfo> borowList = detail.ToList();

                        #endregion 

                        #region 验证是否已部分归还 及 串码是否已取消

                        foreach (var item in borowList)
                        {
                            if (Convert.ToBoolean(item.IsReturn))
                            {
                                return new WebReturn() { ReturnValue = false, Message = "该借贷单已归还，取消失败！" };
                            }
                            if (item.RetCount!=0)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "该借贷单已归还，取消失败！" };
                            }

                            var imei = from a in item.Pro_BorowOrderIMEI
                                       join b in lqh.Umsdb.Pro_IMEI
                                       on a.IMEI equals b.IMEI
                                       select b;
                            foreach (var child in imei)
                            {    //child.OutID != null || child.RepairID != null || child.SellID != null || child.VIPID != null || child.AssetID或存在其他操作
                                if ( child.BorowID == null  )
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "串码 " + child.IMEI + " 已取消,取消失败！" };
                                }
                            }
                           
                        }
                        #endregion 

                        #region 权限验证  取消借贷只需仓库权限

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, null, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #region  有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count > 0)
                        {
                            if (!ValidHallIDS.Contains(model.HallID))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权限" };
                            }
                        }

                        #endregion 

                        #region 验证商品权限
                        List<string> classids = new List<string>();
                        foreach (var item in borowList)
                        {
                            var queryc = from p in lqh.Umsdb.Pro_ProInfo
                                         where p.ProID == item.ProID.ToString()
                                         select p;
                            classids.Add(queryc.First().Pro_ClassID.ToString());
                        }
                        if (ValidProIDS.Count > 0)
                        {
                            foreach (var child in classids)
                            {
                                if (!ValidProIDS.Contains(child))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权限" };
                                }
                            }
                        }

                        #endregion 

                        #endregion

                        #region  验证取消是否超时

                        if (Convert.ToDecimal(user.CancelLimit) != 0)
                        {
                            var intime = from retn in lqh.Umsdb.Pro_BorowInfo
                                         where retn.ID == borowID
                                         select retn;
                            DateTime rdate = DateTime.Parse(intime.First().BorowDate.ToString());
                            TimeSpan dateDiff = DateTime.Now.Subtract(rdate);

                            if (dateDiff.TotalHours > (double)user.CancelLimit)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "取消超时,取消失败！" };
                            }
                        }

                        #endregion 

                        #region   更新库存

                       List<int> idList = new List<int>();
                        foreach (var item in borowList)
                        {
                            idList.Add(item.BorowListID);
                            var store = from b in lqh.Umsdb.Pro_StoreInfo
                                        where b.InListID == item.InListID && b.ProID==item.ProID && b.HallID==model.HallID
                                        select b;
                           
                            if (store.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(item.ProCount.ToString()))
                                {
                                    store.First().ProCount += decimal.Parse(item.ProCount.ToString());
                                }
                            }
                            else
                            {
                                return new Model.WebReturn() {ReturnValue=false,Message="库存不足，无法取消！" };
                            }
                        }

                       #endregion

                        #region 更新串码表

                        var imei_query = from bimei in lqh.Umsdb.Pro_BorowOrderIMEI
                                         where idList.Contains((int)bimei.BorowListID)
                                         select bimei;
                        foreach (var item in imei_query)
                        {
                            var imei = lqh.Umsdb.Pro_IMEI.Where(p => p.IMEI == item.IMEI);
                            if (imei.Count() != 0)
                            {
                                imei.First().BorowID = null;
                                imei.First().State = 0;
                            }
                            else
                            {
                                return new Model.WebReturn() { ReturnValue=false,Message="库存中找不到串码"+item.IMEI};
                            }
                        }

                        #endregion

                        model.IsDelete =true;
                        model.DeleteDate = DateTime.Now;
                        model.Deleter = user.UserID;

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue=true,Message="取消成功"};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false ,Message=ex.Message};
                    }
                }
            }
        }
    }
}
