using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_BorowListInfo
    {
        public Pro_BorowListInfo()
        {
            this.Pro_BorowOrderIMEI = new List<Pro_BorowOrderIMEI>();
        }

        public int BorowListID { get; set; }
        public Nullable<int> BorowID { get; set; }
        public string ProID { get; set; }
        public string InListID { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public string Note { get; set; }
        public string AduitID { get; set; }
        public Nullable<decimal> RetCount { get; set; }
        public Nullable<bool> IsReturn { get; set; }
        public Nullable<decimal> ProPrice { get; set; }
        public virtual Pro_BorowInfo Pro_BorowInfo { get; set; }
        public virtual ICollection<Pro_BorowOrderIMEI> Pro_BorowOrderIMEI { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
    }
}
