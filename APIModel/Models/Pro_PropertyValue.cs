using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_PropertyValue
    {
        public Pro_PropertyValue()
        {
            this.Pro_ProProperty_F = new List<Pro_ProProperty_F>();
        }

        public int ID { get; set; }
        public Nullable<int> Pro_PropertyID { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
        public virtual Pro_Property Pro_Property { get; set; }
        public virtual ICollection<Pro_ProProperty_F> Pro_ProProperty_F { get; set; }
    }
}
