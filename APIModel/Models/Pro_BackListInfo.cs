using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_BackListInfo
    {
        public Pro_BackListInfo()
        {
            this.Pro_BackOrderIMEI = new List<Pro_BackOrderIMEI>();
        }

        public int BackListID { get; set; }
        public Nullable<int> BackID { get; set; }
        public string InListID { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string Note { get; set; }
        public string ProID { get; set; }
        public virtual Pro_BackInfo Pro_BackInfo { get; set; }
        public virtual ICollection<Pro_BackOrderIMEI> Pro_BackOrderIMEI { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList { get; set; }
    }
}
