using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_ProMainInfo
    {
        public int ProMainID { get; set; }
        public Nullable<int> ClassID { get; set; }
        public string ProMainName { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public string Introduction { get; set; }
    }
}
