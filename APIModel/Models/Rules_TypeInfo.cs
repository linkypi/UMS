using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Rules_TypeInfo
    {
        public Rules_TypeInfo()
        {
            this.Rules_Pro_RulesTypeInfo = new List<Rules_Pro_RulesTypeInfo>();
        }

        public int ID { get; set; }
        public string RulesName { get; set; }
        public bool ShowToCus { get; set; }
        public bool CanGetBack { get; set; }
        public string Note { get; set; }
        public virtual ICollection<Rules_Pro_RulesTypeInfo> Rules_Pro_RulesTypeInfo { get; set; }
    }
}
