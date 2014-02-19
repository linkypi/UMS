using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPBackAduit
    {
        public string ID { get; set; }
        public string AduitID { get; set; }
        public string AduitUser { get; set; }
        public Nullable<System.DateTime> AduitDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string ApplyUser { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public Nullable<bool> Aduited { get; set; }
        public Nullable<bool> Passed { get; set; }
        public Nullable<bool> Used { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> Money { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public string HallID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
    }
}
