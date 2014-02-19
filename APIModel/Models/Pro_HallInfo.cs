using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_HallInfo
    {
        public Pro_HallInfo()
        {
            this.Off_AduitHallInfo = new List<Off_AduitHallInfo>();
            this.Pro_BackInfo = new List<Pro_BackInfo>();
            this.Pro_BorowAduit_bak = new List<Pro_BorowAduit_bak>();
            this.Pro_BorowInfo = new List<Pro_BorowInfo>();
            this.Pro_ChangeProInfo = new List<Pro_ChangeProInfo>();
            this.Pro_SellOffAduitInfo = new List<Pro_SellOffAduitInfo>();
            this.Pro_SellInfo_Aduit = new List<Pro_SellInfo_Aduit>();
            this.Pro_InOrder = new List<Pro_InOrder>();
            this.Pro_OutInfo = new List<Pro_OutInfo>();
            this.Pro_OutInfo1 = new List<Pro_OutInfo>();
            this.Pro_RepairReturnInfo = new List<Pro_RepairReturnInfo>();
            this.Pro_RepairInfo = new List<Pro_RepairInfo>();
            this.Pro_RepairReturnInfo_BAK = new List<Pro_RepairReturnInfo_BAK>();
            this.Pro_SellAduit = new List<Pro_SellAduit>();
            this.Pro_SellInfo = new List<Pro_SellInfo>();
            this.Pro_SellAduit_bak = new List<Pro_SellAduit_bak>();
            this.Pro_SellBackAduit = new List<Pro_SellBackAduit>();
            this.Pro_SellBackAduit_bak = new List<Pro_SellBackAduit_bak>();
            this.Pro_StoreInfo = new List<Pro_StoreInfo>();
            this.Rules_AllCurrentRulesInfo = new List<Rules_AllCurrentRulesInfo>();
            this.Rules_HallOffInfo = new List<Rules_HallOffInfo>();
            this.SMS_SellBackAduit = new List<SMS_SellBackAduit>();
            this.Sys_Role_Menu_HallInfo = new List<Sys_Role_Menu_HallInfo>();
            this.Sys_RoleMethod = new List<Sys_RoleMethod>();
            this.Sys_UserOPList = new List<Sys_UserOPList>();
            this.VIP_HallInfoHeader = new List<VIP_HallInfoHeader>();
            this.VIP_RenewBackAduit = new List<VIP_RenewBackAduit>();
            this.VIP_RenewBackAduit_bak = new List<VIP_RenewBackAduit_bak>();
            this.VIP_VIPBackAduit = new List<VIP_VIPBackAduit>();
            this.SMS_SignInfo = new List<SMS_SignInfo>();
        }

        public string HallID { get; set; }
        public string HallName { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<int> AreaID { get; set; }
        public Nullable<int> LevelID { get; set; }
        public Nullable<bool> CanIn { get; set; }
        public string Note { get; set; }
        public Nullable<bool> CanBack { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public string DisPlayName { get; set; }
        public string ShortName { get; set; }
        public int SellNum { get; set; }
        public string PrintName { get; set; }
        public virtual ICollection<Off_AduitHallInfo> Off_AduitHallInfo { get; set; }
        public virtual Pro_AreaInfo Pro_AreaInfo { get; set; }
        public virtual ICollection<Pro_BackInfo> Pro_BackInfo { get; set; }
        public virtual ICollection<Pro_BorowAduit_bak> Pro_BorowAduit_bak { get; set; }
        public virtual ICollection<Pro_BorowInfo> Pro_BorowInfo { get; set; }
        public virtual ICollection<Pro_ChangeProInfo> Pro_ChangeProInfo { get; set; }
        public virtual ICollection<Pro_SellOffAduitInfo> Pro_SellOffAduitInfo { get; set; }
        public virtual ICollection<Pro_SellInfo_Aduit> Pro_SellInfo_Aduit { get; set; }
        public virtual ICollection<Pro_InOrder> Pro_InOrder { get; set; }
        public virtual ICollection<Pro_OutInfo> Pro_OutInfo { get; set; }
        public virtual ICollection<Pro_OutInfo> Pro_OutInfo1 { get; set; }
        public virtual ICollection<Pro_RepairReturnInfo> Pro_RepairReturnInfo { get; set; }
        public virtual ICollection<Pro_RepairInfo> Pro_RepairInfo { get; set; }
        public virtual ICollection<Pro_RepairReturnInfo_BAK> Pro_RepairReturnInfo_BAK { get; set; }
        public virtual ICollection<Pro_SellAduit> Pro_SellAduit { get; set; }
        public virtual ICollection<Pro_SellInfo> Pro_SellInfo { get; set; }
        public virtual ICollection<Pro_SellAduit_bak> Pro_SellAduit_bak { get; set; }
        public virtual ICollection<Pro_SellBackAduit> Pro_SellBackAduit { get; set; }
        public virtual ICollection<Pro_SellBackAduit_bak> Pro_SellBackAduit_bak { get; set; }
        public virtual ICollection<Pro_StoreInfo> Pro_StoreInfo { get; set; }
        public virtual ICollection<Rules_AllCurrentRulesInfo> Rules_AllCurrentRulesInfo { get; set; }
        public virtual ICollection<Rules_HallOffInfo> Rules_HallOffInfo { get; set; }
        public virtual ICollection<SMS_SellBackAduit> SMS_SellBackAduit { get; set; }
        public virtual ICollection<Sys_Role_Menu_HallInfo> Sys_Role_Menu_HallInfo { get; set; }
        public virtual ICollection<Sys_RoleMethod> Sys_RoleMethod { get; set; }
        public virtual ICollection<Sys_UserOPList> Sys_UserOPList { get; set; }
        public virtual ICollection<VIP_HallInfoHeader> VIP_HallInfoHeader { get; set; }
        public virtual ICollection<VIP_RenewBackAduit> VIP_RenewBackAduit { get; set; }
        public virtual ICollection<VIP_RenewBackAduit_bak> VIP_RenewBackAduit_bak { get; set; }
        public virtual ICollection<VIP_VIPBackAduit> VIP_VIPBackAduit { get; set; }
        public virtual ICollection<SMS_SignInfo> SMS_SignInfo { get; set; }
    }
}
