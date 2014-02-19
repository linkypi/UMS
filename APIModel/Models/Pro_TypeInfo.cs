using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_TypeInfo
    {
        public Pro_TypeInfo()
        {
            this.Pro_ProInfo = new List<Pro_ProInfo>();
        }

        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> Order { get; set; }
        public string Note { get; set; }
        public string ChildFormURL { get; set; }
        public virtual ICollection<Pro_ProInfo> Pro_ProInfo { get; set; }
    }
}
