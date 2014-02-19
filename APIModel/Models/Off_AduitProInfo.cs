using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Off_AduitProInfo
    {
        public int ID { get; set; }
        public int ProMainID { get; set; }
        public int SellType { get; set; }
        public int AduitTypeID { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> Price { get; set; }
        public virtual Off_AduitTypeInfo Off_AduitTypeInfo { get; set; }
        public virtual Pro_SellType Pro_SellType { get; set; }
        public virtual Pro_ProMainInfo Pro_ProMainInfo { get; set; }
    }
}
