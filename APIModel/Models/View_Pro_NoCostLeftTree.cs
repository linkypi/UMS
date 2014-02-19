using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_Pro_NoCostLeftTree
    {
        public string ProID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> ClassID { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public string ProName { get; set; }
        public string ProFormat { get; set; }
    }
}
