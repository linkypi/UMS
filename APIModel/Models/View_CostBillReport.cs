using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_CostBillReport
    {
        public string ProName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ProFormat { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public Nullable<decimal> OldCostPrice { get; set; }
        public Nullable<decimal> NewCostPrice { get; set; }
        public string Note { get; set; }
        public string InListID { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
    }
}
