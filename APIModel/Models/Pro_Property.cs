using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_Property
    {
        public Pro_Property()
        {
            this.Pro_PropertyValue = new List<Pro_PropertyValue>();
        }

        public int ID { get; set; }
        public string Cate { get; set; }
        public string Note { get; set; }
        public Nullable<bool> Flag { get; set; }
        public virtual ICollection<Pro_PropertyValue> Pro_PropertyValue { get; set; }
    }
}
