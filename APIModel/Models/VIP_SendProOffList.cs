using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_SendProOffList
    {
        public VIP_SendProOffList()
        {
            this.Pro_SellSendInfo = new List<Pro_SellSendInfo>();
        }

        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> OffID { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public Nullable<decimal> LimitCount { get; set; }
        public Nullable<decimal> PerCount { get; set; }
        public Nullable<decimal> ProCost { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual ICollection<Pro_SellSendInfo> Pro_SellSendInfo { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
    }
}
