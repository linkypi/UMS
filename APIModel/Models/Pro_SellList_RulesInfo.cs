using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_SellList_RulesInfo
    {
        public int ID { get; set; }
        public Nullable<int> SellListID { get; set; }
        public Nullable<int> Rules_ProMain_ID { get; set; }
        public decimal OffPrice { get; set; }
        public decimal RealPrice { get; set; }
        public bool ShowToCus { get; set; }
        public bool CanGetBack { get; set; }
        public virtual Pro_SellListInfo Pro_SellListInfo { get; set; }
        public virtual Rules_Pro_RulesTypeInfo Rules_Pro_RulesTypeInfo { get; set; }
    }
}
