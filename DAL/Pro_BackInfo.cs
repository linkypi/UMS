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
    /// 退库
    /// </summary>
    public class Pro_BackInfo
    {
          private int MethodID;

	    public Pro_BackInfo()
	    {
		    this.MethodID = 0;
	    }

        public Pro_BackInfo(int MenthodID)
	    {
		    this.MethodID = MenthodID;
	    }

        /// <summary>
        /// 新增退库
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_BackInfo model, List<string> s,List<Model.Pro_IMEI_Deleted> del)
        {
            //验证是否可退库 
            //生成单号 存储过程OrderMacker
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

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS,lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count>0&&!ValidHallIDS.Contains(model.HallID))
                        {
                            var que = from h in lqh.Umsdb.Pro_HallInfo
                                      where h.HallID == model.HallID
                                      select h;
                            return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作"  + que.First().HallName  };
                        }

                        //验证商品权限
                        if (ValidProIDS.Count > 0)
                        {
                            List<string> classids = new List<string>();
                            foreach (var item in model.Pro_BackListInfo)
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

                        string msg = null;
                        lqh.Umsdb.OrderMacker(1, "TK", "TK", ref msg);
                        if (msg == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "退库单生成出错" };
                        }
                        model.BackID = msg;

                        var query = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                    where s.Contains(b.IMEI) && (b.SellID == null || b.SellID == 0) && (b.BorowID == null || b.BorowID == 0) && (b.RepairID == null || b.RepairID == 0) && (b.BorowID == null || b.RepairID == 0) && (b.AssetID == null || b.AssetID == 0)
                                    select b;
                        if (query.Count() != s.Count())
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "退库失败，部分串码不存在！" };
                        }
                   
                       
                        foreach (var i in model.Pro_BackListInfo)
                        { 
                            var query_s=from b in lqh.Umsdb.GetTable<Model.Pro_StoreInfo>()
                                    where b.InListID==i.InListID&&b.ProID==i.ProID.ToString()&&b.HallID==model.HallID
                                    select b;
                            try
                            {
                                Model.Pro_StoreInfo store = query_s.First();
                                store.ProCount -= (decimal)i.ProCount;
                                if (store.ProCount < 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "退库失败，库存不足！" };
                                }
                            }
                            catch
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "该批次商品库存已经被删除，请联系管理员！" };
                            }
                             lqh.Umsdb.SubmitChanges();
                        }
                        lqh.Umsdb.Pro_IMEI.DeleteAllOnSubmit(query);
                        lqh.Umsdb.Pro_IMEI_Deleted.InsertAllOnSubmit(del);
                        lqh.Umsdb.Pro_BackInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "退库成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "退库失败" };
                        throw ex;
                    }
                }
            }

            //插入表头
            //插入明细
            //插入串号明细
            //减去库存
            //删除串号
            // 返回


        }
    }
}