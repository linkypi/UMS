using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class Report_OldIMEITrackInfo
    {
        public string IMEI { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public string proname { get; set; }
        public string ProFormat { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
        public string InUserName { get; set; }
        public string plat { get; set; }
        public Nullable<int> ClassID { get; set; }
        public string hallid { get; set; }
    }
}
