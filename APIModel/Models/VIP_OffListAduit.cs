using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_OffListAduit
    {
        public VIP_OffListAduit()
        {
            this.Package_GroupInfo = new List<Package_GroupInfo>();
            this.VIP_VIPTypeOffLIst = new List<VIP_VIPTypeOffLIst>();
            this.VIP_VIPOffLIst = new List<VIP_VIPOffLIst>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public decimal ArriveMoney { get; set; }
        public decimal OffMoney { get; set; }
        public decimal ArriveCount { get; set; }
        public decimal OffRate { get; set; }
        public decimal OffPoint { get; set; }
        public decimal OffPointMoney { get; set; }
        public decimal MaxPoint { get; set; }
        public decimal MinPoint { get; set; }
        public decimal SendPoint { get; set; }
        public bool HaveTop { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdUser { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool Flag { get; set; }
        public decimal UseLimit { get; set; }
        public bool SendTicket { get; set; }
        public Nullable<int> VIPTicketMaxCount { get; set; }
        public string discountPic { get; set; }
        public string discountSynopsis { get; set; }
        public string discountInfo { get; set; }
        public Nullable<bool> UnOver { get; set; }
        public string discountPicbigid__ { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public int HeadID { get; set; }
        public virtual ICollection<Package_GroupInfo> Package_GroupInfo { get; set; }
        public virtual ICollection<VIP_VIPTypeOffLIst> VIP_VIPTypeOffLIst { get; set; }
        public virtual VIP_OffListAduitHeader VIP_OffListAduitHeader { get; set; }
        public virtual ICollection<VIP_VIPOffLIst> VIP_VIPOffLIst { get; set; }
    }
}
