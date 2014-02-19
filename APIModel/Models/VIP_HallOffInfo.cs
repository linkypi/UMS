using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_HallOffInfo
    {
        public int ID { get; set; }
        public string HallID { get; set; }
        public Nullable<int> OffID { get; set; }
        public Nullable<int> TempOffID { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
    }
}
