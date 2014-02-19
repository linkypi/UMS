using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_SellTypeProduct
    {
        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> SellType { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> LowPrice { get; set; }
        public Nullable<decimal> MinPrice { get; set; }
        public Nullable<decimal> MaxPrice { get; set; }
        public bool IsTicketUseful { get; set; }
        public bool IsAduit { get; set; }
        public Nullable<decimal> ProCost { get; set; }
        public Nullable<bool> UpdateFlag { get; set; }
        public string ProName { get; set; }
        public string ClassName { get; set; }
        public string ClassID { get; set; }
        public string TypeID { get; set; }
        public string TypeName { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> NewPrice { get; set; }
        public string ProFormat { get; set; }
        public bool ISdecimals { get; set; }
    }
}
