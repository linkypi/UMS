using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_InOrderIMEI
    {
        public int ID { get; set; }
        public string InListID { get; set; }
        public string IMEI { get; set; }
        public string Note { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList { get; set; }
    }
}
