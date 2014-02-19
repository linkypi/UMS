using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_RenewBackAduit
    {
        public VIP_RenewBackAduit()
        {
            this.VIP_RenewBack = new List<VIP_RenewBack>();
        }

        public int ID { get; set; }
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
        public Nullable<decimal> Point { get; set; }
        public Nullable<int> Validity { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public Nullable<int> ReNewID { get; set; }
        public string HallID { get; set; }
        public Nullable<System.DateTime> NewDate { get; set; }
        public string UserID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo2 { get; set; }
        public virtual VIP_Renew VIP_Renew { get; set; }
        public virtual ICollection<VIP_RenewBack> VIP_RenewBack { get; set; }
        public virtual VIP_RenewBackAduit VIP_RenewBackAduit1 { get; set; }
        public virtual VIP_RenewBackAduit VIP_RenewBackAduit2 { get; set; }
        public virtual VIP_RenewBackAduit VIP_RenewBackAduit11 { get; set; }
        public virtual VIP_RenewBackAduit VIP_RenewBackAduit3 { get; set; }
    }
}
