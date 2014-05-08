using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class ASP_Factory:Sys_InitParentInfo
    {
         private int MethodID;

        public ASP_Factory()
        {
            this.MethodID = 0;
        }

        public ASP_Factory(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        ///367
        /// </summary>
        /// <param name="user"></param>
        /// <param name="recinfo"></param>
        /// <param name="checkinfo"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.ASP_Factory facinfo)
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

                        var reper = from a in lqh.Umsdb.ASP_Factory
                                    where a.FacName == facinfo.FacName
                                    select a;

                        if (reper.Count() > 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该厂家名称已存在，保存失败！" };
                        }
                        var reper2 = from a in lqh.Umsdb.ASP_Factory
                                    where a.FacID == facinfo.FacID
                                    select a;

                        if (reper2.Count() > 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该厂家编码已存在，保存失败！" };
                        }

                        facinfo.SysDate = DateTime.Now;
                        facinfo.IsDelete = false;
                        lqh.Umsdb.ASP_Factory.InsertOnSubmit(facinfo);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "添加成功！" };
                    }
                    catch (Exception ez)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ez.Message };
                    }
                }
            }
        }


        /// <summary>
        ///369
        /// </summary>
        /// <param name="user"></param>
        /// <param name="recinfo"></param>
        /// <param name="checkinfo"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.ASP_Factory facinfo)
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

                        var reper = from a in lqh.Umsdb.ASP_Factory
                                    where a.ID == facinfo.ID
                                    select a;

                        if (reper.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "未能找到指定数据，修改失败！" };
                        }
                        var reper2 = from a in lqh.Umsdb.ASP_Factory
                                     where a.ID != facinfo.ID &&a.FacName == facinfo.FacName
                                     select a;

                        if (reper2.Count() > 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该厂家名称已存在，修改失败！" };
                        }

                        var reper3 = from a in lqh.Umsdb.ASP_Factory
                                     where a.ID != facinfo.ID && a.FacID == facinfo.FacID
                                     select a;

                        if (reper3.Count() > 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该厂家编码已存在，修改失败！" };
                        }
                        Model.ASP_Factory model = reper.First();
                        model.FacName = facinfo.FacName;
                        model.Fax = facinfo.Fax;
                        model.Addr = facinfo.Addr;
                        model.Area = facinfo.Area;
                        model.Bank = facinfo.Bank;
                        model.BankNum = facinfo.BankNum;
                        model.City = facinfo.City;
                        model.Contacts = facinfo.Contacts;
                        model.Email = facinfo.Email;
                        model.FacID = facinfo.FacID;
                        model.Note = facinfo.Note;
                        model.Phone = facinfo.Phone;
                        model.PostCode = facinfo.PostCode;
                        model.PriceLevel = facinfo.PriceLevel;
                        model.Province = facinfo.Province;
                        model.Responser = facinfo.Responser;
                        model.TaxCode = facinfo.TaxCode;
                        model.UpdTime = DateTime.Now;
                     
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功！" };
                    }
                    catch (Exception ez)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ez.Message };
                    }
                }
            }
        }

        /// <summary>
        ///370
        /// </summary>
        /// <param name="user"></param>
        /// <param name="recinfo"></param>
        /// <param name="checkinfo"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, int id)
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

                        var reper = from a in lqh.Umsdb.ASP_Factory
                                    where a.ID == id
                                    select a;

                        if (reper.Count() == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "未能找到指定数据，修改失败！" };
                        }
                        Model.ASP_Factory model = reper.First();
                        if (model.IsDelete == true)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "该数据以删除！" };
                        }
                        model.IsDelete = true;
                        model.DelTime = DateTime.Now;

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功！" };
                    }
                    catch (Exception ez)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ez.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 368
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

                    var aduit_query = from b in lqh.Umsdb.ASP_Factory
                                      where b.IsDelete == false 
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "FacName":
                                Model.ReportSqlParams_String name = (Model.ReportSqlParams_String)item;

                                    aduit_query = from b in aduit_query
                                                  where b.FacName.Contains( name.ParamValues)
                                                  select b;
                               
                                break;
                            case "SysDate":
                                Model.ReportSqlParams_DataTime mm = (Model.ReportSqlParams_DataTime)item;

                                aduit_query = from b in aduit_query
                                              where b.SysDate >= mm.ParamValues
                                              select b;

                                break;
                            case "FacID":
                                Model.ReportSqlParams_String pass = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.FacID==pass.ParamValues
                                              select b;
                                break;
                         
                            case "Area":
                                Model.ReportSqlParams_String mm2 = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.Area==mm2.ParamValues
                                              select b;
                                break;

                            case "Province":
                                Model.ReportSqlParams_String pass2 = (Model.ReportSqlParams_String)item;


                                aduit_query = from b in aduit_query
                                              where b.Province==pass2.ParamValues
                                              select b;

                                break;

                            case "City":
                                Model.ReportSqlParams_String imei = (Model.ReportSqlParams_String)item;

                                aduit_query = from b in aduit_query
                                              where b.City == imei.ParamValues
                                              select b;
                                break;

                        }
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

                        List<Model.ASP_Factory> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.ASP_Factory> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Message = ex.Message, ReturnValue = false };
                }
            }

        }

        public List<Model.ASP_Factory> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.ASP_Factory>()
                                where b.IsDelete == false
                                select b;

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.ASP_Factory>();
                }
            }
        }
    }
}
