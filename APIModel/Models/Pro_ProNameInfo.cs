using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ProNameInfo
    {
        public Pro_ProNameInfo()
        {
            this.Pro_ProMainInfo = new List<Pro_ProMainInfo>();
        }

        public int ID { get; set; }
        public string NameID { get; set; }
        public string Note { get; set; }
        public string MainName { get; set; }
        public virtual ICollection<Pro_ProMainInfo> Pro_ProMainInfo { get; set; }
    }
}
