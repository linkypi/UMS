using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Package_GroupInfo
    {
        public Nullable<int> SellTypeID { get; set; }
        public Nullable<bool> IsMust { get; set; }
        public string Note { get; set; }
        public string GroupName { get; set; }
        public int ID { get; set; }
        public Nullable<int> TempOffID { get; set; }
        public string SellTypeName { get; set; }
        public string SubNote { get; set; }
    }
}
