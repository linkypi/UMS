using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 续期
    /// </summary>
    public class VIP_Renew
    {
        private int MethodID;

        public VIP_Renew()
        {
            this.MethodID = 0;
        }

        public VIP_Renew(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        #region 新增续期
        /// <summary>
        /// 续期
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.VIP_Renew model)
        {
            //有效期都是天
            //续期的方式 配置表 Sys_Option 金额续期比列 积分续期比列  (接口)
            //更新有效期 VIP_VIPInfo 
            //返回
            if (model == null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    try
                    {
                        #region 验证信息的有效性
                        if (model.Point == null && model.RenewMoney == null)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
                        }
                        if (model.Point > 0 && model.RenewMoney > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "两种续期方式不能同时进行！" };
                        }
                        var query_VIP = from b in lqh.Umsdb.VIP_VIPInfo
                                        where b.ID == model.VIP_ID
                                        select b;
                        if (query_VIP.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该会员查找失败！" };
                        }
                        Model.VIP_VIPInfo NewVIP = query_VIP.First();
                        #endregion
                        #region 现金续期
                        if (model.RenewMoney > 0)
                        {
                            var query = from b in lqh.Umsdb.Sys_Option
                                        where b.ID == 1
                                        select b;
                            if (query.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "配置信息有误" };
                            }
                            #region 获取比例
                            Model.Sys_Option Option = query.First();
                            decimal percent = (decimal)int.Parse(Option.Value) / int.Parse(Option.Value2);
                            model.RenewDate = DateTime.Now;
                            model.RenewTypeClassName = Option.ClassName;
                            model.RenewTypeName = Option.Name;
                            model.RenewValue1 = decimal.Parse(Option.Value.ToString());
                            model.RenewValue2 = decimal.Parse(Option.Value2.ToString());
                            decimal Validity = (decimal)model.RenewMoney / percent;
                            if (model.Validity != Validity)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "续期计算出错！" };
                            }
                            #endregion
                        }
                        #endregion
                        #region 现金续期
                        if (model.Point > 0)
                        {
                            var query = from b in lqh.Umsdb.Sys_Option
                                        where b.ID == 2
                                        select b;
                            if (query.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "配置信息有误" };
                            }
                            #region 获取比例
                            Model.Sys_Option Option = query.First();
                            double percent = (double)int.Parse(Option.Value) / int.Parse(Option.Value2);
                            model.RenewDate = DateTime.Now;
                            model.RenewTypeClassName = Option.ClassName;
                            model.RenewTypeName = Option.Name;
                            model.RenewValue1 = decimal.Parse(Option.Value.ToString());
                            model.RenewValue2 = decimal.Parse(Option.Value2.ToString());
                            double Validity = double.Parse(model.Point.ToString()) / percent;
                            if (model.Validity != int.Parse(Validity.ToString()))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "续期计算出错！" };
                            }
                            NewVIP.Point -= model.Point;
                            #endregion
                        }
                        #endregion

                        #region 更新会员期限                    
                        NewVIP.Validity += model.Validity;
                   
                        if (NewVIP.EndTime == null||NewVIP.EndTime<DateTime.Now)
                        {
                            NewVIP.EndTime = DateTime.Now.AddDays((double)model.Validity);
                        }
                        else
                        {
                            NewVIP.EndTime = NewVIP.EndTime.Value.AddDays((double)model.Validity);
                        }
                                         
                        NewVIP.UpdUser = user.UserID;
                        NewVIP.SysDate = DateTime.Now;
                        lqh.Umsdb.VIP_Renew.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        #region 成功返回数据
                        var query_View= (from b in lqh.Umsdb.View_VIPInfo
                                     where b.ID == NewVIP.ID
                                     select b).ToList();
                        if (query_View.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "返回失败" };
                        }
                        #endregion
                        ts.Complete();

                        //ArrayList arr = new ArrayList();
                        //arr.Add(model.Validity);
                        //arr.Add(model.Point);
                     
                   
                        return new Model.WebReturn() {Obj=query_View.First(), ReturnValue = true, Message = "续期成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false ,Message="服务器异常！"};
                    }
                }
            }
        }
                        #endregion
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="vip"></param>
        /// <returns></returns>
        public Model.WebReturn SearchVIP(Model.Sys_UserInfo user, Model.VIP_VIPInfo vip)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var query = from v in lqh.Umsdb.VIP_VIPInfo
                                    join cardtype in lqh.Umsdb.VIP_IDCardType on v.IDCard_ID equals cardtype.ID
                                    where v.IDCard == vip.IDCard
                                    || v.IMEI == vip.IMEI || v.MemberName == vip.MemberName ||
                                    v.MobiPhone == vip.MobiPhone
                                    select new
                                    {
                                        v.ID,
                                        v.Point,
                                        v.Flag,
                                        v.IDCard_ID,
                                        v.IDCard,
                                        v.IMEI,
                                        v.MemberName,
                                        v.MobiPhone,
                                        v.Validity,
                                        cardtype.Name
                                    };

                        if (query.Where(p => p.IDCard_ID == vip.IDCard_ID && p.Flag == null).Count() != 0)
                        {
                            List<Model.RenewModel> models = new List<Model.RenewModel>();
                            Model.RenewModel rm = null;

                            foreach (var item in query.Where(p => p.IDCard_ID == vip.IDCard_ID))
                            {
                                rm = new Model.RenewModel();
                                rm.VIPID = item.ID;
                                rm.Point = decimal.Parse(item.Point.ToString());
                                rm.IDCard = item.IDCard;
                                rm.IDCard_ID = int.Parse(item.IDCard_ID.ToString());
                                rm.IMEI = item.IMEI;
                                rm.CardType = item.Name;
                                rm.MemberName = item.MemberName;
                                rm.MobilePhone = item.MobiPhone;
                                rm.Validity = string.IsNullOrEmpty(item.Validity.ToString()) ? 0 : int.Parse(item.Validity.ToString());
                                models.Add(rm);
                            }
                            return new Model.WebReturn() { ReturnValue = true, Obj = models };
                        }
                        return new Model.WebReturn() { ReturnValue = false };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.VIP_RenewBack model)
        {
            //验证退款金额是否 有效，一定要小于等于客户支付金额
            //取消续期天数 Validity
            //取消信息 VIP_RenewBack
            //更新审批单状态
            //更新会员信息VIP_VIPInfo Flag(不需要)
            //返回
            string Msg = "";

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var exist_QX = from b in lqh.Umsdb.GetTable<Model.VIP_Renew>()
                                       where b.ID == model.Old_Renew_ID
                                       select b;
                        if (exist_QX.Count() > 0)
                        {
                            Msg = "续期" + model.ID + "已取消";
                            return new Model.WebReturn() { Obj = true, Message = Msg };
                        }
                        // Model.VIP_RenewBack renew_back = new Model.VIP_RenewBack();
                        var query = from b in lqh.Umsdb.GetTable<Model.VIP_RenewBackAduit>()
                                    where b.AduitID == model.AduitID.ToString()
                                    select b;

                        if (query.Count() == 0)
                        {
                            Msg = "审批单不存在";
                            return new Model.WebReturn { Obj = true, Message = Msg };
                        }
                        Model.VIP_RenewBackAduit renewAduit = query.First();
                        if (renewAduit.Used == true)
                        {
                            Msg = "审批" + model.ID + " 已使用";
                            return new Model.WebReturn { Obj = true, Message = Msg };
                        }
                        if (renewAduit.Aduited == false)
                        {
                            Msg = "审批单" + model.ID + "未审核";
                            return new Model.WebReturn { Obj = true, Message = Msg };
                        }
                        if (renewAduit.Passed != true)
                        {
                            Msg = "审批" + model.ID + "未通过";
                            return new Model.WebReturn { Obj = true, Message = Msg };
                        }
                        renewAduit.Used = true;

                        var vip = from b in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                                  where b.ID == model.VIP_Renew.VIP_ID
                                  select b;
                        Model.VIP_VIPInfo vipinfo = vip.First();
                        vipinfo.Validity -= model.Validity;
                        model.VIP_Renew.Note = "已经取消";

                        lqh.Umsdb.VIP_RenewBack.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { Obj = true, Message = "取消成功" };
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
        /// 查询续期单    
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn SearchRenew(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region 权限

                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var renew_query = from b in lqh.Umsdb.View_VIP_Renew
                                      select b;

                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "State":

                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)item;
                                if (mm.ParamValues == "Yes")
                                {
                                    renew_query = from re in renew_query
                                                  where re.State == "Y"
                                                  select re;
                                }
                                else
                                {
                                    renew_query = from re in renew_query
                                                  where re.State == "N"
                                                  select re;
                                }
                                break;

                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                if (mm5.ParamValues != null)
                                {
                                    renew_query = from b in renew_query
                                                  where b.SysDate >= mm5.ParamValues
                                                  select b;
                                }
                                break;
                            case "EndTime":
                                Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)item;
                                if (mm6.ParamValues != null)
                                {
                                    renew_query = from b in renew_query
                                                  where b.SysDate <= mm6.ParamValues
                                                  select b;
                                }
                                break;

                            case "IMEI":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    renew_query = from b in renew_query
                                                  where b.IMEI.Contains(para.ParamValues)
                                                  select b;
                                }
                                break;
                            case "MobiPhone":
                                Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                if (para1.ParamValues != null)
                                {
                                    renew_query = from b in renew_query
                                                  where para1.ParamValues.Contains(b.MobiPhone)
                                                  select b;
                                }
                                break;

                            case "IDCard":
                                Model.ReportSqlParams_String para2 = (Model.ReportSqlParams_String)item;
                                if (para2.ParamValues != null)
                                {
                                    renew_query = from b in renew_query
                                                  where b.IDCard.Contains(para2.ParamValues)
                                                  select b;
                                }
                                break;
                            case "MemberName":
                                Model.ReportSqlParams_String name = (Model.ReportSqlParams_String)item;
                                if (name.ParamValues != null)
                                {
                                    renew_query = from b in renew_query
                                                  where b.MemberName.Contains(name.ParamValues)
                                                  select b;
                                }
                                break;
                        }
                    }

                    #endregion

                    #region 过滤仓库

                    //if (ValidHallIDS.Count() > 0)
                    //{
                    //    renew_query = from b in renew_query
                    //                    where ValidHallIDS.Contains(b.HallID)
                    //                    orderby b.SysDate descending
                    //                    select b;
                    //}
                    //else
                    //{
                    //    renew_query = from b in renew_query
                    //                    orderby b.SysDate descending
                    //                    select b;
                    //}
                    #endregion

                    pageParam.RecordCount = renew_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from r in renew_query.Take(pageParam.PageSize).ToList()
                                      //  where r.UserID == user.UserID && r.UserID == user.UserID
                                      select r;

                        List<Model.View_VIP_Renew> list = results.ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    else
                    {
                        var results = from r in renew_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      //where r.UserID == user.UserID && r.UserID == user.UserID
                                      select r;

                        List<Model.View_VIP_Renew> list = results.ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }
    }
}

