using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_SellOffAduitInfoList
    {
        public Nullable<bool> IsPass { get; set; }
        public string AduitDate { get; set; }
        public string Note { get; set; }
        public string AduitUser { get; set; }
        public int AduitID { get; set; }
    }
}
