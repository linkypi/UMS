using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_BorowReturnDetail
    {
        public string ProName { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string InListID { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public string ProFormat { get; set; }
        public string IsReturn { get; set; }
        public int BorowListID { get; set; }
        public Nullable<int> BorowID { get; set; }
        public string ProID { get; set; }
        public Nullable<bool> NeedIMEI { get; set; }
        public string BID { get; set; }
        public decimal RetCount { get; set; }
        public decimal UnRetCount { get; set; }
        public string HallID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
    }
}
