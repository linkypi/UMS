using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_SpecialOffListInfo
    {
        public int ID { get; set; }
        public Nullable<int> SellID { get; set; }
        public string OldID { get; set; }
        public Nullable<int> pro_classid { get; set; }
        public string classname { get; set; }
        public Nullable<int> pro_typeid { get; set; }
        public string typename { get; set; }
        public string proname { get; set; }
        public string proformat { get; set; }
        public int ProCount { get; set; }
        public Nullable<int> selltype { get; set; }
        public string SellTypeName { get; set; }
        public Nullable<int> proprice { get; set; }
        public Nullable<int> Realproprice { get; set; }
        public string IMEI { get; set; }
        public string TicketID { get; set; }
        public Nullable<int> CashTicket { get; set; }
        public Nullable<int> TicketUsed { get; set; }
        public Nullable<int> OffPoint { get; set; }
        public string OffName { get; set; }
        public Nullable<int> OffPrice { get; set; }
        public string SepecialOffName { get; set; }
        public Nullable<decimal> OffSepecialPrice { get; set; }
        public Nullable<int> WholeSaleOffPrice { get; set; }
        public Nullable<int> OtherCash { get; set; }
        public string IsFree { get; set; }
        public Nullable<decimal> CashPrice { get; set; }
        public string selldate { get; set; }
        public Nullable<int> BackID { get; set; }
    }
}
