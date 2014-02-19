using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_SalaryCurrentDelete
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
    }
}
