using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_SellOffAduitProList
    {
        public string ProID { get; set; }
        public decimal ProCount { get; set; }
        public string Note { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public string ProName { get; set; }
        public string ProFormat { get; set; }
        public Nullable<int> SellAduitID { get; set; }
        public Nullable<int> BackAduitID { get; set; }
        public decimal OtherOff { get; set; }
        public int ClassID { get; set; }
        public int TypeID { get; set; }
        public decimal ProPrice { get; set; }
        public Nullable<int> OldSellListID { get; set; }
        public Nullable<decimal> Preferent { get; set; }
    }
}
