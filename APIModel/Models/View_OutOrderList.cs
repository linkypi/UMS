using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_OutOrderList
    {
        public Nullable<int> OutID { get; set; }
        public string InListID { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string Note { get; set; }
        public string ProID { get; set; }
        public string ProName { get; set; }
        public string TypeName { get; set; }
        public string ClassName { get; set; }
        public string Aduit { get; set; }
        public string FromHallName { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string Pro_HallName { get; set; }
        public string UserName { get; set; }
        public string DeleterName { get; set; }
        public string OldID { get; set; }
        public string OutDate { get; set; }
        public string OutOrderID { get; set; }
        public string ProFormat { get; set; }
        public Nullable<bool> NeedIMEI { get; set; }
        public int OutListID { get; set; }
    }
}
