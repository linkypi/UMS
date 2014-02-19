using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_BorowAduitList
    {
        public int ID { get; set; }
        public Nullable<int> BAduitID { get; set; }
        public string ProID { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> ProPrice { get; set; }
        public virtual Pro_BorowAduit Pro_BorowAduit { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
    }
}
