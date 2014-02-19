using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellBackInfo_Aduit
    {
        public Pro_SellBackInfo_Aduit()
        {
            this.Pro_SellBackList = new List<Pro_SellBackList>();
            this.Pro_SellSpecalOffList = new List<Pro_SellSpecalOffList>();
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
        }

        public int ID { get; set; }
        public string SellBackID { get; set; }
        public Nullable<int> SellID { get; set; }
        public string UserID { get; set; }
        public string UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public bool Aduited { get; set; }
        public string AduitID { get; set; }
        public decimal BackMoney { get; set; }
        public Nullable<int> BackID { get; set; }
        public Nullable<int> OffTicketID { get; set; }
        public decimal OffTicketPrice { get; set; }
        public decimal CashTotle { get; set; }
        public Nullable<int> BackOffTicketID { get; set; }
        public decimal BackOffTicketPrice { get; set; }
        public decimal CardPay { get; set; }
        public decimal CashPay { get; set; }
        public decimal OldCashTotle { get; set; }
        public string BillID { get; set; }
        public Nullable<decimal> ShouldBackCash { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public string CusVIPCardID { get; set; }
        public decimal NewCashTotle { get; set; }
        public Nullable<int> OffAduit { get; set; }
        public virtual ICollection<Pro_SellBackList> Pro_SellBackList { get; set; }
        public virtual Pro_SellOffAduitInfo Pro_SellOffAduitInfo { get; set; }
        public virtual ICollection<Pro_SellSpecalOffList> Pro_SellSpecalOffList { get; set; }
        public virtual Pro_SellInfo Pro_SellInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
    }
}
