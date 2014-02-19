using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellTypeProduct_bak
    {
        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> SellType { get; set; }
        public decimal Price { get; set; }
        public decimal LowPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool IsTicketUseful { get; set; }
        public bool IsAduit { get; set; }
        public decimal ProCost { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_SellType Pro_SellType { get; set; }
    }
}
