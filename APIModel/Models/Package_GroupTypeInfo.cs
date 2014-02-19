using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Package_GroupTypeInfo
    {
        public Package_GroupTypeInfo()
        {
            this.Package_GroupInfo = new List<Package_GroupInfo>();
        }

        public int ID { get; set; }
        public string GroupName { get; set; }
        public string ClassName { get; set; }
        public virtual ICollection<Package_GroupInfo> Package_GroupInfo { get; set; }
    }
}
