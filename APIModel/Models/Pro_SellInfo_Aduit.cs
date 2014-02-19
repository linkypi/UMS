using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellInfo_Aduit
    {
        public Pro_SellInfo_Aduit()
        {
            this.Pro_SellSpecalOffList = new List<Pro_SellSpecalOffList>();
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
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
        public Nullable<int> OffAduit { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Pro_SellOffAduitInfo Pro_SellOffAduitInfo { get; set; }
        public virtual ICollection<Pro_SellSpecalOffList> Pro_SellSpecalOffList { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
        public virtual VIP_OffTicket VIP_OffTicket { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
    }
}
