using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Package_GroupInfo
    {
        public Package_GroupInfo()
        {
            this.Package_ProInfo = new List<Package_ProInfo>();
        }

        public int ID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public string GroupName { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsMust { get; set; }
        public Nullable<int> OffID { get; set; }
        public Nullable<int> SellType { get; set; }
        public Nullable<int> TempOffID { get; set; }
        public string SubNote { get; set; }
        public virtual Package_GroupInfo Package_GroupInfo1 { get; set; }
        public virtual Package_GroupInfo Package_GroupInfo2 { get; set; }
        public virtual Package_GroupTypeInfo Package_GroupTypeInfo { get; set; }
        public virtual Pro_SellType Pro_SellType { get; set; }
        public virtual VIP_OffList VIP_OffList { get; set; }
        public virtual VIP_OffListAduit VIP_OffListAduit { get; set; }
        public virtual ICollection<Package_ProInfo> Package_ProInfo { get; set; }
    }
}
