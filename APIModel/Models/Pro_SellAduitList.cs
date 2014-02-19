using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellAduitList
    {
        public int ID { get; set; }
        public Nullable<int> SellAuditID { get; set; }
        public string ProID { get; set; }
        public decimal OffMoney { get; set; }
        public decimal ProCount { get; set; }
        public decimal ProPrice { get; set; }
        public Nullable<int> SellTypeID { get; set; }
        public string Note { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_SellAduit Pro_SellAduit { get; set; }
    }
}
