using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPInfo
    {
        public VIP_VIPInfo()
        {
            this.Pro_SellBackAduit = new List<Pro_SellBackAduit>();
            this.Pro_SellBackAduit_bak = new List<Pro_SellBackAduit_bak>();
            this.Pro_SellInfo = new List<Pro_SellInfo>();
            this.Pro_SellInfo_Aduit = new List<Pro_SellInfo_Aduit>();
            this.VIP_CardChange = new List<VIP_CardChange>();
            this.VIP_CardChange1 = new List<VIP_CardChange>();
            this.VIP_OffTicket = new List<VIP_OffTicket>();
            this.VIP_Renew = new List<VIP_Renew>();
            this.VIP_VIPBack = new List<VIP_VIPBack>();
            this.VIP_VIPOffLIst = new List<VIP_VIPOffLIst>();
            this.VIP_VIPInfo_BAK = new List<VIP_VIPInfo_BAK>();
            this.VIP_VIPService = new List<VIP_VIPService>();
        }

        public int ID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string MemberName { get; set; }
        public string Sex { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string MobiPhone { get; set; }
        public string TelePhone { get; set; }
        public string QQ { get; set; }
        public string Address { get; set; }
        public Nullable<int> IDCard_ID { get; set; }
        public string IDCard { get; set; }
        public Nullable<int> Validity { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<decimal> Point { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string Seller { get; set; }
        public string UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> ProPrice { get; set; }
        public string IMEI { get; set; }
        public string Password { get; set; }
        public string userName { get; set; }
        public string HallID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string Email { get; set; }
        public string LZUser { get; set; }
        public virtual ICollection<Pro_SellBackAduit> Pro_SellBackAduit { get; set; }
        public virtual ICollection<Pro_SellBackAduit_bak> Pro_SellBackAduit_bak { get; set; }
        public virtual ICollection<Pro_SellInfo> Pro_SellInfo { get; set; }
        public virtual ICollection<Pro_SellInfo_Aduit> Pro_SellInfo_Aduit { get; set; }
        public virtual Pro_SellListInfo Pro_SellListInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual ICollection<VIP_CardChange> VIP_CardChange { get; set; }
        public virtual ICollection<VIP_CardChange> VIP_CardChange1 { get; set; }
        public virtual VIP_IDCardType VIP_IDCardType { get; set; }
        public virtual ICollection<VIP_OffTicket> VIP_OffTicket { get; set; }
        public virtual ICollection<VIP_Renew> VIP_Renew { get; set; }
        public virtual ICollection<VIP_VIPBack> VIP_VIPBack { get; set; }
        public virtual ICollection<VIP_VIPOffLIst> VIP_VIPOffLIst { get; set; }
        public virtual ICollection<VIP_VIPInfo_BAK> VIP_VIPInfo_BAK { get; set; }
        public virtual VIP_VIPType VIP_VIPType { get; set; }
        public virtual ICollection<VIP_VIPService> VIP_VIPService { get; set; }
    }
}
