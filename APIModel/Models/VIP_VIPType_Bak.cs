using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPType_Bak
    {
        public VIP_VIPType_Bak()
        {
            this.VIP_VIPTypeService_BAK = new List<VIP_VIPTypeService_BAK>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Flag { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> Cost_production { get; set; }
        public Nullable<decimal> SPoint { get; set; }
        public Nullable<decimal> SBalance { get; set; }
        public Nullable<int> Validity { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string UpdUser { get; set; }
        public Nullable<int> OldID { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual VIP_VIPType VIP_VIPType { get; set; }
        public virtual VIP_VIPType_Bak VIP_VIPType_Bak1 { get; set; }
        public virtual VIP_VIPType_Bak VIP_VIPType_Bak2 { get; set; }
        public virtual ICollection<VIP_VIPTypeService_BAK> VIP_VIPTypeService_BAK { get; set; }
    }
}
