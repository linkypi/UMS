using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Pro_SellInfo
    {
        public string AuditID { get; set; }
        public string SellID { get; set; }
        public string Seller { get; set; }
        public string SellDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string OldID { get; set; }
        public string Note { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public Nullable<decimal> CardPay { get; set; }
        public Nullable<decimal> CashPay { get; set; }
        public Nullable<int> OffID { get; set; }
        public Nullable<int> SpecalOffID { get; set; }
        public Nullable<int> OffTicketID { get; set; }
        public Nullable<decimal> OffTicketPrice { get; set; }
        public Nullable<decimal> CashTotle { get; set; }
        public int ID { get; set; }
        public string HallName { get; set; }
        public string UserName { get; set; }
        public string HallID { get; set; }
        public string Applyed { get; set; }
    }
}
