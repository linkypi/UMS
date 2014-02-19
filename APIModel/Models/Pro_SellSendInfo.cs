using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellSendInfo
    {
        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> OffID { get; set; }
        public string IMEI { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public Nullable<int> SepecialID { get; set; }
        public Nullable<int> SellListID { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<int> SendProOffID { get; set; }
        public Nullable<decimal> ProCost { get; set; }
        public string Note { get; set; }
        public string InOrderList { get; set; }
        public Nullable<int> BackID { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_SellBack Pro_SellBack { get; set; }
        public virtual Pro_SellInfo Pro_SellInfo { get; set; }
        public virtual Pro_SellListInfo Pro_SellListInfo { get; set; }
        public virtual Pro_SellSpecalOffList Pro_SellSpecalOffList { get; set; }
        public virtual VIP_SendProOffList VIP_SendProOffList { get; set; }
    }
}
