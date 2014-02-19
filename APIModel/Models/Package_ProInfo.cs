using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Package_ProInfo
    {
        public int ID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public Nullable<int> ProMainNameID { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<int> SellType { get; set; }
        public string Note { get; set; }
        public virtual Package_GroupInfo Package_GroupInfo { get; set; }
    }
}
