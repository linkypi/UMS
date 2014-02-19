using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_HallInfo
    {
        public string AreaName { get; set; }
        public string HallName { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<int> AreaID { get; set; }
        public Nullable<int> LevelID { get; set; }
        public Nullable<bool> CanIn { get; set; }
        public string Note { get; set; }
        public Nullable<bool> CanBack { get; set; }
        public string HallID { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public string DisPlayName { get; set; }
        public string ShortName { get; set; }
        public Nullable<int> SellNum { get; set; }
        public string PrintName { get; set; }
        public string IsCanIn { get; set; }
        public string IsCanback { get; set; }
        public string LevelName { get; set; }
    }
}
