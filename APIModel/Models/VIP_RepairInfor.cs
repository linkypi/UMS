using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_RepairInfor
    {
        public int repairInforId { get; set; }
        public string servicePoint { get; set; }
        public string oddNum { get; set; }
        public string progress { get; set; }
        public string IMEI { get; set; }
        public string productName { get; set; }
        public string productColour { get; set; }
        public string productSeries { get; set; }
        public string productNo { get; set; }
        public string producRemarks { get; set; }
        public string phoneAccessory { get; set; }
        public string manualOddNum { get; set; }
        public string repairInforWay { get; set; }
        public string receivingMan { get; set; }
        public string engineer { get; set; }
        public Nullable<System.DateTime> repairInforTime { get; set; }
        public string vipImei { get; set; }
        public Nullable<int> progressid { get; set; }
    }
}
