using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_Sell_JiPeiKa_temp
    {
        public int ID { get; set; }
        public int SellListID { get; set; }
        public string IMEI { get; set; }
        public virtual Pro_SellListInfo_Temp Pro_SellListInfo_Temp { get; set; }
    }
}
