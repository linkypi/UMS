using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_RemindList
    {
        public Sys_RemindList()
        {
            this.Sys_UserRemindList = new List<Sys_UserRemindList>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Flag { get; set; }
        public string Note { get; set; }
        public string ProcName { get; set; }
        public Nullable<int> MenuID { get; set; }
        public Nullable<decimal> Order { get; set; }
        public Nullable<bool> IsInTime { get; set; }
        public Nullable<int> Count { get; set; }
        public virtual Sys_MenuInfo Sys_MenuInfo { get; set; }
        public virtual ICollection<Sys_UserRemindList> Sys_UserRemindList { get; set; }
    }
}
