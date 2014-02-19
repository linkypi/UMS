using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Pro_ProProperty_F
    {
        public int ID { get; set; }
        public string ProID { get; set; }
        public int ValueID { get; set; }
        public string Note { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual Pro_PropertyValue Pro_PropertyValue { get; set; }
    }
}
