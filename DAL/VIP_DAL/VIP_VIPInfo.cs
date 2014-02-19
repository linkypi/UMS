using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL.VIP_DAL
{
    public partial class VIP_VIPInfo
    {
        
        public VIP_VIPInfo() { }

#region Method
        public Model.WebReturn Add(Model.VIP_VIPInfo model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    try
                    {
                        lqh.Umsdb.VIP_VIPInfo.InsertOnSubmit(model);
                        Model.VIP_VIPInfo vip=new Model.VIP_VIPInfo();
                        //vip.Pro_SellInfo.AddRange(model.Pro_SellInfo);                      
                        //vip.Pro_IMEI.AddRange(model.Pro_IMEI);
                        //vip.VIP_VIPService.AddRange(vip.VIP_VIPService);                       
                        /*在此处填写减库存，消串号代码
                          XS_Operate(model.ID)           
                        */
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = true, Message = "新增成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = false };
                        throw ex;
                    }
                }
            }
        }
        public Model.WebReturn Update(Model.VIP_VIPInfo model)
        {
            string Msg = "";

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var queryZC = from b in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                                      where b.ID == model.ID
                                      select b;
                        if (queryZC.Count() == 0)
                        {
                            Msg = "会员" + model.ID + " 不存在";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        else
                        {
                            Model.VIP_VIPInfo_BAK vip_bak = (Model.VIP_VIPInfo_BAK)queryZC;
                            Model.VIP_VIPInfo vip = model;
                                                     
                        }
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
                        IQueryable<Model.VIP_VIPInfo> hall = from p in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                                                                   where p.ID == Id
                                                                   select p;


                       Model.VIP_VIPInfo firstHall = hall.First();
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
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public Model.WebReturn  Delete(Model.VIP_VIPInfo model)
        {

            string Msg = "";

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var query = from b in  lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                                    where b.ID == model.ID
                                    select b;
                        if (query.Count() == 0)
                        {
                            Msg = "删除的会员 " + model.ID + " 不存在";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        else
                        {
                            Model.VIP_VIPInfo vip = new Model.VIP_VIPInfo();
                            vip = model;
                        }
                        lqh.Umsdb.SubmitChanges();

                        ts.Complete();

                        return new Model.WebReturn() { Obj = true, Message = "提交成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = false };
                        throw ex;
                    }

                }

            }
        }
#endregion
    }
}
