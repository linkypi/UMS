using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_VIPBackAduit
    {
        public string AduitID { get; set; }
        public string AduitUser { get; set; }
        public string ID { get; set; }
        public string AduitDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string ApplyUser { get; set; }
        public string ApplyDate { get; set; }
        public string Aduited { get; set; }
        public string Used { get; set; }
        public string UseDate { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> Money { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public string HallID { get; set; }
        public string MemberName { get; set; }
        public string MobiPhone { get; set; }
        public string IDCard { get; set; }
        public Nullable<int> Validity { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<decimal> Point { get; set; }
        public string Seller { get; set; }
        public string IMEI { get; set; }
        public string HallName { get; set; }
        public string VIPType { get; set; }
        public string CardTypeName { get; set; }
    }
}
