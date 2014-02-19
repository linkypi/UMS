using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ChangeProListInfo
    {
        public Pro_ChangeProListInfo()
        {
            this.Pro_ChangeIMEIInfo = new List<Pro_ChangeIMEIInfo>();
        }

        public int ChangeListID { get; set; }
        public string InListID { get; set; }
        public string OldProID { get; set; }
        public string NewProID { get; set; }
        public decimal ProCount { get; set; }
        public Nullable<int> ChangeID { get; set; }
        public string Note { get; set; }
        public string NewInListID { get; set; }
        public Nullable<bool> Flag { get; set; }
        public virtual ICollection<Pro_ChangeIMEIInfo> Pro_ChangeIMEIInfo { get; set; }
        public virtual Pro_ChangeProInfo Pro_ChangeProInfo { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo1 { get; set; }
    }
}
