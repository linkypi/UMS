using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_UserOPList
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string UpdUserID { get; set; }
        public Nullable<int> OpID { get; set; }
        public string HallID { get; set; }
        public string Note { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<System.DateTime> LeaveDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual Sys_UserOp Sys_UserOp { get; set; }
    }
}
