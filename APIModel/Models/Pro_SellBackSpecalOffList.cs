using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellBackSpecalOffList
    {
        public int ID { get; set; }
        public Nullable<int> BackID { get; set; }
        public Nullable<int> OffID { get; set; }
        public string Note { get; set; }
        public decimal OffPrice { get; set; }
        public virtual Pro_SellBack Pro_SellBack { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
    }
}
