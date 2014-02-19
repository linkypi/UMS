using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class SMS_SellBack
    {
           private int MethodID;

           public SMS_SellBack()
        {
            this.MethodID = 0;
        }

           public SMS_SellBack(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }


        /// <summary>
        /// 310
        /// </summary>
        /// <param name="user"></param>
        /// <param name="aduitid"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user,string aduitid,string note)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var aduit = from a in lqh.Umsdb.SMS_SellBackAduit
                                    where a.AduitID == aduitid
                                    select a;
                        if (aduitid.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue =false,Message="未能找到指定审批单！"};
                        }
                        aduit.First().Used = true;
                        aduit.First().UseDate = DateTime.Now;
                        Model.SMS_SellBackAduit model = aduit.First();


                        var sinfo = from a in lqh.Umsdb.SMS_SignInfo
                                    where a.ID == model.SignID
                                    select a;
                        if (sinfo.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定合同信息！" };
                        }
                       Model. SMS_SignInfo sms = sinfo.First();

                        Model.SMS_SellBack back = new Model.SMS_SellBack();
                        back.AduitID = aduitid;
                        back.BackCount = model.ApplyCount;
                        back.BackMoney = (decimal)model.ApplyMoney;
                        back.BillID = sinfo.First().OldSellID;
                        back.CusName = model.CusName;
                        back.CusPhone = model.CusPhone;
                        back.Note = note;
                       
                        string msg = null;
                        lqh.Umsdb.OrderMacker(1, "SMSB", "SMSB", ref msg);
                        if (msg == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单生成出错！" };
                        }
                        back.SellBackID = msg;
                        back.SellID = (int)model.SignID;
                        back.SysDate = DateTime.Now;
                        back.UserID = user.UserID;
                        lqh.Umsdb.SMS_SellBack.InsertOnSubmit(back);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "退货成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue  =false,Message=ex.Message};   
                    }
                }

            }
        }
    }
}
