using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_RepairReturnInfo
    {
        public Pro_RepairReturnInfo()
        {
            this.Pro_RepairReturnListInfo = new List<Pro_RepairReturnListInfo>();
        }

        public int ID { get; set; }
        public string HallID { get; set; }
        public string RepairReturnID { get; set; }
        public Nullable<int> RepairID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> RepairReturnDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string Deleter { get; set; }
        public Nullable<bool> IsReceived { get; set; }
        public string Receiver { get; set; }
        public Nullable<System.DateTime> RecvTime { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Pro_RepairInfo Pro_RepairInfo { get; set; }
        public virtual ICollection<Pro_RepairReturnListInfo> Pro_RepairReturnListInfo { get; set; }
        public virtual Pro_RepairReturnInfo Pro_RepairReturnInfo1 { get; set; }
        public virtual Pro_RepairReturnInfo Pro_RepairReturnInfo2 { get; set; }
    }
}
