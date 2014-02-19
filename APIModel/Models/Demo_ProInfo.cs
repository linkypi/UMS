using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Demo_ProInfo
    {
        public int ProID { get; set; }
        public string ProClass { get; set; }
        public string ProType { get; set; }
        public string ProName { get; set; }
        public decimal ProPrice { get; set; }
    }
}
