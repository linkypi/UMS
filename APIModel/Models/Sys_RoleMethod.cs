using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_RoleMethod
    {
        public int ID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> MenuID { get; set; }
        public string HallID { get; set; }
        public Nullable<int> MethodID { get; set; }
        public Nullable<int> ClassID { get; set; }
        public string Note { get; set; }
        public Nullable<int> DateLimit { get; set; }
        public virtual Pro_ClassInfo Pro_ClassInfo { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Sys_MenuInfo Sys_MenuInfo { get; set; }
        public virtual Sys_MethodInfo Sys_MethodInfo { get; set; }
    }
}
