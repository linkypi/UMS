using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellListServiceInfo
    {
        public Pro_SellListServiceInfo()
        {
            this.Pro_SellListInfo = new List<Pro_SellListInfo>();
        }

        public int ID { get; set; }
        public virtual ICollection<Pro_SellListInfo> Pro_SellListInfo { get; set; }
    }
}
