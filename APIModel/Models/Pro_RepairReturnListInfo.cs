using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_RepairReturnListInfo
    {
        public int RepairReturnListID { get; set; }
        public Nullable<int> RepairReturnID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> RepairListID { get; set; }
        public string InListID { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string OLD_IMEI { get; set; }
        public string NEW_IMEI { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_RepairListInfo Pro_RepairListInfo { get; set; }
        public virtual Pro_RepairReturnInfo Pro_RepairReturnInfo { get; set; }
    }
}
