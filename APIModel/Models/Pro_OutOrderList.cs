using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_OutOrderList
    {
        public Pro_OutOrderList()
        {
            this.Pro_OutOrderIMEI = new List<Pro_OutOrderIMEI>();
        }

        public int OutListID { get; set; }
        public Nullable<int> OutID { get; set; }
        public string InListID { get; set; }
        public decimal ProCount { get; set; }
        public string Note { get; set; }
        public string ProID { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList { get; set; }
        public virtual Pro_OutInfo Pro_OutInfo { get; set; }
        public virtual ICollection<Pro_OutOrderIMEI> Pro_OutOrderIMEI { get; set; }
    }
}
