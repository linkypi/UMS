using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_AirOutInfo
    {
        public string FromHallName { get; set; }
        public string ToHallName { get; set; }
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
    }
}
