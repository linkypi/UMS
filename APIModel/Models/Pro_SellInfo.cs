using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellInfo
    {
        public Pro_SellInfo()
        {
            this.Pro_IMEI = new List<Pro_IMEI>();
            this.Pro_SellBack = new List<Pro_SellBack>();
            this.Pro_SellBackAduit = new List<Pro_SellBackAduit>();
            this.Pro_SellBackAduit_bak = new List<Pro_SellBackAduit_bak>();
            this.Pro_SellBackInfo_Aduit = new List<Pro_SellBackInfo_Aduit>();
            this.Pro_SellOffAduitInfo = new List<Pro_SellOffAduitInfo>();
            this.Pro_SellSpecalOffList = new List<Pro_SellSpecalOffList>();
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
            this.Pro_SellSendInfo = new List<Pro_SellSendInfo>();
            this.SMS_SellBackAduit = new List<SMS_SellBackAduit>();
            this.VIP_CardChange = new List<VIP_CardChange>();
        }

        public int ID { get; set; }
        public string SellID { get; set; }
        public string Seller { get; set; }
        public Nullable<System.DateTime> SellDate { get; set; }
        public string OldID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public string HallID { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public decimal CardPay { get; set; }
        public decimal CashPay { get; set; }
        public Nullable<int> OffID { get; set; }
        public Nullable<int> SpecalOffID { get; set; }
        public Nullable<int> OffTicketID { get; set; }
        public decimal OffTicketPrice { get; set; }
        public decimal CashTotle { get; set; }
        public string AuditID { get; set; }
        public string BillID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual ICollection<Pro_IMEI> Pro_IMEI { get; set; }
        public virtual ICollection<Pro_SellBack> Pro_SellBack { get; set; }
        public virtual ICollection<Pro_SellBackAduit> Pro_SellBackAduit { get; set; }
        public virtual ICollection<Pro_SellBackAduit_bak> Pro_SellBackAduit_bak { get; set; }
        public virtual ICollection<Pro_SellBackInfo_Aduit> Pro_SellBackInfo_Aduit { get; set; }
        public virtual ICollection<Pro_SellOffAduitInfo> Pro_SellOffAduitInfo { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
        public virtual VIP_OffTicket VIP_OffTicket { get; set; }
        public virtual ICollection<Pro_SellSpecalOffList> Pro_SellSpecalOffList { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual ICollection<Pro_SellSendInfo> Pro_SellSendInfo { get; set; }
        public virtual ICollection<SMS_SellBackAduit> SMS_SellBackAduit { get; set; }
        public virtual ICollection<VIP_CardChange> VIP_CardChange { get; set; }
    }
}
