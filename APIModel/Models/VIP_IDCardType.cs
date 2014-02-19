using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_IDCardType
    {
        public VIP_IDCardType()
        {
            this.VIP_VIPInfo = new List<VIP_VIPInfo>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Flag { get; set; }
        public string Note { get; set; }
        public virtual ICollection<VIP_VIPInfo> VIP_VIPInfo { get; set; }
    }
}
