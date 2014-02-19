using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class SMS_SignInfo
    {
        public SMS_SignInfo()
        {
            this.SMS_SellBackAduit = new List<SMS_SellBackAduit>();
            this.SMS_SignSendPayInfo_Delete = new List<SMS_SignSendPayInfo_Delete>();
            this.SMS_SignSendPayInfo = new List<SMS_SignSendPayInfo>();
        }

        public int ID { get; set; }
        public string SellID { get; set; }
        public string OldSellID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string HallID { get; set; }
        public string Industry { get; set; }
        public Nullable<System.DateTime> SignDate { get; set; }
        public string CpcName { get; set; }
        public string CpcAdd { get; set; }
        public string SMSContent { get; set; }
        public decimal SignPay { get; set; }
        public decimal SignCount { get; set; }
        public decimal RealPay { get; set; }
        public decimal RealCount { get; set; }
        public Nullable<System.DateTime> PayAllDate { get; set; }
        public Nullable<System.DateTime> RealPayAllDate { get; set; }
        public Nullable<decimal> PayBack { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public string BillHeader { get; set; }
        public string BillNum { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public string Sellor { get; set; }
        public Nullable<decimal> RatePay { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsOver { get; set; }
        public virtual Pro_HallInfo Pro_HallInfo { get; set; }
        public virtual ICollection<SMS_SellBackAduit> SMS_SellBackAduit { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual ICollection<SMS_SignSendPayInfo_Delete> SMS_SignSendPayInfo_Delete { get; set; }
        public virtual ICollection<SMS_SignSendPayInfo> SMS_SignSendPayInfo { get; set; }
    }
}
