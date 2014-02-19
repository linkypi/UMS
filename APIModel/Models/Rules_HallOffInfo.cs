using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Rules_HallOffInfo
    {
        public int ID { get; set; }
        public string HallID { get; set; }
        public Nullable<int> RulesID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual Rules_OffList Rules_OffList { get; set; }
    }
}
