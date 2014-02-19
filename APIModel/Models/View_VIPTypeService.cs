using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_VIPTypeService
    {
        public string ProID { get; set; }
        public string ProName { get; set; }
        public string Name { get; set; }
        public Nullable<int> ID { get; set; }
        public string TypeName { get; set; }
        public string ClassName { get; set; }
        public Nullable<decimal> SCount { get; set; }
        public int ServiceID { get; set; }
        public Nullable<bool> Flag { get; set; }
    }
}
