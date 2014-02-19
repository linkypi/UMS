using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_UserBorowInfo
    {
        public string BorowDate { get; set; }
        public string ReturnDate { get; set; }
        public string BorowID { get; set; }
        public Nullable<bool> IsReturn { get; set; }
        public string InListID { get; set; }
        public Nullable<decimal> BorowCount { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public string ProName { get; set; }
        public Nullable<int> BorowDays { get; set; }
        public string Borrower { get; set; }
        public decimal UnReturnCount { get; set; }
        public Nullable<int> ID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public string ProFormat { get; set; }
    }
}
