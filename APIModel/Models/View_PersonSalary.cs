using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_PersonSalary
    {
        public Nullable<decimal> salary { get; set; }
        public string Seller { get; set; }
        public string HallID { get; set; }
        public Nullable<System.DateTime> SellDate { get; set; }
        public int ID { get; set; }
    }
}
