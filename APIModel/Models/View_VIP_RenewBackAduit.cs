using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_VIP_RenewBackAduit
    {
        public string AduitID { get; set; }
        public string AduitUser { get; set; }
        public string AduitDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string ApplyUser { get; set; }
        public string ApplyDate { get; set; }
        public string Aduited { get; set; }
        public string Passed { get; set; }
        public string Used { get; set; }
        public string UseDate { get; set; }
        public string Note { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public Nullable<int> ReNewID { get; set; }
        public string HallID { get; set; }
        public string UserID { get; set; }
        public string MemberName { get; set; }
        public string MobiPhone { get; set; }
        public int IDCard_ID { get; set; }
        public string IDCard { get; set; }
        public int CurrentValidity { get; set; }
        public string StartTime { get; set; }
        public decimal Point { get; set; }
        public string IMEI { get; set; }
        public string VIPType { get; set; }
        public string CardTypeName { get; set; }
        public string HallName { get; set; }
        public decimal RenewMoney { get; set; }
        public string RenewTypeName { get; set; }
        public int RenewValidity { get; set; }
        public decimal RenewPoint { get; set; }
        public int ID { get; set; }
        public string RenewDate { get; set; }
        public string NewEndTime { get; set; }
        public System.DateTime OldStartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Sex { get; set; }
        public decimal AduitMoney { get; set; }
        public decimal AduitPoint { get; set; }
        public int BackValidity { get; set; }
        public Nullable<bool> Flag { get; set; }
        public string OldEndDate { get; set; }
    }
}
