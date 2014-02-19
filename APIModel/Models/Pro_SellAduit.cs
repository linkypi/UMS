using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellAduit
    {
        public Pro_SellAduit()
        {
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
            this.Pro_SellAduitList = new List<Pro_SellAduitList>();
        }

        public int ID { get; set; }
        public string AduitID { get; set; }
        public string AduitUser { get; set; }
        public Nullable<System.DateTime> AduitDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string ApplyUser { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public bool Aduited { get; set; }
        public bool Passed { get; set; }
        public bool Used { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Note { get; set; }
        public decimal Money { get; set; }
        public string HallID { get; set; }
        public string CustName { get; set; }
        public string CustPhone { get; set; }
        public Nullable<bool> Aduited2 { get; set; }
        public Nullable<bool> Passed2 { get; set; }
        public Nullable<System.DateTime> AduitDate2 { get; set; }
        public string AduitUser2 { get; set; }
        public Nullable<bool> Aduited3 { get; set; }
        public Nullable<bool> Passed3 { get; set; }
        public Nullable<System.DateTime> AduitDate3 { get; set; }
        public string AduitUser3 { get; set; }
        public Nullable<bool> Aduited1 { get; set; }
        public Nullable<bool> Passed1 { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual ICollection<Pro_SellAduitList> Pro_SellAduitList { get; set; }
    }
}
