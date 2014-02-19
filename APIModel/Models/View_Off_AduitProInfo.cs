using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Off_AduitProInfo
    {
        public int ProMainID { get; set; }
        public int SellType { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string ClassName { get; set; }
        public int AduitTypeID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string ProMainName { get; set; }
    }
}
