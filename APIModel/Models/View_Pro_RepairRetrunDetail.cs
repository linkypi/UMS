using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Pro_RepairRetrunDetail
    {
        public string TypeName { get; set; }
        public int TypeID { get; set; }
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public string ProID { get; set; }
        public string ProName { get; set; }
        public int ID { get; set; }
        public string NEW_IMEI { get; set; }
        public string OLD_IMEI { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string Note { get; set; }
        public string InListID { get; set; }
        public string ProFormat { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string HallID { get; set; }
    }
}
