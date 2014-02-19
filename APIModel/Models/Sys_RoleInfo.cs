using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_RoleInfo
    {
        public Sys_RoleInfo()
        {
            this.Sys_Role_Menu_HallInfo = new List<Sys_Role_Menu_HallInfo>();
            this.Sys_Role_Menu_ProInfo = new List<Sys_Role_Menu_ProInfo>();
            this.Sys_Role_MenuInfo = new List<Sys_Role_MenuInfo>();
            this.Sys_UserInfo = new List<Sys_UserInfo>();
        }

        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Menu_ID_List { get; set; }
        public string Method_ID_List { get; set; }
        public string Note { get; set; }
        public Nullable<bool> Flag { get; set; }
        public string MenuXML { get; set; }
        public Nullable<System.DateTime> UpDateTime { get; set; }
        public string Updater { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string MobileMenuJson { get; set; }
        public virtual ICollection<Sys_Role_Menu_HallInfo> Sys_Role_Menu_HallInfo { get; set; }
        public virtual ICollection<Sys_Role_Menu_ProInfo> Sys_Role_Menu_ProInfo { get; set; }
        public virtual ICollection<Sys_Role_MenuInfo> Sys_Role_MenuInfo { get; set; }
        public virtual ICollection<Sys_UserInfo> Sys_UserInfo { get; set; }
    }
}
