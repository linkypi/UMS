using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_OffTicket
    {
        public VIP_OffTicket()
        {
            this.Pro_SellInfo = new List<Pro_SellInfo>();
            this.Pro_SellInfo_Aduit = new List<Pro_SellInfo_Aduit>();
        }

        public int ID { get; set; }
        public string TicketID { get; set; }
        public Nullable<int> OffID { get; set; }
        public string Name { get; set; }
        public int VIP_ID { get; set; }
        public bool Used { get; set; }
        public bool Flag { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Source { get; set; }
        public string Note { get; set; }
        public virtual ICollection<Pro_SellInfo> Pro_SellInfo { get; set; }
        public virtual ICollection<Pro_SellInfo_Aduit> Pro_SellInfo_Aduit { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
    }
}
