using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_OffListAduit
    {
        public int OffID { get; set; }
        public string OffName { get; set; }
        public Nullable<decimal> OffMoney { get; set; }
        public Nullable<decimal> OffRate { get; set; }
        public Nullable<decimal> OffPoint { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
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
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool Flag { get; set; }
        public decimal UseLimit { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string AduitUser2 { get; set; }
        public string AduitUser1 { get; set; }
        public string Aduited { get; set; }
        public string Passed { get; set; }
        public string Aduited1 { get; set; }
        public string Passed1 { get; set; }
        public string Aduited2 { get; set; }
        public string Passed2 { get; set; }
        public string Name { get; set; }
        public string AduitNote2 { get; set; }
        public string AduitNote1 { get; set; }
        public string AduitDate2 { get; set; }
        public string AduitDate1 { get; set; }
        public bool IsDelete { get; set; }
        public string ApplyNote { get; set; }
    }
}
