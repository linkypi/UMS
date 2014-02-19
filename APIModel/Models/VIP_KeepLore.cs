using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_KeepLore
    {
        public int keepLoreId { get; set; }
        public string keepLoreTitle { get; set; }
        public string keepLoreAbstract { get; set; }
        public string keepLoreInfor { get; set; }
        public string keepLorePicbig { get; set; }
        public string keepLorePicsmall { get; set; }
        public Nullable<System.DateTime> keepLoreTime { get; set; }
    }
}
