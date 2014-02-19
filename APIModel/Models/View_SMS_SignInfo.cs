using System;
using System.Collections.Generic;

namespace APIModel.Models
{
    public partial class View_SMS_SignInfo
    {
        public string HallName { get; set; }
        public int ID { get; set; }
        public string SellID { get; set; }
        public string OldSellID { get; set; }
        public string username { get; set; }
        public Nullable<System.DateTime> SysDate { get; set; }
        public string HallID { get; set; }
        public string Industry { get; set; }
        public string SignDate { get; set; }
        public string CpcName { get; set; }
        public string CpcAdd { get; set; }
        public string SMSContent { get; set; }
        public Nullable<decimal> SignPay { get; set; }
        public Nullable<decimal> SignCount { get; set; }
        public Nullable<decimal> RealPay { get; set; }
        public Nullable<decimal> RealCount { get; set; }
        public string PayAllDate { get; set; }
        public string RealPayAllDate { get; set; }
        public Nullable<decimal> PayBack { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public string BillHeader { get; set; }
        public string BillNum { get; set; }
        public string BillDate { get; set; }
        public string Sellor { get; set; }
        public Nullable<decimal> RatePay { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsOver { get; set; }
    }
}
