using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellAduit_bak
    {
        public int ID { get; set; }
        public string AduitID { get; set; }
        public string AduitUser { get; set; }
        public Nullable<System.DateTime> AduitDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string ApplyUser { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public bool Aduited { get; set; }
        public bool Passed { get; set; }
        public bool Used { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Note { get; set; }
        public decimal Money { get; set; }
        public string HallID { get; set; }
        public string CustName { get; set; }
        public string CustPhone { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
    }
}
