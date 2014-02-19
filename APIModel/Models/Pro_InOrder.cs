using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_InOrder
    {
        public Pro_InOrder()
        {
            this.Pro_InOrderList = new List<Pro_InOrderList>();
        }

        public int ID { get; set; }
        public string InOrderID { get; set; }
        public string Pro_HallID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual ICollection<Pro_InOrderList> Pro_InOrderList { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
    }
}
