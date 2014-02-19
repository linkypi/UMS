using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_StoreInfo
    {
        public int ID { get; set; }
        public decimal ProCount { get; set; }
        public string Note { get; set; }
        public string HallID { get; set; }
        public string InListID { get; set; }
        public string ProID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
    }
}
