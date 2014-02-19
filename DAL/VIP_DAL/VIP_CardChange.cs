using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL.VIP_DAL
{
    public partial class VIP_CardChange
    {
        public VIP_CardChange() { }             
        /// <summary>
        /// 换卡
        /// </summary>
        public Model.WebReturn add(Model.VIP_VIPInfo model)
        {
            string Msg = "";

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        //退卡
                        var query = from b in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                                    where b.IDCard_ID == model.IDCard_ID
                                    select b;
                        if (query.Count() == 0)
                        {
                            Msg = "该会员不存在,无法进行换卡操作";
                            return new Model.WebReturn { Obj = true, Message = Msg };
                        }

                        Model.VIP_VIPInfo vip = query.First(); 
                        vip.Flag = false;
                        Model.Pro_IMEI_Deleted delete=new Model.Pro_IMEI_Deleted();
                        delete.IMEI=Convert.ToString(vip.ID);
                        //注册
                        Model.VIP_CardChange change= new Model.VIP_CardChange();
                        change.OLD_VIP_ID = vip.ID;
                        change.NEW_VIP_ID = model.ID;
                        change.UserID = model.UpdUser;
                        //change.VIP_VIPInfo = model;                        
                        change.SellID = model.SellID;
                        change.SysDate = model.SysDate;
                       // change.Pro_SellInfo=model.Pro_SellInfo
                       // change.Sys_UserInfo = model.Sys_UserInfo;
                        lqh.Umsdb.VIP_VIPInfo.InsertOnSubmit(model);
                       

                        lqh.Umsdb.VIP_CardChange.InsertOnSubmit(change);                
                        
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = true, Message = "换卡成功" };
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
                        IQueryable<Model.VIP_CardChange> hall = from p in lqh.Umsdb.GetTable<Model.VIP_CardChange>()
                                                                   where p.ID == Id
                                                                   select p;


                       Model.VIP_CardChange firstHall = hall.First();
                        
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


    }
}
