using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_SMS_SignSendPayInfo
    {
        public Nullable<decimal> RealPay { get; set; }
        public Nullable<decimal> RealCount { get; set; }
        public string ProID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string PayDate { get; set; }
        public string UserName { get; set; }
        public string Receiver { get; set; }
        public Nullable<int> SellID { get; set; }
        public int ID { get; set; }
    }
}
