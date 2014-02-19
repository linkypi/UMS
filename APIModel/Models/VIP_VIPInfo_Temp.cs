using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPInfo_Temp
    {
        public VIP_VIPInfo_Temp()
        {
            this.Pro_IMEI = new List<Pro_IMEI>();
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
        public Nullable<int> SellListID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> ProPrice { get; set; }
        public string IMEI { get; set; }
        public string Password { get; set; }
        public string userName { get; set; }
        public string HallID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string LZUser { get; set; }
        public virtual ICollection<Pro_IMEI> Pro_IMEI { get; set; }
        public virtual Pro_SellListInfo_Temp Pro_SellListInfo_Temp { get; set; }
        public virtual ICollection<VIP_VIPService> VIP_VIPService { get; set; }
    }
}
