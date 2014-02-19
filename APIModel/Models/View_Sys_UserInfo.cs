using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Sys_UserInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string RealName { get; set; }
        public string UserIP { get; set; }
        public Nullable<int> DtpID { get; set; }
        public string UpdUserID { get; set; }
        public int RoleID { get; set; }
        public Nullable<bool> CanLogin { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string SysTime { get; set; }
        public Nullable<decimal> CancelLimit { get; set; }
        public Nullable<decimal> AduitLimit { get; set; }
        public string DtpName { get; set; }
        public string Name { get; set; }
        public Nullable<int> OpID { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LeaveDate { get; set; }
        public string IsLogin { get; set; }
        public string OpUpdUser { get; set; }
        public Nullable<bool> UserFlag { get; set; }
        public string Aduit { get; set; }
        public Nullable<bool> Flag { get; set; }
        public int ID { get; set; }
        public string LeaveTime { get; set; }
        public string RoleName { get; set; }
        public string HallID { get; set; }
        public string HallName { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<decimal> AuditOffPrice { get; set; }
        public bool IsBoss { get; set; }
        public string HasDefault { get; set; }
        public Nullable<decimal> BorowAduitPrice { get; set; }
    }
}
