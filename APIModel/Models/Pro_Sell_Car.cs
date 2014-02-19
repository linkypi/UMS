using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_Sell_Car
    {
        public int ID { get; set; }
        public string CarName { get; set; }
        public string CarID { get; set; }
        public string ProID { get; set; }
        public string Address { get; set; }
        public string Desc { get; set; }
        public bool IsOther { get; set; }
        public string Note { get; set; }
        public Nullable<int> SellListID { get; set; }
        public virtual Pro_SellListInfo Pro_SellListInfo { get; set; }
    }
}
