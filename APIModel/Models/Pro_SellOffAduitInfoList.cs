using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellOffAduitInfoList
    {
        public Pro_SellOffAduitInfoList()
        {
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
        }

        public int ID { get; set; }
        public string UserID { get; set; }
        public Nullable<bool> IsPass { get; set; }
        public System.DateTime AduitDate { get; set; }
        public string Note { get; set; }
        public int AduitID { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
        public virtual Pro_SellOffAduitInfo Pro_SellOffAduitInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
    }
}
