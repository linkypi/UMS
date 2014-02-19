using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_CardChange
    {
        public int ID { get; set; }
        public Nullable<int> OLD_VIP_ID { get; set; }
        public Nullable<int> NEW_VIP_ID { get; set; }
        public string UserID { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public virtual Pro_SellInfo Pro_SellInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo1 { get; set; }
    }
}
