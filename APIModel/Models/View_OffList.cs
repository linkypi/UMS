using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_OffList
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
        public int Type { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdUser { get; set; }
        public string RealName { get; set; }
        public string Note { get; set; }
        public decimal ArriveMoney { get; set; }
        public string SalesName { get; set; }
    }
}
