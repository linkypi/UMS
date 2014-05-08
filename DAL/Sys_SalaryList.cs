using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    class Sys_SalaryList
    {
        private int MethodID;

	    public Sys_SalaryList()
	    {
		    this.MethodID = 0;
	    }

        public Sys_SalaryList(int MethodID)
	    {
		    this.MethodID = MethodID;
	    }

        /// <summary>
        /// 356
        /// </summary>
        /// <param name="user"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn GetSalaryByPros(Model.Sys_UserInfo user, List<Model.SalaryBill> models, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    List<int> promains = new List<int>();
                    List<string> pros = new List<string>();
                    foreach (var item in models)
                    {
                        if (item.IsProMain)
                        {
                            promains.Add(item.ProMainID);
                        }
                        else
                        {
                            pros.Add(item.ProID);
                        }
                    }

                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    var aduit_query = from a in lqh.Umsdb.View_SalaryWithPercent
                                      where promains.Contains((int)a.ProMainID) || pros.Contains(a.ProID)
                                      select a;
            

                    pageParam.RecordCount = aduit_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in aduit_query.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_SalaryWithPercent> aduitList = results.ToList();
                        foreach (var item in aduitList)
                        {
                            if (Convert.ToInt32(item.ProMainID) == 0)
                            {
                                item.IsMainPro = false;
                            }
                            else
                            {
                                item.IsMainPro = true;
                            }
                        }
                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in aduit_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_SalaryWithPercent> aduitList = results.ToList();
                        foreach (var item in aduitList)
                        {
                            if (Convert.ToInt32(item.ProMainID) == 0)
                            {
                                item.IsMainPro = false;
                            }
                            else
                            {
                                item.IsMainPro = true;
                            }
                        }
                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion 
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};   
                }
            }
        }

        /// <summary>
        /// 346
        /// </summary>
        /// <param name="user"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn CheckSalaryWithPercent(Model.Sys_UserInfo user,List<Model.SalaryBill> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    if (models.Count <= 0) { return new Model.WebReturn() { ReturnValue = false, Message = "请添加数据！" }; }

                    List<Model.SalaryBill> promains = new List<Model.SalaryBill>();
                    List<Model.SalaryBill> pros = new List<Model.SalaryBill>();

                    #region  验证用户菜单权限

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                    if (!result.ReturnValue)
                    { return result; }

                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    List<string> classids = new List<string>();


                    #endregion

                    #region  验证数据
                  
                    List<int> promainids = new List<int>();
                    List<string> proids = new List<string>();
                    List<int> days = new List<int>();
                    List<decimal> salarys = new List<decimal>();
                    List<decimal> ratios = new List<decimal>();
                    List<int> selltypes = new List<int>();
                    List<decimal> overRatios = new List<decimal>();

                    foreach (var item in models)
                    {
                        if (item.IsProMain)
                        {
                            promains.Add(item);
                            promainids.Add(item.ProMainID);
                        }
                        else
                        {
                            pros.Add(item);
                            proids.Add(item.ProID);
                        }
                        if (string.IsNullOrEmpty(item.ProMainID.ToString()) && (string.IsNullOrEmpty(item.ProID)))
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "请确保商品编码和总商品编码必有其一！" }; ;
                        }
                        foreach (var child in item.Children)
                        {
                            days.Add(child.Day);
                            salarys.Add(child.BaseSalary);
                            ratios.Add(child.Ratio);
                            overRatios.Add(child.OverRatio);
                           
                        }
                        selltypes.Add(item.SellType);
                    }

                    #region  验证数据

                    var main = (from a in lqh.Umsdb.Pro_ProMainInfo
                                select a.ProMainID).ToList();
                    var valmain = from a in promainids
                                    where main.Contains(a)==false
                                    select a;
                    if (valmain.Count() > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "总商品编码不存在：" + valmain.First() + " ！" };
                    }
                 

                    var pro = from a in lqh.Umsdb.Pro_ProInfo
                                select a.ProID;
                    var valpro = from a in proids
                                    where pro.Contains(a) == false
                                    select a;
                    if (valpro.Count() > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "商品编码不存在：" + valpro.First() + " ！" };
                    }

                    var valoratio = from a in overRatios
                                  where a > 1 && a < 0
                                  select a;
                    if (valoratio.Count() > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "超额比例有误: " + valoratio.First() + " ！" };
                    }

                    var valdays = from a in days
                               where a > 31 && a < 1
                               select a;
                    if (valdays.Count() > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "日期无效: " + days.First() + " ！" };
                    }

                    var salary = from a in salarys 
                                 where a < 0
                                 select a;
                    if (salary.Count() > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "基本提成不能小于零！" };
                    }

                    var st = from a in lqh.Umsdb.Pro_SellType
                             select a .ID  ;
                    var valst = from m in selltypes
                                where st.Contains(m) == false
                                select m;

                    if (valst.Count() > 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "销售类别不存在！" };
                    }
               
                    #endregion 

                    #endregion

                    #region  验证商品权限

                    if (ValidProIDS.Count > 0)
                    {
                        var que = from c in classids
                                  join p in lqh.Umsdb.Pro_ClassInfo
                                  on c equals p.ClassID.ToString()
                                  where !ValidProIDS.Contains(c)
                                  select p;

                        if (que.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "您无权操作该商品：" + que.First().ClassName };
                        }
                    }

                    #endregion

                    List<Model.SalaryBill> retList = new List<Model.SalaryBill>();
                   
                    //加载总商品
                    var val = (from m in promains
                              join a in lqh.Umsdb.Pro_ProMainInfo
                              on m.ProMainID equals a.ProMainID
                              join c in lqh.Umsdb.Pro_ClassInfo
                              on a.ClassID equals c.ClassID
                              join t in lqh.Umsdb.Pro_TypeInfo
                              on a.TypeID equals t.TypeID
                              join s in lqh.Umsdb.Pro_SellType
                              on m.SellType equals s.ID
                              where m.IsProMain ==true
                              select new
                              {
                                  c.ClassName,
                                  t.TypeName,
                                  a.ProMainName,
                                  m.ProMainID,
                                  c.ClassID,
                                  s.Name,
                                  SellTypeID = s.ID,
                                  m.StartDate,
                                  m.EndDate,
                                  m.Children
                              }).Distinct();
                    if (val.Count() > 0)
                    {
                        foreach (var child in val)
                        {
                            Model.SalaryBill sb = new Model.SalaryBill();
                            sb.ProName = child.ProMainName;
                            sb.TypeName = child.TypeName;
                            sb.ClassName = child.ClassName;
                            sb.ClassID = child.ClassID.ToString();
                            sb.IsProMain = true;
                            sb.SellType = child.SellTypeID;
                            sb.EndDate = child.EndDate;
                            sb.StartDate = child.StartDate;
                            sb.SellTypeName = child.Name;
                            sb.ProMainID = child.ProMainID;
                            sb.Children = new List<Model.SalaryBillChild>();
                            sb.Children.AddRange(child.Children);
                            retList.Add(sb);
                        }
                    }

                    //添加属性商品
                    var val2 = from m in pros
                              join p in lqh.Umsdb.Pro_ProInfo
                              on m.ProID equals p.ProID
                              join c in lqh.Umsdb.Pro_ClassInfo
                              on p.Pro_ClassID equals c.ClassID
                              join t in lqh.Umsdb.Pro_TypeInfo
                              on p.Pro_TypeID equals t.TypeID
                               join s in lqh.Umsdb.Pro_SellType
                             on m.SellType equals s.ID
                              where m.IsProMain == false
                              select new
                              {
                                  p.ProName,
                                  c.ClassName,
                                  t.TypeName,
                                  c.ClassID,
                                  p.ProID,
                                  m.StartDate,
                                  m.EndDate,
                                  s.Name,SellTypeID = s.ID ,
                                  m.Children
                              };
                    if (val2.Count() > 0)
                    {
                        foreach (var child in val2)
                        {
                            Model.SalaryBill sb = new Model.SalaryBill();
                            sb.ProName = child.ProName;
                            sb.TypeName = child.TypeName;
                            sb.ProID = child.ProID;
                            sb.SellTypeName = child.Name;
                            sb.SellType = child.SellTypeID;
                            sb.EndDate = child.EndDate;
                            sb.StartDate = child.StartDate;
                            sb.IsProMain = false;
                            sb.ClassID = child.ClassID.ToString();
                            sb.ClassName = child.ClassName;
                            sb.Children = new List<Model.SalaryBillChild>();
                            sb.Children.AddRange(child.Children);
                            retList.Add(sb);
                        }
                    }

                    return new Model.WebReturn() { ReturnValue = true, Obj = retList };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }

        /// <summary>
        /// 获取商品提成
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetProSalary(Model.Sys_UserInfo user,List<Model.SalaryBill> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                foreach(var item in models)
                {
                    //if (item.Children != null)
                    //{
                    //    item.Children.Clear();
                    //}
                    var query = from p in lqh.Umsdb.View_SalaryList
                                where p.ProID == item.ProID
                                select p;
                    if (query.Count() != 0)
                    {
                        foreach (var child in query)
                        {
                            Model.SalaryBillChild m = new Model.SalaryBillChild();
                            m.ID = child.ID;
                            m.OpID = Convert.ToInt32(child.OpID);
                            m.SpecalSalary = Convert.ToDecimal(child.SpecialSalary);
                            m.BaseSalary =Convert.ToDecimal( child.BaseSalary);
                            m.OldBaseSalary = Convert.ToDecimal(child.BaseSalary);
                            m.EndDate = Convert.ToDateTime(child.EndDate);
                            m.OldEndDate = Convert.ToDateTime(child.EndDate);
                            m.StartDate = Convert.ToDateTime(child.StartDate);
                            m.OldStartDate = Convert.ToDateTime(child.StartDate);
                            m.SellTypeName = child.SellTypeName;
                            m.SellTypeID =Convert.ToInt32(child.SellType);
                            m.UpdateFlag = Convert.ToBoolean(child.UpdateFlag);
                            if (item.Children == null)
                            {
                                item.Children = new List<Model.SalaryBillChild>();
                            }
                            item.Children.Add(m);
                        }
                    }
                }
                return new Model.WebReturn() { ReturnValue = true, Obj = models };
            }
        }

        /// <summary>
        /// 添加/更新提成  160
        /// </summary>
        /// <param name="user"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Sys_SalaryChange header, List<string> classids)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //lqh.Umsdb.CommandTimeout = 15 * 60;
                TransactionOptions transactionOption = new TransactionOptions();

                //设置事务隔离级别
                transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

                // 设置事务超时时间为60秒
                transactionOption.Timeout = new TimeSpan(0, 15, 0);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
             
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

                        #region  验证商品权限

                        if (ValidProIDS.Count > 0)
                        {
                            var que = from c in classids
                                      join p in lqh.Umsdb.Pro_ClassInfo
                                      on c equals p.ClassID.ToString()
                                      where !ValidProIDS.Contains(c)
                                      select p;

                            if (que.Count() > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "您无权操作该商品：" + que.First().ClassName };
                            }
                        }

                        #endregion

                        string changedid = string.Empty;
                        lqh.Umsdb.OrderMacker(1, "SM", "SM", ref changedid);
                        if (changedid == "")
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "提成单号生成出错" };
                        }
                        #region 

                        header.ChangeID = changedid;
                        header.SysDate = DateTime.Now;
                        header.UserID = user.UserID;
                        lqh.Umsdb.Sys_SalaryChange.InsertOnSubmit(header);
                        lqh.Umsdb.SubmitChanges();

                        #endregion 

                        #region   同步到销售明细selllistinfo中的提成

                        lqh.Umsdb.UpdateSellSalary();

                        #endregion 

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true,Message="保存成功"};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false,Message=ex.Message};
                    }
                }
            }

        }

        public Model.WebReturn SearchMySalary(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //#region 权限

                    //List<string> ValidHallIDS = new List<string>();
                    ////有权限的商品
                    //List<string> ValidProIDS = new List<string>();

                    //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    //if (ret.ReturnValue != true)
                    //{ return ret; }

                    //#endregion

                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }

                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();
                     
                    #region "过滤数据"

                    DateTime sdate = new DateTime();
                    DateTime edate =   DateTime.Now;
                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                                sdate =(DateTime) mm5.ParamValues;
                                break;
                            case "EndTime":
                                Model.ReportSqlParams_DataTime eda = (Model.ReportSqlParams_DataTime)item;
                                edate = (DateTime)eda.ParamValues;
                                break;
                        }
                    }
                    //lqh.Umsdb.GenerateSalaryByDate(args[0], args[1]);  
                    var aduit_query = lqh.Umsdb.Proc_SalaryReportDetail(sdate.ToShortDateString(),
                        edate.ToShortDateString(),user.UserName,"");


                    List<Model.Proc_SalaryReportDetailResult> list = aduit_query.ToList();

                    #endregion

                    pageParam.RecordCount = list.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in list.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.Proc_SalaryReportDetailResult> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in list.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.Proc_SalaryReportDetailResult> aduitList = results.ToList();

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false};
                }
            }
        }

        /// <summary>
        /// 获取提成报表   183
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetReportSalary(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                lqh.Umsdb.CommandTimeout = 15 * 60;
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

                    //if (pageParam == null || pageParam.PageIndex < 0 )
                    //{
                    //    return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    //}
                    //if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"
                     List<string> args = new List<string>();

         
                    foreach (var item in pageParam.ParamList)
                    {
                        Model.ReportSqlParams_String ar = (Model.ReportSqlParams_String)item;
                        args.Add(ar.ParamValues);
                    }
                    string sdate = args[0]==""?"2000-1-1":args[0];
                    if (DateTime.Parse(sdate) > DateTime.Now)
                    {
                        pageParam.RecordCount = 0;
                        pageParam.Obj = null;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    //IList<Model.Proc_MySalaryResult>  list= lqh.Umsdb.Proc_MySalary().ToList();
                   // lqh.Umsdb.GenerateSalaryByDate(args[0],args[1]);
                   var query = lqh.Umsdb.Proc_SalaryReport(args[0],args[1],args[2],args[3]).ToList();

                   List<Model.Proc_SalaryReportResult> list = query.ToList();
                    #endregion
                    
                     pageParam.RecordCount = list.Count;

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in list.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.Proc_SalaryReportResult> aduitList = results.ToList();
                        Get(lqh, args, aduitList);


                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in list.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.Proc_SalaryReportResult> aduitList = results.ToList();
                        Get(lqh, args, aduitList);
                        pageParam.Obj = aduitList; 
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false ,Message=ex.Message};
                }
            }
        }

        private static void Get(LinQSqlHelper lqh, List<string> args, List<Model.Proc_SalaryReportResult> aduitList)
        {
            string seller = "";
            int index = 1;
            foreach (var item in aduitList)
            {
                seller += item.Seller;
                if (index < aduitList.Count)
                {
                    seller += ",";
                }
                index++;
            }
            List<Model.Proc_SalaryReportDetailResult> children = lqh.Umsdb.Proc_SalaryReportDetail(args[0], args[1], seller, args[3]).ToList();
            foreach (var child in aduitList)
            {
                var list2 = from a in children
                            where a.Seller == child.Seller
                            select a;
                child.Children = new List<Model.Proc_SalaryReportDetailResult>();
                child.Children.AddRange(list2.ToList());
            }
        }

        /// <summary>
        /// 获取提成报表明细  184
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetReportSalaryDetail(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
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

                    //if (pageParam == null || pageParam.PageIndex < 0 )
                    //{
                    //    return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    //}
                    //if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    #region "过滤数据"
                    List<string> args = new List<string>();

                    foreach (var item in pageParam.ParamList)
                    {
                        Model.ReportSqlParams_String ar = (Model.ReportSqlParams_String)item;
                        args.Add(ar.ParamValues);
                    }

                    string sdate = args[0] == "" ? "2000-1-1" : args[0];
                    if (DateTime.Parse(sdate) > DateTime.Now)
                    {
                        pageParam.RecordCount = 0;
                        pageParam.Obj = null;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    //lqh.Umsdb.GenerateSalaryByDate(args[0], args[1]);
                    var query = lqh.Umsdb.Proc_SalaryReportDetail(args[0], args[1], args[2], args[3]).ToList();

                    // List<Model.Proc_SalaryReportResult> list = GetDetail(query.ToList(),args.ToArray(),lqh);

                    List<Model.Proc_SalaryReportDetailResult> retlist = query.ToList();
                    

                    #endregion

                    pageParam.RecordCount = retlist.Count;

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in retlist.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.Proc_SalaryReportDetailResult> aduitList = results.ToList();
                        if (aduitList.Count >= 60000)
                        {
                            return new Model.WebReturn() { ReturnValue=false,Message="数据量过大，导出失败！"};
                        }

                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in retlist.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.Proc_SalaryReportDetailResult> aduitList = results.ToList();
                        if (aduitList.Count >= 60000)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "数据量过大，导出失败！" };
                        }
                        pageParam.Obj = aduitList;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }

        /// <summary>
        /// 获取提成报表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetSalaryPlanReport(Model.Sys_UserInfo user,int pageindex,int pagesize)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from rp in lqh.Umsdb.View_SalaryPlanReport
                                select rp;
                    int total = query.Count();
                    var result = query.Skip(pageindex * pagesize).Take(pagesize);
                    return new Model.WebReturn() { ReturnValue = true, Obj = result.ToList(), ArrList = new System.Collections.ArrayList() { total} };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue =false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 删除提成方案
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public Model.WebReturn DeleteSellType(Model.Sys_UserInfo user, List<int> idlist)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope t = new TransactionScope())
                {
                    try
                    {
                        var salary = from s in lqh.Umsdb.Sys_SalaryList
                                     where idlist.Contains(s.ID)
                                     select s;
                        lqh.Umsdb.Sys_SalaryList.DeleteAllOnSubmit(salary.ToList());

                        lqh.Umsdb.SubmitChanges();
                        t.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功", Obj = idlist };
                    }
                    catch (System.Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                    }
                 
                }
            }
        }

        /// <summary>
        /// 导入Excel后生成提成   292 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public Model.WebReturn GenerateSalary(Model.Sys_UserInfo user, List<Model.SalaryBill> models)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    if (models.Count <= 0) { return new Model.WebReturn() { ReturnValue = false, Message="请添加数据！"}; }
                   
                    #region  验证用户菜单权限

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                    if (!result.ReturnValue)
                    { return result; }

                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS, lqh);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    List<string> classids = new List<string>();
                 
                
                    #endregion

                    #region  验证数据
                    int index = 0;
                    foreach (var item in models)
                    {
                        index++;
                  
                            if (string.IsNullOrEmpty(item.ProMainID.ToString()) && (string.IsNullOrEmpty(item.ClassName)
                           || string.IsNullOrEmpty(item.TypeName) || string.IsNullOrEmpty(item.ProName)))
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "请确保商品信息完整" }; ;
                            }

                            if (item.ProMainID != 0)
                            {
                                var main = from a in lqh.Umsdb.Pro_ProMainInfo
                                           where a.ProMainID == item.ProMainID
                                           select a;
                                if (main.Count() == 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "总商品编码有误：" + item.ProMainID + " ！" };
                                }
                            }
                            else
                            {
                                var pro = from a in lqh.Umsdb.Pro_ProInfo
                                          join c in lqh.Umsdb.Pro_ClassInfo
                                          on a.Pro_ClassID equals c.ClassID
                                          join t in lqh.Umsdb.Pro_TypeInfo
                                          on a.Pro_TypeID equals t.TypeID
                                          where a.ProName == item.ProName && c.ClassName == item.ClassName
                                          && t.TypeName == item.TypeName
                                          select a;
                                if (pro.Count() == 0)
                                {
                                    return new Model.WebReturn() { ReturnValue = false, Message = "商品不存在：" + item.ClassName + "  " + item.TypeName + "  " + item.ProName + " ！" };
                                }
                                item.ProID = pro.First().ProID;
                                classids.Add(pro.First().Pro_ClassID.ToString());
                            }

                            var days = from a in item.Children
                                       where a.Day > 31 && a.Day < 1
                                       select a;
                            if (days.Count() > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "日期无效: " + days.First().Day + " ！" };
                            }

                            var salary = from a in item.Children
                                         where a.BaseSalary < 0
                                         select a;
                            if (salary.Count() > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "基本提成不能小于零！" };
                            }

                            var st = from a in item.Children
                                     join b in lqh.Umsdb.Pro_SellType
                                     on a.SellTypeName equals b.Name
                                     select new
                                     {
                                         b.ID,
                                         a.SellTypeName
                                     };
                            if (st.Count() == 0)
                            {
                                continue;
                            }
                            if (st.Count() != item.Children.Count())
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "销售类别：" + st.First().SellTypeName + "不存在！" };
                            }
                            else
                            {
                                int stid = st.First().ID;
                                foreach (var xx in item.Children)
                                {
                                    xx.SellTypeID = stid;
                                }
                            }
                      
                       
                    }

                    #endregion 

                    #region  验证商品权限

                    if (ValidProIDS.Count > 0)
                    {
                        var que = from c in classids
                                  join p in lqh.Umsdb.Pro_ClassInfo
                                  on c equals p.ClassID.ToString()
                                  where !ValidProIDS.Contains(c)
                                  select p;

                        if (que.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message ="您无权操作该商品：" + que.First().ClassName };
                        }
                    }

                    #endregion


                    List<Model.SalaryBill> retList = new List<Model.SalaryBill>();
                    //添加属性商品

                    var list = from a in models
                               where a.ProMainID == 0
                               select a;
                    if (list.Count() > 0)
                    {
                        foreach (var item in list)
                        {
                            retList.Add(item);
                        }
                    }
                    //foreach (var item in models)
                    //{
                    //    if (item.ProMainID != 0) { continue; }
                    //    Model.SalaryBill sb = new Model.SalaryBill();
                    //    sb.ProName = item.ProName;
                    //    sb.TypeName = item.TypeName;
                    //    sb.ProID = item.ProID;
                    //    sb.ClassName = item.ClassName;
                    //    sb.Children = new List<Model.SalaryBillChild>();
                    //    sb.Children.AddRange(item.Children);
                    //    retList.Add(sb);
                    //}

                    //var val1 = from a in lqh.Umsdb.Pro_ProMainInfo
                    //          join p in lqh.Umsdb.Pro_ProInfo
                    //          on a.ProMainID equals p.ProMainID
                    //          join m in models
                    //          on a.ProMainID equals m.ProMainID
                    //          join c in lqh.Umsdb.Pro_ClassInfo
                    //          on p.Pro_ClassID equals c.ClassID
                    //          join t in lqh.Umsdb.Pro_TypeInfo
                    //          on p.Pro_TypeID equals t.TypeID
                    //          where m.ProMainID != 0
                    //          select m.ProMainID ;
                    ////into temp
                    //          //from x in models
                    //          //where temp!=x
                    //          //select x
                    //var ss = from a in models
                    //         where !val1.Contains(a.ProMainID)
                    //         select a;
                    //if(ss.Count()>0)
                    //{
                    //    return new Model.WebReturn() { ReturnValue = false, Message = "总商品编码有误：" + ss.First().ProMainID + " ！" };
                    //}

                    var val =  from m in models
                            join a in lqh.Umsdb.Pro_ProMainInfo
                            on m.ProMainID equals a.ProMainID
                              join p in lqh.Umsdb.Pro_ProInfo
                              on a.ProMainID equals p.ProMainID
                            
                              //on a.ProMainID equals m.ProMainID
                              join c in lqh.Umsdb.Pro_ClassInfo
                                   on p.Pro_ClassID equals c.ClassID
                              join t in lqh.Umsdb.Pro_TypeInfo
                              on p.Pro_TypeID equals t.TypeID
                              //join st in lqh.Umsdb.Pro_SellType
                             
                              where m.ProMainID != 0
                              select new
                              {
                                  p.ProName,
                                  c.ClassName,
                                  t.TypeName,
                                  p.ProID,
                                  m.Children
                              };
                    if (val.Count() > 0)
                    {
                        foreach (var child in val)
                        {
                            Model.SalaryBill sb = new Model.SalaryBill();
                            sb.ProName = child.ProName;
                            sb.TypeName = child.TypeName;
                            sb.ProID = child.ProID;
                            sb.ClassName = child.ClassName;
                            sb.Children = new List<Model.SalaryBillChild>();
                            sb.Children.AddRange(child.Children);
                            retList.Add(sb);
                        }
                    }

                    //添加总商品
             
                    return new Model.WebReturn() {ReturnValue=true,Obj = retList };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() {ReturnValue  =false,Message=ex.Message };
                }
            }
        }

        /// <summary>
        /// 同步提成   293
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn AsynSalary(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
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

                    //1.备份提成旧数据
                    //2.删除后添加新提成到实时表
                    //3.同步selllistinfo中提成为0 的数据
                   // lqh.Umsdb.AsynSalary(user.UserID,"");
                    return new Model.WebReturn() { Message="同步完成！",ReturnValue =true };
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                }
            }
        }

        /// <summary>
        /// 299
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.WebReturn GetCurrentSalary(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                var list = from a in lqh.Umsdb.View_CurrentSalary
                           orderby a.ProID 
                           orderby a.SellType 
                           orderby a.SalaryYear
                           orderby a.SalaryMonth
                           orderby a.SalaryDay
                           select a;

                return new Model.WebReturn() { ReturnValue=true,Obj = list.ToList()};

            }

        }

        /// <summary>
        /// 獲取各個倉庫總提成 316
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sdate"></param>
        /// <param name="edate"></param>
        /// <returns></returns>
        public Model.WebReturn GetEveryHallSalary(Model.Sys_UserInfo user,DateTime sdate,DateTime edate)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //var list = lqh.Umsdb.EveryHallTotalSalary(sdate.ToShortDateString(),edate.ToShortDateString());

                return new Model.WebReturn() { ReturnValue = true};

            }

        }


        /// <summary>
        /// 添加提成  347
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Model.WebReturn AddWithPercent(Model.Sys_UserInfo user, List<Model.SalaryBill> list, string note)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                lqh.Umsdb.CommandTimeout = 15 * 60;
                TransactionOptions transactionOption = new TransactionOptions();

                //设置事务隔离级别
                transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

                // 设置事务超时时间为60秒
                transactionOption.Timeout = new TimeSpan(0, 15, 0);
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
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

                        #region  验证商品权限

                        //var classids = (from a in model.Sys_SalaryBillListInfo
                        //                select Convert.ToInt32(a.ClassID)).ToList();

                        //if (ValidProIDS.Count > 0)
                        //{
                        //    var que = from c in classids
                        //              join p in lqh.Umsdb.Pro_ClassInfo
                        //              on c equals p.ClassID
                        //              where !ValidProIDS.Contains(c.ToString())
                        //              select p;

                        //    if (que.Count() > 0)
                        //    {
                        //        return new Model.WebReturn() { ReturnValue = false, Message = "您无权操作该商品：" + que.First().ClassName };
                        //    }
                        //}

                        #endregion

                        #region 验证数据


                        //var valratio = from a in model.Sys_SalaryBillListInfo
                        //               where a.Ratio > 1 || a.Ratio < 0
                        //               select a;
                        //if (valratio.Count() > 0)
                        //{
                        //    Model.Sys_SalaryBillListInfo m = valratio.First();
                        //    var st = from a in lqh.Umsdb.Pro_SellType
                        //             where a.ID == m.SellType
                        //             select a;
                        //    var pro = from a in lqh.Umsdb.Pro_ProInfo
                        //              where a.ProID == m.ProID
                        //              select a;
                        //    return new Model.WebReturn()
                        //    {
                        //        ReturnValue = false,
                        //        Message = "商品 " + pro.First().ProName + " 中的 " + st.First().Name + " 百分比有误！"
                        //    };
                        //}
                        #endregion

                        var stepinfo = from a in lqh.Umsdb.Sys_SalaryPriceStep
                            select a;

                        #region  更新最新提成

                        Model.Sys_SalaryBillInfo model = new Model.Sys_SalaryBillInfo();
                        List<Model.Sys_CurrentSalary> models = new List<Model.Sys_CurrentSalary>();

                        model.Note = note;
                        model.SysDate = DateTime.Now;
                        model.UserID = user.UserID;
                        model.Sys_SalaryBillListInfo = new System.Data.Linq.EntitySet<Model.Sys_SalaryBillListInfo>();
                        foreach (var item in list)
                        {
                            Model.Sys_SalaryBillListInfo b = new Model.Sys_SalaryBillListInfo();
                            b.BeginDate = item.StartDate;
                            b.EndDate = item.EndDate;
                            b.ProID = item.ProID;
                            b.ProMainID = item.ProMainID;
                            b.SellType = item.SellType;
                            b.Sys_SalaryList_StepInfo = new System.Data.Linq.EntitySet<Model.Sys_SalaryList_StepInfo>();

                            var pros = new List<string>();
                            if (string.IsNullOrEmpty(item.ProID))
                            {
                                 pros = (from a in lqh.Umsdb.Pro_ProMainInfo
                                           join p in lqh.Umsdb.Pro_ProInfo
                                           on a.ProMainID equals p.ProMainID
                                           where a.ProMainID == item.ProMainID
                                           select p.ProID).ToList();

                            }
                            foreach (var child in item.Children)
                            {
                                 Model.Sys_SalaryList_StepInfo s = new Model.Sys_SalaryList_StepInfo();
                                 s.BaseSalary = child.BaseSalary;
                                 s.OverNum = child.OverNum;
                                 s.OverRate = child.OverRatio;

                                 if (stepinfo.Where(a => a.PriceNum == child.Step).Count() > 0)
                                 {
                                     s.StepID = stepinfo.Where(a => a.PriceNum == child.Step).First().ID;
                                 }
                                 else
                                 {
                                     Model.Sys_SalaryPriceStep x = new Model.Sys_SalaryPriceStep();
                                     x.PriceNum = child.Step;
                                     lqh.Umsdb.Sys_SalaryPriceStep.InsertOnSubmit(x);
                                     lqh.Umsdb.SubmitChanges();
                                     s.StepID = x.ID;
                                 }
                                 b.Sys_SalaryList_StepInfo.Add(s);

                                 if (string.IsNullOrEmpty(item.ProID))
                                 {
                                     foreach (var xx in pros)
                                     {
                                        Model.Sys_CurrentSalary sc = new Model.Sys_CurrentSalary();
                                        sc.BaseSalary = child.BaseSalary;
                                        sc.EndDate = item.EndDate;
                                        sc.StartDate = item.StartDate;
                                        sc.SellType = item.SellType;
                                        sc.StartDate = item.StartDate;
                                        sc.OverNum = child.OverNum;
                                        sc.OverRatio = child.OverRatio;
                                        sc.PriceNum = child.Step;
                                        sc.ProID = xx;
                                        models.Add(sc);
                                     }
                                 }
                                 else
                                 {
                                     Model.Sys_CurrentSalary sc = new Model.Sys_CurrentSalary();
                                     sc.BaseSalary = child.BaseSalary;
                                     sc.EndDate = item.EndDate;
                                     sc.StartDate = item.StartDate;
                                     sc.SellType = item.SellType;
                                     sc.StartDate = item.StartDate;
                                     sc.OverNum = child.OverNum;
                                     sc.OverRatio = child.OverRatio;
                                     sc.PriceNum = child.Step;
                                     sc.ProID = item.ProID;
                                     models.Add(sc);
                                 }
                            }
                            model.Sys_SalaryBillListInfo.Add(b);
                        }
                       
                        lqh.Umsdb.Sys_SalaryBillInfo.InsertOnSubmit(model);
                        lqh.Umsdb.Sys_CurrentSalary.InsertAllOnSubmit(models);
                        lqh.Umsdb.SubmitChanges();

                        #endregion

                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Message = "保存成功！" };
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
