using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPType
    {
        public VIP_VIPType()
        {
            this.Pro_ProInfo = new List<Pro_ProInfo>();
            this.VIP_VIPInfo = new List<VIP_VIPInfo>();
            this.VIP_VIPTypeOffLIst = new List<VIP_VIPTypeOffLIst>();
            this.VIP_VIPTypeService_BAK = new List<VIP_VIPTypeService_BAK>();
            this.VIP_VIPTypeService = new List<VIP_VIPTypeService>();
            this.VIP_VIPType_Bak = new List<VIP_VIPType_Bak>();
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
        public virtual ICollection<Pro_ProInfo> Pro_ProInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual ICollection<VIP_VIPInfo> VIP_VIPInfo { get; set; }
        public virtual ICollection<VIP_VIPTypeOffLIst> VIP_VIPTypeOffLIst { get; set; }
        public virtual ICollection<VIP_VIPTypeService_BAK> VIP_VIPTypeService_BAK { get; set; }
        public virtual ICollection<VIP_VIPTypeService> VIP_VIPTypeService { get; set; }
        public virtual ICollection<VIP_VIPType_Bak> VIP_VIPType_Bak { get; set; }
    }
}
