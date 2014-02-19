using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_OffList
    {
        public VIP_OffList()
        {
            this.Package_GroupInfo = new List<Package_GroupInfo>();
            this.Pro_SellBackList = new List<Pro_SellBackList>();
            this.Pro_SellBackSpecalOffList = new List<Pro_SellBackSpecalOffList>();
            this.Pro_SellInfo = new List<Pro_SellInfo>();
            this.Pro_SellInfo_Aduit = new List<Pro_SellInfo_Aduit>();
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
            this.Pro_SellSpecalOffList = new List<Pro_SellSpecalOffList>();
            this.VIP_HallOffInfo = new List<VIP_HallOffInfo>();
            this.VIP_OffTicket = new List<VIP_OffTicket>();
            this.VIP_ProOffList = new List<VIP_ProOffList>();
            this.VIP_SendProOffList = new List<VIP_SendProOffList>();
            this.VIP_VIPOffLIst = new List<VIP_VIPOffLIst>();
            this.VIP_VIPTypeOffLIst = new List<VIP_VIPTypeOffLIst>();
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
        public virtual ICollection<Package_GroupInfo> Package_GroupInfo { get; set; }
        public virtual Package_SalesNameInfo Package_SalesNameInfo { get; set; }
        public virtual ICollection<Pro_SellBackList> Pro_SellBackList { get; set; }
        public virtual ICollection<Pro_SellBackSpecalOffList> Pro_SellBackSpecalOffList { get; set; }
        public virtual ICollection<Pro_SellInfo> Pro_SellInfo { get; set; }
        public virtual ICollection<Pro_SellInfo_Aduit> Pro_SellInfo_Aduit { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
        public virtual ICollection<Pro_SellSpecalOffList> Pro_SellSpecalOffList { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual ICollection<VIP_HallOffInfo> VIP_HallOffInfo { get; set; }
        public virtual ICollection<VIP_OffTicket> VIP_OffTicket { get; set; }
        public virtual ICollection<VIP_ProOffList> VIP_ProOffList { get; set; }
        public virtual ICollection<VIP_SendProOffList> VIP_SendProOffList { get; set; }
        public virtual ICollection<VIP_VIPOffLIst> VIP_VIPOffLIst { get; set; }
        public virtual VIP_OffList VIP_OffList1 { get; set; }
        public virtual VIP_OffList VIP_OffList2 { get; set; }
        public virtual ICollection<VIP_VIPTypeOffLIst> VIP_VIPTypeOffLIst { get; set; }
    }
}
