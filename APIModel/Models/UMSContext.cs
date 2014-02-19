using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using APIModel.Models.Mapping;

namespace APIModel.Models
{
    public partial class UMSContext : DbContext
    {
        static UMSContext()
        {
            Database.SetInitializer<UMSContext>(null);
        }

        public UMSContext()
            : base("UMSContext")
        {
        }

        public DbSet<Demo_HallInfo> Demo_HallInfo { get; set; }
        public DbSet<Demo_MapReport> Demo_MapReport { get; set; }
        public DbSet<Demo_ProInfo> Demo_ProInfo { get; set; }
        public DbSet<Demo_ReportViewColumnInfo> Demo_ReportViewColumnInfo { get; set; }
        public DbSet<Demo_ReportViewInfo> Demo_ReportViewInfo { get; set; }
        public DbSet<Demo_各厅库存> Demo_各厅库存 { get; set; }
        public DbSet<Demo_归还记录> Demo_归还记录 { get; set; }
        public DbSet<Demo_借贷查询> Demo_借贷查询 { get; set; }
        public DbSet<Demo_进销存报表> Demo_进销存报表 { get; set; }
        public DbSet<Demo_提成报表> Demo_提成报表 { get; set; }
        public DbSet<Demo_退货查询> Demo_退货查询 { get; set; }
        public DbSet<Demo_销售汇总> Demo_销售汇总 { get; set; }
        public DbSet<Demo_销售明细> Demo_销售明细 { get; set; }
        public DbSet<Demo_周转率报表> Demo_周转率报表 { get; set; }
        public DbSet<Off_AduitHallInfo> Off_AduitHallInfo { get; set; }
        public DbSet<Off_AduitProInfo> Off_AduitProInfo { get; set; }
        public DbSet<Off_AduitTypeInfo> Off_AduitTypeInfo { get; set; }
        public DbSet<Package_GroupInfo> Package_GroupInfo { get; set; }
        public DbSet<Package_GroupTypeInfo> Package_GroupTypeInfo { get; set; }
        public DbSet<Package_ProInfo> Package_ProInfo { get; set; }
        public DbSet<Package_SalesNameInfo> Package_SalesNameInfo { get; set; }
        public DbSet<Pro_AirOutInfo> Pro_AirOutInfo { get; set; }
        public DbSet<Pro_AirOutListInfo> Pro_AirOutListInfo { get; set; }
        public DbSet<Pro_AreaInfo> Pro_AreaInfo { get; set; }
        public DbSet<Pro_BackInfo> Pro_BackInfo { get; set; }
        public DbSet<Pro_BackListInfo> Pro_BackListInfo { get; set; }
        public DbSet<Pro_BackOrderIMEI> Pro_BackOrderIMEI { get; set; }
        public DbSet<Pro_BigAreaInfo> Pro_BigAreaInfo { get; set; }
        public DbSet<Pro_BorowAduit> Pro_BorowAduit { get; set; }
        public DbSet<Pro_BorowAduit_bak> Pro_BorowAduit_bak { get; set; }
        public DbSet<Pro_BorowAduitList> Pro_BorowAduitList { get; set; }
        public DbSet<Pro_BorowInfo> Pro_BorowInfo { get; set; }
        public DbSet<Pro_BorowListInfo> Pro_BorowListInfo { get; set; }
        public DbSet<Pro_BorowOrderIMEI> Pro_BorowOrderIMEI { get; set; }
        public DbSet<Pro_CashTicket> Pro_CashTicket { get; set; }
        public DbSet<Pro_ChangeIMEIInfo> Pro_ChangeIMEIInfo { get; set; }
        public DbSet<Pro_ChangeProInfo> Pro_ChangeProInfo { get; set; }
        public DbSet<Pro_ChangeProListInfo> Pro_ChangeProListInfo { get; set; }
        public DbSet<Pro_ClassInfo> Pro_ClassInfo { get; set; }
        public DbSet<Pro_ClassType> Pro_ClassType { get; set; }
        public DbSet<Pro_HallInfo> Pro_HallInfo { get; set; }
        public DbSet<Pro_IMEI> Pro_IMEI { get; set; }
        public DbSet<Pro_IMEI_Deleted> Pro_IMEI_Deleted { get; set; }
        public DbSet<Pro_InOrder> Pro_InOrder { get; set; }
        public DbSet<Pro_InOrderIMEI> Pro_InOrderIMEI { get; set; }
        public DbSet<Pro_InOrderList> Pro_InOrderList { get; set; }
        public DbSet<Pro_LevelInfo> Pro_LevelInfo { get; set; }
        public DbSet<Pro_OutInfo> Pro_OutInfo { get; set; }
        public DbSet<Pro_OutOrderIMEI> Pro_OutOrderIMEI { get; set; }
        public DbSet<Pro_OutOrderList> Pro_OutOrderList { get; set; }
        public DbSet<Pro_PriceChange> Pro_PriceChange { get; set; }
        public DbSet<Pro_PriceChangeList> Pro_PriceChangeList { get; set; }
        public DbSet<Pro_PriceCost_InorderList> Pro_PriceCost_InorderList { get; set; }
        public DbSet<Pro_PriceCostChange> Pro_PriceCostChange { get; set; }
        public DbSet<Pro_PriceCostChangeList> Pro_PriceCostChangeList { get; set; }
        public DbSet<Pro_ProInfo> Pro_ProInfo { get; set; }
        public DbSet<Pro_ProMainInfo> Pro_ProMainInfo { get; set; }
        public DbSet<Pro_ProNameInfo> Pro_ProNameInfo { get; set; }
        public DbSet<Pro_Property> Pro_Property { get; set; }
        public DbSet<Pro_PropertyValue> Pro_PropertyValue { get; set; }
        public DbSet<Pro_ProProperty_F> Pro_ProProperty_F { get; set; }
        public DbSet<Pro_RepairInfo> Pro_RepairInfo { get; set; }
        public DbSet<Pro_RepairListInfo> Pro_RepairListInfo { get; set; }
        public DbSet<Pro_RepairReturnInfo> Pro_RepairReturnInfo { get; set; }
        public DbSet<Pro_RepairReturnInfo_BAK> Pro_RepairReturnInfo_BAK { get; set; }
        public DbSet<Pro_RepairReturnListInfo> Pro_RepairReturnListInfo { get; set; }
        public DbSet<Pro_ReturnInfo> Pro_ReturnInfo { get; set; }
        public DbSet<Pro_ReturnListInfo> Pro_ReturnListInfo { get; set; }
        public DbSet<Pro_ReturnOrderIMEI> Pro_ReturnOrderIMEI { get; set; }
        public DbSet<Pro_SalaryListOneDay> Pro_SalaryListOneDay { get; set; }
        public DbSet<Pro_Sell_Car> Pro_Sell_Car { get; set; }
        public DbSet<Pro_Sell_JiPeiKa> Pro_Sell_JiPeiKa { get; set; }
        public DbSet<Pro_Sell_JiPeiKa_temp> Pro_Sell_JiPeiKa_temp { get; set; }
        public DbSet<Pro_Sell_Service> Pro_Sell_Service { get; set; }
        public DbSet<Pro_Sell_Yanbao> Pro_Sell_Yanbao { get; set; }
        public DbSet<Pro_Sell_Yanbao_temp> Pro_Sell_Yanbao_temp { get; set; }
        public DbSet<Pro_SellAduit> Pro_SellAduit { get; set; }
        public DbSet<Pro_SellAduit_bak> Pro_SellAduit_bak { get; set; }
        public DbSet<Pro_SellAduitList> Pro_SellAduitList { get; set; }
        public DbSet<Pro_SellBack> Pro_SellBack { get; set; }
        public DbSet<Pro_SellBackAduit> Pro_SellBackAduit { get; set; }
        public DbSet<Pro_SellBackAduit_bak> Pro_SellBackAduit_bak { get; set; }
        public DbSet<Pro_SellBackAduitList> Pro_SellBackAduitList { get; set; }
        public DbSet<Pro_SellBackAduitOffList> Pro_SellBackAduitOffList { get; set; }
        public DbSet<Pro_SellBackIMEIList> Pro_SellBackIMEIList { get; set; }
        public DbSet<Pro_SellBackInfo_Aduit> Pro_SellBackInfo_Aduit { get; set; }
        public DbSet<Pro_SellBackList> Pro_SellBackList { get; set; }
        public DbSet<Pro_SellBackSpecalOffList> Pro_SellBackSpecalOffList { get; set; }
        public DbSet<Pro_SellIMEIList> Pro_SellIMEIList { get; set; }
        public DbSet<Pro_SellInfo> Pro_SellInfo { get; set; }
        public DbSet<Pro_SellInfo_Aduit> Pro_SellInfo_Aduit { get; set; }
        public DbSet<Pro_SellList_RulesInfo> Pro_SellList_RulesInfo { get; set; }
        public DbSet<Pro_SellListInfo> Pro_SellListInfo { get; set; }
        public DbSet<Pro_SellListInfo_Temp> Pro_SellListInfo_Temp { get; set; }
        public DbSet<Pro_SellListServiceInfo> Pro_SellListServiceInfo { get; set; }
        public DbSet<Pro_SellOffAduitInfo> Pro_SellOffAduitInfo { get; set; }
        public DbSet<Pro_SellOffAduitInfoList> Pro_SellOffAduitInfoList { get; set; }
        public DbSet<Pro_SellSendInfo> Pro_SellSendInfo { get; set; }
        public DbSet<Pro_SellSpecalOffList> Pro_SellSpecalOffList { get; set; }
        public DbSet<Pro_SellType> Pro_SellType { get; set; }
        public DbSet<Pro_SellTypeProduct> Pro_SellTypeProduct { get; set; }
        public DbSet<Pro_SellTypeProduct_bak> Pro_SellTypeProduct_bak { get; set; }
        public DbSet<Pro_StoreInfo> Pro_StoreInfo { get; set; }
        public DbSet<Pro_TypeInfo> Pro_TypeInfo { get; set; }
        public DbSet<Pro_YanbaoPriceStepInfo> Pro_YanbaoPriceStepInfo { get; set; }
        public DbSet<Pro_YanbaoPriceStepInfo_bak> Pro_YanbaoPriceStepInfo_bak { get; set; }
        public DbSet<Rules_AllCurrentRulesInfo> Rules_AllCurrentRulesInfo { get; set; }
        public DbSet<Rules_HallOffInfo> Rules_HallOffInfo { get; set; }
        public DbSet<Rules_OffList> Rules_OffList { get; set; }
        public DbSet<Rules_Pro_RulesTypeInfo> Rules_Pro_RulesTypeInfo { get; set; }
        public DbSet<Rules_ProMainInfo> Rules_ProMainInfo { get; set; }
        public DbSet<Rules_SellTypeInfo> Rules_SellTypeInfo { get; set; }
        public DbSet<Rules_TypeInfo> Rules_TypeInfo { get; set; }
        public DbSet<SMS_SellBack> SMS_SellBack { get; set; }
        public DbSet<SMS_SellBackAduit> SMS_SellBackAduit { get; set; }
        public DbSet<SMS_SignInfo> SMS_SignInfo { get; set; }
        public DbSet<SMS_SignPayInListInfo> SMS_SignPayInListInfo { get; set; }
        public DbSet<SMS_SignSendPayInfo> SMS_SignSendPayInfo { get; set; }
        public DbSet<SMS_SignSendPayInfo_Delete> SMS_SignSendPayInfo_Delete { get; set; }
        public DbSet<Sys_DeptInfo> Sys_DeptInfo { get; set; }
        public DbSet<Sys_InitDataStatus> Sys_InitDataStatus { get; set; }
        public DbSet<Sys_LoginInfo> Sys_LoginInfo { get; set; }
        public DbSet<Sys_MenuInfo> Sys_MenuInfo { get; set; }
        public DbSet<Sys_MethodInfo> Sys_MethodInfo { get; set; }
        public DbSet<Sys_Option> Sys_Option { get; set; }
        public DbSet<Sys_OrderMakerInfo> Sys_OrderMakerInfo { get; set; }
        public DbSet<Sys_RemindList> Sys_RemindList { get; set; }
        public DbSet<Sys_Role_HallInfo> Sys_Role_HallInfo { get; set; }
        public DbSet<Sys_Role_Menu_HallInfo> Sys_Role_Menu_HallInfo { get; set; }
        public DbSet<Sys_Role_Menu_HallInfo_bak> Sys_Role_Menu_HallInfo_bak { get; set; }
        public DbSet<Sys_Role_Menu_ProInfo> Sys_Role_Menu_ProInfo { get; set; }
        public DbSet<Sys_Role_Menu_ProInfo_bak> Sys_Role_Menu_ProInfo_bak { get; set; }
        public DbSet<Sys_Role_MenuInfo> Sys_Role_MenuInfo { get; set; }
        public DbSet<Sys_Role_MenuInfo_bak> Sys_Role_MenuInfo_bak { get; set; }
        public DbSet<Sys_RoleInfo> Sys_RoleInfo { get; set; }
        public DbSet<Sys_RoleInfo_back> Sys_RoleInfo_back { get; set; }
        public DbSet<Sys_RoleMethod> Sys_RoleMethod { get; set; }
        public DbSet<Sys_SalaryChange> Sys_SalaryChange { get; set; }
        public DbSet<Sys_SalaryCurrentDelete> Sys_SalaryCurrentDelete { get; set; }
        public DbSet<Sys_SalaryCurrentList> Sys_SalaryCurrentList { get; set; }
        public DbSet<Sys_SalaryCurrentListDeleteList> Sys_SalaryCurrentListDeleteList { get; set; }
        public DbSet<Sys_SalaryList> Sys_SalaryList { get; set; }
        public DbSet<Sys_SalaryList_bak> Sys_SalaryList_bak { get; set; }
        public DbSet<Sys_SalaryTemp> Sys_SalaryTemp { get; set; }
        public DbSet<Sys_UserDefaultOpenPage> Sys_UserDefaultOpenPage { get; set; }
        public DbSet<Sys_UserInfo> Sys_UserInfo { get; set; }
        public DbSet<Sys_UserOp> Sys_UserOp { get; set; }
        public DbSet<Sys_UserOPList> Sys_UserOPList { get; set; }
        public DbSet<Sys_UserRemindList> Sys_UserRemindList { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<VIP_Ads> VIP_Ads { get; set; }
        public DbSet<VIP_Adspic> VIP_Adspic { get; set; }
        public DbSet<VIP_CardChange> VIP_CardChange { get; set; }
        public DbSet<VIP_HallInfoHeader> VIP_HallInfoHeader { get; set; }
        public DbSet<VIP_HallOffInfo> VIP_HallOffInfo { get; set; }
        public DbSet<VIP_IDCardType> VIP_IDCardType { get; set; }
        public DbSet<VIP_KeepLore> VIP_KeepLore { get; set; }
        public DbSet<VIP_news> VIP_news { get; set; }
        public DbSet<VIP_newspics> VIP_newspics { get; set; }
        public DbSet<VIP_OffList> VIP_OffList { get; set; }
        public DbSet<VIP_OffListAduit> VIP_OffListAduit { get; set; }
        public DbSet<VIP_OffListAduitHeader> VIP_OffListAduitHeader { get; set; }
        public DbSet<VIP_OffListPics> VIP_OffListPics { get; set; }
        public DbSet<VIP_OffTicket> VIP_OffTicket { get; set; }
        public DbSet<VIP_ProOffList> VIP_ProOffList { get; set; }
        public DbSet<VIP_Renew> VIP_Renew { get; set; }
        public DbSet<VIP_RenewBack> VIP_RenewBack { get; set; }
        public DbSet<VIP_RenewBackAduit> VIP_RenewBackAduit { get; set; }
        public DbSet<VIP_RenewBackAduit_bak> VIP_RenewBackAduit_bak { get; set; }
        public DbSet<VIP_RepairInfor> VIP_RepairInfor { get; set; }
        public DbSet<VIP_SearchShop> VIP_SearchShop { get; set; }
        public DbSet<VIP_SendProList> VIP_SendProList { get; set; }
        public DbSet<VIP_SendProOffList> VIP_SendProOffList { get; set; }
        public DbSet<VIP_tianyadiscountPic> VIP_tianyadiscountPic { get; set; }
        public DbSet<VIP_tianYidiscount> VIP_tianYidiscount { get; set; }
        public DbSet<VIP_Version> VIP_Version { get; set; }
        public DbSet<VIP_VIPBack> VIP_VIPBack { get; set; }
        public DbSet<VIP_VIPBackAduit> VIP_VIPBackAduit { get; set; }
        public DbSet<VIP_VIPInfo> VIP_VIPInfo { get; set; }
        public DbSet<VIP_VIPInfo_BAK> VIP_VIPInfo_BAK { get; set; }
        public DbSet<VIP_VIPInfo_Temp> VIP_VIPInfo_Temp { get; set; }
        public DbSet<VIP_VIPOffLIst> VIP_VIPOffLIst { get; set; }
        public DbSet<VIP_VIPService> VIP_VIPService { get; set; }
        public DbSet<VIP_VIPService_BAK> VIP_VIPService_BAK { get; set; }
        public DbSet<VIP_VIPType> VIP_VIPType { get; set; }
        public DbSet<VIP_VIPType_Bak> VIP_VIPType_Bak { get; set; }
        public DbSet<VIP_VIPTypeOffLIst> VIP_VIPTypeOffLIst { get; set; }
        public DbSet<VIP_VIPTypeService> VIP_VIPTypeService { get; set; }
        public DbSet<VIP_VIPTypeService_BAK> VIP_VIPTypeService_BAK { get; set; }
        public DbSet<VIP_Warrantyservice> VIP_Warrantyservice { get; set; }
        public DbSet<VIP_weekly> VIP_weekly { get; set; }
        public DbSet<Chart_MapReport> Chart_MapReport { get; set; }
        public DbSet<Print_SellBackInfo> Print_SellBackInfo { get; set; }
        public DbSet<Print_SellBackListInfo> Print_SellBackListInfo { get; set; }
        public DbSet<Print_SellInfo> Print_SellInfo { get; set; }
        public DbSet<Print_SellListInfo> Print_SellListInfo { get; set; }
        public DbSet<Pro_OutModel> Pro_OutModel { get; set; }
        public DbSet<Report_AirOutListInfo> Report_AirOutListInfo { get; set; }
        public DbSet<Report_BackIMEIInfo> Report_BackIMEIInfo { get; set; }
        public DbSet<Report_BackInfo> Report_BackInfo { get; set; }
        public DbSet<Report_BackListInfo> Report_BackListInfo { get; set; }
        public DbSet<Report_Borow> Report_Borow { get; set; }
        public DbSet<Report_BorrowAduitInfo> Report_BorrowAduitInfo { get; set; }
        public DbSet<Report_BorrowAduitListInfo> Report_BorrowAduitListInfo { get; set; }
        public DbSet<Report_BorrowIMEIInfo> Report_BorrowIMEIInfo { get; set; }
        public DbSet<Report_BorrowInfo> Report_BorrowInfo { get; set; }
        public DbSet<Report_ChangeIMEIInfo> Report_ChangeIMEIInfo { get; set; }
        public DbSet<Report_ChangeListInfo> Report_ChangeListInfo { get; set; }
        public DbSet<Report_ChangeProInfo> Report_ChangeProInfo { get; set; }
        public DbSet<Report_CostPriceListInfo> Report_CostPriceListInfo { get; set; }
        public DbSet<Report_EveryHallStoreInfo> Report_EveryHallStoreInfo { get; set; }
        public DbSet<Report_IMEIInfo> Report_IMEIInfo { get; set; }
        public DbSet<Report_IMEITracksInfo> Report_IMEITracksInfo { get; set; }
        public DbSet<Report_InOrderIMEIInfo> Report_InOrderIMEIInfo { get; set; }
        public DbSet<Report_InOrderInfo> Report_InOrderInfo { get; set; }
        public DbSet<Report_InOrderListInfo> Report_InOrderListInfo { get; set; }
        public DbSet<Report_InOutSellInfo> Report_InOutSellInfo { get; set; }
        public DbSet<Report_OldIMEITrackInfo> Report_OldIMEITrackInfo { get; set; }
        public DbSet<Report_OutInfo> Report_OutInfo { get; set; }
        public DbSet<Report_OutOrderIMEIInfo> Report_OutOrderIMEIInfo { get; set; }
        public DbSet<Report_OutOrderListInfo> Report_OutOrderListInfo { get; set; }
        public DbSet<Report_PriceInfo> Report_PriceInfo { get; set; }
        public DbSet<Report_ProductInfo> Report_ProductInfo { get; set; }
        public DbSet<Report_Profit> Report_Profit { get; set; }
        public DbSet<Report_Profit2> Report_Profit2 { get; set; }
        public DbSet<Report_ReNewInfo> Report_ReNewInfo { get; set; }
        public DbSet<Report_RepairInfo> Report_RepairInfo { get; set; }
        public DbSet<Report_RepairReturnInfo> Report_RepairReturnInfo { get; set; }
        public DbSet<Report_Return> Report_Return { get; set; }
        public DbSet<Report_ReturnIMEIInfo> Report_ReturnIMEIInfo { get; set; }
        public DbSet<Report_ReturnListInfo> Report_ReturnListInfo { get; set; }
        public DbSet<Report_SellAduitInfo> Report_SellAduitInfo { get; set; }
        public DbSet<Report_SellAduitListInfo> Report_SellAduitListInfo { get; set; }
        public DbSet<Report_SellChart> Report_SellChart { get; set; }
        public DbSet<Report_SellListInfo> Report_SellListInfo { get; set; }
        public DbSet<Report_SellListInfo2> Report_SellListInfo2 { get; set; }
        public DbSet<Report_SellReport> Report_SellReport { get; set; }
        public DbSet<Report_SellReport2> Report_SellReport2 { get; set; }
        public DbSet<Report_SellReport3> Report_SellReport3 { get; set; }
        public DbSet<Report_SMSSign> Report_SMSSign { get; set; }
        public DbSet<Report_StoreInfo> Report_StoreInfo { get; set; }
        public DbSet<Report_VipBuyTimeInfo> Report_VipBuyTimeInfo { get; set; }
        public DbSet<Report_VipBuyTimeInfo2> Report_VipBuyTimeInfo2 { get; set; }
        public DbSet<Report_YANBAO> Report_YANBAO { get; set; }
        public DbSet<Report_YANBAO2> Report_YANBAO2 { get; set; }
        public DbSet<View_AirOutInfo> View_AirOutInfo { get; set; }
        public DbSet<View_AirOutListModel> View_AirOutListModel { get; set; }
        public DbSet<View_BorowAduit> View_BorowAduit { get; set; }
        public DbSet<View_BorowAduit2> View_BorowAduit2 { get; set; }
        public DbSet<View_BorowAduit3> View_BorowAduit3 { get; set; }
        public DbSet<View_BorowAduit4> View_BorowAduit4 { get; set; }
        public DbSet<View_BorowInfo> View_BorowInfo { get; set; }
        public DbSet<View_BorowReturnDetail> View_BorowReturnDetail { get; set; }
        public DbSet<View_BorowReturnInfo> View_BorowReturnInfo { get; set; }
        public DbSet<View_CostBillReport> View_CostBillReport { get; set; }
        public DbSet<View_GroupTypeInfo> View_GroupTypeInfo { get; set; }
        public DbSet<View_HallInfo> View_HallInfo { get; set; }
        public DbSet<View_Off_AduitProInfo> View_Off_AduitProInfo { get; set; }
        public DbSet<View_Off_AduitTypeInfo> View_Off_AduitTypeInfo { get; set; }
        public DbSet<View_OffList> View_OffList { get; set; }
        public DbSet<View_OffListAduit> View_OffListAduit { get; set; }
        public DbSet<View_OtherSellBackCount> View_OtherSellBackCount { get; set; }
        public DbSet<View_OutOrderList> View_OutOrderList { get; set; }
        public DbSet<View_OutSearch> View_OutSearch { get; set; }
        public DbSet<View_Package_GroupInfo> View_Package_GroupInfo { get; set; }
        public DbSet<View_PackageGroupTypeInfo> View_PackageGroupTypeInfo { get; set; }
        public DbSet<View_PackageSalesNameInfo> View_PackageSalesNameInfo { get; set; }
        public DbSet<View_PerSellBackSalary> View_PerSellBackSalary { get; set; }
        public DbSet<View_PersonSalary> View_PersonSalary { get; set; }
        public DbSet<View_PersonSellbackCount> View_PersonSellbackCount { get; set; }
        public DbSet<View_PriceBillReport> View_PriceBillReport { get; set; }
        public DbSet<View_PriceTypeClassInfo> View_PriceTypeClassInfo { get; set; }
        public DbSet<View_Pro_ChangeList> View_Pro_ChangeList { get; set; }
        public DbSet<View_Pro_Class_TypeInfo> View_Pro_Class_TypeInfo { get; set; }
        public DbSet<View_Pro_CostChangeList> View_Pro_CostChangeList { get; set; }
        public DbSet<View_Pro_InOrder> View_Pro_InOrder { get; set; }
        public DbSet<View_Pro_NoCostInfo> View_Pro_NoCostInfo { get; set; }
        public DbSet<View_Pro_NoCostLeftTree> View_Pro_NoCostLeftTree { get; set; }
        public DbSet<View_Pro_RepairInfo> View_Pro_RepairInfo { get; set; }
        public DbSet<View_Pro_RepairRetrunDetail> View_Pro_RepairRetrunDetail { get; set; }
        public DbSet<View_Pro_RepairReturnInfo> View_Pro_RepairReturnInfo { get; set; }
        public DbSet<View_Pro_SellAduit> View_Pro_SellAduit { get; set; }
        public DbSet<View_Pro_SellBackAduit> View_Pro_SellBackAduit { get; set; }
        public DbSet<View_Pro_SellInfo> View_Pro_SellInfo { get; set; }
        public DbSet<View_ProAllType> View_ProAllType { get; set; }
        public DbSet<View_ProBandInfo> View_ProBandInfo { get; set; }
        public DbSet<View_ProInfo> View_ProInfo { get; set; }
        public DbSet<View_ProMainInfo> View_ProMainInfo { get; set; }
        public DbSet<View_ProNameInfo> View_ProNameInfo { get; set; }
        public DbSet<View_ProOffList> View_ProOffList { get; set; }
        public DbSet<View_ProSellBackAduitDetail> View_ProSellBackAduitDetail { get; set; }
        public DbSet<View_RemindList> View_RemindList { get; set; }
        public DbSet<View_RepaireRetList> View_RepaireRetList { get; set; }
        public DbSet<View_RepairInfo> View_RepairInfo { get; set; }
        public DbSet<View_RepairReturnReceive> View_RepairReturnReceive { get; set; }
        public DbSet<View_ReturnInfo> View_ReturnInfo { get; set; }
        public DbSet<View_Rules_OffList> View_Rules_OffList { get; set; }
        public DbSet<View_SalaryList> View_SalaryList { get; set; }
        public DbSet<View_Sell_Back_NewSpecialOffListInfo> View_Sell_Back_NewSpecialOffListInfo { get; set; }
        public DbSet<View_SellBackListInfo> View_SellBackListInfo { get; set; }
        public DbSet<View_SellBackNewInfo> View_SellBackNewInfo { get; set; }
        public DbSet<View_SellListInfo> View_SellListInfo { get; set; }
        public DbSet<View_SellOffAduitInfo> View_SellOffAduitInfo { get; set; }
        public DbSet<View_SellOffAduitInfo2> View_SellOffAduitInfo2 { get; set; }
        public DbSet<View_SellOffAduitInfoList> View_SellOffAduitInfoList { get; set; }
        public DbSet<View_SellOffAduitProList> View_SellOffAduitProList { get; set; }
        public DbSet<View_SellTypeProduct> View_SellTypeProduct { get; set; }
        public DbSet<View_SingleSellPrice> View_SingleSellPrice { get; set; }
        public DbSet<View_SMS_SellBackAduit> View_SMS_SellBackAduit { get; set; }
        public DbSet<View_SMS_SignInfo> View_SMS_SignInfo { get; set; }
        public DbSet<View_SMS_SignSendPayInfo> View_SMS_SignSendPayInfo { get; set; }
        public DbSet<View_SpecialOffListInfo> View_SpecialOffListInfo { get; set; }
        public DbSet<View_StoreWithNum> View_StoreWithNum { get; set; }
        public DbSet<View_Sys_Option> View_Sys_Option { get; set; }
        public DbSet<View_Sys_UserInfo> View_Sys_UserInfo { get; set; }
        public DbSet<View_UserBorowInfo> View_UserBorowInfo { get; set; }
        public DbSet<View_UserRemindList> View_UserRemindList { get; set; }
        public DbSet<View_VIP_OffList> View_VIP_OffList { get; set; }
        public DbSet<View_VIP_Renew> View_VIP_Renew { get; set; }
        public DbSet<View_VIP_RenewBackAduit> View_VIP_RenewBackAduit { get; set; }
        public DbSet<View_VIPBackAduit> View_VIPBackAduit { get; set; }
        public DbSet<View_VIPBackApply> View_VIPBackApply { get; set; }
        public DbSet<View_VIPInfo> View_VIPInfo { get; set; }
        public DbSet<View_VIPOffListAduit> View_VIPOffListAduit { get; set; }
        public DbSet<View_VIPOffListAduitHeader> View_VIPOffListAduitHeader { get; set; }
        public DbSet<View_VIPService> View_VIPService { get; set; }
        public DbSet<View_VIPTypeService> View_VIPTypeService { get; set; }
        public DbSet<View_YanBoPriceStepInfo> View_YanBoPriceStepInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Demo_HallInfoMap());
            modelBuilder.Configurations.Add(new Demo_MapReportMap());
            modelBuilder.Configurations.Add(new Demo_ProInfoMap());
            modelBuilder.Configurations.Add(new Demo_ReportViewColumnInfoMap());
            modelBuilder.Configurations.Add(new Demo_ReportViewInfoMap());
            modelBuilder.Configurations.Add(new Demo_各厅库存Map());
            modelBuilder.Configurations.Add(new Demo_归还记录Map());
            modelBuilder.Configurations.Add(new Demo_借贷查询Map());
            modelBuilder.Configurations.Add(new Demo_进销存报表Map());
            modelBuilder.Configurations.Add(new Demo_提成报表Map());
            modelBuilder.Configurations.Add(new Demo_退货查询Map());
            modelBuilder.Configurations.Add(new Demo_销售汇总Map());
            modelBuilder.Configurations.Add(new Demo_销售明细Map());
            modelBuilder.Configurations.Add(new Demo_周转率报表Map());
            modelBuilder.Configurations.Add(new Off_AduitHallInfoMap());
            modelBuilder.Configurations.Add(new Off_AduitProInfoMap());
            modelBuilder.Configurations.Add(new Off_AduitTypeInfoMap());
            modelBuilder.Configurations.Add(new Package_GroupInfoMap());
            modelBuilder.Configurations.Add(new Package_GroupTypeInfoMap());
            modelBuilder.Configurations.Add(new Package_ProInfoMap());
            modelBuilder.Configurations.Add(new Package_SalesNameInfoMap());
            modelBuilder.Configurations.Add(new Pro_AirOutInfoMap());
            modelBuilder.Configurations.Add(new Pro_AirOutListInfoMap());
            modelBuilder.Configurations.Add(new Pro_AreaInfoMap());
            modelBuilder.Configurations.Add(new Pro_BackInfoMap());
            modelBuilder.Configurations.Add(new Pro_BackListInfoMap());
            modelBuilder.Configurations.Add(new Pro_BackOrderIMEIMap());
            modelBuilder.Configurations.Add(new Pro_BigAreaInfoMap());
            modelBuilder.Configurations.Add(new Pro_BorowAduitMap());
            modelBuilder.Configurations.Add(new Pro_BorowAduit_bakMap());
            modelBuilder.Configurations.Add(new Pro_BorowAduitListMap());
            modelBuilder.Configurations.Add(new Pro_BorowInfoMap());
            modelBuilder.Configurations.Add(new Pro_BorowListInfoMap());
            modelBuilder.Configurations.Add(new Pro_BorowOrderIMEIMap());
            modelBuilder.Configurations.Add(new Pro_CashTicketMap());
            modelBuilder.Configurations.Add(new Pro_ChangeIMEIInfoMap());
            modelBuilder.Configurations.Add(new Pro_ChangeProInfoMap());
            modelBuilder.Configurations.Add(new Pro_ChangeProListInfoMap());
            modelBuilder.Configurations.Add(new Pro_ClassInfoMap());
            modelBuilder.Configurations.Add(new Pro_ClassTypeMap());
            modelBuilder.Configurations.Add(new Pro_HallInfoMap());
            modelBuilder.Configurations.Add(new Pro_IMEIMap());
            modelBuilder.Configurations.Add(new Pro_IMEI_DeletedMap());
            modelBuilder.Configurations.Add(new Pro_InOrderMap());
            modelBuilder.Configurations.Add(new Pro_InOrderIMEIMap());
            modelBuilder.Configurations.Add(new Pro_InOrderListMap());
            modelBuilder.Configurations.Add(new Pro_LevelInfoMap());
            modelBuilder.Configurations.Add(new Pro_OutInfoMap());
            modelBuilder.Configurations.Add(new Pro_OutOrderIMEIMap());
            modelBuilder.Configurations.Add(new Pro_OutOrderListMap());
            modelBuilder.Configurations.Add(new Pro_PriceChangeMap());
            modelBuilder.Configurations.Add(new Pro_PriceChangeListMap());
            modelBuilder.Configurations.Add(new Pro_PriceCost_InorderListMap());
            modelBuilder.Configurations.Add(new Pro_PriceCostChangeMap());
            modelBuilder.Configurations.Add(new Pro_PriceCostChangeListMap());
            modelBuilder.Configurations.Add(new Pro_ProInfoMap());
            modelBuilder.Configurations.Add(new Pro_ProMainInfoMap());
            modelBuilder.Configurations.Add(new Pro_ProNameInfoMap());
            modelBuilder.Configurations.Add(new Pro_PropertyMap());
            modelBuilder.Configurations.Add(new Pro_PropertyValueMap());
            modelBuilder.Configurations.Add(new Pro_ProProperty_FMap());
            modelBuilder.Configurations.Add(new Pro_RepairInfoMap());
            modelBuilder.Configurations.Add(new Pro_RepairListInfoMap());
            modelBuilder.Configurations.Add(new Pro_RepairReturnInfoMap());
            modelBuilder.Configurations.Add(new Pro_RepairReturnInfo_BAKMap());
            modelBuilder.Configurations.Add(new Pro_RepairReturnListInfoMap());
            modelBuilder.Configurations.Add(new Pro_ReturnInfoMap());
            modelBuilder.Configurations.Add(new Pro_ReturnListInfoMap());
            modelBuilder.Configurations.Add(new Pro_ReturnOrderIMEIMap());
            modelBuilder.Configurations.Add(new Pro_SalaryListOneDayMap());
            modelBuilder.Configurations.Add(new Pro_Sell_CarMap());
            modelBuilder.Configurations.Add(new Pro_Sell_JiPeiKaMap());
            modelBuilder.Configurations.Add(new Pro_Sell_JiPeiKa_tempMap());
            modelBuilder.Configurations.Add(new Pro_Sell_ServiceMap());
            modelBuilder.Configurations.Add(new Pro_Sell_YanbaoMap());
            modelBuilder.Configurations.Add(new Pro_Sell_Yanbao_tempMap());
            modelBuilder.Configurations.Add(new Pro_SellAduitMap());
            modelBuilder.Configurations.Add(new Pro_SellAduit_bakMap());
            modelBuilder.Configurations.Add(new Pro_SellAduitListMap());
            modelBuilder.Configurations.Add(new Pro_SellBackMap());
            modelBuilder.Configurations.Add(new Pro_SellBackAduitMap());
            modelBuilder.Configurations.Add(new Pro_SellBackAduit_bakMap());
            modelBuilder.Configurations.Add(new Pro_SellBackAduitListMap());
            modelBuilder.Configurations.Add(new Pro_SellBackAduitOffListMap());
            modelBuilder.Configurations.Add(new Pro_SellBackIMEIListMap());
            modelBuilder.Configurations.Add(new Pro_SellBackInfo_AduitMap());
            modelBuilder.Configurations.Add(new Pro_SellBackListMap());
            modelBuilder.Configurations.Add(new Pro_SellBackSpecalOffListMap());
            modelBuilder.Configurations.Add(new Pro_SellIMEIListMap());
            modelBuilder.Configurations.Add(new Pro_SellInfoMap());
            modelBuilder.Configurations.Add(new Pro_SellInfo_AduitMap());
            modelBuilder.Configurations.Add(new Pro_SellList_RulesInfoMap());
            modelBuilder.Configurations.Add(new Pro_SellListInfoMap());
            modelBuilder.Configurations.Add(new Pro_SellListInfo_TempMap());
            modelBuilder.Configurations.Add(new Pro_SellListServiceInfoMap());
            modelBuilder.Configurations.Add(new Pro_SellOffAduitInfoMap());
            modelBuilder.Configurations.Add(new Pro_SellOffAduitInfoListMap());
            modelBuilder.Configurations.Add(new Pro_SellSendInfoMap());
            modelBuilder.Configurations.Add(new Pro_SellSpecalOffListMap());
            modelBuilder.Configurations.Add(new Pro_SellTypeMap());
            modelBuilder.Configurations.Add(new Pro_SellTypeProductMap());
            modelBuilder.Configurations.Add(new Pro_SellTypeProduct_bakMap());
            modelBuilder.Configurations.Add(new Pro_StoreInfoMap());
            modelBuilder.Configurations.Add(new Pro_TypeInfoMap());
            modelBuilder.Configurations.Add(new Pro_YanbaoPriceStepInfoMap());
            modelBuilder.Configurations.Add(new Pro_YanbaoPriceStepInfo_bakMap());
            modelBuilder.Configurations.Add(new Rules_AllCurrentRulesInfoMap());
            modelBuilder.Configurations.Add(new Rules_HallOffInfoMap());
            modelBuilder.Configurations.Add(new Rules_OffListMap());
            modelBuilder.Configurations.Add(new Rules_Pro_RulesTypeInfoMap());
            modelBuilder.Configurations.Add(new Rules_ProMainInfoMap());
            modelBuilder.Configurations.Add(new Rules_SellTypeInfoMap());
            modelBuilder.Configurations.Add(new Rules_TypeInfoMap());
            modelBuilder.Configurations.Add(new SMS_SellBackMap());
            modelBuilder.Configurations.Add(new SMS_SellBackAduitMap());
            modelBuilder.Configurations.Add(new SMS_SignInfoMap());
            modelBuilder.Configurations.Add(new SMS_SignPayInListInfoMap());
            modelBuilder.Configurations.Add(new SMS_SignSendPayInfoMap());
            modelBuilder.Configurations.Add(new SMS_SignSendPayInfo_DeleteMap());
            modelBuilder.Configurations.Add(new Sys_DeptInfoMap());
            modelBuilder.Configurations.Add(new Sys_InitDataStatusMap());
            modelBuilder.Configurations.Add(new Sys_LoginInfoMap());
            modelBuilder.Configurations.Add(new Sys_MenuInfoMap());
            modelBuilder.Configurations.Add(new Sys_MethodInfoMap());
            modelBuilder.Configurations.Add(new Sys_OptionMap());
            modelBuilder.Configurations.Add(new Sys_OrderMakerInfoMap());
            modelBuilder.Configurations.Add(new Sys_RemindListMap());
            modelBuilder.Configurations.Add(new Sys_Role_HallInfoMap());
            modelBuilder.Configurations.Add(new Sys_Role_Menu_HallInfoMap());
            modelBuilder.Configurations.Add(new Sys_Role_Menu_HallInfo_bakMap());
            modelBuilder.Configurations.Add(new Sys_Role_Menu_ProInfoMap());
            modelBuilder.Configurations.Add(new Sys_Role_Menu_ProInfo_bakMap());
            modelBuilder.Configurations.Add(new Sys_Role_MenuInfoMap());
            modelBuilder.Configurations.Add(new Sys_Role_MenuInfo_bakMap());
            modelBuilder.Configurations.Add(new Sys_RoleInfoMap());
            modelBuilder.Configurations.Add(new Sys_RoleInfo_backMap());
            modelBuilder.Configurations.Add(new Sys_RoleMethodMap());
            modelBuilder.Configurations.Add(new Sys_SalaryChangeMap());
            modelBuilder.Configurations.Add(new Sys_SalaryCurrentDeleteMap());
            modelBuilder.Configurations.Add(new Sys_SalaryCurrentListMap());
            modelBuilder.Configurations.Add(new Sys_SalaryCurrentListDeleteListMap());
            modelBuilder.Configurations.Add(new Sys_SalaryListMap());
            modelBuilder.Configurations.Add(new Sys_SalaryList_bakMap());
            modelBuilder.Configurations.Add(new Sys_SalaryTempMap());
            modelBuilder.Configurations.Add(new Sys_UserDefaultOpenPageMap());
            modelBuilder.Configurations.Add(new Sys_UserInfoMap());
            modelBuilder.Configurations.Add(new Sys_UserOpMap());
            modelBuilder.Configurations.Add(new Sys_UserOPListMap());
            modelBuilder.Configurations.Add(new Sys_UserRemindListMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new VIP_AdsMap());
            modelBuilder.Configurations.Add(new VIP_AdspicMap());
            modelBuilder.Configurations.Add(new VIP_CardChangeMap());
            modelBuilder.Configurations.Add(new VIP_HallInfoHeaderMap());
            modelBuilder.Configurations.Add(new VIP_HallOffInfoMap());
            modelBuilder.Configurations.Add(new VIP_IDCardTypeMap());
            modelBuilder.Configurations.Add(new VIP_KeepLoreMap());
            modelBuilder.Configurations.Add(new VIP_newsMap());
            modelBuilder.Configurations.Add(new VIP_newspicsMap());
            modelBuilder.Configurations.Add(new VIP_OffListMap());
            modelBuilder.Configurations.Add(new VIP_OffListAduitMap());
            modelBuilder.Configurations.Add(new VIP_OffListAduitHeaderMap());
            modelBuilder.Configurations.Add(new VIP_OffListPicsMap());
            modelBuilder.Configurations.Add(new VIP_OffTicketMap());
            modelBuilder.Configurations.Add(new VIP_ProOffListMap());
            modelBuilder.Configurations.Add(new VIP_RenewMap());
            modelBuilder.Configurations.Add(new VIP_RenewBackMap());
            modelBuilder.Configurations.Add(new VIP_RenewBackAduitMap());
            modelBuilder.Configurations.Add(new VIP_RenewBackAduit_bakMap());
            modelBuilder.Configurations.Add(new VIP_RepairInforMap());
            modelBuilder.Configurations.Add(new VIP_SearchShopMap());
            modelBuilder.Configurations.Add(new VIP_SendProListMap());
            modelBuilder.Configurations.Add(new VIP_SendProOffListMap());
            modelBuilder.Configurations.Add(new VIP_tianyadiscountPicMap());
            modelBuilder.Configurations.Add(new VIP_tianYidiscountMap());
            modelBuilder.Configurations.Add(new VIP_VersionMap());
            modelBuilder.Configurations.Add(new VIP_VIPBackMap());
            modelBuilder.Configurations.Add(new VIP_VIPBackAduitMap());
            modelBuilder.Configurations.Add(new VIP_VIPInfoMap());
            modelBuilder.Configurations.Add(new VIP_VIPInfo_BAKMap());
            modelBuilder.Configurations.Add(new VIP_VIPInfo_TempMap());
            modelBuilder.Configurations.Add(new VIP_VIPOffLIstMap());
            modelBuilder.Configurations.Add(new VIP_VIPServiceMap());
            modelBuilder.Configurations.Add(new VIP_VIPService_BAKMap());
            modelBuilder.Configurations.Add(new VIP_VIPTypeMap());
            modelBuilder.Configurations.Add(new VIP_VIPType_BakMap());
            modelBuilder.Configurations.Add(new VIP_VIPTypeOffLIstMap());
            modelBuilder.Configurations.Add(new VIP_VIPTypeServiceMap());
            modelBuilder.Configurations.Add(new VIP_VIPTypeService_BAKMap());
            modelBuilder.Configurations.Add(new VIP_WarrantyserviceMap());
            modelBuilder.Configurations.Add(new VIP_weeklyMap());
            modelBuilder.Configurations.Add(new Chart_MapReportMap());
            modelBuilder.Configurations.Add(new Print_SellBackInfoMap());
            modelBuilder.Configurations.Add(new Print_SellBackListInfoMap());
            modelBuilder.Configurations.Add(new Print_SellInfoMap());
            modelBuilder.Configurations.Add(new Print_SellListInfoMap());
            modelBuilder.Configurations.Add(new Pro_OutModelMap());
            modelBuilder.Configurations.Add(new Report_AirOutListInfoMap());
            modelBuilder.Configurations.Add(new Report_BackIMEIInfoMap());
            modelBuilder.Configurations.Add(new Report_BackInfoMap());
            modelBuilder.Configurations.Add(new Report_BackListInfoMap());
            modelBuilder.Configurations.Add(new Report_BorowMap());
            modelBuilder.Configurations.Add(new Report_BorrowAduitInfoMap());
            modelBuilder.Configurations.Add(new Report_BorrowAduitListInfoMap());
            modelBuilder.Configurations.Add(new Report_BorrowIMEIInfoMap());
            modelBuilder.Configurations.Add(new Report_BorrowInfoMap());
            modelBuilder.Configurations.Add(new Report_ChangeIMEIInfoMap());
            modelBuilder.Configurations.Add(new Report_ChangeListInfoMap());
            modelBuilder.Configurations.Add(new Report_ChangeProInfoMap());
            modelBuilder.Configurations.Add(new Report_CostPriceListInfoMap());
            modelBuilder.Configurations.Add(new Report_EveryHallStoreInfoMap());
            modelBuilder.Configurations.Add(new Report_IMEIInfoMap());
            modelBuilder.Configurations.Add(new Report_IMEITracksInfoMap());
            modelBuilder.Configurations.Add(new Report_InOrderIMEIInfoMap());
            modelBuilder.Configurations.Add(new Report_InOrderInfoMap());
            modelBuilder.Configurations.Add(new Report_InOrderListInfoMap());
            modelBuilder.Configurations.Add(new Report_InOutSellInfoMap());
            modelBuilder.Configurations.Add(new Report_OldIMEITrackInfoMap());
            modelBuilder.Configurations.Add(new Report_OutInfoMap());
            modelBuilder.Configurations.Add(new Report_OutOrderIMEIInfoMap());
            modelBuilder.Configurations.Add(new Report_OutOrderListInfoMap());
            modelBuilder.Configurations.Add(new Report_PriceInfoMap());
            modelBuilder.Configurations.Add(new Report_ProductInfoMap());
            modelBuilder.Configurations.Add(new Report_ProfitMap());
            modelBuilder.Configurations.Add(new Report_Profit2Map());
            modelBuilder.Configurations.Add(new Report_ReNewInfoMap());
            modelBuilder.Configurations.Add(new Report_RepairInfoMap());
            modelBuilder.Configurations.Add(new Report_RepairReturnInfoMap());
            modelBuilder.Configurations.Add(new Report_ReturnMap());
            modelBuilder.Configurations.Add(new Report_ReturnIMEIInfoMap());
            modelBuilder.Configurations.Add(new Report_ReturnListInfoMap());
            modelBuilder.Configurations.Add(new Report_SellAduitInfoMap());
            modelBuilder.Configurations.Add(new Report_SellAduitListInfoMap());
            modelBuilder.Configurations.Add(new Report_SellChartMap());
            modelBuilder.Configurations.Add(new Report_SellListInfoMap());
            modelBuilder.Configurations.Add(new Report_SellListInfo2Map());
            modelBuilder.Configurations.Add(new Report_SellReportMap());
            modelBuilder.Configurations.Add(new Report_SellReport2Map());
            modelBuilder.Configurations.Add(new Report_SellReport3Map());
            modelBuilder.Configurations.Add(new Report_SMSSignMap());
            modelBuilder.Configurations.Add(new Report_StoreInfoMap());
            modelBuilder.Configurations.Add(new Report_VipBuyTimeInfoMap());
            modelBuilder.Configurations.Add(new Report_VipBuyTimeInfo2Map());
            modelBuilder.Configurations.Add(new Report_YANBAOMap());
            modelBuilder.Configurations.Add(new Report_YANBAO2Map());
            modelBuilder.Configurations.Add(new View_AirOutInfoMap());
            modelBuilder.Configurations.Add(new View_AirOutListModelMap());
            modelBuilder.Configurations.Add(new View_BorowAduitMap());
            modelBuilder.Configurations.Add(new View_BorowAduit2Map());
            modelBuilder.Configurations.Add(new View_BorowAduit3Map());
            modelBuilder.Configurations.Add(new View_BorowAduit4Map());
            modelBuilder.Configurations.Add(new View_BorowInfoMap());
            modelBuilder.Configurations.Add(new View_BorowReturnDetailMap());
            modelBuilder.Configurations.Add(new View_BorowReturnInfoMap());
            modelBuilder.Configurations.Add(new View_CostBillReportMap());
            modelBuilder.Configurations.Add(new View_GroupTypeInfoMap());
            modelBuilder.Configurations.Add(new View_HallInfoMap());
            modelBuilder.Configurations.Add(new View_Off_AduitProInfoMap());
            modelBuilder.Configurations.Add(new View_Off_AduitTypeInfoMap());
            modelBuilder.Configurations.Add(new View_OffListMap());
            modelBuilder.Configurations.Add(new View_OffListAduitMap());
            modelBuilder.Configurations.Add(new View_OtherSellBackCountMap());
            modelBuilder.Configurations.Add(new View_OutOrderListMap());
            modelBuilder.Configurations.Add(new View_OutSearchMap());
            modelBuilder.Configurations.Add(new View_Package_GroupInfoMap());
            modelBuilder.Configurations.Add(new View_PackageGroupTypeInfoMap());
            modelBuilder.Configurations.Add(new View_PackageSalesNameInfoMap());
            modelBuilder.Configurations.Add(new View_PerSellBackSalaryMap());
            modelBuilder.Configurations.Add(new View_PersonSalaryMap());
            modelBuilder.Configurations.Add(new View_PersonSellbackCountMap());
            modelBuilder.Configurations.Add(new View_PriceBillReportMap());
            modelBuilder.Configurations.Add(new View_PriceTypeClassInfoMap());
            modelBuilder.Configurations.Add(new View_Pro_ChangeListMap());
            modelBuilder.Configurations.Add(new View_Pro_Class_TypeInfoMap());
            modelBuilder.Configurations.Add(new View_Pro_CostChangeListMap());
            modelBuilder.Configurations.Add(new View_Pro_InOrderMap());
            modelBuilder.Configurations.Add(new View_Pro_NoCostInfoMap());
            modelBuilder.Configurations.Add(new View_Pro_NoCostLeftTreeMap());
            modelBuilder.Configurations.Add(new View_Pro_RepairInfoMap());
            modelBuilder.Configurations.Add(new View_Pro_RepairRetrunDetailMap());
            modelBuilder.Configurations.Add(new View_Pro_RepairReturnInfoMap());
            modelBuilder.Configurations.Add(new View_Pro_SellAduitMap());
            modelBuilder.Configurations.Add(new View_Pro_SellBackAduitMap());
            modelBuilder.Configurations.Add(new View_Pro_SellInfoMap());
            modelBuilder.Configurations.Add(new View_ProAllTypeMap());
            modelBuilder.Configurations.Add(new View_ProBandInfoMap());
            modelBuilder.Configurations.Add(new View_ProInfoMap());
            modelBuilder.Configurations.Add(new View_ProMainInfoMap());
            modelBuilder.Configurations.Add(new View_ProNameInfoMap());
            modelBuilder.Configurations.Add(new View_ProOffListMap());
            modelBuilder.Configurations.Add(new View_ProSellBackAduitDetailMap());
            modelBuilder.Configurations.Add(new View_RemindListMap());
            modelBuilder.Configurations.Add(new View_RepaireRetListMap());
            modelBuilder.Configurations.Add(new View_RepairInfoMap());
            modelBuilder.Configurations.Add(new View_RepairReturnReceiveMap());
            modelBuilder.Configurations.Add(new View_ReturnInfoMap());
            modelBuilder.Configurations.Add(new View_Rules_OffListMap());
            modelBuilder.Configurations.Add(new View_SalaryListMap());
            modelBuilder.Configurations.Add(new View_Sell_Back_NewSpecialOffListInfoMap());
            modelBuilder.Configurations.Add(new View_SellBackListInfoMap());
            modelBuilder.Configurations.Add(new View_SellBackNewInfoMap());
            modelBuilder.Configurations.Add(new View_SellListInfoMap());
            modelBuilder.Configurations.Add(new View_SellOffAduitInfoMap());
            modelBuilder.Configurations.Add(new View_SellOffAduitInfo2Map());
            modelBuilder.Configurations.Add(new View_SellOffAduitInfoListMap());
            modelBuilder.Configurations.Add(new View_SellOffAduitProListMap());
            modelBuilder.Configurations.Add(new View_SellTypeProductMap());
            modelBuilder.Configurations.Add(new View_SingleSellPriceMap());
            modelBuilder.Configurations.Add(new View_SMS_SellBackAduitMap());
            modelBuilder.Configurations.Add(new View_SMS_SignInfoMap());
            modelBuilder.Configurations.Add(new View_SMS_SignSendPayInfoMap());
            modelBuilder.Configurations.Add(new View_SpecialOffListInfoMap());
            modelBuilder.Configurations.Add(new View_StoreWithNumMap());
            modelBuilder.Configurations.Add(new View_Sys_OptionMap());
            modelBuilder.Configurations.Add(new View_Sys_UserInfoMap());
            modelBuilder.Configurations.Add(new View_UserBorowInfoMap());
            modelBuilder.Configurations.Add(new View_UserRemindListMap());
            modelBuilder.Configurations.Add(new View_VIP_OffListMap());
            modelBuilder.Configurations.Add(new View_VIP_RenewMap());
            modelBuilder.Configurations.Add(new View_VIP_RenewBackAduitMap());
            modelBuilder.Configurations.Add(new View_VIPBackAduitMap());
            modelBuilder.Configurations.Add(new View_VIPBackApplyMap());
            modelBuilder.Configurations.Add(new View_VIPInfoMap());
            modelBuilder.Configurations.Add(new View_VIPOffListAduitMap());
            modelBuilder.Configurations.Add(new View_VIPOffListAduitHeaderMap());
            modelBuilder.Configurations.Add(new View_VIPServiceMap());
            modelBuilder.Configurations.Add(new View_VIPTypeServiceMap());
            modelBuilder.Configurations.Add(new View_YanBoPriceStepInfoMap());
        }
    }
}
