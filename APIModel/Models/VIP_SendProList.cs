using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_SendProList
    {
        public int ID { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<int> SellListID { get; set; }
        public Nullable<int> SendID { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string IMEI { get; set; }
    }
}
