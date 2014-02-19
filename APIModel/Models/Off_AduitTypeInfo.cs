using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Off_AduitTypeInfo
    {
        public Off_AduitTypeInfo()
        {
            this.Off_AduitHallInfo = new List<Off_AduitHallInfo>();
            this.Off_AduitProInfo = new List<Off_AduitProInfo>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public decimal Price { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool Flag { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public string DeleteUserID { get; set; }
        public System.DateTime AddDate { get; set; }
        public string AddUserID { get; set; }
        public Nullable<int> OldAduitType { get; set; }
        public virtual ICollection<Off_AduitHallInfo> Off_AduitHallInfo { get; set; }
        public virtual ICollection<Off_AduitProInfo> Off_AduitProInfo { get; set; }
    }
}
