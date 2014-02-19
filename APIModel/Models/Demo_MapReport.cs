using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Demo_MapReport
    {
        public string HallID { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Sells { get; set; }
        public decimal SellPrice { get; set; }
        public int ID { get; set; }
        public int ProClass { get; set; }
        public decimal Profit { get; set; }
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public string HallName { get; set; }
        public string TypeName { get; set; }
    }
}
