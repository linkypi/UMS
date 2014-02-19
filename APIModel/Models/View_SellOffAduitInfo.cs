using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_SellOffAduitInfo
    {
        public decimal NextPrice { get; set; }
        public string HallName { get; set; }
        public string HallID { get; set; }
        public string IsAduited { get; set; }
        public string IsPass { get; set; }
        public string ApplyDate { get; set; }
        public string ApplyUser { get; set; }
        public string ApplyNote { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<int> BackID { get; set; }
        public int ID { get; set; }
        public string AduitNote { get; set; }
        public string UserID { get; set; }
        public string LastAduiter { get; set; }
    }
}
