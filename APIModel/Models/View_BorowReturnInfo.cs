using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_BorowReturnInfo
    {
        public string BorowID { get; set; }
        public string HallName { get; set; }
        public string Dept { get; set; }
        public string Borrower { get; set; }
        public string BorrowType { get; set; }
        public string IsReturn { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string UserID { get; set; }
        public string HallID { get; set; }
        public string Note { get; set; }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string BorowDate { get; set; }
    }
}
