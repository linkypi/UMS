using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellBackIMEIList
    {
        public int ID { get; set; }
        public Nullable<int> SellBackListID { get; set; }
        public string IMEI { get; set; }
        public string Note { get; set; }
        public virtual Pro_SellBackList Pro_SellBackList { get; set; }
    }
}
