using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Rules_AllCurrentRulesInfo
    {
        public int ID { get; set; }
        public Nullable<int> RulesID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Note { get; set; }
        public Nullable<int> RulesTypeID { get; set; }
        public string RulesName { get; set; }
        public bool ShowToCus { get; set; }
        public bool CanGetBack { get; set; }
        public Nullable<int> ProMainID { get; set; }
        public Nullable<int> SellType { get; set; }
        public decimal OffPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal OrderBy { get; set; }
        public decimal Salay { get; set; }
        public string HallID { get; set; }
        public Nullable<int> Rules_ProMain_ID { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
    }
}
