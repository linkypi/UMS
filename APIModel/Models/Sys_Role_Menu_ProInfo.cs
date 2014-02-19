using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_Role_Menu_ProInfo
    {
        public int ID { get; set; }
        public Nullable<int> MenuID { get; set; }
        public Nullable<int> ClassID { get; set; }
        public string Note { get; set; }
        public Nullable<int> RoleID { get; set; }
        public virtual Pro_ClassInfo Pro_ClassInfo { get; set; }
        public virtual Sys_MenuInfo Sys_MenuInfo { get; set; }
        public virtual Sys_RoleInfo Sys_RoleInfo { get; set; }
    }
}
