using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Pro_SellAduit
    {
        private int MethodID;

	    public Pro_SellAduit()
	    {
		    this.MethodID = 0;
	    }

        public Pro_SellAduit(int MenthodID)
	    {
		    this.MethodID = MenthodID;
	    }

        /// <summary>
        /// 提交销售申请
        /// </summary>
        /// <param name="psa"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Pro_SellAduit psa)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证用户操作仓库  商品的权限 
                        
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        List<string> classids = new List<string>();
                        foreach (var item in psa.Pro_SellAduitList)
                        {
                            var queryc = from p in lqh.Umsdb.Pro_ProInfo
                                         where p.ProID == item.ProID.ToString()
                                         select p;
                            classids.Add(queryc.First().Pro_ClassID.ToString());
                        }
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                        //有仓库限制，而且仓库不在权限范围内
                        if (ValidHallIDS.Count() > 0)
                        {
                            if (!ValidHallIDS.Contains(psa.HallID))
                            {
                                var que = from h in lqh.Umsdb.Pro_HallInfo
                                          where h.HallID == psa.HallID
                                          select h;
                                return new Model.WebReturn() { ReturnValue = false, Message = que.First().HallName + "仓库无权操作" };
                            }
                            
                        }
                        //验证商品权限
                        if (ValidProIDS.Count > 0)
                        {
                            foreach (var child in classids)
                            {
                                if (!ValidProIDS.Contains(child))
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "无权操作商品" };
                                }
                            }
                        }

                        #endregion

                        string aduitid = null;
                        lqh.Umsdb.OrderMacker(1, "SSP", "SSP", ref aduitid);
                        if (string.IsNullOrEmpty(aduitid))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "审批单生成出错" };
                        }

                        psa.AduitID = aduitid;
                        psa.Aduited = false;
                        psa.Passed = false;

                        lqh.Umsdb.Pro_SellAduit.InsertOnSubmit(psa);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true ,Obj=psa.AduitID};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false,Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 批发一级审批
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn Aduit(Model.Sys_UserInfo user, List<Model.Pro_SellAduit> models,bool batchflag)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {

                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region "验证用户操作仓库  商品的权限 "

                        
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }
                         
                         
                        #endregion

                        foreach (var item in models)
                        {
                            var query = from aduit in lqh.Umsdb.Pro_SellAduit
                                        where aduit.ID == item.ID
                                        select aduit;

                            if (query.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "未能找到审批单，审批失败！" };
                            }
                            Model.Pro_SellAduit aduitModel = query.First();

                     
                            if (Convert.ToBoolean(aduitModel.Aduited1))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 已审批，审批失败！" };
                            }

                            #region 验证仓库权限
                            if (!ValidHallIDS.Contains(aduitModel.HallID))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 无权操作！" };
                            }
                            #endregion

                            #region  验证批发单价是否存在

                          

                             
                            aduitModel.AduitDate = DateTime.Now;
                            aduitModel.AduitUser = item.AduitUser;
                            aduitModel.Aduited1 = true;
                            aduitModel.Note1 = item.Note1;
                            aduitModel.Passed1 = item.Passed1;
                            if (item.Passed1 != true)
                            {
                                aduitModel.Aduited = true;
                                aduitModel.Passed = false;
                                
                            }
                            #endregion 
                        }

                 
                         
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue=true};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false ,Message=ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 批发二级审批
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn Aduit2(Model.Sys_UserInfo user, List<Model.Pro_SellAduit> models, bool batchflag)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region "验证用户操作仓库  商品的权限 "


                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }


                        #endregion

                        foreach (var item in models)
                        {
                            var query = from aduit in lqh.Umsdb.Pro_SellAduit
                                        where aduit.ID == item.ID
                                        select aduit;

                            if (query.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "未能找到审批单，审批失败！" };
                            }
                            Model.Pro_SellAduit aduitModel = query.First();


                            if (!Convert.ToBoolean(aduitModel.Aduited1))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 一级尚未审批！" };
                            }
                            if (Convert.ToBoolean(aduitModel.Aduited2))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 二级已审批！" };
                            }
                            #region 验证仓库权限
                            if (!ValidHallIDS.Contains(aduitModel.HallID))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 无权操作！" };
                            }
                            #endregion

                            #region  验证批发单价是否存在

                            
                            aduitModel.AduitDate2 = DateTime.Now;
                            aduitModel.AduitUser2 = item.AduitUser2;
                            aduitModel.Aduited2 = true;
                            aduitModel.Note2 = item.Note2;
                            aduitModel.Passed2 = item.Passed2;
                            if (item.Passed2 != true)
                            {
                                aduitModel.Aduited = true;
                                aduitModel.Passed = false;

                            }
                            #endregion
                        }



                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 批发三级审批
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn Aduit3(Model.Sys_UserInfo user, List<Model.Pro_SellAduit> models, bool batchflag)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region "验证用户操作仓库  商品的权限 "


                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }


                        #endregion

                        foreach (var item in models)
                        {
                            var query = from aduit in lqh.Umsdb.Pro_SellAduit
                                        where aduit.ID == item.ID
                                        select aduit;

                            if (query.Count() == 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "未能找到审批单，审批失败！" };
                            }
                            Model.Pro_SellAduit aduitModel = query.First();


                            if (!Convert.ToBoolean(aduitModel.Aduited2))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 二级尚未审批！" };
                            }
                            if (Convert.ToBoolean(aduitModel.Aduited3))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 三级已审批！" };
                            }
                            #region 验证仓库权限
                            if (!ValidHallIDS.Contains(aduitModel.HallID))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "审批单 " + aduitModel.AduitID + " 无权操作！" };
                            }
                            #endregion

                            #region  验证批发单价是否存在


                            aduitModel.AduitDate3 = DateTime.Now;
                            aduitModel.AduitUser3 = item.AduitUser3;
                            aduitModel.Aduited3 = true;
                            aduitModel.Note3 = item.Note3;
                            aduitModel.Passed3 = item.Passed3;
                            aduitModel.Aduited = true;


                            aduitModel.Passed = (item.Passed3==true);

                             
                            #endregion
                        }



                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 拣货
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn Check(Model.Sys_UserInfo user, List<Model.View_SellTypeProduct> models,string hallid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        bool sucess = true;
                        foreach(var item in models)
                        {
                            var query = from s in lqh.Umsdb.Pro_StoreInfo
                                        where s.ProID == item.ProID && s.HallID==hallid
                                        select s;

                            decimal count = (decimal)item.ProCount;

                            foreach (var child in query)
                            {
                                if (child.ProCount >0 && child.ProCount < count)
                                {
                                    count -= child.ProCount;
                                    if (count == 0)
                                    {
                                        break;
                                    }
                                    continue;
                                }
                                if (child.ProCount > count)
                                {
                                    count = 0;
                                    break;
                                }
                            }
                            if (count!=0)
                            {
                                item.Note = "失败";
                                sucess = false;
                            }
                            else
                            {
                                item.Note = "成功";
                            }
                       }

                        return new Model.WebReturn(){Obj=models,ReturnValue= sucess};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 获取待审批列表
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="countOnePage">每页提取的数量</param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user, int pageIndex, int countOnePage)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    int? total = 0;
                    List<Model.GetSellAduitListByPageResult> results=
                    lqh.Umsdb.GetSellAduitListByPage(pageIndex, countOnePage, ref total).ToList();

                    if (results.Count() != 0)
                    {
                        List<Model.AduitModel> models = new List<Model.AduitModel>();
                        Model.AduitModel am = null;

                        foreach (var item in results)
                        {
                            am = new Model.AduitModel();
                            am.ID = int.Parse(item.ID.ToString());
                            am.AduitID = item.AduitID;
                            am.ApplyUser = item.ApplyUser;
                            am.ApplyDate = DateTime.Parse(item.ApplyDate.ToString());
                            am.HallID = item.HallID;
                            am.HallName = item.HallName;
                            models.Add(am);
                        }
                        ArrayList arrList = new ArrayList();
                        arrList.Add(total);
                        return new Model.WebReturn() { Obj = models, ReturnValue = true ,ArrList = arrList};
                    }
                    return new Model.WebReturn() { ReturnValue = false };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false};
                }
            }
        }


        public Model.WebReturn GetModelDetail(Model.Sys_UserInfo user, string aduitid)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    List<Model.GetSAModelResult> model =  lqh.Umsdb.GetSAModel(aduitid).ToList();

                    if (model.Count == 0)
                    {
                        return new WebReturn() { ReturnValue=false,Message = "审批单无效",Obj = new Model.GetSAModelResult()};  
                    }
                    return new WebReturn() { ReturnValue = true, Obj = model };  
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false };
                }
            }
        }

        /// <summary>
        /// 根据审批单获取其信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.WebReturn GetListByID(Model.Sys_UserInfo user, int id)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = lqh.Umsdb.GetSellAduitListByID(id).ToList();

                    List<Model.AduitListInfo> models = new List<Model.AduitListInfo>();
                    Model.AduitListInfo a = null;

                    bool flag = false;

                    if (query.Count() != 0)
                    {
                        flag = true;
                        foreach (var item in query)
                        {
                            a = new Model.AduitListInfo();
                            a.ID = (int)item.ID;

                            a.ProPrice = decimal.Round(Convert.ToDecimal(item.Price), 4);
                           
                            a.MaxPrice =  decimal.Round(Convert.ToDecimal(item.MaxPrice),4);
                            a.MinPrice =  decimal.Round(Convert.ToDecimal(item.MinPrice),4);
                            a.ProID = item.ProID;
                            a.SellTypeID =Convert.ToInt32( item.SellTypeID);
                            a.OffMoney = decimal.Round(Convert.ToDecimal(item.OffMoney), 4);
                            a.NewPrice = decimal.Round(Convert.ToDecimal(item.ProPrice), 4);
                            a.ProFormat = item.ProFormat;
                            // 设置审批单价 NewPrice
                            //if (Convert.ToBoolean(item.Aduited))
                            //{
                            //    a.NewPrice = a.ProPrice - a.OffMoney;
                            //}
                            //else
                            //{
                            //    a.NewPrice = a.ProPrice;   
                            //}

                            if (Convert.ToBoolean(item.IsDecimals))
                            {
                                a.ProCount = decimal.Round(Convert.ToDecimal(item.ProCount),2);
                            }
                            else
                            {
                                a.ProCount =Convert.ToInt32(item.ProCount);
                            }
                            a.ProName = item.ProName;
                            a.ProTypeName = item.TypeName;
                            a.ProClassName = item.ClassName;
                            a.IsDecimal =Convert.ToBoolean( item.IsDecimals);
                            models.Add(a);
                        }
                    }
                    string msg = "";
                    if (!flag)
                    {
                        var query2 = from list in lqh.Umsdb.Pro_SellAduitList
                                     where list.SellAuditID == id 
                                     select list;
                        foreach(var child in query2)
                        {
                            var que = from s in lqh.Umsdb.Pro_SellTypeProduct 
                                    where s.SellType == child.SellTypeID && s.ProID==child.ProID
                                    select s;
                            if (que.Count() == 0)
                            {
                                var pro = from p in lqh.Umsdb.Pro_ProInfo
                                          where p.ProID == child.ProID
                                          select new { 
                                          p.ProName
                                          };
                                msg = "商品" + pro.First().ProName + "未设置批发单价";
                                break;
                            }
                        }
                    }
                    return new Model.WebReturn() { ReturnValue = flag,Obj=models,Message=msg};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue =false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 一级审批查询
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
                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return new WebReturn() { ReturnValue = false, Obj = pageParam }; }

                    #region 权限

                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0 )
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"

                    var aduit_query = from b in lqh.Umsdb.View_Pro_SellAduit
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "Aduited":
                                Model.ReportSqlParams_Bool mme = (Model.ReportSqlParams_Bool)item;

                                if (mme.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited != true
                                                  select b;
                                }
                                break;

                            case "Passed":
                                Model.ReportSqlParams_Bool pass = (Model.ReportSqlParams_Bool)item;

                                if (pass.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed != true
                                                  select b;
                                }
                          
                                break;

                            case "Aduited1":
                                Model.ReportSqlParams_Bool mm = (Model.ReportSqlParams_Bool)item;

                                if (mm.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited1 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited1 != true
                                                  select b;
                                }
                          
                                break;

                            case "Passed1":
                                Model.ReportSqlParams_Bool pass1 = (Model.ReportSqlParams_Bool)item;

                                if (pass1.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed1 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed1 != true
                                                  select b;
                                }
                          
                                break;

                                //////  2 //////  
                            case "Aduited2":
                                Model.ReportSqlParams_Bool mm2 = (Model.ReportSqlParams_Bool)item;

                                if (mm2.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited2 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited2 != true
                                                  select b;
                                }
                          
                                break;

                            case "Passed2":
                                Model.ReportSqlParams_Bool pass2 = (Model.ReportSqlParams_Bool)item;

                                if (pass2.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed2 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed2 != true
                                                  select b;
                                }
                          
                                break;
                            //////  2 //////  
                            case "Aduited3":
                                Model.ReportSqlParams_Bool mm3 = (Model.ReportSqlParams_Bool)item;

                                if (mm3.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited3 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited3 != true
                                                  select b;
                                }
                                break;

                            case "Passed3":
                                Model.ReportSqlParams_Bool pass3 = (Model.ReportSqlParams_Bool)item;

                                if (pass3.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed3 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed3 != true
                                                  select b;
                                }
                                break;
                             

                            case "Used":
                                Model.ReportSqlParams_Bool use = (Model.ReportSqlParams_Bool)item;

                                aduit_query = from b in aduit_query
                                              where b.HasUsed == use.ParamValues
                                              select b;
                                break;

                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                if (mm5.ParamValues != null)
                                {
                                    Model.ReportSqlParams_DataTime mm6 = new ReportSqlParams_DataTime();
                                    mm6.ParamValues = DateTime.Now;
                                    foreach (var xxd in pageParam.ParamList)
                                    {
                                        if (xxd.ParamName == "EndTime")
                                        {
                                            mm6 = (Model.ReportSqlParams_DataTime)xxd;
                                            break;
                                        }
                                    }
                                    if (mm5.ParamValues == mm6.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                       where b.SysDate >= mm5.ParamValues &&
                                                       b.SysDate < DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                                                       select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                       where b.SysDate >= mm5.ParamValues
                                                       && b.SysDate <= DateTime.Parse(mm6.ParamValues.ToString()).AddDays(1)
                                                       select b;
                                    }
                                }
                                break;

                            case "EndTime":
                                Model.ReportSqlParams_DataTime ed = (Model.ReportSqlParams_DataTime)item;
                                if (ed.ParamValues != null)
                                {
                                        aduit_query = from b in aduit_query
                                                      where b.SysDate < DateTime.Parse(ed.ParamValues.ToString()).AddDays(1)
                                                      select b;
                                 
                                   
                                }
                                break;
                     
                            case "ApplyUser":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.ApplyUser.Contains(para.ParamValues.ToString())
                                                  select b;
                                }
                                break;

                            case "AduitID":
                                Model.ReportSqlParams_String para2 = (Model.ReportSqlParams_String)item;
                                if (para2.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.AduitID.Contains(para2.ParamValues)
                                                  select b;
                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                if (para1.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where para1.ParamValues.Contains(b.HallID)
                                                  select b;

                                }
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

                        List<Model.View_Pro_SellAduit> aduitList = results.ToList();
                      
                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_Pro_SellAduit> aduitList = results.ToList();
                       
                        pageParam.Obj = aduitList;
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

        /// <summary>
        /// 二级审批查询
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn Search2(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return new WebReturn() { ReturnValue = false, Obj = pageParam }; }

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
                  
                    var aduit_query = from b in lqh.Umsdb.View_Pro_SellAduit
                                      where b.HasAduited1==true && b.HasPassed1==true
                                      select b;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "Aduited2":
                                Model.ReportSqlParams_Bool mm = (Model.ReportSqlParams_Bool)item;

                                if (mm.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited2 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited2 != true
                                                  select b;
                                }
                                break;

                            case "Passed2":
                                Model.ReportSqlParams_Bool pass = (Model.ReportSqlParams_Bool)item;

                                if (pass.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed2 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed2 != true
                                                  select b;
                                }
                                break;

                            case "Used":
                                Model.ReportSqlParams_Bool use = (Model.ReportSqlParams_Bool)item;

                                aduit_query = from b in aduit_query
                                              where b.HasUsed == use.ParamValues
                                              select b;
                                break;

                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                if (mm5.ParamValues != null)
                                {
                                    Model.ReportSqlParams_DataTime mm6 = new ReportSqlParams_DataTime();
                                    mm6.ParamValues = DateTime.Now;
                                    foreach (var xxd in pageParam.ParamList)
                                    {
                                        if (xxd.ParamName == "EndTime")
                                        {
                                            mm6 = (Model.ReportSqlParams_DataTime)xxd;
                                            break;
                                        }
                                    }
                                    if (mm5.ParamValues == mm6.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.SysDate >= mm5.ParamValues &&
                                                      b.SysDate < DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.SysDate >= mm5.ParamValues && b.SysDate <= mm6.ParamValues
                                                      select b;
                                    }
                                }
                                break;
                            case "EndTime":
                                Model.ReportSqlParams_DataTime ed = (Model.ReportSqlParams_DataTime)item;
                                if (ed.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.SysDate < DateTime.Parse(ed.ParamValues.ToString()).AddDays(1)
                                                  select b;


                                }
                                break;

                            case "ApplyUser":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.ApplyUser.Contains(para.ParamValues.ToString())
                                                  select b;
                                }
                                break;

                            case "AduitID":
                                Model.ReportSqlParams_String para2 = (Model.ReportSqlParams_String)item;
                                if (para2.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.AduitID.Contains(para2.ParamValues)
                                                  select b;
                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                if (para1.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where para1.ParamValues.Contains(b.HallID)
                                                  select b;

                                }
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

                        List<Model.View_Pro_SellAduit> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_Pro_SellAduit> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
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


        /// <summary>
        /// 三级审批查询
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn Search3(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                    if (!result.ReturnValue)
                    { return new WebReturn() { ReturnValue = false, Obj = pageParam }; }

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

                    var aduit_query = from b in lqh.Umsdb.View_Pro_SellAduit
                                      where b.HasAduited2 ==true && b.HasPassed2 ==true
                                      select b;
                    foreach (var item in pageParam.ParamList)  
                    {
                        switch (item.ParamName)
                        {
                            case "Aduited3":
                                Model.ReportSqlParams_Bool mm = (Model.ReportSqlParams_Bool)item;

                                if (mm.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited3 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasAduited3 != true
                                                  select b;
                                }
                                break;

                            case "Passed3":
                                Model.ReportSqlParams_Bool pass = (Model.ReportSqlParams_Bool)item;


                                if (pass.ParamValues)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed3 == true
                                                  select b;
                                }
                                else
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.HasPassed3 != true
                                                  select b;
                                }
                                break;

                            case "Used":
                                Model.ReportSqlParams_Bool use = (Model.ReportSqlParams_Bool)item;

                            
                                aduit_query = from b in aduit_query
                                              where b.HasUsed == use.ParamValues
                                              select b;
                                break;

                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                if (mm5.ParamValues != null)
                                {
                                    Model.ReportSqlParams_DataTime mm6 = new ReportSqlParams_DataTime();
                                    mm6.ParamValues = DateTime.Now;
                                    foreach (var xxd in pageParam.ParamList)
                                    {
                                        if (xxd.ParamName == "EndTime")
                                        {
                                            mm6 = (Model.ReportSqlParams_DataTime)xxd;
                                            break;
                                        }
                                    }
                                    if (mm5.ParamValues == mm6.ParamValues)
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.SysDate >= mm5.ParamValues &&
                                                      b.SysDate < DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                                                      select b;
                                    }
                                    else
                                    {
                                        aduit_query = from b in aduit_query
                                                      where b.SysDate >= mm5.ParamValues
                                                      && b.SysDate <= DateTime.Parse(mm6.ParamValues.ToString()).AddDays(1)
                                                      select b;
                                    }
                                }
                                break;
                            case "EndTime":
                                Model.ReportSqlParams_DataTime ed = (Model.ReportSqlParams_DataTime)item;
                                if (ed.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.SysDate < DateTime.Parse(ed.ParamValues.ToString()).AddDays(1)
                                                  select b;


                                }
                                break;
                            case "ApplyUser":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.ApplyUser.Contains(para.ParamValues.ToString())
                                                  select b;
                                }
                                break;

                            case "AduitID":
                                Model.ReportSqlParams_String para2 = (Model.ReportSqlParams_String)item;
                                if (para2.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where b.AduitID.Contains(para2.ParamValues)
                                                  select b;
                                }
                                break;
                            case "HallID":
                                Model.ReportSqlParams_ListString para1 = (Model.ReportSqlParams_ListString)item;
                                if (para1.ParamValues != null)
                                {
                                    aduit_query = from b in aduit_query
                                                  where para1.ParamValues.Contains(b.HallID)
                                                  select b;

                                }
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

                        List<Model.View_Pro_SellAduit> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_Pro_SellAduit> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
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


        /// <summary>
        /// 获取商品所有原批发价格   批发申请页面
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn GetProOldPriceList(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region "验证用户操作仓库  商品的权限 "

                    List<string> hallids = new List<string>();
                    List<string> classids = new List<string>();
                   
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                    if (ret.ReturnValue != true)
                    { return new WebReturn() { ReturnValue = true, Obj =new List<View_SellTypeProduct>() }; }
                 
                   
                    #endregion
                    var query = from list in lqh.Umsdb.View_SellTypeProduct
                                where ValidProIDS.Contains(list.ClassID)
                                select list;
                    return new WebReturn() { ReturnValue = true,Obj=query.ToList()};
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue=false,Message = ex.Message};
                }
            }
        }


        /// <summary>
        /// 212
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user,List<int> idList)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);
                        if (!result.ReturnValue)
                        { return result; }

                        #region  验证用户删除权限


                        if (user.CancelLimit == null || user.CancelLimit == 0)
                        {
                            return new WebReturn() { ReturnValue = false, Message = "权限不足，删除失败！" };
                        }

                        #endregion 

                      

                        var aduit = from a in lqh.Umsdb.Pro_SellAduit
                                    where idList.Contains(a.ID)
                                    select a;

                        if (aduit.Count() != idList.Count)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分记录已删除，删除失败！" };
                        }
                        var usedlist = from a in aduit
                                       where a.Used == true
                                       select a;
                        if (usedlist.Count()>0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分审批单已使用，删除失败！" };
                        }

                        lqh.Umsdb.Pro_SellAduit.DeleteAllOnSubmit(aduit);

                        List<Model.Pro_SellAduit_bak> list = new List<Pro_SellAduit_bak>();
                        foreach (var model in aduit)
                        {
                            Model.Pro_SellAduit_bak sa = new Model.Pro_SellAduit_bak();
                            sa.AduitDate = model.AduitDate;
                            sa.Aduited = model.Aduited;
                            sa.AduitID = model.AduitID;
                            sa.AduitUser = model.AduitUser;
                            sa.ApplyDate = model.ApplyDate;
                            sa.ApplyUser = model.ApplyUser;
                            sa.CustName = model.CustName;
                            sa.CustPhone = model.CustPhone;
                            sa.HallID = model.HallID;
                            sa.ID = model.ID;
                            sa.Money = model.Money;
                            sa.Note = model.Note;
                            sa.Passed = model.Passed;
                            sa.SysDate = model.SysDate;
                            sa.Used = model.Used;
                            sa.UseDate = model.UseDate;
                            list.Add(sa);
                        }
                        lqh.Umsdb.Pro_SellAduit_bak.InsertAllOnSubmit(list);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }
    }
}
