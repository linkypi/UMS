using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_SearchShop
    {
        public int shopId { get; set; }
        public string shopName { get; set; }
        public string shopAdd { get; set; }
        public string shopPhone { get; set; }
        public string shopPicbig { get; set; }
        public string shopPic { get; set; }
        public Nullable<long> Shoplongitude { get; set; }
        public Nullable<long> shopLatitude { get; set; }
    }
}
