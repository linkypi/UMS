using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Services;
using DAL;
using Model;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.SessionState;
using Common;
using System.Reflection;
using System.Collections;
using System.Transactions;
using ReportModel;
using Sys_UserInfo = Model.Sys_UserInfo;
using System.Data.Services;


namespace UserMSService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“UserMsService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 UserMsService.svc 或 UserMsService.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceContract(SessionMode = SessionMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]


    #region Model序列化定义

    [ServiceKnownType(typeof(Model.Report_IMEIInfo))]
    [ServiceKnownType(typeof(Model.GetInOutSellInfoResult))]
    [ServiceKnownType(typeof(Model.AduitListInfo))]
    [ServiceKnownType(typeof(Model.AduitModel))]
    [ServiceKnownType(typeof(Model.BorowListModel))]
    [ServiceKnownType(typeof(Model.BorowModel))]
    [ServiceKnownType(typeof(Model.IMEIModel))]
    [ServiceKnownType(typeof(Model.MenuInfo))]
    [ServiceKnownType(typeof(Model.NoIMEIModel))]
    [ServiceKnownType(typeof(Model.Pro_SellInfo_Child))]
    [ServiceKnownType(typeof(Model.Pro_SellListInfo_Child))]
    [ServiceKnownType(typeof(Model.RenewModel))]
    [ServiceKnownType(typeof(Model.RepairListModel))]
    [ServiceKnownType(typeof(Model.RepairModel))]
    [ServiceKnownType(typeof(Model.SetSelection))]
    [ServiceKnownType(typeof(Model.Sys_InitDataStatus_Child))]
    [ServiceKnownType(typeof(Model.Pro_AreaInfo))]
    [ServiceKnownType(typeof(Model.Pro_BackInfo))]
    [ServiceKnownType(typeof(Model.Pro_BackListInfo))]
    [ServiceKnownType(typeof(Model.Pro_BackOrderIMEI))]
    [ServiceKnownType(typeof(Model.Pro_BorowAduit))]
    [ServiceKnownType(typeof(Model.Pro_BorowAduitList))]
    [ServiceKnownType(typeof(Model.Pro_BorowInfo))]
    [ServiceKnownType(typeof(Model.Pro_BorowListInfo))]
    [ServiceKnownType(typeof(Model.Pro_BorowOrderIMEI))]
    [ServiceKnownType(typeof(Model.Pro_CashTicket))]
    [ServiceKnownType(typeof(Model.Pro_PriceChangeList))]
    [ServiceKnownType(typeof(Model.Pro_ClassInfo))]
    [ServiceKnownType(typeof(Model.Pro_HallInfo))]
    [ServiceKnownType(typeof(Model.Pro_IMEI))]
    [ServiceKnownType(typeof(Model.Pro_IMEI_Deleted))]
    [ServiceKnownType(typeof(Model.Pro_InOrder))]
    [ServiceKnownType(typeof(Model.Pro_InOrderIMEI))]
    [ServiceKnownType(typeof(Model.Pro_InOrderList))]
    [ServiceKnownType(typeof(Model.Pro_LevelInfo))]
    [ServiceKnownType(typeof(Model.Pro_OutInfo))]
    [ServiceKnownType(typeof(Model.Pro_OutModel))]
    [ServiceKnownType(typeof(Model.Pro_OutOrderIMEI))]
    [ServiceKnownType(typeof(Model.Pro_OutOrderList))]
    [ServiceKnownType(typeof(Model.Pro_PriceChange))]
    [ServiceKnownType(typeof(Model.Pro_PriceCostChange))]
    [ServiceKnownType(typeof(Model.Pro_PriceCostChangeList))]
    [ServiceKnownType(typeof(Model.Pro_ProInfo))]
    [ServiceKnownType(typeof(Model.Pro_Property))]
    [ServiceKnownType(typeof(Model.Pro_PropertyValue))]
    [ServiceKnownType(typeof(Model.Pro_ProProperty_F))]
    [ServiceKnownType(typeof(Model.Pro_RepairInfo))]
    [ServiceKnownType(typeof(Model.Pro_RepairListInfo))]
    [ServiceKnownType(typeof(Model.Pro_RepairReturnInfo))]
    [ServiceKnownType(typeof(Model.Pro_RepairReturnListInfo))]
    [ServiceKnownType(typeof(Model.Pro_ReturnInfo))]
    [ServiceKnownType(typeof(Model.Pro_ReturnListInfo))]
    [ServiceKnownType(typeof(Model.Pro_ReturnOrderIMEI))]
    [ServiceKnownType(typeof(Model.Pro_SellAduit))]
    [ServiceKnownType(typeof(Model.Pro_SellAduitList))]
    [ServiceKnownType(typeof(Model.Pro_SellBack))]
    [ServiceKnownType(typeof(Model.Pro_SellBackAduit))]
    [ServiceKnownType(typeof(Model.Pro_SellBackIMEIList))]
    [ServiceKnownType(typeof(Model.Pro_SellBackList))]
    [ServiceKnownType(typeof(Model.Pro_SellBackSpecalOffList))]
    [ServiceKnownType(typeof(Model.Pro_SellIMEIList))]
    [ServiceKnownType(typeof(Model.Pro_SellInfo))]
    [ServiceKnownType(typeof(Model.Pro_SellListInfo))]
    [ServiceKnownType(typeof(Model.Pro_SellListServiceInfo))]
    [ServiceKnownType(typeof(Model.Pro_SellSendInfo))]
    [ServiceKnownType(typeof(Model.Pro_SellSpecalOffList))]
    [ServiceKnownType(typeof(Model.Pro_SellType))]
    [ServiceKnownType(typeof(Model.Pro_SellTypeProduct))]
    [ServiceKnownType(typeof(Model.Pro_StoreInfo))]
    [ServiceKnownType(typeof(Model.Pro_TypeInfo))]
    [ServiceKnownType(typeof(Model.Sys_DeptInfo))]
    [ServiceKnownType(typeof(Model.Sys_InitDataStatus))]
    [ServiceKnownType(typeof(Model.Sys_LoginInfo))]
    [ServiceKnownType(typeof(Model.Sys_MenuInfo))]
    [ServiceKnownType(typeof(Model.Sys_MethodInfo))]
    [ServiceKnownType(typeof(Model.Sys_Option))]
    [ServiceKnownType(typeof(Model.Sys_OrderMakerInfo))]
    [ServiceKnownType(typeof(Model.Sys_RemindList))]
    [ServiceKnownType(typeof(Model.Sys_Role_HallInfo))]
    [ServiceKnownType(typeof(Model.Sys_Role_Menu_HallInfo))]
    [ServiceKnownType(typeof(Model.Sys_Role_Menu_ProInfo))]
    [ServiceKnownType(typeof(Model.Sys_Role_MenuInfo))]
    [ServiceKnownType(typeof(Model.Sys_RoleInfo))]
    [ServiceKnownType(typeof(Model.Sys_RoleInfo_back))]
    [ServiceKnownType(typeof(Model.Sys_RoleMethod))]
    [ServiceKnownType(typeof(Model.Sys_SalaryList))]
    [ServiceKnownType(typeof(Model.Sys_UserInfo))]
    [ServiceKnownType(typeof(Model.Sys_UserOp))]
    [ServiceKnownType(typeof(Model.Sys_UserOPList))]
    [ServiceKnownType(typeof(Model.Sys_UserRemindList))]
    [ServiceKnownType(typeof(Model.VIP_CardChange))]
    [ServiceKnownType(typeof(Model.VIP_HallOffInfo))]
    [ServiceKnownType(typeof(Model.VIP_IDCardType))]
    [ServiceKnownType(typeof(Model.VIP_OffList))]
    [ServiceKnownType(typeof(Model.VIP_OffTicket))]
    [ServiceKnownType(typeof(Model.VIP_ProOffList))]
    [ServiceKnownType(typeof(Model.VIP_Renew))]
    [ServiceKnownType(typeof(Model.VIP_RenewBack))]
    [ServiceKnownType(typeof(Model.VIP_RenewBackAduit))]
    [ServiceKnownType(typeof(Model.VIP_SendProList))]
    [ServiceKnownType(typeof(Model.VIP_SendProOffList))]
    [ServiceKnownType(typeof(Model.VIP_VIPBack))]
    [ServiceKnownType(typeof(Model.VIP_VIPBackAduit))]
    [ServiceKnownType(typeof(Model.VIP_VIPInfo))]
    [ServiceKnownType(typeof(Model.VIP_VIPInfo_BAK))]
    [ServiceKnownType(typeof(Model.VIP_VIPOffLIst))]
    [ServiceKnownType(typeof(Model.VIP_VIPService))]
    [ServiceKnownType(typeof(Model.VIP_VIPType))]
    [ServiceKnownType(typeof(Model.VIP_VIPType_Bak))]
    [ServiceKnownType(typeof(Model.VIP_VIPTypeOffLIst))]
    [ServiceKnownType(typeof(Model.VIP_VIPTypeService))]
    [ServiceKnownType(typeof(Model.VIP_VIPTypeService_BAK))]
    [ServiceKnownType(typeof(Model.GetBorowAduitInfoByAIDResult))]
    [ServiceKnownType(typeof(Model.GetBorowAduitListByPageResult))]
    [ServiceKnownType(typeof(Model.GetBorowListByIDResult))]
    [ServiceKnownType(typeof(Model.GetBorowListByUIDResult))]
    [ServiceKnownType(typeof(Model.GetRenewBackAduitListResult))]
    [ServiceKnownType(typeof(Model.GetRepairListByUIDResult))]
    [ServiceKnownType(typeof(Model.GetSellAduitListByPageResult))]
    [ServiceKnownType(typeof(Model.GetSellBackAduitListResult))]
    [ServiceKnownType(typeof(Model.GetVIPBackAduitListResult))]
    [ServiceKnownType(typeof(Model.Sys_UserInfo))]
    [ServiceKnownType(typeof(Model.Pro_SellAduitList))]
    [ServiceKnownType(typeof(Model.Pro_Sell_Yanbao))]
    [ServiceKnownType(typeof(Model.Pro_YanbaoPriceStepInfo))]
    [ServiceKnownType(typeof(Model.View_Pro_InOrder))]
    [ServiceKnownType(typeof(Model.View_Pro_RepairInfo))]
    [ServiceKnownType(typeof(Model.View_BorowAduit))]
    [ServiceKnownType(typeof(Model.ReportSqlParams_String))]
    [ServiceKnownType(typeof(Model.ReportSqlParams_Bool))]
    [ServiceKnownType(typeof(Model.ReportSqlParams_ListString))]
    [ServiceKnownType(typeof(Model.ReportSqlParams_DataTime))]
    [ServiceKnownType(typeof(Model.ReportPagingParam))]
    [ServiceKnownType(typeof(Model.SelecterIMEI))]
    [ServiceKnownType(typeof(Model.SeleterModel))]
    [ServiceKnownType(typeof(Model.Pro_OutModel))]
    [ServiceKnownType(typeof(Model.View_OutOrderList))]
    [ServiceKnownType(typeof(Model.View_OutIMEI))]
    [ServiceKnownType(typeof(Model.GetSAModelResult))]
    [ServiceKnownType(typeof(Model.View_Pro_RepairReturnInfo))]
    [ServiceKnownType(typeof(Model.View_Pro_RepairRetrunDetail))]
    [ServiceKnownType(typeof(Model.View_Pro_SellInfo))]
    [ServiceKnownType(typeof(Model.View_Pro_SellBackAduit))]
    [ServiceKnownType(typeof(Model.View_Pro_SellAduit))] 
    [ServiceKnownType(typeof(Model.View_VIP_Renew))] 
    [ServiceKnownType(typeof(Model.View_VIPTypeService))]
    [ServiceKnownType(typeof(Model.VIP_VIPInfo_Temp))]
    [ServiceKnownType(typeof(Model.Pro_SellListInfo_Temp))]
    [ServiceKnownType(typeof(Model.View_VIPInfo))]
    [ServiceKnownType(typeof(Model.View_VIPService))]
    [ServiceKnownType(typeof(Model.View_BorowInfo))]
    [ServiceKnownType(typeof(Model.View_ReturnInfo))]
    [ServiceKnownType(typeof(Model.View_VIPBackAduit))]
    [ServiceKnownType(typeof(Model.View_VIP_RenewBackAduit))]
    [ServiceKnownType(typeof(Model.CancelListModel))]
    [ServiceKnownType(typeof(Model.View_RepairInfo))]
    [ServiceKnownType(typeof(Model.TreeModel))]
    [ServiceKnownType(typeof(Model.View_Pro_ChangeList))]
    [ServiceKnownType(typeof(Model.View_Pro_CostChangeList))]
    [ServiceKnownType(typeof(Model.View_UserBorowInfo))]
    [ServiceKnownType(typeof(Model.View_BorowReturnInfo))]
    [ServiceKnownType(typeof(Model.View_BorowReturnDetail))]
    [ServiceKnownType(typeof(Model.View_YanBoPriceStepInfo))]
    [ServiceKnownType(typeof(Model.Pro_YanbaoPriceStepInfo))]
    [ServiceKnownType(typeof(Model.Pro_YanbaoPriceStepInfo_bak))]
    [ServiceKnownType(typeof(Model.View_SellTypeProduct))]
    [ServiceKnownType(typeof(Model.View_ProAllType))]
    [ServiceKnownType(typeof(Model.View_VIP_OffList))]
    [ServiceKnownType(typeof(Model.View_SellTypeProduct))]
    [ServiceKnownType(typeof(Model.View_SalaryList))]
    [ServiceKnownType(typeof(Model.PriceBill))]
    [ServiceKnownType(typeof(Model.PriceBillChild))]
    [ServiceKnownType(typeof(Model.CostBill))]
    [ServiceKnownType(typeof(Model.CostBillChild))]
    [ServiceKnownType(typeof(Model.Pro_Sell_Yanbao_temp))] 
    [ServiceKnownType(typeof(Model.SalaryBill))]
    [ServiceKnownType(typeof(Model.SalaryBillChild))]
    [ServiceKnownType(typeof(Model.Demo_ReportViewColumnInfo))]
    [ServiceKnownType(typeof(Model.Demo_ReportViewInfo))]
    [ServiceKnownType(typeof(Model.Report_Borow))]
    [ServiceKnownType(typeof(Model.Report_Return))]
    [ServiceKnownType(typeof(Model.Report_InOutSellInfo))]  
    [ServiceKnownType(typeof(Model.Report_Return))]
    [ServiceKnownType(typeof(Model.View_MySalary))]  
    [ServiceKnownType(typeof(Model.View_InOrderView))]
    [ServiceKnownType(typeof(Model.View_RepairReturnReceive))] 
    [ServiceKnownType(typeof(Model.View_OutSearch))]

    [ServiceKnownType(typeof(Model.Role_Pro_Property))]
    [ServiceKnownType(typeof(Model.ProModel))]
    [ServiceKnownType(typeof(Model.PropertyModel))]
    [ServiceKnownType(typeof(Model.PropertyValueModel))]
 

    [ServiceKnownType(typeof(Model.Proc_SalaryReportResult))]
    [ServiceKnownType(typeof(Model.Proc_SalaryReportDetailResult))]
    [ServiceKnownType(typeof(Model.ProClassModel))]
    [ServiceKnownType(typeof(Model.Pro_ChangeProInfo))]
    [ServiceKnownType(typeof(Model.View_PriceBillReport))]
    [ServiceKnownType(typeof(Model.View_CostBillReport))]
    [ServiceKnownType(typeof(Model.View_SalaryPlanReport))]
    [ServiceKnownType(typeof(Model.View_LowPriceList))]

    [ServiceKnownType(typeof(Model.LowPriceModel))]
    [ServiceKnownType(typeof(Model.LPMChildren))]
    [ServiceKnownType(typeof(Model.View_HallInfo))]
    [ServiceKnownType(typeof(Model.View_ProSellBackAduitDetail))]
    [ServiceKnownType(typeof(Model.Role_Hall))]
    [ServiceKnownType(typeof(Model.Pro_ProMainInfo))]
    [ServiceKnownType(typeof(Model.View_ProMainInfo))] 
    [ServiceKnownType(typeof(Model.View_SellBackOffList))]
    [ServiceKnownType(typeof(Model.View_ProInfo))]
    [ServiceKnownType(typeof(Model.View_RepaireRetList))] 
    [ServiceKnownType(typeof(Model.Print_SellListInfo))]
    [ServiceKnownType(typeof(Model.Print_SellBackListInfo))]
    [ServiceKnownType(typeof(Model.View_OffList))]
    [ServiceKnownType(typeof(Model.Pro_Sell_JiPeiKa))]
    [ServiceKnownType(typeof(Model.Pro_Sell_JiPeiKa_temp))]
    [ServiceKnownType(typeof(Model.View_ProOffList))]
    [ServiceKnownType(typeof(Model.OffModel))]
    [ServiceKnownType(typeof(Model.ProModel))]
    [ServiceKnownType(typeof(Model.HallModel))]
    [ServiceKnownType(typeof(Model.VIPModel))]
    [ServiceKnownType(typeof(Model.VIPTypeModel))]
    [ServiceKnownType(typeof(Model.Pro_AirOutInfo))]
    [ServiceKnownType(typeof(Model.Pro_AirOutListInfo))]
    [ServiceKnownType(typeof(Model.View_AirOutInfo))]
    [ServiceKnownType(typeof(Model.View_AirOutListModel))]
    [ServiceKnownType(typeof(Model.View_SellOffAduitInfo))]
    [ServiceKnownType(typeof(Model.View_SellOffAduitProList))]
    [ServiceKnownType(typeof(Model.View_SellOffAduitInfoList))]
    [ServiceKnownType(typeof(Model.Off_AduitTypeInfo))]
    [ServiceKnownType(typeof(Model.View_Off_AduitTypeInfo))]
    [ServiceKnownType(typeof(Model.Pro_SellOffAduitInfoList))]
    [ServiceKnownType(typeof(Model.Pro_SellOffAduitInfo))]
    [ServiceKnownType(typeof(Model.View_SellOffAduitInfo2))]
    [ServiceKnownType(typeof(Model.BAduitModel))]
    [ServiceKnownType(typeof(Model.Pro_ProNameInfo))]
    [ServiceKnownType(typeof(Model.View_PackageSalesNameInfo))]
    [ServiceKnownType(typeof(Model.View_ProNameInfo))]
    [ServiceKnownType(typeof(Model.Package_SalesNameInfo))]
    [ServiceKnownType(typeof(Model.View_RemindList))]
    [ServiceKnownType(typeof(Model.Sys_RemindList))]
    [ServiceKnownType(typeof(Model.GetUserRemindListResult))]
    [ServiceKnownType(typeof(Model.View_BorowAduit2))]
    [ServiceKnownType(typeof(Model.View_BorowAduit3))]
    [ServiceKnownType(typeof(Model.Pro_SellListInfo_SingleSell))]
    [ServiceKnownType(typeof(Model.View_UserRemindList))]
    [ServiceKnownType(typeof(Model.Pro_SellListInfo_AirSell))]
    [ServiceKnownType(typeof(Model.Pro_SellListInfo_MemberSell))]
    [ServiceKnownType(typeof(Model.Pro_SellListInfo_PhoneNumSell))]
    [ServiceKnownType(typeof(Model.Pro_SellListInfo_TicketSell))]
    [ServiceKnownType(typeof(Model.Pro_SellListInfo_YanBaoSell))]
    [ServiceKnownType(typeof(Model.View_PackageGroupTypeInfo))]
    [ServiceKnownType(typeof(Model.Package_GroupTypeInfo))]
    [ServiceKnownType(typeof(Model.GetUserAllRemindListResult))]
    [ServiceKnownType(typeof(Model.OutImportModel))]
       [ServiceKnownType(typeof(Model.OutImportModelList))]
       [ServiceKnownType(typeof(Model.Sys_SalaryChange))]
    [ServiceKnownType(typeof(Model.Rules_TypeInfo))]
    [ServiceKnownType(typeof(Model.Rules_SellTypeInfo))]
    [ServiceKnownType(typeof(Model.Rules_ProMainInfo))]
    [ServiceKnownType(typeof(Model.Rules_Pro_RulesTypeInfo))]

    [ServiceKnownType(typeof(Model.Rules_OffList))]
    [ServiceKnownType(typeof(Model.Rules_HallOffInfo))]
    [ServiceKnownType(typeof(Model.Rules_AllCurrentRulesInfo))]
    [ServiceKnownType(typeof(Model.RulesProMain))]
    [ServiceKnownType(typeof(Model.RuleOffModel))]
    [ServiceKnownType(typeof(Model.View_Rules_OffList))]
    [ServiceKnownType(typeof(Model.View_BorowAduit4))]
    [ServiceKnownType(typeof(Model.Rules_ImportModel))]
    [ServiceKnownType(typeof(Model.SalaryImportModel))]
    [ServiceKnownType(typeof(Model.View_OffListAduit))]
    [ServiceKnownType(typeof(Model.VIP_OffListAduit))]
    [ServiceKnownType(typeof(Model.Sys_SalaryCurrentList))]
    [ServiceKnownType(typeof(Model.View_CurrentSalary))]
    [ServiceKnownType(typeof(Model.VIPOffAduitHeader))]
    [ServiceKnownType(typeof(Model.VIP_HallInfoHeader  ))]
    [ServiceKnownType(typeof(Model.VIP_OffListAduitHeader))]
    [ServiceKnownType(typeof(Model.View_VIPOffListAduitHeader))]
    [ServiceKnownType(typeof(Model.View_VIPOffListAduit))]
    [ServiceKnownType(typeof(Model.View_Package_GroupInfo))]
    [ServiceKnownType(typeof(Model.View_Package_ProInfo))]
    [ServiceKnownType(typeof(Model.Pro_BigAreaInfo))]
    [ServiceKnownType(typeof(Model.View_SMS_SignInfo))]
       [ServiceKnownType(typeof(Model.View_SMS_SignSendPayInfo))]
       [ServiceKnownType(typeof(Model.View_SMS_SellBackAduit))]
       [ServiceKnownType(typeof(Model.SMS_SignInfo))]
       [ServiceKnownType(typeof(Model.SMS_SignSendPayInfo))]
       [ServiceKnownType(typeof(Model.SMS_SignSendPayInfo_Delete))]

   [ServiceKnownType(typeof(Model.ASP_CurrentOrder_BackupPhoneInfo))]
   [ServiceKnownType(typeof(Model.ASP_CurrentOrder_ErrorInfo))]
   [ServiceKnownType(typeof(Model.ASP_CurrentOrderInfo))]
   [ServiceKnownType(typeof(Model.ASP_ErrorInfo))]
    [ServiceKnownType(typeof(Model.ASP_FethchInfo))]
    [ServiceKnownType(typeof(Model.ASP_ReceiveInfo))]
    [ServiceKnownType(typeof(Model.ASP_RepairInfo))]
    [ServiceKnownType(typeof(Model.ASP_StepInfo))]
    [ServiceKnownType(typeof(Model.ASP_CheckInfo))]
    [ServiceKnownType(typeof(Model.ASP_CallBackInfo))]
    [ServiceKnownType(typeof(Model.ASP_BrokenInfo))]
    [ServiceKnownType(typeof(Model.ASP_AduitInfo))]
    [ServiceKnownType(typeof(Model.View_BJModels))]
    [ServiceKnownType(typeof(Model.Asset_UseInfo))]
    [ServiceKnownType(typeof(Model.View_ASPReceiveInfo))]
    [ServiceKnownType(typeof(Model.ASP_CurrentOrder_Pros))]
    [ServiceKnownType(typeof(Model.View_ASPCurrentOrderPros))]
    [ServiceKnownType(typeof(Model.View_ASPCurrentOrderInfo))]
  
    [ServiceKnownType(typeof(Model.View_ASPRepairInfo))]
    [ServiceKnownType(typeof(Model.BJModel))]
    [ServiceKnownType(typeof(Model.CHKModel))]
    [ServiceKnownType(typeof(Model.Sys_SalaryWithPercent))]
    [ServiceKnownType(typeof(Model.View_SalaryWithPercent))]
    
    [ServiceKnownType(typeof(Model.Pro_BillConflictInfo))]
    [ServiceKnownType(typeof(Model.Pro_BillFieldInfo))]
    [ServiceKnownType(typeof(Model.Pro_BillInfo))]
    [ServiceKnownType(typeof(Model.Pro_BillInfo_temp   ))]
    [ServiceKnownType(typeof(Model.Sys_SalaryBillInfo))]
    [ServiceKnownType(typeof(Model.Sys_SalaryBillListInfo))]
    [ServiceKnownType(typeof(Model.ASP_ErrType))]
    [ServiceKnownType(typeof(Model.View_ASPErrInfo))]
    [ServiceKnownType(typeof(Model.ASP_ProOther))]

    [ServiceKnownType(typeof(Model.Sys_SalaryPriceStep))]
    [ServiceKnownType(typeof(Model.View_ASPZJInfo))]
    [ServiceKnownType(typeof(Model.View_ASPSHInfo))]
    [ServiceKnownType(typeof(Model.View_ASPGetPhoneInfo))]   //
    [ServiceKnownType(typeof(Model.Sys_SalaryList_StepInfo))]
    #endregion
    #region List Model序列化定义
    [ServiceKnownType(typeof(List<Model.Report_IMEIInfo>))]
    [ServiceKnownType(typeof(List<Model.GetInOutSellInfoResult>))]
    [ServiceKnownType(typeof(List<string>))]
    [ServiceKnownType(typeof(List<Model.AduitListInfo>))]
    [ServiceKnownType(typeof(List<Model.AduitModel>))]
    [ServiceKnownType(typeof(List<Model.BorowListModel>))]
    [ServiceKnownType(typeof(List<Model.BorowModel>))]
    [ServiceKnownType(typeof(List<Model.IMEIModel>))]
    [ServiceKnownType(typeof(List<Model.MenuInfo>))]
    [ServiceKnownType(typeof(List<Model.NoIMEIModel>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellInfo_Child>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo_Child>))]
    [ServiceKnownType(typeof(List<Model.RepairListModel>))]
    [ServiceKnownType(typeof(List<Model.RenewModel>))]
    [ServiceKnownType(typeof(List<Model.RepairModel>))]
    [ServiceKnownType(typeof(List<Model.SetSelection>))]
    [ServiceKnownType(typeof(List<Model.Sys_InitDataStatus_Child>))]
    [ServiceKnownType(typeof(List<Model.Pro_AreaInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_BackInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_BackListInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_BackOrderIMEI>))]
    [ServiceKnownType(typeof(List<Model.Pro_BorowAduit>))]
    [ServiceKnownType(typeof(List<Model.Pro_BorowAduitList>))]
    [ServiceKnownType(typeof(List<Model.Pro_BorowInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_BorowListInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_BorowOrderIMEI>))]
    [ServiceKnownType(typeof(List<Model.Pro_CashTicket>))]
    [ServiceKnownType(typeof(List<Model.Pro_PriceChangeList>))]
    [ServiceKnownType(typeof(List<Model.Pro_ClassInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_HallInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_IMEI>))]
    [ServiceKnownType(typeof(List<Model.Pro_IMEI_Deleted>))]
    [ServiceKnownType(typeof(List<Model.Pro_InOrder>))]
    [ServiceKnownType(typeof(List<Model.Pro_InOrderIMEI>))]
    [ServiceKnownType(typeof(List<Model.Pro_InOrderList>))]
    [ServiceKnownType(typeof(List<Model.Pro_LevelInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_OutInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_OutModel>))]
    [ServiceKnownType(typeof(List<Model.Pro_OutOrderIMEI>))]
    [ServiceKnownType(typeof(List<Model.Pro_OutOrderList>))]
    [ServiceKnownType(typeof(List<Model.Pro_PriceChange>))]
    [ServiceKnownType(typeof(List<Model.Pro_PriceCostChange>))]
    [ServiceKnownType(typeof(List<Model.Pro_PriceCostChangeList>))]
    [ServiceKnownType(typeof(List<Model.Pro_ProInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_Property>))]
    [ServiceKnownType(typeof(List<Model.Pro_PropertyValue>))]
    [ServiceKnownType(typeof(List<Model.Pro_ProProperty_F>))]
    [ServiceKnownType(typeof(List<Model.Pro_RepairInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_RepairListInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_RepairReturnInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_RepairReturnListInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_ReturnInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_ReturnListInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_ReturnOrderIMEI>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellAduit>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellAduitList>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellBack>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellBackAduit>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellBackIMEIList>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellBackList>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellBackSpecalOffList>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellIMEIList>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellListServiceInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellSendInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellSpecalOffList>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellType>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellTypeProduct>))]
    [ServiceKnownType(typeof(List<Model.Pro_StoreInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_TypeInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_DeptInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_InitDataStatus>))]
    [ServiceKnownType(typeof(List<Model.Sys_LoginInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_MenuInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_MethodInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_Option>))]
    [ServiceKnownType(typeof(List<Model.Sys_OrderMakerInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_RemindList>))]
    [ServiceKnownType(typeof(List<Model.Sys_Role_HallInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_Role_Menu_HallInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_Role_Menu_ProInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_Role_MenuInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_RoleInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_RoleInfo_back>))]
    [ServiceKnownType(typeof(List<Model.Sys_RoleMethod>))]
    [ServiceKnownType(typeof(List<Model.Sys_SalaryList>))]
    [ServiceKnownType(typeof(List<Model.Sys_UserInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_UserOp>))]
    [ServiceKnownType(typeof(List<Model.Sys_UserOPList>))]
    [ServiceKnownType(typeof(List<Model.Sys_UserRemindList>))]
    [ServiceKnownType(typeof(List<Model.VIP_CardChange>))]
    [ServiceKnownType(typeof(List<Model.VIP_HallOffInfo>))]
    [ServiceKnownType(typeof(List<Model.VIP_IDCardType>))]
    [ServiceKnownType(typeof(List<Model.VIP_OffList>))]
    [ServiceKnownType(typeof(List<Model.VIP_OffTicket>))]
    [ServiceKnownType(typeof(List<Model.VIP_ProOffList>))]
    [ServiceKnownType(typeof(List<Model.VIP_Renew>))]
    [ServiceKnownType(typeof(List<Model.VIP_RenewBack>))]
    [ServiceKnownType(typeof(List<Model.VIP_RenewBackAduit>))]
    [ServiceKnownType(typeof(List<Model.VIP_SendProList>))]
    [ServiceKnownType(typeof(List<Model.VIP_SendProOffList>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPBack>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPBackAduit>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPInfo>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPInfo_BAK>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPOffLIst>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPService>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPType>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPType_Bak>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPTypeOffLIst>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPTypeService>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPTypeService_BAK>))]
    [ServiceKnownType(typeof(List<Model.GetBorowAduitInfoByAIDResult>))]
    [ServiceKnownType(typeof(List<Model.GetBorowAduitListByPageResult>))]
    [ServiceKnownType(typeof(List<Model.GetBorowListByIDResult>))]
    [ServiceKnownType(typeof(List<Model.GetBorowListByUIDResult>))]
    [ServiceKnownType(typeof(List<Model.GetRenewBackAduitListResult>))]
    [ServiceKnownType(typeof(List<Model.GetRepairListByUIDResult>))]
    [ServiceKnownType(typeof(List<Model.GetSellAduitListByPageResult>))]
    [ServiceKnownType(typeof(List<Model.GetSellBackAduitListResult>))]
    [ServiceKnownType(typeof(List<Model.GetVIPBackAduitListResult>))]
    [ServiceKnownType(typeof(List<Model.Sys_UserInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellAduitList>))]
    [ServiceKnownType(typeof(List<Model.Pro_Sell_Yanbao>))]
    [ServiceKnownType(typeof(List<Model.Pro_YanbaoPriceStepInfo>))]
    [ServiceKnownType(typeof(List<Model.View_Pro_InOrder>))]
    [ServiceKnownType(typeof(List<Model.View_Pro_RepairInfo>))]
    [ServiceKnownType(typeof(List<Model.View_BorowAduit>))]
    [ServiceKnownType(typeof(List<Model.ReportSqlParams_DataTime>))]
    [ServiceKnownType(typeof(List<Model.ReportSqlParams_ListString>))]
    [ServiceKnownType(typeof(List<Model.ReportSqlParams_String>))]
    [ServiceKnownType(typeof(List<Model.ReportSqlParams_Bool>))]
    [ServiceKnownType(typeof(List<Model.ReportPagingParam>))]
    [ServiceKnownType(typeof(List<Model.SelecterIMEI>))]
    [ServiceKnownType(typeof(List<Model.SeleterModel>))]
    [ServiceKnownType(typeof(List<Model.Pro_OutModel>))]
    [ServiceKnownType(typeof(List<Model.View_OutOrderList>))]
    [ServiceKnownType(typeof(List<Model.View_OutIMEI>))]
    [ServiceKnownType(typeof(List<Model.GetSAModelResult>))]
    [ServiceKnownType(typeof(List<Model.View_Pro_RepairReturnInfo>))]
    [ServiceKnownType(typeof(List<Model.View_Pro_RepairRetrunDetail>))]
    [ServiceKnownType(typeof(List<Model.View_Pro_SellInfo>))]
    [ServiceKnownType(typeof(List<Model.View_Pro_SellBackAduit>))]
    [ServiceKnownType(typeof(List<Model.View_Pro_SellAduit>))] 
    [ServiceKnownType(typeof(List<Model.View_VIP_Renew>))]
    [ServiceKnownType(typeof(List<Model.View_VIPBackApply>))]
    [ServiceKnownType(typeof(List<Model.View_BorowInfo>))]
    [ServiceKnownType(typeof(List<Model.View_VIPBackAduit>))]
    [ServiceKnownType(typeof(List<Model.View_VIPTypeService>))]
    [ServiceKnownType(typeof(List<Model.VIP_VIPInfo_Temp>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo_Temp>))]
    [ServiceKnownType(typeof(List<Model.View_VIPInfo>))]
    [ServiceKnownType(typeof(List<Model.View_ReturnInfo>))]
    [ServiceKnownType(typeof(List<Model.View_VIPService>))]
    [ServiceKnownType(typeof(List<Model.View_VIP_RenewBackAduit>))] 
    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo_Temp>))]
    [ServiceKnownType(typeof(List<Model.CancelListModel>))]
    [ServiceKnownType(typeof(List<Model.View_RepairInfo>))]
    [ServiceKnownType(typeof(List<int>))]
    [ServiceKnownType(typeof(List<Model.TreeModel>))]
    [ServiceKnownType(typeof(List<Model.View_Pro_ChangeList>))]
    [ServiceKnownType(typeof(List<Model.View_Pro_CostChangeList>))]
    [ServiceKnownType(typeof(List<Model.View_UserBorowInfo>))]
    [ServiceKnownType(typeof(List<Model.View_BorowReturnInfo>))]
    [ServiceKnownType(typeof(List<Model.View_BorowReturnDetail>))]
    [ServiceKnownType(typeof(List<Model.View_YanBoPriceStepInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_YanbaoPriceStepInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_YanbaoPriceStepInfo_bak>))]
    [ServiceKnownType(typeof(List<Model.View_VIP_OffList>))]  
      [ServiceKnownType(typeof(List<Model.View_SellTypeProduct>))]
    [ServiceKnownType(typeof(List<Model.View_Sys_UserInfo>))] 
      [ServiceKnownType(typeof(List<Model.View_ProAllType>))] 
    [ServiceKnownType(typeof(List<Model.View_SalaryList>))]
    [ServiceKnownType(typeof(List<Model.PriceBill>))]
    [ServiceKnownType(typeof(List<Model.PriceBillChild>))]
    [ServiceKnownType(typeof(List<Model.CostBill>))]
    [ServiceKnownType(typeof(List<Model.CostBillChild>))]
    [ServiceKnownType(typeof(List<Model.Pro_Sell_Yanbao_temp>))] 
    [ServiceKnownType(typeof(List<Model.SalaryBill>))]
    [ServiceKnownType(typeof(List<Model.SalaryBillChild>))]
    [ServiceKnownType(typeof(List<Model.Demo_ReportViewColumnInfo>))]
    [ServiceKnownType(typeof(List<Model.Demo_ReportViewInfo>))]
    [ServiceKnownType(typeof(List<Model.Report_Borow>))]
    [ServiceKnownType(typeof(List<Model.Report_Return>))]
    [ServiceKnownType(typeof(List<Model.View_MySalary>))]
    [ServiceKnownType(typeof(List<Model.View_InOrderView>))] 
    [ServiceKnownType(typeof(List<Model.Report_InOutSellInfo>))]
    [ServiceKnownType(typeof(List<Model.View_RepairReturnReceive>))] 
    [ServiceKnownType(typeof(List<Model.View_OutSearch>))]


    [ServiceKnownType(typeof(List<Model.Role_Pro_Property>))]
    [ServiceKnownType(typeof(List<Model.ProModel>))]
    [ServiceKnownType(typeof(List<Model.PropertyModel>))]
    [ServiceKnownType(typeof(List<Model.PropertyValueModel>))]
    [ServiceKnownType(typeof(List<Model.Proc_SalaryReportResult>))]
    [ServiceKnownType(typeof(List<Model.Proc_SalaryReportDetailResult>))]
    [ServiceKnownType(typeof(List<Model.ProClassModel>))]
    [ServiceKnownType(typeof(List<Model.Pro_ChangeProInfo>))]
    [ServiceKnownType(typeof(List<Model.View_PriceBillReport>))]
    [ServiceKnownType(typeof(List<Model.View_CostBillReport>))]
    [ServiceKnownType(typeof(List<Model.View_SalaryPlanReport>))]
    [ServiceKnownType(typeof(List<Model.View_LowPriceList>))]
    [ServiceKnownType(typeof(List<Model.LowPriceModel>))]
    [ServiceKnownType(typeof(List<Model.LPMChildren>))]
    [ServiceKnownType(typeof(List<Model.View_HallInfo>))]
    [ServiceKnownType(typeof(List<Model.View_ProSellBackAduitDetail>))]
    [ServiceKnownType(typeof(List<Model.Role_Hall>))]
    [ServiceKnownType(typeof(List<Model.Pro_ProMainInfo>))] 
    [ServiceKnownType(typeof(List<Model.View_SellBackOffList>))]
    [ServiceKnownType(typeof(List<Model.View_ProMainInfo>))]
    [ServiceKnownType(typeof(List<Model.View_ProInfo>))]
    [ServiceKnownType(typeof(List<Model.View_RepaireRetList>))]
    [ServiceKnownType(typeof(List<Model.Print_SellListInfo>))]
    [ServiceKnownType(typeof(List<Model.Print_SellBackListInfo>))]
    [ServiceKnownType(typeof(List<Model.View_OffList>))]
    [ServiceKnownType(typeof(List<Model.View_ProOffList>))]
    [ServiceKnownType(typeof(List<Model.OffModel>))]
    [ServiceKnownType(typeof(List<Model.ProModel>))]
    [ServiceKnownType(typeof(List<Model.HallModel>))]
    [ServiceKnownType(typeof(List<Model.VIPModel>))]
    [ServiceKnownType(typeof(List<Model.VIPTypeModel>))]
    [ServiceKnownType(typeof(List<Model.Pro_AirOutInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_AirOutListInfo>))]
    [ServiceKnownType(typeof(List<Model.View_AirOutInfo>))]
    [ServiceKnownType(typeof(List<Model.View_AirOutListModel>))]
    [ServiceKnownType(typeof(List<Model.View_SellOffAduitInfo>))]
    [ServiceKnownType(typeof(List<Model.View_SellOffAduitProList>))]
    [ServiceKnownType(typeof(List<Model.View_SellOffAduitInfoList>))]
    [ServiceKnownType(typeof(List<Model.Off_AduitTypeInfo>))]
    [ServiceKnownType(typeof(List<Model.View_Off_AduitTypeInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellOffAduitInfoList>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellOffAduitInfo>))]
    [ServiceKnownType(typeof(List<Model.View_SellOffAduitInfo2>))]
    [ServiceKnownType(typeof(List<Model.BAduitModel>))]
    [ServiceKnownType(typeof(List<Model.Pro_ProNameInfo>))]
    [ServiceKnownType(typeof(List<Model.View_PackageSalesNameInfo>))]
    [ServiceKnownType(typeof(List<Model.View_ProNameInfo>))]
    [ServiceKnownType(typeof(List<Model.Package_SalesNameInfo>))]
    [ServiceKnownType(typeof(List<Model.View_RemindList>))]
    [ServiceKnownType(typeof(List<Model.Sys_RemindList>))]
    [ServiceKnownType(typeof(List<Model.GetUserRemindListResult>))]
    [ServiceKnownType(typeof(List<Model.View_BorowAduit2>))]
    [ServiceKnownType(typeof(List<Model.View_BorowAduit3>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo_SingleSell>))]
    [ServiceKnownType(typeof(List<Model.View_UserRemindList>))]

    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo_AirSell>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo_MemberSell>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo_PhoneNumSell>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo_TicketSell>))]
    [ServiceKnownType(typeof(List<Model.Pro_SellListInfo_YanBaoSell>))]
    [ServiceKnownType(typeof(List<Model.View_PackageGroupTypeInfo>))]
    [ServiceKnownType(typeof(List<Model.Package_GroupTypeInfo>))]
    [ServiceKnownType(typeof(List<Model.GetUserAllRemindListResult>))]
    [ServiceKnownType(typeof(List<Model.OutImportModel>))]
    [ServiceKnownType(typeof(List<Model.OutImportModelList>))]
    [ServiceKnownType(typeof(List<Model.Rules_TypeInfo>))]
    [ServiceKnownType(typeof(List<Model.Rules_SellTypeInfo>))]
    [ServiceKnownType(typeof(List<Model.Rules_ProMainInfo>))]
    [ServiceKnownType(typeof(List<Model.Rules_Pro_RulesTypeInfo>))]
    [ServiceKnownType(typeof(List<Model.Rules_OffList>))]
    [ServiceKnownType(typeof(List<Model.Rules_HallOffInfo>))]
    [ServiceKnownType(typeof(List<Model.Rules_AllCurrentRulesInfo>))]
    [ServiceKnownType(typeof(List<Model.RulesProMain>))]
    [ServiceKnownType(typeof(List<Model.RuleOffModel>))]
     [ServiceKnownType(typeof(List<Model.View_Rules_OffList>))]
     [ServiceKnownType(typeof(List<Model.View_BorowAduit4>))]
     [ServiceKnownType(typeof(List<Model.Rules_ImportModel>))]
     [ServiceKnownType(typeof(List<Model.SalaryImportModel>))]
     [ServiceKnownType(typeof(List<Model.View_OffListAduit>))]
     [ServiceKnownType(typeof(List<Model.VIP_OffListAduit>))]
     [ServiceKnownType(typeof(List<Model.Sys_SalaryCurrentList>))]
     [ServiceKnownType(typeof(List<Model.View_CurrentSalary>))]
     [ServiceKnownType(typeof(List<Model.Sys_SalaryChange>))]
    [ServiceKnownType(typeof(List<Model.VIPOffAduitHeader>))]
    [ServiceKnownType(typeof(List<Model.VIP_HallInfoHeader>))]
    [ServiceKnownType(typeof(List<Model.VIP_OffListAduitHeader>))]
    [ServiceKnownType(typeof(List<Model.View_VIPOffListAduitHeader>))]
    [ServiceKnownType(typeof(List<Model.View_VIPOffListAduit>))]
    [ServiceKnownType(typeof(List<Model.View_Package_GroupInfo>))]
    [ServiceKnownType(typeof(List<Model.View_Package_ProInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_BigAreaInfo>))]
    [ServiceKnownType(typeof(List<Model.View_SMS_SignInfo>))]
    [ServiceKnownType(typeof(List<Model.View_SMS_SignSendPayInfo>))]
    [ServiceKnownType(typeof(List<Model.View_SMS_SellBackAduit>))]
    [ServiceKnownType(typeof(List<Model.SMS_SignInfo>))]
    [ServiceKnownType(typeof(List<Model.SMS_SignSendPayInfo>))]
    [ServiceKnownType(typeof(List<Model.SMS_SignSendPayInfo_Delete>))]

    [ServiceKnownType(typeof(List<Model.ASP_CurrentOrder_BackupPhoneInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_CurrentOrder_ErrorInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_CurrentOrderInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_ErrorInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_FethchInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_ReceiveInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_RepairInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_StepInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_CheckInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_CallBackInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_BrokenInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_AduitInfo>))]
    [ServiceKnownType(typeof(List<Model.View_BJModels>))]
    [ServiceKnownType(typeof(List<Model.View_ASPReceiveInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_CurrentOrder_Pros>))]
    [ServiceKnownType(typeof(List<Model.View_ASPCurrentOrderPros>))]
    [ServiceKnownType(typeof(List<Model.View_ASPCurrentOrderInfo>))]
    [ServiceKnownType(typeof(List<Model.View_ASPCurrentOrderInfo>))]
    [ServiceKnownType(typeof(List<Model.Asset_UseInfo>))]
    [ServiceKnownType(typeof(List<Model.View_ASPRepairInfo>))]
    [ServiceKnownType(typeof(List<Model.BJModel>))]
    [ServiceKnownType(typeof(List<Model.CHKModel>))]
    [ServiceKnownType(typeof(List<Model.Pro_BillConflictInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_BillFieldInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_SalaryWithPercent>))]
    [ServiceKnownType(typeof(List<Model.View_SalaryWithPercent>))]
    [ServiceKnownType(typeof(List<Model.Pro_BillInfo>))]
    [ServiceKnownType(typeof(List<Model.Pro_BillInfo_temp>))]
    [ServiceKnownType(typeof(List<Model.Sys_SalaryBillInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_SalaryBillListInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_ErrType>))]
    [ServiceKnownType(typeof(List<Model.View_ASPErrInfo>))]
    [ServiceKnownType(typeof(List<Model.ASP_ProOther>))]
    [ServiceKnownType(typeof(List<Model.View_ASPZJInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_SalaryPriceStep>))]
    [ServiceKnownType(typeof(List<Model.View_ASPSHInfo>))]
    [ServiceKnownType(typeof(List<Model.View_ASPGetPhoneInfo>))]
    [ServiceKnownType(typeof(List<Model.Sys_SalaryList_StepInfo>))]
    #endregion
    public class UserMsService
    {
        public WebReturn webR;
        public Sys_UserInfo LoginSysUserInfo;

        public static void Test()
        {
            return;
        }

        [OperationContract]
        public WebReturn Main(int MethodId, object[] args)
        {
           
            //全局验证
            //webR = MainCheck(MethodId);         
            //if (!webR.ReturnValue)
            //{
            //    return webR;
            //}


//            调试用
            if (ConfigurationManager.AppSettings["AdminDebugMode"] == "1")
            {
    if (!IsLogin())
                {
                    Login("1", "1","",DateTime.Now);
                }
            }
            else
            {
                if (!IsLogin())
                {
                    return new WebReturn() { ReturnValue = false, Message = "尚未登录" };
                }
            }

//            调试用END
            
            var webreturn= Common.MainCheckHelp.MainRequest(MethodId, args, (Model.Sys_UserInfo)MySession["User"]);
            return webreturn;

        }

        [OperationContract]
        [WebMethod(EnableSession = true)] 
        public WebReturn Login(string username, string password, string sign,DateTime dt)
        {
            
            //初始化菜单

            //登陆
            //验证信息
            //Sys_roleMethod List 菜单 方法 仓库
            //菜单 xml
            //
            //InitdataState 需要初始化的信息
            //返回 userinfo InitdataState
            Model.Sys_RoleInfo menu = new Model.Sys_RoleInfo();
            webR = new WebReturn();
            var min = (dt - DateTime.Now).TotalMinutes;
            if (Math.Abs(min) > 15)
            {
                webR.Message = "电脑时间不正确, 请将电脑日期修改为正确的时间";
                webR.Obj = null;
                webR.ReturnValue = false;
                return webR;
            }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                webR.Message = "用户名或者密码不能为空";
                webR.Obj = null;
                webR.ReturnValue = false;
                return webR;
            }
            username = username.Trim();
            password = password.Trim();

            #region 解密时间戳
            if (!Common.MainCheckHelp.CheckTimeTrick(sign))
            {
                webR.Message = "签名错误";
                webR.Obj = null;
                webR.ReturnValue = false;
                return webR;
            }
            #endregion
           

            //验证用户信息
            Model.Sys_UserInfo User = new Model.Sys_UserInfo();
            User.UserName = username;
            User.UserPwd = password;
  
            DAL.Sys_LoginInfo login = new DAL.Sys_LoginInfo();
            webR = login.Add(User);
            if (webR.Obj!=null)
            MySession["User"]=  webR.Obj;
            LoginSysUserInfo = webR.Obj as Sys_UserInfo;
            return webR;
            
        }
     
        [OperationContract]
        [WebMethod(EnableSession = true)] 
        public WebReturn InitData(DateTime dt)
        {

            //是否登陆

            if (!IsLogin())
            {
                webR=new WebReturn();
                webR.Message = "用户尚未登录";
                webR.ReturnValue = false;
                return webR;
            }
            //解析需要初始化清单
            DAL.Sys_InitDataStatus dal=new DAL.Sys_InitDataStatus();

            return dal.GetList((Sys_UserInfo) MySession["User"], dt);
               
            
           

            //返回清单实体类

        }

        [OperationContract]
        [WebMethod(EnableSession = true)]

        public WebReturn UpdatePwd(string username, string password, string Newpassword, string sign)
        {
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                webR = new WebReturn();
                webR.Message = "原用户名或密码不能为空";
                webR.Obj = null;
                webR.ReturnValue = false;
                return webR;
            }
            username = username.Trim();
            password = password.Trim();

            #region 解密时间戳
            if (!Common.MainCheckHelp.CheckTimeTrick(sign))
            {
                webR = new WebReturn();
                webR.Message = "签名错误";
                webR.Obj = null;
                webR.ReturnValue = false;
                return webR;
            }
            #endregion

            DAL.Sys_UserInfo user = new DAL.Sys_UserInfo();
            webR= user.UpDatePwd(new Sys_UserInfo() { UserName = username, UserPwd = password }, Newpassword);


            return webR;
        }

        public WebReturn MainCheck(int MethodId)
        {

            //WebReturn r = new WebReturn();
            ////验证是否已经登录
            if (!IsLogin())
            {
                webR.Message = "用户尚未登录";
                webR.ReturnValue = false;
            }
            webR = RoleCheckHelp.MainCheck(this.LoginSysUserInfo, MethodId);
            return webR;
        }



        public bool IsLogin()
        {
            try
            {
                return MySession["User"] != null;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        [OperationContract]
        [WebMethod(EnableSession = true)] 

        public Model.WebReturn GetRole(Model.Sys_UserInfo role)
        {
            //DAL_.Pro_SellInfo s = new DAL_.Pro_SellInfo();
            //Model.Pro_SellInfo sell = new Model.Pro_SellInfo() { VIP_ID = 3, HallID = "1" };
            //sell.Pro_SellListInfo.Add(
            //    new Model.Pro_SellListInfo() { ProID = "2", ProCount = 2, SellType = 2 }
            //    );
            //sell.Pro_SellListInfo.Add(
            //    new Model.Pro_SellListInfo() { ProID = "2", ProCount = 1, SellType = 1 }
            //    );
            //sell.Pro_SellListInfo.Add(
            //    new Model.Pro_SellListInfo() { ProID = "3", ProCount = 1, SellType = 1 }
            //    );
            //Model.WebReturn r = s.GetSellOff(null, sell);
           
            //return r;
            return null;
        }
        [OperationContract]
        [WebMethod(EnableSession = true)] 

        public Model.Pro_IMEI_Deleted mmmm()
        {
            return new Model.Pro_IMEI_Deleted();
        }



        public IDictionary<string, object> MySession
        {
            get
            {

                return MyWcfContext.Current.Items;
            }

        }


        #region 查询报表主入口
        [OperationContract]
        [WebMethod(EnableSession = true)] 

        public WebReturn MainReport(int MethodId,string username,string password,string sign, object[] args)
        {
           
            //全局验证
            //webR = MainCheck(MethodId);         
            //if (!webR.ReturnValue)
            //{
            //    return webR;
            //}
            #region 是否登陆
            if (MySession["User"] == null)//未登录
            {
                webR= Login(username, password, sign,DateTime.Now);
                if (webR.ReturnValue !=true)
                {
                    return webR;
                }
            }
            #endregion
            return Common.MainCheckHelp.MainRequest(MethodId, args, (Model.Sys_UserInfo)MySession["User"]);
            
        }


        #endregion

        #region 报表信息查询
        [OperationContract]
        [WebMethod(EnableSession = true)] 

        public WebReturn MainReportViewInfo(string reportName)
        {

            //全局验证
            //webR = MainCheck(MethodId);         
            //if (!webR.ReturnValue)
            //{
            //    return webR;
            //}
            #region 是否登陆
            if (MySession["User"] == null)//未登录
            {
                webR = new WebReturn() {  ReturnValue=false, Message="未登录"};
                return webR;
            }
            #endregion


            return Common.MainCheckHelp.GetReportViewInfo(reportName);

        }


        #endregion

        #region 获取销售类别商品信息
        [OperationContract]
        [WebMethod(EnableSession = true)] 

        public List<Model.Pro_SellTypeProduct> GetSellTyPro()
        {
          
            DAL.Pro_SellTypeProduct SellTypePro = new DAL.Pro_SellTypeProduct();
            return SellTypePro.GetModel();
        }
        #endregion 


        [WebGet]
        public IQueryable<Model.Pro_ProInfo> TESTER()
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                return lqh.Umsdb.Pro_ProInfo;
            }
        }
    }
}

