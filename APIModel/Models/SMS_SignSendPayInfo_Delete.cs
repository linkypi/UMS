using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class SMS_SignSendPayInfo_Delete
    {
        public int ID { get; set; }
        public Nullable<int> OldID { get; set; }
        public Nullable<int> SellID { get; set; }
        public Nullable<decimal> RealPay { get; set; }
        public Nullable<decimal> RealCount { get; set; }
        public string ProID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public virtual SMS_SignInfo SMS_SignInfo { get; set; }
    }
}
