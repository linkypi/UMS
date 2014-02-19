using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ProMainInfo
    {
        public Pro_ProMainInfo()
        {
            this.Off_AduitProInfo = new List<Off_AduitProInfo>();
            this.Pro_ProInfo = new List<Pro_ProInfo>();
            this.Rules_ProMainInfo = new List<Rules_ProMainInfo>();
        }

        public int ProMainID { get; set; }
        public Nullable<int> ClassID { get; set; }
        public string ProMainName { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> ProNameID { get; set; }
        public string Introduction { get; set; }
        public virtual ICollection<Off_AduitProInfo> Off_AduitProInfo { get; set; }
        public virtual ICollection<Pro_ProInfo> Pro_ProInfo { get; set; }
        public virtual Pro_ProNameInfo Pro_ProNameInfo { get; set; }
        public virtual ICollection<Rules_ProMainInfo> Rules_ProMainInfo { get; set; }
    }
}
