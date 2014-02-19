using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Collections.Generic;
using System.Data.Linq;

namespace DAL
{
    /// <summary>
    /// 归还
    /// </summary>
    public class Pro_ReturnInfo
    {
         private int MethodID;

	    public Pro_ReturnInfo()
	    {
		    this.MethodID = 0;
	    }

        public Pro_ReturnInfo(int MenthodID)
	    {
		    this.MethodID = MenthodID;
	    }
        /// <summary>
        /// 归还报表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetReturnList(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from list in lqh.Umsdb.Report_Return
                                select list;

                    return new WebReturn() { ReturnValue = true, Obj=query.ToList()};
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue=false,Message=ex.Message};
                }
            }
        
        }

        /// <summary>
        /// 归还
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_ReturnInfo model)
        {
            //生成单号 存储过程OrderMacker
            //插入表头 
            //插入明细
            //插入串号明细
            //加库存
            //更新串号表      
            //返回
            if (model == null) return new Model.WebReturn();
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
                        List<string> ValidClassIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidClassIDS,lqh);

                        if (model.Pro_ReturnListInfo == null)
                        {
                            return new WebReturn() { ReturnValue =false,Message="无数据可归还"};
                        }
                        if (model.Pro_ReturnListInfo.Count == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "无数据可归还" };
                        }
                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        var queh = from b in lqh.Umsdb.Pro_BorowInfo
                                   join list in lqh.Umsdb.Pro_BorowListInfo
                                   on b.ID equals list.BorowID
                                   where list.BorowListID == model.Pro_ReturnListInfo[0].BorowListID
                                   select b;

                        if (ValidHallIDS.Count>0&&!ValidHallIDS.Contains(queh.First().HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == queh.First().HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                        }

                        //验证商品权限
                        if (ValidClassIDS.Count > 0)
                        {
                            List<string> cids = new List<string>();
                            List<string> pids = new List<string>();
                            foreach (var item in model.Pro_ReturnListInfo)
                            {
                                pids.Add(item.ProID);
                            }
                            var  cn = from c in lqh.Umsdb.Pro_ClassInfo  
                                      join p in lqh.Umsdb.Pro_ProInfo on c.ClassID equals p.Pro_ClassID
                                    where pids.Contains(p.ProID)
                                     select c;
 
                            foreach (var item in cn)
                            {
                                cids.Add(item.ClassID.ToString());
                            }
                            foreach (var child in cids)
                            {
                                if (!ValidClassIDS.Contains(child))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                                }
                            }
                        }
                        #endregion

                        List<string> imeilist = new List<string>();


                        #region 判断数据是否已归还 及 数量是否有效

                        //List<string> imeiList = new System.Collections.Generic.List<string>();
                        //List<int> borowlistid = new List<int>();
 
                        //foreach (var item in model.Pro_ReturnListInfo)
                        //{
                        //    if (item.Pro_ReturnOrderIMEI!=null)
                        //    {
                        //        foreach (var child in item.Pro_ReturnOrderIMEI)
                        //        {
                        //            imeiList.Add(child.IMEI);
                        //            borowlistid.Add((int)item.BorowListID);
                        //        }
                        //    }
                        //}
                        //if (imeiList.Count != 0)
                        //{
                        //    var query = from rem in lqh.Umsdb.Pro_ReturnOrderIMEI
                        //                join rl in lqh.Umsdb.Pro_ReturnListInfo on rem.ReturnListID equals rl.ReturnListID
                        //                join r in lqh.Umsdb.Pro_ReturnInfo on rl.ReturnID equals r.ID
                        //                where imeiList.Contains(rem.IMEI)&& r.IsDelete == false && borowlistid.Contains((int)rl.BorowListID)
                        //                select rem;
                        //    if (query.Count() > 0)
                        //    {
                        //        return new WebReturn() { ReturnValue=false,Message="串码 "+query.First().IMEI+" 已经归还"};
                        //    }

                        //}
                        if (model.Pro_ReturnListInfo.Count()==0)
                        {
                            return new Model.WebReturn() {ReturnValue=false,Message="无数据可归还，归还失败！" };
                        }
                        var borowid = (from bx in lqh.Umsdb.Pro_BorowListInfo
                                       where bx.BorowListID == model.Pro_ReturnListInfo[0].BorowListID
                                       select bx.BorowID).First();

                        List<Pro_ReturnListInfo> checkModels = new List<Pro_ReturnListInfo>();

                        foreach (var item in model.Pro_ReturnListInfo)
                        {
                            if (item.ProCount == 0)
                            {
                                continue;
                            }
                            bool finded = false;
                            foreach(var child in checkModels)
                            {
                                if (child.ProID == item.ProID)
                                {
                                    finded = true;
                                    child.ProCount += item.ProCount;
                                }
                            }
                            if (!finded)
                            {
                                Pro_ReturnListInfo am = new Pro_ReturnListInfo();
                                am.Pro_ReturnOrderIMEI = new EntitySet<Pro_ReturnOrderIMEI>();
                                am.BorowListID = item.BorowListID;
                                am.ProCount = item.ProCount;
                                am.ProID = item.ProID;
                                foreach (var child in item.Pro_ReturnOrderIMEI)
                                {
                                    Pro_ReturnOrderIMEI pm = new Pro_ReturnOrderIMEI();
                                    pm.IMEI = child.IMEI;
                                    am.Pro_ReturnOrderIMEI.Add(pm);
                                }
                                checkModels.Add(am);
                            }
                        }

                        foreach (var item in checkModels)
                        {
                            decimal unRetCount = 0;
                            var borow = from blist in lqh.Umsdb.Pro_BorowListInfo
                                        join b in lqh.Umsdb.Pro_BorowInfo on blist.BorowID equals b.ID
                                        join p in lqh.Umsdb.Pro_ProInfo on blist.ProID equals p.ProID
                                        where b.ID == borowid
                                        && blist.ProID == item.ProID && blist.IsReturn ==false
                                        select new
                                        {
                                            blist.ProID,
                                            blist.ProCount,
                                            blist.RetCount,
                                            blist.IsReturn,
                                            p.NeedIMEI,
                                            p.ProName
                                        };

                            if (borow.Count()==0)
                            {
                                return new Model.WebReturn() { ReturnValue=false,Message="未能找到指定借贷单，归还失败！"};
                            }
                            foreach (var child in borow)
                            {
                                unRetCount +=(decimal)( child.ProCount - child.RetCount);
                                
                            }
                            if (unRetCount == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "商品" + borow.First().ProName + "已归还完毕，归还失败！" };
                            }
                            if (item.ProCount>unRetCount)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "商品" + borow.First().ProName +"的归还数量已超出未归还的数量！" };
                            }
                            //判断串码是否已经归还 及 归还的串码是否有效
                            if (borow.First().NeedIMEI)
                            {
                                #region 判断串码是否已归还 

                                var imeis = from a in lqh.Umsdb.Pro_BorowOrderIMEI
                                            where a.BorowListID == item.BorowListID && a.IsReturn == true
                                            select a;
                                foreach (var child in item.Pro_ReturnOrderIMEI)
                                {
                                    foreach (var itemx in imeis)
                                    {
                                        if (child.IMEI == itemx.IMEI)
                                        {
                                            return new Model.WebReturn(){ReturnValue =false,Message="串码"+child.IMEI+"已归还，归还失败！"};
                                        }
                                    }
                                }

                                #endregion 

                             
                                #region  判断归还的串码是否有效
                                 foreach (var child in item.Pro_ReturnOrderIMEI)
                                {
                                    imeilist.Add(child.IMEI.ToString().ToUpper());
                                   
                                 }
                                #endregion
                            }
                        }
                        #region  判断归还的串码是否有效

                        var val_imeis = (from a in lqh.Umsdb.Pro_BorowInfo
                                         join b in lqh.Umsdb.Pro_BorowListInfo
                                         on a.ID equals b.BorowID
                                         join c in lqh.Umsdb.Pro_BorowOrderIMEI
                                         on b.BorowListID equals c.BorowListID
                                         where a.IsReturn == false && a.ID == borowid
                                         select c.IMEI).Distinct().ToList();

                        var remind = imeilist.Where(p => !val_imeis.Contains(p));
                        if (remind.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "串码" + remind.First() + "无效，归还失败！" };
                        }

                        #endregion 

                        #endregion

                        #region  验证送修单是否已取消

                        var rr = from a in lqh.Umsdb.Pro_BorowInfo
                                 where a.ID == borowid
                                 select a;
                        if (rr.Count()==0)
                        {
                            return new WebReturn() { ReturnValue=false,Message="未能找到借贷记录，归还失败！"};
                        }
                        if (Convert.ToBoolean(rr.First().IsDelete))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "借贷单已取消，归还失败！" };
                        }
                        #endregion 

                        string msg = null;
                        lqh.Umsdb.OrderMacker(1, "GH", "GH", ref msg);
                        if (msg == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "单号生成出错" };
                        }
                        model.BorowID = borowid;
                        model.ReturnID = msg;
                        model.IsDelete = false;
                        model.UserID = user.UserID;
                        lqh.Umsdb.Pro_ReturnInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                     
                     
                        foreach (var item in model.Pro_ReturnListInfo)
                        {
                            if (item.ProCount==0)
                            {
                                continue;
                            }
                            var borow = from blist in lqh.Umsdb.Pro_BorowListInfo
                                        where blist.BorowListID == item.BorowListID
                                        select blist;
                            if (borow.Count()==0)
                            {
                                return new Model.WebReturn() { ReturnValue =false,Message="未能找到指定借贷单，归还失败！"};
                            }
                            borow.First().RetCount += item.ProCount;
                            if (borow.First().RetCount > borow.First().ProCount)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "数据有误，归还失败！" };
                            }
                            else if (borow.First().RetCount == borow.First().ProCount)
                            {
                                borow.First().IsReturn = true;
                            }
                           

                            #region 有串码归还  更新串码及库存

                            if (item.Pro_ReturnOrderIMEI.Count != 0)
                            {
                                foreach (var io in item.Pro_ReturnOrderIMEI)
                                {
                                    var imei = from order in lqh.Umsdb.Pro_IMEI
                                               where (order.IMEI+"") == io.IMEI
                                               select order;
                                    if (imei.Count() > 0)
                                    {
                                        //更新串码的借贷和归还字段
                                        imei.First().ReturnID = io.ID;
                                        imei.First().State = 0;
                                        imei.First().BorowID = null;

                                        //更新库存
                                        var imei_store = from s in lqh.Umsdb.Pro_StoreInfo
                                                    where s.ProID == imei.First().ProID
                                                        && s.InListID == imei.First().InListID
                                                        && s.HallID == imei.First().HallID
                                                    select s;
                                        if (imei_store.Count() > 0)
                                        {
                                            if (imei_store.First().ProCount < 0)
                                            {
                                                return new WebReturn() { ReturnValue = false, Message = "库存有误，归还失败！" };
                                            }
                                            imei_store.First().ProCount += 1;
                                        }
                                        else
                                        {   //库存中不存在此条记录
                                            return new WebReturn() { ReturnValue = false, Message = "库存有误，归还失败！" };
                                        }

                                        var borowrr = from blist in lqh.Umsdb.Pro_BorowOrderIMEI
                                                    where blist.BorowListID == item.BorowListID && blist.IMEI == io.IMEI
                                                    select blist;
                                        borowrr.First().IsReturn = true;
                                    }
                                    else
                                    {
                                        return new Model.WebReturn() {ReturnValue=false,Message="未能找到指定串码，归还失败！" };
                                    }
                                }
                               // lqh.Umsdb.SubmitChanges();
                                continue;
                            }
                        
                           #endregion

                            #region 无串码归还

                            var store = from b in lqh.Umsdb.Pro_StoreInfo
                                        where b.InListID == item.InListID && b.ProID == item.ProID && b.HallID == (
                                        from binfo in lqh.Umsdb.GetTable<Model.Pro_BorowInfo>()
                                        where binfo.ID == borowid
                                        select  binfo.HallID  ).First()
                                        select b;

                           // var m = lqh.Umsdb.GetTable<Model.Pro_IMEI>().Where(p => p.InListID == item.InListID);

                            if (store.Count() > 0)
                            {
                                if (store.First().ProCount < 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false,Message ="库存有误，归还失败！"};
                                 }
                                store.First().ProCount += decimal.Parse(item.ProCount.ToString());
                            }
                            else
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "库存有误，归还失败！" };
                            }
                           
                            #endregion
                        }
                        lqh.Umsdb.SubmitChanges();

                        #region  判断是否归还完毕

                        model.BorowID = borowid;
                      
                        var qb = from borow in lqh.Umsdb.Pro_BorowInfo
                                 join list in lqh.Umsdb.Pro_BorowListInfo
                                 on borow.ID equals list.BorowID
                                 where borow.ID == borowid
                                 select  list;
                        bool allret = true;
                        if (qb.Count()==0)
                        {
                            return new Model.WebReturn() {ReturnValue=false,Message="未能找到借贷单数据，归还失败！" };
                        }
                        foreach (var item in qb)
                        {
                           // item.Pro_BorowOrderIMEI
                            var pro = from p in lqh.Umsdb.Pro_ProInfo
                                      where p.ProID == item.ProID
                                      select p;
                            if (pro.First().NeedIMEI)
                            {
                                var imei_return = from a in lqh.Umsdb.Pro_BorowOrderIMEI
                                                  where a.IsReturn == true && a.BorowListID == item.BorowListID
                                                  select a;
                                if (imei_return.Count() == item.ProCount)
                                {
                                    item.IsReturn = true;
                                }
                                else
                                {
                                    allret = false;
                                    item.IsReturn = false;
                                }
                                continue;
                            }
                          
                            if (item.ProCount != item.RetCount)//!Convert.ToBoolean(item.IsReturn)
                            {
                                allret = false;
                                item.IsReturn = false;
                            }
                            else
                            {
                                item.IsReturn = true;
                            }
                        }

                        var borowx = from a in lqh.Umsdb.Pro_BorowInfo
                                    where a.ID ==borowid
                                    select a;
                        borowx.First().IsReturn = allret;


                        #endregion

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() {ReturnValue = true, Message = "归还成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
           
        }

        /// <summary>
        /// 归还全部选中项    //缺少更新： returnid 没有更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn ReturnBorow(Model.Sys_UserInfo user, List<Model.Pro_ReturnInfo> models)
        {
            if (models == null) return new Model.WebReturn();
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

                          Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS,lqh);

                          if (ret.ReturnValue != true)
                          { return ret; }

                          //有仓库限制，而且仓库不在权限范围内
                          List<string> classids = new List<string>();
                          List<string> hallids = new System.Collections.Generic.List<string>();
                          foreach (var item in models)
                          {
                              var query = from borow in lqh.Umsdb.Pro_BorowInfo
                                          where borow.ID == item.BorowID
                                          select borow;
                              if (query.Count() > 0)
                              {
                                  hallids.Add(query.First().HallID);
                              }
                              var quep = from list in lqh.Umsdb.Pro_BorowListInfo
                                         join p in lqh.Umsdb.Pro_ProInfo
                                         on list.ProID equals p.ProID
                                         where list.BorowID == query.First().ID
                                         select p;
                              if (quep.Count() > 0)
                              {
                                  foreach (var child in quep)
                                  {
                                      classids.Add(child.Pro_ClassID.ToString());
                                  }
                              }
                          }
                          #region  验证仓库
                          if (ValidHallIDS.Count > 0)
                          {
                              foreach (var item in hallids)
                              {
                                  if (!ValidHallIDS.Contains(item))
                                  {
                                      var que = from h in lqh.Umsdb.Pro_HallInfo
                                                where h.HallID == item
                                                select h;
                                      return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                                  }
                              }
                          }
                          #endregion

                          #region 验证商品
                          if (ValidProIDS.Count > 0)
                          {
                              foreach (var child in classids)
                              {
                                  if (!ValidProIDS.Contains(child))
                                  {
                                      return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                                  }
                              }
                          }
                          #endregion

                          #endregion

                          #region  验证借贷单是否已归还
                          List<int> blist = new List<int>();
                          foreach (var item in models)
                          {
                              blist.Add((int)item.BorowID);
                          }
                          var val_borow = from a in lqh.Umsdb.Pro_BorowInfo
                                          where blist.Contains(a.ID) 
                                          select a;
                          List<Model.Pro_BorowInfo> binfo = val_borow.ToList();
                          //if (binfo.Count == 0)
                          //{
                          //    return new Model.WebReturn() { ReturnValue = false, Message = "未能找到借贷单 " + binfo[0].BorowID + " ，归还失败！" };
                          //}

                          if (blist.Count !=binfo.Count)
                          {
                              return new Model.WebReturn() { ReturnValue = false, Message = "未能找到借贷单指定借贷单，归还失败！" };
                          }
                       
                          foreach (var item in binfo)
                          {
                              if (Convert.ToBoolean(item.IsDelete))
                              {
                                  return new Model.WebReturn() { ReturnValue = false, Message = "借贷单 " + item.BorowID + " 已取消！" };
                              }

                              if (Convert.ToBoolean(item.IsReturn))
                              {
                                  return new Model.WebReturn() { ReturnValue = false, Message = "借贷单 " + item.BorowID + " 已归还！" };
                              }
                          }
                          #endregion

                          foreach (var item in models)
                            {
                                //RBack(lqh,item);
                                var borow = from v in lqh.Umsdb.Pro_BorowInfo
                                            join b in lqh.Umsdb.Pro_BorowListInfo
                                            on v.ID equals b.BorowID
                                            join p in lqh.Umsdb.Pro_ProInfo
                                            on b.ProID equals p.ProID
                                            where v.ID == item.BorowID && b.IsReturn ==false
                                            select new
                                            {
                                                b.ProID,
                                                b.ProCount,
                                                b.RetCount,
                                                b.IsReturn,
                                                b.InListID,
                                                b.BorowListID,
                                                p.NeedIMEI,
                                                v.HallID
                                            };
                                if (borow.Count() == 0)
                                {
                                    return new Model.WebReturn() { ReturnValue=false,Message="未能找到指定的借贷单，归还失败！"};
                                }

                                string msg = null;
                                lqh.Umsdb.OrderMacker(1, "GH", "GH", ref msg);
                                if (msg == "")
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "单号生成出错" };
                                }
                                item.ReturnID = msg;
                                item.IsDelete = false;
                                item.ReturnDate = DateTime.Now.Date;
                                item.SysDate = DateTime.Now;
                                item.UserID = user.UserID;
                                item.Pro_ReturnListInfo = new EntitySet<Pro_ReturnListInfo>();
                                foreach (var child in borow)
                                {
                                    Pro_ReturnListInfo pr = new Pro_ReturnListInfo();
                                    pr.BorowListID = child.BorowListID;
                                    pr.InListID = child.InListID;

                                    //获取尚未归还的数量
                                    decimal retcount =(decimal)(child.ProCount - child.RetCount);
                                    pr.ProCount = retcount;
                                    pr.ProID = child.ProID;

                                    if (child.NeedIMEI)
                                    {
                                        pr.Pro_ReturnOrderIMEI = new EntitySet<Pro_ReturnOrderIMEI>();
                                        var imei = from a in lqh.Umsdb.Pro_BorowOrderIMEI
                                                   where a.BorowListID == child.BorowListID && a.IsReturn ==false
                                                   select a;
                                        if (imei.Count()!=0)
                                        {
                                            foreach(var itemx in imei)
                                            {
                                                Pro_ReturnOrderIMEI mm = new Pro_ReturnOrderIMEI();
                                                mm.IMEI = itemx.IMEI;
                                                itemx.IsReturn = true;
                                                pr.Pro_ReturnOrderIMEI.Add(mm);

                                                //标记串码表的借贷单号为null
                                                var org_imei = from a in lqh.Umsdb.Pro_IMEI
                                                               where a.IMEI == itemx.IMEI
                                                               select a;
                                                if (org_imei.Count()==0)
                                                {
                                                    return new Model.WebReturn (){ReturnValue =false,Message ="库存不足,归还失败！"};
                                                }
                                                org_imei.First().BorowID = null;
                                                org_imei.First().State = 0;
                                            }
                                        }
                                    }
                                    item.Pro_ReturnListInfo.Add(pr);
                                    //标记明细已归还
                                   var bblist = from a in lqh.Umsdb.Pro_BorowListInfo
                                                where a.BorowListID == child.BorowListID
                                                select a;
                                   bblist.First().IsReturn = true;
                                   bblist.First().RetCount += retcount;
                                   //更新库存
                                    var store = from s in lqh.Umsdb.Pro_StoreInfo
                                                where s.InListID == child.InListID &&
                                                s.ProID == child.ProID && s.HallID ==child.HallID
                                                select s;
                                    if (store.Count()==0)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "库存不足,归还失败！" };
                                    }
                                    store.First().ProCount += retcount;
                                }

                                //标记借贷单已归还
                                var bb = from b in lqh.Umsdb.Pro_BorowInfo
                                         where b.ID == item.BorowID
                                         select b;

                                bb.First().IsReturn = true;
                                lqh.Umsdb.Pro_ReturnInfo.InsertOnSubmit(item);
                            }

                            lqh.Umsdb.SubmitChanges();
                            ts.Complete();
                            return new Model.WebReturn() { ReturnValue = true, Message = "归还成功" };
                      }
                      catch (Exception ex)
                      {
                          return new Model.WebReturn() { ReturnValue = false,Message=ex.Message };
                      }
                }
            }
        }

        private  void RBack(LinQSqlHelper lqh ,Model.Pro_ReturnInfo model)
        {
            string msg = null;
                lqh.Umsdb.OrderMacker(1, "GH", "GH", ref msg).ToString();
                model.ReturnID = msg;
                model.IsDelete = false;
                model.Pro_ReturnListInfo = new System.Data.Linq.EntitySet<Model.Pro_ReturnListInfo>();

                var query = from borow in lqh.Umsdb.Pro_BorowInfo
                            join list in lqh.Umsdb.Pro_BorowListInfo on borow.ID equals list.BorowID
                            join p in lqh.Umsdb.Pro_ProInfo on list.ProID equals p.ProID
                            where borow.ID == model.BorowID
                            select new
                            {
                                borow.BorowID,
                                borow.HallID,
                                list.BorowListID,
                                BCount = list.ProCount,
                                list.InListID,
                                list.ProID,
                                p.NeedIMEI
                            };

                //if (query.Count() == 0)
                //{
                //    return new WebReturn() { ReturnValue=false,Message="未能找到归还明细，归还失败！"};
                //}
                Model.Pro_ReturnListInfo listInfo = null;
                Model.Pro_ReturnOrderIMEI rimei = null;
                foreach (var item in query)
                {
                    var returninfo = from rlist in lqh.Umsdb.Pro_ReturnListInfo
                                     join r in lqh.Umsdb.Pro_ReturnInfo
                                     on rlist.ReturnID equals r.ID
                                     where rlist.BorowListID == item.BorowListID && r.IsDelete ==false
                                     select rlist;

                    decimal Rcount = 0;
                    int ReturnListID = 0;
                    if (returninfo.Count() != 0)
                    {
                        Rcount = (decimal)returninfo.First().ProCount;
                        ReturnListID = returninfo.First().ReturnListID;
                    }
                    else
                    {
                        Rcount = 0;
                        ReturnListID = -1;
                    }

                    decimal remain = (decimal)item.BCount - Rcount; //获取未归还的数据
                    if (remain > 0) 
                    {
                        listInfo = new Model.Pro_ReturnListInfo();
                        listInfo.Pro_ReturnOrderIMEI = new System.Data.Linq.EntitySet<Model.Pro_ReturnOrderIMEI>();
                        listInfo.BorowListID = item.BorowListID;
                        listInfo.InListID = item.InListID;
                        listInfo.ProCount =remain;
                        listInfo.ProID = item.ProID;

                        //更新库存
                        var qStore = from store in lqh.Umsdb.Pro_StoreInfo
                                     where store.ProID == listInfo.ProID && store.InListID == listInfo.InListID && store.HallID == item.HallID
                                     select store;
                        if (qStore.Count() != 0)
                        {
                            qStore.First().ProCount += decimal.Parse(listInfo.ProCount.ToString());
                            lqh.Umsdb.SubmitChanges();
                        }
                    }
                    if (!item.NeedIMEI)
                    {
                        if (listInfo != null)
                        {
                            model.Pro_ReturnListInfo.Add(listInfo);
                        }
                        continue;
                    }
                    //获取借贷的串码
                    var x1 = from list in lqh.Umsdb.Pro_BorowOrderIMEI
                             where list.BorowListID == item.BorowListID
                             select list.IMEI.ToLower();
                    //获取已归还的串码
                    var x2 = from list in lqh.Umsdb.Pro_ReturnOrderIMEI
                             where list.ReturnListID == ReturnListID
                             select list.IMEI.ToLower();
                    //获取未归还的串码
                    var remainq = from a in x1
                                  where !x2.Contains(a)
                                  select a;

                    foreach (var child in remainq)
                    {
                        rimei = new Model.Pro_ReturnOrderIMEI();
                        rimei.IMEI = child.ToUpper();
                        listInfo.Pro_ReturnOrderIMEI.Add(rimei);
                    }
                    model.Pro_ReturnListInfo.Add(listInfo);
                }

                model.IsDelete = false;
                lqh.Umsdb.Pro_ReturnInfo.InsertOnSubmit(model);
                lqh.Umsdb.SubmitChanges();

                //更新串码表数据  将串码表中的Borow 借贷标志清空
                var que = from borow in lqh.Umsdb.Pro_BorowInfo
                          join list in lqh.Umsdb.Pro_BorowListInfo on borow.ID equals list.BorowID
                          join imei in lqh.Umsdb.Pro_BorowOrderIMEI on listInfo.BorowListID equals imei.BorowListID
                          where borow.ID == model.BorowID
                          select new
                          {
                              imei.IMEI
                          };
                List<string> imeistr = new List<string>();

                foreach (var item in que)
                {
                    imeistr.Add(item.IMEI);
                }

                var ProIMEI = from imei in lqh.Umsdb.Pro_IMEI
                              where imeistr.Contains(imei.IMEI)
                              select imei;
                foreach (var item in ProIMEI)
                {
                    var orderIMEI = from o in lqh.Umsdb.Pro_ReturnOrderIMEI
                                    where o.IMEI==item.IMEI
                                    select o;
                    item.ReturnID = orderIMEI.First().ID;
                    item.State = 0;
                    item.BorowID = null;
                    lqh.Umsdb.SubmitChanges();
                }
               
               //归还成功后将借贷表字段  IsReturn  改为  True
                var qb = from borow in lqh.Umsdb.Pro_BorowInfo
                            where borow.ID == model.BorowID
                            select borow;
                qb.First().IsReturn = true;
                lqh.Umsdb.SubmitChanges();
        }

        /// <summary>
        /// 取消归还
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, int returnID)
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

                        #region  验证用户取消权限


                        if (user.CancelLimit == null || user.CancelLimit == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "权限不足，取消失败！" };
                        }

                        #endregion 

                        var rrinfo = from a in lqh.Umsdb.Pro_ReturnInfo
                                     where a.ID == returnID
                                     select a;
                        List<Model.Pro_ReturnInfo> retmodel = rrinfo.ToList();
                        if (rrinfo.Count()==0)
                        {
                            return new WebReturn() { ReturnValue=false,Message="未能找到指定归还单号，取消失败！"};
                        }
                        if (Convert.ToBoolean(retmodel[0].IsDelete))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该单已取消，取消失败！" };
                        }
                        #region 验证用户操作仓库  商品的权限 

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS,lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        //有仓库限制，而且仓库不在权限范围内
                        var queryhp = from list in lqh.Umsdb.Pro_ReturnListInfo
                                    join r in lqh.Umsdb.Pro_ReturnInfo
                                    on list.ReturnID equals r.ID
                                    join b in lqh.Umsdb.Pro_BorowInfo
                                    on r.BorowID equals b.ID
                                    where r.ID == returnID
                                    select new
                                    {
                                        list.ProID,
                                        b.HallID
                                    };


                        if (ValidHallIDS.Count > 0)
                        {
                            if (!ValidHallIDS.Contains(queryhp.First().HallID))
                            {
                                var que = from h in lqh.Umsdb.Pro_HallInfo
                                          where h.HallID == queryhp.First().HallID
                                          select h;
                                return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                            }
                        }

                        //验证商品权限
                        if (ValidProIDS.Count > 0)
                        {
                            List<string> classids = new List<string>();
                            foreach (var item in queryhp)
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
                            var intime = from retn in lqh.Umsdb.Pro_ReturnInfo
                                         where retn.ID == returnID
                                         select retn;
                            DateTime rdate = DateTime.Parse(intime.First().ReturnDate.ToString());
                            TimeSpan dateDiff = DateTime.Now.Subtract(rdate);

                            if (dateDiff.TotalHours > (double)user.CancelLimit)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "取消超时！" };
                            }
                        }

                        #endregion 


                        #region  减库存  更新串码ReturnID=null

                        var query = from list in lqh.Umsdb.Pro_ReturnListInfo
                                    join r in lqh.Umsdb.Pro_ReturnInfo
                                    on list.ReturnID equals r.ID
                                    join p in lqh.Umsdb.Pro_ProInfo
                                    on list.ProID equals p.ProID
                                    where r.ID == returnID
                                    select new
                                    {
                                        list.ProID,
                                        list.ProCount,
                                        list.BorowListID,
                                        list.InListID,
                                        list.ReturnListID,
                                        p.NeedIMEI
                                    };
                        foreach (var item in query)
                        {
                            //获取借贷商品所在的仓库
                            var hall  = from h in lqh.Umsdb.Pro_HallInfo
                                        join b in lqh.Umsdb.Pro_BorowInfo
                                        on h.HallID equals b.HallID
                                        join list in lqh.Umsdb.Pro_BorowListInfo 
                                        on b.ID equals list.BorowID
                                        where list.BorowListID == (int)item.BorowListID
                                        select h;
                            //获取仓库库存
                            var store = from s in lqh.Umsdb.Pro_StoreInfo
                                        where s.InListID == item.InListID && s.ProID == item.ProID
                                        && s.HallID == hall.First().HallID
                                        select s;
                            if(store.First().ProCount==0)
                            {
                                return new Model.WebReturn() { ReturnValue =false,Message="当前库存不足，无法取消！"};
                            }
                            if (store.First().ProCount < (decimal)item.ProCount)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "当前库存不足，无法取消！" };
                            }
                            store.First().ProCount -= (decimal)item.ProCount;


                            //标记借贷明细为未归还
                            var bb = from b in lqh.Umsdb.Pro_BorowListInfo
                                     where b.BorowListID == item.BorowListID
                                     select b;
                            if (bb.Count()!=0)
                            {
                                bb.First().IsReturn = false;
                                bb.First().RetCount -= item.ProCount ;
                            }
                            else
                            {
                                return new Model.WebReturn() {ReturnValue = false,Message="未能找到指定借贷明细，归还失败！" };
                            }
                            if (!item.NeedIMEI)
                            {
                                continue;
                            }
                            var imei =
                                       from ro in lqh.Umsdb.Pro_ReturnOrderIMEI
                                       where ro.ReturnListID == item.ReturnListID
                                       select ro;
                            if (imei.Count() > 0)
                            {
                                foreach(var itemx in imei)
                                {
                                    var org_imei = from im in lqh.Umsdb.Pro_IMEI
                                                   where im.IMEI == itemx.IMEI.ToUpper()
                                                   select im;
                                    if (org_imei.Count()==0)
                                    {
                                        return new Model.WebReturn(){ReturnValue = false,Message="库存不足，归还失败！"};
                                    }
                                    if (org_imei.First().SellID != null || org_imei.First().OutID != null || org_imei.First().BorowID !=null
                                    || org_imei.First().VIPID != null || org_imei.First().RepairID!=null || org_imei.First().AssetID!=null)
                                    {
                                          return new WebReturn() { ReturnValue = false, Message = "串码存在其他操作，无法取消！" };
                                    }
                                    org_imei.First().ReturnID = null;
                                    //org_imei.First().State = 0;

                                          //获取串码表中的原借贷单号
                                    var oldBorow = from blist in lqh.Umsdb.Pro_BorowOrderIMEI
                                                   where blist.BorowListID == item.BorowListID
                                                   && blist.IMEI == (itemx.IMEI+"").ToUpper()
                                                   select blist;
                                    if (oldBorow.Count() > 0)
                                    {
                                        var imei_update = from im in lqh.Umsdb.Pro_IMEI
                                                          where im.IMEI == (oldBorow.First().IMEI+"").ToUpper()
                                                          select im;
                                        imei_update.First().BorowID = oldBorow.First().ID;
                                        oldBorow.First().IsReturn = false; //标记借贷串码为未归还
                                    }
                                    else
                                    {
                                        return new Model.WebReturn(){ReturnValue = false,Message="未能找到指定借贷明细，归还失败！"};
                                    }
                                }
                            }
                            else
                            {
                                return new Model.WebReturn() { ReturnValue = false,Message="未能找到指定串码数据，无法取消！"};
                            }
                        }

                        #endregion

                        #region 标记该归还单号已删除

                        var reinfo = from r in lqh.Umsdb.Pro_ReturnInfo
                                     where r.ID == returnID
                                     select r;
                        reinfo.First().IsDelete = true;
                        reinfo.First().DeleteDate = DateTime.Now;
                        reinfo.First().Deleter = user.UserName;
                        #endregion

                        #region  将原借贷单标记为未归还

                        var query2 =from b in lqh.Umsdb.Pro_BorowInfo
                                    where b.ID==
                                     ( from r in lqh.Umsdb.Pro_ReturnInfo
                                     where r.ID == returnID
                                     select new {r.BorowID}).First().BorowID
                                     select b;
                        if(query2.Count()==0)
                        {
                            return new WebReturn (){ReturnValue=false,Message="取消出错，没有找到对应的借贷单号"};
                        }
                        query2.First().IsReturn = false;

                        #endregion 

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue =true,Message="取消成功"};
                    }
                    catch(Exception ex)
                    {
                        return new Model.WebReturn() { Obj = false ,Message=ex.Message};
                        throw ex;
                    }

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


                    if (pageParam == null || pageParam.PageIndex < 0 )
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var return_query = from b in lqh.Umsdb.View_ReturnInfo
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            //case "IsDelete":
                            //    Model.ReportSqlParams_String isdelete = (Model.ReportSqlParams_String)item;

                            //    return_query = from b in return_query
                            //                  where b.IsDelete == isdelete.ParamValues
                            //                  select b;
                            //    break;

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
                                        return_query = from b in return_query
                                                       where b.SysDate >= mm5.ParamValues &&
                                                       b.SysDate < DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                                                       select b;
                                    }
                                    else
                                    {
                                        return_query = from b in return_query
                                                       where b.SysDate >= mm5.ParamValues && b.SysDate <= mm6.ParamValues
                                                       select b;
                                    }
                                }
                                break;
                    
                            case "User":
                                Model.ReportSqlParams_String borower = (Model.ReportSqlParams_String)item;
                                if (borower.ParamValues != null)
                                {
                                    return_query = from b in return_query
                                                  where b.UserName == borower.ParamValues
                                                  select b;

                                }
                                break;
                        }
                    }

                    #endregion


                    #region 过滤仓库

                    if (ValidHallIDS.Count() > 0)
                    {
                        return_query = from b in return_query
                                      where ValidHallIDS.Contains(b.HallID)
                                      orderby b.SysDate descending
                                      select b;
                    }
                    else
                    {
                        return_query = from b in return_query
                                      orderby b.SysDate descending
                                      select b;
                    }
                    #endregion
    
                    pageParam.RecordCount = return_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in return_query.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ReturnInfo> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in return_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ReturnInfo> aduitList = results.ToList();

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
        /// 货取返还单明细
        /// </summary>
        /// <param name="user"></param>
        /// <param name="returnID"></param>
        /// <returns></returns>
        public Model.WebReturn GetDetailByID(Model.Sys_UserInfo user, int returnID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var returnList = from r in lqh.Umsdb.Pro_ReturnInfo
                                     join list in lqh.Umsdb.Pro_ReturnListInfo on r.ID equals list.ReturnID
                                     join p in lqh.Umsdb.Pro_ProInfo
                                     on list.ProID equals p.ProID
                                     join t in lqh.Umsdb.Pro_TypeInfo on p.Pro_TypeID equals t.TypeID
                                     join c in lqh.Umsdb.Pro_ClassInfo on p.Pro_ClassID equals c.ClassID
                                     where r.ID == returnID
                                     select new { 
                                       list.ProID,
                                       list.ProCount,
                                       list.Note,
                                       list.InListID,
                                       list.ReturnListID,
                                       p.ProName,
                                       p.ProFormat,
                                       t.TypeName,
                                       c.ClassName
                                     };

                    if (returnList.Count() == 0)
                    {
                        return new WebReturn() { ReturnValue = false,Message="该单无详情"};
                    }
                    List<CancelListModel> models = new List<CancelListModel>();

                    CancelListModel rlm = null;
                    foreach (var item in returnList)
                    {
                        rlm = new CancelListModel();
                        rlm.ClassName = item.ClassName;
                        rlm.InListID = item.InListID;
                        rlm.Note= item.Note;
                        rlm.ProCount = (decimal)item.ProCount;
                        rlm.ProID = item.ProID;
                        rlm.ProName = item.ProName;
                       // rlm.ProFormat = item.ProFormat;
                        rlm.TypeName = item.TypeName;
                        rlm.ImeiList = new List<IMEIModel>();

                        var imei = from im in lqh.Umsdb.Pro_ReturnOrderIMEI
                                   where im.ReturnListID == item.ReturnListID
                                   select im;

                        IMEIModel orimei = null; 
                       foreach(var child in imei)
                        {
                            orimei = new IMEIModel();
                            orimei.NewIMEI= child.IMEI;
                            orimei.Note = child.Note;

                            rlm.ImeiList.Add(orimei);
                        }
                        models.Add(rlm);
                    }
                    return new WebReturn() { ReturnValue = true,Obj = models};
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue=false,Message = ex.Message};
                }
            }
        }
    }
}
