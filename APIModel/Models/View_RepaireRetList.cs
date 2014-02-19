using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_RepaireRetList
    {
        public string IMEI { get; set; }
        public decimal ProCount { get; set; }
        public string Note { get; set; }
        public string InListID { get; set; }
        public string ProID { get; set; }
        public string ProName { get; set; }
        public bool NeedIMEI { get; set; }
        public bool ISdecimals { get; set; }
        public string ProFormat { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public decimal BackCount { get; set; }
        public int RepairListID { get; set; }
        public Nullable<int> RepairID { get; set; }
        public Nullable<decimal> AduitCount { get; set; }
        public string IsReturn { get; set; }
    }
}
