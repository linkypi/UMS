using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellOffAduitInfo
    {
        public Pro_SellOffAduitInfo()
        {
            this.Pro_IMEI = new List<Pro_IMEI>();
            this.Pro_SellBackInfo_Aduit = new List<Pro_SellBackInfo_Aduit>();
            this.Pro_SellInfo_Aduit = new List<Pro_SellInfo_Aduit>();
            this.Pro_SellOffAduitInfoList = new List<Pro_SellOffAduitInfoList>();
        }

        public int ID { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<int> BackID { get; set; }
        public string ApplyUserID { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public string AduitUserID { get; set; }
        public Nullable<System.DateTime> AduitDate { get; set; }
        public bool IsPass { get; set; }
        public bool IsAduited { get; set; }
        public string ApplyNote { get; set; }
        public string AduitNote { get; set; }
        public string HallID { get; set; }
        public decimal NextPrice { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual ICollection<Pro_IMEI> Pro_IMEI { get; set; }
        public virtual Pro_SellBack Pro_SellBack { get; set; }
        public virtual ICollection<Pro_SellBackInfo_Aduit> Pro_SellBackInfo_Aduit { get; set; }
        public virtual Pro_SellInfo Pro_SellInfo { get; set; }
        public virtual ICollection<Pro_SellInfo_Aduit> Pro_SellInfo_Aduit { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual ICollection<Pro_SellOffAduitInfoList> Pro_SellOffAduitInfoList { get; set; }
    }
}
