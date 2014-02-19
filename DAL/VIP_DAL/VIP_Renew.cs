using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL.VIP_DAL
{
   public partial class VIP_Renew
    {  
       public VIP_Renew() { }
#region Method
        public Model.WebReturn Add(Model.VIP_Renew model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    try
                    {

                        lqh.Umsdb.VIP_Renew.InsertOnSubmit(model);
                        Model.VIP_VIPInfo vip=new Model.VIP_VIPInfo();
                        vip.Validity += model.Validity;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = true, Message = "续期成功" };
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
        /// 提交审核申请
        /// </summary>
        public Model.WebReturn Apply(Model.VIP_RenewBackAduit model)
        {
            string Msg="";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    try
                    {
                        Model.VIP_Renew  renew=new Model.VIP_Renew();
                        var queryYZ=from b in lqh.Umsdb.GetTable<Model.VIP_Renew>()
                                    where b.AduitID== Convert.ToInt32(model.AduitID)
                                    select b;
                        if(queryYZ.Count()>0)
                        {
                            Msg = "申请" + model.ID + "已存在";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        Model.VIP_Renew first=queryYZ.First();
                        DateTime now=DateTime.Now;
                        DateTime renewdate=(DateTime)first.RenewDate;
                        TimeSpan a= now.Subtract(renewdate);
                        TimeSpan span=new TimeSpan(24,0,0);
                        if(TimeSpan.Compare(a,span)>0)
                        {
                               Msg = "超过24小时，无法提出申请！";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        if(model.Money >first.RenewMoney)
                        {
                              Msg = "返还金额超过续期金额！";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                         if(model.Validity >first.Validity)
                        {
                              Msg = "返还天数超过续期天数！";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        lqh.Umsdb.VIP_RenewBackAduit.InsertOnSubmit(model);                                   
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
        public Model.WebReturn Audited(Model.VIP_RenewBackAduit model)
        {
            string Msg = "";

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.VIP_Renew  renew=new Model.VIP_Renew();
                        var querySH=from b in lqh.Umsdb.GetTable<Model.VIP_RenewBackAduit>()
                                    where b.AduitID== model.AduitID
                                    select b;
                        if(querySH.Count()==0)
                        {
                            Msg = "审核单" + model.ID + "不存在";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        Model.VIP_RenewBackAduit first = querySH.First();
                        if (first.Aduited == true)
                        {
                            Msg = "申请 " + model.ID + " 已经审核 ";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        lqh.Umsdb.VIP_RenewBackAduit.InsertOnSubmit(model);    
                        first.Aduited = true;                         
                        lqh.Umsdb.SubmitChanges();

                        ts.Complete();

                        return new Model.WebReturn() { Obj = true, Message = "更新成功" };
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
        /// 
        /// </summary>
        public Model.WebReturn Update(Model.VIP_RenewBack model)
        {
            string Msg = "";

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var queryXQ = from b in lqh.Umsdb.GetTable<Model.VIP_RenewBack>()
                                      where b.ID == model.ID
                                      select b;
                        if (queryXQ.Count() > 0)
                        {
                            Msg = "续期" + model.ID + "已取消过了";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                       Model.VIP_Renew renew=new Model.VIP_Renew();
                        var query = from b in lqh.Umsdb.GetTable<Model.VIP_RenewBackAduit>()
                                    where Convert.ToInt32(b.AduitID)==renew.AduitID
                                    select b;
                      
                        if (query.Count() == 0)
                        {
                            Msg = "审批单不存在";
                            return new Model.WebReturn { Obj = true, Message = Msg };
                        }
                        Model.VIP_RenewBackAduit renewAduit=query.First();
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
                        lqh.Umsdb.VIP_RenewBack.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = true, Message = "更新成功" };
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
                        IQueryable<Model.VIP_Renew> hall = from p in lqh.Umsdb.GetTable<Model.VIP_Renew>()
                                                                   where p.ID == Id
                                                                   select p;


                       Model.VIP_Renew firstHall = hall.First();
                        
                       
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
    
        /// <summary>
        /// 删除一条数据
        /// </summary>
      
#endregion
    }
}

