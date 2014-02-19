using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_BorowInfo
    {
        public Pro_BorowInfo()
        {
            this.Pro_BorowListInfo = new List<Pro_BorowListInfo>();
            this.Pro_ReturnInfo = new List<Pro_ReturnInfo>();
        }

        public int ID { get; set; }
        public string HallID { get; set; }
        public string BorowID { get; set; }
        public string OldID { get; set; }
        public Nullable<System.DateTime> BorowDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Note { get; set; }
        public string Dept { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string Deleter { get; set; }
        public string Borrower { get; set; }
        public string BorrowType { get; set; }
        public string AduitID { get; set; }
        public Nullable<bool> IsReturn { get; set; }
        public string MobilPhone { get; set; }
        public Nullable<System.DateTime> EstimateReturnTime { get; set; }
        public virtual ICollection<Pro_BorowListInfo> Pro_BorowListInfo { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Pro_BorowInfo Pro_BorowInfo1 { get; set; }
        public virtual Pro_BorowInfo Pro_BorowInfo2 { get; set; }
        public virtual ICollection<Pro_ReturnInfo> Pro_ReturnInfo { get; set; }
    }
}
