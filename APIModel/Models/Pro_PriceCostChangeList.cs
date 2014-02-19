using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_PriceCostChangeList
    {
        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> ChangeID { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> OldCostPrice { get; set; }
        public Nullable<decimal> NewCostPrice { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<decimal> NewRetailPrice { get; set; }
        public Nullable<decimal> OldRetailPrice { get; set; }
        public virtual Pro_PriceCostChange Pro_PriceCostChange { get; set; }
    }
}
