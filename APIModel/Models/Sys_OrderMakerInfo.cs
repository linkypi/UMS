using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_OrderMakerInfo
    {
        public string Header { get; set; }
        public string OrderDate { get; set; }
        public Nullable<int> OrderNO { get; set; }
        public string OrderType { get; set; }
        public string Introduction { get; set; }
    }
}
