using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_HallInfoHeader
    {
        public int ID { get; set; }
        public string HallID { get; set; }
        public Nullable<int> HeadID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual VIP_OffListAduitHeader VIP_OffListAduitHeader { get; set; }
    }
}
