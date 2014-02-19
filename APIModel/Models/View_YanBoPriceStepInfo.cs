using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_YanBoPriceStepInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ProID { get; set; }
        public Nullable<decimal> ProPrice { get; set; }
        public Nullable<decimal> StepPrice { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> LowPrice { get; set; }
        public Nullable<decimal> ProCost { get; set; }
        public string ProName { get; set; }
        public string ProTypeName { get; set; }
        public string ProClassName { get; set; }
        public Nullable<bool> UpdateFlag { get; set; }
        public Nullable<decimal> OldProPrice { get; set; }
        public Nullable<decimal> OldStepPrice { get; set; }
        public Nullable<decimal> OldLowPrice { get; set; }
        public Nullable<decimal> OldProCost { get; set; }
        public string ProFormat { get; set; }
        public string OldName { get; set; }
        public bool ISdecimals { get; set; }
    }
}
