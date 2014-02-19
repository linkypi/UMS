using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_VIP_Renew
    {
        public Nullable<int> VIPID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string MemberName { get; set; }
        public string Sex { get; set; }
        public string MobiPhone { get; set; }
        public string TelePhone { get; set; }
        public string IDCard { get; set; }
        public Nullable<int> CurrentValidity { get; set; }
        public string StartTime { get; set; }
        public Nullable<decimal> CurrentPoint { get; set; }
        public string Seller { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string IMEI { get; set; }
        public string HallName { get; set; }
        public string CardTypeName { get; set; }
        public Nullable<int> IDCard_ID { get; set; }
        public Nullable<decimal> RenewMoney { get; set; }
        public Nullable<int> Validity { get; set; }
        public string RenewTypeName { get; set; }
        public string RenewTypeClassName { get; set; }
        public string Note { get; set; }
        public string OldID { get; set; }
        public string RenewDate { get; set; }
        public Nullable<decimal> Point { get; set; }
        public Nullable<decimal> RenewValue1 { get; set; }
        public Nullable<decimal> RenewValue2 { get; set; }
        public int RenewID { get; set; }
        public string State { get; set; }
        public string VIPType { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
    }
}
