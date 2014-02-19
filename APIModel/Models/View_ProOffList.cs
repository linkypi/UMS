using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_ProOffList
    {
        public int OffID { get; set; }
        public string OffName { get; set; }
        public Nullable<decimal> OffMoney { get; set; }
        public Nullable<decimal> OffRate { get; set; }
        public Nullable<decimal> OffPoint { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool OffFlag { get; set; }
        public Nullable<bool> UnOver { get; set; }
        public Nullable<int> VIPTicketMaxCount { get; set; }
        public string discountPic { get; set; }
        public string discountSynopsis { get; set; }
        public string discountInfo { get; set; }
        public string ProID { get; set; }
        public string OffProNote { get; set; }
        public decimal ProCount { get; set; }
        public Nullable<int> SellTypeID { get; set; }
        public Nullable<decimal> AfterOffPrice { get; set; }
        public string ProName { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public bool NeedIMEI { get; set; }
        public string ProFormat { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string OffUpdUser { get; set; }
        public string Name { get; set; }
        public int OffType { get; set; }
        public decimal SendPoint { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Point { get; set; }
        public Nullable<decimal> ProOffMoney { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Note { get; set; }
    }
}
