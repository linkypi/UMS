using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace UserMS
{
    public static class Store
    {
        public static Color defaultcolor = Windows8Palette.Palette.AccentColor;
        public static bool IsTesting=false;
        public static int LoginServer = 0;
        public static API.UserMsServiceClient wsclient;
        public static API.Sys_UserInfo LoginUserInfo;
        public static API.Sys_RoleInfo LoginRoleInfo;
//        public static IsolatedStorageSettings clientSettings = IsolatedStorageSettings.SiteSettings;
        public static string LoginUserName="";
        public static string LoginUserPassword="";
        //public static string[] NeedInitDataList = new string[] {"Object"};
      public static CookieContainer cookieJar=new CookieContainer();
        public static List<API.Sys_RoleInfo> RoleInfo;
        public static List<API.Pro_ProInfo> ProInfo;
        public static List<API.Pro_TypeInfo> ProTypeInfo; 
        public static List<API.Pro_HallInfo> ProHallInfo;
        public static List<API.VIP_VIPType> VIPType;
        public static List<API.Pro_ClassInfo> ProClassInfo;
        public static List<API.VIP_IDCardType> CardType;
        public static List<API.Sys_UserOPList> UserOpList;
        public static List<API.Pro_AreaInfo> AreaInfo;
        public static List<API.Sys_UserOp> UserOp;
        public static List<API.Sys_UserInfo> UserInfos;
        public static List<API.Sys_Option> Options;
        public static List<API.Sys_MenuInfo> MenuInfos;
        public static List<API.Pro_SellType> SellTypes;
        public static List<API.Sys_DeptInfo> DeptInfo;
        public static List<API.Pro_LevelInfo> Level;
        public static List<API.Pro_ProMainInfo> ProMainInfo;
        public static List<API.Pro_YanbaoPriceStepInfo> YanbaoPriceStep;
        public static List<API.Pro_ProNameInfo> ProNameInfo;
        public static List<API.Rules_TypeInfo> RulesTypeInfo;
        public static List<API.Package_SalesNameInfo> PacSalesNameInfo;
        public static List<API.Pro_BigAreaInfo> BigAreaInfo;
        public static List<API.ASP_CheckInfo> CheckInfo;
        public static List<API.ASP_ErrorInfo> ErrorInfo;
        public static List<API.Pro_BillFieldInfo> BillFields;
        public static List<API.ASP_ErrType> ErrorTypes;
        public static List<API.ASP_ProOther> ProOthers;
        public static List<API.Sys_SalaryPriceStep> PriceStep;
        public static List<API.ASP_Factory> Factorys;
        public static List<API.ASP_Dealer> Dealers;
        public static List<API.ASP_RepairerProductInfo> RepairerProductInfo;

        public static string ReportServiceURL = ConfigurationManager.AppSettings["ReportServiceUrl"];
        
        public static Dictionary<Type, Dictionary<string, string>> ExcelDataColumns = new Dictionary<Type, Dictionary<string, string>>()
            {
                {
                    typeof(API.Pro_IMEI),new Dictionary<string, string>()
                    {
                        {"串码","IMEI"}
                    }
                    },
                    {typeof(API.Pro_SellInfo),new Dictionary<string, string>()
                        {
                           {"ID1","ID"},
{"SellID1","SellID"},
{"Seller1","Seller"},
{"SellDate1","SellDate"},
{"OldID1","OldID"},
{"UserID1","UserID"},
{"SysDate1","SysDate"},
{"Note1","Note"},
{"HallID1","HallID"},
{"VIP_ID1","VIP_ID"},
{"CusName1","CusName"},
{"CusPhone1","CusPhone"},
{"CardPay1","CardPay"},
{"CashPay1","CashPay"},
{"OffID1","OffID"},
{"SpecalOffID1","SpecalOffID"},
{"OffTicketID1","OffTicketID"},
{"OffTicketPrice1","OffTicketPrice"},
{"CashTotle1","CashTotle"},
{"AuditID1","AuditID"},
{"BillID1","BillID"},

                        }},
                        {typeof(API.OutImportModel),new Dictionary<string, string>()
                        {
                            {"原始单号","OldID"},
                            {"调出仓","FromHall"},
                            {"调入仓","ToHall"},
                            {"备注","Note"},
                            {"商品编码","ProID"},
                            {"商品数量","ProCount"},
                            {"串号","IMEI"}}},  
                        {typeof(API.Rules_ImportModel),new Dictionary<string, string>()
                        {   {"商品名称","ProName"},
                            {"规则类型","RulesName"},
                            {"默认优惠金额","OffPrice"},
                            {"最小优惠","MinPrice"},
                            {"最大优惠","MaxPrice"}}},  
                        {typeof(API.SalaryImportModel),new Dictionary<string, string>()
                        {   {"总商品编码","ProMainID"},
                            {"商品类别","ClassName"},
                            {"商品品牌","TypeName"},
                            {"商品型号","ProName"},
                            {"销售类别","SellTypeName"},
                            {"基本提成","BaseSalary"},
                            {"日期","Day"}}},  
            };
            
        /// <summary>
        /// 批发ID
        /// </summary>
        public static  int  SellTypeWhioleID=2;

        public static void SetClientStore(string key, object value)
        {
//            clientSettings[key] = value;
        }

        public static object GetClientStore(string key)
        {
            try
            {
                return null;
//                return clientSettings[key];
            }
            catch (Exception)
            {
                return null;
            
            }
        }

    }
}