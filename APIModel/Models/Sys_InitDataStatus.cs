using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_InitDataStatus
    {
        public int ID { get; set; }
        public string DataName { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string Note { get; set; }
        public string DLLName { get; set; }
        public string ClassName { get; set; }
    }
}
