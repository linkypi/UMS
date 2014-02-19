using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Rules_Pro_RulesTypeInfo
    {
        public Rules_Pro_RulesTypeInfo()
        {
            this.Pro_SellList_RulesInfo = new List<Pro_SellList_RulesInfo>();
        }

        public int ID { get; set; }
        public Nullable<int> RulesProID { get; set; }
        public Nullable<int> RulesTypeID { get; set; }
        public decimal OffPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal OrderBy { get; set; }
        public virtual ICollection<Pro_SellList_RulesInfo> Pro_SellList_RulesInfo { get; set; }
        public virtual Rules_ProMainInfo Rules_ProMainInfo { get; set; }
        public virtual Rules_TypeInfo Rules_TypeInfo { get; set; }
    }
}
