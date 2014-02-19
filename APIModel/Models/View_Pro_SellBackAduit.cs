using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Pro_SellBackAduit
    {
        public Nullable<int> SellID { get; set; }
        public int ID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string ApplyDate { get; set; }
        public string Aduited { get; set; }
        public string Passed { get; set; }
        public string Used { get; set; }
        public string UseDate { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> AduitMoney { get; set; }
        public string AduitID { get; set; }
        public string HallID { get; set; }
        public string AduitDate { get; set; }
        public string AuditID { get; set; }
        public string SellIDS { get; set; }
        public string SellDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SellSysDate { get; set; }
        public string OldID { get; set; }
        public string SellNote { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public Nullable<decimal> CardPay { get; set; }
        public Nullable<decimal> CashPay { get; set; }
        public Nullable<int> OffID { get; set; }
        public Nullable<int> SpecalOffID { get; set; }
        public Nullable<int> OffTicketID { get; set; }
        public Nullable<decimal> OffTicketPrice { get; set; }
        public Nullable<decimal> CashTotle { get; set; }
        public int SID { get; set; }
        public string HallName { get; set; }
        public string ApplyUser { get; set; }
        public string UserName { get; set; }
        public string Seller { get; set; }
        public string AduitUser { get; set; }
        public Nullable<decimal> ApplyMoney { get; set; }
        public string IMEI { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public bool HasUsed { get; set; }
        public bool HasAduited { get; set; }
        public bool HasPassed { get; set; }
    }
}
