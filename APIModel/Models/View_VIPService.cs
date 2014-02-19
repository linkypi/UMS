using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_VIPService
    {
        public Nullable<decimal> SCount { get; set; }
        public Nullable<decimal> UsedCount { get; set; }
        public string ProID { get; set; }
        public int ID { get; set; }
        public string ProName { get; set; }
        public string ProFormat { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> VIPID { get; set; }
    }
}
