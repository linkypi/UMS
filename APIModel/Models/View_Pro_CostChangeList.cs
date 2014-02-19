using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Pro_CostChangeList
    {
        public int ID { get; set; }
        public Nullable<int> ChangeID { get; set; }
        public string ProID { get; set; }
        public Nullable<decimal> OldCostPrice { get; set; }
        public Nullable<decimal> NewCostPrice { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<bool> UpdateFlag { get; set; }
        public decimal RetailPrice { get; set; }
    }
}
