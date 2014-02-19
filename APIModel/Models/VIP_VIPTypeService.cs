using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPTypeService
    {
        public VIP_VIPTypeService()
        {
            this.VIP_VIPTypeService_BAK = new List<VIP_VIPTypeService_BAK>();
        }

        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public decimal SCount { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual VIP_VIPType VIP_VIPType { get; set; }
        public virtual ICollection<VIP_VIPTypeService_BAK> VIP_VIPTypeService_BAK { get; set; }
    }
}
