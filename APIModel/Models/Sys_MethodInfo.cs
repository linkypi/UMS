using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_MethodInfo
    {
        public Sys_MethodInfo()
        {
            this.Sys_RoleMethod = new List<Sys_RoleMethod>();
        }

        public int MethodID { get; set; }
        public Nullable<int> MenuID { get; set; }
        public string Name { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public string DllName { get; set; }
        public string Note { get; set; }
        public string Log { get; set; }
        public Nullable<int> Validity { get; set; }
        public virtual Sys_MenuInfo Sys_MenuInfo { get; set; }
        public virtual ICollection<Sys_RoleMethod> Sys_RoleMethod { get; set; }
    }
}
