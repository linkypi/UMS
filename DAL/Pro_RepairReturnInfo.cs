using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 返库
    /// </summary>
    public class Pro_RepairReturnInfo
    {
           private int MethodID;

	    public Pro_RepairReturnInfo()
	    {
		    this.MethodID = 0;
	    }

        public Pro_RepairReturnInfo(int MenthodID)
	    {
		    this.MethodID = MenthodID;
	    }

        /// <summary>
        /// 单个接收
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn SingleReceive(Model.Sys_UserInfo user, int rid,  DateTime dt)
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

                        #region "验证用户操作仓库  商品的权限 "

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();
                        List<string> classids = new List<string>();
                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion 

                        var query = from list in lqh.Umsdb.Pro_RepairReturnListInfo
                                    where list.RepairReturnID == rid
                                    select list;
                        var model = from m in lqh.Umsdb.Pro_RepairReturnInfo
                                    where m.ID == rid
                                    select m;
                        if (model.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false,Message="数据库中找不到该返库单号"};
                        }

                        if (Convert.ToBoolean(model.First().IsDelete))
                        {
                            return new Model.WebReturn() { ReturnValue = false,Message="返库单已取消，接收失败！"};
                        }

                        if (Convert.ToBoolean(model.First().IsReceived))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "返库单已接收，接收失败！" };
                        }
                        model.First().Receiver = user.UserID;
                        model.First().RecvTime = dt;

                        if (query.Count() == 0|| model.Count()==0)
                        {
                            return new WebReturn() { ReturnValue=false,Message="接收失败"};
                        }
                        string hallid = model.First().HallID;

                        #region 验证仓库权限

                        if (ValidHallIDS.Count > 0 && !ValidHallIDS.Contains(hallid))
                        {
                            var queh = from h in lqh.Umsdb.Pro_HallInfo
                                       where h.HallID == hallid
                                       select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = queh.First().HallName + "仓库无权操作" };
                        }

                        #endregion 

                        foreach (var rinfo in query)
                        {
                            var quec = from c in lqh.Umsdb.Pro_ClassInfo
                                       join p in lqh.Umsdb.Pro_ProInfo
                                       on c.ClassID equals p.Pro_ClassID
                                       where p.ProID == rinfo.ProID
                                       select c;
                        
                            classids.Add(quec.First().ClassID.ToString());
                           
                            #region 若串码有改变则更新串码

                            if (!string.IsNullOrEmpty(rinfo.NEW_IMEI))
                            {
                                var query1 = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                             where b.IMEI == rinfo.OLD_IMEI && b.InListID == rinfo.InListID
                                             && b.ProID == rinfo.ProID && b.HallID == hallid
                                             select b;
                                List<Model.Pro_IMEI> oldimei = query1.ToList(); 
                               
                                if (oldimei.Count() == 0)
                                {
                                    return new Model.WebReturn() { ReturnValue =false,Message="未能找到指定串码数据，接收失败！"};
                                }
                                if (oldimei[0].RepairID == null)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "串码"+query1.First().IMEI+"已返库，接收失败！" };
                                }
                                //原串码直接删除  更改于2013.6.15（原来只需将原串码的NEW_IMEI_ID加标记）
                                //标记已删除
                                Model.Pro_IMEI_Deleted del = new Pro_IMEI_Deleted();
                                del.IMEI = oldimei[0].IMEI;
                                lqh.Umsdb.Pro_IMEI_Deleted.InsertOnSubmit(del);

                                lqh.Umsdb.Pro_IMEI.DeleteOnSubmit(oldimei[0]);

                                //验证新串码是否已存在
                                var imei = from a in lqh.Umsdb.Pro_IMEI
                                           where a.IMEI == rinfo.NEW_IMEI && string.IsNullOrEmpty(rinfo.NEW_IMEI)==false
                                           select a;
                                if (imei.Count() != 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "新串码 " + rinfo.NEW_IMEI + " 已存在，接收失败！" };
                                }

                                Model.Pro_IMEI newIMEI = new Model.Pro_IMEI();
                                newIMEI.IMEI = rinfo.NEW_IMEI;
                                newIMEI.HallID = hallid;
                                newIMEI.InListID = rinfo.InListID;
                                newIMEI.ProID = rinfo.ProID;
                                lqh.Umsdb.Pro_IMEI.InsertOnSubmit(newIMEI);
                                lqh.Umsdb.SubmitChanges();
                                //oldimei[0].RepairID = null;
                                //oldimei[0].NEW_IMEI_ID = newIMEI.ID;

                                //为新的串码添加新库存
                                var store = from s in lqh.Umsdb.Pro_StoreInfo
                                            where s.ProID == rinfo.ProID && s.InListID == rinfo.InListID && s.HallID == hallid
                                            select s;
                                if (store.Count() == 0)
                                {
                                  return new WebReturn() { ReturnValue = false,Message="库存有误，接收失败！"};
                                }
                                else
                                {
                                    store.First().ProCount += 1;
                                }
                                lqh.Umsdb.SubmitChanges();
                                continue;
                            }
                            #endregion

                            #region  串码没有改变

                            if (!string.IsNullOrEmpty(rinfo.OLD_IMEI) && string.IsNullOrEmpty(rinfo.NEW_IMEI))
                            {
                                var query1 = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                             where b.IMEI == rinfo.OLD_IMEI && b.InListID == rinfo.InListID
                                             && b.ProID == rinfo.ProID && b.HallID == hallid
                                             select b;
                                query1.First().RepairID = null;
                                query1.First().State = 0;
                            }

                            #endregion

                            #region  更新库存

                            var query3 = from store in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                         where store.InListID == rinfo.InListID && store.ProID == rinfo.ProID && store.HallID == hallid
                                         select store;
                            if (query3.Count()==0)
                            {
                                return new Model.WebReturn() { ReturnValue=false,Message="库存不足，接收失败！"};
                            }
                            query3.First().ProCount += decimal.Parse(rinfo.ProCount.ToString());

                            #endregion
                        }

                        #region   验证商品权限

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

                        //标记已接收返库
                         model.First().IsReceived = true;
                         model.First().IsDelete = false;

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Message = "接收成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 批量接收
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn BatchReceive(Model.Sys_UserInfo user,List<int> ridList)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证用户操作仓库  商品的权限 
                        
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }


                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();
                        List<string> classids = new List<string>();
                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion

                        #region   验证返库明细中的新串码是否重复

                        var valImei =( from ma in lqh.Umsdb.Pro_RepairReturnListInfo
                                      where ridList.Contains((int)ma.RepairReturnID) 
                                      select ma.NEW_IMEI).ToList();
                        foreach (var item in valImei)
                        {
                            if (string.IsNullOrEmpty(item)) { continue; }
                            if (valImei.Where(p => p == item).Count() > 1)
                            {
                                return new WebReturn() { ReturnValue =false,Message="返库串码中存在相同新串码：\n"+item+"  接收失败！"};
                            }
                        }

                        #endregion 


                        //获取全部返库单
                        var queryList = from q in lqh.Umsdb.Pro_RepairReturnInfo
                                    where ridList.Contains(q.ID)
                                    select q;
                        if (queryList.Count() != ridList.Count)
                        {
                            return new WebReturn() { ReturnValue=false,Message="部分返库单不存在，接收失败！"};
                        }
                        var models = queryList.ToList();
                        foreach (var model in models)
                        {
                            //对每一张单执行接收
                            if (Convert.ToBoolean(model.IsDelete))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "返库单 "+model.RepairReturnID+" 已取消，接收失败！" };
                            }
                            if (Convert.ToBoolean(model.IsReceived))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "返库单" + model.RepairReturnID + "已接收，接收失败！" };
                            }

                            #region 验证仓库权限

                            if (ValidHallIDS.Count > 0 && !ValidHallIDS.Contains(model.HallID))
                            {
                                var queh = from h in lqh.Umsdb.Pro_HallInfo
                                           where h.HallID == model.HallID
                                           select h;
                                return new Model.WebReturn() { ReturnValue = false, Message = queh.First().HallName + "仓库无权操作" };
                            }
                            #endregion

                            var query = from list in lqh.Umsdb.Pro_RepairReturnListInfo
                                        where list.RepairReturnID == model.ID
                                        select list;

                            foreach (var rinfo in query)
                            {
                                var quec = from c in lqh.Umsdb.Pro_ClassInfo
                                           join p in lqh.Umsdb.Pro_ProInfo
                                           on c.ClassID equals p.Pro_ClassID
                                           where p.ProID == rinfo.ProID
                                           select c;

                                classids.Add(quec.First().ClassID.ToString());

                                #region 若串码有改变则更新串码

                                if (!string.IsNullOrEmpty(rinfo.NEW_IMEI))
                                {
                                    var query1 = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                                 where b.IMEI == rinfo.OLD_IMEI && b.InListID == rinfo.InListID
                                                 && b.ProID == rinfo.ProID && b.HallID == model.HallID
                                                 select b;
                                    List<Model.Pro_IMEI> oldimei = query1.ToList();
                                    if (oldimei.Count() == 0)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定串码数据，接收失败！" };
                                    }
                                    if (oldimei[0].RepairID == null)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "串码" + query1.First().IMEI + "已返库，接收失败！" };
                                    }
                                    //原串码直接删除  更改于2013.6.15（原来只需将原串码的NEW_IMEI_ID加标记）
                                    lqh.Umsdb.Pro_IMEI.DeleteOnSubmit(oldimei[0]);

                                    //验证新串码是否已存在
                                    //验证新串码是否已存在
                                    var imei = from a in lqh.Umsdb.Pro_IMEI
                                               where a.IMEI == rinfo.NEW_IMEI && string.IsNullOrEmpty(rinfo.NEW_IMEI)==false
                                               select a;
                                    if (imei.Count() != 0)
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "新串码 " + rinfo.NEW_IMEI + " 已存在，接收失败！" };
                                    }


                                    Model.Pro_IMEI newIMEI = new Model.Pro_IMEI();
                                    newIMEI.IMEI = rinfo.NEW_IMEI;
                                    newIMEI.HallID = model.HallID;
                                    newIMEI.InListID = rinfo.InListID;
                                    newIMEI.ProID = rinfo.ProID;
                                    lqh.Umsdb.Pro_IMEI.InsertOnSubmit(newIMEI);
                                   // lqh.Umsdb.SubmitChanges();
                                    //oldimei[0].RepairID = null;
                                    //oldimei[0].NEW_IMEI_ID = newIMEI.ID;

                                    //为新的串码添加新库存
                                    var store = from s in lqh.Umsdb.Pro_StoreInfo
                                                where s.ProID == rinfo.ProID && s.InListID == rinfo.InListID && s.HallID == model.HallID
                                                select s;
                                    if (store.Count() == 0)
                                    {
                                        return new WebReturn() {ReturnValue = false,Message="库存有误，接收失败！" };
                                    }
                                    else
                                    {
                                        store.First().ProCount += 1;
                                    }
                                    lqh.Umsdb.SubmitChanges();
                                    continue; //跳到下一循环
                                }
                                #endregion

                                #region  串码没有改变

                                if (!string.IsNullOrEmpty(rinfo.OLD_IMEI) && string.IsNullOrEmpty(rinfo.NEW_IMEI))
                                {
                                    var query1 = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                                 where b.IMEI == rinfo.OLD_IMEI && b.InListID == rinfo.InListID
                                                 && b.ProID == rinfo.ProID && b.HallID == model.HallID
                                                 select b;
                                    query1.First().RepairID = null;
                                    query1.First().State = 0;
                                }

                                #endregion

                                #region  更新库存

                                var query3 = from store in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                             where store.InListID == rinfo.InListID && store.ProID == rinfo.ProID && store.HallID == model.HallID
                                             select store;
                                if (query3.Count() == 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "库存不足，接收失败！" };
                                }
                                query3.First().ProCount += decimal.Parse(rinfo.ProCount.ToString());

                                #endregion
                            }


                            #region   验证商品权限
                            if (ValidProIDS.Count > 0)
                            {
                                foreach (var child in classids)
                                {
                                    if (!ValidProIDS.Contains(child))
                                    {
                                        return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作，接收失败！" };
                                    }
                                }
                            }
                            #endregion

                            //标记已接收接收返库
                            model.IsReceived = true;
                            model.IsDelete = false;
                            lqh.Umsdb.SubmitChanges();
                        }

                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true ,Message="接收成功"};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 单个返库
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="repairID"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_RepairReturnInfo model)
        {
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

                        #region 验证用户操作仓库  商品的权限 

                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (!ret.ReturnValue)
                        { return ret; }

                        //有仓库限制，而且仓库不在权限范围内 ValidHallIDS.Count=0 表示无需验证仓库
                        if (ValidHallIDS.Count > 0 && !ValidHallIDS.Contains(model.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                        }

                        //验证商品权限   ValidProIDS.Count=0表示无需验证商品
                        if (ValidProIDS.Count > 0)
                        {
                            List<string> classids = new List<string>();
                            foreach (var item in model.Pro_RepairReturnListInfo)
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

                        #region  验证串码返库是否正确
                          List<string> proids = new List<string>();
                          List<string> imeis_new = new List<string>();
                          List<string> imeis_old = new List<string>();


                          foreach (var item in model.Pro_RepairReturnListInfo)
                          {
                              if (item.OLD_IMEI.ToUpper().Equals(item.NEW_IMEI.ToUpper()))
                              {
                                  return new WebReturn() { ReturnValue = false, Message = "返库新串码 " + item.NEW_IMEI + " 与旧串码一致,返库失败！" };
                              }
                              if (!string.IsNullOrEmpty(item.NEW_IMEI))
                              {
                                  imeis_new.Add(item.NEW_IMEI);
                              }
                              imeis_old.Add(item.OLD_IMEI);
                          }

                         #region 验证旧串码是否正确 及 旧串码是否已返库

                         var que_old = from a in lqh.Umsdb.Pro_IMEI
                                        where imeis_old.Contains(a.IMEI)// && a.RepairID == null
                                        select a;
                        if (que_old.Count()==0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "未能找到指定旧串码,返库失败！" };
                        }
                        List<string> imeis1 = new List<string>();

                        foreach (var item in que_old)
                        {
                            if (item.RepairID==null)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "旧串码 " + item.IMEI + " 已返库,返库失败！" };
                            }
                            imeis1.Add(item.IMEI);
                        }
                        foreach (var item in imeis_old)
                        {
                          if (!imeis1.Contains(item))
                          {
                              return new WebReturn() { ReturnValue = false, Message = "旧串码 "+item+" 不正确,返库失败！" };
                          }
                        }

                       #endregion 

                        #region 验证新串码是否已存在

                        var imei_new = from a in lqh.Umsdb.Pro_IMEI
                                       where imeis_new.Contains(a.IMEI)
                                       select a;
                        if (imei_new.Count()!=0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "新串码 " + imei_new.First().IMEI + " 已存在,返库失败！" };
                        }
                        #endregion 

                        #endregion

                        #region  验证数据是否已返库


                        foreach (var item in model.Pro_RepairReturnListInfo)
                        {
                            var rrinfo = from b in lqh.Umsdb.Pro_RepairListInfo
                                         join a in lqh.Umsdb.Pro_RepairReturnListInfo
                                           on b.RepairListID equals a.RepairListID
                                         join c in lqh.Umsdb.Pro_RepairReturnInfo
                                         on a.RepairReturnID equals c.ID
                                         join p in lqh.Umsdb.Pro_ProInfo on b.ProID equals p.ProID
                                         where c.IsDelete != true && b.RepairListID == item.RepairListID
                                         select new { a,p.NeedIMEI,p.ProName };
                            if (rrinfo.Count() != 0)
                            {
                                if (rrinfo.First().NeedIMEI)
                                {
                                    return new WebReturn() { ReturnValue = false, Message = "串码 " + item.OLD_IMEI + " 已返库，返库失败！" };
                                }
                                else
                                {
                                    return new WebReturn() { ReturnValue = false, Message = "商品 " + rrinfo.First().ProName + " 已返库，返库失败！" };
                                }
                            }

                        }

                        //var rrinfo = from a in model.Pro_RepairReturnListInfo
                        //             join b in lqh.Umsdb.Pro_RepairListInfo
                        //             on a.RepairListID equals b.RepairListID
                        //             join p in lqh.Umsdb.Pro_ProInfo on a.ProID equals p.ProID
                                     
                        //             select new { a.ProCount, RetCount = b.ProCount, p.ProName, p.NeedIMEI, a.OLD_IMEI };

                        //foreach (var item in rrinfo)
                        //{
                        //    if (item.ProCount == item.RetCount)
                        //    {
                        //        if (item.NeedIMEI)
                        //        {
                        //            return new WebReturn() { ReturnValue = false, Message = "串码" + item.OLD_IMEI + "已返库！" };
                        //        }
                        //        else
                        //        {
                        //            return new WebReturn() { ReturnValue = false, Message = "商品" + item.ProName + "已返库！" };
                        //        }

                        //    }
                        //}

                        #endregion 

                        string repairReturnID = null;
                        lqh.Umsdb.OrderMacker(1, "RFK", "RFK", ref repairReturnID);
                        if (repairReturnID == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "返库单生成出错" };
                        }
                        model.RepairReturnID = repairReturnID;
                        model.IsReceived = false;
                        model.IsDelete = false;
                        lqh.Umsdb.Pro_RepairReturnInfo.InsertOnSubmit(model);
                        //lqh.Umsdb.SubmitChanges();

                        #region `验证数据是否已经返库完毕
                        //var val = from a in lqh.Umsdb.View_RepaireRetList
                        //          where  a.RepairID == model.RepairID
                        //          select a;
                        //if (val.Count() <= 0)
                        //{
                        //    return new Model.WebReturn() { ReturnValue=false,Message="无法找到送修记录，返库出错！"};
                        //}
                        //bool allret = true;
                        //foreach (var item in val)
                        //{
                        //    if (item.IsReturn == "N")
                        //    {
                        //        allret = false;
                        //        break;
                        //    }
                        //}

                        var repair = from r in lqh.Umsdb.Pro_RepairInfo
                                     where r.ID == model.RepairID
                                     select r;
                        if (repair.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "无法找到送修记录，返库出错！" };
                        }
                        repair.First().IsReturn = true;
                        lqh.Umsdb.SubmitChanges();
                        #endregion

                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Message = "返库成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false ,Message = ex.Message};
                    }
                }
           }
       }

        /// <summary>
        /// 批量返库    -- 无新串码返库
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <param name="repairID"></param>
        /// <returns></returns>
        public Model.WebReturn BatchReturn(Model.Sys_UserInfo user, List<Model.Pro_RepairReturnInfo> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        //#region "验证用户操作仓库  商品的权限 "

                        //List<string> ValidHallIDS = new List<string>();
                        ////有权限的商品
                        //List<string> ValidProIDS = new List<string>();
                        //List<string> classids = new List<string>();
                        //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS,lqh);

                        //if (ret.ReturnValue != true)
                        //{ return ret; }

                        //#endregion 

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        foreach (var model in models)
                        {
                            //#region 验证仓库权限

                            //if (ValidHallIDS.Count>0&&!ValidHallIDS.Contains(model.HallID))
                            //{
                            //    var queh = from h in lqh.Umsdb.Pro_HallInfo
                            //              where h.HallID == model.HallID
                            //              select h;
                            //    return new Model.WebReturn() { ReturnValue = false, Message = queh.First().HallName + "仓库无权操作" };
                            //}
                            //#endregion 

                            model.Pro_RepairReturnListInfo = new System.Data.Linq.EntitySet<Model.Pro_RepairReturnListInfo>();
                            Model.Pro_RepairReturnListInfo rrlist = null;
                           
                            string repairReturnID = null;
                            lqh.Umsdb.OrderMacker(1, "RFK", "RFK", ref repairReturnID);
                            if (string.IsNullOrEmpty(repairReturnID))
                            {
                                return new WebReturn() { ReturnValue = false, Message = "单号生成失败" };
                            }
                            model.RepairReturnID = repairReturnID;
                            model.IsReceived = false;

                            var query = from repair in lqh.Umsdb.Pro_RepairListInfo
                                        where repair.RepairID == model.RepairID
                                        select repair;

                            List<string> imeiList = new List<string>();

                            if (query.Count() != 0)
                            {
                                foreach (var item in query)
                                {
                                    imeiList.Add(item.IMEI);

                                    var queryc = from p in lqh.Umsdb.Pro_ProInfo
                                                 where p.ProID == item.ProID.ToString()
                                                 select p;
                                   // classids.Add(queryc.First().Pro_ClassID.ToString());

                                    rrlist = new Model.Pro_RepairReturnListInfo();
                                    rrlist.InListID = item.InListID;
                                    rrlist.OLD_IMEI = item.IMEI;
                                    rrlist.Note = item.Note;
                                    rrlist.ProCount = item.ProCount;
                                    rrlist.ProID = item.ProID;
                                   
                                    rrlist.RepairListID = item.RepairListID;
                                    model.Pro_RepairReturnListInfo.Add(rrlist);
                                }
                            }
                            else
                            {
                                return new Model.WebReturn() { ReturnValue=false,Message="未能找到送修明细，返库有误！"};
                            }

                            var repairmodel = from r in lqh.Umsdb.Pro_RepairInfo
                                         where r.ID == model.RepairID
                                         select r;
                            if (repairmodel.Count() == 0)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "送修返库出错" };
                            }
                            repairmodel.First().IsReturn = true;


                            lqh.Umsdb.Pro_RepairReturnInfo.InsertOnSubmit(model);
                            lqh.Umsdb.SubmitChanges();

                        }

                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue=true};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue =false,Message=ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 根据送修单获取其详细信息
        /// </summary>
        /// <param name="repairid"></param>
        /// <returns></returns>
        public Model.WebReturn GetIMEIByRepairID(Model.Sys_UserInfo user, string repairid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                var query = from repair in lqh.Umsdb.Pro_RepairInfo
                            join repairList in lqh.Umsdb.Pro_RepairListInfo
                            on repair.ID equals repairList.RepairID
                            where repair.RepairID == repairid
                            select new
                            {
                                repair.HallID,
                                repairList.ProID,
                                repairList.IMEI,
                                repairList.InListID,
                                repairList.ProCount,
                                repairList.RepairListID
                            };

                if (query.Count() == 0)
                {
                    return new Model.WebReturn(){ReturnValue=false};
            }
                List<Model.SetSelection> smodels = new List<Model.SetSelection>();
        
                Model.SetSelection ss = null;
                foreach (var qe in query)
                {
                    ss = new Model.SetSelection();
                    ss.InList = qe.InListID;
                    ss.Proid = qe.ProID;
                    ss.Countnum =int.Parse( qe.ProCount.ToString());
                 //   ss.RepairListID = qe.RepairListID;
                    ss.HallID = qe.HallID;
                    if (ss.OldIMEI == null)
                    {
                        ss.OldIMEI = new List<string>();
                    }
                    if (!string.IsNullOrEmpty(qe.IMEI))
                    {
                        ss.OldIMEI.Add(qe.IMEI);
                    }
                    smodels.Add(ss);
                }
                return new Model.WebReturn() { Obj=smodels,ReturnValue=true};
            }
        }

        /// <summary>
        /// 查询记录
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
                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return new WebReturn() { ReturnValue = false, Obj = pageParam }; }

                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var return_query = from b in lqh.Umsdb.View_Pro_RepairReturnInfo
                                       select b;

                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "Canceled":
                                Model.ReportSqlParams_String cancel = (Model.ReportSqlParams_String)item;
                                if (cancel.ParamValues != null)
                                {
                                    return_query = from b in return_query
                                                   where b.Canceled.Contains(cancel.ParamValues)
                                                   select b;
                                }
                                break;

                            case "IsReceived":
                                Model.ReportSqlParams_String receive = (Model.ReportSqlParams_String)item;
                                if (receive.ParamValues != null)
                                {
                                    return_query = from b in return_query
                                                   where b.IsReceived.Contains(receive.ParamValues)
                                                   select b;
                                }
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
                      
                            case "UserName":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    return_query = from b in return_query
                                                   where b.UserName.Contains(para.ParamValues)
                                                   select b;
                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                if (para1.ParamValues != null)
                                {
                                    return_query = from b in return_query
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
                        var results = from r in return_query.Take(pageParam.PageSize).ToList()
                                      select r;

                        List<Model.View_Pro_RepairReturnInfo> list = results.ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    else
                    {
                        var results = from r in return_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select r;

                        List<Model.View_Pro_RepairReturnInfo> list = results.ToList();
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
        /// 返库接收查询
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn SearchReceive(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
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

                    var return_query = from b in lqh.Umsdb.View_RepairReturnReceive
                                       select b;

                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "IsReceived":
                                Model.ReportSqlParams_String cancel = (Model.ReportSqlParams_String)item;
                                if (cancel.ParamValues != null)
                                {
                                    return_query = from b in return_query
                                                   where b.IsReceived.Contains(cancel.ParamValues)
                                                   select b;
                                }
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
                      
                            case "UserName":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    return_query = from b in return_query
                                                   where b.UserName.Contains(para.ParamValues)
                                                   select b;
                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                if (para1.ParamValues != null)
                                {
                                    return_query = from b in return_query
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
                        var results = from r in return_query.Take(pageParam.PageSize).ToList()
                                   
                                      select r;

                        List<Model.View_RepairReturnReceive> list = results.ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    else
                    {
                        var results = from r in return_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                  
                                      select r;

                        List<Model.View_RepairReturnReceive> list = results.ToList();
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
        /// 获取返库明细
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.WebReturn GetDetailByID(Model.Sys_UserInfo user, int id)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                var result = from detail in lqh.Umsdb.View_Pro_RepairRetrunDetail
                             where detail.ID == id
                             select detail;
                List<Model.View_Pro_RepairRetrunDetail> list = new List<View_Pro_RepairRetrunDetail>();
                if (result.Count() != 0)
                {
                    list = result.ToList();
                    return new WebReturn() { ReturnValue = true, Obj = list };
                }
                else
                {
                    return new WebReturn() { ReturnValue = false, Obj = list };
                }
            }
        }

        /// <summary>
        /// 取消返库
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.WebReturn ReturnCancel(Model.Sys_UserInfo user, int id)
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

                        #region 获取明细

                        var rrinfo = from rr in lqh.Umsdb.Pro_RepairReturnInfo
                                     where rr.ID == id && rr.IsDelete == false 
                                     select rr;

                        if (rrinfo.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "返库取消出错，数据库中没有该返库记录" };
                        }
                        if (Convert.ToBoolean(rrinfo.First().IsReceived))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "返库单已接收，无法取消！" };
                        }
                        var rrlist = from rr in lqh.Umsdb.Pro_RepairReturnInfo
                                     join list in lqh.Umsdb.Pro_RepairReturnListInfo
                                     on rr.ID equals list.RepairReturnID
                                     where rr.ID == id
                                     select list;

                        #endregion 

                        #region 权限验证

                        //List<string> ValidHallIDS = new List<string>();
                        ////有权限的商品
                        //List<string> ValidProIDS = new List<string>();

                        //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS,lqh);

                        //if (ret.ReturnValue != true)
                        //{ return ret; }

                        ////有仓库限制，而且仓库不在权限范围内
                        //if (ValidHallIDS.Count>0&&!ValidHallIDS.Contains(rrinfo.First().HallID))
                        //{
                        //    var que = from h in lqh.Umsdb.Pro_HallInfo
                        //              where h.HallID == rrinfo.First().HallID
                        //              select h;
                        //    return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                        //}

                        ////验证商品权限
                        //if (ValidProIDS.Count > 0)
                        //{
                        //    List<string> classids = new List<string>();
                        //    foreach (var item in rrlist)
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

                    
                        #region  验证取消是否超时

                        if (Convert.ToDecimal(user.CancelLimit) != 0)
                        {
                            var intime = from retn in lqh.Umsdb.Pro_RepairReturnInfo
                                         where retn.ID == id
                                         select retn;
                            DateTime rdate = DateTime.Parse(intime.First().RepairReturnDate.ToString());
                            TimeSpan dateDiff = DateTime.Now.Subtract(rdate);

                            if (dateDiff.TotalHours > (double)user.CancelLimit)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "返库取消超时！" };
                            }
                        }

                        #endregion 

                        rrinfo.First().IsDelete = true;

                        var repaire = from a in lqh.Umsdb.Pro_RepairInfo
                                      where a.ID ==rrinfo.First().RepairID
                                      select a;
                        if(repaire.Count()==0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未找到送修记录，取消有误！" }; ;
                        }
                        repaire.First().IsReturn = false;
                        #region 备份数据

                        //var r = from re in lqh.Umsdb.Pro_RepairInfo
                        //        where re.ID == rrinfo.First().RepairID
                        //        select re;
                        //r.First().IsReturn = false;

                        //Model.Pro_RepairReturnInfo_BAK rrbak = new Pro_RepairReturnInfo_BAK();
                        //rrbak.OriginalID = rrinfo.First().ID;
                        //rrbak.OldID = rrinfo.First().OldID;
                        //rrbak.Note = rrinfo.First().Note;
                        //rrbak.HallID = rrinfo.First().HallID;
                        //rrbak.RepairID = rrinfo.First().RepairID;
                        //rrbak.RepairReturnDate = rrinfo.First().RepairReturnDate;
                        //rrbak.RepairReturnID = rrinfo.First().RepairReturnID;
                        //rrbak.SysDate = rrinfo.First().SysDate;
                        //rrbak.UserID = rrinfo.First().UserID;
                        //rrbak.DeleteDate = DateTime.Now;
                        //rrbak.Deleter = user.UserName;
                        //rrbak.IsDelete = true;
                        //lqh.Umsdb.Pro_RepairReturnInfo_BAK.InsertOnSubmit(rrbak);

                        #endregion

                        #region 恢复数据  

                        //foreach (var item in rrlist)
                        //{
                        //    //若是有串码商品 则恢复其送修单
                        //    if (!string.IsNullOrEmpty(item.OLD_IMEI))
                        //    {
                        //        //串码改变
                        //        if (!string.IsNullOrEmpty(item.NEW_IMEI))
                        //        {
                        //            #region 删除新串码  还原旧串码
                        //            // 删除新串码
                        //            var query = from imei in lqh.Umsdb.Pro_IMEI
                        //                        where imei.IMEI == item.NEW_IMEI
                        //                        select imei;
                        //            if (query.Count() != 0)
                        //            {
                        //                lqh.Umsdb.Pro_IMEI.DeleteOnSubmit(query.First());
                        //            }
                        //            //还原旧串码
                        //            var que = from imei in lqh.Umsdb.Pro_IMEI
                        //                      where imei.IMEI == item.OLD_IMEI
                        //                      select imei;
                        //            if (que.Count() != 0)
                        //            {
                        //                que.First().NEW_IMEI_ID = null;
                        //            }
                        //            #region 删除库存中的新串码
                        //            var que2 = from im in lqh.Umsdb.Pro_IMEI
                        //                       where im.IMEI == item.NEW_IMEI
                        //                       select im;
                        //            if (que2.Count() > 0)
                        //            {
                        //                var store = from s in lqh.Umsdb.Pro_StoreInfo
                        //                            where s.InListID == que2.First().InListID
                        //                            select s;

                        //                if (store.Count() > 0)
                        //                {
                        //                    lqh.Umsdb.Pro_StoreInfo.DeleteOnSubmit(store.First());
                        //                }
                        //            }
                        //            #endregion

                        //            #endregion

                        //        }
                        //        else
                        //        {
                        //            #region 更新库存

                        //            var que_store = from s in lqh.Umsdb.Pro_StoreInfo
                        //                            where s.ProID == item.ProID && s.InListID == item.InListID
                        //                            && s.HallID == rrinfo.First().HallID
                        //                            select s;
                        //            que_store.First().ProCount -= 1;

                        //            #endregion
                        //        }


                        //        var query2 = from im in lqh.Umsdb.Pro_IMEI
                        //                        where im.ProID == item.ProID && im.InListID == item.InListID
                        //                        && im.HallID == rrinfo.First().HallID && im.IMEI == item.OLD_IMEI
                        //                        select im;
                        //        query2.First().RepairID =item.RepairListID;
                        //        continue;
                        //    }
                        //}

                        #endregion

                        #region  删除返库数据

                       // lqh.Umsdb.Pro_RepairReturnInfo.DeleteOnSubmit(rrinfo.First());

                        #endregion

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue = true,Message="取消成功",Obj=id};
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
