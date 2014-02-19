using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_AreaInfo
    {
        public Pro_AreaInfo()
        {
            this.Pro_HallInfo = new List<Pro_HallInfo>();
        }

        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<int> Order { get; set; }
        public string Note { get; set; }
        public string Points { get; set; }
        public string MapColor { get; set; }
        public Nullable<int> BigAreaID { get; set; }
        public virtual Pro_BigAreaInfo Pro_BigAreaInfo { get; set; }
        public virtual ICollection<Pro_HallInfo> Pro_HallInfo { get; set; }
    }
}
