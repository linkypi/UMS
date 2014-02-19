using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Pro_InOrder
    {
        public int ID { get; set; }
        public string InOrderID { get; set; }
        public string Pro_HallID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public string HallName { get; set; }
    }
}
