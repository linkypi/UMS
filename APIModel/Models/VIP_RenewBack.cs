using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_RenewBack
    {
        public int ID { get; set; }
        public Nullable<int> Old_Renew_ID { get; set; }
        public Nullable<int> AduitID { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> Money { get; set; }
        public Nullable<decimal> Point { get; set; }
        public Nullable<int> Validity { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public Nullable<System.DateTime> NewDate { get; set; }
        public virtual VIP_Renew VIP_Renew { get; set; }
        public virtual VIP_RenewBackAduit VIP_RenewBackAduit { get; set; }
    }
}
