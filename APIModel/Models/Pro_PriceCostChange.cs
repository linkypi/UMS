using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_PriceCostChange
    {
        public Pro_PriceCostChange()
        {
            this.Pro_PriceCost_InorderList = new List<Pro_PriceCost_InorderList>();
            this.Pro_PriceCostChangeList = new List<Pro_PriceCostChangeList>();
        }

        public int ID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string Note { get; set; }
        public string ChangedID { get; set; }
        public virtual ICollection<Pro_PriceCost_InorderList> Pro_PriceCost_InorderList { get; set; }
        public virtual ICollection<Pro_PriceCostChangeList> Pro_PriceCostChangeList { get; set; }
    }
}
