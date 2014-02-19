using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ClassType
    {
        public Pro_ClassType()
        {
            this.Pro_ClassInfo = new List<Pro_ClassInfo>();
            this.Pro_ProInfo = new List<Pro_ProInfo>();
        }

        public int ID { get; set; }
        public string ClassTypeName { get; set; }
        public bool AsPrice { get; set; }
        public string Note { get; set; }
        public virtual ICollection<Pro_ClassInfo> Pro_ClassInfo { get; set; }
        public virtual ICollection<Pro_ProInfo> Pro_ProInfo { get; set; }
    }
}
