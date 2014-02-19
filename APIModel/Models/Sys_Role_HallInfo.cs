using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_Role_HallInfo
    {
        public int ID { get; set; }
        public Nullable<int> Role_Menu_ID { get; set; }
        public string HallID { get; set; }
        public string Note { get; set; }
        public Nullable<int> RoleID { get; set; }
        public virtual Sys_Role_MenuInfo Sys_Role_MenuInfo { get; set; }
    }
}
