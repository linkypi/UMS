using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_OutSearch
    {
        public string FromHallID { get; set; }
        public int ID { get; set; }
        public string Pro_HallID { get; set; }
        public string UserID { get; set; }
        public string OutOrderID { get; set; }
        public string OldID { get; set; }
        public string OutDate { get; set; }
        public string FromUserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Note { get; set; }
        public string ToUserID { get; set; }
        public string NewToDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string Deleter { get; set; }
        public string Aduit { get; set; }
        public Nullable<System.DateTime> CancelDate { get; set; }
        public string FromHallName { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string Pro_HallName { get; set; }
        public string UserName { get; set; }
        public string DeleterName { get; set; }
        public string IMEI { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string ProName { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> ClassID { get; set; }
    }
}
