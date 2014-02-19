using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Demo_ReportViewColumnInfo
    {
        public int ID { get; set; }
        public Nullable<int> ReportID { get; set; }
        public string ColDisPlayName { get; set; }
        public string ColName { get; set; }
        public Nullable<double> OrderBy { get; set; }
        public string FormatStr { get; set; }
        public virtual Demo_ReportViewInfo Demo_ReportViewInfo { get; set; }
    }
}
