using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ReturnInfo
    {
        public Pro_ReturnInfo()
        {
            this.Pro_ReturnListInfo = new List<Pro_ReturnListInfo>();
        }

        public int ID { get; set; }
        public string ReturnID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Note { get; set; }
        public Nullable<int> BorowID { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string Deleter { get; set; }
        public virtual Pro_BorowInfo Pro_BorowInfo { get; set; }
        public virtual ICollection<Pro_ReturnListInfo> Pro_ReturnListInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
    }
}
