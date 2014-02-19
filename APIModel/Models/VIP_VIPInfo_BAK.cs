using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPInfo_BAK
    {
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
        public string UpdUser { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public Nullable<int> Old_ID { get; set; }
        public string HallID { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual VIP_VIPInfo VIP_VIPInfo { get; set; }
    }
}
