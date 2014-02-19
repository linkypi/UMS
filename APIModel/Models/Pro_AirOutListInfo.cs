using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_AirOutListInfo
    {
        public int ListID { get; set; }
        public string InListID { get; set; }
        public string OldProID { get; set; }
        public string NewProID { get; set; }
        public decimal ProCount { get; set; }
        public Nullable<int> AirOutID { get; set; }
        public string Note { get; set; }
        public string NewInListID { get; set; }
        public virtual Pro_AirOutInfo Pro_AirOutInfo { get; set; }
    }
}
