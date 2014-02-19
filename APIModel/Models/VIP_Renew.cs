using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_Renew
    {
        public VIP_Renew()
        {
            this.VIP_RenewBack = new List<VIP_RenewBack>();
            this.VIP_RenewBackAduit = new List<VIP_RenewBackAduit>();
            this.VIP_RenewBackAduit_bak = new List<VIP_RenewBackAduit_bak>();
        }

        public int ID { get; set; }
        public Nullable<int> VIP_ID { get; set; }
        public Nullable<decimal> RenewMoney { get; set; }
        public Nullable<int> Validity { get; set; }
        public string RenewTypeName { get; set; }
        public string RenewTypeClassName { get; set; }
        public Nullable<decimal> RenewValue1 { get; set; }
        public Nullable<decimal> RenewValue2 { get; set; }
        public string Note { get; set; }
        public string OldID { get; set; }
        public Nullable<int> AduitID { get; set; }
        public Nullable<System.DateTime> RenewDate { get; set; }
        public Nullable<decimal> Point { get; set; }
        public string HallID { get; set; }
        public Nullable<System.DateTime> OldEndDate { get; set; }
        public string Seller { get; set; }
        public virtual ICollection<VIP_RenewBack> VIP_RenewBack { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
        public virtual ICollection<VIP_RenewBackAduit> VIP_RenewBackAduit { get; set; }
        public virtual ICollection<VIP_RenewBackAduit_bak> VIP_RenewBackAduit_bak { get; set; }
    }
}
