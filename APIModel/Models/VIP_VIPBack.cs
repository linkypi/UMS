using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPBack
    {
        public int ID { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public Nullable<decimal> Return_Money { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public string AduitID { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
    }
}
