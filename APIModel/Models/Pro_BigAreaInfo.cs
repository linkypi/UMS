using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_BigAreaInfo
    {
        public Pro_BigAreaInfo()
        {
            this.Pro_AreaInfo = new List<Pro_AreaInfo>();
        }

        public int BigAreaID { get; set; }
        public string BigAreaName { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<int> Order { get; set; }
        public string Note { get; set; }
        public virtual ICollection<Pro_AreaInfo> Pro_AreaInfo { get; set; }
    }
}
