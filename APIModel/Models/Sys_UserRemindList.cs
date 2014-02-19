using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_UserRemindList
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public Nullable<int> Remind { get; set; }
        public string Note { get; set; }
        public Nullable<int> Count { get; set; }
        public virtual Sys_RemindList Sys_RemindList { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
    }
}
