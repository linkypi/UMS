using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_LevelInfo
    {
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<decimal> Order { get; set; }
        public string Note { get; set; }
    }
}
