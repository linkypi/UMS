using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellIMEIList
    {
        public int ID { get; set; }
        public Nullable<int> SellListID { get; set; }
        public string IMEI { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> CashPrice { get; set; }
        public Nullable<int> SellSpecalID { get; set; }
        public virtual Pro_SellListInfo Pro_SellListInfo { get; set; }
        public virtual Pro_SellSpecalOffList Pro_SellSpecalOffList { get; set; }
    }
}
