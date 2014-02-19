using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_BorowAduit
    {
        public Pro_BorowAduit()
        {
            this.Pro_BorowAduitList = new List<Pro_BorowAduitList>();
        }

        public int ID { get; set; }
        public string AduitID { get; set; }
        public string AduitUser { get; set; }
        public Nullable<System.DateTime> AduitDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string ApplyUser { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public Nullable<bool> Aduited { get; set; }
        public Nullable<bool> Passed { get; set; }
        public Nullable<bool> Used { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Note { get; set; }
        public string HallID { get; set; }
        public string Borrower { get; set; }
        public string BorrowType { get; set; }
        public string Dept { get; set; }
        public string MobilPhone { get; set; }
        public Nullable<System.DateTime> EstimateReturnTime { get; set; }
        public string AduitUser2 { get; set; }
        public Nullable<System.DateTime> AduitDate2 { get; set; }
        public Nullable<bool> Aduited2 { get; set; }
        public Nullable<bool> Passed2 { get; set; }
        public string AduitUser3 { get; set; }
        public Nullable<System.DateTime> AduitDate3 { get; set; }
        public Nullable<bool> Aduited3 { get; set; }
        public Nullable<bool> Passed3 { get; set; }
        public Nullable<bool> InternalBorow { get; set; }
        public Nullable<bool> Aduited1 { get; set; }
        public Nullable<bool> Passed1 { get; set; }
        public Nullable<decimal> TotalMoney { get; set; }
        public Nullable<bool> Aduited4 { get; set; }
        public Nullable<bool> Passed4 { get; set; }
        public string AduitUser4 { get; set; }
        public Nullable<System.DateTime> AduitDate4 { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string Note4 { get; set; }
        public virtual ICollection<Pro_BorowAduitList> Pro_BorowAduitList { get; set; }
    }
}
