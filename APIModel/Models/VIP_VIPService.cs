using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPService
    {
        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> VIPID { get; set; }
        public decimal SCount { get; set; }
        public decimal UsedCount { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
        public virtual VIP_VIPInfo_Temp VIP_VIPInfo_Temp { get; set; }
    }
}
