using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_VIPInfo
    {
        public int ID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string MemberName { get; set; }
        public string Sex { get; set; }
        public string Birthday { get; set; }
        public string MobiPhone { get; set; }
        public string TelePhone { get; set; }
        public string Address { get; set; }
        public Nullable<int> IDCard_ID { get; set; }
        public string IDCard { get; set; }
        public Nullable<int> Validity { get; set; }
        public string StartTime { get; set; }
        public Nullable<decimal> Point { get; set; }
        public string Seller { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string UpdUser { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public Nullable<decimal> ProPrice { get; set; }
        public string IMEI { get; set; }
        public string Password { get; set; }
        public string userName { get; set; }
        public string HallID { get; set; }
        public string OldID { get; set; }
        public string QQ { get; set; }
        public string VIPTypeName { get; set; }
        public string IDCardName { get; set; }
        public Nullable<bool> Flag { get; set; }
        public string UpdUserName { get; set; }
        public string HallName { get; set; }
        public Nullable<decimal> Cost_production { get; set; }
        public Nullable<decimal> SPoint { get; set; }
        public Nullable<decimal> SBalance { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.DateTime> NewStartTime { get; set; }
        public string LZUserName { get; set; }
        public string LZUser { get; set; }
        public string VIPNote { get; set; }
    }
}
