using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Package_SalesNameInfo
    {
        public Package_SalesNameInfo()
        {
            this.VIP_OffList = new List<VIP_OffList>();
        }

        public int ID { get; set; }
        public string SalesName { get; set; }
        public string Note { get; set; }
        public Nullable<int> Parent { get; set; }
        public virtual ICollection<VIP_OffList> VIP_OffList { get; set; }
    }
}
