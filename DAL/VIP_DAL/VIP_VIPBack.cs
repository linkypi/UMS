using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL.VIP_DAL
{
   public partial class VIP_VIPBack
    {
       public VIP_VIPBack() { }
#region Method
       
        /// <summary>
        /// 提交审核申请
        /// </summary>
        public Model.WebReturn Apply(Model.VIP_VIPBackAduit model)
        {
            string Msg="";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    try
                    {
                        Model.VIP_Renew  renew=new Model.VIP_Renew();
                        var queryTC=from b in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                                    where b.ID== model.VIP_ID
                                    select b;
                        if(queryTC.Count()==0)
                        {
                            Msg = "会员" + model.ID + "不存在";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                         var queryTC1=from b in lqh.Umsdb.GetTable<Model.VIP_VIPBackAduit>()
                                      where b.VIP_ID == model.VIP_ID
                                    select b;
                        if(queryTC1.Count()>0)
                        {
                            Msg = "该会员已申请退卡";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                         Model.VIP_VIPInfo vip=queryTC.First();
                       var queryCB=from b in lqh.Umsdb.GetTable<Model.Pro_SellListInfo>()
                                   where Convert.ToInt32(b.SellID)==vip.SellID
                                   select b;
                       Model.Pro_SellListInfo first=queryCB.First();
                        if(model.Money>first.CashPrice)
                        {
                               Msg = "金额超出了额定退卡金额！";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }

                        lqh.Umsdb.VIP_VIPBackAduit.InsertOnSubmit(model);                                   
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = true, Message = "申请成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = false };
                        throw ex;
                    }
                }
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        public Model.WebReturn Audited(Model.VIP_VIPBackAduit model)
        {
            string Msg = "";

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var querySH=from b in lqh.Umsdb.GetTable<Model.VIP_VIPBackAduit>()
                                    where b.AduitID== model.AduitID
                                    select b;
                        if(querySH.Count()==0)
                        {
                            Msg = "审核单" + model.ID + "不存在";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        Model.VIP_VIPBackAduit first = querySH.First();
                        if (first.Aduited == true)
                        {
                            Msg = "申请 " + model.ID + " 已经审核 ";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        first.Aduited = true;
                        lqh.Umsdb.VIP_VIPBackAduit.InsertOnSubmit(model);     
                        lqh.Umsdb.SubmitChanges();

                        ts.Complete();
                        return new Model.WebReturn() { Obj = true, Message = "审核成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn { Obj = false };
                        throw ex;
                    }

                }

            }
        }
        /// <summary>
        /// 退卡
        /// </summary>
        public Model.WebReturn Add(Model.VIP_VIPBack model)
        {
            string Msg = "";

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var queryXQ = from b in lqh.Umsdb.GetTable<Model.VIP_VIPBack>()
                                      where b.ID == model.ID
                                      select b;
                        if (queryXQ.Count() > 0)
                        {
                            Msg = "该卡" + model.ID + "已退";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }                    
                        var query = from b in lqh.Umsdb.GetTable<Model.VIP_VIPBackAduit>()
                                    where b.AduitID==model.AduitID
                                    select b;                      
                        if (query.Count() == 0)
                        {
                            Msg = "审批单不存在";
                            return new Model.WebReturn { Obj = true, Message = Msg };
                        }
                        Model.VIP_VIPBackAduit renewAduit = query.First();
                        if (renewAduit.Used==true)
                        {
                             Msg = "审批" + model.ID + " 已过时";
                                return new Model.WebReturn { Obj = true, Message = Msg };
                        }
                        if (renewAduit.Aduited==false)
                        {
                                Msg = "审批单" + model.ID + "未审核";
                                return new Model.WebReturn { Obj = true, Message = Msg };
                         }
                        if (renewAduit.Passed != true)
                        {
                            Msg = "审批" + model.ID + "未通过";
                            return new Model.WebReturn { Obj = true, Message = Msg };
                        }
                        renewAduit.Used = false;
                        var queryHY=from b in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                                    where b.ID==model.VIP_ID
                                    select b;
                        Model.Pro_IMEI_Deleted delete=new Model.Pro_IMEI_Deleted();
                        Model.VIP_VIPInfo vip=queryHY.First();
                        vip.Flag=false;
                        delete.IMEI=Convert.ToString(model.VIP_ID);
                        lqh.Umsdb.VIP_VIPBack.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = true, Message = "退卡成功" };
                        }
                                                                  
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = false };
                        throw ex;
                    }
                }

            }
        }
    
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.WebReturn GetModel(int Id)
        {

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {                 
                    try
                    {
                        IQueryable<Model.VIP_VIPBack> hall = from p in lqh.Umsdb.GetTable<Model.VIP_VIPBack>()
                                                                   where p.ID == Id
                                                                   select p;


                       Model.VIP_VIPBack firstHall = hall.First();
                        
                        //firstHall.HallName = model.HallName;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { Obj = firstHall, Message = "获取成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null};
                        throw ex;
                    }

                }

            }
        }
      
#endregion
    }
}
