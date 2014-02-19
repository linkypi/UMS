using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_BackInfo
    {
        public Pro_BackInfo()
        {
            this.Pro_BackListInfo = new List<Pro_BackListInfo>();
        }

        public int ID { get; set; }
        public string HallID { get; set; }
        public string BackID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> BackDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public virtual ICollection<Pro_BackListInfo> Pro_BackListInfo { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
    }
}
