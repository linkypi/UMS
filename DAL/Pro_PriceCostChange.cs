using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Pro_PriceCostChange
    {
           private int MethodID;

	    public Pro_PriceCostChange()
	    {
		    this.MethodID = 0;
	    }

        public Pro_PriceCostChange(int methodID)
	    {
            this.MethodID = methodID;
	    }

        /// <summary>
        /// 添加新成本
        /// </summary>
        /// <param name="user"></param>
        /// <param name="headModel"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user,List<Model.CostBill> update_models )
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

                        List<string> classids = new List<string>();
                        foreach (var item in update_models)
                        {
                            var queryc = from p in lqh.Umsdb.Pro_ProInfo
                                         where p.ProID == item.ProID.ToString()
                                         select p;
                            classids.Add(queryc.First().Pro_ClassID.ToString());
                        }
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
                        //    if (!ValidHallIDS.Contains(modesl.HallID))
                        //    {
                        //        return new Model.WebReturn() { ReturnValue = false, Message = "无权操作该仓库" };
                        //    }
                        //}
                        #endregion

                        #region  过滤商品
                        if (ValidProIDS.Count > 0)
                        {
                            foreach (var item in classids)
                            {
                                if (!ValidProIDS.Contains(item))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品无权操作" };
                                }
                            }
                        }
                        #endregion

                        #endregion 

                        string changedid = string.Empty;
                        lqh.Umsdb.OrderMacker(1, "CBJ", "CBJ", ref changedid);
                        if (changedid == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "调价单生成出错" };
                        }
                        //添加表头
                        Model.Pro_PriceCostChange header = new Model.Pro_PriceCostChange();
                        header.SysDate = DateTime.Now;
                        header.UserID =user.UserID;
                        header.ChangedID = changedid;
                       
                        header.Pro_PriceCostChangeList = new System.Data.Linq.EntitySet<Model.Pro_PriceCostChangeList>();
                        header.Pro_PriceCost_InorderList = new System.Data.Linq.EntitySet<Model.Pro_PriceCost_InorderList>();

                    
                        foreach (var child in update_models)
                        {
                            foreach (var item in child.Children)
                            {
                                if (!item.UpdateFlag)   //新增
                                {   
                                    #region   新增

                                    Model.Pro_PriceCostChangeList pp = new Model.Pro_PriceCostChangeList();
                             
                                    pp.ProID = child.ProID;
                                    pp.NewCostPrice = item.NewCostPrice;
                                    pp.OldCostPrice = item.OldCostPrice;
                                    pp.NewRetailPrice = item.RetailPrice;
                                    pp.OldRetailPrice = item.OldRetailPrice;
                                    pp.StartDate = item.StartDate;
                                    pp.EndDate = item.EndDate;
                                    header.Pro_PriceCostChangeList.Add(pp);

                                    #endregion
                                }
                                else
                                {
                                    #region

                                    var que = from list in lqh.Umsdb.Pro_PriceCostChangeList
                                              where list.ID == item.ID
                                              orderby list.ID descending
                                              select list;
                                    if (que.Count() != 0)
                                    {  
                                        //更新   先删除后新增
                                        lqh.Umsdb.Pro_PriceCostChangeList.DeleteOnSubmit(que.First());
                                        Model.Pro_PriceCostChangeList ppc = new Model.Pro_PriceCostChangeList();

                                        ppc.ProID = child.ProID;
                                        ppc.NewCostPrice = item.NewCostPrice;
                                        ppc.OldCostPrice = item.OldCostPrice;
                                        ppc.NewRetailPrice = item.RetailPrice;
                                        ppc.OldRetailPrice = item.OldRetailPrice;
                                        ppc.StartDate = item.StartDate;
                                        ppc.EndDate = item.EndDate;
                                        header.Pro_PriceCostChangeList.Add(ppc);
                                    }
                                    else
                                    {
                                        return new Model.WebReturn() { ReturnValue = false,Message="未找到原数据，数据备份有误"};
                                    }

                                    #endregion 
                                }
                            }
                        }
                       
                       #region   根据时间段更新入库成本

                        foreach (var child in update_models)
                        {
                            foreach (var item in child.Children)
                            {
                                var query = from inorder in lqh.Umsdb.Pro_InOrder
                                            join list in lqh.Umsdb.Pro_InOrderList 
                                            on inorder.ID equals list.Pro_InOrderID

                                            join swaplist in lqh.Umsdb.Pro_InOrderList  //选出类型转换的批次
                                            on list.InListID equals swaplist.InitInListID

                                            where inorder.InDate >= item.StartDate && inorder.InDate <= item.EndDate
                                            && list.ProID == child.ProID
                                            select list;

                                foreach (var item2 in query)
                                {  
                                    item2.Price = item.NewCostPrice;
                                    item2.RetailPrice = item.RetailPrice;
                                    //备份成本改变的批次
                                    Model.Pro_PriceCost_InorderList pp = new Model.Pro_PriceCost_InorderList();

                                    pp.ProID = child.ProID;
                                    pp.InListID = item2.InListID;
                                    pp.OldCost = item.OldCostPrice;
                                    pp.NewCost = item.NewCostPrice;
                                    pp.NewRetailPrice = item.RetailPrice;
                                    pp.OldRetailPrice = item.OldRetailPrice;
                                    header.Pro_PriceCost_InorderList.Add(pp);
                                }
                            }
                        }

                       #endregion 
                       lqh.Umsdb.Pro_PriceCostChange.InsertOnSubmit(header);
                       lqh.Umsdb.SubmitChanges();
                       ts.Complete();
                       return new Model.WebReturn() { ReturnValue =true,Message="保存成功"};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue =false,Message=ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 获取商品成本
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetProCostInfo(Model.Sys_UserInfo user,List<Model.CostBill> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    foreach (var item in models)
                    {
                        if (item.Children != null)
                        {
                            item.Children.Clear();
                        }
                    }
                    foreach (var item in models)
                    {
                        var query = from p in lqh.Umsdb.View_Pro_CostChangeList
                                    where p.ProID== item.ProID
                                    select p;
                        foreach (var child in query)
                        {
                            Model.CostBillChild pc = new Model.CostBillChild();
                            pc.EndDate = Convert.ToDateTime(child.EndDate);
                            pc.NewCostPrice = Math.Round((decimal)child.NewCostPrice,3);
                            pc.CurCostPrice = Math.Round((decimal)child.NewCostPrice, 3);
                            pc.UpdateFlag =Convert.ToBoolean( child.UpdateFlag);
                            pc.OldCostPrice = Math.Round((decimal)child.OldCostPrice, 3);
                            pc.StartDate =Convert.ToDateTime( child.StartDate);
                            pc.OldStartDate = Convert.ToDateTime(child.StartDate);
                             pc.OldEndDate = Convert.ToDateTime(child.EndDate);
                             pc.RetailPrice = child.RetailPrice;
                             pc.OldRetailPrice = child.RetailPrice;
                             pc.ID = child.ID;
                             if (item.Children == null)
                             {
                                 item.Children = new List<Model.CostBillChild>();
                             }
                             item.Children.Add(pc);
                        }

                    }
                
                    return new Model.WebReturn() { ReturnValue = true,Obj=models};
                    
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false,Message= ex.Message};
                }
            }
        }

      
        /// <summary>
        /// 获取未定价商品
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetProNoCostInfo(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //try
                //{
                //    var query = from p in lqh.Umsdb.View_Pro_CostChangeList
                //                where p.CostPrice == 0
                //                select p;
                //    if (query.Count() != 0)
                //    {
                //        ArrayList arr = new ArrayList();
                //        List<string> proids = new List<string>();
                //        foreach (var item in query)
                //        {
                //            proids.Add(item.ProID);
                //        }
                //        arr.Add(proids);
                //        return new Model.WebReturn() { ReturnValue = true, Obj = query.ToList(), ArrList = arr };
                //    }
                //    else
                //    {
                //        return new Model.WebReturn() { ReturnValue = false, Message = "" };
                //    }
                //}
                //catch (Exception ex)
                //{
                  return new Model.WebReturn() { ReturnValue = false, Message = "" };
                //}
            }
        }


        public Model.WebReturn GetTreeInfo(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from v in lqh.Umsdb.View_Pro_NoCostLeftTree
                                select v;
                    return new Model.WebReturn() {Obj=query.ToList(), ReturnValue=true};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 获取成本调价单报表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetCostReport(Model.Sys_UserInfo user,int pageindex,int pagesize)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from c in lqh.Umsdb.View_CostBillReport
                                select c;
                    int total = query.Count();
                    var result = query.Skip(pageindex * pagesize).Take(pagesize);

                    return new Model.WebReturn() { Obj = result.ToList(), ReturnValue = true, ArrList = new ArrayList() {total } };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }
    }
}
