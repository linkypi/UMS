using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_ProOffList
    {
        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> OffID { get; set; }
        public string Note { get; set; }
        public Nullable<int> SellTypeID { get; set; }
        public decimal ProCount { get; set; }
        public Nullable<decimal> Price { get; set; }
        public decimal AfterOffPrice { get; set; }
        public decimal Salary { get; set; }
        public decimal Rate { get; set; }
        public decimal ReduceMoney { get; set; }
        public decimal Point { get; set; }
        public decimal OffMoney { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_SellType Pro_SellType { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
    }
}
