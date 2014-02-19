using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_Role_Menu_HallInfo
    {
        public int ID { get; set; }
        public Nullable<int> MenuID { get; set; }
        public string HallID { get; set; }
        public string Note { get; set; }
        public Nullable<int> RoleID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Sys_MenuInfo Sys_MenuInfo { get; set; }
        public virtual Sys_RoleInfo Sys_RoleInfo { get; set; }
    }
}
