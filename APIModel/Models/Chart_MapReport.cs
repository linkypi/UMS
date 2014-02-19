using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Chart_MapReport
    {
        public long ID { get; set; }
        public string HallID { get; set; }
        public string HallName { get; set; }
        public int ProClass { get; set; }
        public string TypeName { get; set; }
        public System.DateTime DATE { get; set; }
        public Nullable<decimal> Sells { get; set; }
        public Nullable<decimal> SellPrice { get; set; }
        public string AreaName { get; set; }
        public int AreaID { get; set; }
        public int Profit { get; set; }
        public string ClassTypeName { get; set; }
        public bool AsPrice { get; set; }
        public Nullable<int> BigAreaID { get; set; }
        public string BigAreaName { get; set; }
    }
}
