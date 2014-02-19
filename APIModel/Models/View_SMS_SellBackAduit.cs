using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_SMS_SellBackAduit
    {
        public string HallName { get; set; }
        public string AduitUser { get; set; }
        public Nullable<System.DateTime> AduitDate { get; set; }
        public string AduitNote { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string ApplyUser { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public string Passed { get; set; }
        public string Aduited { get; set; }
        public string Used { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Note { get; set; }
        public string HallID { get; set; }
        public string AduitID { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<decimal> ApplyMoney { get; set; }
        public Nullable<decimal> ApplyCount { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public int ID { get; set; }
        public Nullable<int> SignID { get; set; }
        public bool IsDelete { get; set; }
    }
}
