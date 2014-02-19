using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Pro_PriceChange
    {
          private int MethodID;

	    public Pro_PriceChange()
	    {
		    this.MethodID = 0;
	    }

        public Pro_PriceChange(int MenthodID)
	    {
		    this.MethodID = MenthodID; 
	    }

        /// <summary>
        /// 获取左树及商品的所有类别  销售  类别，品牌  型号
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn GetTreeInfo(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {

                    var query = from pc in lqh.Umsdb.View_PriceTypeClassInfo
                                select pc;

                   // string typeid = query.First().TypeID.ToString();

                    List<Model.TreeModel> list = new List<Model.TreeModel>();

                    Model.TreeModel tm = null;

                    var ClassInfo  =( from c in query
                                select new { c.ClassName,c.ClassID}).Distinct();

                    foreach (var item in ClassInfo)
                    {
                            tm = new Model.TreeModel();
                            tm.Name = item.ClassName;
                            tm.ID = item.ClassID.ToString();

                             var typeInfo = (from t in query
                                              where t.ClassID == item.ClassID 
                                              select new {t.TypeName,t.TypeID} ).Distinct();
                             Model.TreeModel c = null;
                             tm.Children = new List<Model.TreeModel>();

                             foreach (var child in typeInfo)
                             {
                                 c = new Model.TreeModel();
                                 c.Name = child.TypeName;
                                 c.ID = child.TypeID.ToString();
                                 tm.Children.Add(c);
                             }
                             list.Add(tm);
                    }

                    var query2 = from pc in lqh.Umsdb.View_ProAllType
                                select pc;
                    ArrayList arr = new ArrayList();
                    arr.Add(query2.ToList());
                    return new Model.WebReturn() { ReturnValue =true,Obj = list,ArrList=arr};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue =false,Message = ex.Message};
                }
            }
        }

        /// <summary>
        /// 获取已定价的商品
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn GetPriceProductInfo(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = (from s in lqh.Umsdb.Pro_SellTypeProduct
                                 select new { s.ProID}).Distinct();
                    List<string> list = new List<string>();
                    foreach (var item in query)
                    {
                        list.Add(item.ProID);
                    }
                    return new Model.WebReturn() { ReturnValue = true, Obj = list, Message = "" };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 获取商品的单卖价格
        /// </summary>
        /// <param name="user"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn GetPrice(Model.Sys_UserInfo user, List<Model.BAduitModel> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    List<string> proids = new List<string>();
                    foreach(var child in models)
                    {
                        proids.Add(child.ProID);
                    }
                    var price = from a in lqh.Umsdb.View_SingleSellPrice
                             where proids.Contains(a.ProID)
                                select a;

                    List<decimal> prices = new List<decimal>();

                    foreach (var item in price)
                    {
                        foreach (var child in models)
                        {
                            if (child.ProID == item.ProID)
                            {
                                child.ProPrice = item.Price;
                                break;
                            }
                        }
                    }

                    return new Model.WebReturn() { ReturnValue = true, Obj = models };

                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message=ex.Message };
                }
            }

        }


        /// <summary>
        /// 获取商品所有销售价格
        /// </summary>
        /// <param name="user"></param>
        /// <param name="proids"></param>
        /// <returns></returns>
        public Model.WebReturn GetPriceListInfo(Model.Sys_UserInfo user, List<Model.PriceBill> pros)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    foreach (var child in pros)
                    {
                        if (child.IsMainPro)
                        {
                            continue;
                        }
                        var query = from s in lqh.Umsdb.Pro_SellTypeProduct
                                    join t in lqh.Umsdb.Pro_SellType
                                    on s.SellType equals t.ID
                                    where s.ProID == child.ProID
                                    select new
                                    {
                                        s.ID,
                                        s.SellType,
                                        s.ProID,
                                        s.ProCost,
                                        s.Price,
                                        s.MinPrice,
                                        s.MaxPrice,
                                        s.LowPrice,
                                        s.IsTicketUseful,
                                        s.IsAduit,
                                        t.Name
                                    };
                        child.Children = new List<Model.PriceBillChild>();
                        foreach (var item in query)
                        {
                            Model.PriceBillChild pc = new Model.PriceBillChild();
                            pc.Price =Math.Round( item.Price,4);
                            pc.OldPrice = Math.Round(item.Price, 4);
                            pc.SellTypeID = (int)item.SellType;
                            pc.SellTypeName = item.Name;
                            pc.MinPrice =Math.Round( item.MinPrice,4);
                            pc.OldMinPrice = Math.Round(item.MinPrice, 4);
                            pc.MaxPrice = Math.Round( item.MaxPrice,4);
                            pc.OldMaxPrice = Math.Round(item.MaxPrice, 4);
                            pc.LowPrice =Math.Round(item.LowPrice,4);
                            pc.OldLowPrice = Math.Round(item.LowPrice, 4);
                            pc.HasPrice = true;
                            pc.IsTicketUseful = item.IsTicketUseful;
                            pc.OldIsTicketUseful = item.IsTicketUseful;
                            pc.IsAduit = item.IsAduit;
                            pc.OldIsAduit = item.IsAduit;
                            pc.ID = item.ID;
                           

                            child.Children.Add(pc);
                        }
                    }

                    return new Model.WebReturn() { ReturnValue = true, Obj = pros };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn(){ReturnValue=false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 添加调价单
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, List<Model.PriceBill> models, List<Model.PriceBill> returnModels)
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

                        #region 权限验证

                        List<string> classids = new List<string>();
                        foreach (var item in models)
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
                        #endregion

                        string changeid = "";
                        lqh.Umsdb.OrderMacker(1, "LSJ", "LSJ", ref changeid);
                        if (string.IsNullOrEmpty(changeid))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "调价单生成出错" };
                        }
                        //添加表头
                        Model.Pro_PriceChange ppc = new Model.Pro_PriceChange();
                        ppc.ChangeID = changeid;
                        ppc.SysDate = DateTime.Now;
                        ppc.UserID = user.UserID;
                        ppc.Pro_PriceChangeList = new System.Data.Linq.EntitySet<Model.Pro_PriceChangeList>();


                        List<Model.Pro_SellTypeProduct> adds = new List<Model.Pro_SellTypeProduct>();

                        foreach (var item in models)
                        {
                            foreach(var child in item.Children)
                            {
                                if (item.IsMainPro)  //若是总商品   则更新总商品下的所有同一销售类别的商品价格
                                {
                                    var prolist= from a in lqh.Umsdb.Pro_ProInfo
                                          where a.ProMainID == item.ProMainID
                                          select a;

                                    #region  更新

                                    if (child.HasPrice) //若是更新
                                    {
                                        foreach (var xxd in prolist)
                                        {
                                            var pros = from a in lqh.Umsdb.Pro_SellTypeProduct
                                                       where a.ProID == xxd.ProID && a.SellType == child.SellTypeID
                                                       select a;
                                          
                                            if (pros.Count() == 0)
                                            {
                                                //不存在则新增
                                                AddSellTypePro(ppc,adds, child,xxd.ProID);
                                            }
                                            else
                                            {
                                                Model.Pro_SellTypeProduct pst = pros.First();
                                                UpdateSellTypePro(ppc, item, child, pst);
                                            }
                                          
                                        }
                                    }
                                    #endregion

                                    #region 新增
                                    else
                                    {
                                        foreach (var xxd in prolist)
                                        {
                                            var pros = from a in lqh.Umsdb.Pro_SellTypeProduct
                                                       where a.ProID == xxd.ProID && a.SellType == child.SellTypeID
                                                       select a;

                                            if (pros.Count() == 0)
                                            {
                                                //不存在则新增
                                                AddSellTypePro(ppc,adds, child, xxd.ProID);
                                            }
                                            else
                                            {
                                                //存在则更新
                                                UpdateSellTypePro(ppc, item, child,pros.First());
                                            }
                                        }
                                    }
                                    #endregion

                                }
                                else               //若不是总商品 则只更新指定销售类别的商品
                                {
                                    var que = from p in lqh.Umsdb.Pro_SellTypeProduct
                                          where p.ProID == item.ProID 
                                          && p.SellType == child.SellTypeID
                                          select p;

                                    #region  更新

                                    if (child.HasPrice) //若是更新
                                    {
                                        if (que.Count() == 0)
                                        {
                                            return new Model.WebReturn() { ReturnValue=false,Message="未能找到指定数据，保存失败！"};
                                        }
                                        foreach (var xxd in que)
                                        {
                                            UpdateSellTypePro(ppc, item, child, xxd);
                                        }
                                    }
                                    #endregion

                                    #region 新增
                                    else
                                    {
                                        if (que.Count() > 0)
                                        {
                                            //若已经存在则更新
                                            foreach (var xxd in que)
                                            {
                                                UpdateSellTypePro(ppc, item, child, xxd);
                                            }
                                        }
                                        else
                                        {
                                            //否则新增

                                            AddSellTypePro(ppc,adds, child, item.ProID);
                                           
                                        }
                                       
                                    }
                                    #endregion
                                }
                            }
                        }
                        #region 提交数据  
                        lqh.Umsdb.Pro_SellTypeProduct.InsertAllOnSubmit(adds);
                        lqh.Umsdb.Pro_PriceChange.InsertOnSubmit(ppc);
                        lqh.Umsdb.SubmitChanges();

                        #endregion 

                        #region  添加成功后返回新数据

                        foreach (var child in returnModels)
                        {
                            if (child.IsMainPro)
                            {
                                continue;
                            }
                            var query = from s in lqh.Umsdb.Pro_SellTypeProduct
                                        join t in lqh.Umsdb.Pro_SellType
                                        on s.SellType equals t.ID
                                        where s.ProID == child.ProID
                                        select new
                                        {
                                            s.ID,
                                            s.SellType,
                                            s.ProID,
                                            s.ProCost,
                                            s.Price,
                                            s.MinPrice,
                                            s.MaxPrice,
                                            s.LowPrice,
                                            s.IsTicketUseful,
                                            s.IsAduit,
                                            t.Name
                                        };
                            child.Children = new List<Model.PriceBillChild>();
                            foreach (var item in query)
                            {
                                Model.PriceBillChild pc = new Model.PriceBillChild();
                                pc.Price = Math.Round(item.Price, 4);
                                pc.OldPrice = Math.Round(item.Price, 4);
                                pc.SellTypeID = (int)item.SellType;
                                pc.SellTypeName = item.Name;
                                pc.MinPrice = Math.Round(item.MinPrice, 4);
                                pc.OldMinPrice = Math.Round(item.MinPrice, 4);
                                pc.MaxPrice = Math.Round(item.MaxPrice, 4);
                                pc.OldMaxPrice = Math.Round(item.MaxPrice, 4);
                                pc.LowPrice = Math.Round(item.LowPrice, 4);
                                pc.OldLowPrice = Math.Round(item.LowPrice, 4);
                                pc.HasPrice = true;
                                pc.IsTicketUseful = item.IsTicketUseful;
                                pc.OldIsTicketUseful = item.IsTicketUseful;
                                pc.IsAduit = item.IsAduit;
                                pc.OldIsAduit = item.IsAduit;
                                pc.ID = item.ID;

                                child.Children.Add(pc);
                            }
                        }

                        #endregion 

                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "保存成功", Obj = returnModels };
                        
                      
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                    }
                }
            }
        }

        private static void UpdateSellTypePro(Model.Pro_PriceChange ppc, Model.PriceBill item, Model.PriceBillChild child, Model.Pro_SellTypeProduct pst)
        {
            //备份
            Model.Pro_PriceChangeList list = new Model.Pro_PriceChangeList();
            list.IsAduit = pst.IsAduit;
            list.IsTicketUseful = pst.IsTicketUseful;
            list.LowPrice = pst.LowPrice;
            list.MaxPrice = pst.MaxPrice;
            list.MinPrice = pst.MinPrice;
            list.Price = pst.Price;
            list.ProID = item.ProID;
            list.SellType = child.SellTypeID;
            ppc.Pro_PriceChangeList.Add(list);

            //更新
            pst.Price = child.Price;
            pst.IsTicketUseful = child.IsTicketUseful;
            pst.LowPrice = child.LowPrice;
            pst.MaxPrice = child.MaxPrice;
            pst.MinPrice = child.MinPrice;
            pst.IsAduit = child.IsAduit;
        }

        private void AddSellTypePro(Model.Pro_PriceChange ppc, List<Model.Pro_SellTypeProduct> adds, Model.PriceBillChild child, string proid)
        {
            //备份
            Model.Pro_PriceChangeList list = new Model.Pro_PriceChangeList();
            list.IsAduit =false;
            list.IsTicketUseful = false;
            list.LowPrice = 0;
            list.MaxPrice = 0;
            list.MinPrice =0;
            list.Price = 0;
            list.ProID = proid;
            list.SellType = child.SellTypeID;
            ppc.Pro_PriceChangeList.Add(list);

            Model.Pro_SellTypeProduct ps = new Model.Pro_SellTypeProduct();
            ps.ProID = proid;
            ps.IsTicketUseful = child.IsTicketUseful;
            ps.LowPrice = child.LowPrice;
            ps.MaxPrice = child.MaxPrice;
            ps.MinPrice = child.MinPrice;
            ps.IsAduit = child.IsAduit;
            ps.Price = child.Price;
            ps.SellType = child.SellTypeID;
            adds.Add(ps);
        }

        /// <summary>
        /// 获取零售调价单报表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetPriceReport(Model.Sys_UserInfo user,int pageindex,int pagesize)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from price in lqh.Umsdb.View_PriceBillReport
                                select price;
                    int total = query.Count();
                    var result = query.Skip(pageindex * pagesize).Take(pagesize);


                    return new Model.WebReturn() { ReturnValue = true, Obj = result.ToList(), ArrList = new ArrayList() { total} };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }
    }
}
