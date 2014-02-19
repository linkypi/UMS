using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPTypeOffLIst
    {
        public int ID { get; set; }
        public int OffID { get; set; }
        public Nullable<int> VIPType { get; set; }
        public string Note { get; set; }
        public Nullable<int> TempOffID { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
        public virtual VIP_OffListAduit VIP_OffListAduit { get; set; }
        public virtual VIP_VIPType VIP_VIPType { get; set; }
    }
}
