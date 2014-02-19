using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Off_AduitHallInfo
    {
        public int ID { get; set; }
        public string HallID { get; set; }
        public string Note { get; set; }
        public Nullable<int> AduitTypeID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Off_AduitTypeInfo Off_AduitTypeInfo { get; set; }
    }
}
