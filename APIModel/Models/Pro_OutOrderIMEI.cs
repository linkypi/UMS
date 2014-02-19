using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_OutOrderIMEI
    {
        public int ID { get; set; }
        public Nullable<int> OutListID { get; set; }
        public string IMEI { get; set; }
        public string Note { get; set; }
        public virtual Pro_OutOrderList Pro_OutOrderList { get; set; }
    }
}
