using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_AirOutListModel
    {
        public string FromHallName { get; set; }
        public string ToHallName { get; set; }
        public string InListID { get; set; }
        public string NewInListID { get; set; }
        public decimal ProCount { get; set; }
        public string NewProName { get; set; }
        public string NewTypeName { get; set; }
        public string NewClassName { get; set; }
        public string NewProFormat { get; set; }
        public string ProName { get; set; }
        public string TypeName { get; set; }
        public string ClassName { get; set; }
        public string ProFormat { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Audit { get; set; }
        public int ID { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string FromHallID { get; set; }
        public string Pro_HallID { get; set; }
        public string Note { get; set; }
        public string OutOrderID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> OutDate { get; set; }
        public string OutTime { get; set; }
        public string NewToDate { get; set; }
        public string FromUserID { get; set; }
        public string ToUserID { get; set; }
    }
}
