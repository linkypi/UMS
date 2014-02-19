using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Pro_RepairReturnInfo
    {
        public int ID { get; set; }
        public string HallID { get; set; }
        public string HallName { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string RepairReturnDate { get; set; }
        public string RepairReturnID { get; set; }
        public string Canceled { get; set; }
        public string IsReceived { get; set; }
        public string OldID { get; set; }
    }
}
