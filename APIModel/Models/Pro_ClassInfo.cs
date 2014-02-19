using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ClassInfo
    {
        public Pro_ClassInfo()
        {
            this.Pro_ProInfo = new List<Pro_ProInfo>();
            this.Sys_Role_Menu_ProInfo = new List<Sys_Role_Menu_ProInfo>();
            this.Sys_RoleMethod = new List<Sys_RoleMethod>();
        }

        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Note { get; set; }
        public Nullable<bool> HasSalary { get; set; }
        public Nullable<int> ClassTypeID { get; set; }
        public virtual Pro_ClassType Pro_ClassType { get; set; }
        public virtual ICollection<Pro_ProInfo> Pro_ProInfo { get; set; }
        public virtual ICollection<Sys_Role_Menu_ProInfo> Sys_Role_Menu_ProInfo { get; set; }
        public virtual ICollection<Sys_RoleMethod> Sys_RoleMethod { get; set; }
    }
}
