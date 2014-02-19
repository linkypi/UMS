using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_PriceChangeList
    {
        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> ChangeID { get; set; }
        public Nullable<int> SellType { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> LowPrice { get; set; }
        public Nullable<decimal> MinPrice { get; set; }
        public Nullable<decimal> MaxPrice { get; set; }
        public Nullable<bool> IsTicketUseful { get; set; }
        public Nullable<bool> IsAduit { get; set; }
        public virtual Pro_PriceChange Pro_PriceChange { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_SellType Pro_SellType { get; set; }
    }
}
