using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;
using Model;


namespace DAL
{
    /// <summary>
    /// 商品
    /// 
    /// 
    /// 无判断值,只能获取全部信息，并非更新信息
    /// 
    /// 
    /// 
    /// </summary>
    public class Pro_ProInfo : Sys_InitParentInfo
    {
        private int MethodID;

        public Pro_ProInfo()
        {
            this.MethodID = 0;
        }

        public Pro_ProInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
        private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
              new Model.ReportSqlParams_String(){ParamName="ClassName" },
            new Model.ReportSqlParams_String(){ParamName="TypeName" },
            new Model.ReportSqlParams_String(){ParamName="ProMainName"}
        
        };

        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }
        #region  获取商品列表
        /// <summary>
        /// 获取商品实体
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {

                    #region 权限
                    //List<string> ValidHallIDS = new List<string>();
                    ////有权限的商品
                    //List<string> ValidProIDS = new List<string>();

                    //Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MethodID, ValidHallIDS, ValidProIDS);

                    //if (ret.ReturnValue != true)
                    //{ return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }

                    #region 将传入的参数与指定的参数了左连接

                    var param_join = from b in pageParam.ParamList
                                     join c in this.ParamList
                                     on new { b.ParamName, t = b.GetType() }
                                     equals
                                     new { c.ParamName, t = c.GetType() }
                                     into temp1
                                     from c1 in temp1.DefaultIfEmpty()
                                     select new
                                     {
                                         ParamFront = b,
                                         ParamBehind = c1
                                     };

                    #endregion

                    #region 获取数据

                    var inorder_query = from b in lqh.Umsdb.View_ProInfo
                                        select b;
                    foreach (var m in param_join)
                    {


                        switch (m.ParamFront.ParamName)
                        {
                            case "ClassName":
                                Model.ReportSqlParams_String mm1 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm1.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.ClassName.Contains(mm1.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "TypeName":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.TypeName.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "ProMainName":
                                Model.ReportSqlParams_String mm2 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm2.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.ProName.Contains(mm2.ParamValues)
                                                    select b;
                                    break;
                                }


                            default: break;
                        }
                    }
                    #endregion

                    #region 排序
                    inorder_query = from b in inorder_query
                                    orderby b.ProID descending
                                    select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数


                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.View_ProInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.View_ProInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }
                catch
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = "服务端出错 ！" };
                }
            }             
        }
        #endregion

        public List<Model.Pro_ProInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                var dl = new DataLoadOptions();
                dl.LoadWith<Model.Pro_ProInfo>(p => p.Pro_SellTypeProduct);
                lqh.Umsdb.LoadOptions = dl;
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_ProInfo>()
                                select b;
                    var list = new List<Model.Pro_ProInfo>();
                    foreach (var proProInfo in query)
                    {
                        proProInfo.IsTicketUsedable = proProInfo.Pro_SellTypeProduct.Any(p => p.SellType == 2|| p.SellType==9);
                        list.Add(proProInfo);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    return new List<Model.Pro_ProInfo>();
                }
            }
        }
        #region 获取串码
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, string IMEI)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                where b.IMEI == IMEI
                                select b;
                    if (query == null || query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "不存在串码" };
                    }
                    var Pro = lqh.Umsdb.GetTable<Model.Pro_ProInfo>().Where(p => p.ProID == query.First().ProID);
                    if (Pro == null || Pro.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "不存在该串码的商品" };
                    }
                    return new Model.WebReturn() { ReturnValue = true, Obj = Pro.First(), Message = "已获取" };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    throw ex;
                }
            }
        }
        #endregion

        #region 新增商品和商品权限（旧）
        //public Model.WebReturn Add(Model.Sys_UserInfo user, List<Model.Role_Pro_Property> RoleList)
        //{
        //    using (LinQSqlHelper lqh = new LinQSqlHelper())
        //    {
        //        using (TransactionScope ts = new TransactionScope())
        //        {
        //            try
        //            {

        //                #region 获取并设置商品信息
        //                foreach (var item in RoleList)
        //                {
        //                    if (item.ProModel == null)
        //                        continue;
        //                    #region 验证数据有效性并添加商品
        //                    string msg = "";

        //                    List<Model.Pro_ProInfo> ProList = new List<Model.Pro_ProInfo>();
        //                    int i = 0;
        //                    foreach (var ProItem in item.ProModel)
        //                    {
        //                        lqh.Umsdb.OrderMacker(item.ProModel.Count(), "SP", "SP", ref msg);
        //                        if (string.IsNullOrEmpty(msg))
        //                        {
        //                            return new WebReturn() { ReturnValue = false, Message = "生成商品编号出错" };
        //                        }
        //                        string[] InListIDStr = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        //                        if (InListIDStr.Count() != item.ProModel.Count())
        //                        {
        //                            return new WebReturn() { ReturnValue = false, Message = "生成商品编号数量与商品数量不一致" };
        //                        }


        //                        Model.Pro_ProInfo Pro = new Model.Pro_ProInfo();
        //                        Pro.ProID = InListIDStr[i];
        //                        Pro.IsService = ProItem.IsService == "是" ? true : false;
        //                        Pro.NeedIMEI = ProItem.NeedIMEI == "是" ? true : false;
        //                        Pro.Pro_ClassID = ProItem.ClassID;
        //                        Pro.Pro_TypeID = ProItem.TypeID;
        //                        Pro.ProName = ProItem.ProName;
        //                        Pro.VIP_TypeID = ProItem.VIPTypeID;
        //                        Pro.Pro_ProProperty_F = new EntitySet<Pro_ProProperty_F>();
        //                        //添加属性值
        //                        foreach (var PropertyItem in ProItem.PropertyModel)
        //                        {
        //                            foreach (var PropertyValueItem in PropertyItem.PropertyValueModel)
        //                            {
        //                                Pro_ProProperty_F Property_F = new Pro_ProProperty_F() { ValueID = PropertyValueItem.ID };
        //                                Pro.Pro_ProProperty_F.Add(Property_F);
        //                            }
        //                        }
        //                        ProList.Add(Pro);
        //                        i++;
        //                    }
        //                    List<Model.Pro_ProInfo> OldProList = (from b in lqh.Umsdb.Pro_ProInfo select b).ToList();
        //                    int count = (from b in OldProList
        //                                 join c in ProList on
        //                                    new
        //                                    {
        //                                        b.Pro_ClassID,
        //                                        b.Pro_TypeID,
        //                                        b.ProName
        //                                    }
        //                                    equals
        //                           new
        //                           {
        //                               c.Pro_ClassID,
        //                               c.Pro_TypeID,
        //                               c.ProName
        //                           }
        //                                 select c).Count();
        //                    if (count > 0)
        //                    {
        //                        return new Model.WebReturn() { ReturnValue = false, Message = "部分商品已存在！" };
        //                    }
        //                    lqh.Umsdb.Pro_ProInfo.InsertAllOnSubmit(ProList);
        //                    lqh.Umsdb.SubmitChanges();
        //                    #endregion
        //                    #region 获取角色的所有菜单
        //                    List<Model.Sys_Role_MenuInfo> Role_MenuInf = (from b in lqh.Umsdb.Sys_Role_MenuInfo
        //                                                                  where b.RoleID == item.RoleID
        //                                                                  select b).Distinct().ToList();
        //                    #endregion
        //                    #region 为菜单添加商品类别权限
        //                    List<Model.Sys_Role_Menu_ProInfo> Role_Pro = new List<Model.Sys_Role_Menu_ProInfo>();
        //                    foreach (var MenuItem in Role_MenuInf)
        //                    {
        //                        foreach (var ProItem in ProList)
        //                        {
        //                            Model.Sys_Role_Menu_ProInfo Role_ProItem = new Model.Sys_Role_Menu_ProInfo();
        //                            Role_ProItem.ClassID = ProItem.Pro_ClassID;
        //                            Role_ProItem.MenuID = MenuItem.MenuID;
        //                            Role_ProItem.RoleID = MenuItem.RoleID;
        //                            Role_Pro.Add(Role_ProItem);
        //                        }
        //                    }
        //                    Role_Pro = (from b in Role_Pro
        //                                where ((from c in lqh.Umsdb.Sys_Role_Menu_ProInfo
        //                                        where b.MenuID != c.MenuID || b.RoleID != c.RoleID || b.ClassID != c.ClassID
        //                                        select c).Count() > 0)
        //                                select b).ToList();
        //                    lqh.Umsdb.Sys_Role_Menu_ProInfo.InsertAllOnSubmit(Role_Pro);
        //                    lqh.Umsdb.SubmitChanges();
        //                    #endregion
        //                }
        //                ts.Complete();
        //                #endregion
        //                return new Model.WebReturn() { ReturnValue = true, Message = "添加成功" };
        //            }
        //            catch
        //            {
        //                return new Model.WebReturn() { ReturnValue = false, Message = "添加失败！" };
        //            }
        //        }
        //    }
        //}
        #endregion

        #region 新增商品和商品权限（新）
        public Model.WebReturn Add(Model.Sys_UserInfo user, List<Model.ProModel> ProModelList)
        {

            if (ProModelList == null)
                return new WebReturn() { ReturnValue = false, Message = "无数据" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证数据有效性并添加商品
                        string msg = "";

                        List<Model.Pro_ProInfo> ProList = new List<Model.Pro_ProInfo>();
                        int i = 0;
                        int a = 0;
                        lqh.Umsdb.OrderMacker(ProModelList.Count(), "SP", "SP", ref msg);
                        if (string.IsNullOrEmpty(msg))
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成商品编号出错" };
                        }
                        string[] InListIDStr = msg.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (InListIDStr.Count() != ProModelList.Count())
                        {
                            return new WebReturn() { ReturnValue = false, Message = "生成商品编号数量与商品数量不一致" };
                        }
                        foreach (var ProItem in ProModelList)
                        {
                            Model.Pro_ProInfo Pro = new Model.Pro_ProInfo();
                            Pro.ProID = InListIDStr[a];
                            Pro.IsService = ProItem.IsService == "是" ? true : false;
                            Pro.NeedIMEI = ProItem.NeedIMEI == "是" ? true : false;
                            Pro.ISdecimals = ProItem.Isdecimal == "是" ? true : false;
                            Pro.AfterRate = ProItem.AfterRate;
                            Pro.AfterSep = ProItem.AfterSep== "是" ? true : false;
                            Pro.AfterTicket = ProItem.AfterTicket;
                            Pro.BeforeRate = ProItem.BeforeRate;
                            Pro.BeforeSep = ProItem.BeforeSep == "是" ? true : false;
                            Pro.BeforeTicket = ProItem.BeforeTicket;
                            Pro.NeedMoreorLess = ProItem.IsNeedMoreorLess == "是" ? true : false;
                            Pro.Note = ProItem.Note;
                            Pro.PrintName = ProItem.PrintName;
                            Pro.SepDate = ProItem.SepDate;
                            Pro.TicketLevel = ProItem.TicketLevel;
                            

                            Pro.Pro_ClassID = ProItem.ClassID;
                            Pro.Pro_TypeID = ProItem.TypeID;
                            Pro.ProName = ProItem.ProName;
                            Pro.VIP_TypeID = ProItem.VIPTypeID;
                            Pro.ProMainID = ProItem.ProMainID;
                            Pro.YanBaoModelID = ProItem.YanBaoModelID;
                            Pro.AirHallID = ProItem.AirHallID;


                            Pro.AssetFrom = ProItem.AssetFrom;
                            Pro.AssetPeriod = ProItem.AssetPeriod;
                            Pro.AssetRate = ProItem.AssetRate;
                            Pro.AssetPrice = ProItem.AssetPrice;
                            Pro.AssetValue = ProItem.AssetValue;
                            Pro.AssetFinish = ProItem.AssetFinish;
                            Pro.AssetStatus = ProItem.AssetStatus;


                            Pro.Pro_ProProperty_F = new EntitySet<Pro_ProProperty_F>();
                           
                            string Property = "";
                            //添加属性值
                            if (ProItem.PropertyModel != null)
                            {
                                foreach (var PropertyItem in ProItem.PropertyModel)
                                {
                                    i = 0;
                                    foreach (var PropertyValueItem in PropertyItem.PropertyValueModel)
                                    {
                                        if (i == 0)
                                            Property += PropertyValueItem.Pvalue;
                                        else
                                            Property += "/" + PropertyValueItem.Pvalue;
                                        Pro_ProProperty_F Property_F = new Pro_ProProperty_F() { ValueID = PropertyValueItem.ID };
                                        Pro.Pro_ProProperty_F.Add(Property_F);
                                        i++;
                                    }
                                }
                            }
                            a++;
                            Pro.ProFormat = Property;
                            ProList.Add(Pro);                      
                        }
                        List<Model.Pro_ProInfo> OldProList = (from b in lqh.Umsdb.Pro_ProInfo select b).ToList();
                        int count = (from b in OldProList
                                     join c in ProList on
                                        new
                                        {
                                            b.ProMainID,
                                            ValueIDS= String.Join("/",(from x in b.Pro_ProProperty_F orderby x.ValueID select x.ValueID ))
                                        }
                                        equals
                               new
                               {
                                   c.ProMainID,
                                   ValueIDS = String.Join("/", (from x in c.Pro_ProProperty_F orderby x.ValueID select x.ValueID))
                               }
                                     select c).Count();
                        if (count > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "部分商品已存在！" };
                        }
                        lqh.Umsdb.Pro_ProInfo.InsertAllOnSubmit(ProList);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete(); 
                        #endregion                           
                        return new Model.WebReturn() { ReturnValue = true, Message = "添加成功", Obj = ProList };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "添加失败！" };
                    }
                }
            }
        }
        #endregion

        #region  销售使用 
        public Model.WebReturn GetModel(Model.Sys_UserInfo user, string IMEI, string hallID)
        {
            if (IMEI == null || hallID == null)
                return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "传参错误！" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                var dl = new DataLoadOptions();
                dl.LoadWith<Model.Pro_ProInfo>(p => p.Pro_SellTypeProduct);
                lqh.Umsdb.LoadOptions = dl;
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                where b.IMEI == IMEI && b.OutID == null && b.SellID == null
                                && b.BorowID == null && b.RepairID == null && b.AuditID==null &&b.AssetID==null
                                select b;
                    if (query == null || query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "不存在串码或存在其他操作！" };
                    }
                    var Pro = lqh.Umsdb.GetTable<Model.Pro_ProInfo>().Where(p => p.ProID == query.First().ProID);
                    if (Pro == null || Pro.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "不存在该串码的商品" };
                    }
                    if (!string.IsNullOrEmpty(hallID))
                    {
                        var qureyHall = from b in lqh.Umsdb.Pro_HallInfo
                                        where b.HallID == hallID
                                        select b;
                        if (qureyHall.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "不存在仓库ID" };
                        }
                        var query_IMEIhall = from b in query
                                             where b.HallID == hallID
                                             select b;
                        if (query_IMEIhall.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "串码不存在该仓库" };
                        }
                    }

                    return new Model.WebReturn() { ReturnValue = true, Obj = Pro.First(), Message = "已获取" };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    throw ex;
                }
            }
        }


        public Model.WebReturn GetModel(Model.Sys_UserInfo user, string IMEI, string hallID,int assets)
        {
            if (IMEI == null || hallID == null)
                return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "传参错误！" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {

                var dl = new DataLoadOptions();
                dl.LoadWith<Model.Pro_ProInfo>(p => p.Pro_SellTypeProduct);
                lqh.Umsdb.LoadOptions = dl;
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.Pro_IMEI>()
                                where b.IMEI == IMEI && b.OutID == null && b.SellID == null
                                && b.BorowID == null && b.RepairID == null && b.AuditID == null 
                                select b;
                    if (query == null || query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "不存在串码或存在其他操作！" };
                    }
                    var Pro = lqh.Umsdb.GetTable<Model.Pro_ProInfo>().Where(p => p.ProID == query.First().ProID);
                    if (Pro == null || Pro.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "不存在该串码的商品" };
                    }
                    if (!string.IsNullOrEmpty(hallID))
                    {
                        var qureyHall = from b in lqh.Umsdb.Pro_HallInfo
                                        where b.HallID == hallID
                                        select b;
                        if (qureyHall.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "不存在仓库ID" };
                        }
                        var query_IMEIhall = from b in query
                                             where b.HallID == hallID
                                             select b;
                        if (query_IMEIhall.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Obj = null, Message = "串码不存在该仓库" };
                        }
                    }

                    var imeimodel = query.First();
                    ArrayList arrlist=new ArrayList();
                    arrlist.Add(imeimodel.Pro_StoreInfo.Pro_InOrderList.Pro_InOrder);
                    arrlist.Add(imeimodel.Asset_UseInfo);
                    arrlist.Add(imeimodel.Pro_InOrderList);

                    return new Model.WebReturn() { ReturnValue = true, Obj = Pro.First(), Message = "已获取",ArrList = arrlist};
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                    throw ex;
                }
            }
        }

        public Model.WebReturn GetIMEIStatus(Model.Sys_UserInfo user, string IMEI)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from a in lqh.Umsdb.GetTable<Model.Pro_IMEI>() where a.IMEI == IMEI select a;
                    if (query.Any())
                    {
                        WebReturn r = new WebReturn();
                        r.ArrList.Add(query.First());
                        try
                        {
                            var q =
                                lqh.Umsdb.Pro_SellListInfo.Where(
                                    p => p.IMEI == IMEI && (p.Pro_SellInfo != null && p.Pro_SellBackList.Count==0));
                            r.ArrList.Add(q.First());

                        }
                        catch (Exception ex)
                        {
                            r.ArrList.Add(null);
                        }
                        r.ReturnValue = true;
                        return r;

                    }
                    else
                    {
                        return new WebReturn() { Message = "没有该串码", ReturnValue = false };
                    }
                }
                catch (Exception ex)
                {
                    return new WebReturn() { Message = ex.Message, ReturnValue = false };

                }
            }
        }


        public Model.WebReturn GetStore(Model.Sys_UserInfo user, List<string> ProID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    DataLoadOptions dataload = new DataLoadOptions();

                    dataload.LoadWith<Model.Pro_ProInfo>(c => c.Pro_SellTypeProduct);
                    dataload.LoadWith<Model.Pro_ProInfo>(c => c.Pro_StoreInfo);              
                    lqh.Umsdb.LoadOptions = dataload;

                    var query = from a in lqh.Umsdb.GetTable<Model.Pro_ProInfo>() where ProID.Contains(a.ProID)  select a;
                    if (query.Any())
                    {
                        foreach (var item in query)
                        {
                            var stores = new EntitySet<Pro_StoreInfo>();
                            stores.AddRange(item.Pro_StoreInfo.Where(p => p.ProCount>0));
                            item.Pro_StoreInfo = stores;
                        }
                        return new Model.WebReturn() { ReturnValue = true, Obj = query.ToList() };
                    }
                    else
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "无可用数据" };
                    }
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }

        public Model.WebReturn GetStore(Model.Sys_UserInfo user, string HallID, string ProID)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    DataLoadOptions dataload = new DataLoadOptions();

                    dataload.LoadWith<Model.Pro_ProInfo>(c => c.Pro_SellTypeProduct);
                    dataload.LoadWith<Model.Pro_ProInfo>(c => c.Pro_StoreInfo);
                    lqh.Umsdb.LoadOptions = dataload;

                    var query = from a in lqh.Umsdb.GetTable<Model.Pro_ProInfo>() where a.ProID == ProID select a;
                    if (query.Any())
                    {
                        var item = query.First();
                        var stores = new EntitySet<Pro_StoreInfo>();
                        stores.AddRange(item.Pro_StoreInfo.Where(p => p.HallID == HallID));
                        item.Pro_StoreInfo = stores;
                        return new Model.WebReturn() { ReturnValue = true, Obj = item };
                    }
                    else
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "无可用数据" };
                    }
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                }
            }
        }
        #endregion

        #region 删除商品
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.Pro_ProInfo ProModel)
        {

            if (ProModel == null)
                return new WebReturn() { ReturnValue = false, Message = "无数据" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 验证数据有效性
                        var query = (from b in lqh.Umsdb.Pro_InOrderList
                                    where b.ProID == ProModel.ProID
                                    select b).ToList();
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "商品已入库，不能删除！" };
                        }
                        #endregion
                        var Pro = from b in lqh.Umsdb.Pro_ProInfo
                                  where b.ProID == ProModel.ProID
                                  select b;
                        if (Pro == null || Pro.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "不存在商品！" };
                        }
                        lqh.Umsdb.Pro_ProInfo.DeleteOnSubmit(Pro.First());
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                     
                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功" };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "删除失败！" };
                    }
                }
            }
        }
        #endregion


        #region 修改商品
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Pro_ProInfo ProModel)
        {

            if (ProModel == null)
                return new WebReturn() { ReturnValue = false, Message = "无数据" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                   
                        var Pro = from b in lqh.Umsdb.Pro_ProInfo
                                  where b.ProID == ProModel.ProID
                                  select b;
                        if (Pro == null || Pro.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "不存在商品！" };
                        }
                        if (ProModel.PrintName.Length > 8)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "打印名称不能超过8个字！" };
                        }
                        Model.Pro_ProInfo ProInfo = Pro.First();
                        ProInfo.AfterRate = ProModel.AfterRate;
                        ProInfo.AfterSep = ProModel.AfterSep;
                        ProInfo.AfterTicket = ProModel.AfterTicket;
                        ProInfo.BeforeRate = ProModel.BeforeRate;
                        ProInfo.BeforeSep = ProModel.BeforeSep;
                        ProInfo.BeforeTicket = ProModel.BeforeTicket;
                        ProInfo.ISdecimals = ProModel.ISdecimals;
                        ProInfo.IsService = ProModel.IsService;
                        ProInfo.NeedIMEI = ProModel.NeedIMEI;
                        ProInfo.NeedMoreorLess = ProModel.NeedMoreorLess;
                        ProInfo.Note = ProModel.Note;
                        ProInfo.PrintName = ProModel.PrintName;
                        ProInfo.SepDate = ProModel.SepDate;
                        ProInfo.TicketLevel = ProModel.TicketLevel;
                        ProInfo.YanBaoModelID = ProModel.YanBaoModelID;
                        ProInfo.AirHallID = ProModel.AirHallID;
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功", Obj=ProInfo };
                    }
                    catch
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "修改失败！" };
                    }
                }
            }
        }
        #endregion

        #region   根据角色获取商品信息   售后维修使用

        /// <summary>
        /// 318,
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isBJ">是否只获取备机类别数据</param>
        /// <returns></returns>
        public Model.WebReturn GetListByRoleHallInfo(Model.Sys_UserInfo user,string hid , bool   isBJ)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    int classid = 0;
                    if (isBJ)
                    {
                        var cls = from a in lqh.Umsdb.Pro_ClassInfo
                                  where a.ClassName == "备机"
                                  select a;
                        classid = cls.First().ClassID;
                    }
                    //非串码
                    var list = from a in lqh.Umsdb.Pro_ProInfo
                               join b in lqh.Umsdb.Pro_StoreInfo on a.ProID equals b.ProID
                               join c in lqh.Umsdb.Sys_Role_HallInfo on b.HallID equals c.HallID
                               join d in lqh.Umsdb.Sys_UserInfo on c.RoleID equals d.RoleID
                               where d.UserID == user.UserID && c.HallID == hid  
                               && b.ProCount>0 && a.NeedIMEI==false
                               select new {a.ProID,a.ProName,a.ProFormat,a.NeedIMEI,a.Pro_ClassID
                                   ,a.Pro_TypeID,a.ProMainID,b.InListID,a.ISdecimals};

                    List<Model.BJModel> arr = new List<BJModel>();
                    foreach (var item in list)
                    {
                        if (isBJ)
                        {
                            if (item.Pro_ClassID != classid)
                            {
                                continue;
                            }
                        }
                        Model.BJModel bj = new BJModel();
                        bj.ClassID = Convert.ToInt32(item.Pro_ClassID);
                        bj.InListID = item.InListID;
                        bj.NeedIMEI = item.NeedIMEI;
                        bj.ProFormat = item.ProFormat;
                        bj.ProID = item.ProID;
                        bj.ProMainID =  Convert.ToInt32(item.ProMainID);
                        bj.ProName = item.ProName;
                        bj.TypeID =  Convert.ToInt32(item.Pro_TypeID);
                        bj.IsDecimal = item.ISdecimals;
                        arr.Add(bj);
                    }

                    //串码
                    var list2 = from a in lqh.Umsdb.Pro_ProInfo
                               join b in lqh.Umsdb.Pro_StoreInfo on a.ProID equals b.ProID
                               join c in lqh.Umsdb.Sys_Role_HallInfo on b.HallID equals c.HallID
                               join d in lqh.Umsdb.Sys_UserInfo on c.RoleID equals d.RoleID
                                join m in lqh.Umsdb.Pro_IMEI on a.ProID equals m.ProID 
                               where d.UserID == user.UserID && c.HallID == hid  && b.InListID == m.InListID
                               && b.ProCount > 0 && a.NeedIMEI==true 
                               && m.BJID==null &&m.AssetID==null && m.AuditID == null&&m.BorowID == null
                               &&m.OutID == null && m.PJID ==null && m.RepairID == null && m.ReturnID == null 
                               && m.SellID == null && m.VIPID == null
                               select new
                               {
                                   a.ProID,
                                   a.ProName,
                                   a.ProFormat,
                                   a.NeedIMEI,
                                   a.Pro_ClassID,
                                   a.Pro_TypeID,
                                   a.ProMainID,
                                   b.InListID,
                                   m.IMEI,
                                   a.ISdecimals
                               };

                    foreach (var item in list2)
                    {
                        if (isBJ)
                        {
                            if (item.Pro_ClassID != classid)
                            {
                                continue;
                            }
                        }
                        Model.BJModel bj = new BJModel();
                        bj.ClassID = Convert.ToInt32(item.Pro_ClassID);
                        bj.IMEI = item.IMEI;
                        bj.InListID = item.InListID;
                        bj.NeedIMEI = item.NeedIMEI;
                        bj.ProFormat = item.ProFormat;
                        bj.ProID = item.ProID;
                        bj.ProMainID = Convert.ToInt32(item.ProMainID);
                        bj.ProName = item.ProName;
                        bj.TypeID = Convert.ToInt32(item.Pro_TypeID);
                        bj.IsDecimal = item.ISdecimals;
                        arr.Add(bj);
                    }
                    return new WebReturn() {ReturnValue = true,Obj = arr };
                }
                catch (Exception ex)
                {
                    return new WebReturn() { Obj = new List<Model.Pro_ProInfo>(),Message =ex.Message,ReturnValue=false};
                    
                }
            }
        }


        /// <summary>
        /// 3
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isBJ">是否只获取备机类别数据</param>
        /// <returns></returns>
        //public Model.WebReturn GetPJList(Model.Sys_UserInfo user, string hid)
        //{
        //    using (LinQSqlHelper lqh = new LinQSqlHelper())
        //    {
        //        try
        //        {
        //            //非串码
        //            var list = from a in lqh.Umsdb.Pro_ProInfo
        //                       join b in lqh.Umsdb.Pro_StoreInfo on a.ProID equals b.ProID
        //                       join c in lqh.Umsdb.Sys_Role_HallInfo on b.HallID equals c.HallID
        //                       join d in lqh.Umsdb.Sys_UserInfo on c.RoleID equals d.RoleID
        //                       where d.UserID == user.UserID && c.HallID == hid
        //                       && b.ProCount > 0 && a.NeedIMEI == false
        //                       select new
        //                       {
        //                           a.ProID,
        //                           a.ProName,
        //                           a.ProFormat,
        //                           a.NeedIMEI,
        //                           a.Pro_ClassID
        //                           ,
        //                           a.Pro_TypeID,
        //                           a.ProMainID,
        //                           b.InListID,
        //                           a.ISdecimals
        //                       };

        //            List<Model.BJModel> arr = new List<BJModel>();
        //            foreach (var item in list)
        //            {
                       
        //                Model.BJModel bj = new BJModel();
        //                bj.ClassID = Convert.ToInt32(item.Pro_ClassID);
        //                bj.InListID = item.InListID;
        //                bj.NeedIMEI = item.NeedIMEI;
        //                bj.ProFormat = item.ProFormat;
        //                bj.ProID = item.ProID;
        //                bj.ProMainID = Convert.ToInt32(item.ProMainID);
        //                bj.ProName = item.ProName;
        //                bj.TypeID = Convert.ToInt32(item.Pro_TypeID);
        //                bj.IsDecimal = item.ISdecimals;
        //                arr.Add(bj);
        //            }

        //            //串码
        //            var list2 = from a in lqh.Umsdb.Pro_ProInfo
        //                        join b in lqh.Umsdb.Pro_StoreInfo on a.ProID equals b.ProID
        //                        join c in lqh.Umsdb.Sys_Role_HallInfo on b.HallID equals c.HallID
        //                        join d in lqh.Umsdb.Sys_UserInfo on c.RoleID equals d.RoleID
        //                        join m in lqh.Umsdb.Pro_IMEI on a.ProID equals m.ProID
        //                        where d.UserID == user.UserID && c.HallID == hid && b.InListID == m.InListID
        //                        && b.ProCount > 0 && a.NeedIMEI == true
        //                        && m.BJID == null && m.AssetID == null && m.AuditID == null && m.BorowID == null
        //                        && m.OutID == null && m.PJID == null && m.RepairID == null && m.ReturnID == null
        //                        && m.SellID == null && m.VIPID == null
        //                        select new
        //                        {
        //                            a.ProID,
        //                            a.ProName,
        //                            a.ProFormat,
        //                            a.NeedIMEI,
        //                            a.Pro_ClassID,
        //                            a.Pro_TypeID,
        //                            a.ProMainID,
        //                            b.InListID,
        //                            m.IMEI,
        //                            a.ISdecimals
        //                        };

        //            foreach (var item in list2)
        //            {
        //                Model.BJModel bj = new BJModel();
        //                bj.ClassID = Convert.ToInt32(item.Pro_ClassID);
        //                bj.IMEI = item.IMEI;
        //                bj.InListID = item.InListID;
        //                bj.NeedIMEI = item.NeedIMEI;
        //                bj.ProFormat = item.ProFormat;
        //                bj.ProID = item.ProID;
        //                bj.ProMainID = Convert.ToInt32(item.ProMainID);
        //                bj.ProName = item.ProName;
        //                bj.TypeID = Convert.ToInt32(item.Pro_TypeID);
        //                bj.IsDecimal = item.ISdecimals;
        //                arr.Add(bj);
        //            }
        //            return new WebReturn() { ReturnValue = true, Obj = arr };
        //        }
        //        catch (Exception ex)
        //        {
        //            return new WebReturn() { Obj = new List<Model.Pro_ProInfo>(), Message = ex.Message, ReturnValue = false };

        //        }
        //    }
        //}   
        
        #endregion 

    }

   
}
