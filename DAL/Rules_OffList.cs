using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;

namespace DAL
{
    public class Rules_OffList
    {
         private int MethodID;

        public Rules_OffList()
        {
            this.MethodID = 0;
        }

        public Rules_OffList(int MenthodID)
        {
            this.MethodID = MenthodID; 
        }

        /// <summary>
        /// 生存规则    288
        /// </summary>
        /// <param name="user"></param>
        /// <param name="models"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public Model.WebReturn GenerateRules(Model.Sys_UserInfo user,List<Model.Rules_ImportModel> models,List<int> cls)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    #region 权限验证

                    Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                    if (!result.ReturnValue)
                    { return result; }

                    List<string> hallids = new List<string>();
                 
                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    #region  验证规则类型

                    List<string> types = new List<string>();
                    var typs = from a in lqh.Umsdb.Rules_TypeInfo
                               select a;
                    foreach (var item in typs)
                    {
                        types.Add(item.RulesName);
                    }
                    var val = from a in models
                              where !types.Contains(a.RulesName)
                              select a;
                    if (val.Count() > 0)
                    {
                        return new WebReturn() { ReturnValue = false, Message = "规则类型：" + val.First().RulesName + " 不存在！" }; ;
                    }
                    #endregion 

                    #region 验证商品是否都存在

                    var p = (from a in lqh.Umsdb.Pro_ProInfo
                             select a.ProName).Distinct().ToList();

                    var valid = from a in models
                                where !p.Contains(a.ProName)
                                select a;
                    if (valid.Count() >0)
                    {
                        string msg = "";
                        foreach (var item in valid)
                        {
                            msg += item.ProName;
                            break;
                        }
                        return new WebReturn() {ReturnValue=false,Message="商品 "+msg+" 不存在！" };
                    }

                    #endregion 

                    var promains = from b in models
                                   join a in lqh.Umsdb.Pro_ProInfo on b.ProName equals a.ProName
                                   join c in lqh.Umsdb.Pro_ProMainInfo on a.ProMainID equals c.ProMainID
                                   join d in lqh.Umsdb.Pro_TypeInfo on a.Pro_TypeID equals d.TypeID
                                   join e in lqh.Umsdb.Pro_ClassInfo on a.Pro_ClassID equals e.ClassID
                                   join t in lqh.Umsdb.Rules_TypeInfo  on b.RulesName equals t.RulesName
                                   where cls.Contains((int)a.Pro_ClassID)
                                   select new { 
                                   c.ProMainName, c.ProMainID,
                                   a.ProName, d.TypeName, e.ClassName,

                                   b.MaxPrice,  b.MinPrice,
                                   b.OffPrice, t.CanGetBack,
                                   t.ShowToCus, RulesTypeName = t.RulesName,
                                   RulesTypeID = t.ID
                                   };
                    List<Model.RulesProMain> list = new List<RulesProMain>();

                    foreach (var item in promains)
                    {
                        Pro_RulesTypeInfo rt = new Pro_RulesTypeInfo();
                        rt.RulesTypeName = item.RulesTypeName;
                        rt.RulesTypeID = item.RulesTypeID;
                        rt.ShowToCus = item.ShowToCus;
                        rt.OffPrice = item.OffPrice;
                        rt.MinPrice = item.MinPrice;
                        rt.MaxPrice = item.MaxPrice;
                        rt.CanGetBack = item.CanGetBack;

                        Model.RulesProMain erp = Exist(item.ProMainID, list);
                        if (erp == null)
                        {
                            RulesProMain rp = new RulesProMain();
                            rp.ClassName = item.ClassName;
                            rp.ProMainID = item.ProMainID;
                            rp.ProMainName = item.ProMainName;
                            rp.TypeName = item.TypeName;
                            if (rp.Pro_RulesTypeInfo == null)
                            {
                                rp.Pro_RulesTypeInfo = new List<Pro_RulesTypeInfo>();
                                rp.Pro_RulesTypeInfo.Add(rt);
                                list.Add(rp);
                                continue;
                            }

                            rp.Pro_RulesTypeInfo.Add(rt);
                            list.Add(rp);
                        }
                        else
                        {
                            if (erp.Pro_RulesTypeInfo == null)
                            {
                                erp.Pro_RulesTypeInfo = new List<Pro_RulesTypeInfo>();
                                erp.Pro_RulesTypeInfo.Add(rt);
                                continue;
                            }
                            bool exist = false;
                            foreach (var child in erp.Pro_RulesTypeInfo)
                            {
                                if (child.RulesTypeID == rt.RulesTypeID)
                                {
                                    exist = true;
                                    break;
                                }
                            }
                            if (!exist)
                            {
                                erp.Pro_RulesTypeInfo.Add(rt);
                            }
                        }
                
                    }

                    return new WebReturn() { ReturnValue = true,Obj=list, Message = "" };
                }
                catch (Exception ex)
                {
                    return new WebReturn() { ReturnValue=false,Message=ex.Message};
                }
            }
        }

        Model.RulesProMain Exist(int promainid, List<Model.RulesProMain> models)
        {
            foreach(var item in models)
            {
                if (item.ProMainID == promainid)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 修改套餐停止时间  289
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idList"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public Model.WebReturn Stop(Model.Sys_UserInfo user,List<int> idList,string enddate)
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

                        List<string> hallids = new List<string>();

                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion

                        var valRuels = from a in lqh.Umsdb.Rules_OffList
                                       where idList.Contains(a.ID) && a.Flag==true 
                                       select a;
                        if (valRuels.Count() != idList.Count)
                        {
                            List<int> list = new List<int>();
                            foreach (var item in valRuels)
                            {
                                list.Add(item.ID);
                            }
                            var rest = from a in idList
                                       join b in lqh.Umsdb.Rules_OffList on a equals b.ID
                                       where !list.Contains(a)
                                       select b;
                            return new WebReturn() { Message="规则："+rest.First().Note+ "不存在！",ReturnValue =false};
                        }

                        foreach (var item in valRuels)
                        {
                            item.EndDate = Convert.ToDateTime(enddate);
                        }

                        var cur = from a in lqh.Umsdb.Rules_AllCurrentRulesInfo
                                  where idList.Contains((int)a.RulesID)
                                  select a;
                        foreach (var item in cur)
                        {
                            item.EndDate = Convert.ToDateTime(enddate);
                        }

                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue = true, Message = "修改成功！" };
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue = false, Message = ex.Message };
                    }
                }
            }
        }

        /// <summary>
        /// 281
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user,Model.Rules_OffList model)
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

                        List<string> hallids = new List<string>();
                        foreach (var item in model.Rules_HallOffInfo)
                        {
                            hallids.Add(item.HallID);
                        }
                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #region  过滤仓库
                        if (ValidHallIDS.Count > 0)
                        {
                            var que = from a in hallids
                                      where !ValidHallIDS.Contains(a)
                                      select a ;
                            if (que.Count()>0)
                            {
                                var hall = from h in lqh.Umsdb.Pro_HallInfo
                                          where h.HallID == que.First()
                                          select h;
                                return new Model.WebReturn() { ReturnValue = false, Message = "仓库无权操作" + hall.First().HallName };
                            }
                        }
                        #endregion

                        #endregion

                        var query = from a in lqh.Umsdb.Rules_OffList
                                    where a.Note == model.Note && a.Flag ==true
                                    select a;
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false,Message="已存在相同描述的规则活动，保存失败！"};
                        }

                        List<int> stlist = new List<int>();
                        List<string> halls = new List<string>();
                        List<int> proms = new List<int>();
                        List<int> rts = new List<int>();

                        foreach (var item in model.Rules_SellTypeInfo)
                        {
                            stlist.Add((int)item.SellType);
                        }

                        foreach (var item in model.Rules_HallOffInfo)
                        {
                            hallids.Add(item.HallID);
                        }

                        foreach (var item in model.Rules_ProMainInfo)
                        {
                            proms.Add((int)item.ProMainID);
                            foreach (var child in item.Rules_Pro_RulesTypeInfo)
                            {
                                rts.Add((int)child.RulesTypeID);
                            }
                        }

                        var val = from a in lqh.Umsdb.Rules_OffList
                                  join b in lqh.Umsdb.Rules_ProMainInfo
                                  on a.ID equals b.RulesID
                                  join px in lqh.Umsdb.Pro_ProMainInfo
                                  on b.ProMainID equals px.ProMainID
                                  join p in lqh.Umsdb.Rules_Pro_RulesTypeInfo
                                  on b.ID equals p.RulesProID
                                  join s in lqh.Umsdb.Rules_SellTypeInfo
                                  on a.ID equals s.RulesID
                                  join g in lqh.Umsdb.Pro_SellType
                                  on s.SellType equals g.ID
                                  join h in lqh.Umsdb.Rules_HallOffInfo
                                  on a.ID equals h.RulesID
                                  //join m in model.Rules_HallOffInfo
                                  //on h.HallID equals m.HallID
                                  join hx in lqh.Umsdb.Pro_HallInfo
                                  on h.HallID equals hx.HallID
                                  //join x in model.Rules_SellTypeInfo
                                  //on s.ID equals x.SellType
                                  where hallids.Contains(h.HallID) && stlist.Contains((int)s.SellType) &&
                                  ((model.EndDate >= a.StartDate && model.EndDate <= a.EndDate)
                                  || (model.StartDate >= a.StartDate && model.StartDate <= a.EndDate))
                                  && a.Flag == true && rts.Contains((int)p.RulesTypeID) && proms.Contains( px.ProMainID)
                                  select new { 
                                     b.ProMainID ,px.ProMainName,
                                     g.Name,hx.HallName,
                                     p.RulesTypeID
                                  };

                        if (val.Count() > 0)
                        {
                            return new WebReturn()
                            {
                                ReturnValue = false,//主商品为销售类别为
                                Message = val.First().HallName + " 已存在相同时段规则活动:\n"
                                + val.First().ProMainName + ", " + val.First().Name + ",  保存失败！"
                            };
                            //foreach (var item in val)
                            //{
                            //    foreach (var xxd in model.Rules_ProMainInfo)
                            //    {
                            //        foreach (var dd in xxd.Rules_Pro_RulesTypeInfo)
                            //        {
                            //            if (item.RulesTypeID == dd.RulesTypeID && item.ProMainID == xxd.ProMainID)
                            //            {
                            //                return new WebReturn()
                            //                {
                            //                    ReturnValue = false,//主商品为销售类别为
                            //                    Message = "门店 " + item.HallName + " 已存在相同时段规则活动:\n"
                            //                    + item.ProMainName + "，" + item.Name+", 保存失败！"
                            //                };
                            //            }
                            //        }
                            //    }
                            //}
                        }


                        //foreach (var child in model.Rules_SellTypeInfo)
                        //{
                        //    foreach (var xxd in model.Rules_ProMainInfo)
                        //    {
                        //        foreach (var dd in xxd.Rules_Pro_RulesTypeInfo)
                        //        {
                        //            foreach (var hh in model.Rules_HallOffInfo)
                        //            {
                        //                var list = from b in lqh.Umsdb.Rules_ProMainInfo
                        //                           join a in lqh.Umsdb.Rules_OffList
                        //                           on b.RulesID equals a.ID
                        //                           join s in lqh.Umsdb.Rules_SellTypeInfo
                        //                           on a.ID equals s.RulesID
                        //                           join g in lqh.Umsdb.Pro_SellType
                        //                           on s.SellType equals g.ID
                        //                           join p in lqh.Umsdb.Rules_Pro_RulesTypeInfo
                        //                           on b.ID equals p.RulesProID
                        //                           join t in lqh.Umsdb.Rules_TypeInfo
                        //                           on p.RulesTypeID equals t.ID
                        //                           join h in lqh.Umsdb.Rules_HallOffInfo
                        //                           on a.ID equals h.RulesID
                        //                           where b.ProMainID == xxd.ProMainID
                        //                           && s.SellType == child.SellType
                        //                           && h.HallID== hh.HallID
                        //                           && ((model.EndDate >= a.StartDate && model.EndDate <= a.EndDate)
                        //                           || (model.StartDate >= a.StartDate && model.StartDate <= a.EndDate))
                        //                           && a.Flag == true && p.RulesTypeID == dd.RulesTypeID
                        //                           select a;

                        //                if (list.Count() > 0)
                        //                {
                        //                    var msg = from b in lqh.Umsdb.Rules_ProMainInfo
                        //                              join a in lqh.Umsdb.Rules_OffList
                        //                              on b.RulesID equals a.ID
                        //                              join p in lqh.Umsdb.Pro_ProMainInfo
                        //                              on b.ProMainID equals p.ProMainID
                        //                              join s in lqh.Umsdb.Rules_SellTypeInfo
                        //                              on a.ID equals s.RulesID

                        //                              join h in lqh.Umsdb.Rules_HallOffInfo
                        //                              on a.ID equals h.RulesID
                        //                              join hx in lqh.Umsdb.Pro_HallInfo
                        //                              on h.HallID equals hx.HallID  
                        //                              //join n in model.Rules_HallOffInfo
                        //                              //on h.HallID equals n.HallID

                        //                              join g in lqh.Umsdb.Pro_SellType
                        //                              on s.SellType equals g.ID
                        //                              where s.SellType == child.SellType && b.ProMainID == xxd.ProMainID

                        //                              select new
                        //                              {
                        //                                  p.ProMainName,
                        //                                  g.Name,
                        //                                  hx.HallName
                        //                              };

                        //                    return new WebReturn()
                        //                    {
                        //                        ReturnValue = false,//主商品为销售类别为
                        //                        Message = "保存失败！门店 "+msg.First().HallName+" 已存在相同时段规则活动: " + msg.First().ProMainName + "，" + msg.First().Name
                        //                    };
                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                        model.Flag = true;
                        model.SysDate = DateTime.Now;
                        lqh.Umsdb.Rules_OffList.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();

                        //将当前活动规则保存到  Rules_AllCurrentRulesInfo
                        List<Model.Rules_AllCurrentRulesInfo> modles = new List<Model.Rules_AllCurrentRulesInfo>();
                        foreach (var item in model.Rules_HallOffInfo)
                        {
                            foreach (var child in model.Rules_SellTypeInfo)
                            {
                                foreach (var xxd in model.Rules_ProMainInfo)
                                {
                                    foreach(var cc in xxd.Rules_Pro_RulesTypeInfo)
                                    {
                                        Model.Rules_AllCurrentRulesInfo rar = new Model.Rules_AllCurrentRulesInfo();
                                        rar.StartDate = Convert.ToDateTime(model.StartDate);
                                        rar.EndDate =Convert.ToDateTime(model.EndDate);
                                        rar.Note = model.Note;
                                        rar.MaxPrice = cc.MaxPrice;
                                        rar.MinPrice = cc.MinPrice;
                                        rar.OffPrice = cc.OffPrice;
                                        rar.OrderBy = cc.OrderBy;
                                        rar.RulesID = model.ID;
                                        rar.RulesTypeID = cc.RulesTypeID;
                                        rar.RulesName = cc.Rules_TypeInfo.RulesName;
                                        rar.ShowToCus = cc.Rules_TypeInfo.ShowToCus;
                                        rar.CanGetBack = cc.Rules_TypeInfo.CanGetBack;
                                        rar.HallID = item.HallID;
                                        rar.SellType = child.SellType;
                                        rar.ProMainID = xxd.ProMainID;
                                        rar.Rules_ProMain_ID = cc.ID;
                                        modles.Add(rar);
                                    }
                                }
                            }
                        }
                        if (modles.Count != 0)
                        {
                            lqh.Umsdb.Rules_AllCurrentRulesInfo.InsertAllOnSubmit(modles);
                        }
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue= true,Message="保存成功！"};
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue  =false,Message = ex.Message};
                    }
                }
            }
        }

        /// <summary>
        /// 282
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
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

                        //有权限的仓库
                        List<string> ValidHallIDS = new List<string>();
                        //有权限的商品
                        List<string> ValidProIDS = new List<string>();

                        Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion


                        var query = from a in lqh.Umsdb.Rules_OffList
                                    where a.ID == id
                                    select a;
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未能找到指定规则活动，删除失败！" };
                        }
                        Model.Rules_OffList ro = query.First();
                        if (!ro.Flag)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "该规则活动已删除！" };
                        }
                       
                        ro.Flag = false;
                        ro.DeleteDate = DateTime.Now;
                        ro.Deleter = user.UserID;

                        //删除时时更新表
                        var allCurRulesInfo = from a in lqh.Umsdb.Rules_AllCurrentRulesInfo
                                              //join b in lqh.Umsdb.Rules_OffList
                                              //on a.RulesID equals b.ID
                                              where a.RulesID == id
                                              select a;
                    
                        if (allCurRulesInfo.Count() > 0)
                        {
                            lqh.Umsdb.Rules_AllCurrentRulesInfo.DeleteAllOnSubmit(allCurRulesInfo.ToList());
                        }

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

        /// <summary>
        /// 283
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn GetModels(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from a in lqh.Umsdb.View_Rules_OffList
                                where a.Flag == true
                                select a;
                    ////var query = from a in lqh.Umsdb.Rules_OffList
                    ////           join b in lqh.Umsdb.Sys_UserInfo
                    ////           on a.UserID equals b.UserID
                    ////           where a.Flag == true
                    ////           select new { 
                    ////               a.ID,
                    ////             a.DeleteDate,
                    ////             a.SysDate,
                    ////             a.EndDate,
                    ////             a.StartDate,
                    ////             a.Flag,
                    ////             a.Note,
                    ////             b.UserName
                    ////           } ;

               

                    #region "过滤数据"

                    foreach (var item in pageParam.ParamList)
                    {
                        switch (item.ParamName)
                        {
                            //case "StartDate":
                            //    Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)item;
                            //    if (mm5.ParamValues != null)
                            //    {
                            //        Model.ReportSqlParams_DataTime mm6 = new ReportSqlParams_DataTime();
                            //        mm6.ParamValues = DateTime.Now;
                            //        foreach (var xxd in pageParam.ParamList)
                            //        {
                            //            if (xxd.ParamName == "EndDate")
                            //            {
                            //                mm6 = (Model.ReportSqlParams_DataTime)xxd;
                            //                break;
                            //            }
                            //        }
                            //        if (mm5.ParamValues == mm6.ParamValues)
                            //        {
                            //            query = from b in query
                            //                    where b.SysDate >= mm5.ParamValues &&
                            //                    b.SysDate < DateTime.Parse(mm5.ParamValues.ToString()).AddDays(1)
                            //                    select b;
                            //        }
                            //        else
                            //        {
                            //            query = from b in query
                            //                    where b.SysDate >= mm5.ParamValues && b.SysDate <= mm6.ParamValues
                            //                    select b;
                            //        }
                            //    }
                            //    break;
                            case "State":
                                Model.ReportSqlParams_String state = (Model.ReportSqlParams_String)item;
                                if (state.ParamValues != null)
                                {
                                    DateTime now = DateTime.Now;
                                    switch (state.ParamValues)
                                    {
                                        case "正在进行":
                                            query = from b in query
                                                    where now >= b.StartDate && now <= b.EndDate 
                                                    select b;
                                         
                                            break;
                                        case "未开始":
                                            query = from b in query
                                                    where now < b.StartDate
                                                    select b;
                                            break;
                                        case "已结束":
                                            query = from b in query
                                                    where now > b.EndDate
                                                    select b;
                                            break;
                                    }
                              
                                }

                                break;

                            case "UserName":
                                Model.ReportSqlParams_String para = (Model.ReportSqlParams_String)item;
                                if (para.ParamValues != null)
                                {
                                    query = from b in query
                                                  where para.ParamValues == b.UserName
                                                  select b;
                                }
                                break;

                            case "Note":
                                Model.ReportSqlParams_String para_bote = (Model.ReportSqlParams_String)item;
                                if (para_bote.ParamValues != null)
                                {
                                    query = from b in query
                                            where  b.Note.Contains(para_bote.ParamValues)
                                            select b;
                                }
                                break;
                    
                        }
                    }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();

                    pageParam.RecordCount = query.Count();
                    DateTime n = DateTime.Now;
                   
                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        var results = from a in query.Take(pageParam.PageSize).ToList()
                                      select a;

                        List<Model.View_Rules_OffList> models = new List<Model.View_Rules_OffList>();
                        if (results.Count() > 0)
                        {
                            foreach (var item in results)
                            {
                                Model.View_Rules_OffList m = new Model.View_Rules_OffList();
                                m.DeleteDate = Convert.ToDateTime(item.DeleteDate);
                                // m.Deleter = item.Deleter;
                                m.State = item.State;
                                m.ID = item.ID;
                                m.EndDate = (DateTime)item.EndDate;
                                m.StartDate = (DateTime)item.StartDate;
                                m.Flag = item.Flag;
                                m.UserName = item.UserName;
                                m.Note = item.Note;
                                models.Add(m);
                            }
                        }
                        foreach (var item in models)
                        {
                            if (n >= item.StartDate && n <= item.EndDate)
                            {
                                item.State = "正在进行";
                            }
                            if (n < item.StartDate)
                            {
                                item.State = "未开始";
                            }

                            if (n > item.EndDate)
                            {
                                item.State = "已结束";
                            }
                        }
                        pageParam.Obj = models;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        var results = from a in query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList()
                                      select a;
                        List<Model.View_Rules_OffList> models = new List<Model.View_Rules_OffList>();
                        if (results.Count() > 0)
                        {
                            foreach (var item in results)
                            {
                                Model.View_Rules_OffList m = new Model.View_Rules_OffList();
                                m.DeleteDate = Convert.ToDateTime(item.DeleteDate);
                                // m.Deleter = item.Deleter;
                                m.State = item.State;
                                m.ID = item.ID;
                                m.EndDate = (DateTime)item.EndDate;
                                m.StartDate = (DateTime)item.StartDate;
                                m.Flag = item.Flag;
                                m.UserName = item.UserName;
                                m.Note = item.Note;
                                models.Add(m);
                            }
                        }
                        foreach (var item in models)
                        {
                            if (n >= item.StartDate && n <= item.EndDate)
                            {
                                item.State = "正在进行";
                            }
                            if (n < item.StartDate)
                            {
                                item.State = "未开始";
                            }

                            if (n > item.EndDate)
                            {
                                item.State = "已结束";
                            }
                        }
                        pageParam.Obj = models.ToList();
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() {ReturnValue = false,Message=ex.Message };
                }
            }
        }

        /// <summary>
        /// 284
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.WebReturn GetDetail(Model.Sys_UserInfo user, int id)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    List<Model.RulesProMain> proMains = new List<Model.RulesProMain>();
                    var pros = from a in lqh.Umsdb.Rules_ProMainInfo
                               join b in lqh.Umsdb.Rules_OffList
                               on a.RulesID equals b.ID
                               join c in lqh.Umsdb.Pro_ProMainInfo
                               on a.ProMainID equals c.ProMainID
                               join x in lqh.Umsdb.Pro_ClassInfo 
                               on c.ClassID equals x.ClassID
                               join t in lqh.Umsdb.Pro_TypeInfo
                               on c.TypeID equals t.TypeID
                               where b.ID == id && b.Flag==true
                               select new { 
                                   a.ID,
                                 c.ProMainName,
                                 a.ProMainID,
                                 t.TypeName,
                                 x.ClassName
                               };

                    foreach (var item in pros)
                    {
                        Model.RulesProMain rp = new Model.RulesProMain();
                        rp.ProMainID = (int)item.ProMainID;
                        rp.ProMainName = item.ProMainName;
                        rp.ID = item.ID;
                        rp.ClassName = item.ClassName;
                        rp.TypeName = item.TypeName;
                        rp.Pro_RulesTypeInfo = new List<Model.Pro_RulesTypeInfo>();

                        var list = from a in lqh.Umsdb.Rules_Pro_RulesTypeInfo
                                   join b in lqh.Umsdb.Rules_TypeInfo
                                   on a.RulesTypeID equals b.ID
                                   where a.RulesProID == rp.ID
                                   select new { 
                                     a.OffPrice,
                                     a.MinPrice,
                                     a.MaxPrice,
                                     a.OrderBy,
                                     b.CanGetBack,
                                     b.RulesName,
                                     b.ShowToCus
                                     ,b.Note
                                   };

                        foreach (var chid in list)
                        {
                            Model.Pro_RulesTypeInfo pr = new Model.Pro_RulesTypeInfo();
                            pr.MaxPrice = chid.MaxPrice;
                            pr.MinPrice = chid.MinPrice;
                            pr.OffPrice = chid.OffPrice;
                            pr.OrderBy = chid.OrderBy;
                            pr.RulesTypeName = chid.RulesName;
                            pr.ShowToCus = chid.ShowToCus;
                            pr.CanGetBack = chid.CanGetBack;
                            rp.Pro_RulesTypeInfo.Add(pr);
                        }
                        proMains.Add(rp);
                    }

                    var hall = from a in lqh.Umsdb.Rules_HallOffInfo
                               join b in lqh.Umsdb.Rules_OffList
                               on a.RulesID equals b.ID
                               join c in lqh.Umsdb.Pro_HallInfo
                               on a.HallID equals c.HallID
                               where b.ID == id && b.Flag == true
                               select new {
                               a.HallID
                               ,c.HallName
                               
                               };

                    List<Model.Pro_HallInfo> halls = new List<Model.Pro_HallInfo>();
                    foreach (var item in hall)
                    {
                        Model.Pro_HallInfo h = new Model.Pro_HallInfo();
                        h.HallName = item.HallName;
                        h.HallID = item.HallID;
                        halls.Add(h);
                    }

                    var st = from a in lqh.Umsdb.Rules_SellTypeInfo
                             join b in lqh.Umsdb.Rules_OffList
                             on a.RulesID equals b.ID
                             join c in lqh.Umsdb.Pro_SellType
                             on a.SellType equals c.ID
                             where b.ID == id && b.Flag == true
                             select new
                             {
                                 a.SellType
                                 ,
                                 c.Name

                             };
                    List<Model.Pro_SellType> selltypes = new List<Model.Pro_SellType>();
                    foreach (var item in st)
                    {
                        Model.Pro_SellType s = new Model.Pro_SellType();
                        s.Name = item.Name;
                        s.ID = (int)item.SellType;
                        selltypes.Add(s);
                    }

                    return new Model.WebReturn() { ReturnValue = true, Obj = proMains, ArrList = new System.Collections.ArrayList() {halls,selltypes } };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue =false,Message=ex.Message};
                }
            }
        }


    }
}
