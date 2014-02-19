using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_Warrantyservice
    {
        public int warrantyId { get; set; }
        public string customerName { get; set; }
        public string IDCard { get; set; }
        public string phone { get; set; }
        public string phonePrice { get; set; }
        public string warrantyPrice { get; set; }
        public Nullable<System.DateTime> warrantyStTime { get; set; }
        public Nullable<System.DateTime> warrantyEdTime { get; set; }
        public string phoneModel { get; set; }
        public string phoneImei { get; set; }
        public string tickeNum { get; set; }
        public string batteryNum { get; set; }
        public string chargerNum { get; set; }
        public string agreementNum { get; set; }
        public string vipImei { get; set; }
        public Nullable<long> servePhone { get; set; }
    }
}
