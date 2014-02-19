using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_MenuInfo
    {
        public Sys_MenuInfo()
        {
            this.Demo_ReportViewInfo = new List<Demo_ReportViewInfo>();
            this.Sys_MenuInfo1 = new List<Sys_MenuInfo>();
            this.Sys_MethodInfo = new List<Sys_MethodInfo>();
            this.Sys_RemindList = new List<Sys_RemindList>();
            this.Sys_Role_Menu_HallInfo = new List<Sys_Role_Menu_HallInfo>();
            this.Sys_Role_MenuInfo = new List<Sys_Role_MenuInfo>();
            this.Sys_Role_Menu_ProInfo = new List<Sys_Role_Menu_ProInfo>();
            this.Sys_RoleMethod = new List<Sys_RoleMethod>();
            this.Sys_UserDefaultOpenPage = new List<Sys_UserDefaultOpenPage>();
        }

        public int MenuID { get; set; }
        public string MenuText { get; set; }
        public string MenuValue { get; set; }
        public string Note { get; set; }
        public Nullable<int> Parent { get; set; }
        public string MenuImg { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<decimal> Order { get; set; }
        public Nullable<bool> HasHallRole { get; set; }
        public Nullable<bool> HasProRole { get; set; }
        public bool DisplayMobile { get; set; }
        public virtual ICollection<Demo_ReportViewInfo> Demo_ReportViewInfo { get; set; }
        public virtual ICollection<Sys_MenuInfo> Sys_MenuInfo1 { get; set; }
        public virtual Sys_MenuInfo Sys_MenuInfo2 { get; set; }
        public virtual ICollection<Sys_MethodInfo> Sys_MethodInfo { get; set; }
        public virtual ICollection<Sys_RemindList> Sys_RemindList { get; set; }
        public virtual ICollection<Sys_Role_Menu_HallInfo> Sys_Role_Menu_HallInfo { get; set; }
        public virtual ICollection<Sys_Role_MenuInfo> Sys_Role_MenuInfo { get; set; }
        public virtual ICollection<Sys_Role_Menu_ProInfo> Sys_Role_Menu_ProInfo { get; set; }
        public virtual ICollection<Sys_RoleMethod> Sys_RoleMethod { get; set; }
        public virtual ICollection<Sys_UserDefaultOpenPage> Sys_UserDefaultOpenPage { get; set; }
    }
}
