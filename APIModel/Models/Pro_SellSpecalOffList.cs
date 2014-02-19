using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellSpecalOffList
    {
        public Pro_SellSpecalOffList()
        {
            this.Pro_SellBackAduitOffList = new List<Pro_SellBackAduitOffList>();
            this.Pro_SellIMEIList = new List<Pro_SellIMEIList>();
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
            this.Pro_SellSendInfo = new List<Pro_SellSendInfo>();
        }

        public int ID { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<int> SpecalOffID { get; set; }
        public string Note { get; set; }
        public Nullable<int> BackID { get; set; }
        public decimal OffMoney { get; set; }
        public Nullable<int> SellAduitID { get; set; }
        public Nullable<int> BackAduitID { get; set; }
        public virtual Pro_SellBack Pro_SellBack { get; set; }
        public virtual ICollection<Pro_SellBackAduitOffList> Pro_SellBackAduitOffList { get; set; }
        public virtual Pro_SellBackInfo_Aduit Pro_SellBackInfo_Aduit { get; set; }
        public virtual ICollection<Pro_SellIMEIList> Pro_SellIMEIList { get; set; }
        public virtual Pro_SellInfo Pro_SellInfo { get; set; }
        public virtual Pro_SellInfo_Aduit Pro_SellInfo_Aduit { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
        public virtual ICollection<Pro_SellSendInfo> Pro_SellSendInfo { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
    }
}
