using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_RepairInfo
    {
        public Pro_RepairInfo()
        {
            this.Pro_RepairListInfo = new List<Pro_RepairListInfo>();
            this.Pro_RepairReturnInfo = new List<Pro_RepairReturnInfo>();
            this.Pro_RepairReturnInfo_BAK = new List<Pro_RepairReturnInfo_BAK>();
        }

        public int ID { get; set; }
        public string HallID { get; set; }
        public string RepairID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> RepairDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsReturn { get; set; }
        public Nullable<bool> IsReceive { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string Deleter { get; set; }
        public string Receiver { get; set; }
        public Nullable<System.DateTime> RecvTime { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual ICollection<Pro_RepairListInfo> Pro_RepairListInfo { get; set; }
        public virtual ICollection<Pro_RepairReturnInfo> Pro_RepairReturnInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual ICollection<Pro_RepairReturnInfo_BAK> Pro_RepairReturnInfo_BAK { get; set; }
    }
}
