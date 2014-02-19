using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_RoleInfo_back
    {
        public Sys_RoleInfo_back()
        {
            this.Sys_Role_Menu_HallInfo_bak = new List<Sys_Role_Menu_HallInfo_bak>();
            this.Sys_Role_Menu_ProInfo_bak = new List<Sys_Role_Menu_ProInfo_bak>();
            this.Sys_Role_MenuInfo_bak = new List<Sys_Role_MenuInfo_bak>();
        }

        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Menu_ID_List { get; set; }
        public string Method_ID_List { get; set; }
        public string Note { get; set; }
        public Nullable<bool> Flag { get; set; }
        public string MenuXML { get; set; }
        public int ID { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string Updater { get; set; }
        public virtual ICollection<Sys_Role_Menu_HallInfo_bak> Sys_Role_Menu_HallInfo_bak { get; set; }
        public virtual ICollection<Sys_Role_Menu_ProInfo_bak> Sys_Role_Menu_ProInfo_bak { get; set; }
        public virtual ICollection<Sys_Role_MenuInfo_bak> Sys_Role_MenuInfo_bak { get; set; }
    }
}
