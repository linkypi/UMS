using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Rules_SellTypeInfo
    {
        public int ID { get; set; }
        public Nullable<int> RulesID { get; set; }
        public Nullable<int> SellType { get; set; }
        public virtual Pro_SellType Pro_SellType { get; set; }
        public virtual Rules_OffList Rules_OffList { get; set; }
    }
}
