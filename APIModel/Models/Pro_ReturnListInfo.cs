using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ReturnListInfo
    {
        public Pro_ReturnListInfo()
        {
            this.Pro_ReturnOrderIMEI = new List<Pro_ReturnOrderIMEI>();
        }

        public int ReturnListID { get; set; }
        public Nullable<int> ReturnID { get; set; }
        public string ProID { get; set; }
        public string InListID { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string Note { get; set; }
        public Nullable<int> BorowListID { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList { get; set; }
        public virtual Pro_ReturnInfo Pro_ReturnInfo { get; set; }
        public virtual ICollection<Pro_ReturnOrderIMEI> Pro_ReturnOrderIMEI { get; set; }
    }
}
