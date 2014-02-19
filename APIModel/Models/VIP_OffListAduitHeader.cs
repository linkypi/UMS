using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_OffListAduitHeader
    {
        public VIP_OffListAduitHeader()
        {
            this.VIP_HallInfoHeader = new List<VIP_HallInfoHeader>();
            this.VIP_OffListAduit = new List<VIP_OffListAduit>();
        }

        public int ID { get; set; }
        public string Destination { get; set; }
        public string SaleTarget { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Applyer { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public Nullable<bool> Aduited { get; set; }
        public Nullable<bool> Passed { get; set; }
        public Nullable<bool> Aduited1 { get; set; }
        public Nullable<bool> Passed1 { get; set; }
        public string AduitNote1 { get; set; }
        public Nullable<System.DateTime> AduitDate1 { get; set; }
        public Nullable<bool> Aduited2 { get; set; }
        public Nullable<bool> Passed2 { get; set; }
        public string AduitNote2 { get; set; }
        public Nullable<System.DateTime> AduitDate2 { get; set; }
        public string ApplyNote { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Scope { get; set; }
        public string Creater { get; set; }
        public string AduitUser1 { get; set; }
        public string AduitUser2 { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string Deleter { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<bool> Aduited3 { get; set; }
        public Nullable<bool> Passed3 { get; set; }
        public string AduitUser3 { get; set; }
        public Nullable<System.DateTime> AduitDate3 { get; set; }
        public string AduitNote3 { get; set; }
        public virtual ICollection<VIP_HallInfoHeader> VIP_HallInfoHeader { get; set; }
        public virtual ICollection<VIP_OffListAduit> VIP_OffListAduit { get; set; }
    }
}
