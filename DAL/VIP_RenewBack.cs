using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class VIP_RenewBack
    {
          private int MenthodID;

	    public VIP_RenewBack()
	    {
		    this.MenthodID = 0;
	    }

        public VIP_RenewBack(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }

        public Model.WebReturn Add(Model.Sys_UserInfo user,Model.VIP_RenewBack model)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var query = from b in lqh.Umsdb.VIP_RenewBackAduit
                                    where b.ID == model.AduitID
                                    select b;
                        if (query.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "不存在取消须期审批" };
                        }

                        #region "验证用户操作仓库  商品的权限 "
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MenthodID, ValidHallIDS, ValidProIDS,lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        //var queryvip = from v in lqh.Umsdb.VIP_VIPInfo
                        //               join rn in lqh.Umsdb.VIP_Renew
                        //               on v.ID equals rn.VIP_ID
                        //               join rb in lqh.Umsdb.VIP_RenewBack
                        //               on rn.ID equals rb.Old_Renew_ID
                        //               where rb.Old_Renew_ID == query.First().ReNewID
                        //               select v;
                        //if (ValidHallIDS.Count>0&& !ValidHallIDS.Contains(queryvip.First().HallID))
                        //{
                        //    var que = from h in lqh.Umsdb.Pro_HallInfo
                        //              where h.HallID == queryvip.First().HallID
                        //              select h;
                        //    return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                        //}
                        #endregion

                
                        #region 判断审批单的状态
                        Model.VIP_RenewBackAduit BackAduit = query.First();
                        if (BackAduit.Money == null && BackAduit.Point == null)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "无审批内容！" };
                        }
                        if (BackAduit.Money>0 && BackAduit.Point >0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "审批内容有误！" };
                        }

                        if (BackAduit.Validity == null || BackAduit.Validity <= 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "不存在续期！" };
                        }
                        if (BackAduit.Aduited != true)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "未审批！" };
                        }
                        if (BackAduit.Passed != true)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "审批未通过！" };
                        }
                        if (BackAduit.Used == true)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "审批单已使用！" };
                        }
                        #endregion 
                        model.Money = BackAduit.Money;
                        model.Old_Renew_ID = BackAduit.ReNewID;
                        model.NewDate = BackAduit.NewDate;
                        model.Point = BackAduit.Point;
                        model.SysDate = DateTime.Now;
                        model.Validity = BackAduit.Validity;                    
                        #region 验证取消是否超时

                        var valTime = from m in lqh.Umsdb.Sys_MethodInfo
                                      where m.MethodID == MenthodID
                                      select m;
                        int count = Convert.ToInt32(valTime.First().Validity.ToString());
                        if (count != 0)
                        {
                            var intime = from renew in lqh.Umsdb.VIP_Renew
                                         where renew.ID == model.Old_Renew_ID
                                         select renew;
                            DateTime time = DateTime.Parse(intime.First().RenewDate.ToString());


                            TimeSpan dateDiff = DateTime.Now.Subtract(time);
                            if (dateDiff.Hours > count)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "取消续期超时" };
                            }
                        }

                        #endregion 

                        lqh.Umsdb.VIP_RenewBack.InsertOnSubmit(model);

                        var queryVIP = from b in lqh.Umsdb.VIP_VIPInfo
                                       where b.ID == BackAduit.VIP_ID
                                       select b;
                        if (queryVIP.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "不存在会员！" };
                        }
                        Model.VIP_VIPInfo NewVIP = queryVIP.First();
                        //如是取消积分续期   
                        if (model.Point>0)
                        {                        
                            NewVIP.Point += model.Point;                         
                        }
             
                        NewVIP.Validity -= model.Validity;
                        int Days = (int)model.Validity;
                        TimeSpan Span = new TimeSpan(Days, 0, 0, 0);
                        NewVIP.EndTime = NewVIP.EndTime.Value.Subtract(Span);
                        BackAduit.Used = true;
                        #region 成功返回数据
                        var query_View = (from b in lqh.Umsdb.View_VIP_RenewBackAduit
                                          where b.VIP_ID == NewVIP.ID
                                          select b).ToList();
                        if (query_View.Count() == 0)
                        {
                            return new Model.WebReturn() {  ReturnValue = false, Message = "返回失败" };
                        }
                        #endregion
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        //ArrayList arr = new ArrayList();
                        //arr.Add(vinfo.Validity);
                        //arr.Add(vinfo.Point);

                        return new Model.WebReturn() { Obj=query_View.First(),ReturnValue=true,Message="取消成功！"};

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() {ReturnValue=false };
                    }
                }
            }
        }

        /// <summary>
        /// 获取取消续期审批单信息
        /// </summary>
        /// <param name="aduitID"></param>
        /// <returns></returns>
        public Model.WebReturn GetAduitInfo(Model.Sys_UserInfo user, string aduitID, int vipid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var query = from aduit in lqh.Umsdb.VIP_RenewBackAduit
                                    where aduit.AduitID == aduitID && aduit.VIP_ID == vipid
                                    select aduit;

                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单不存在" };
                        }
                        bool flag = bool.Parse(query.First().Aduited.ToString());
                        if (!flag)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单未审批" };
                        }
                        flag =bool.Parse( query.First().Passed.ToString());
                        if (!flag)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批不通过" };
                        }

                        Model.RenewModel model = new Model.RenewModel();

                        if (string.IsNullOrEmpty(query.First().Money.ToString()))
                        {
                            model.AduitMoney = 0;
                        }
                        else
                        {
                            model.AduitMoney = decimal.Parse(query.First().Money.ToString());
                        }

                        if (string.IsNullOrEmpty(query.First().Validity.ToString()))
                        {
                            model.AduitValidity = 0;
                        }
                        else
                        {
                            model.AduitValidity = int.Parse(query.First().Validity.ToString());
                        }
                        model.OldRenewID =int.Parse( query.First().ReNewID.ToString());
                        model.AduitID =query.First().ID;
                        model.Flag = true;
                        return new Model.WebReturn() { ReturnValue=true,Obj=model};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                    }
                }
            }
        }
    }
}
