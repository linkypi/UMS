using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class VIP_VIPService_BAK
    {
        public int ID { get; set; }
        public Nullable<int> OldID { get; set; }
        public Nullable<int> OldVIPID { get; set; }
        public Nullable<int> NewVIPID { get; set; }
        public string UpdUser { get; set; }
        public Nullable<int> ProID { get; set; }
        public Nullable<decimal> SCount { get; set; }
    }
}
