using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Sys_UserDefaultOpenPage
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public Nullable<int> MenuID { get; set; }
        public virtual Sys_MenuInfo Sys_MenuInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
    }
}
