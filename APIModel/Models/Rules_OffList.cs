using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Rules_OffList
    {
        public Rules_OffList()
        {
            this.Rules_HallOffInfo = new List<Rules_HallOffInfo>();
            this.Rules_ProMainInfo = new List<Rules_ProMainInfo>();
            this.Rules_SellTypeInfo = new List<Rules_SellTypeInfo>();
        }

        public int ID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool Flag { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string Deleter { get; set; }
        public virtual ICollection<Rules_HallOffInfo> Rules_HallOffInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual ICollection<Rules_ProMainInfo> Rules_ProMainInfo { get; set; }
        public virtual ICollection<Rules_SellTypeInfo> Rules_SellTypeInfo { get; set; }
    }
}
