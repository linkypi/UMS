using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_UserRemindList
    {
        public string UserID { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<bool> IsInTime { get; set; }
        public Nullable<decimal> Order { get; set; }
        public Nullable<int> MenuID { get; set; }
        public string ProcName { get; set; }
        public string Note { get; set; }
        public Nullable<bool> Flag { get; set; }
        public string Name { get; set; }
        public string MenuValue { get; set; }
        public string MenuText { get; set; }
        public int ID { get; set; }
    }
}
