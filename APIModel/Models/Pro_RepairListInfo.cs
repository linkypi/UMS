using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_RepairListInfo
    {
        public Pro_RepairListInfo()
        {
            this.Pro_IMEI = new List<Pro_IMEI>();
            this.Pro_RepairReturnListInfo = new List<Pro_RepairReturnListInfo>();
        }

        public int RepairListID { get; set; }
        public Nullable<int> RepairID { get; set; }
        public string ProID { get; set; }
        public string InListID { get; set; }
        public string Note { get; set; }
        public decimal ProCount { get; set; }
        public string IMEI { get; set; }
        public virtual ICollection<Pro_IMEI> Pro_IMEI { get; set; }
        public virtual Pro_RepairInfo Pro_RepairInfo { get; set; }
        public virtual ICollection<Pro_RepairReturnListInfo> Pro_RepairReturnListInfo { get; set; }
    }
}
