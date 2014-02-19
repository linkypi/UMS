using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class SMS_SignPayInListInfo
    {
        public int ID { get; set; }
        public Nullable<int> SignListID { get; set; }
        public string InListID { get; set; }
        public string ProID { get; set; }
        public Nullable<decimal> ProCount { get; set; }
        public virtual Pro_InOrderList Pro_InOrderList { get; set; }
        public virtual Pro_ProInfo Pro_ProInfo { get; set; }
        public virtual SMS_SignSendPayInfo SMS_SignSendPayInfo { get; set; }
    }
}
