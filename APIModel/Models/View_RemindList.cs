using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_RemindList
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public string ProcName { get; set; }
        public Nullable<int> MenuID { get; set; }
        public string IsInTime { get; set; }
        public Nullable<decimal> Order { get; set; }
        public int ID { get; set; }
        public string OldName { get; set; }
        public string OldNote { get; set; }
        public string OldProcName { get; set; }
        public Nullable<int> OldMenuID { get; set; }
        public string OldIsInTime { get; set; }
        public Nullable<decimal> OldOrder { get; set; }
        public string Flag { get; set; }
        public string OldFlag { get; set; }
    }
}
