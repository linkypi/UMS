using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Rules_ProMainInfo
    {
        public Rules_ProMainInfo()
        {
            this.Rules_Pro_RulesTypeInfo = new List<Rules_Pro_RulesTypeInfo>();
        }

        public int ID { get; set; }
        public Nullable<int> RulesID { get; set; }
        public Nullable<int> ProMainID { get; set; }
        public virtual Pro_ProMainInfo Pro_ProMainInfo { get; set; }
        public virtual Rules_OffList Rules_OffList { get; set; }
        public virtual ICollection<Rules_Pro_RulesTypeInfo> Rules_Pro_RulesTypeInfo { get; set; }
    }
}
