using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_VIPBackApply
    {
        public string MemberName { get; set; }
        public string MobiPhone { get; set; }
        public Nullable<int> IDCard_ID { get; set; }
        public string IDCard { get; set; }
        public Nullable<int> Validity { get; set; }
        public string StartTime { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<decimal> Point { get; set; }
        public string IMEI { get; set; }
        public string CardTypeName { get; set; }
        public string VIPType { get; set; }
        public int ID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Seller { get; set; }
        public string Applyed { get; set; }
    }
}
