using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ChangeProInfo
    {
        public Pro_ChangeProInfo()
        {
            this.Pro_ChangeProListInfo = new List<Pro_ChangeProListInfo>();
        }

        public int ID { get; set; }
        public string HallID { get; set; }
        public string UserID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual ICollection<Pro_ChangeProListInfo> Pro_ChangeProListInfo { get; set; }
    }
}
