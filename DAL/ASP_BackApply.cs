using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class ASP_BackApply
    {
        private int MethodID;

        public ASP_BackApply()
        {
            this.MethodID = 0;
        }

        public ASP_BackApply(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        /// 确认退款 383
        /// </summary>
        /// <param name="user"></param>
        /// <param name="applys"></param>
        /// <returns></returns>
        public Model.WebReturn ConfirmBack(Model.Sys_UserInfo user,List< Model.View_ASPBackApply> applys,string note)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 权限验证

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                        if (!result.ReturnValue)
                        { return result; }

                        //List<int> classids = new List<int>();
                        //foreach (var item in recinfo.ASP_CurrentOrder_BackupPhoneInfo)
                        //{
                        //    var queey = from p in lqh.Umsdb.Pro_ProInfo
                        //                where p.ProID == item.ProID
                        //                select p;
                        //    classids.Add((int)queey.First().Pro_ClassID);
                        //}
                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion

                        var rep = from a in lqh.Umsdb.ASP_BackApply
                                  where applys.Select(b=>b.ID).Contains(a.ID)
                                  select a;

                        if (rep.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定退款单，确认失败！" };
                        }

                        foreach (var item in rep)
                        {
                            if (Convert.ToBoolean(item.IsAudit) == false)
                            {
                                return new WebReturn() { ReturnValue=false,Message="退款单 " +item.ApplyID+" 未审批！"};
                            }
                            if (Convert.ToBoolean(item.Passed) == false)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "退款单 " + item.ApplyID + " 审批未通过！" };
                            }
                            if (Convert.ToBoolean(item.HasConfirm) == true)
                            {
                                return new WebReturn() { ReturnValue = false, Message = "退款单 " + item.ApplyID + " 已确认！" };
                            }

                            item.HasConfirm = true;
                            item.Confirmer = user.UserID;
                            item.ConNote = note;
                            item.ConfDate = DateTime.Now;


                            lqh.Umsdb.SubmitChanges();
                        }

                        lqh.Umsdb.SubmitChanges();

                        ts.Complete();
                        return new WebReturn() { ReturnValue = true, Message = "保存成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.ASP_BackApply apply)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 权限验证

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                        if (!result.ReturnValue)
                        { return result; }

                        //List<int> classids = new List<int>();
                        //foreach (var item in recinfo.ASP_CurrentOrder_BackupPhoneInfo)
                        //{
                        //    var queey = from p in lqh.Umsdb.Pro_ProInfo
                        //                where p.ProID == item.ProID
                        //                select p;
                        //    classids.Add((int)queey.First().Pro_ClassID);
                        //}
                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion

                        var rep = from a in lqh.Umsdb.ASP_RepairInfo
                                  where a.ID == apply.RepairID
                                  select a;
                    
                        if (rep.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，申请失败！" };
                        } 
                        Model.ASP_RepairInfo m = rep.First();
                        if (m.IsDelete == true)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该维修单已删除，申请失败！" };
                        }
                        if (m.BackApplyID > 0)
                        {
                            return new WebReturn() { ReturnValue=false,Message="该维修单已申请退款！"};
                        }
                        apply.ApplyDate = DateTime.Now;
                        apply.Applyer = user.UserID;
                        string message = "";
                        lqh.Umsdb.OrderMacker(1, "RTK", "RTK", ref message);
                        if (string.IsNullOrEmpty(message))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "单号生成有误！" };
                        }

                        apply.ApplyID = message;

                        lqh.Umsdb.ASP_BackApply.InsertOnSubmit(apply);
                        rep.First().BackApplyID = apply.ID;
                        lqh.Umsdb.SubmitChanges();
                        
                        ts.Complete();
                        return new WebReturn() { ReturnValue = true, Message = "申请成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue=false,Message = ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 373
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn Search(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region 权限

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return new WebReturn() { ReturnValue = false, Obj = pageParam }; }

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

                    var aduit_query = from b in lqh.Umsdb.View_ASPBackApply
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "IsAudit":
                                Model.ReportSqlParams_Bool mm = (Model.ReportSqlParams_Bool)item;
                                if (mm.ParamValues == false)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.Audit == null || b.Audit==false
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.Audit ==true
                                                  select b;
                                }
                                break;

                            case "IsPassed":
                                Model.ReportSqlParams_Bool pass = (Model.ReportSqlParams_Bool)item;

                                aduit_query = from b in aduit_query
                                              where b.Pass == pass.ParamValues
                                              select b;

                                break;
                            case "OldID":
                                Model.ReportSqlParams_String oid = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.OldID.Contains(oid.ParamValues)
                                              select b;
                                break;
                            case "HallID":
                                Model.ReportSqlParams_String pass1 = (Model.ReportSqlParams_String)item;


                                aduit_query = from b in aduit_query
                                              where b.HallID == pass1.ParamValues
                                              select b;

                                break;

                            case "Cus_Name":
                                Model.ReportSqlParams_String mm2 = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.Cus_Name.Contains(mm2.ParamValues)
                                              select b;
                                break;

                            case "Cus_Phone":
                                Model.ReportSqlParams_String pass2 = (Model.ReportSqlParams_String)item;


                                aduit_query = from b in aduit_query
                                              where b.Cus_Phone.Contains(pass2.ParamValues)
                                              select b;

                                break;

                            case "Pro_IMEI":
                                Model.ReportSqlParams_String imei = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.Pro_IMEI == imei.ParamValues
                                              select b;
                                break;

                            case "VIP_IMEI": //会员卡号
                                Model.ReportSqlParams_String mm3 = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.IMEI == mm3.ParamValues
                                              select b;
                                break;


                        }
                    }

                    #endregion

                    #region 过滤仓库

                    if (ValidHallIDS.Count() > 0)
                    {
                        aduit_query = from b in aduit_query
                                      where ValidHallIDS.Contains(b.HallID)
                                      orderby b.SysDate descending
                                      select b;
                    }
                    else
                    {
                        aduit_query = from b in aduit_query
                                      orderby b.SysDate descending
                                      select b;
                    }
                    #endregion
                    pageParam.RecordCount = aduit_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in aduit_query.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPBackApply> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_ASPBackApply> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion

                    return new Model.WebReturn() { ReturnValue = true };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Message = ex.Message, ReturnValue = false };
                }
            }

        }

        /// <summary>
        /// 审批 374
        /// </summary>
        /// <param name="user"></param>
        /// <param name="apply"></param>
        /// <returns></returns>
        public Model.WebReturn Audit(Model.Sys_UserInfo user,int repairid, int id,bool passed,string note)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 权限验证

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                        if (!result.ReturnValue)
                        { return result; }

                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion

                        var applyinfo = from a in lqh.Umsdb.ASP_BackApply
                                  where a.ID == id
                                  select a;
                        if (applyinfo.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定退款申请单，审批失败！" };
                        }
                        Model.ASP_BackApply ba = applyinfo.First();
                        if (ba.IsAudit == true)
                        {
                            return new WebReturn() { ReturnValue=false,Message="该单已审批！"};
                        }
                        ba.Auditer = user.UserID;
                        ba.AuditDate = DateTime.Now;
                        ba.IsAudit = true;
                        ba.Passed = passed;
                        ba.AuditNote = note;

                        #region  返还配件


                        var repinfo = from a in lqh.Umsdb.ASP_RepairInfo
                                     where a.ID == repairid
                                     select a;
                        if (repinfo.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "未能找到指定维修单，审批失败！" };
                        }

                        var pros = from a in lqh.Umsdb.ASP_CurrentOrder_Pros
                                    where a.RepairID == repairid
                                    select a;
                        if (pros.Count() > 0)
                        {
                            //return new WebReturn() { ReturnValue = false, Message = "未能找到指定配件数据，审批失败！" };

                            foreach (var xx in pros)
                            {
                                if (!string.IsNullOrEmpty(xx.IMEI))
                                {
                                    var imei = from a in lqh.Umsdb.Pro_IMEI
                                                where a.IMEI.ToUpper() == xx.IMEI.ToUpper()
                                                select a;
                                    if (imei.Count() == 0)
                                    {
                                        return new WebReturn() { ReturnValue = false, Message = "串码不存在：" + xx.IMEI + " ,审批失败！" };
                                    }
                                    imei.First().State = 0;
                                    imei.First().PJID = null;
                                }
                                var store = from a in lqh.Umsdb.Pro_StoreInfo
                                            where a.ProID == xx.ProID && a.InListID == xx.InListID
                                            && a.HallID == repinfo.First().HallID
                                            select a;
                                if (store.Count() == 0)
                                {
                                    return new WebReturn() { ReturnValue = false, Message = "库存不足！" };
                                }
                                store.First().ProCount += 1;

                                xx.OrderID = 0;
                            }
                        }

                        #endregion

                        lqh.Umsdb.SubmitChanges();

                        ts.Complete();
                        return new WebReturn() { ReturnValue = true, Message = "审批成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

    }
}
