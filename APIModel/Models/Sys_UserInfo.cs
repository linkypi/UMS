using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_UserInfo
    {
        public Sys_UserInfo()
        {
            this.Pro_BackInfo = new List<Pro_BackInfo>();
            this.Pro_BorowAduit_bak = new List<Pro_BorowAduit_bak>();
            this.Pro_BorowAduit_bak1 = new List<Pro_BorowAduit_bak>();
            this.Pro_BorowAduit_bak2 = new List<Pro_BorowAduit_bak>();
            this.Pro_BorowAduit_bak3 = new List<Pro_BorowAduit_bak>();
            this.Pro_BorowInfo = new List<Pro_BorowInfo>();
            this.Pro_ChangeProInfo = new List<Pro_ChangeProInfo>();
            this.Pro_InOrder = new List<Pro_InOrder>();
            this.Pro_OutInfo = new List<Pro_OutInfo>();
            this.Pro_OutInfo1 = new List<Pro_OutInfo>();
            this.Pro_OutInfo2 = new List<Pro_OutInfo>();
            this.Pro_PriceChange = new List<Pro_PriceChange>();
            this.Pro_RepairInfo = new List<Pro_RepairInfo>();
            this.Pro_ReturnInfo = new List<Pro_ReturnInfo>();
            this.Pro_SellAduit = new List<Pro_SellAduit>();
            this.Pro_SellAduit1 = new List<Pro_SellAduit>();
            this.Pro_SellAduit_bak = new List<Pro_SellAduit_bak>();
            this.Pro_SellAduit_bak1 = new List<Pro_SellAduit_bak>();
            this.Pro_SellBack = new List<Pro_SellBack>();
            this.Pro_SellBack1 = new List<Pro_SellBack>();
            this.Pro_SellBackAduit = new List<Pro_SellBackAduit>();
            this.Pro_SellBackAduit1 = new List<Pro_SellBackAduit>();
            this.Pro_SellBackAduit_bak = new List<Pro_SellBackAduit_bak>();
            this.Pro_SellBackAduit_bak1 = new List<Pro_SellBackAduit_bak>();
            this.Pro_SellBackInfo_Aduit = new List<Pro_SellBackInfo_Aduit>();
            this.Pro_SellBackInfo_Aduit1 = new List<Pro_SellBackInfo_Aduit>();
            this.Pro_SellInfo = new List<Pro_SellInfo>();
            this.Pro_SellInfo1 = new List<Pro_SellInfo>();
            this.Pro_SellInfo_Aduit = new List<Pro_SellInfo_Aduit>();
            this.Pro_SellInfo_Aduit1 = new List<Pro_SellInfo_Aduit>();
            this.Pro_SellOffAduitInfo = new List<Pro_SellOffAduitInfo>();
            this.Pro_SellOffAduitInfo1 = new List<Pro_SellOffAduitInfo>();
            this.Pro_SellOffAduitInfoList = new List<Pro_SellOffAduitInfoList>();
            this.Rules_OffList = new List<Rules_OffList>();
            this.Rules_OffList1 = new List<Rules_OffList>();
            this.SMS_SignInfo = new List<SMS_SignInfo>();
            this.SMS_SignSendPayInfo = new List<SMS_SignSendPayInfo>();
            this.SMS_SignSendPayInfo1 = new List<SMS_SignSendPayInfo>();
            this.Sys_SalaryChange = new List<Sys_SalaryChange>();
            this.Sys_UserDefaultOpenPage = new List<Sys_UserDefaultOpenPage>();
            this.VIP_OffList = new List<VIP_OffList>();
            this.Sys_UserRemindList = new List<Sys_UserRemindList>();
            this.Sys_UserOPList = new List<Sys_UserOPList>();
            this.Sys_UserOPList1 = new List<Sys_UserOPList>();
            this.VIP_CardChange = new List<VIP_CardChange>();
            this.VIP_RenewBackAduit = new List<VIP_RenewBackAduit>();
            this.VIP_RenewBackAduit1 = new List<VIP_RenewBackAduit>();
            this.VIP_RenewBackAduit_bak = new List<VIP_RenewBackAduit_bak>();
            this.VIP_RenewBackAduit_bak1 = new List<VIP_RenewBackAduit_bak>();
            this.VIP_RenewBackAduit2 = new List<VIP_RenewBackAduit>();
            this.VIP_RenewBackAduit_bak2 = new List<VIP_RenewBackAduit_bak>();
            this.VIP_VIPBackAduit = new List<VIP_VIPBackAduit>();
            this.VIP_VIPBack = new List<VIP_VIPBack>();
            this.VIP_VIPBackAduit1 = new List<VIP_VIPBackAduit>();
            this.VIP_VIPInfo_BAK = new List<VIP_VIPInfo_BAK>();
            this.VIP_VIPInfo = new List<VIP_VIPInfo>();
            this.VIP_VIPInfo1 = new List<VIP_VIPInfo>();
            this.VIP_VIPTypeService_BAK = new List<VIP_VIPTypeService_BAK>();
            this.VIP_VIPType_Bak = new List<VIP_VIPType_Bak>();
            this.VIP_VIPType = new List<VIP_VIPType>();
        }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string RealName { get; set; }
        public string UserIP { get; set; }
        public Nullable<int> DtpID { get; set; }
        public string UpdUserID { get; set; }
        public int RoleID { get; set; }
        public string Note { get; set; }
        public Nullable<bool> CanLogin { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<decimal> CancelLimit { get; set; }
        public Nullable<decimal> AduitLimit { get; set; }
        public Nullable<System.DateTime> UpTime { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<decimal> AuditOffPrice { get; set; }
        public bool IsBoss { get; set; }
        public Nullable<decimal> BrwLimit { get; set; }
        public Nullable<decimal> BorowAduitPrice { get; set; }
        public virtual ICollection<Pro_BackInfo> Pro_BackInfo { get; set; }
        public virtual ICollection<Pro_BorowAduit_bak> Pro_BorowAduit_bak { get; set; }
        public virtual ICollection<Pro_BorowAduit_bak> Pro_BorowAduit_bak1 { get; set; }
        public virtual ICollection<Pro_BorowAduit_bak> Pro_BorowAduit_bak2 { get; set; }
        public virtual ICollection<Pro_BorowAduit_bak> Pro_BorowAduit_bak3 { get; set; }
        public virtual ICollection<Pro_BorowInfo> Pro_BorowInfo { get; set; }
        public virtual ICollection<Pro_ChangeProInfo> Pro_ChangeProInfo { get; set; }
        public virtual ICollection<Pro_InOrder> Pro_InOrder { get; set; }
        public virtual ICollection<Pro_OutInfo> Pro_OutInfo { get; set; }
        public virtual ICollection<Pro_OutInfo> Pro_OutInfo1 { get; set; }
        public virtual ICollection<Pro_OutInfo> Pro_OutInfo2 { get; set; }
        public virtual ICollection<Pro_PriceChange> Pro_PriceChange { get; set; }
        public virtual ICollection<Pro_RepairInfo> Pro_RepairInfo { get; set; }
        public virtual ICollection<Pro_ReturnInfo> Pro_ReturnInfo { get; set; }
        public virtual ICollection<Pro_SellAduit> Pro_SellAduit { get; set; }
        public virtual ICollection<Pro_SellAduit> Pro_SellAduit1 { get; set; }
        public virtual ICollection<Pro_SellAduit_bak> Pro_SellAduit_bak { get; set; }
        public virtual ICollection<Pro_SellAduit_bak> Pro_SellAduit_bak1 { get; set; }
        public virtual ICollection<Pro_SellBack> Pro_SellBack { get; set; }
        public virtual ICollection<Pro_SellBack> Pro_SellBack1 { get; set; }
        public virtual ICollection<Pro_SellBackAduit> Pro_SellBackAduit { get; set; }
        public virtual ICollection<Pro_SellBackAduit> Pro_SellBackAduit1 { get; set; }
        public virtual ICollection<Pro_SellBackAduit_bak> Pro_SellBackAduit_bak { get; set; }
        public virtual ICollection<Pro_SellBackAduit_bak> Pro_SellBackAduit_bak1 { get; set; }
        public virtual ICollection<Pro_SellBackInfo_Aduit> Pro_SellBackInfo_Aduit { get; set; }
        public virtual ICollection<Pro_SellBackInfo_Aduit> Pro_SellBackInfo_Aduit1 { get; set; }
        public virtual ICollection<Pro_SellInfo> Pro_SellInfo { get; set; }
        public virtual ICollection<Pro_SellInfo> Pro_SellInfo1 { get; set; }
        public virtual ICollection<Pro_SellInfo_Aduit> Pro_SellInfo_Aduit { get; set; }
        public virtual ICollection<Pro_SellInfo_Aduit> Pro_SellInfo_Aduit1 { get; set; }
        public virtual ICollection<Pro_SellOffAduitInfo> Pro_SellOffAduitInfo { get; set; }
        public virtual ICollection<Pro_SellOffAduitInfo> Pro_SellOffAduitInfo1 { get; set; }
        public virtual ICollection<Pro_SellOffAduitInfoList> Pro_SellOffAduitInfoList { get; set; }
        public virtual ICollection<Rules_OffList> Rules_OffList { get; set; }
        public virtual ICollection<Rules_OffList> Rules_OffList1 { get; set; }
        public virtual ICollection<SMS_SignInfo> SMS_SignInfo { get; set; }
        public virtual ICollection<SMS_SignSendPayInfo> SMS_SignSendPayInfo { get; set; }
        public virtual ICollection<SMS_SignSendPayInfo> SMS_SignSendPayInfo1 { get; set; }
        public virtual Sys_DeptInfo Sys_DeptInfo { get; set; }
        public virtual Sys_RoleInfo Sys_RoleInfo { get; set; }
        public virtual ICollection<Sys_SalaryChange> Sys_SalaryChange { get; set; }
        public virtual ICollection<Sys_UserDefaultOpenPage> Sys_UserDefaultOpenPage { get; set; }
        public virtual ICollection<VIP_OffList> VIP_OffList { get; set; }
        public virtual ICollection<Sys_UserRemindList> Sys_UserRemindList { get; set; }
        public virtual ICollection<Sys_UserOPList> Sys_UserOPList { get; set; }
        public virtual ICollection<Sys_UserOPList> Sys_UserOPList1 { get; set; }
        public virtual ICollection<VIP_CardChange> VIP_CardChange { get; set; }
        public virtual ICollection<VIP_RenewBackAduit> VIP_RenewBackAduit { get; set; }
        public virtual ICollection<VIP_RenewBackAduit> VIP_RenewBackAduit1 { get; set; }
        public virtual ICollection<VIP_RenewBackAduit_bak> VIP_RenewBackAduit_bak { get; set; }
        public virtual ICollection<VIP_RenewBackAduit_bak> VIP_RenewBackAduit_bak1 { get; set; }
        public virtual ICollection<VIP_RenewBackAduit> VIP_RenewBackAduit2 { get; set; }
        public virtual ICollection<VIP_RenewBackAduit_bak> VIP_RenewBackAduit_bak2 { get; set; }
        public virtual ICollection<VIP_VIPBackAduit> VIP_VIPBackAduit { get; set; }
        public virtual ICollection<VIP_VIPBack> VIP_VIPBack { get; set; }
        public virtual ICollection<VIP_VIPBackAduit> VIP_VIPBackAduit1 { get; set; }
        public virtual ICollection<VIP_VIPInfo_BAK> VIP_VIPInfo_BAK { get; set; }
        public virtual ICollection<VIP_VIPInfo> VIP_VIPInfo { get; set; }
        public virtual ICollection<VIP_VIPInfo> VIP_VIPInfo1 { get; set; }
        public virtual ICollection<VIP_VIPTypeService_BAK> VIP_VIPTypeService_BAK { get; set; }
        public virtual ICollection<VIP_VIPType_Bak> VIP_VIPType_Bak { get; set; }
        public virtual ICollection<VIP_VIPType> VIP_VIPType { get; set; }
    }
}
