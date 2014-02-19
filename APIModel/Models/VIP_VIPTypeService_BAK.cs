using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPTypeService_BAK
    {
        public int ID { get; set; }
        public string ProID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<decimal> SCount { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string UpdUser { get; set; }
        public Nullable<int> OldID { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual VIP_VIPType VIP_VIPType { get; set; }
        public virtual VIP_VIPType_Bak VIP_VIPType_Bak { get; set; }
        public virtual VIP_VIPTypeService VIP_VIPTypeService { get; set; }
    }
}
