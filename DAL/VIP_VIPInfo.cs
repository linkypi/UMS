using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    /// <summary>
    /// 会员注册
    /// </summary>
    public class VIP_VIPInfo
    {
        private int _MethodID;
        private int MenuID = 51;
        public int MethodID
        {
            get { return _MethodID; }
            set { _MethodID = value; }
        }
        public VIP_VIPInfo()
        {
            this.MethodID = 0;
        }

        public VIP_VIPInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }
          private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
            new Model.ReportSqlParams_String(){ParamName="IMEI" },
            new Model.ReportSqlParams_ListString(){ParamName="IMEI_List" },
            new Model.ReportSqlParams_String(){ParamName="IDCard"},
            new Model.ReportSqlParams_String(){ParamName="MemberName"}, 
            new Model.ReportSqlParams_String(){ParamName="MobiPhone"},
            new Model.ReportSqlParams_String(){ParamName="StartTime"},
            new Model.ReportSqlParams_String(){ParamName="EndTime"},
        };
    
        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }
        #region 获取VIP实体
        /// <summary>
        /// 获取调入实体
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

                    if (pageParam == null )
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();
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

                    var inorder_query = from b in lqh.Umsdb.View_VIPInfo
                                        
                                        //join r in lqh.Umsdb.Sys_Role_Menu_HallInfo on b.FromHallID equals r.HallID
                                        //where r.RoleID == user.RoleID && r.MenuID == 13
                                        //where c.RoleID == user.RoleID && c.MenuID == MenuID
                                        select b;
                    foreach (var m in param_join)
                    {
                        //此处出现问题拉
                        //if (m.ParamBehind == null)//不存在字段
                        //{
                        //    continue;
                        //}
                        //new Model.ReportSqlParams(){ParamName="InOrderID" },
                        //new Model.ReportSqlParams(){ParamName="Pro_HallID"},
                        //new Model.ReportSqlParams(){ParamName="OldID"}, 
                        //new Model.ReportSqlParams(){ParamName="UserName"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_start"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_end"},
                        //new Model.ReportSqlParams(){ParamName="Note"}

                        switch (m.ParamFront.ParamName)
                        {
                            case "IMEI":
                                Model.ReportSqlParams_String mm = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.IMEI.Contains(mm.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "IMEI_List":
                                Model.ReportSqlParams_ListString mmL = (Model.ReportSqlParams_ListString)m.ParamFront;
                                if (mmL.ParamValues.Count()==0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where mmL.ParamValues.Contains(b.IMEI)
                                                    select b;
                                    break;
                                }

                            case "IDCard":
                                Model.ReportSqlParams_String mm0 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm0.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.IDCard.Contains(mm0.ParamValues)
                                                    select b;
                                    break;
                                }
                            case "MemberName":
                                Model.ReportSqlParams_String mm1 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (mm1.ParamValues == null || mm1.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.MemberName.Contains(mm1.ParamValues)
                                                    select b;
                                    break;
                                }

                            case "MobiPhone":
                                Model.ReportSqlParams_String mm2 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (mm2.ParamValues == null || mm2.ParamValues.Count() == 0)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.MobiPhone==mm2.ParamValues
                                                    select b;
                                    break;
                                }
                            case "StartTime":
                                Model.ReportSqlParams_DataTime mm5 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm5.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.NewStartTime >= mm5.ParamValues
                                                    select b;
                                    break;
                                }
                            case "EndTime":
                                Model.ReportSqlParams_DataTime mm6 = (Model.ReportSqlParams_DataTime)m.ParamFront;
                                if (mm6.ParamValues == null)
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.NewStartTime <= mm6.ParamValues
                                                    select b;
                                    break;
                                }
                            default: break;
                        }
                    }
                    #endregion

                    #region 过滤仓库
                    //if (ValidHallIDS.Count() > 0)
                    //    inorder_query = from b in inorder_query
                    //                    where ValidHallIDS.Contains(b.Pro_HallID)
                    //                    orderby b.SysDate descending
                    //                    select b;

                    //else
                        inorder_query = from b in inorder_query
                                        orderby b.SysDate descending
                                        select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数
                    int pagecount=0;
                    if (pageParam.PageSize > 0)
                    {
                        pagecount = pageParam.RecordCount / pageParam.PageSize;
                    }

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.View_VIPInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        if (pageParam.PageSize > 0)
                        {
                            List<Model.View_VIPInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                            pageParam.Obj = list;
                        }
                        else
                            pageParam.Obj = inorder_query.ToList();
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
                //}
            }
        }
        #endregion
        
        #region 注册
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.VIP_VIPInfo_Temp model)
        {

            //插入会员信息
            //不需要(插入类别指定的服务信息 VIP_VIPService)
            //调用销售 待定 留接口
            //返回
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    try
                    {
                       
                        #region 添加会员
                        //判断会员是否已经存在 
                        if (lqh.Umsdb.VIP_VIPInfo.Where(p => p.IMEI == model.IMEI).Count() != 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "已存在该会员" };                      
                        }
                        //判断该会员卡号是不是在一个仓库
                        if (lqh.Umsdb.Pro_IMEI.Where(p => p.IMEI == model.IMEI && p.HallID == model.HallID).Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "该卡不属于该营业厅" };                      
                        }
                        double days =(double) model.Validity;
                        model.EndTime = model.StartTime.Value.AddDays(days);
                        model.Flag = true;
                        model.SysDate = DateTime.Now;
                        model.UpdUser = user.UserID;
                        
                        #endregion

                        #region 添加会员服务
                       
                        var query_Service = (from b in lqh.Umsdb.VIP_VIPTypeService
                                          
                                            select b).ToList();
                        if (query_Service.Count != 0)
                        {
                            query_Service=(from b in query_Service
                                            where b.TypeID == model.TypeID
                                            select b).ToList();
                            if (query_Service.Count() == 0)
                            {
                                return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "不存在卡类型" };

                            }
                        }
                        //添加会员服务
                        foreach (var queryService in query_Service)
                        {
                            Model.VIP_VIPService service = new Model.VIP_VIPService();
                            service.ProID = queryService.ProID;
                            service.SCount = queryService.SCount;
                            if (model.VIP_VIPService == null)
                            {
                                model.VIP_VIPService = new System.Data.Linq.EntitySet<Model.VIP_VIPService>();

                            }
                            model.VIP_VIPService.Add(service);
                        }
                   
                        #endregion
                        #region 更改串码(不要需要)
                        var GetList = from b in lqh.Umsdb.Pro_IMEI
                                      join c in lqh.Umsdb.Pro_StoreInfo on b.InListID equals c.InListID
                                      where b.IMEI == model.IMEI
                                      select new
                                          {
                                              b,
                                              c
                                          };
                        if (GetList == null || GetList.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "库存出错，请联系管理员" };
                        }
                        Pro_IMEI newImei = GetList.First().b;
                        //newImei.VIPID = model.ID;
                        #endregion
                        /* 不减库存
                        Pro_StoreInfo newSotre = GetList.First().c;
                        newSotre.ProCount -= 1;
                         * */
                        #region 生成订单
                        var queryType = from b in lqh.Umsdb.VIP_VIPType
                                        where b.ID == model.TypeID
                                        select b;
                        if (queryType == null || queryType.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "不存在卡类型" };
                        }
                        Model.Pro_SellListInfo_Temp tem = new Model.Pro_SellListInfo_Temp();
                        Model.VIP_VIPType VIPType = queryType.First();
                        if(VIPType.Cost_production!=null)
                           tem.CashPrice =(decimal)VIPType.Cost_production;
                        tem.IMEI = model.IMEI;                     
                        tem.ProID = newImei.ProID;
                        tem.ProCount = 1;
                        tem.ProPrice = tem.CashPrice;
                        tem.IsFree = false;
                        tem.SellType = 1;
                        tem.SellType_Pro_ID = 1;

                        tem.HallID = model.HallID;
                        tem.OldID = model.OldID;
                        tem.UserID = user.UserID;
                        tem.InsertDate = DateTime.Now;
                        tem.Note = model.Note;

                        lqh.Umsdb.Pro_SellListInfo_Temp.InsertOnSubmit(tem);
                        lqh.Umsdb.SubmitChanges();
                        #endregion 
                        model.SellListID = tem.ID;
                        lqh.Umsdb.VIP_VIPInfo_Temp.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                         
                        return new Model.WebReturn() { Obj = null, ReturnValue=true, Message = "新增成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message="未成功" };
                    }
                }
            }
                     
        }
        #endregion 
        #region 修改会员卡资料
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.VIP_VIPInfo vip_vipInfo)
        {
            //备份VIP_VIPInfo_BAK
            //修改会员信息 只修改无关联字段，客户个人信息 
            //返回
            string Msg = "";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var queryZC = from b in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                                      where b.ID == vip_vipInfo.ID
                                      select b;
                        if (queryZC.Count() == 0)
                        {
                            Msg = "会员" + vip_vipInfo.ID + " 不存在";
                            return new Model.WebReturn() {  ReturnValue=false, Message = Msg };
                        }
                        #region 插入备份表
                        Model.VIP_VIPInfo vip = queryZC.First();
                        Model.VIP_VIPInfo_BAK vip_bak =new Model.VIP_VIPInfo_BAK();                
                        vip_bak.Address = vip.Address;
                        vip_bak.Birthday = vip.Birthday;
                        vip_bak.IDCard = vip.IDCard;
                        vip_bak.IDCard_ID = vip.IDCard_ID;
                        vip_bak.MemberName = vip.MemberName;
                        vip_bak.MobiPhone = vip.MobiPhone;
                        vip_bak.Note = vip.Note;
                        vip_bak.Old_ID = vip.ID;
                        vip_bak.QQ = vip.QQ;
                        vip_bak.Sex = vip.Sex;
                        vip_bak.SysDate = vip.SysDate;
                        vip_bak.TelePhone = vip.TelePhone;
                        vip_bak.UpdUser = vip.UpdUser;
                        vip_bak.Validity = vip.Validity;
                        vip_bak.TypeID = vip.TypeID;
                        vip_bak.TelePhone = vip.TelePhone;
                        vip_bak.StartTime = vip.StartTime;
                        vip_bak.Flag = vip.Flag;
                        vip_bak.Point = vip.Point;
                        vip_bak.Balance = vip.Balance;
                        vip_bak.Note = vip.Note;
                        vip_bak.HallID = vip.HallID;

                        lqh.Umsdb.VIP_VIPInfo_BAK.InsertOnSubmit(vip_bak);
                        #endregion
                        #region 更新会员信息
                        vip_vipInfo.UpdUser = user.UserID;
                        vip.MemberName = vip_vipInfo.MemberName;
                        vip.Sex = vip_vipInfo.Sex;
                        vip.Birthday = vip_vipInfo.Birthday;
                        vip.MobiPhone = vip_vipInfo.MobiPhone;
                        vip.TelePhone = vip_vipInfo.TelePhone;
                        vip.QQ = vip_vipInfo.QQ;
                        vip.Address = vip_vipInfo.Address;
                        vip.IDCard = vip_vipInfo.IDCard;
                        vip.IDCard_ID = vip_vipInfo.IDCard_ID;
                        vip.SysDate = DateTime.Now;
                        vip.UpdUser = user.UserID;
                        vip.Note = vip_vipInfo.Note;        
                        lqh.Umsdb.SubmitChanges();
                        #endregion
                        #region 返回更新内容
                        var query = (from b in lqh.Umsdb.View_VIPInfo
                                     where b.ID == vip.ID
                                     select b).ToList();
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "返回失败" };
                        }
                        ts.Complete();
                        #endregion                         
                        return new Model.WebReturn() { Obj =query.First(), ReturnValue=true, Message = "更新成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue=false};
                    }
                }

            }
        }
        #endregion
        #region 退卡操作
        /// <summary>
        /// 退卡
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.VIP_VIPBack model)
        {
            //验证退卡审批单状态 审批单金额有效性，不能超过卡年费
            //更改审批单状态
            //插入退卡信息，VIP_VIPBack
            //卡作废，将卡放入Pro_IMEI_Deleted，Pro_IMEI不能删

            string Msg = ""; 
            if(model==null)
                return new Model.WebReturn() { ReturnValue = false, Message = "参数错误！" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        var exist_vipback = from b in lqh.Umsdb.VIP_VIPBack
                                      where b.VIP_ID == model.VIP_ID
                                      select b;
                        if (exist_vipback.Count() > 0)
                        {
                            Msg = "该会员" + model.VIP_ID + "已退卡";
                            return new Model.WebReturn() { ReturnValue=false, Message = Msg };
                        }
                        var exist_vip = from b in lqh.Umsdb.VIP_VIPInfo
                                      where b.ID == model.VIP_ID&&b.Flag==true
                                      select b;
                         if (exist_vip.Count()== 0)
                        {
                            Msg = "该会员" + model.VIP_ID + "不存在";
                            return new Model.WebReturn() { ReturnValue=false, Message = Msg };
                        }

                        var query = from b in lqh.Umsdb.VIP_VIPBackAduit
                                    where b.AduitID == model.AduitID&&b.VIP_ID==model.VIP_ID
                                    select b;
                        if (query.Count() == 0)
                        {
                            Msg = "审批单不存在";
                            return new Model.WebReturn {  ReturnValue=false, Message = Msg };
                        }
                        Model.VIP_VIPBackAduit backAduit = query.First();
                        if (backAduit.Used == true)
                        {
                            Msg = "审批" + model.ID + " 已使用";
                            return new Model.WebReturn { Obj = null, ReturnValue = false, Message = Msg };
                        }
                        if (backAduit.Aduited !=true)
                        {
                            Msg = "审批单" + model.ID + "未审核";
                            return new Model.WebReturn { Obj = null, ReturnValue = false, Message = Msg };
                        }
                        if (backAduit.Passed != true)
                        {
                            Msg = "审批" + model.ID + "未通过";
                            return new Model.WebReturn { Obj = null, ReturnValue = false, Message = Msg };
                        }
                        //var cost_cb=from b in lqh.Umsdb.GetTable<Model.VIP_VIPType>()
                        //            where b.ID==exist_vip.First().TypeID
                        //            select b;
                        Model.VIP_VIPInfo vip = exist_vip.First();
                        if (backAduit.Money > vip.ProPrice)
                        {
                            Msg = "审批金额过大";
                            return new Model.WebReturn { Obj = null, ReturnValue = false, Message = Msg };
                        }

                        backAduit.Used = true;                   
                        vip.Flag = false;
                        model.UserID = user.UserID;
                        model.Note = "成功退卡";
                        model.Return_Money = backAduit.Money;
                        model.SysDate = DateTime.Now;
                        /* 不用考虑库存，故不用销串码
                        Model.Pro_IMEI_Deleted delete = new Model.Pro_IMEI_Deleted();                       
                        delete.IMEI = vip.IMEI;
                        lqh.Umsdb.VIP_VIPBack.InsertOnSubmit(model);
                        lqh.Umsdb.Pro_IMEI_Deleted.InsertOnSubmit(delete);
                         */
                        lqh.Umsdb.VIP_VIPBack.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() {  ReturnValue=true, Message = "退卡成功" };
                    }

                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue=false,Message="退卡失败" };
                    }
                }
            }
        }
        #endregion
        #region 其他操作
        public Model.WebReturn  VIP_offTicket(Model.Sys_UserInfo user, Model.VIP_VIPBack model)
       {
           using(LinQSqlHelper lqh =new LinQSqlHelper())
           {
               var ticket = from b in lqh.Umsdb.GetTable<Model.VIP_OffTicket>()
                            select b;
               return new Model.WebReturn() { Obj = ticket.ToList(), ReturnValue = false, Message = "退卡成功" };
           }
       }
        #endregion
        #region 新的换卡，补卡，升级卡操作
        public Model.WebReturn LevelUp(Model.Sys_UserInfo user, Model.VIP_VIPInfo_Temp model,int VIPID)
        {
            if (model == null) 
                return new Model.WebReturn() {  ReturnValue = false, Message = "参数有误" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region 获取会员资料
                        var query = from b in lqh.Umsdb.VIP_VIPInfo
                                    where b.ID == VIPID
                                    select b;
                        if (query.Count() == 0 )
                        {
                            return new Model.WebReturn { Obj = null, ReturnValue = false, Message = "该会员不存在,无法进行操作" };
                        }
                        var query_VIPType = from b in lqh.Umsdb.Pro_ProInfo
                                            from imei in b.Pro_IMEI
                                            join r in lqh.Umsdb.VIP_VIPType on b.VIP_TypeID equals r.ID
                                            from service in r.VIP_VIPTypeService
                                            where imei.IMEI == model.IMEI && imei.VIPID == null && imei.SellID == null
                                            && imei.OutID == null && imei.BorowID == null && imei.RepairID == null && (imei.AssetID == null || imei.AssetID == 0)
                                            select r;
                        if (query_VIPType.Count() == 0)
                        {
                            return new Model.WebReturn {  ReturnValue = false, Message = "该会员卡不存在或存在其他操作" };
                        }

                        var query_Hall = from b in lqh.Umsdb.Pro_IMEI
                                         where b.IMEI==model.IMEI
                                         select b.HallID;
                        if (query_Hall.Count() == 0)
                        {
                            return new Model.WebReturn { ReturnValue = false, Message = "该会员卡不存在或存在其他操作" };
                        }
                        #endregion
                        Model.VIP_VIPInfo VIPInfo = query.First();
                        Model.VIP_VIPType VIPType = query_VIPType.First();
                        #region 添加会员
                        //判断会员是否已经存在 
                        model.Address = VIPInfo.Address;
                        model.Balance = VIPInfo.Balance;
                        model.Birthday = VIPInfo.Birthday;
                        model.StartTime = VIPInfo.StartTime;
                        double days = (double)VIPType.Validity;
                        model.EndTime = model.StartTime.Value.AddDays(days);
                        model.Flag = true;
                        model.HallID = query_Hall.First();
                        model.IDCard = VIPInfo.IDCard;
                        model.IDCard_ID = VIPInfo.IDCard_ID;
                        model.MemberName = VIPInfo.MemberName;
                        model.MobiPhone = VIPInfo.MobiPhone;
                      //  model.Note = VIPInfo.Note;
                        model.Password = VIPInfo.Password;
                      //  model.Point = VIPInfo.Point;
                        model.ProPrice = VIPType.Cost_production;
                        model.QQ = VIPInfo.QQ;
                        model.Sex = VIPInfo.Sex;
                        model.SysDate = DateTime.Now;
                        model.TelePhone = VIPInfo.TelePhone;
                        model.TypeID = VIPType.ID;
                        model.UpdUser = user.UserID;
                        model.UserName = VIPInfo.UserName;
                        model.Validity = VIPType.Validity;
                        
                 
                        #endregion

                        #region 添加会员服务
                        var query_Service = from b in lqh.Umsdb.VIP_VIPTypeService
                                    where b.TypeID == model.TypeID
                                    select b;
                        if (query_Service.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "不存在卡类型" };
                        }
                        //添加会员服务
                        foreach (var queryService in query_Service)
                        {
                            Model.VIP_VIPService service = new Model.VIP_VIPService();
                            service.ProID = queryService.ProID;
                            service.SCount = queryService.SCount;
                            if (model.VIP_VIPService == null)
                            {
                                model.VIP_VIPService = new System.Data.Linq.EntitySet<Model.VIP_VIPService>();
                            
                            }
                            model.VIP_VIPService.Add(service);
                        }                 
                        #endregion
                        #region 更改串码
                        var GetList = from b in lqh.Umsdb.Pro_IMEI
                                      join c in lqh.Umsdb.Pro_StoreInfo on b.InListID equals c.InListID
                                      where b.IMEI == model.IMEI
                                      select new
                                      {
                                          b,
                                          c
                                      };
                        if (GetList == null || GetList.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "库存出错，请联系管理员" };
                        }
                        Pro_IMEI newImei = GetList.First().b;
                        //newImei.VIPID = model.ID;
                        #endregion
                        /* 不减库存
                        Pro_StoreInfo newSotre = GetList.First().c;
                        newSotre.ProCount -= 1;
                         * */
                        #region 生成订单

                        Model.Pro_SellListInfo_Temp tem = new Model.Pro_SellListInfo_Temp();       
                        tem.CashPrice = (decimal)VIPType.Cost_production;
                        tem.IMEI = model.IMEI;
                        tem.ProID = newImei.ProID;
                        tem.ProCount = 1;
                        tem.ProPrice = tem.CashPrice;
                        tem.IsFree = false;
                        tem.SellType = 1;
                        tem.SellType_Pro_ID = 1;

                        tem.OldID = model.OldID;
                        tem.Note = model.Note;
                        tem.UserID = user.UserID;
                        tem.HallID = model.HallID;
                        tem.InsertDate = DateTime.Now;
                        lqh.Umsdb.Pro_SellListInfo_Temp.InsertOnSubmit(tem);
                        lqh.Umsdb.SubmitChanges();
                        #endregion
                        model.SellListID = tem.ID;
                        lqh.Umsdb.VIP_VIPInfo_Temp.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();

                        return new Model.WebReturn() { ReturnValue = true, Message = "操作成功" };
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "操作失败" };
                    }
                }
            }
        }
        #endregion
        #region 暂时不需要
        /// <summary>
        /// 换卡、升级、补卡
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn LevelUp(Model.Sys_UserInfo user, Model.VIP_VIPInfo model, string imei)
        {
            //插入会员信息VIP_VIPInfo
            //插入类别指定的服务信息 VIP_VIPService
            //调用销售 待定 留接口
            //插入换卡信息VIP_CardChange 
            //更新旧卡信息VIP_VIPInfo Flag
            //返回
            string Msg = "";

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        //退卡
                        var query = from b in lqh.Umsdb.GetTable<Model.VIP_VIPInfo>()
                                    where b.IMEI == imei
                                    select b;
                        if (query.Count() == 0 || query == null)
                        {
                            Msg = "该会员不存在,无法进行换卡操作";
                            return new Model.WebReturn { Obj = null, ReturnValue = false, Message = Msg };
                        }

                        Model.VIP_VIPInfo vip = query.First();
                        Model.VIP_VIPBack vipback = new Model.VIP_VIPBack();
                        //退卡
                        vipback.VIP_ID = vip.ID;
                        vipback.UserID = user.UserID;
                        vip.Flag = true;
                        lqh.Umsdb.VIP_VIPBack.InsertOnSubmit(vipback);
                        //注册
                        Model.VIP_VIPInfo newvip = new Model.VIP_VIPInfo();

                        newvip.UpdUser = user.UserID;
                        newvip.IMEI = model.IMEI;
                        newvip.Address = vip.Address;
                        newvip.Balance = vip.Balance;
                        newvip.Birthday = vip.Birthday;
                        newvip.IDCard = vip.IDCard;
                        newvip.IDCard_ID = vip.IDCard_ID;
                        newvip.MemberName = vip.MemberName;
                        newvip.MobiPhone = vip.MobiPhone;
                        newvip.Note = vip.Note;
                        newvip.Point = vip.Point;
                        newvip.ProPrice = vip.ProPrice;
                        newvip.QQ = vip.QQ;
                        newvip.Seller = user.UserName;
                        newvip.SellID = int.Parse(user.UserID);
                        newvip.Sex = vip.Sex;
                        newvip.StartTime = DateTime.Now;
                        newvip.SysDate = DateTime.Now;
                        newvip.TelePhone = vip.TelePhone;
                        newvip.TypeID = model.TypeID;
                        newvip.UpdUser = vip.UpdUser;
                        newvip.Validity = model.Validity;
                        newvip.ProPrice = model.ProPrice;

                        lqh.Umsdb.VIP_VIPInfo.InsertOnSubmit(newvip);
                        lqh.Umsdb.SubmitChanges();

                        var query_oldService = from b in lqh.Umsdb.VIP_VIPService
                                               where b.VIPID == model.ID
                                               select b;
                        if (query_oldService == null || query_oldService.Count() == 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "不存在服务" };
                        }
                        foreach (var serviceItem in query_oldService)
                        {

                            Model.VIP_VIPService_BAK serviceBAK = new Model.VIP_VIPService_BAK();
                            serviceBAK.OldID = serviceItem.ID;
                            serviceBAK.OldVIPID = int.Parse(imei);
                            serviceBAK.NewVIPID = model.ID;
                            serviceBAK.ProID = int.Parse(serviceItem.ProID);
                            serviceBAK.SCount = serviceItem.SCount;
                            serviceBAK.UpdUser = user.UserID;
                            lqh.Umsdb.VIP_VIPService_BAK.InsertOnSubmit(serviceBAK);

                            //旧服务复制
                            //Model.VIP_VIPService service = new Model.VIP_VIPService();
                            //service.ProID = serviceItem.ProID;
                            //service.SCount = serviceItem.SCount;
                            //service.VIPID = newvip.ID;
                            //lqh.Umsdb.VIP_VIPService.InsertOnSubmit(service);
                            lqh.Umsdb.SubmitChanges();

                        }
                        #region 暂时不需要
                        var queryService = (from b in lqh.Umsdb.VIP_VIPService
                                            where b.VIPID == model.ID
                                            select new
                                            {
                                                b.ProID,
                                                b.SCount
                                            }).Union(
                                            from b in lqh.Umsdb.VIP_VIPService
                                            where b.VIPID == model.ID
                                            select new
                                            {
                                                b.ProID,
                                                b.SCount
                                            });

                        var queryService1 = from b in queryService
                                            group b by b.ProID into g
                                            select new
                                            {
                                                g.Key,
                                                TotalScount = g.Sum(p => p.SCount)
                                            };
                        foreach (var index in queryService1)
                        {
                            Model.VIP_VIPService service = new Model.VIP_VIPService();
                            service.ProID = index.Key;
                            service.SCount = index.TotalScount;
                            service.VIPID = newvip.ID;
                            lqh.Umsdb.VIP_VIPService.InsertOnSubmit(service);
                            lqh.Umsdb.SubmitChanges();
                        }
                        #endregion
                        Model.Pro_IMEI_Deleted delete = new Model.Pro_IMEI_Deleted();
                        delete.IMEI = vip.IMEI;
                        lqh.Umsdb.Pro_IMEI_Deleted.InsertOnSubmit(delete);
                        ts.Complete();
                        return new Model.WebReturn() { Obj = null, ReturnValue = true, Message = "成功更换" };
                    }

                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "操作失败" };
                    }
                }

            }
        }
          #endregion 

        /// <summary>
        /// 根据会员卡号获取vip信息  319
        /// </summary>
        /// <param name="user"></param>
        /// <param name="imei"></param>
        /// <returns></returns>
        public Model.WebReturn GetVipInfo(Model.Sys_UserInfo user, string imei )
        {
            using(LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try 
	            {	        
		             var vip = from a in lqh.Umsdb.VIP_VIPInfo
                            where a.IMEI.ToLower() == imei.ToLower()  
                            select a ;
                    return new WebReturn(){ReturnValue =true,Obj = vip.First()};
	            }
	            catch (Exception ex)
	            {
		            return new WebReturn(){ReturnValue = false,Message=ex.Message};
	            }
           
            }
        }
    }
}
