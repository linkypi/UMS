using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Rules_OffList
    {
        public string UserName { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool Flag { get; set; }
        public int ID { get; set; }
        public string State { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string Deleter { get; set; }
    }
}
