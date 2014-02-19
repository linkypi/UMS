using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_Ads
    {
        public int adsId { get; set; }
        public string adsName { get; set; }
        public string adsInfo { get; set; }
        public string adsPicbig { get; set; }
        public string adsPicsid { get; set; }
        public System.DateTime adscreatetime { get; set; }
    }
}
