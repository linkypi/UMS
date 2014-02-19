using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_PriceBillReport
    {
        public string ProID { get; set; }
        public string ProName { get; set; }
        public string ProFormat { get; set; }
        public string TypeName { get; set; }
        public string ClassName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> LowPrice { get; set; }
        public Nullable<decimal> MinPrice { get; set; }
        public Nullable<decimal> MaxPrice { get; set; }
        public string IsTicketUseful { get; set; }
        public string SellTypeName { get; set; }
    }
}
