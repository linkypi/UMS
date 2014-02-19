using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class SMS_SignSendPayInfo
    {
        public SMS_SignSendPayInfo()
        {
            this.SMS_SignPayInListInfo = new List<SMS_SignPayInListInfo>();
        }

        public int ID { get; set; }
        public int SellID { get; set; }
        public decimal RealPay { get; set; }
        public decimal RealCount { get; set; }
        public string ProID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public Nullable<System.DateTime> PayDate { get; set; }
        public string UserID { get; set; }
        public string Receiver { get; set; }
        public virtual SMS_SignInfo SMS_SignInfo { get; set; }
        public virtual ICollection<SMS_SignPayInListInfo> SMS_SignPayInListInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
    }
}
