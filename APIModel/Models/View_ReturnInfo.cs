using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_ReturnInfo
    {
        public string UserName { get; set; }
        public int ID { get; set; }
        public string ReturnID { get; set; }
        public string OldID { get; set; }
        public string ReturnDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public Nullable<int> BorowID { get; set; }
        public string DeleteDate { get; set; }
        public string Deleter { get; set; }
        public string HallName { get; set; }
        public string HallID { get; set; }
        public string Borrower { get; set; }
    }
}
