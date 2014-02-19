using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Pro_ChangeList
    {
        public string ProName { get; set; }
        public string TypeName { get; set; }
        public string ClassName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> OldPrice { get; set; }
        public Nullable<decimal> LowPrice { get; set; }
        public Nullable<decimal> OldLowPrice { get; set; }
        public Nullable<decimal> MinPrice { get; set; }
        public Nullable<decimal> OldMinPrice { get; set; }
        public Nullable<decimal> MaxPrice { get; set; }
        public Nullable<decimal> OldMaxPrice { get; set; }
        public Nullable<bool> IsTicketUseful { get; set; }
        public Nullable<bool> OldIsTicketUseful { get; set; }
        public Nullable<decimal> ProCost { get; set; }
        public Nullable<decimal> OldProCost { get; set; }
        public string ProID { get; set; }
        public string HasPrice { get; set; }
        public string ID { get; set; }
        public Nullable<int> SellTypeID { get; set; }
        public Nullable<bool> UpdateFlag { get; set; }
        public string ProFormat { get; set; }
        public string ClassID { get; set; }
        public string SellTypeName { get; set; }
        public int STPID { get; set; }
        public string TypeID { get; set; }
    }
}
