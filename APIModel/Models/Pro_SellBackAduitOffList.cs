using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellBackAduitOffList
    {
        public int ID { get; set; }
        public int AduitID { get; set; }
        public int SpecalOffID { get; set; }
        public decimal OffPrice { get; set; }
        public virtual Pro_SellBackAduit Pro_SellBackAduit { get; set; }
        public virtual Pro_SellSpecalOffList Pro_SellSpecalOffList { get; set; }
    }
}
