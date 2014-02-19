using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_weekly
    {
        public int weeklyId { get; set; }
        public Nullable<int> weeklyNum { get; set; }
        public Nullable<System.DateTime> weeklyDate { get; set; }
        public string newsIdList { get; set; }
    }
}
