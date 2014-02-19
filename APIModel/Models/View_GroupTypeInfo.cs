using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_GroupTypeInfo
    {
        public int ID { get; set; }
        public string GroupName { get; set; }
        public string ClassName { get; set; }
        public string OldGroupName { get; set; }
        public string OldClassName { get; set; }
    }
}
