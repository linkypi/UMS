using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_Sell_Service
    {
        public int ID { get; set; }
        public int SellListID { get; set; }
        public string IMEI { get; set; }
        public string ProClass { get; set; }
        public string ProName { get; set; }
        public bool isVIPService { get; set; }
        public string VIPService_ProID { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_SellListInfo Pro_SellListInfo { get; set; }
    }
}
