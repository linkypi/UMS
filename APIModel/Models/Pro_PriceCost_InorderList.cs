using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_PriceCost_InorderList
    {
        public int ID { get; set; }
        public Nullable<int> CostChangeID { get; set; }
        public string InListID { get; set; }
        public Nullable<decimal> OldCost { get; set; }
        public Nullable<decimal> NewCost { get; set; }
        public string ProID { get; set; }
        public Nullable<decimal> OldRetailPrice { get; set; }
        public Nullable<decimal> NewRetailPrice { get; set; }
        public virtual Pro_PriceCostChange Pro_PriceCostChange { get; set; }
    }
}
