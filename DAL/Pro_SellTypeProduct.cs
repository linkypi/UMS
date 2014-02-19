using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Pro_SellTypeProduct 
    {
        private int MenthodID;

	    public Pro_SellTypeProduct()
	    {
		    this.MenthodID = 0;
	    }

        public Pro_SellTypeProduct(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }


        public List<Model.Pro_SellTypeProduct> GetModel()
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.Pro_SellTypeProduct
                                select b;
                    if (query == null || query.Count() == 0)
                    {
                        return null;
                    }             
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取结算价
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetLowPriceList(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    List<Model.LowPriceModel> models = new List<Model.LowPriceModel>();

                    var query = from b in lqh.Umsdb.View_LowPriceList
                                select b;
                    foreach (var item in query)
                    {
                        if (!Exist(item.ProID, models))
                        {
                            Model.LowPriceModel m = new Model.LowPriceModel();
                            m.ProID = item.ProID;
                            m.ProFormat = item.ProFormat;
                            m.ProName = item.ProName;
                            m.TypeName = item.TypeName;
                            m.ClassID = item.ClassID.ToString();
                            m.ClassName = item.ClassName;
                            m.Children = new List<Model.LPMChildren>();

                            var que = from p in query
                                      where p.ProID == item.ProID
                                      select p;
                            foreach (var child in que)
                            {
                                Model.LPMChildren lpc = new Model.LPMChildren();
                                lpc.LowPrice = (double)child.LowPrice;
                                lpc.CurrentLowPrice = (double)child.LowPrice;
                                lpc.SellType = child.SellTypeName;
                                lpc.SellTypeID = (int)child.SellType;
                                m.Children.Add(lpc);
                            }
                            models.Add(m);
                        }
                    }
               
                    return new Model.WebReturn() { ReturnValue=true,Obj = models};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue  =false,Message = ex.Message};
                }
            }
        }

        private bool Exist(string pid, List<Model.LowPriceModel> models)
        {
            foreach (var item in models)
            {
                if (pid == item.ProID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///保存结算价
        /// </summary>
        /// <param name="user"></param>
        /// <param name="updateModels"></param>
        /// <param name="newModels"></param>
        /// <returns></returns>
        public Model.WebReturn SaveLowPrice(Model.Sys_UserInfo user, List<Model.LowPriceModel> updateModels)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope t = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        {
                            return result;
                        }
                        foreach(var item in updateModels)
                        {
                            foreach (var child in item.Children)
                            {
                                var stp = from s in lqh.Umsdb.Pro_SellTypeProduct
                                          where s.ProID == item.ProID && s.SellType == child.SellTypeID
                                          select s;
                                if (stp.Count() == 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "数据库中找不到要更新的数据" };
                                }
                                stp.First().LowPrice = (decimal)child.CurrentLowPrice;
                            }
                        }
                        lqh.Umsdb.SubmitChanges();
                        return new Model.WebReturn() { ReturnValue = true};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message = ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 根据商品编码获取单卖价格  用途： 售后维修  323
        /// </summary>
        /// <param name="user"></param>
        /// <param name="proid"></param>
        /// <returns></returns>
        public Model.WebReturn GetSunglePriceByProID(Model.Sys_UserInfo user,List<string> proids)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var list = (from a in lqh.Umsdb.Pro_SellTypeProduct
                               join b in lqh.Umsdb.Pro_SellType
                               on a.SellType equals b.ID
                               where proids.Contains(a.ProID) && b.Name == "单卖"
                               select a).Distinct();
                    List<Model.Pro_SellTypeProduct> stp = new List<Model.Pro_SellTypeProduct>();
                    if (list.Count() > 0)
                    {
                        stp = list.ToList();
                    }

                    return new Model.WebReturn() { ReturnValue = true ,Obj=stp};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue =false,Message=ex.Message};   
                }
            }
        }


        /// <summary>
        /// 获取当前结算价报表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetLowPriceReport(Model.Sys_UserInfo user,int pageindex,int pagesize)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from r in lqh.Umsdb.View_LowPriceList
                                select r;
                    int total = query.Count();

                    var result = query.Skip(pageindex * pagesize).Take(pagesize);
                    return new Model.WebReturn() { ReturnValue = true, Obj = result.ToList(), ArrList = new System.Collections.ArrayList() { total} };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() {ReturnValue = false,Message =ex.Message };
                }
            }

        }
    }
}
