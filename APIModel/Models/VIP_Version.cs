using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_Version
    {
        public int versionId { get; set; }
        public string versionDesc { get; set; }
        public string versionName { get; set; }
        public string versionNo { get; set; }
        public string versionSrc { get; set; }
        public Nullable<System.DateTime> versionAddtime { get; set; }
    }
}
