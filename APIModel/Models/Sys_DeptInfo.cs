using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_DeptInfo
    {
        public Sys_DeptInfo()
        {
            this.Sys_DeptInfo1 = new List<Sys_DeptInfo>();
            this.Sys_UserInfo = new List<Sys_UserInfo>();
        }

        public int DtpID { get; set; }
        public Nullable<int> Parent { get; set; }
        public string DtpName { get; set; }
        public Nullable<bool> Flag { get; set; }
        public string Note { get; set; }
        public string Head { get; set; }
        public string HeadTele { get; set; }
        public virtual ICollection<Sys_DeptInfo> Sys_DeptInfo1 { get; set; }
        public virtual Sys_DeptInfo Sys_DeptInfo2 { get; set; }
        public virtual ICollection<Sys_UserInfo> Sys_UserInfo { get; set; }
    }
}
