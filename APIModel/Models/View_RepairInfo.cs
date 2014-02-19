using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_RepairInfo
    {
        public string UserName { get; set; }
        public string HallName { get; set; }
        public string IsReturn { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string UserID { get; set; }
        public string RepairDate { get; set; }
        public string OldID { get; set; }
        public string RepairID { get; set; }
        public string HallID { get; set; }
        public int ID { get; set; }
        public string IsReceive { get; set; }
        public string Receiver { get; set; }
        public string RecvTime { get; set; }
    }
}
