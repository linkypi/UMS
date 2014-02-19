using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_Role_Menu_ProInfo_bak
    {
        public int ID { get; set; }
        public Nullable<int> MenuID { get; set; }
        public Nullable<int> ClassID { get; set; }
        public string Note { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> RoleBakID { get; set; }
        public virtual Sys_RoleInfo_back Sys_RoleInfo_back { get; set; }
    }
}
