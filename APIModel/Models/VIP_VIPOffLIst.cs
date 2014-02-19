using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPOffLIst
    {
        public int ID { get; set; }
        public Nullable<int> OffID { get; set; }
        public Nullable<int> VIPID { get; set; }
        public string Note { get; set; }
        public Nullable<int> TempOffID { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
        public virtual VIP_OffListAduit VIP_OffListAduit { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
    }
}
