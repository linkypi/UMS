using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellBackAduitList
    {
        public int ID { get; set; }
        public int AduitID { get; set; }
        public int SellListID { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public Nullable<decimal> BackPrice { get; set; }
        public Nullable<decimal> AduitBackPrice { get; set; }
        public virtual Pro_SellBackAduit Pro_SellBackAduit { get; set; }
        public virtual Pro_SellListInfo Pro_SellListInfo { get; set; }
    }
}
