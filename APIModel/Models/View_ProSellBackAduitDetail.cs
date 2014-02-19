using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_ProSellBackAduitDetail
    {
        public string ProName { get; set; }
        public string ProFormat { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public decimal ProCount { get; set; }
        public decimal ProPrice { get; set; }
        public decimal OffPrice { get; set; }
        public decimal OffSepecialPrice { get; set; }
        public decimal OtherCash { get; set; }
        public string IMEI { get; set; }
        public string TicketID { get; set; }
        public decimal TicketUsed { get; set; }
        public Nullable<int> SellID { get; set; }
        public decimal BackCount { get; set; }
        public decimal BackPrice { get; set; }
        public decimal AduitBackPrice { get; set; }
        public Nullable<int> ID { get; set; }
        public Nullable<bool> Aduited { get; set; }
        public Nullable<int> BackID { get; set; }
        public string InListID { get; set; }
        public decimal CashTicket { get; set; }
        public decimal CashPrice { get; set; }
        public Nullable<decimal> AnBu { get; set; }
        public Nullable<decimal> LieShouPrice { get; set; }
        public int SellListID { get; set; }
        public Nullable<int> AduitID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string HallID { get; set; }
    }
}
